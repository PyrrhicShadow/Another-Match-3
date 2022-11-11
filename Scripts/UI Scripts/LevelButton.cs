using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LevelButton : MonoBehaviour {

    [Header("Active Stuff")]
    [SerializeField] bool active;  
    [SerializeField] GameObject lockStatus; 
    private Button myButton; 

    [SerializeField] Image[] stars; 
    [SerializeField] Text levelText; 
    [SerializeField] int lvl; 
    [SerializeField] GameObject confirmPanel; 

    // Start is called before the first frame update
    void Start() {
        myButton = GetComponent<Button>(); 

        ActivateStars(); 
        ShowLevel();
        DecideSprite(); 
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

    private void DecideSprite() {
        if (active) {
            lockStatus.SetActive(false); 
            levelText.gameObject.SetActive(true); 
            myButton.enabled = true; 
        }
        else {
            lockStatus.SetActive(true);  
            levelText.gameObject.SetActive(false);
            myButton.enabled = false; 
        }
    }

    void ShowLevel() {
        levelText.text = lvl.ToString(); 
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ConfirmPanel() {
        confirmPanel.SetActive(true); 
        confirmPanel.GetComponent<ConfirmPanel>().setLvl(lvl); 
    }
}
