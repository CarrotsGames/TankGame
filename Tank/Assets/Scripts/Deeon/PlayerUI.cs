using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    
    public int Lives;
    PlayerController pc;

    public GameObject[] Images;

    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (pc != null)
        Lives = pc.Health;
        else
        {
            Lives = 0;
        }

		if(Lives == 3)
        {
            foreach(GameObject go in Images)
            {
                go.SetActive(true); 
            }
        }
        else if(Lives == 2)
        {
            Images[0].SetActive(true);
            Images[1].SetActive(true);
            Images[2].SetActive(false);
        }
        else if(Lives == 1)
        {
            Images[0].SetActive(true);
            Images[1].SetActive(false);
            Images[2].SetActive(false);
        }
        else
        {
            Images[0].SetActive(false);
            Images[1].SetActive(false);
            Images[2].SetActive(false);
        }
	}
    
}
