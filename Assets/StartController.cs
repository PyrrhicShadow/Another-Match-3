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
        GameData data = FindObjectOfType<GameData>(); 
        Vector3 flipX = new Vector3(-1, 1, 1); 
        if (data.saveData.hasSummoned()) {
            game.rectTransform.localScale = flipX; 
            start.rectTransform.localScale = flipX; 
            background.color = GameColors.bgColors[5]; 
        }
        else {
            game.rectTransform.localScale = Vector3.one; 
            start.rectTransform.localScale = Vector3.one; 
            background.color = GameColors.bgColors[0]; 
        }
    }
}
