using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "World", menuName = "Level")]
public class Level : ScriptableObject {
    // width, heigh, dots, board layout, score goals, balance
    [Header("Board Dimensions")]
    public int width; 
    public int height; 

    [Header("Starting conditions")]
    public Tile[] boardLayout; 
    public GameObject[] dots; 
    public GameObject[] eyes; 
    public int eyeRatio; 
    // public Animator endAnim; 
    public Color backgroundColor; 

    [Header("Score")]
    public int balance; 
    public int scoreGoal; 
}
