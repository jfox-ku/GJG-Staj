using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private Type PlayerType;
    Rigidbody2D rb;
    float baseDrag = 1.5f;
    public List<Collider2D> Colliders;

    [Header("Line Stuff")]
    public DragLineDrawer dragDrawer;
    public event Action<List<ItemScript>> InventoryEvent;


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
    private bool waitingForStart;

    public ParticleMamagerScript ParticleManager;
    public UIScoreScript UIScore;

    [Header("Jump stuff")]
    public float jumpMult = 1;
    public float[] itemMultipliers;

    private void Awake() {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (DefaultInputManager.instance != null) {
            DefaultInputManager.instance.InputUpEvent += InputUp;
            DefaultInputManager.instance.InputDownEvent += InputDown;
            DefaultInputManager.instance.InputEvent += InputDrag;
        }

        if (inventory == null) {
            inventory = new List<ItemScript>();

        }
        
        changeType(Type.RECT);

    }

    private void OnDisable() {
        if(DefaultInputManager.instance != null) {
            DefaultInputManager.instance.InputUpEvent -= InputUp;
            DefaultInputManager.instance.InputDownEvent -= InputDown;
            DefaultInputManager.instance.InputEvent -= InputDrag;
        }

        
    }

    #region Input
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
        if (waitingForStart) {
            waitingForStart = false;
            GameObject.FindGameObjectWithTag("TitleText").SetActive(false);
            UIScore.gameStarted = true;
        }

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
        if (inventory == null) {
            inventory = new List<ItemScript>();

        }
        if (!dragCancel && canDrag) {
            var dir = (Vector3)clampDown(inp - (Vector2)dragStartPos);
            dragDrawer.UpdateLine(transform.position, transform.position + dir / 250f);
        }
    }
    #endregion

    public void WaitForStart() {
        GameObject.FindGameObjectWithTag("TitleText").SetActive(true);
        
        waitingForStart = true;
        rb.drag = 1000f;
        rb.angularVelocity = UnityEngine.Random.Range(-90f,90f);

    }

    void FixedUpdate()
    {
        if(rb.angularVelocity > 60f) {
            //Debug.LogWarning("Angular Velocity Over 60 Deg");
            rb.angularVelocity = 60f;
        }

        if(rb.velocity.magnitude > 25f) {
            //Debug.LogWarning("Max velocity reached");
            rb.velocity = rb.velocity.normalized * 25f;
        }

    }


    #region changestuff
    public void changeType(Type tip) {
            
        if(tip != PlayerType) {
            //Debug.Log("Change type to " + tip);
            PlayerType = tip;
            changeCollider();
        }
        

    }

    

    public void changeCollider() {

        for(int i = 0; i<Colliders.Count; i++) {
            var coll = Colliders[i].GetComponent<ColliderType>();
            if(coll.tip == PlayerType) Colliders[i].gameObject.SetActive(true);
            else Colliders[i].gameObject.SetActive(false);
        }
        
    }
    #endregion changestuff

    #region CollisionStuff
    public float speedGainOnJump = 0.5f;
    public float wallJumpMultiplier = 1f;
    private void OnCollisionEnter2D(Collision2D collision) {
        var obje = collision.gameObject;
        if (obje.CompareTag("Jumpable")) {

            jump(obje);
            
            
        }else if (obje.CompareTag("Wall")) {

            if(WallHitCDKeeper == 0) {
                StartCoroutine(wallHitCooldown());
                rb.velocity = new Vector2(-rb.velocity.x * wallJumpMultiplier, rb.velocity.y);
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

    private UpzoneScript RefKeeper;
    private void OnTriggerStay2D(Collider2D collision) {
        var obje = collision.gameObject;
        if (obje.CompareTag("Upzone")) {
            
            if(RefKeeper == null) {
                RefKeeper = obje.GetComponent<UpzoneScript>();
            }
            //Is it bad to pass the rigid body so often? Should a refence be kept in Upzone script?
            RefKeeper.moveFollowDelta(this.transform,rb.velocity.y);
        }

    }
    #endregion


    private void jump(GameObject obje) {
        canDrag = true;
        var jumpable = obje.GetComponent<JumpableScript>();
        if (jumpable.allowJump == false) return;
        //maybe null check
        int jumpMultiplier = 1;
        if (jumpable.tip == PlayerType) {
            jumpMultiplier = 2;
            jumpable.Explode();
            ParticleManager.Play(PlayerType,obje.transform.position);
        }


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
        
        

        bool found = false;
        foreach (ItemScript i in inventory) {
            if(i.itemName == item.itemName) {
                
                i.AddItem();
                i.OnPickUp();
                Debug.Log("Player has item " + item.itemName +" Count: "+i.count);
                InventoryEvent?.Invoke(inventory);
                found = true;
            }

        }
        //Debug.Log("Adding " + item.itemName + " to inventory first time.");
        if (!found) {
            inventory.Add(item);
            item.AddItem();
            item.OnPickUp();
            InventoryEvent?.Invoke(inventory);
        }
        

    }


    #endregion 

}
