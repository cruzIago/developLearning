using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textInWells : MonoBehaviour
{
    Camera camera;
    Vector3 lockPos;
    Quaternion lockRot;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Canvas>().worldCamera;
        lockPos = transform.position;
        lockRot = transform.rotation;
    }

    // Update is called once per frame
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
