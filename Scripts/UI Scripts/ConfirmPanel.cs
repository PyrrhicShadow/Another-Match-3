using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class ConfirmPanel : MonoBehaviour {

    [SerializeField] string sceneToLoad; 
    [SerializeField] Image[] stars; 
    [SerializeField] int lvl; 

    // Start is called before the first frame update
    void Start() {
        ActivateStars(); 
    }

    private void ActivateStars() {
        for (int i = 0; i < stars.Length; i++) {
            /* 
            if (stars[i] == earned) {
                stars[i].enabled = true; 
            } 
            */
            stars[i].enabled = false; 
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Cancel() {
        this.gameObject.SetActive(false); 
    }

    public void Play() {
        PlayerPrefs.SetInt("CurrentLevel", lvl - 1); 
        SceneManager.LoadScene(sceneToLoad); 
    }

    public void setLvl(int lvl) {
        this.lvl = lvl; 
    }
}
