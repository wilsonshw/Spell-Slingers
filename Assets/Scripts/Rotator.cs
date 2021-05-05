using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotateSpeed;
    public float rotateDir;
    private float dirChangeTimer;
    public bool doNotChangeDir;

    void Update()
    {
        if (!doNotChangeDir)
        {
            if (dirChangeTimer >= 0)
                dirChangeTimer -= Time.deltaTime;
            else
            {
                rotateDir *= -1;
                dirChangeTimer = Random.Range(2, 6);
            }
        }


        transform.Rotate(0, 0, rotateSpeed * rotateDir);
    }
}
