using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour {

    private int rollAmount;

    public GameManager gameManager;

    public void d20(int RollAmount)
    {
        for (int i = 0; i < RollAmount; i++)
        {
            gameManager.RollOutcome = Random.Range(0, 21);
        }    
    }

    public void d12(int RollAmount)
    {
        for (int i = 0; i < RollAmount; i++)
        {
            gameManager.RollOutcome = Random.Range(0, 21);
        }
    }

    public void d10(int RollAmount)
    {
        for (int i = 0; i < RollAmount; i++)
        {
            gameManager.RollOutcome = Random.Range(0, 21);
        }
    }

    public void d8(int RollAmount)
    {
        for (int i = 0; i < RollAmount; i++)
        {
            gameManager.RollOutcome = Random.Range(0, 21);
        }
    }

    public void d6(int RollAmount)
    {
        for (int i = 0; i < RollAmount; i++)
        {
            gameManager.RollOutcome = Random.Range(0, 21);
        }
    }

    public void d4(int RollAmount)
    {
        for (int i = 0; i < RollAmount; i++)
        {
            gameManager.RollOutcome = Random.Range(0, 21);
        }

    }
        
}
