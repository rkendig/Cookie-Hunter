using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeInteract : MonoBehaviour {
    PlayerController pc;
    private Text interactionText;

	// Use this for initialization
	void Start () {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        interactionText = GameObject.Find("Interaction Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
     
	}

    //If the player walks over the cookie, let the player controller know this is an interactable object
    void OnTriggerEnter(Collider other) {
        if (other.name.Equals("Player")) {
            pc.interactable = this.gameObject;
            interactionText.text = "Press F to find cookie";
        }
    }

    private void OnTriggerExit(Collider other) {
       if (other.name.Equals("Player")) {
            interactionText.text = "";
            pc.interactable = null;
        }
    }
}
