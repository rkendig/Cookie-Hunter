using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    
    public Transform player; //this is actually the tranform of the Main Camera not the player
    GameController gc; 
    public GameObject interactable, BrownCookie, GreenCookie, WhiteCookie, VioletCookie ;
    GameObject[] cookies;
    public Text interactionText;
    //private Vector3 hitNormal, movement; //orientation of the slope.
   // public float slideFriction = 0.3f; // ajusting the friction of the slope
    //private bool isGrounded;

    // For creating the water effects 
    float underwaterDens = 0.15f;
    Color underwaterColor = new Color(0.1f, 0.3f, 0.4f, 1.0f);
    private bool oldFog, underwater;
    private float oldDens; 
    private Color oldColor;
    private FogMode oldMode;
    private Transform curWater = null;
    CharacterController cc;



    private void Start() {
        gc = GameObject.Find("Game Manager").GetComponent<GameController>();
        cc = GetComponent<CharacterController>();
        interactable = null;
        cookies = new GameObject[4] {BrownCookie, GreenCookie, WhiteCookie, VioletCookie};
    }

    void Update() {
        
        // Deal with any underwater effects

        //If it is underwater
        if (curWater != null && player.position.y < curWater.position.y){
            if (!underwater){ // turn on underwater effect only once
                oldFog = RenderSettings.fog;
                oldMode = RenderSettings.fogMode;
                oldDens = RenderSettings.fogDensity;
                oldColor = RenderSettings.fogColor;
                RenderSettings.fog = true;
                RenderSettings.fogMode = FogMode.Exponential;
                RenderSettings.fogDensity = underwaterDens;
                RenderSettings.fogColor = underwaterColor;
                underwater = true;
            }
        } 
        //Else if it is not underwater
        else if (underwater){ // turn off underwater effect, if any
            RenderSettings.fog = oldFog;
            RenderSettings.fogMode = oldMode;
            RenderSettings.fogDensity = oldDens;
            RenderSettings.fogColor = oldColor;
            underwater = false;
        }

        if (interactable != null && Input.GetKeyDown("f")) {
           if (interactable.tag.Equals("Brown")) {
                gc.updateScore(5);
                Destroy(interactable.gameObject);
                interactionText.text = "";
                interactable = null;
           }
           else if (interactable.tag.Equals("White")) {
                gc.updateScore(35);
                Destroy(interactable.gameObject);
                interactionText.text = "";
                interactable = null;
            }
           else if (interactable.tag.Equals("Green")) {
                gc.updateScore(50);
                Destroy(interactable.gameObject);
                interactionText.text = "";
                interactable = null;
           }
           else if (interactable.tag.Equals("Violet")) {
                gc.updateScore(20);
                Destroy(interactable.gameObject);
                interactionText.text = "";
                interactable = null;
           }
           else if (interactable.tag.Equals("IntCol")) {
                interactionText.text = "";
                Destroy(interactable.gameObject);
                //Instantiate a random cookie
                Transform camera = GameObject.Find("Main Camera").GetComponent<Transform>();
                Instantiate(cookies[Random.Range(0, 3)], camera.position + (camera.forward *3), Quaternion.identity);
                //cookie.GetComponent<Rigidbody>().AddForce(camera.forward);
                interactable = null;
           }
           else if (interactable.tag.Equals("Hidden")) {
                interactionText.text = "You have spawned the GOLDEN COOKIE. Look up.";
                Destroy(interactable.gameObject);
                //Instantiate the Golden Cookie 
                Instantiate(Resources.Load("GoldenCookie"), new Vector3(360,200,340), Quaternion.identity);
                Debug.Log("Golden Cookie was created");
                interactable = null;
           }
           else if (interactable.tag.Equals("Golden")) {
                gc.updateScore(1000);
                Destroy(interactable.gameObject);
                interactionText.text = "";
                interactable = null;
           }
        }
    }
    

    void OnTriggerEnter(Collider other) {
     if (other.tag=="Water"){ // if entering a waterplane
         if (transform.position.y > other.transform.position.y){
             // set reference to the current waterplane
             curWater = other.transform; 
         }
     }
 }
 
    void OnTriggerExit(Collider other){
        if (other.transform==curWater) { // if exiting the waterplane...
            if (transform.position.y > curWater.position.y){
                //  null the current waterplane reference
                curWater = null; 
            }
        }
    }

}
