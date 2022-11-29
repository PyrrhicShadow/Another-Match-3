using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class StartController : MonoBehaviour {

    [SerializeField] Text game; 
    [SerializeField] Text start; 
    [SerializeField] Image background; 


    // Start is called before the first frame update
    void Start()
    {
        // if gamedata.hasSummoned, make game and start x-scale = -1 and background color different
    }
}
