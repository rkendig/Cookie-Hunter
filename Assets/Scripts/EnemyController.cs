using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
    public float lookRadius; //lookRadius is how wide the AI's aggro range is
    public float patrolTimer, patrolRadius, cullRadius; //patrolTimer is how long it takes for the AI switch directions when patrolling, patrolRadius is how wide it will patrol
    private Transform target;
    private float timer = 10f, random, distanceToPlayer;
    private bool aggroed;
    public static bool gameOver = false;
    NavMeshAgent agent;
    private List<KeyValuePair<Vector3,NavMeshPath>> paths; //the key is the starting position 
    Vector3 nextPos, start;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player").GetComponent<Transform>();
        random = UnityEngine.Random.Range(0f,1.5f);
        //nextPos = start = transform.position;
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (gameOver) {
            if (Vector3.Distance(target.position, transform.position) < cullRadius) {
                Patrol(); 
            }
        }
        else {
            if (tag.Equals("Brown")) {
                BrownBehavior();
            }
            else if (tag.Equals("Violet")) {
                VioletBehavior();
            } 
            else if (tag.Equals("White")) {
                WhiteBehavior();
            }
            else {
                GreenBehavior();
            } 
        }
    }

    private void BrownBehavior() {
        distanceToPlayer = Vector3.Distance(target.position, transform.position);
        //If the player is fall enough away, do nothing, to cut back on runtime calculations and improve performance
        if (distanceToPlayer > cullRadius) {
            if (agent.hasPath) agent.ResetPath(); 
        }
        //Otherwise, do some behavior
        else {
            if (!aggroed && distanceToPlayer <= lookRadius) {
                aggroed = true; //If player is in range, aggro = true
            }
            else if (aggroed && distanceToPlayer > 15) {  //Break aggro if player gets past 15 units 
                aggroed = false;
            }
            if (aggroed) {
                if (!agent.pathPending) {
                    agent.SetDestination(target.position);
                }
            }
            else {
                Patrol();
            }
        }
    }

    private void VioletBehavior() {
        timer += Time.deltaTime; //use a patrol timer to ensure the SetDestination() func isn't being called too many times a frame.
        if (!agent.pathPending && timer > patrolTimer) {
            agent.SetDestination(target.position);
            timer = 0;
        }
    }

    private void WhiteBehavior() {
        distanceToPlayer = Vector3.Distance(target.position, transform.position);
        //If the player is fall enough away, do nothing, to cut back on runtime calculations and improve performance
        if (distanceToPlayer > cullRadius) {
            if (agent.hasPath) agent.ResetPath(); 
        }
        //Otherwise, do some behavior
        else {
            if (!aggroed && distanceToPlayer <= lookRadius) {
                aggroed = true; //If player is in range, aggro = true
            }
            else if (aggroed && distanceToPlayer > 12) {  //Break aggro if player gets past 12 units 
                aggroed = false;
            }
            if (aggroed) {
                if (!agent.pathPending) {
                    agent.SetDestination(target.position);
                }
            }
            else {
                Patrol();
            }
        }
    }

    private void GreenBehavior() {
        if (aggroed && Vector3.Distance(target.position, transform.position) > lookRadius) {
            agent.ResetPath();
            agent.baseOffset = 0;
            agent.height = 0;
            aggroed = false;
        } 
        else if (aggroed) {
            if (agent.hasPath) {
                agent.baseOffset = 1.5f;
                agent.height = 1.5f;
            }
            if (!agent.pathPending) {
                agent.SetDestination(target.position);
           }
        }
        else if (Vector3.Distance(target.position, transform.position) <= lookRadius) {
            aggroed = true;
        }     
    }

    /* The enemy will pick a random spot within the patrolRadius and move towards it, picking a new spot every patrolTime seconds or once they reach their destination */
    private void Patrol() {
        timer += Time.deltaTime + random; //Add a little bit of randomness
        if (timer >= patrolTimer || agent.transform.position == agent.destination) {
            Vector3 newPos = RandomNavSphere(transform.position, patrolRadius, -1);
            if (!agent.pathPending) {
                agent.SetDestination(newPos);
            }          
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist; //get a random vector
        randDirection += origin; //add current position
 
        NavMeshHit navHit;
        bool good;
        good = NavMesh.SamplePosition(randDirection, out navHit, dist, layermask); //Get the position on the NavMesh
        if (good) return navHit.position;
        else return RandomNavSphere(origin, dist, layermask);
    }
        
    private void OnDrawGizmosSelected()
    {
        try {
        if (agent.pathPending) {
            Gizmos.color = Color.red;
        }
        else if (aggroed) {
            Gizmos.color = Color.blue;
        }
        else Gizmos.color = Color.green;
        }
        catch (Exception e) {
            Gizmos.color = Color.yellow;
        }
        Gizmos.DrawWireSphere(transform.position,lookRadius);
    }

    
    //void OnTriggerEnter(Collider other) {
    //    if (other.name.Equals("Player")) {
    //        gameOver = true;
    //    }
    //}

    //Precomputes the patrol route for some cookies -- THIS FAILED
    public void GetWaypoints() {
        Vector3 position;
        NavMeshPath tempPath;
        paths = new List<KeyValuePair<Vector3, NavMeshPath>>();
        for (int i = 0; i < 3; i++) { 
            position = RandomNavSphere(transform.position, patrolRadius, -1);
            tempPath = new NavMeshPath();
            if (agent.CalculatePath(position, tempPath)) {
                paths.Add(new KeyValuePair<Vector3, NavMeshPath>(nextPos,tempPath));
                nextPos = position;
            }
            else i--;
        }
        position = start;
        tempPath = new NavMeshPath();
        agent.CalculatePath(position, tempPath);
        paths.Add(new KeyValuePair<Vector3, NavMeshPath>(nextPos,tempPath));
        nextPos = position;  
    }

}
