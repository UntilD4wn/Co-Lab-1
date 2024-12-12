using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScanner : MonoBehaviour
{
    public GameObject TerrainScannerPrefab;
    public float duration = 10;
    public float size = 500;

    public float scanDelay = 5f;

    private bool canScan = true;
    public GameObject targetPrefab;

    public void CreateNewPoint()
    {
        GameObject[] existingPoints;
        existingPoints=GameObject.FindGameObjectsWithTag("TargetPoint");
        foreach(GameObject gameObjects in existingPoints)
        {
            Destroy(gameObjects);
        }
        Instantiate(targetPrefab, transform.position, Quaternion.identity);
    }

    IEnumerator ScanTimes(){
        SpawnTerrainScanner();
        yield return new WaitForSeconds(0.1f);
        SpawnTerrainScanner();
        yield return new WaitForSeconds(0.1f);
        SpawnTerrainScanner();
        yield return new WaitForSeconds(0.1f);
        SpawnTerrainScanner();
        yield return new WaitForSeconds(0.1f);
        SpawnTerrainScanner();
        yield return new WaitForSeconds(0.1f);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Scan();
        }
    }

    public void Scan()
    {
        if(canScan == true)
        {
            StartCoroutine(ScanTimes());
            CreateNewPoint();
            StartCoroutine(ScanCooldown());
        }
    }

    public IEnumerator ScanCooldown()
    {

        canScan = false;
        Debug.Log("Scan countdown started. Can scan: " + canScan);
        yield return new WaitForSeconds(scanDelay);
        
        canScan = true;
        Debug.Log("Can now scan again. Can scan: " + canScan);
    }

    void SpawnTerrainScanner()
    {
        GameObject terrainScanner = Instantiate(TerrainScannerPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        ParticleSystem terrainScannerPS = terrainScanner.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (terrainScannerPS != null)
        {
            var main = terrainScannerPS.main;
            main.startLifetime = duration;
            main.startLifetime = size;
        }

        else
            Debug.Log("The first child doesnt have particle system.");
    }

}
