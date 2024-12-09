using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{

    //by Matt

    [SerializeField] private GameObject straightPrefab;
    [SerializeField] private GameObject endPrefab;
    [SerializeField] private GameObject turnLeftPrefab;
    [SerializeField] private GameObject turnRightPrefab;
    [SerializeField] private GameObject playerPrefab;

    public int pathLength = 10; 
    public Vector3 roomSize = new Vector3(10, 0, 10); 

    private Vector3 currentPosition = Vector3.zero; 
    private Quaternion currentRotation = Quaternion.identity; 
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>(); 

    void Start()
    {
        GeneratePath();
    }

    void GeneratePath()
    {
        // Places first starting room
        PlaceRoom(straightPrefab);

        //Generates a room  pathLength amount of times.
        for (int i = 1; i < pathLength - 1; i++)
        {
            GameObject roomPrefab = ChooseRandomRoom();
            PlaceRoom(roomPrefab);
        }

        
        PlaceRoom(endPrefab);
        //Spawn player once generating finished
        Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity);
    }

    GameObject ChooseRandomRoom()
    {
        List<GameObject> validPrefabs = new List<GameObject>();
        Vector3 nextStraight = SimulateNextPosition(straightPrefab);
        Vector3 nextLeft = SimulateNextPosition(turnLeftPrefab);
        Vector3 nextRight = SimulateNextPosition(turnRightPrefab);

        if (IsPositionValid(nextStraight) && !WillCauseLoop(nextStraight))
            validPrefabs.Add(straightPrefab);

        if (IsPositionValid(nextLeft) && !WillCauseLoop(nextLeft))
            validPrefabs.Add(turnLeftPrefab);

        if (IsPositionValid(nextRight) && !WillCauseLoop(nextRight))
            validPrefabs.Add(turnRightPrefab);

        if (validPrefabs.Count > 0)
        {
            return validPrefabs[Random.Range(0, validPrefabs.Count)];
        }

        
        return straightPrefab;
    }

    //Added this to ensure there is no loops (e.g left, left, left)
    //this would result in a deadend

    bool WillCauseLoop(Vector3 nextPosition)
    {
        foreach (Vector3 position in occupiedPositions)
        {
            if (Vector3.Distance(nextPosition, position) < roomSize.x * 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    Vector3 SimulateNextPosition(GameObject prefab)
    {
        Quaternion nextRotation = currentRotation;

        if (prefab == turnLeftPrefab)
            nextRotation *= Quaternion.Euler(0, -90, 0); 
        else if (prefab == turnRightPrefab)
            nextRotation *= Quaternion.Euler(0, 90, 0); 

        return currentPosition + nextRotation * (roomSize.x * Vector3.right);
    }

    bool IsPositionValid(Vector3 position)
    {
        
        return !occupiedPositions.Contains(position);
    }

    void PlaceRoom(GameObject prefab)
    {
        Instantiate(prefab, currentPosition, currentRotation);
        occupiedPositions.Add(currentPosition);

        if (prefab == straightPrefab)
        {
            currentPosition += currentRotation * (roomSize.x * Vector3.right); 
        }
        else if (prefab == turnLeftPrefab)
        {
            currentRotation *= Quaternion.Euler(0, -90, 0); 
            currentPosition += currentRotation * (roomSize.x * Vector3.right); 
        }
        else if (prefab == turnRightPrefab)
        {
            currentRotation *= Quaternion.Euler(0, 90, 0); 
            currentPosition += currentRotation * (roomSize.x * Vector3.right); 
        }
    }
}