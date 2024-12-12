using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int occupiedPositionsIndex;

    public void SetOccupiedPositionsIndex(int newIndex)
    {
        occupiedPositionsIndex = newIndex;
    }

    public int GetOccupiedPositionsIndex()
    {
        return occupiedPositionsIndex;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
