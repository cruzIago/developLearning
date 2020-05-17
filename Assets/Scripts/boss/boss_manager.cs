using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boss_manager : MonoBehaviour
{
    public enum BOSS_STAGE { SPAM, MISS, VIRUS };
    public BOSS_STAGE stage;
    public Text script_text; //Script text
    public Text[] console_texts;

    public Image boss_image;
    public Image binary_image;
    public Image blinker;

    public InputField variable_input;//the "console"

    public int[] console_ids; //Ids where console should be shown
    public int[] text_ids; //Ids that are just text to move forward the scene
    public int[] trigger_ids; //Ids where input field should be seen and used
    public string[] solutions; //Strings of solutions
    public int[] anim_ids; //Ids where animation should play

    //https://wiki.unity3d.com/index.php/SerializableDictionary
    [SerializeField]
    public IntIntDictionary int_int_animation_rel = IntIntDictionary.New<IntIntDictionary>();
    public Dictionary<int, int> anim_id //This stablish a connection between which text should come with an animation and which animation
    {
        get { return int_int_animation_rel.dictionary; }
    }

    public GameObject boss; //Boss character reference 
    public GameObject main; //player character reference
    public GameObject binary; //Binary character reference

    public Vector3[] positions_to_animate; //Positions where the player is headed in animation
    public float[] time_to_animate; //Time needed for animations to be complete

    public int MAX_LINKS = 3; //Number of max tries that player gets for each input

    private int links; //lifes or tries that player has
    private int mistakes;
    private float elapsed_time;
    private int current_text;
    private int current_question;
    private int current_animation;
    private int current_console;

    private bool isAbleToContinue;
    private bool isGameOver;
    private bool isAnswering;
    private Coroutine blink_reference; //To stop the coroutine if needed

    void Start()
    {
        elapsed_time = Time.time;
        current_text = 0;
        current_question = 0;
        current_animation = 0;
        current_console = 0;
        mistakes = 0;
        isAbleToContinue = false;
        isGameOver = false;
        links = MAX_LINKS;

        var submiter = new InputField.SubmitEvent();
        submiter.AddListener(SubmitTextToConsole);
        variable_input.onEndEdit = submiter;
        variable_input.gameObject.SetActive(false);
        nextText();
        print("nuevacontraseña".Contains("nuevacontra"));
    }

    void Update()
    {
        if (!isAnswering && isAbleToContinue && !scene_manager.is_pause_menu_on)
        {
            if (!isGameOver && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)))
            {
                if (blink_reference != null)
                {
                    StopCoroutine(blink_reference);
                }
                isAbleToContinue = false;
                current_text += 1;
                nextText();
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
            {
                isAbleToContinue = false;
                int stars = 0;
                if (mistakes <= 4)
                {
                    stars = 3;
                }
                else if (mistakes <= 7)
                {
                    stars = 2;
                }
                else if (mistakes > 100)
                {
                    stars = 0;
                }
                else
                {
                    stars = 1;
                }
                elapsed_time = Time.time - elapsed_time;
                scene_manager.checkEndScreen(stars, elapsed_time, mistakes);
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

    /*
     * To evaluate each answer to the problem
     */
    public void SubmitTextToConsole(string args0)
    {
        string[] solution = solutions[current_question].Split('/');

        if (args0.ToUpper().Contains(solution[0].ToUpper()) &&
            (args0.ToUpper().Contains(solution[1].ToUpper()) || args0.ToUpper().Contains(solution[2].ToUpper())))
        {
            print("correcto");
            current_question += 1;
            current_text += 1;
            links = MAX_LINKS;
            variable_input.gameObject.SetActive(false);
            isAnswering = false;
            nextText();
        }
        else
        {
            print("incorrecto");
            links -= 1;
            mistakes += 1;
            if (links == 2)
            {
                //To change portraits
                string[] retrieved_text = game_manager.getStringFromLang(text_ids[current_text] + 1).Split('#');
                if (retrieved_text[1].Contains("B"))
                {
                    binary_image.gameObject.SetActive(true);
                    boss_image.gameObject.SetActive(false);
                }
                else
                {
                    binary_image.gameObject.SetActive(false);
                    boss_image.gameObject.SetActive(true);
                }
                script_text.text = retrieved_text[0];
            }
            else if (links == 1)
            {//To change portraits
                string[] retrieved_text = game_manager.getStringFromLang(text_ids[current_text] + 2).Split('#');
                if (retrieved_text[1].Contains("B"))
                {
                    binary_image.gameObject.SetActive(true);
                    boss_image.gameObject.SetActive(false);
                }
                else
                {
                    binary_image.gameObject.SetActive(false);
                    boss_image.gameObject.SetActive(true);
                }
                script_text.text = retrieved_text[0];
            }
            else
            {
                //Lose state
                isAnswering = false;
                mistakes = 1000;
                current_text = text_ids.Length - 1;
                isGameOver = true;

                string[] retrieved_text = game_manager.getStringFromLang(text_ids[current_text]+1).Split('#');

                script_text.text = retrieved_text[0];

                //To change portraits
                if (retrieved_text[1].Contains("B"))
                {
                    binary_image.gameObject.SetActive(true);
                    boss_image.gameObject.SetActive(false);
                }
                else
                {
                    binary_image.gameObject.SetActive(false);
                    boss_image.gameObject.SetActive(true);
                }
                blink_reference = StartCoroutine(blinkArrow());
                variable_input.gameObject.SetActive(false);
            }
        }

    }
    
    /*
     * Next text in the list 
     */
    void nextText()
    {
        if (current_console < console_ids.Length && text_ids[current_text] == console_ids[current_console])
        {
            print("son el mismo");
            console_texts[current_console].gameObject.SetActive(true);
            current_console += 1;
        }

        string[] retrieved_text = game_manager.getStringFromLang(text_ids[current_text]).Split('#');

        script_text.text = retrieved_text[0];

        //To change portraits
        if (retrieved_text[1].Contains("B"))
        {
            binary_image.gameObject.SetActive(true);
            boss_image.gameObject.SetActive(false);
        }
        else
        {
            binary_image.gameObject.SetActive(false);
            boss_image.gameObject.SetActive(true);
        }

        if (current_text != text_ids.Length - 1)
        {
            if (current_question < trigger_ids.Length && text_ids[current_text] == trigger_ids[current_question])
            {
                isAnswering = true;
                variable_input.gameObject.SetActive(true);
                variable_input.ActivateInputField();
                if(current_animation < anim_ids.Length && text_ids[current_text] == anim_ids[current_animation]) {
                    playAnimations(anim_id[anim_ids[current_animation]]);
                }
            }
            else if (current_animation < anim_ids.Length && text_ids[current_text] == anim_ids[current_animation])
            {
                playAnimations(anim_id[anim_ids[current_animation]]);
            }
            else
            {
                blink_reference = StartCoroutine(blinkArrow());
            }
        }
        else
        {
            isGameOver = true;
            StartCoroutine(blinkArrow());
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
                LeanTween.scale(boss, positions_to_animate[current_animation],
                    time_to_animate[current_animation]).setOnStart(() => startAnimation(id)).setOnComplete(() => stopAnimation(id));
                break;
            case 1:
                //Makes SPAM moves towards computer
                LeanTween.rotate(boss, new Vector3(0, 265.0f, 0), 0.5f);
                LeanTween.move(boss, positions_to_animate[current_animation],
                    time_to_animate[current_animation]).setOnStart(() => startAnimation(id)).setOnComplete(() => stopAnimation(id));
                break;
            case 2:
                //Makes player and binary moves towards computer
                LeanTween.rotate(main, new Vector3(0, -45.0f, 0), 0.5f);
                LeanTween.move(main, positions_to_animate[current_animation],
                    time_to_animate[current_animation]).setOnStart(() => startAnimation(id)).setOnComplete(() => stopAnimation(id));
                break;
            case 3:
                //Makes binary bark
                LeanTween.scale(binary, positions_to_animate[current_animation],
                    time_to_animate[current_animation]).setOnStart(() => startAnimation(id)).setOnComplete(() => stopAnimation(id));
                break;
            case 4:
                //Makes binary's head tilt
                LeanTween.scale(binary, positions_to_animate[current_animation],
                    time_to_animate[current_animation]).setOnStart(() => startAnimation(id)).setOnComplete(() => stopAnimation(id));
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
                boss.GetComponent<Animator>().SetBool("laugh", true);
                break;
            case 1:
                boss.GetComponent<Animator>().SetBool("walk", true);
                break;
            case 2:
                binary.GetComponent<Animator>().SetBool("walk", true);
                main.GetComponent<Animator>().SetBool("moving", true);
                break;
            case 3:
                binary.GetComponent<Animator>().SetBool("bark", true);
                break;
            case 4:
                binary.GetComponent<Animator>().SetBool("tilt", true);
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
                boss.GetComponent<Animator>().SetBool("laugh", false);
                break;
            case 1:
                boss.GetComponent<Animator>().SetBool("walk", false);
                break;
            case 2:
                binary.GetComponent<Animator>().SetBool("walk", false);
                main.GetComponent<Animator>().SetBool("moving", false);
                break;
            case 3:
                binary.GetComponent<Animator>().SetBool("bark", false);
                break;
            case 4:
                binary.GetComponent<Animator>().SetBool("tilt", false);
                break;
            default:
                Debug.LogError("There is a problem at stopAnimation(id) at boss_manager");
                break;
        }/*
        current_text += 1;
        nextText();*/
        current_animation += 1;
        blink_reference = StartCoroutine(blinkArrow());
    }


}
