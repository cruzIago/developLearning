using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Just to show the name above blocks 
 */
public class textInWells : MonoBehaviour
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
        lookVector.x = lockRot.x;
        lookVector.y = lockRot.y;
        transform.LookAt(lookVector);
    }
}
