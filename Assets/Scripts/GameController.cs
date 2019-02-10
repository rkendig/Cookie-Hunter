using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public int numOfCookies, numInTrees, percentBrown, percentGreen, percentViolet, percentWhite;
    public GameObject BrownCookie, GreenCookie, WhiteCookie, VioletCookie; //These can be accessed thru Resources/Load
    public Terrain terrain;
    public Transform Pickups;
    private List<KeyValuePair<GameObject,int>> probabilities;
    private List<GameObject> spawnedCookies;
    TerrainData terrainData;
    private int score;
    Text scoreText, timeText, centerText, lifeText;
    public float timeLeft; 
    public Light sun, moon;
    public float timeStart; //How much time to begin with
    private bool phaseOne, phaseTwo, comingFromP1, first, isGameOver;
    CameraController cc;

	// Use this for initialization
	void Start () {
        terrainData = terrain.terrainData;
        spawnedCookies = new List<GameObject>();
		probabilities = new List<KeyValuePair<GameObject,int>>();
        probabilities.Add(new KeyValuePair<GameObject, int>(BrownCookie,percentBrown));
        probabilities.Add(new KeyValuePair<GameObject, int>(GreenCookie,percentGreen));
        probabilities.Add(new KeyValuePair<GameObject, int>(VioletCookie,percentViolet));
        probabilities.Add(new KeyValuePair<GameObject, int>(WhiteCookie,percentWhite));
        probabilities.Sort((x, y) => x.Value.CompareTo(y.Value)); //Sort values based on thier probabilities

        phaseOne = true;
        first = true;
        timeLeft = timeStart;
        scoreText = GameObject.Find("Score Text").GetComponent<Text>();
        timeText = GameObject.Find("Time Text").GetComponent<Text>();
        centerText = GameObject.Find("Center Text").GetComponent<Text>();
        lifeText = GameObject.Find("Lives Text").GetComponent<Text>();
        cc = GameObject.Find("Main Camera").GetComponent<CameraController>();
        spawnPickups();
        
	}
	
	// Update is called once per frame
	void Update () {
        //Keep track of time
        timeLeft -= Time.deltaTime;        
        if ((int)Time.timeSinceLevelLoad == 1) {
            centerText.text = "";
        }

        //Take any input from the player (that isn't movement)
        if (Input.GetKeyDown("z")) {
            timeLeft = .5f; //Shortkey to phase 2
            //sun.transform.position = new Vector3(sun.transform.position.x, 0, sun.transform.position.z);
            //sun.transform.LookAt(new Vector3(311,16,300));
            //moon.transform.position = new Vector3(moon.transform.position.x, 0, moon.transform.position.z);
            //moon.transform.LookAt(new Vector3(311,16,300));
        } 

        if ((phaseOne || phaseTwo) && !isGameOver) {  
            //Update Time text
            timeText.text = "Time: 0" + Mathf.Floor(timeLeft/60) + ":" + ((int)timeLeft%60).ToString("D2");
            if (timeLeft < 6) {
                centerText.text = "Next Phase Starting In: " + (int)timeLeft%60;
            }
            //Rotate Sun and Moon
            sun.transform.RotateAround(new Vector3(311,16,300), -Vector3.right,(180f/timeStart)*Time.deltaTime);
            sun.transform.LookAt(new Vector3(311,16,300));
            moon.transform.RotateAround(new Vector3(311,16,300), -Vector3.right,(180f/timeStart)*Time.deltaTime);
            moon.transform.LookAt(new Vector3(311,16,300));

            if (phaseOne && timeLeft <= 0) {
                timeText.text = "Time: 00:00";
                sun.GetComponent<Light>().enabled = false;
                moon.GetComponent<Light>().enabled = true;
                phaseOne = false;
                comingFromP1 = true;
                //timeLeft = 6; 
                cc.Freeze(); //Freeze the player
            } 
            else if (phaseTwo && timeLeft <= 0) {
                timeText.text = "Time: 00:00";
                sun.GetComponent<Light>().enabled = true;
                moon.GetComponent<Light>().enabled = false;
                phaseTwo = false;
                //timeLeft = 6;
                cc.Freeze(); //Freeze the player
                //GameOver()
            }
        }
        else { //In Between (add a 5second buffer between phases)
           
            if (!comingFromP1 && first) {
                //Turn on the pickup and off the chasing behavior
                foreach (GameObject g in spawnedCookies) {
                        if (g != null) {
                            g.GetComponent<Pickup>().enabled = true;
                            g.GetComponent<Rigidbody>().isKinematic = false;
                            g.GetComponent<BoxCollider>().enabled = true;
                            g.GetComponent<NavMeshAgent>().enabled = false;
                            g.GetComponent<EnemyController>().enabled = false;
                        }
                }
                first = false;
            }

            if (timeLeft <= 0) {
                centerText.text = "";
                timeLeft = timeStart;
                cc.UnFreeze();
                if (comingFromP1) {
                    foreach (GameObject g in spawnedCookies) {
                        if (g != null) {
                            g.GetComponent<BoxCollider>().enabled = false;
                            g.GetComponent<Rigidbody>().isKinematic = true;
                            g.GetComponent<Pickup>().enabled = false;
                            g.GetComponent<NavMeshAgent>().enabled = true;
                            EnemyController ec = g.GetComponent<EnemyController>();
                            ec.enabled = true;
                        }
                    }
                    lifeText.text = "Lives left: 5";
                    Pickup.hits = 0;
                    phaseTwo = true;
                    comingFromP1 = false;
                    first = true;
                }
                else {
                    lifeText.text = "";
                    phaseOne = true;
                } 
            }
        }        
	}
    
    /* We are gonna create and spawn prefab cookie pickups based on the given probalities for each 
       There are going to be a number of cookies that are "placed" in trees so that when the player interacts
       with the tree, the cookie falls out. To do this, just pick a random trees and record their position*/
    void spawnPickups() {

        Transform trees = GameObject.Find("Interactable Trees").GetComponent<Transform>();
        //Create as many cookie pickups as signified by the numOfCookies parameter minus the ones that will go in trees
        for (int i = 0; i < numOfCookies - numInTrees; i++) {
            GameObject selectedCookie = null;

            //Step 1: Select a random type of cookie based on the given probabilites
            int randNum = Random.Range(0,100); //Get a random number
            int cumulativeProb = 0; //cumulative prob
            foreach (KeyValuePair<GameObject,int> item in probabilities) {
                cumulativeProb += item.Value;
                if (randNum < cumulativeProb) {
                    selectedCookie = item.Key;
                    break;
                }          
            }

            //Step 2: Select a random location
            int x = Random.Range(200,420); //random x coordinate within wall bounds
            int z = Random.Range(190,390);  //random z coordinate within wall bounds
            int y = 22; //Drop the cookies from this height
            Vector3 position = new Vector3(x,y,z);

            //Step 3: Create the cookie prefab 
            spawnedCookies.Add(Instantiate(selectedCookie, position, Quaternion.identity,Pickups));

            //Step 4: Create an interactable tree / bush
            if (i < numInTrees) {
                Vector3 treePos = Vector3.Scale(terrainData.GetTreeInstance(Random.Range(0, terrainData.treeInstances.Length - 1)).position, terrainData.size) + terrain.transform.position;
                //If the selected position is not within bounds of my invisibale walls, get a new position
                while (treePos.x < 200 || treePos.x > 420 || treePos.z < 190 || treePos.z > 390 || (treePos.z < 230 && treePos.x < 260)) {
                    treePos = Vector3.Scale(terrainData.GetTreeInstance(Random.Range(0, terrainData.treeInstances.Length - 1)).position, terrainData.size) + terrain.transform.position;
                }
                Instantiate(Resources.Load("Interact Collider", typeof(GameObject)) as GameObject, treePos + Vector3.up, Quaternion.identity, trees);
            }
        }
    }

    //Add to the score
    public void updateScore(int i) {
        score = score + i;
        scoreText.text = "Score: " + score.ToString();
    }

    public void gameOver() {
        centerText.text = "GAME OVER! \n You ate " + score + " points worth of cookies";
        GameObject.Find("Interaction Text").GetComponent<Text>().text = "Press Esc and go back to the Main Menu to restart";
        cc.Freeze(); //Freeze the Character
        EnemyController.gameOver = true;
        isGameOver = true; 
    } 
}
