using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    Transform target;
    Vector3 initialPos;

    [Range(0f, 4f)]
    public float intensity;

	// Use this for initialization
	void Start ()
    {
        target = GetComponent<Transform>();
        initialPos = target.localPosition;
	}

    float pendingShakeDuration = 0f;
	
    public void Shake(float shakeDuration)
    {
        if(shakeDuration > 0)
        {
            pendingShakeDuration += shakeDuration;
        }
    }

    bool isShaking = false;

    void Update()
    {
        if(pendingShakeDuration > 0 && !isShaking)
        {
            StartCoroutine(DoShake());
        }

    }

    IEnumerator DoShake()
    {
        isShaking = true;

        var startTime = Time.realtimeSinceStartup;

        while(Time.realtimeSinceStartup < startTime + pendingShakeDuration)
        {
            var randomPoint = new Vector3(Random.Range(-1f, 1f)* intensity, Random.Range(-1f, 1f) * intensity, initialPos.z);
            target.localPosition = randomPoint;
            yield return null;
        }

        pendingShakeDuration = 0;
        target.localPosition = initialPos;
        isShaking = false;
    }


}
