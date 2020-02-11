using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_camera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;

    private Vector3 offset;
    private float damping = 1;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }
    void Start()
    {

        if (player == null)
        {
            print(name + ": No tengo el jugador");
        }
        else {
            offset = player.transform.position - transform.position;
        }
    }

    private void LateUpdate()
    {
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = player.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = player.transform.position - (rotation * offset);

        transform.LookAt(player.transform);
    }
}
