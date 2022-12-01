using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelSelection : MonoBehaviour {
    [SerializeField] string sceneToLoad; 

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void toStart() {
        SceneManager.LoadScene(sceneToLoad); 
    }
}
