using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>The last level of the game triggers a different endAnim than the normal levels</summary>
public class EndMenuController : MenuController{

    /*****      Deprecated     *****/
    new protected void Start() {
        base.Start(); 
    }

    new public void winGame() {
        if (endAnim != null) { 
            endAnim.SetBool("end", true); 
        }
    }
}
