using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    //Pacman speed
    private float speed = 0.10f;
    //player score
    public int score = 0;

    //RigidBody variable
    Rigidbody2D rb2d;
    //Animator
    Animator animator;
    
    //UI Text element variable use to display the score
    public Text scoreText;

    //Pacman next destination
    private Vector2 dest = Vector2.zero;
    public GameObject pil;

    private bool pacman_alive = false;

	// Use this for initialization
	void Start () {
        //initialize the rigidbody variable
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //stay still when the game starts
        dest = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        if (!pacman_alive)
        {
            //Gets the next point position that towards to the destination
            Vector2 temp = Vector2.MoveTowards(transform.position, dest, speed);
            //setting the object position by using rigidbody
            GetComponent<Rigidbody2D>().MovePosition(temp);
            //When pacman reaches to the destination point
            if ((Vector2)transform.position == dest)
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
            Vector2 dir = dest - (Vector2)transform.position;
            GetComponent<Animator>().SetBool("moving", (dir.x != 0 || dir.y != 0));

        }
    }

    private bool Valid(Vector2 dir)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>() || hit.collider.gameObject.tag == "pill");
    }

    public void addScore(int points)
    {
        score += points;
        scoreText.text = "" + score;
    }

    public void move(Vector2 d)
    {
        transform.position = d;
        dest = d;
    }

    public void setState(bool aliveState)
    {
        pacman_alive = aliveState;
        animator.SetBool("died", pacman_alive);
    }
}
