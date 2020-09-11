using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpableScript : MonoBehaviour
{

    public JumpableSO asset;
    public Type tip;
    public float jumpForce = 0;
    private Rigidbody2D rb;

    public float jumpCooldown = 0.2f;
    public bool allowJump = true;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = asset.drag;
        tip = asset.geoType;
        jumpForce = asset.basePushForce;
        //rb.AddTorque(Random.Range(0f,4f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 getJumpForce() {
        var ret = Vector2.up * jumpForce;
        if (!allowJump) return Vector2.zero;
        return ret;
    }



    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {

            gameObject.SetActive(false);
            return;
        }
        StartCoroutine(waitForJump());
    }

    private IEnumerator waitForJump() {

        yield return new WaitForEndOfFrame();
        var cooldown = jumpCooldown;
        allowJump = false;
        while(cooldown > 0) {
            cooldown -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        allowJump = true;
    }



}
