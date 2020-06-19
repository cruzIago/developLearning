using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Usual kind of camera, nothing really outstanding 
 */
public class main_camera : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    [SerializeField]
    private Transform to_see_through;

    [SerializeField]
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
            print(name + ": player not found");
        }
        else
        {
            offset = player.transform.position - transform.position;
        }
    }

    private void LateUpdate()
    {
        CameraFollow();
    }


    /*
     * This camera follow the target/character and smooth it when turning 
     * https://www.youtube.com/watch?v=wWyx7_cIxP8
     */
    private void CameraFollow()
    {
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = player.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = player.transform.position - (rotation * offset);

        transform.LookAt(player.transform);
    }

    private void SeeThrough()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 4.5f))
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                to_see_through = hit.transform;
                to_see_through.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }
        else {
            to_see_through.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
}
