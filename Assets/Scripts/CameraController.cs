using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    //We dont need the whole gameObject, just the transforms
	public Transform playerCam, character, centerPoint;
    CharacterController player;

	private float mouseX, mouseY;
    [Range(0,10)]
	public float mouseSensitivity, cameraPosition;

	private float moveFB, moveLR;
	[Range(0,25)]
    public float walkSpeed, runSpeed, jumpDist;
    private float verticalVelocity = 0f;

	private float zoom;
    [Range(0,20)]
	public float zoomSpeed = 2;

	public float zoomMin, zoomMax,clampMin, clampMax;
	public float rotationSpeed = 5f;
    private int jumpTimes;
    private float timeSpentRunning;
    private bool isRunning, isFrozen;
    

	// Use this for initialization
	void Start () {
		zoom = -3;
        player = GameObject.Find("Player").GetComponent<CharacterController>();

        //Spawn the player in 1 of 3 spots
        Vector3[] possibleSpawnPoints = new Vector3[] { new Vector3(290, 16, 290), new Vector3(252, 16, 307), new Vector3(365, 16, 250), new Vector3(350,17,182) };
        Vector3 spawnPoint = possibleSpawnPoints[Random.Range(0, 3)];
        player.transform.position = spawnPoint;
        centerPoint.transform.position = spawnPoint;
        transform.position = spawnPoint;
    }

    // Update is called once per frame
    void Update () {
        //changeZoom(); This function was causing bugs, perhaps I'll fix it later and put it back in


        /* Deal with panning the camera. A player can right-click and drag the mouse to
         * initiate the panning of the camera */
   		if (Input.GetMouseButton (1)) {
			mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
			mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity; // sign designates inverted panning
		}
        //Camera never rotates, the center point does
		mouseY = Mathf.Clamp (mouseY, clampMin, clampMax); //Clamp rotation
		playerCam.LookAt(centerPoint);
		centerPoint.localRotation = Quaternion.Euler(mouseY, mouseX, 0);
        

        /* Deal with running vs walking. The player has about 4 seconds worth of running/energy
           before they must stop and recover. While the player is running, timeSpendRunning is
           incremented to a max of 4. While the player is walking, timeSpentRunning is decremented
           to a min of 0. It is decremented half as fast as it is incremented. The player can run
           for a time equal to 4 - timeSpentRunning*/
        if (Input.GetMouseButtonDown(0) && jumpTimes == 0)  {
            isRunning = true;
        }
        if (Input.GetMouseButtonUp(0) || timeSpentRunning >= 4 ) {
            isRunning = false;
        }
        //Calculate movement if the player is running and increment timeSpendRunning
        if (isRunning) {
            timeSpentRunning += Time.deltaTime;
            moveFB = Input.GetAxis("Vertical") * runSpeed;
            moveLR = Input.GetAxis("Horizontal") * runSpeed;
        }
        else {
            //Calculate movement if the player is walking and decrement timeSpendRunning
            timeSpentRunning = Mathf.Max(0, timeSpentRunning - (Time.deltaTime * .5f));
		    moveFB = Input.GetAxis("Vertical") * walkSpeed;
		    moveLR = Input.GetAxis("Horizontal") * walkSpeed;
        }


        /* Deal with actually moving the character */
        if (!isFrozen) {
            Vector3 movement = new Vector3 (moveLR, verticalVelocity, moveFB);
		    movement = character.rotation * movement;
		    player.Move(movement * Time.deltaTime);
		    centerPoint.position = new Vector3 (character.position.x, character.position.y + cameraPosition, character.position.z);
        }
        //turn our player to face the way they are going
		if (Input.GetAxis ("Vertical") > 0 | Input.GetAxis ("Vertical") < 0) {
			Quaternion turnAngle = Quaternion.Euler (0, centerPoint.eulerAngles.y, 0);
			character.rotation = Quaternion.Slerp (character.rotation, turnAngle, Time.deltaTime * rotationSpeed);
		}


        /* Deal with jumping. A player may jump using the space bar only if they are on the ground. 
         * Hence, double jumps are not allowed. */
       if (player.isGrounded) {
            verticalVelocity = Mathf.Max(0f, verticalVelocity);
            jumpTimes = 0;
       }
       else verticalVelocity += Physics.gravity.y * Time.deltaTime;

       if (Input.GetKeyDown("space") && jumpTimes < 1 && !isFrozen) {
            verticalVelocity += jumpDist;
            jumpTimes++;
        }
    }
    //void FixedUpdate(){
    //    if (player.isGrounded)
    //    {
            
    //    }
    //    else verticalVelocity += Physics.gravity.y * Time.deltaTime;
    //}


    private void changeZoom() {
        
		zoom += Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;

		if (zoom > zoomMin) {
			zoom = zoomMin;
        }
		if (zoom < zoomMax) {
			zoom = zoomMax;
        }
        //Zoom the camera
		playerCam.transform.localPosition = new Vector3 (0, 0, zoom);
    }
    //Will Toggle if character is frozen or not
    public void Freeze(){
        isFrozen = true;
    }

    public void UnFreeze() {
        isFrozen = false;
    }

    
}
