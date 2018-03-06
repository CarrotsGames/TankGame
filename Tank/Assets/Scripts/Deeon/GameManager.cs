using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;
using UnityEngine.EventSystems;


public enum GameStates
{
    Playing,
    Paused,
    GameOver,
}

public class GameManager : MonoBehaviour
{

    [Header("Timers")]
    public float CountDownTimerStart;
    public Text CountDownTimerText;
    float CountDownTimer;

    //GameOver restart countdown
    public float EndGameCountdownTimerStart;
    float EndGameCountdownTimer;
    public Text EndGameCountdownText;

    public GameStates currentState;

    [Header("UI")]
    public GameObject GameUI;
    public GameObject PausedUI;
    public GameObject GameOverUI;

    [Header("Player Management")]
    public GameObject[] Players;

    [Header("Powerups")]
    public float PowerUpDropTimerStart;
    float PowerUpDropTimer;

    [Header("Prefabs of powerups")]
    public GameObject[] PowerUps;

    [Header("Empty game objects where you want spawn points to be")]
    public GameObject[] PowerupSpawnPoints;


    private GameObject winner;

    [Header("End Game Zoom offsets")]
    public float endGameZOffset;
    public float endGameYOffset;
    public float endGameXOffset;

    private XboxController controller;

    public EventSystem eSystem;

    [Header("Buttons")]
    public GameObject PauseButton;
    public GameObject GameOverButton;

    private void Awake()
    {
        PowerupSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    // Use this for initialization
    void Start()
    {
        CountDownTimer = CountDownTimerStart;
        EndGameCountdownTimer = EndGameCountdownTimerStart;
        PowerUpDropTimer = PowerUpDropTimerStart;
        winner = null;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameStates.Playing:

                //Make sure timescale is 1
                Time.timeScale = 1;

                //Setting UI actives
                GameUI.SetActive(true);
                PausedUI.SetActive(false);
                GameOverUI.SetActive(false);
                CountDownTimerText.gameObject.SetActive(true);

                //Timer manip
                CountDownTimer -= Time.deltaTime;
                uint timerSeconds = (uint)CountDownTimer;

                //set time to MM:SS instead of just seconds
                string minutes = Mathf.Floor(CountDownTimer / 60).ToString("00");
                string seconds = Mathf.Floor(CountDownTimer % 60).ToString("00");

                CountDownTimerText.text = minutes + " : " + seconds;

                //Powerup spawning

                //decrease timer
                PowerUpDropTimer -= Time.deltaTime;


                //make sure theres at least 1 powerup spawnpoint
                if (PowerupSpawnPoints.Length > 1)
                {

                    //if timer reaches zero or beyond
                    if (PowerUpDropTimer <= 0)
                    {
                        Vector3 randSpawn = new Vector3(0, 0, 0);
                        bool canDrop = false;
                        //to make better do a raycheck downwards and see if the ground is the only thing visible, else choose another random position

                        while (!canDrop)
                        {
                            randSpawn = PowerupSpawnPoints[Random.Range(0, PowerupSpawnPoints.Length)].transform.position;
                            RaycastHit hit;
                            if (Physics.Raycast(randSpawn, -transform.up, out hit))
                            {
                                //if the raycast hits the floor
                                if (hit.collider.gameObject.tag == "Ground")
                                {
                                    //a powerup can be spawned
                                    canDrop = true;
                                }
                                //else it cant
                                else if (hit.collider.gameObject.tag == "Untagged")
                                {
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                        //if a powerup can be dropped
                        if (canDrop)
                        {
                            //spawn a powerup
                            Instantiate(PowerUps[Random.Range(0, PowerUps.Length)], randSpawn, Quaternion.identity);
                        }
                        //reset timer
                        PowerUpDropTimer = PowerUpDropTimerStart;
                    }
                }



                //check for escape input to pause
                if (XCI.GetButtonDown(XboxButton.Start, controller))
                {
                    currentState = GameStates.Paused;
                }



                //check game over
                if (IsGameOver())
                {
                    currentState = GameStates.GameOver;
                }

                break;

            case GameStates.Paused:
                //set timescale to 0
                Time.timeScale = 0;

               // eSystem.SetSelectedGameObject(PauseButton);

                //Setting UI actives
                PausedUI.SetActive(true);
                GameUI.SetActive(true);
                GameOverUI.SetActive(false);

                //check for escape input to pause
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentState = GameStates.Playing;
                }

                break;

            case GameStates.GameOver:

                //Setting UI actives
                GameOverUI.SetActive(true);
                GameUI.SetActive(true);
                PausedUI.SetActive(false);
                CountDownTimerText.gameObject.SetActive(false);

                //Timer Management
                EndGameCountdownTimer -= Time.deltaTime;
                uint endGameCountdown = (uint)EndGameCountdownTimer;

                EndGameCountdownText.text = "Restarting in: " + endGameCountdown.ToString();
                if (EndGameCountdownTimer <= 0)
                {
                    //EndGameCountdownTimer = EndGameCountdownTimerStart;
                    //currentState = GameStates.Playing;

                    //Dirty restart
                    SceneManager.LoadSceneAsync(1);
                }

                if(winner != null)
                {
                    Debug.Log(winner.name);
                    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(winner.transform.position.x + endGameXOffset, winner.transform.position.y + endGameYOffset, winner.transform.position.z + endGameZOffset), 0.01f);
                    //Camera.main.transform.LookAt(winner.transform);
                    //Camera.main.orthographic = false;
                }

                break;
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void ToGame()
    {
        SceneManager.LoadSceneAsync(1);
    }



    private bool IsGameOver()
    {
        float alivePlayers = 0;
        winner = null;
        //check if the timer has reached zero
        if (CountDownTimer <= 0)
        {
            //if it has return true
            winner = null;
            return true;
        }


        //check each player in the scene
        foreach(GameObject player in Players)
        {

            //get the players controller
			if (player != null)
			{
				PlayerController pc = player.GetComponent<PlayerController>();
				//if the player is alive
				if(pc.Health > 0)
				{
					//add 1 to alivePlayers
					alivePlayers++;
					//set winner
					winner = pc.gameObject;
				}
			}
            
        }
        
        //if more than 1 person is still alive
        if(alivePlayers > 1)
        {
           //continue playing
        }
        //if 1 or less people are alive
        else if(alivePlayers <= 1)
        {
            //game is over
            foreach (GameObject player in Players)
            {
                if(player.GetComponent<PlayerController>().Health >= 1)
                {
                    winner = player;
                }
            }
                return true;
        }

        //else return false
        return false;
    }

    public void Resume()
    {
        currentState = GameStates.Playing;
    }

}
