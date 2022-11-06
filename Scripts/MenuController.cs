using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
    
    [SerializeField]
    private Animator startAnim;
    [SerializeField]
    private Animator endAnim;  

    public void buttonOK() {
        if (startAnim != null) {
            startAnim.SetBool("show", true); 
        }
    }

    public void newGame() {
        if (endAnim != null) {
            endAnim.SetBool("end", false); 
            startAnim.SetBool("show", false); 
        }
    }

    // public void endGame() {
    //     if (endAnim != null) {
    //         endAnim.SetBool("end", true); 
    //     }
    // }
}
