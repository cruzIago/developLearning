using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boss_manager : MonoBehaviour
{
    public enum BOSS_STAGE { SPAM, MISS, VIRUS };
    public BOSS_STAGE stage;
    public Text texts; //UI Text
    public Image boss_image;
    public Image binary_image;

    public InputField variable_input;//the "console"

    public int[] text_ids; //Ids that are just text to move forward the scene
    public int[] help_ids; //Ids that are hints for the player
    public int[] trigger_ids; //Ids where input field should be seen and used
    public int[] anim_ids; //Ids where animation should play


    public GameObject boss; //Boss character reference 
    public GameObject main; //player character reference
    public Vector3[] main_positions_to_animate; //Positions where the player is headed in animation
    public float[] time_to_animate; //Time needed for animations to be complete
    
    public int MAX_LINKS = 3; //Number of max tries that player gets for each input

    private int links; //lifes or tries that player has
    private float time; //Reference time to increment


    void Start()
    {
        links = MAX_LINKS;
        var submiter = new InputField.SubmitEvent();
        submiter.AddListener(SubmitTextToConsole);
        variable_input.onEndEdit = submiter;
       // LeanTween.move(main, new Vector3(0,0,10.0f), 2.0f).setOnStart(() => startAnimation(0)).setOnComplete(() => stopAnimation(0));
    }
    void Update()
    {
    }

    void SubmitTextToConsole(string args0) {

    }

    /*
     * Pick the animations that the stage require 
     */
    void playAnimations(int id) {
        if (stage == BOSS_STAGE.SPAM)
        {
            pickSpamStageAnimations(id);
        }
        else if (stage == BOSS_STAGE.MISS)
        {

        }
        else if (stage == BOSS_STAGE.VIRUS) {

        }
    }

    //SPAM stages
    void pickSpamStageAnimations(int id) {
        if (id == 0)
        {
            LeanTween.move(main, main_positions_to_animate[id], time_to_animate[id]).setOnStart(()=>startAnimation(id)).setOnComplete(()=>stopAnimation(id));
        }
        else if (id == 1) {

        }
    }


    void startAnimation(int id)
    {

        main.GetComponent<Animator>().SetBool("moving", true);
    }
    void stopAnimation(int id)
    {

        main.GetComponent<Animator>().SetBool("moving", false);
    }


}
