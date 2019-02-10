using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomForest : MonoBehaviour
{
    [Range(0, 1)]
    public float grassDensity;
    [Range(0, 1)]
    public float flowerDensity;
    [Range(0, 1)]
    public float treeDensity;
    [Range(0, 1)]
    public float rockDensity;

    public int treeSpacing;
    public int rockSpacing;
    public int totalPositions;
    public int availablePositions;

    public LayerMask obstacle;

    public GameObject[] environmentObjects;

    public GameObject grassFolder;
    public GameObject treeFolder;
    public GameObject flowerFolder;
    public GameObject rockFolder;

    private void Start()
    {
        totalPositions = Grid.instance.walkableVector3s.Count;
        availablePositions = totalPositions; 
        Debug.Log("Total available positions: " + totalPositions); 
        environmentObjects = new GameObject[7];
        environmentObjects[0] = Resources.Load("EnvironmentPrefabs/dead_tree") as GameObject;
        environmentObjects[1] = Resources.Load("EnvironmentPrefabs/tree_basswood") as GameObject;
        environmentObjects[2] = Resources.Load("EnvironmentPrefabs/grass_bulrush") as GameObject;
        environmentObjects[3] = Resources.Load("EnvironmentPrefabs/flower_bluebell2") as GameObject;
        environmentObjects[4] = Resources.Load("EnvironmentPrefabs/rock_01") as GameObject;
        environmentObjects[5] = Resources.Load("EnvironmentPrefabs/rock_02") as GameObject;
        environmentObjects[6] = Resources.Load("EnvironmentPrefabs/rock_03") as GameObject;

        placeVegetation(treeDensity, environmentObjects[1], treeSpacing, obstacle, treeFolder);
        placeVegetation(rockDensity, rockSpacing, 4, 7, obstacle, rockFolder);
        placeVegetation(grassDensity, environmentObjects[2], grassFolder);
        placeVegetation(flowerDensity, environmentObjects[3], flowerFolder); 

    }
    //for an environment object that is not an obstacle (i.e. does not make the ground underneath unwalkable for pathfinding)
    private void placeVegetation(float density, GameObject vegToPlace, GameObject parent)
    {
        int vegDensity = (int)(totalPositions * density);
        Debug.Log(vegDensity + " " + parent.name);
        if (availablePositions > 0)
        {
            for (int i = 0; i < vegDensity; i++)
            {

                Vector3 position = Grid.instance.walkableVector3s[Random.Range(0, Grid.instance.walkableVector3s.Count)];
                GameObject veg = Instantiate(vegToPlace, position, vegToPlace.transform.rotation);
                Quaternion rotation = Random.rotation;
                rotation.x = veg.transform.rotation.x;
                rotation.z = veg.transform.rotation.z;
                veg.transform.rotation = rotation;
                veg.transform.SetParent(parent.transform);
                Grid.instance.walkableVector3s.Remove(position);
                availablePositions = Grid.instance.walkableVector3s.Count;
            }
        }
    }
    //for an obstacle environment when there is only one type that needs to be instantiated
    private void placeVegetation(float density, GameObject vegToPlace, int spacing, LayerMask objToScanFor, GameObject parent)
    {
        int vegDensity = (int)(totalPositions * density);
        Debug.Log(vegDensity + " " + parent.name);
        if (availablePositions > 0)
        {
            for (int i = 0; i < vegDensity; i++)
            {
                Vector3 position = Grid.instance.walkableVector3s[Random.Range(0, Grid.instance.walkableVector3s.Count)];

                bool objInRange = (Physics.CheckSphere(position, spacing, objToScanFor));
                if (!objInRange)
                {
                    GameObject veg = Instantiate(vegToPlace, position, vegToPlace.transform.rotation);
                    Quaternion rotation = Random.rotation;
                    rotation.x = veg.transform.rotation.x;
                    rotation.z = veg.transform.rotation.z;
                    veg.transform.rotation = rotation;
                    float scaleValue = Random.Range(.5f, 1);
                    veg.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
                    veg.transform.SetParent(parent.transform);
                    Grid.instance.walkableVector3s.Remove(position);
                    availablePositions = Grid.instance.walkableVector3s.Count;
                }
            }
        }
  
    }
    //for an obstacle environment when there are several difference types that need to be instantiated
    private void placeVegetation(float density, int spacing, int lowerBound, int upperBound, LayerMask objToScanFor, GameObject parent)
    {
        int vegDensity = (int)(totalPositions * density);
        Debug.Log(vegDensity + " " + parent.name);

        if (availablePositions > 0)
        {
            for (int i = 0; i < vegDensity; i++)
            {
                Vector3 position = Grid.instance.walkableVector3s[Random.Range(0, Grid.instance.walkableVector3s.Count)];
                GameObject objectToInstantiate = environmentObjects[Random.Range(lowerBound, upperBound)];
                bool objInRange = (Physics.CheckSphere(position, spacing, objToScanFor));
                if (!objInRange)
                {
                    GameObject veg = Instantiate(objectToInstantiate, position, objectToInstantiate.transform.rotation);
                    Quaternion rotation = Random.rotation;
                    rotation.x = veg.transform.rotation.x;
                    rotation.z = veg.transform.rotation.z;
                    veg.transform.rotation = rotation;
                    float scaleValue = Random.Range(.5f, 1);
                    veg.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
                    veg.transform.SetParent(parent.transform);
                    Grid.instance.walkableVector3s.Remove(position);
                    availablePositions = Grid.instance.walkableVector3s.Count;
                }
            }
        }

    }

}