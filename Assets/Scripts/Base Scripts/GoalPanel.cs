using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GoalPanel : MonoBehaviour {
    
    [SerializeField] Image myImage; 
    [SerializeField] Sprite mySprite; 
    [SerializeField] Color myColor; 
    [SerializeField] Text myText; 
    [SerializeField] string goalText; 

    // Start is called before the first frame update
    void Start() { 
        SetUp(); 
    }

    private void SetUp() {
        myImage.sprite = mySprite; 
        myText.text = goalText; 
        myImage.color = myColor; 
    }

    public void setSprite(Sprite newSprite) {
        mySprite = newSprite; 
    }

    public void setSprite(Sprite newSprite, Color newColor) {
        mySprite = newSprite; 
        myColor = newColor; 
    }

    public void setText(string newText) {
        goalText = newText; 
        myText.text = goalText; 
    }
}
