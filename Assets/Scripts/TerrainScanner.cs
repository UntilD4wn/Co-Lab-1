using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScanner : MonoBehaviour
{
    public GameObject TerrainScannerPrefab;
    public float duration = 10;
    public float size = 500;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SpawnTerrainScanner();
        }
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
