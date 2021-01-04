using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HP : MonoBehaviour {

    public Transform bar;

    public float totalHP;
    public float currentHP;
    

    public TextMeshProUGUI HPText;

	// Use this for initialization
	private void Start () {
        Transform bar = transform.Find("Bar");
        CalculateHealthSize();
    }

    public void CalculateHealthSize()
    {
        float tempHealth = 100 / totalHP / 100;

        float SizeChange = currentHP * tempHealth;
        SetSize(SizeChange);
        

    }

    public void SetSize (float sizeNormalized)
    {
        bar.localScale = new Vector3(1f, sizeNormalized);
    }

}
