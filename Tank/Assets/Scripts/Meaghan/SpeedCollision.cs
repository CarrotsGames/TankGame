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
    private bool alreadyPlayed = false;
    private AudioSource audio;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //Add speed
            player.GetComponent<PlayerController>().HasSpeedBoost = true;

            if (!alreadyPlayed)
            {
                audio.PlayOneShot(pickUpSound, soundVolume);
                alreadyPlayed = true;
            }

            //Destroy ourself
            Destroy(gameObject);
        }
    }
}
