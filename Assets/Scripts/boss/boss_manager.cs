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
    public Image blinker;

    public InputField variable_input;//the "console"

    public int[] text_ids; //Ids that are just text to move forward the scene
    public int[] help_ids; //Ids that are hints for the player
    public int[] trigger_ids; //Ids where input field should be seen and used
    public string[] solutions; //Strings of solutions
    public int[] anim_ids; //Ids where animation should play

    public GameObject boss; //Boss character reference 
    public GameObject main; //player character reference
    public GameObject binary; //Binary character reference

    public Vector3[] main_positions_to_animate; //Positions where the player is headed in animation
    public float[] time_to_animate; //Time needed for animations to be complete

    public int MAX_LINKS = 3; //Number of max tries that player gets for each input

    private int links; //lifes or tries that player has
    private float time; //Reference time to increment
    private int current_text;
    private int current_question;
    private int current_animation;

    private bool isAbleToContinue;
    private Coroutine blink_reference; //To stop the coroutine if needed

    void Start()
    {
        current_text = 0;
        current_question = 0;
        current_animation = 0;
        isAbleToContinue = false;
        links = MAX_LINKS;

        var submiter = new InputField.SubmitEvent();
        submiter.AddListener(SubmitTextToConsole);
        variable_input.onEndEdit = submiter;

        nextText();
        blink_reference = StartCoroutine(blinkArrow());

        //LeanTween.move(main, new Vector3(0,0,10.0f), 2.0f).setOnStart(() => startAnimation(0)).setOnComplete(() => stopAnimation(0));
        //LeanTween.move(binary, new Vector3(0, 0, 10.0f), 2.0f).setOnStart(() => startAnimation(0)).setOnComplete(() => stopAnimation(0));
    }

    void Update()
    {
        if (isAbleToContinue && !scene_manager.is_pause_menu_on)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
            {
                StopCoroutine(blink_reference);
                isAbleToContinue = false;
                current_text += 1;
                nextText();
            }
        }
    }

    /*
     * To avoid that the player skips a text
     */
    IEnumerator blinkArrow()
    {

        yield return new WaitForSeconds(0.5f);
        blinker.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        blinker.gameObject.SetActive(true);

        isAbleToContinue = true;

        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            blinker.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            blinker.gameObject.SetActive(true);
        }

    }

    void SubmitTextToConsole(string args0)
    {
        switch (current_question)
        {
            case 0:
                string[] solution = solutions[current_question].Split('/');
                if (args0.Contains(solution[0]) && (args0.Contains(solution[1]) || args0.Contains(solution[2])))
                {
                    current_text += 1;
                    nextText();
                    variable_input.gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    /*
     * Next text in the list 
     */
    void nextText()
    {
        if (text_ids[current_text] == trigger_ids[current_question])
        {
            variable_input.gameObject.SetActive(true);
        }
        else if (text_ids[current_text] == anim_ids[current_animation])
        {
            playAnimations(current_animation);
        }
    }

    /*
     * Pick the animations that the stage require 
     */
    void playAnimations(int id)
    {
        if (stage == BOSS_STAGE.SPAM)
        {
            pickSpamStageAnimations(id);
        }
        else if (stage == BOSS_STAGE.MISS)
        {

        }
        else if (stage == BOSS_STAGE.VIRUS)
        {

        }
    }

    //SPAM stages
    void pickSpamStageAnimations(int id)
    {
        switch (id)
        {
            case 0:
                //Makes SPAM laugh
                LeanTween.move(boss, boss.transform.position, 2.0f).setOnStart(() => startAnimation(1)).setOnComplete(() => stopAnimation(1));
                break;
            case 1:
                //Makes SPAM laugh
                LeanTween.move(boss, boss.transform.position, 2.0f).setOnStart(() => startAnimation(1)).setOnComplete(() => stopAnimation(1));
                break;
            case 2:
                //Move main character and binary to certain position
                break;
            default:
                Debug.LogError("There is a problem at pickingSpamStageAnimations(id) at boss_manager");
                break;
        }
    }

    
    void startAnimation(int id)
    {
        switch (id)
        {
            case 0:
                binary.GetComponent<Animator>().SetBool("walk", true);
                main.GetComponent<Animator>().SetBool("moving", true);
                break;
            case 1:
                boss.GetComponent<Animator>().SetBool("laugh", true);
                break;
            default:
                Debug.LogError("There is a problem at startAnimation(id) at boss_manager");
                break;
        }
    }

    void stopAnimation(int id)
    {
        switch (id)
        {
            case 0:
                binary.GetComponent<Animator>().SetBool("walk", false);
                main.GetComponent<Animator>().SetBool("moving", false);
                break;
            case 1:
                boss.GetComponent<Animator>().SetBool("laugh", false);
                break;
            default:
                Debug.LogError("There is a problem at stopAnimation(id) at boss_manager");
                break;
        }
    }


}
