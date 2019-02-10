using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {
    PlayerController pc;
    private Text interactionText, lifeText;
    static public bool haventLost = true;
    static public int hits; //keeps track of the hits made against the (lives) of the player
   // private GameController gc;

	// Use this for initialization
	void Start () {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        //gc = GameObject.Find("Game Manager").GetComponent<GameController>();
        hits = 0;
        interactionText = GameObject.Find("Interaction Text").GetComponent<Text>();
        lifeText = GameObject.Find("Lives Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        //If it got stuck in a tree and didnt fall down, delete it. Also, delete it if it fell through
        if ((int)Time.time == 5 && (transform.position.y > 22 || transform.position.y < -10)) {
            Destroy(this.gameObject);
        }
	}

    void OnCollisionEnter(Collision collision) {
        //If its the objects first time hitting the ground and it is rotated past 90 degrees along the x or z axis, try to resituate it so it isnt't upside down
        string name = collision.gameObject.name;
        if (name.Equals("Terrain") && transform.up.y < 0f) {
            transform.rotation = new Quaternion(0,0,0,0); //Reset rotation
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; //Take away any rotational velocity
            transform.position = transform.position + new Vector3(0,.5f,0);
         }
    }

    //If the player walks over the cookie, let the player controller know this is an interactable object
    void OnTriggerStay(Collider other) {
        if (other.name.Equals("Player")) {
            if (enabled) {
                pc.interactable = this.gameObject;
                interactionText.text = "Press F to eat cookie";
            } 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player")) {
            if (!enabled) {
                hits++;
                lifeText.text = "Lives left: " + Mathf.Max(0,(5-hits));
                if (hits == 5) {
                    //The Player has lost 
                     GameObject.Find("Game Manager").GetComponent<GameController>().gameOver();
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Player") && enabled) {
            interactionText.text = "";
            pc.interactable = null;
        }
    }
}
