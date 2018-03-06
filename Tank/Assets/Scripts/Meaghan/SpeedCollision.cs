using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCollision : MonoBehaviour {

    [Header("Audio Stuff")]
    [SerializeField]
    private AudioClip pickUpSound;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float soundVolume;

    private GameObject player;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;
    private bool playAudio = false;
    private AudioSource audio;

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        player3 = GameObject.FindGameObjectWithTag("Player3");
        player4 = GameObject.FindGameObjectWithTag("Player4"); 
    }

    private void Update()
    {
        if (playAudio)
        {
            audio.PlayOneShot(pickUpSound, soundVolume);

            Destroy(gameObject, 0.45f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if(!playAudio)
            {
                //Add speed
                player.GetComponent<PlayerController>().HasSpeedBoost = true;

                playAudio = true;
            }
        }
        else if (other.transform.tag == "Player2")
        {
            if (!playAudio)
            {
                //Add speed
                player2.GetComponent<PlayerController>().HasSpeedBoost = true;

                playAudio = true;
            }
        }
        else if (other.transform.tag == "Player3")
        {
            if (!playAudio)
            {
                //Add speed
                player3.GetComponent<PlayerController>().HasSpeedBoost = true;

                playAudio = true;
            }
        }
        else if (other.transform.tag == "Player4")
        {
            if (!playAudio)
            {
                //Add speed
                player4.GetComponent<PlayerController>().HasSpeedBoost = true;

                playAudio = true;
            }
        }
    }
}
