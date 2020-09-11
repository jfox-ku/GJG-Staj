using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private Type PlayerType;
    Rigidbody2D rb;
    public List<Collider2D> Colliders;

    
    [Header("Details")]
    public Vector2 vel;
    public float angularVel;

    [Header("Drag Controls")]
    public Vector3 dragStartPos;
    public bool canDrag = true;


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

                dragStartPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                rb.velocity = new Vector2(rb.velocity.x/3,0);
                rb.drag = 100f;
               // Debug.Log("Touch began at " + dragStartPos);
            } else if (touch.phase == TouchPhase.Moved) {
                var dir = Camera.main.ScreenToWorldPoint(touch.position) - dragStartPos;
                //Debug.DrawLine(transform.position, Camera.main.ScreenToWorldPoint(touch.position));
                Debug.DrawRay(transform.position,dir);

            } else if (touch.phase == TouchPhase.Stationary) {
                //Debug.DrawLine(transform.position, Camera.main.ScreenToWorldPoint(touch.position));

            } else if (touch.phase == TouchPhase.Ended) {
                
                var dir = Camera.main.ScreenToWorldPoint(touch.position) - dragStartPos;
                Debug.DrawRay(transform.position, dir);
                Debug.Log((Vector2)dir);
                if (dir.y > 0) dir.y = 0;
                if (dir.y < 0 && dir.y > -1f) dir.y = -1f;
                dir = Vector2.ClampMagnitude(dir, 1f);

                rb.drag = 1.5f;
                rb.velocity = new Vector2(rb.velocity.x,0);
                rb.AddForce((Vector2)dir*10f,ForceMode2D.Impulse);
                canDrag = false;
            }


        }
    }

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
            rb.angularVelocity = 45f;
            rb.AddForce(jumpable.getJumpForce() * jumpMultiplier);
            

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

}
