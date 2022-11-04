using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class HintManager : MonoBehaviour, IPointerClickHandler {

    private Board board; 
    private FindMatches findMatches; 
    [SerializeField]
    private float hintDelay; 
    private float hintDelaySec; 
    [SerializeField]
    private GameObject hintParticle; 
    [SerializeField]
    private GameObject currentHint; 
    
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>(); 
        findMatches = FindObjectOfType<FindMatches>(); 
        hintDelaySec = hintDelay; 
    }

    // Update is called once per frame
    void Update()
    {
        hintDelaySec -= Time.deltaTime; 
        if (hintDelaySec <= 0 && currentHint != null) {
            DestroyHint(); 
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            MarkHint(); 
        }
    }

    /// <summary>Create a hint from a chosen match</summary>
    private void MarkHint() {
        hintDelaySec = hintDelay; 
        GameObject move = findMatches.RandomMatch(); 
        if (move != null) {
            currentHint = Instantiate(hintParticle, move.transform.position, Quaternion.identity); 
        }
    }

    /// <summary>Destroys the hint</summary> 
    public void DestroyHint() {
        if (currentHint != null) {
            Destroy(currentHint); 
            currentHint = null; 
            hintDelaySec = hintDelay; 
        }
    }
}
