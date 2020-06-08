using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Auxiliar class to throw spheres in boss stages 
 */
public class virus_special : MonoBehaviour
{
    public GameObject left_sphere_guide;
    public GameObject right_sphere_guide;

    public GameObject blue_sphere;
    public GameObject red_sphere;

    public void launchBall(int i) {
        GameObject temp_ball;
        if (i % 2 != 0)
        {
            temp_ball = Instantiate(red_sphere, right_sphere_guide.transform.position, Quaternion.identity);
            LeanTween.move(temp_ball, right_sphere_guide.transform.position+new Vector3(0,0,-100),1.0f);
        }
        else {
            temp_ball = Instantiate(blue_sphere, left_sphere_guide.transform.position, Quaternion.identity);
            LeanTween.move(temp_ball, left_sphere_guide.transform.position + new Vector3(0, 0, -100), 1.0f);
        }
    }
}
