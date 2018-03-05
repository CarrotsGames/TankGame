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

    //Programmer
    GameObject player;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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

            Destroy(gameObject);
        }
    }
}
