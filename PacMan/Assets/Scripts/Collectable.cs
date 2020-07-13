using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    //The point/score that give to the player when the pil has been collected
    public int points = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "pacman")
        {
            collision.gameObject.GetComponent<PlayerController>().score += points;
            Destroy(this.gameObject);
        }
        
    }
}
