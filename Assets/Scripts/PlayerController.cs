using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private float speed = 5.0f;
    private float rotationSpeed = 2.5f;

    public GameObject explosion;

    public GameObject bullet;
    private GameController gameController;
    public AudioClip crash;
    public AudioClip shoot;

    //Controls for touch hold
    private float holdTime = 0.2f; //or whatever
    private float acumTime = 0;

    private bool invincible = false;

    // Use this for initialization
    void Start()
    {

        // Get a reference to the game controller object and the script
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");

        gameController = gameControllerObject.GetComponent<GameController>();

    }

    // Update is called once per frame
    void Update()
    {
       // getInputUpdate();
        checkTouchInput();

        if (Input.GetMouseButtonDown(0))
        {
            ShootBullet();
            /*
            //For some reason this didn't work.
            var mouse = Input.mousePosition;
            var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
            var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            */
        }
    }
    //Check and respond to touchscreen input
    void checkTouchInput()
    {
        if (Input.touchCount > 0)
        {
            acumTime += Input.GetTouch(0).deltaTime;
            rotateToTouch();
            if (acumTime >= holdTime)
            {
                //Long tap
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.AddForce(transform.up * speed);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended && Input.touchCount==0)
            {
                acumTime = 0;
            }
        }
    }

    //Rotate player ship so that it points toward mouse/screen press.
    void rotateToTouch()
    {
        var mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = transform.position.z;
        var mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        transform.LookAt(mouseWorldSpace, Camera.main.transform.forward);
        transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
    }

    //Used for keyboard input
    void getInputUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (Input.GetAxis("Horizontal") < -.01) //left rotate
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetAxis("Horizontal") > .01) //right rotate
        {
            transform.Rotate(Vector3.back * rotationSpeed);
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) > .01)
        {
            rb.AddForce(transform.up * speed);
        }
    }


    void OnTriggerEnter2D(Collider2D c)
    {
        if (invincible == false)
        {
            // Anything except own bullet is dangerous.
            if (c.gameObject.tag != "Bullet")
            {
                if (gameController.sfxOn)
                {
                    AudioSource.PlayClipAtPoint(crash, Camera.main.transform.position);
                }
                //play explosion animation  
                var rotation = Quaternion.Euler(0, 0, Random.Range(-0.0f, 359.0f));
                Destroy(Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), rotation), .3f);


                // Move the ship to the center of the screen
                transform.position = new Vector3(0, 0, 0);

                // Remove all velocity from the ship
                GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

                //If ship hit a mine or enemy bullet, then destroy mine/bullet
                if (c.tag.Equals("Enemy Bullet"))
                {
                    Destroy(c);
                }
                invincible = true;
                StartCoroutine(invincibleTimer(1.5f));
                gameController.DecrementLives();
            }
        }
    }

    IEnumerator invincibleTimer(float time)
    {
        yield return new WaitForSeconds(time);
        invincible = false;
    }

    void ShootBullet()
    {
        Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, 0),transform.rotation);

        // Play a shoot sound
        if (gameController.sfxOn)
        {
            AudioSource.PlayClipAtPoint(shoot, Camera.main.transform.position);
        }
    }

    public void resetPosition()
    {
        transform.position = new Vector3(0, 0, 0);
    }

}
