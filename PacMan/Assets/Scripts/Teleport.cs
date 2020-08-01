using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    //The position to teleport to
    public Vector2 teleportTo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if is pacman, use the move method to teleport
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().move(teleportTo);

        }
        else//if is other game objects, use transform position for teleport
        {
            collision.gameObject.transform.position = teleportTo;
        }
        
       
        
    }
}
