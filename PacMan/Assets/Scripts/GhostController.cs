using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour {

    private float speed = 2.0f;
    private float changeDirectionTime; //the next time the ghost may change direction
    public Vector2 direction = Vector2.up;
    private Rigidbody2D rb2d;
    private CircleCollider2D cirColli2d;

    private bool fronze = false;
    private Vector2 respawnPos;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        cirColli2d = GetComponent<CircleCollider2D>();

        respawnPos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!fronze)
        {
            //if has wall at front, change direction
            if (!checkValidDir(direction))
            {

                if (canChangeDirection())
                {
                    changeDirection();

                }
                else if (rb2d.velocity.magnitude < speed)
                {
                    changeDirAtRandom();
                }

            }
            //change direction when no walls at front
            else if (canChangeDirection() && Time.time > changeDirectionTime)
            {
                changeDirAtRandom();
            }
            else if (rb2d.velocity.magnitude < speed)//stuck on a object that is not a wall
            {
                changeDirAtRandom();
            }

            //change ghost eye direction
            foreach (Transform t in GetComponentInChildren<Transform>())
            {
                if (t != transform)
                {
                    t.up = direction;

                }
            }


            //ghost movement
            rb2d.velocity = direction * speed;
            if (rb2d.velocity.x == 0)
            {
                transform.position = new Vector2(Mathf.Round(transform.position.x), transform.position.y);
            }
            if (rb2d.velocity.y == 0)
            {
                transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y));
            }
        }
    }

    //check the direction is able to change to or not
    private bool checkValidDir(Vector2 direction)
    {
        RaycastHit2D[] rch2ds = new RaycastHit2D[10];
        //cast collider to see if anything at front of the ghost direction
        //store an RayCastHit2D object into the array if there are something in the way
        float dist = 1;
        cirColli2d.Cast(direction, rch2ds, dist, true);
        foreach (RaycastHit2D rch2d in rch2ds)
        {
            //if has object, and the object's tag is Wall, change direction, otherwise nullpointer
            if (rch2d && rch2d.collider.gameObject.tag == "Wall")
            {
                return false;
            }
        }

        return true;
    }

    //ghost check is able to change direction
    private bool canChangeDirection()
    {
        Vector2 perpRight = Utility.PerpendicularRight(direction);
        bool rightValid = checkValidDir(perpRight);
        Vector2 perpLeft = Utility.PerpendicularLeft(direction);
        bool LeftValid = checkValidDir(perpLeft);
        return rightValid || LeftValid;
    }

    //ghost change direction randomly
    private void changeDirAtRandom()
    {
        changeDirectionTime = Time.time + 1;//cant change direction for a second
        if(Random.Range(0, 2) > 0)
        {
            changeDirection();
        }
    }

    //change ghost direction
    private void changeDirection()
    {
        changeDirectionTime = Time.time + 1;//cant change direction for a second
        Vector2 perpRight = Utility.PerpendicularRight(direction);
        bool rightValid = checkValidDir(perpRight);
        Vector2 perpLeft = Utility.PerpendicularLeft(direction);
        bool LeftValid = checkValidDir(perpLeft);
        if (rightValid || LeftValid)
        {
            int randomDir = Random.Range(0, 2);
            //change direction 90degree, random direction
            if (!LeftValid || (randomDir == 0 && rightValid))
            {
                direction = perpRight;
            }
            else
            {
                direction = perpLeft;
            }
        }
        else
        {
            direction = -direction;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.pacmanKilled();
        }
    }

    public void resetPos()
    {
        transform.position = respawnPos;
        freeze(false);
    }

    public void freeze(bool freeze)
    {
        fronze = freeze;
        rb2d.velocity = Vector2.zero;
    }
}
