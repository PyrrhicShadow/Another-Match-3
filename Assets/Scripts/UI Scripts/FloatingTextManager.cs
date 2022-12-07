using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FloatingTextManager : MonoBehaviour
{
    [SerializeField] GameObject textContainer;
    [SerializeField] GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private void Update() {
        foreach(FloatingText txt in floatingTexts) {
            txt.UpdateFloatingText(); 
        }
    }

    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        FloatingText floatingText = GetFloatingText();

        floatingText.getTxt().text = msg;
        floatingText.getTxt().fontSize = fontSize;
        floatingText.getTxt().color = color;
        floatingText.getGo().transform.position = Camera.main.WorldToScreenPoint(position); // Transfer world space to screen space so we can use it for the UI
        floatingText.setMotion(motion);
        floatingText.setDuration(duration);

        floatingText.Show(); 
    }

    private FloatingText GetFloatingText() {
        FloatingText txt = floatingTexts.Find(t => !t.isActive()); 

        if (txt == null) {
            txt = new FloatingText();
            txt.setGo(Instantiate(textPrefab));
            txt.getGo().transform.SetParent(textContainer.transform);
            txt.setTxt(txt.getGo().GetComponent<Text>());

            floatingTexts.Add(txt); 
        }

        return txt; 
    }
}
