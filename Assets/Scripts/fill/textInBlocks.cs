using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * To put texts above blocks 
 */
public class textInBlocks : MonoBehaviour
{
    Camera camera;
    Vector3 lockPos;
    Quaternion lockRot;
    
    void Start()
    {
        camera = GetComponent<Canvas>().worldCamera;
        lockPos = transform.position;
        lockRot = transform.rotation;
    }
    
    void Update()
    {      
        updateRotation();
    }

    private void updateRotation()
    {
        Vector3 lookVector = camera.transform.forward * 90;
        lookVector.y = lockRot.y;
        transform.LookAt(lookVector);
    }
}
