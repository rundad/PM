using System.Collections;
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
    public GameObject ghost1;
    public GameObject ghost2;
    public GameObject ghost3;
    public GameObject ghost4;
    public AnimationClip pacman_died;
    private float respawnTime;

    private static GameManager instance;
    public List<GameObject> ghosts;

    public GameObject gameOver;
    public GameObject gameWon;

    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject startCountDowns;
    public Text time;

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
	}
	
	// Update is called once per frame
	void Update () {
        switch (gameState)
        {
            case GameState.PACMAN_DYING:
                if(Time.time > respawnTime)
                {
                    gameState = GameState.PACMAN_DEAD;
                    respawnTime = Time.time + 1;
                    pacman.SetActive(false);
                }
                break;
            case GameState.PACMAN_DEAD:
                if(Time.time > respawnTime)
                {
                    gameState = GameState.PLAY;
                    pacman.GetComponent<PlayerController>().setLives(pacman.GetComponent<PlayerController>().lives - 1);
                    if (pacman.GetComponent<PlayerController>().lives >= 0)
                    {
                        pacman.SetActive(true);
                    }
                    else
                    {
                        gameState = GameState.GAME_OVER;
                    }
                    pacman.GetComponent<PlayerController>().move(Vector2.zero);
                    pacman.GetComponent<PlayerController>().setState(false);
                    foreach (GameObject ghost in ghosts)
                    {
                        ghost.GetComponent<GhostController>().resetPos();
                    }
                }
                break;
            case GameState.GAME_OVER:
                gamePanel.SetActive(false);

                gameOver.SetActive(true);
                gameWon.SetActive(false);
                setGameState(false);
                if (Input.anyKeyDown)
                {
                    resetGame();
                }
                break;
        }
        if (gamePanel.activeInHierarchy)
        {
            time.text = "Time:\n\n" + Time.time;
        }
	}

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
        SceneManager.LoadScene(0);
    }

    private void setGameState(bool state)
    {
        pacman.GetComponent<PlayerController>().enabled = state;
        ghost1.GetComponent<GhostController>().enabled = state;
        ghost2.GetComponent<GhostController>().enabled = state;
        ghost3.GetComponent<GhostController>().enabled = state;
        ghost4.GetComponent<GhostController>().enabled = state;
    }

    public void onStartButton()
    {
        StartCoroutine(startCountDown());
        startPanel.SetActive(false);
    }

    public void onExitButton()
    {
        Application.Quit();
    }

    IEnumerator startCountDown()
    {
        GameObject gameobject = Instantiate(startCountDowns);
        yield return new WaitForSeconds(4f);
        Destroy(gameobject);
        setGameState(true);
        gamePanel.SetActive(true);
    }
}
