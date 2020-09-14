using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private Type PlayerType;
    Rigidbody2D rb;
    float baseDrag = 1.5f;
    public List<Collider2D> Colliders;

    [Header("Line Stuff")]
    [SerializeField] private List<Vector2> drawPos; 
    private LineRenderer lineRenderer;
    public Canvas playerLineCanvas;
    public Material lineMat;
    public float maxLineLength;
    public Gradient lineGrad;

    
    [Header("Details")]
    public Vector2 vel;
    public float angularVel;

    [Header("Drag Controls")]
    public Vector3 dragStartPos;
    public bool canDrag = true;
    public float throwMultiplier = 12f;


    [Header("Collision stuff")]
    private float WallHitCD = 0.1f;
    private float WallHitCDKeeper;

    //this is bad
    public Sprite tri;
    public Sprite rect;
    public Sprite circ;

   
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        changeType(Type.RECT);

    }


    void Update()
    {
        if(canDrag)
        touchInput();
        

    }


    public void touchInput() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);


            if (touch.phase == TouchPhase.Began) {

                dragStartPos = touch.position;
                rb.drag = 70f;
                StartCoroutine(dragLerper(baseDrag));
                DrawLine(dragStartPos,dragStartPos);
               // Debug.Log("Touch began at " + dragStartPos);
            } else if (touch.phase == TouchPhase.Moved) {

                var dir = (Vector3)clampDown(touch.position - (Vector2)dragStartPos);
                UpdateLine(transform.position,transform.position+dir / 200f);

            } else if (touch.phase == TouchPhase.Stationary) {

                var dir = (Vector3)clampDown(touch.position - (Vector2)dragStartPos);
                UpdateLine(transform.position, transform.position + dir / 200f);
                


            } else if (touch.phase == TouchPhase.Ended) {

                var dir = (Vector3)clampDown(touch.position - (Vector2)dragStartPos);
                if (Mathf.Abs(dir.x) < 10f) {
                    dir.x = 0;
                    dir.y = -1f;
                }

                StopCoroutine(dragLerper(baseDrag));
                rb.drag = baseDrag;
                rb.velocity = new Vector2(rb.velocity.x,0);
                rb.AddForce((Vector2)dir.normalized*throwMultiplier,ForceMode2D.Impulse);
                canDrag = false;
                DestroyLine();
            }


        }
    }

    #region changestuff
    public void changeType(Type tip) {
            
        if(tip != PlayerType) {
            //Debug.Log("Change type to " + tip);
            PlayerType = tip;
            changeSprite();
            changeCollider();
        }
        

    }

    public void changeSprite() {
        Sprite sprite = this.GetComponent<SpriteRenderer>().sprite;
        switch (PlayerType) {
            case Type.TRI:
                sprite = tri;
                break;
            case Type.RECT:
                sprite = rect;
                break;
            case Type.CIRC:
                sprite = circ;
                break;

        }

        if(sprite!=null)
        this.GetComponent<SpriteRenderer>().sprite = sprite;  
    }

    public void changeCollider() {

        for(int i = 0; i<Colliders.Count; i++) {
            var coll = Colliders[i].GetComponent<ColliderType>();
            if(coll.tip == PlayerType) Colliders[i].gameObject.SetActive(true);
            else Colliders[i].gameObject.SetActive(false);
        }
        
    }
    #endregion changestuff


    private void OnCollisionEnter2D(Collision2D collision) {
        var obje = collision.gameObject;
        if (obje.CompareTag("Jumpable")) {
            canDrag = true;
            var jumpable = obje.GetComponent<JumpableScript>();
            if (jumpable.allowJump == false) return;
            //maybe null check
            int jumpMultiplier = 1;
            if (jumpable.tip == PlayerType) jumpMultiplier = 2;
            changeType(jumpable.tip);
            rb.velocity = new Vector2(rb.velocity.x/2,0);
            //rb.angularVelocity = 45f;
            rb.AddForce(jumpable.getJumpForce() * jumpMultiplier);

        }else if (obje.CompareTag("Wall")) {
            if(WallHitCDKeeper == 0) {
                StartCoroutine(wallHitCooldown());
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            }
            

        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        var obje = collision.gameObject;
        if (obje.CompareTag("Killzone")) {
            CustomSceneManager.Instance.loadScene(0);
        }
    }

    private void FixedUpdate() {
        vel = rb.velocity;
        angularVel = rb.angularVelocity;
    }


    private IEnumerator wallHitCooldown() {
        while (WallHitCDKeeper > 0) {
            WallHitCDKeeper -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        WallHitCDKeeper = WallHitCD;
    }
    
    private IEnumerator dragLerper(float target) {
        rb.drag = 100f;
        while(rb.drag > target) {
            rb.drag = rb.drag * 19 / 20;
            yield return new WaitForEndOfFrame();
        }
        rb.drag = target;
        
    }


    #region linestuff
    private void DrawLine(Vector2 start, Vector2 end) {
        //Debug.Log("Drawing line " + start + " => " + end);

        if((end-start).magnitude > maxLineLength) {
            end = start + (start - end).normalized * maxLineLength;
        }

        Vector3[] arr = {start, end };
        if (lineRenderer == null) {
            GameObject lineObject = new GameObject();
            lineObject.transform.SetParent(playerLineCanvas.transform);
            this.lineRenderer = lineObject.AddComponent<LineRenderer>();
            
        }
        
        this.lineRenderer.startWidth = 0.15f;
        this.lineRenderer.endWidth = 0.05f;
        this.lineRenderer.positionCount = 2;
        lineRenderer.material = lineMat;
        this.lineRenderer.SetPositions(arr);
        this.lineRenderer.colorGradient = lineGrad;
        

    }

    private void UpdateLine(Vector2 start, Vector2 end) {
        //Debug.Log("Updating line " + start + " => " + end);
        if ((start - end).magnitude > maxLineLength) {
            end = start + (end - start).normalized * maxLineLength;
        }
        if (this.lineRenderer == null) {
            Debug.Log("Update line called no line renderer.");
            return;
        }
        Vector3[] arr = { start, end };
        this.lineRenderer.SetPositions(arr);

    }

    private void DestroyLine() {
        if (lineRenderer != null) {
            Destroy(lineRenderer.gameObject);
        }

    }


    private Vector2 clampDown(Vector2 dir) {
        if (dir.y > 0) dir.y = 0;
        return dir;
    }

    #endregion linestuff
}
