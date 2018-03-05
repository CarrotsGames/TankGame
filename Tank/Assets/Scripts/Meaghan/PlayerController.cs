using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;


public class PlayerController : MonoBehaviour {

    //Variables
    //Desginer
    [Header("Controller")]
    [SerializeField]
    private XboxController controller;
   
    [Header("Alter Movement")]
    [SerializeField]
    private float speed = 3.0f;
    [SerializeField]
    private float speedBoost = 6.0f;
    [SerializeField]
    private float maxSpeedBoostTime = 5.0f;

    [Header("Alter Rotation")]
    [SerializeField]
    private float rotation = 1.0f;
    [SerializeField]
    private float smooth = 5.0f;
    [SerializeField]
    private float tiltAngle = 30.0f;

    [Header("Alter Bullet")]
    [SerializeField]
    private float bulletSpeed = 20.0f;
    private float maxShootCoolDown = 5.0f;

    [Header("Alter Health")]
    [SerializeField]
    private int health = 3;
    [SerializeField]
    private int healthIncrease = 1;

    //Programmer
    private Transform canonTransform;
    private float keyPress = 0.0f;
    private GameObject bulletPrefab;
    private float bulletCoolDown = 5.1f;
    private float storedSpeed;
    private bool hasSpeedBoost = false;
    private float speedTimer = 0.0f;
    
    //Getters and setters
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public float BulletCoolDown
    {
        get { return bulletCoolDown; }
        set { bulletCoolDown = value; }
    }

    // Use this for initialization
    void Start ()
    {
        canonTransform = transform.GetChild(0);
        bulletPrefab = Resources.Load("projectile") as GameObject;
        storedSpeed = speed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Movement();
        Rotation();

        //Timers
        bulletCoolDown += Time.deltaTime;

        if(hasSpeedBoost)
           speedTimer += Time.deltaTime;

        if(speedTimer < maxSpeedBoostTime)
        {
            speed = speedBoost;
        }
        else
        {
            hasSpeedBoost = false;
            speedTimer = 0.0f;
            speed = storedSpeed;
        }

        //If we can fire
        if (bulletCoolDown > maxShootCoolDown)
        {
            //If we press the fire button
            if (Input.GetMouseButtonDown(0))
            {
                //Fire
                Shoot();
            }
        }



        if(health == 0)
        {
            //Terminate existance 
            Destroy(gameObject);
        }
       
    }

    private void Movement()
    {
        //Movement based on individual keys (not using physics)

        //Create a temp pos based on your current pos
        Vector3 pos = transform.position;

        //Alter the temp pos
        if (XCI.GetAxis(XboxAxis.LeftStickY, controller) > 0 || XCI.GetButton(XboxButton.DPadUp, controller))
        {
            pos.z += speed * Time.deltaTime;
        }

        if (XCI.GetAxis(XboxAxis.LeftStickY, controller) < 0 || XCI.GetButton(XboxButton.DPadDown, controller))
        {
            pos.z -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            pos.x += speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= speed * Time.deltaTime;
        }


        //Make the real pos equal to the temnp pos
        transform.position = pos;
    }

    private void Rotation()
    {
        //Check key inputs for rotation
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            keyPress -= rotation * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            keyPress += rotation * Time.deltaTime;
        }



        //Create the target (only around the horizontal axis)
        float tiltAroundY = keyPress * tiltAngle;
        Quaternion target = Quaternion.Euler(0, tiltAroundY, 0);

        //Rotate the object
        canonTransform.rotation = Quaternion.Slerp(canonTransform.rotation, target, Time.deltaTime * smooth);
    }

    private void Shoot()
    {
        //Create the projectile
        GameObject projectile = Instantiate(bulletPrefab) as GameObject;
        
        //Set the position and the velocity of the bullet
        projectile.transform.position = canonTransform.position + canonTransform.forward;
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = canonTransform.forward * bulletSpeed;

        //Reset the timer
        bulletCoolDown = 0.0f;
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Speed")
        {
            hasSpeedBoost = true;

            //Destroy the other gameObject
            Destroy(other.gameObject);
        }

        if(other.tag == "Health")
        {
            health += healthIncrease;

            //Destroy the other gameObject
            Destroy(other.gameObject);
        }
    }
}
