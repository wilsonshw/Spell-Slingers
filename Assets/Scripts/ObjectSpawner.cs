using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singleton
{
    public class ObjectSpawner
    {
        public List<GameObject> objects;
        // Start is called before the first frame update
        public void Spawn(string objectName)
        {
            foreach (var item in objects)
            {
                item.SetActive(objectName == item.name);
            }
        }
    }
}
