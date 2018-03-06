using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;


public class PlayerController : MonoBehaviour {

    //Variables
    //Desginer
    [Header("Choose Controller Number")]
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
    [SerializeField]
    private float maxShootCoolDown = 5.0f;
    [SerializeField]
    private Transform canonTransform;

    [Header("Alter Health")]
    [SerializeField]
    private int health = 3;
    [SerializeField]
    private int healthIncrease = 1;

    [Header("Audio Stuff")]
    [SerializeField]
    private AudioClip death;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float deathVolume;

    [Header("Particles")]
    [SerializeField]
    private ParticleSystem deathParticle;
    [SerializeField]
    private ParticleSystem damageParticle;

    [Header("Insert Crown Here")]
    [SerializeField]
    private GameObject crown;

    [Header("Animation")]
    [SerializeField]
    private bool isMoving = false;
    [SerializeField]
    private bool isShooting = false;
    [SerializeField]
    private bool idle = true;

    //Programmer
    private float keyPress = 0.0f;
    private GameObject bulletPrefab;
    private float bulletCoolDown = 5.1f;
    private float storedSpeed;
    private bool hasSpeedBoost = false;
    private float speedTimer = 0.0f;
    private AudioSource audio;
    private bool alreadyPlayed = false;
    private bool playDamageParticle;
    private bool playDeathParticle;
    

    //Getters and setters
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public int HealthIncrease
    {
        get { return healthIncrease; }
    }

    public float BulletCoolDown
    {
        get { return bulletCoolDown; }
        set { bulletCoolDown = value; }
    }

    public bool HasSpeedBoost
    {
        get { return hasSpeedBoost; }
        set { hasSpeedBoost = value; }
    }

    public bool PlayDamageParticle
    {
        get { return playDamageParticle; }
        set { playDamageParticle = value; }
    }

    public bool PlayDeathParticle
    {
        get { return playDeathParticle; }
        set { playDeathParticle = value; }
    }

    // Use this for initialization
    void Start ()
    {
        bulletPrefab = Resources.Load("projectile") as GameObject;
        damageParticle.transform.position = transform.position;
        audio = GetComponent<AudioSource>();
        storedSpeed = speed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(health == 0)
        {
            if(!alreadyPlayed)
            {
                //Play the death sound
                audio.PlayOneShot(death, deathVolume);
            
                alreadyPlayed = true;
            }

            if (playDeathParticle)
            {
                deathParticle.Play();
                playDeathParticle = false;

                //Terminate existance 
                Destroy(gameObject, 0.65f);
            }
            
        }
        else
        {
            //Play the particle
            if(playDamageParticle)
            {
                damageParticle.Play();
                playDamageParticle = false;
            }


            StickMovement();
            Rotation();
            Crown();

            //Timers
            bulletCoolDown += Time.deltaTime;

            if (hasSpeedBoost)
            {
                speedTimer += Time.deltaTime;

                if (speedTimer < maxSpeedBoostTime)
                {
                    speed = speedBoost;
                }
                else
                {
                    speed = storedSpeed;
                    hasSpeedBoost = false;
                }
            }
            else
            {
                speedTimer = 0.0f;
            }
            

            //If we can fire
            if (bulletCoolDown > maxShootCoolDown)
            {
                //If we press the fire button
                if (XCI.GetButton(XboxButton.A, controller))
                {
                    isShooting = true;
                    idle = false;
                    //Fire
                    Shoot();
                }
                else
                {
                    isShooting = false;
                    idle = true;
                }
            }
        }
       
    }

    private void StickMovement()
    {
        //Obtain the values
        float moveHorizontal = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float moveVertical = XCI.GetAxis(XboxAxis.LeftStickY, controller);

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);


       if(moveHorizontal > 0 || moveVertical > 0)
       {
            isMoving = true;
            idle = false;
       }
       else
       {
           isMoving = false;
           idle = true;
       }

        //Update the position
        transform.position += movement * speed * Time.deltaTime;
    }

    private void DPadMovement()
    {
        //Movement based on individual keys (not using physics)

        //Create a temp pos based on your current pos
        Vector3 pos = transform.position;

       //Alter the temp pos
       if (XCI.GetButton(XboxButton.DPadUp, controller))
       {                                                  
           pos.z += speed * Time.deltaTime;

       } 
                                                          
       if (XCI.GetButton(XboxButton.DPadDown, controller))
       {                                                 
           pos.z -= speed * Time.deltaTime;

        }                                                 
                                                         
       if (XCI.GetButton(XboxButton.DPadRight, controller))
       {                                                  
           pos.x += speed * Time.deltaTime;

        }                                                  
                                                          
       if (XCI.GetButton(XboxButton.DPadLeft, controller))
       {
           pos.x -= speed * Time.deltaTime;
       }


        //Make the real pos equal to the temnp pos
        transform.position = pos;



    }

    private void Rotation()
    {
        //Check key inputs for rotation
        if(XCI.GetButton(XboxButton.LeftBumper, controller))
        {
            keyPress -= rotation * Time.deltaTime;
        }
        else if (XCI.GetButton(XboxButton.RightBumper, controller))
        {
            keyPress += rotation * Time.deltaTime;
        }

        //Create the target (only around the horizontal axis)
        float tiltAroundY = keyPress * tiltAngle;
        Quaternion target = Quaternion.Euler(0, tiltAroundY, 0);

        //Rotate the object
        canonTransform.rotation = Quaternion.Slerp(canonTransform.rotation, target, Time.deltaTime * smooth);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

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

    private void Crown()
    {
        if(health < 3)
        {
            //Deactivate crown
            crown.SetActive(false);
        }
    }
}
