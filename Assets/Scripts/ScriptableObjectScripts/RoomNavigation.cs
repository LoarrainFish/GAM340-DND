using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNavigation : MonoBehaviour {

    public Room currentRoom;
    private GameManager gameManager;

    

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    public void UnpackExitsInRoom()
    {
        for (int i = 0; i < currentRoom.exits.Length; i++)
        {
            string tempExitString = (i+1 + "." + currentRoom.exits[i].exitDescription);
            gameManager.tempExitLocations.Add(tempExitString);
            gameManager.exitIDs.Add(currentRoom.exits[i].RoomID);

        }

        gameManager.ConvertExits();
    }



}
