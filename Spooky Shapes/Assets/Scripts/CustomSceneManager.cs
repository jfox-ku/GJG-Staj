using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public static CustomSceneManager _instance;
    public List<Scene> Scenes;


    public static CustomSceneManager Instance {
        get {
            if(_instance == null) {
                _instance = GameObject.FindObjectOfType<CustomSceneManager>();

                if (_instance == null) {
                    GameObject cont = new GameObject("SceneManager Object");
                    _instance = cont.AddComponent<CustomSceneManager>();
                }

            }

            return _instance;

        }

        
    }


    public void loadScene(string sceneToLoad) {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void loadScene(int sceneToLoad) {
        SceneManager.LoadScene(sceneToLoad);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
