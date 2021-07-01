using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SummonSpawner : MonoBehaviour
{
    public GameObject[] meteorPrefabs;
    public string whichHand;
    public float timer;
    public bool rightHandDone;
    public bool leftHandDone;

    // Start is called before the first frame update
    public void Spawn(string objectName)
    {
        for (int i = 0; i < meteorPrefabs.Length; i++)
        {
            objectName = "Meteor";
            if (objectName == meteorPrefabs[i].name)
            {
                Vector3 forwardDir = Camera.main.transform.forward;
                forwardDir.y = 0;
                forwardDir.Normalize();
                var meteorInstantiate = Instantiate(meteorPrefabs[i], transform.position + forwardDir * 5, Quaternion.identity) as GameObject;
                Destroy(meteorInstantiate.gameObject, 3f);
            }
        }
       

    }

    private void Update()
    {

        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            rightHandDone = false;
            leftHandDone = false;
        }
        /*if (rightHandDone)
        {
            
            else
            {
                castMeteor = false;
            }
        }

        if (leftHandDone)
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
                if (rightHandDone)
                {
                    castMeteor = true;
                    rightHandDone = false;
                    leftHandDone = false;
                }
            }
            else
            {
                castMeteor = false;
            }
        }*/
    }


}
