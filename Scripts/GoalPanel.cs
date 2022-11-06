using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GoalPanel : MonoBehaviour {
    
    [SerializeField]
    private Image myImage; 
    [SerializeField]
    private Sprite mySprite; 
    [SerializeField]
    private Text myText; 
    [SerializeField]
    private string goalText; 

    // Start is called before the first frame update
    void Start() { 
        SetUp(); 
    }

    private void SetUp() {
        myImage.sprite = mySprite; 
        myText.text = goalText; 
    }

    public void setSprite(Sprite newSprite) {
        mySprite = newSprite; 
    }

    public void setText(string newText) {
        goalText = newText; 
        myText.text = goalText; 
    }
}
