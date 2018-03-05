using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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

    [Tooltip("Prefabs of powerups")]
    public GameObject[] PowerUps;

    [Tooltip("Empty game objects where you want spawn points to be")]
    public GameObject[] PowerupSpawnPoints;

    private void Awake()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        PowerupSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    // Use this for initialization
    void Start()
    {
        CountDownTimer = CountDownTimerStart;
        EndGameCountdownTimer = EndGameCountdownTimerStart;
        PowerUpDropTimer = PowerUpDropTimerStart;
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

                //Powerup spawning;

                //decrease timer
                PowerUpDropTimer -= Time.deltaTime;


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
                                if (hit.collider.gameObject.tag == "Ground")
                                {
                                    canDrop = true;
                                }
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
                        if (canDrop)
                        {
                            Instantiate(PowerUps[Random.Range(0, PowerUps.Length)], randSpawn, Quaternion.identity);
                        }
                        //reset timer
                        PowerUpDropTimer = PowerUpDropTimerStart;
                    }
                }



                //check for escape input to pause
                if (Input.GetKeyDown(KeyCode.Escape))
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

                //Setting UI actives
                PausedUI.SetActive(true);
                GameUI.SetActive(false);
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

                break;
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void ToGame()
    {
        SceneManager.LoadSceneAsync(0);
    }



    private bool IsGameOver()
    {
        //float alivePlayers;
        //check if the timer has reached zero
        if (CountDownTimer <= 0)
        {
            //if it has return true
            return true;
        }


        //check each player in the scene
        //foreach(GameObject player in Players)
        //{
        //get the players controller
        //    PlayerController pc = player.GetComponent<PlayerController>();
        //if the player is alive
        //    if(pc.getAlive())
        //    {
        //add 1 to alivePlayers
        //        alivePlayers++;
        //    }
        //    
        //}
        //
        //if more than 1 person is still alive
        //if(alivePlayers > 1)
        //{
        //   //continue playing
        //}
        //if 1 or less people are alive
        //else if(alivePlayers <= 1)
        //{
        //game is over
        //    return true;
        //}

        //else return false
        return false;
    }
}
