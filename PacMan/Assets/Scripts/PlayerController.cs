using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Pacman speed
    public float speed = 0.35f;
    //RigidBody variable
    Rigidbody2D rb2d;

    //Pacman next destination
    private Vector2 dest = Vector2.zero;
    

	// Use this for initialization
	void Start () {
        //initialize the rigidbody variable
        rb2d = GetComponent<Rigidbody2D>();

        //stay still when the game starts
        dest = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //move pacman to the destination point
        Vector2 temp = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(temp);
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            dest = (Vector2)transform.position + Vector2.up;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            dest = (Vector2)transform.position + Vector2.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            dest = (Vector2)transform.position + Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            dest = (Vector2)transform.position + Vector2.right;
        }
    }
}
