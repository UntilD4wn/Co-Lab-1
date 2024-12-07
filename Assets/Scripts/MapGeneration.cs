using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public GameObject straightPrefab;
    public GameObject endPrefab;
    public GameObject turnLeftPrefab;
    public GameObject turnRightPrefab;

    public int pathLength = 10; 
    public Vector3 roomSize = new Vector3(10, 0, 10); 

    private Vector3 currentPosition = Vector3.zero; 
    private Quaternion currentRotation = Quaternion.identity; 
    private HashSet<Vector3> placedRooms = new HashSet<Vector3>(); 

    void Start()
    {
        GeneratePath();
    }

    void GeneratePath()
    {
        
        PlaceRoom(straightPrefab);

        
        for (int i = 1; i < pathLength - 1; i++)
        {
            GameObject roomPrefab = ChooseRandomRoom();
            PlaceRoom(roomPrefab);
        }

        
        PlaceRoom(endPrefab);
    }

    GameObject ChooseRandomRoom()
    {
        int randomChoice = Random.Range(0, 100);

        if (randomChoice < 50) 
        {
            if (IsPositionValid(SimulateNextPosition(straightPrefab)))
                return straightPrefab;
        }

        if (randomChoice < 75) 
        {
            if (IsPositionValid(SimulateNextPosition(turnLeftPrefab)))
                return turnLeftPrefab;
        }

        
        if (IsPositionValid(SimulateNextPosition(turnRightPrefab)))
            return turnRightPrefab;

        
        return straightPrefab;
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
        return !placedRooms.Contains(position);
    }

    void PlaceRoom(GameObject prefab)
    {
        Instantiate(prefab, currentPosition, currentRotation);
        placedRooms.Add(currentPosition);
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