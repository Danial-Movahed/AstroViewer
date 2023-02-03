using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycasting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2000, Color.yellow);
        if(Input.GetKey(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }
        }
    }
}
