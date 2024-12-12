using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGeneration : MonoBehaviour
{

    public NavMeshBuildSettings buildSettings; 
    public LayerMask navMeshLayer; 
    public NavMeshData navMeshData; 
    private NavMeshDataInstance navMeshInstance; 

    public LayerMask includedLayers; 

    

    public Transform roomsParent;

    [SerializeField] private GameObject straightPrefab;
    [SerializeField] private GameObject endPrefab;
    [SerializeField] private GameObject turnLeftPrefab;
    [SerializeField] private GameObject turnRightPrefab;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject enemyPrefab;

    public int pathLength = 10; 
    public Vector3 roomSize = new Vector3(10, 0, 10); 

    private Vector3 currentPosition = Vector3.zero; 
    private Quaternion currentRotation = Quaternion.identity; 
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>(); 

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(enemyPrefab, new Vector3(0,0,0), Quaternion.identity);
    }

    void Start()
    {
        GeneratePath();

        navMeshData = new NavMeshData();
        navMeshInstance = NavMesh.AddNavMeshData(navMeshData);

        
        BakeNavMesh();
        StartCoroutine(SpawnEnemy());
    }

    

    void BakeNavMesh()
    {
        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
        foreach (Transform room in roomsParent)
        {
            
            CollectRoomSources(room, sources);
        }
        Bounds navMeshBounds = new Bounds(Vector3.zero, Vector3.zero);
        foreach (Transform room in roomsParent)
        {
            
            foreach (Transform child in room)
            {
                Renderer childRenderer = child.GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    navMeshBounds.Encapsulate(childRenderer.bounds);
                }
            }
        }

        navMeshData = NavMeshBuilder.BuildNavMeshData(
            buildSettings, 
            sources, 
            navMeshBounds, 
            Vector3.zero, 
            Quaternion.identity 
        );

        if (navMeshData != null)
        {
            navMeshInstance = NavMesh.AddNavMeshData(navMeshData);
        }
        else
        {
            Debug.LogError("Failed to build NavMeshData.");
        }
    }

    
    void CollectRoomSources(Transform room, List<NavMeshBuildSource> sources)
    {
    
    foreach (Transform child in room)
    { 
        int layer = child.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Walkable") || layer == LayerMask.NameToLayer("Wall")) 
        {
            NavMeshBuildSource source = new NavMeshBuildSource();
            MeshFilter meshFilter = child.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {   
                source.sourceObject = meshFilter.sharedMesh;
                source.transform = child.localToWorldMatrix;
                
                if (layer == LayerMask.NameToLayer("Wall"))
                {
                    
                    source.area = 1; 
                }
                else
                {
                    source.area = 0; 
                }

                sources.Add(source);
            }

            
            MeshCollider meshCollider = child.GetComponent<MeshCollider>();
            if (meshCollider != null && meshCollider.enabled)
            {  
                source.sourceObject = meshCollider.sharedMesh;
                source.transform = child.localToWorldMatrix;
                if (layer == LayerMask.NameToLayer("Wall"))
                {
                    source.area = 1; 
                }
                else
                {
                    source.area = 0; 
                }
                sources.Add(source);
            }
        }
    }
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
        GameObject newRoom = Instantiate(prefab, currentPosition, currentRotation, roomsParent);
        occupiedPositions.Add(currentPosition);
        newRoom.GetComponent<Room>().SetOccupiedPositionsIndex(occupiedPositions.Count);


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