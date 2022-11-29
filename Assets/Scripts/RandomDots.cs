using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class RandomDots : MonoBehaviour {
    [SerializeField] Sprite[] dots; 
    private Image myImage; 
    private float timer; 

    // Start is called before the first frame update
    void Start() {
        myImage = this.gameObject.GetComponent<Image>(); 
    }

    // Update is called once per frame
    void Update() {
        timer -= Time.deltaTime; 
        if (timer <= 0) {
            ChangeSprite(); 
            timer = 3; 
        }
    }

    private void ChangeSprite() {
        int rand = Random.Range(0, dots.Length); 
        myImage.sprite = dots[rand]; 
        Debug.Log("Sprite Changed"); 
    }

}
