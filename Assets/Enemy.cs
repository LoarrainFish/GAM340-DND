using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Text Adventure/Enemy/Enemy")]
public class Enemy : ScriptableObject {

    public string enemyName;
    public int enemyID;
    public int enemyHP;
    public int enemyDamage;
    public int enemyCR;
    public int enemyXPValue;
}
