using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    [SerializeField]
    private int hp = 1; 
    [SerializeField]
    private bool breakable; 
    private SpriteRenderer sprite; 
    [SerializeField]
    private Sprite[] sprites; 

    void Start() {
        breakable = false; 
        sprite = GetComponent<SpriteRenderer>(); 
    }

    void Update() {
        if (hp <= 0) {
            Destroy(this.gameObject); 
        } 
    }

    /// <summary>Changes the breakable bool to the value of status </summary>
    public void setBreakable(bool status) {
        breakable = status; 
    }

    /// <summary>Returns true if this tile is breakable, false otherwise </summary>
    public bool isBreakable() {
        return breakable;
    }

    /// <summary>If this tile is breakable, subtract <paramref name="dmg"/> from this tile's current health</summary>
    /// <param name="dmg">amount of damage for this tile to take if breakable</param>
    public void TakeDmg(int dmg) {
        if (breakable) {
            hp -= dmg; 
            this.TileDmg(); 
        }
    }

    /// <summary>Takes current sprite alpha and cut it in half</summary>
    private void TileDmg() {
        Color color = sprite.color; 
        float newAlpha = color.a * 0.5f; 
        sprite.color = new Color(color.r, color.g, color.b, newAlpha); 

        // change it from making color lighter to switching sprites 
        // sprite.sprite = sprites[i - 1]; 
    }

    /// <summary>Returns this tile's current hp</summary>
    public int getHp() {
        return hp; 
    }
}
