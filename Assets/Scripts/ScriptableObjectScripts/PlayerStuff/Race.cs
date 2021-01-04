using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Text Adventure/Character/Race")]
public class Race : ScriptableObject{

    public string raceName;
    public int RaceID;
    public int[] raceMod;


}
