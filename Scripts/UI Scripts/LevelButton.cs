using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LevelButton : MonoBehaviour {

    [Header("Active Stuff")]
    [SerializeField] bool active; 
    [SerializeField] Sprite activeSprite; 
    [SerializeField] Sprite lockedSprite; 
    [SerializeField] Image lockStatusImage; 
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
            lockStatusImage.sprite = activeSprite; 
            levelText.color = Color.white; 
            myButton.enabled = true; 
        }
        else {
            lockStatusImage.sprite = lockedSprite; 
            levelText.color = Color.gray; 
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
