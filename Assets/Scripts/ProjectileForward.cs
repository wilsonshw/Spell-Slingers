using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ProjectileForward : MonoBehaviour
{
    private void Start()
    {
        Vector3 lookPos = transform.position + Camera.main.transform.forward;
        //lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, 5 * Time.deltaTime);
    }
}
