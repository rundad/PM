using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    //The point/score that give to the player when the pil has been collected
    public int points = 100;
    //is the pill a super pill
    private bool isSuperPill = false;
	// Use this for initialization
	void Start () {
		if(this.gameObject.name == "superPill")//if current object's name is super pill
        {
            isSuperPill = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "pacman")
        {
            if (isSuperPill)//if pacman collide with a super pill
            {
                GameManager.getInstance().eatSuperPill();
                
            }
            collision.gameObject.GetComponent<PlayerController>().addScore(points);//add score
            this.gameObject.SetActive(false);//current object set to inactive
        }
        
    }
}
