using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    [SerializeField] private int roomIndex = 0;
    private CharacterController characterController;

    public void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void SetRoomIndex(int newIndex){
        roomIndex = newIndex;
    }

    public int GetRoomIndex()
    {
        return roomIndex;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.collider.CompareTag("Room"))
        {
            roomIndex = hit.gameObject.GetComponentInParent<Room>().GetOccupiedPositionsIndex();
        }
    }
}
