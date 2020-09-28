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

    public ParticleSystem PartSys;
    public event Action<GameObject> DestEvent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
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
            
                
            

            //gameObject.SetActive(false);
            //Will use this when pooling
            //gameObject.transform.SetParent(FindObjectOfType<InactiveCollector>().transform);
            return;
        }
        
    }

    public void Disable() {
        gameObject.SetActive(false);
        if (respawning) {
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
        var partSys = Instantiate(PartSys, this.transform.position, Quaternion.identity, this.transform.parent);
        partSys.Play();

    }




}
