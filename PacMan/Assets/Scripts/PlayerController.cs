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
        //Gets the next point position that towards to the destination
        Vector2 temp = Vector2.MoveTowards(transform.position, dest, speed);
        //setting the object position by using rigidbody
        GetComponent<Rigidbody2D>().MovePosition(temp);
        //When pacman reaches to the destination point
        if((Vector2)transform.position == dest)
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && Valid(Vector2.up))
            {
                dest = (Vector2)transform.position + Vector2.up;
                transform.up = Vector2.up;
            }
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && Valid(Vector2.down))
            {
                dest = (Vector2)transform.position + Vector2.down;
                transform.up = Vector2.down;
            }
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Valid(Vector2.left))
            {
                dest = (Vector2)transform.position + Vector2.left;
                transform.up = Vector2.left;
            }
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Valid(Vector2.right))
            {
                dest = (Vector2)transform.position + Vector2.right;
                transform.up = Vector2.right;
            }
        }
        
    }

    private bool Valid(Vector2 dir)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>());
    }


}
