using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class review_manager : MonoBehaviour
{
    public GameObject[] stages; //Group stages in roder
    public GameObject[] respawn_points; //Points where the player will respawn at start of stage

    public player_controller main;

    private int actual_stage; //the stage the player is

    private int fails;
    private float elapsed_time;

    public void Start()
    {
        elapsed_time = Time.time;
        actual_stage = 0;
        stages[actual_stage].SetActive(true);
        main.transform.position=respawn_points[actual_stage].transform.position;
    }

    public void nextReview(int mistakes) {
        fails += mistakes;
        if (actual_stage < stages.Length-1)
        {
            StartCoroutine(changeStage());/*
            stages[actual_stage].SetActive(false);
            actual_stage += 1;
            main.transform.position = respawn_points[actual_stage].transform.position;
            stages[actual_stage].SetActive(true);*/
        }
        else {
            int stars = 0;
            if (mistakes <= 5)
            {
                stars = 3;
            }
            else if (mistakes <= 8)
            {
                stars = 2;
            }
            else {
                stars = 1;
            }
            elapsed_time = Time.time - elapsed_time;
           scene_manager.checkEndScreen(stars,elapsed_time,fails);
        }
    }
    IEnumerator changeStage() {
        yield return new WaitForSeconds(1.5f);
        stages[actual_stage].SetActive(false);
        actual_stage += 1;
        main.transform.position = respawn_points[actual_stage].transform.position;
        stages[actual_stage].SetActive(true);
    }
}

