using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollision : MonoBehaviour {


    GameObject player;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //Add health
            player.GetComponent<PlayerController>().Health += player.GetComponent<PlayerController>().HealthIncrease;
        
            //Destroy ourself
            Destroy(gameObject);
        }
    }
}
