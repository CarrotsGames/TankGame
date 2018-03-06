using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeYPos : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			transform.position = new Vector3 (transform.position.x, transform.position.y + 0.2f, transform.position.z);
        }
    }
}
