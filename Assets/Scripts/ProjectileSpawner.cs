using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject[] projectilePrefabs;
    public Transform whichHand;

    // Start is called before the first frame update
    public void Spawn(string objectName)
    {
        for(int i = 0;i<projectilePrefabs.Length;i++)
        {
            if(objectName == projectilePrefabs[i].name)
            {
                var projectileInstantiate = Instantiate(projectilePrefabs[i], whichHand.position, Quaternion.identity) as GameObject;
                Destroy(projectileInstantiate.gameObject, 3f);
            }
        }
    }


}
