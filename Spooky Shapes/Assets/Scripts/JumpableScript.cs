using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JumpableScript : MonoBehaviour
{

    public JumpableSO asset;
    public Type tip;
    public float jumpForce = 0;
    private Rigidbody2D rb;

    public float jumpCooldown = 0.01f;
    public bool allowJump = true;
    public bool respawning = false;
    public bool isSaver = false;

    public ParticleSystem PartSys;
    public event Action<GameObject> DestEvent;

    private GameObject playerRef;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        isSaver = this.gameObject.GetComponent<SaverFollowScript>() != null;
        tip = asset.geoType;
        jumpForce = asset.basePushForce;
        //rb.AddTorque(Random.Range(0f,4f));
        if (respawning) rb.drag = 1000f;
        else {
            rb.drag = asset.drag;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 getJumpForce() {
        var ret = Vector2.up * jumpForce;
        return ret;
    }



    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            var player = collision.gameObject.GetComponent<PlayerScript>();

            Disable();
            

            //gameObject.SetActive(false);
            //Will use this when pooling
            //gameObject.transform.SetParent(FindObjectOfType<InactiveCollector>().transform);
            return;
        }
        
    }


    public void Disable() {
        this.gameObject.transform.SetParent(JumpablePoolManager.instance.transform);
        //this.gameObject.SetActive(false);
        
        if (isSaver) {
            Destroy(this.gameObject);
            return;
        } else {

            if (respawning) {
                myRespawner.currentPiece = null;
                myRespawner.Respawn(tip);
            }

            //Debug.Log("Invoking dest event! My type is "+tip);
            DestEvent?.Invoke(this.gameObject);
        }
        
        

    }


   

    public void Enable() {
        gameObject.SetActive(true);
        
        if (respawning) rb.drag = 1000f;
        else {
            rb.drag = asset.drag;
        }
    }

    public void Explode() {
        if (PartSys == null) return;
        var partSys = Instantiate(PartSys, this.transform.position, Quaternion.identity, this.transform.parent);
        partSys.Play();

    }

    public RespawnJumpableScript myRespawner;
    public void SetAsRespawning(RespawnJumpableScript pos) {
        rb.drag = 1000f;
        respawning = true;
        myRespawner = pos;

    }





}
