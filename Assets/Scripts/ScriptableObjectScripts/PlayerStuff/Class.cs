using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Text Adventure/Character/Class")]
public class Class : ScriptableObject{

    public string className;
    public int ClassID;
    public int HP;
    public int startingGold;
    public int baseDamage;

    public int[] abilityScores;

    public Attack[] attacks;
}
