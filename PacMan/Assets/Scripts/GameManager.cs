﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public enum GameState
    {
        PLAY, PACMAN_DYING, PACMAN_DEAD, GAME_OVER, GAME_WON
    };

    public GameState gameState = GameState.PLAY;

    public GameObject pacman;
    public AnimationClip pacman_died;
    private float respawnTime;

    private static GameManager instance;
    public List<GameObject> ghosts;
    public List<GameObject> pills;

    public GameObject gameOver;
    public GameObject gameWon;

    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject startCountDowns;
    public Text time;
    public GameObject resetText;

    private Transform playerTransform;
    public bool isSuperPacman;
	// Use this for initialization
	void Start () {
		if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //gameOver.SetActive(false);
        //gameWon.SetActive(false);
        setGameState(false);
        playerTransform = pacman.transform;
        if (!playerTransform)
        {
            print("pacman does not exist..");
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        
        switch (gameState)
        {
            case GameState.PLAY:
                bool pillActive = false;
                foreach(GameObject pill in pills)//check any pills are active(not collected yet)
                {
                    if (pill.activeSelf)
                    {
                        pillActive = true;
                        break;
                    }
                }
                if (!pillActive)//all pills got collected by pacman
                {
                    gameState = GameState.GAME_WON;//change game state to win
                    
                }
                break;
            case GameState.PACMAN_DYING://when pacman collided with ghosts
                if(Time.time > respawnTime)//check the time is after playing the pacman dying animation
                {
                    gameState = GameState.PACMAN_DEAD;//change game state to dead
                    respawnTime = Time.time + 1;//another second to the respawn time to delay the dead state behaviors one second late
                    pacman.SetActive(false);//set pacman inactive(disappear/died)
                }
                break;
            case GameState.PACMAN_DEAD:
                if(Time.time > respawnTime)//after a second from dying state
                {
                    gameState = GameState.PLAY;//change game state to play state
                    pacman.GetComponent<PlayerController>().setLives();//update player's lives
                    if (pacman.GetComponent<PlayerController>().getLives() > 0)//if player still have lives
                    {
                        pacman.SetActive(true);//keep the pacman active
                    }
                    else
                    {
                        gameState = GameState.GAME_OVER;//losses all lives, change game state to game over
                    }
                    pacman.GetComponent<PlayerController>().move(Vector2.zero);//moves pacman to its respawn position
                    pacman.GetComponent<PlayerController>().setState(false);//change pacman's animation back to idle
                    foreach (GameObject ghost in ghosts)//reset all ghosts position
                    {
                        ghost.GetComponent<GhostController>().resetPos();
                    }
                }
                break;
            case GameState.GAME_OVER://game over state: player losses all lives
                gamePanel.SetActive(false);//disable game panel

                gameOver.SetActive(true);//enable game over UI Image
                gameWon.SetActive(false);//disable game win UI Image
                resetText.SetActive(true);//enable game reset text UI
                setGameState(false);//disable all moving game objects movement
                if (Input.GetKeyDown(KeyCode.Return))//player can reset the game when the game is over
                {
                    resetGame();
                }
                break;
            case GameState.GAME_WON://player collected all pills
                gamePanel.SetActive(false);//disable game panel
                gameWon.SetActive(true);//enable game win UI Image
                resetText.SetActive(true);//enable game reset text UI
                setGameState(false);//disable all game object movement
                if (Input.GetKeyDown(KeyCode.Return))//player can restart the game by enter any key down
                {
                    resetGame();
                }
                break;
        }
        if (gamePanel.activeInHierarchy)//start the game time record when the game panel is enabled
        {
            time.text = "Time:\n\n" + Time.time;
        }
	}

    //When pacman touched with ghosts, change game state to dying
    //Change pacman's animation by setting pacman's state
    //Set respawn time as the current time in the scene + the dying animation length
    //freeze the ghosts
    public static void pacmanKilled()
    {
        instance.pacman.GetComponent<PlayerController>().setState(true);
        instance.gameState = GameState.PACMAN_DYING;
        instance.respawnTime = Time.time + instance.pacman_died.length;
        foreach(GameObject ghost in instance.ghosts)
        {
            ghost.GetComponent<GhostController>().freeze(true);
        }

    }

    //reset the game by reloading the game scene
    public void resetGame()
    {
        //pacman.GetComponent<PlayerController>().move(Vector2.zero);
        //pacman.GetComponent<PlayerController>().setState(false);
        //pacman.GetComponent<PlayerController>().setLives(2);
        //pacman.SetActive(true);
        //foreach (GameObject ghost in ghosts)
        //{
        //ghost.GetComponent<GhostController>().resetPos();
        // }
        foreach (GameObject ghost in ghosts)
        {
            ghost.GetComponent<GhostController>().freeze(true);
        }
        SceneManager.LoadScene(0);
    }

    //Disable pacman and ghosts movement by disable their controller
    private void setGameState(bool state)
    {
        pacman.GetComponent<PlayerController>().enabled = state;
        foreach(GameObject ghost in ghosts)
        {
            ghost.GetComponent<GhostController>().enabled = state;
        }
    }

    //START button function on the GUI
    //start the game start count down
    //set the start panel inactive
    public void onStartButton()
    {
        StartCoroutine(startCountDown());
        startPanel.SetActive(false);
    }

    //EXIT button function, close the application
    public void onExitButton()
    {
        Application.Quit();
    }

    //Plays the start counts down
    //wait 4 second for the count down to finish
    //destory the count down prefab clone after the count down
    //enable the movement of all game objects
    //enable the game panel
    IEnumerator startCountDown()
    {
        GameObject gameobject = Instantiate(startCountDowns);
        yield return new WaitForSeconds(4f);
        Destroy(gameobject);
        setGameState(true);
        gamePanel.SetActive(true);
    }

    //Return gameManager instance
    public static GameManager getInstance()
    {
        return instance;
    }

    //Freeze ghosts and set the color of 'a' in rgba for the ghosts to look a bit transparent
    private void freezeGhosts()
    {
        foreach(GameObject ghost in ghosts)
        {
            Color color = ghost.GetComponent<SpriteRenderer>().color;
            color.a = color.a - 0.3f;
            ghost.GetComponent<GhostController>().freeze(true);
            ghost.GetComponent<SpriteRenderer>().color = color;
        }
    }

    //Un-freeze the ghosts and reset the color back to normal
    private void unFreezeGhosts()
    {
        foreach (GameObject ghost in ghosts)
        {
            Color color = ghost.GetComponent<SpriteRenderer>().color;
            color.a = 1.0f;
            ghost.GetComponent<GhostController>().freeze(false);
            ghost.GetComponent<SpriteRenderer>().color = color;
        }
    }

    //Coroutine to wait for 3 seconds, which is the time for pacman can being a super pacman
    //un-freeze the ghosts after 3 seconds
    //set isSuperPacman state to false, indicates that pacman is not a superpacman anymore
    IEnumerator ghostRecovery()
    {
        yield return new WaitForSeconds(3f);
        unFreezeGhosts();
        isSuperPacman = false;
    }

    //Called when pacman collects a super pill
    //freeze the ghosts
    //start a coroutine to reset the ghosts 
    public void eatSuperPill()
    {
        isSuperPacman = true;
        freezeGhosts();
        StartCoroutine(ghostRecovery());
    }
}
