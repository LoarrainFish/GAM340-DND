using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootObject", menuName = "Text Adventure/LootObject")]
public class Loot : ScriptableObject {

    public int LootID;
    public int GoldValue;
    [Tooltip("1 is Common, 2 is Uncommon, 3 is Rare, 4 is Epic, 5 is Legendary")]
    public int RarityID;
    public Effects[] effects;

}
