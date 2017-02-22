using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour {
    private GameController gameController;

    // Use this for initialization
    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();

        // Set the bullet to destroy itself 
        var bulletLife = Mathf.Min(gameController.wave*0.1f, 10);
        Destroy(gameObject, bulletLife);
      
        //Find the player's ship
        GameObject playerShip = GameObject.FindGameObjectWithTag("Ship");

        //GetComponent<Rigidbody2D>().AddForce(transform.up * 600);
        GetComponent<Rigidbody2D>().velocity = (playerShip.transform.position - transform.position).normalized*4.5f;

    }



}
