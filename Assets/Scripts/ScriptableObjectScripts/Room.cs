using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Text Adventure/Room") ]
public class Room : ScriptableObject {

    public int roomID;
    [TextArea(3, 6)]
    public string description;
    public string roomName;
    public string examineRoom;
    public Exit[] exits;
    public Effects[] effects;
}
