using System.Collections;
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
    private bool alreadyPlayed = false;
    private AudioSource audio;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audio = GetComponent<AudioSource>();
    }
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if(player.GetComponent<PlayerController>().Health < 3)
            {
                //Add health
                player.GetComponent<PlayerController>().Health += player.GetComponent<PlayerController>().HealthIncrease;
            }

            if(!alreadyPlayed)
            {
                audio.PlayOneShot(pickUpSound, soundVolume);
                alreadyPlayed = true;
            }
        
            //Destroy ourself
            Destroy(gameObject);
        }
    }
}
