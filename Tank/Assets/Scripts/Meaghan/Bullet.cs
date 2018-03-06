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
    private bool alreadyPlayed;
    private bool canPlay;
    private AudioSource audio;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        
        if(collision.transform.tag == "Wall")
        {
           audio.PlayOneShot(bounceOffWall, bounceVolume);
        }
    }
}
