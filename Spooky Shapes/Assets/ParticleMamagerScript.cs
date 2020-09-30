using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMamagerScript : MonoBehaviour
{
    public GameObject ParticlePrefab;
    private Queue<ParticleSystem> ParticleQueue;
    public int maxSystemCount = 10;
    public List<Color> ColorsInOrder;

    // Start is called before the first frame update
    void Start()
    {
        ParticleQueue = new Queue<ParticleSystem>();
        for(int i = 0; i < maxSystemCount;i++) {
            var particle = Instantiate(ParticlePrefab,this.transform);
            var sys = particle.GetComponent<ParticleSystem>();
            sys.gameObject.SetActive(false);
            ParticleQueue.Enqueue(sys);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(Type tip,Vector2 pos) {
        ParticleSystem toPlay;

        if (ParticleQueue.Count == 0) {
            var particle = Instantiate(ParticlePrefab, pos, Quaternion.identity,this.transform);
            toPlay = particle.GetComponent<ParticleSystem>();

        } else {
            toPlay = ParticleQueue.Dequeue();
            toPlay.gameObject.SetActive(true);
        }
        

        toPlay.gameObject.transform.position = pos;
        var main = toPlay.main;
        main.startColor = getColorByType(tip);
        toPlay.Play();
        StartCoroutine(WaitAndDisable(toPlay));
        
    }

    private Color getColorByType(Type tip) {
        return ColorsInOrder[(int)tip];

    }

    public IEnumerator WaitAndDisable(ParticleSystem sys) {
        
        while (true) {
            yield return new WaitForSeconds(2f);
            if (sys.isStopped) {
                sys.gameObject.SetActive(false);
                ParticleQueue.Enqueue(sys);
                break;
            }
        }

    }

}
