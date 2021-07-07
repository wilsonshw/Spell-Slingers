using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StaticSpawner : MonoBehaviour
{
    public GameObject[] staticPrefabs;
    public Vector3 spawnPosition;

    // Start is called before the first frame update
    public void Spawn(string objectName)
    {
        for (int i = 0; i < staticPrefabs.Length; i++)
        {
            if (objectName == staticPrefabs[i].name)
            {
                var staticInstantiate = Instantiate(staticPrefabs[i], spawnPosition, Quaternion.identity) as GameObject;
                Destroy(staticInstantiate.gameObject, 10f);
            }
        }
    }


}
