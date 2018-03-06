using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    //Variables
    //Designer
    [Header("Bullet Values")]
    [SerializeField]
    private float destroyTime = 5.0f;
    [SerializeField]
    private int healthReduction = 1;

    [Header("Audio Stuff")]
    [SerializeField]
    private AudioClip bounceOffWall;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float bounceVolume;

    //Programmer
    private GameObject player;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;
    private bool alreadyPlayed;
    private bool canPlay;
    private AudioSource audio;
    private bool playAudio = false;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        player3 = GameObject.FindGameObjectWithTag("Player3");
        player4 = GameObject.FindGameObjectWithTag("Player4");
        audio = GetComponent<AudioSource>();
        canPlay = true;
        alreadyPlayed = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Destroy the object after a certian amount of time
        Destroy(gameObject, destroyTime);
	}

    private void FixedUpdate()
    {
        if (playAudio)
        {
            audio.PlayOneShot(bounceOffWall, bounceVolume);
            playAudio = false;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            //Reduce health by amount
            player.GetComponent<PlayerController>().Health -= healthReduction;

            //Reset the timer
            player.GetComponent<PlayerController>().BulletCoolDown = 0.0f;

            //Destroy 
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "Player2")
        {
            //Reduce health by amount
            player2.GetComponent<PlayerController>().Health -= healthReduction;

            //Reset the timer
            player2.GetComponent<PlayerController>().BulletCoolDown = 0.0f;

            //Destroy 
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "Player3")
        {
            //Reduce health by amount
            player3.GetComponent<PlayerController>().Health -= healthReduction;

            //Reset the timer
            player3.GetComponent<PlayerController>().BulletCoolDown = 0.0f;

            //Destroy 
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "Player4")
        {
            //Reduce health by amount
            player4.GetComponent<PlayerController>().Health -= healthReduction;

            //Reset the timer
            player4.GetComponent<PlayerController>().BulletCoolDown = 0.0f;

            //Destroy 
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Wall")
        {
            playAudio = true;
        }
    }
}
