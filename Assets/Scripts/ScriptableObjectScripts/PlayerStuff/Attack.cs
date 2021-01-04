using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Text Adventure/Character/Attack")]
public class Attack : ScriptableObject
{
    public int AttackID;

    [TextArea(3,3)]  public string attackDescriptionBase;
    [TextArea(3,3)]  public string attackDescriptionSuccess;
    [TextArea(3,3)]  public string attackDescriptionFailure;

    public string attackDescSmall;
    public int damageDiceAmount;
    public int damageDiceType;
    public int attackBonus;

    public int AttackDC;

}
