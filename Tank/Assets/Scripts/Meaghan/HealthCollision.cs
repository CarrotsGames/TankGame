﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollision : MonoBehaviour {

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
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        player3 = GameObject.FindGameObjectWithTag("Player3");
        player4 = GameObject.FindGameObjectWithTag("Player4");
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (playAudio)
        {
            audio.PlayOneShot(pickUpSound, soundVolume);

            Destroy(gameObject, 1.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if(!playAudio)
            {
                if (player.GetComponent<PlayerController>().Health < 3)
                {
                    //Add health
                    player.GetComponent<PlayerController>().Health += player.GetComponent<PlayerController>().HealthIncrease;
                }

                playAudio = true;
            }
            
        }
        else if (other.transform.tag == "Player2")
        {
            if (!playAudio)
            {
                if (player2.GetComponent<PlayerController>().Health < 3)
                {
                    //Add health
                    player2.GetComponent<PlayerController>().Health += player.GetComponent<PlayerController>().HealthIncrease;
                }

                playAudio = true;
            }

        }
        else if (other.transform.tag == "Player3")
        {
            if (!playAudio)
            {
                if (player3.GetComponent<PlayerController>().Health < 3)
                {
                    //Add health
                    player3.GetComponent<PlayerController>().Health += player.GetComponent<PlayerController>().HealthIncrease;
                }

                playAudio = true;
            }

        }
        else if (other.transform.tag == "Player4")
        {
            if (!playAudio)
            {
                if (player4.GetComponent<PlayerController>().Health < 3)
                {
                    //Add health
                    player4.GetComponent<PlayerController>().Health += player.GetComponent<PlayerController>().HealthIncrease;
                }

                playAudio = true;
            }

        }
    }
}
