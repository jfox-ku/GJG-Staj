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
    public DragLineDrawer dragDrawer;


    [Header("Drag Controls")]
    public Vector3 dragStartPos;
    public bool canDrag = true;
    public bool isDragging = false;
    public float throwMultiplier = 12f;
    public float dragSlowMultiplier = 19f / 20f;
    private bool dragCancel = false;


    [Header("Collision stuff")]
    private float WallHitCD = 0.1f;
    private float WallHitCDKeeper;


    public List<ItemScript> inventory;
    DefaultInputManager ImInstance;

    public ParticleSystem PartSys;


    public Sprite[] Sprites;

    [Header("Jump stuff")]
    public float jumpMult = 1;
    public float[] itemMultipliers;


    void Start()
    {

        DefaultInputManager.instance.InputUpEvent += InputUp;
        DefaultInputManager.instance.InputDownEvent += InputDown;
        DefaultInputManager.instance.InputEvent += InputDrag;

        if (inventory == null) {
            inventory = new List<ItemScript>();

        }
        rb = this.GetComponent<Rigidbody2D>();
        changeType(Type.RECT);

    }

    private void OnDestroy() {
        DefaultInputManager.instance.InputUpEvent -= InputUp;
        DefaultInputManager.instance.InputDownEvent -= InputDown;
        DefaultInputManager.instance.InputEvent -= InputDrag;
    }

    private void InputDown(Vector2 inp) {
        //Debug.Log("Input down event!");
        if (canDrag) {
            dragCancel = false;
            isDragging = true;
            dragStartPos = inp;
            rb.drag = 70f;
            StartCoroutine(dragLerper(baseDrag));
            dragDrawer.DrawLine(dragStartPos, dragStartPos);
        }
        
    }

    private void InputUp(Vector2 inp) {
        //Debug.Log("Input up event!");
        if (!dragCancel && canDrag) {
            var dir = (Vector3)clampDown(inp - (Vector2)dragStartPos);

            //This is for just tapping the screen.
            if (Mathf.Abs(dir.x) < 20f) {
                dir.x = 0;
                dir.y = -1f;
            }

            StopCoroutine(dragLerper(baseDrag));
            rb.drag = baseDrag;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce((Vector2)dir.normalized * throwMultiplier, ForceMode2D.Impulse);
            canDrag = false;
            isDragging = false;
        }


        dragDrawer.DestroyLine();
    }

    

    private void InputDrag(Vector2 inp) {
        //Debug.Log("Input drag event!");
        if (!dragCancel && canDrag) {
            var dir = (Vector3)clampDown(inp - (Vector2)dragStartPos);
            dragDrawer.UpdateLine(transform.position, transform.position + dir / 250f);
        }
    }

    void Update()
    {

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
        sprite = Sprites[(int)PlayerType];

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

    public float speedGainOnJump = 1.2f;
    private void OnCollisionEnter2D(Collision2D collision) {
        var obje = collision.gameObject;
        if (obje.CompareTag("Jumpable")) {

            jump(obje);
            
            
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
            if ((int)transform.position.y > PlayerPrefs.GetInt("Score")) PlayerPrefs.SetInt("Score", (int)transform.position.y);
            CustomSceneManager.Instance.loadScene(0);
        }else if (obje.CompareTag("LoadNextZone")) {
            //Debug.Log("Next Zone Triggered");
            var rm = FindObjectOfType<RunManagerScript>();
            rm.LoadZoneTrigger(obje);
        }
        if (obje.CompareTag("Item")) {
            var giver = obje.GetComponent<RewardFrameScript>();
            giver.giveItem();

        }

    }

    private void OnTriggerStay2D(Collider2D collision) {
        var obje = collision.gameObject;
        if (obje.CompareTag("Upzone")) {
            //Can this get component call be avoided? Is it bad?
            obje.GetComponent<UpzoneScript>().MoveUpConfiner();
        }

    }

    

    private void jump(GameObject obje) {
        canDrag = true;
        var jumpable = obje.GetComponent<JumpableScript>();
        if (jumpable.allowJump == false) return;
        //maybe null check
        int jumpMultiplier = 1;
        if (jumpable.tip == PlayerType) jumpMultiplier = 2;

        changeType(jumpable.tip);
        rb.velocity = new Vector2(rb.velocity.x * speedGainOnJump, 0);
        //rb.angularVelocity = 45f;
        if (isDragging) {
            dragCancel = true;
            dragDrawer.DestroyLine();
        }
        rb.drag = baseDrag;
        rb.AddForce(jumpable.getJumpForce() * (jumpMultiplier+itemMultipliers[(int)PlayerType]));

        //Shake is weird right now. Need better numbers? (seems to overshoot and bug out)
        //CinemachineShakeScript.Instance.ShakeCamera(0.5f* jumpMultiplier, 0.2f);

        jumpable.Disable();
    }


    private IEnumerator wallHitCooldown() {
        while (WallHitCDKeeper > 0) {
            WallHitCDKeeper -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        WallHitCDKeeper = WallHitCD;
    }


    public float itemDragWait = 0f;
    //Slow down player at the start of dragging. Increase rigidbody linear drag.
    private IEnumerator dragLerper(float target) {
        rb.drag = 100f;
        yield return new WaitForSeconds(itemDragWait);
        while(rb.drag > target) {
            rb.drag = rb.drag * dragSlowMultiplier;
            yield return new WaitForEndOfFrame();
        }
        rb.drag = target;
        
    }


    private Vector2 clampDown(Vector2 dir) {
        if (dir.y > 0) dir.y = 0;
        return dir;
    }

    #region Items
    public void addToInventory(ItemScript item) {
        
        if(inventory== null) {
            inventory = new List<ItemScript>();

        }
        foreach(ItemScript i in inventory) {
            if(i.itemName == item.itemName) {
                
                i.AddItem();
                i.OnPickUp();
                //Debug.Log("Player has item " + item.itemName);
                return;
            }

        }
        //Debug.Log("Adding " + item.itemName + " to inventory first time.");
        inventory.Add(item);
        item.OnPickUp();

    }


    #endregion 

}
