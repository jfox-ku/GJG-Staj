using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpableScript : MonoBehaviour
{

    public JumpableSO asset;
    public Type tip;
    public float jumpForce = 0;
    private Rigidbody2D rb;

    public float jumpCooldown = 0.01f;
    public bool allowJump = true;

    public ParticleSystem PartSys;

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
        return ret;
    }



    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            var partSys = Instantiate(PartSys, this.transform.position ,Quaternion.identity,this.transform.parent);
            partSys.Play();

            //gameObject.SetActive(false);
            //Will use this when pooling
            //gameObject.transform.SetParent(FindObjectOfType<InactiveCollector>().transform);
            return;
        }
        StartCoroutine(waitForJump());
    }

    public void Disable() {
        gameObject.SetActive(false);
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
