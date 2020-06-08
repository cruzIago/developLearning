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

    public Image background_console;

    public int[] console_ids; //Ids where console should be shown
    public int[] text_ids; //Ids that are just text to move forward the scene
    public int[] trigger_ids; //Ids where input field should be seen and used
    public string[] solutions; //Strings of solutions
    public int[] anim_ids; //Ids where animation should play

    //https://wiki.unity3d.com/index.php/SerializableDictionary
    [SerializeField]
    public IntIntDictionary int_int_animation_rel = IntIntDictionary.New<IntIntDictionary>();
    public Dictionary<int, int> anim_play_id //This stablish a connection between which text should come with an animation and which animation
    {
        get { return int_int_animation_rel.dictionary; }
    }

    public GameObject boss; //Boss character reference 
    public GameObject main; //player character reference
    public GameObject binary; //Binary character reference
    public GameObject board; //Board object reference

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

    private int current_position;
    private int current_time;

    private bool is_special_launch_finished = false; //used in multiple launch of VIRUS

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

                string[] retrieved_text = game_manager.getStringFromLang(text_ids[current_text] + 1).Split('#');

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
                if (current_animation < anim_ids.Length && text_ids[current_text] == anim_ids[current_animation])
                {
                    playAnimations(anim_play_id[anim_ids[current_animation]]);
                }
            }
            else if (current_animation < anim_ids.Length && text_ids[current_text] == anim_ids[current_animation])
            {
                playAnimations(anim_play_id[anim_ids[current_animation]]);
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
            pickMissStageAnimations(id);
        }
        else if (stage == BOSS_STAGE.VIRUS)
        {
            pickVirusStageAnimations(id);
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

    //MISS stages
    void pickMissStageAnimations(int id)
    {
        switch (id)
        {
            case 6:
                //Makes MISS "run" to the end point
                LeanTween.rotate(boss, new Vector3(0, 0, 0), time_to_animate[current_time]);
                current_time += 1;

                void auxiliarMissEnd()
                {
                    LeanTween.rotate(boss, new Vector3(0, 90.0f, 0), time_to_animate[current_time]);
                    current_time += 1;
                    LeanTween.move(boss, positions_to_animate[current_position], time_to_animate[current_time]);
                    current_time += 1;
                    current_position += 1;
                    LeanTween.rotate(boss, new Vector3(0, 270.0f, 0), time_to_animate[current_time]).setOnComplete(() => stopAnimation(id));
                    current_time += 1;
                }

                LeanTween.move(boss, positions_to_animate[current_position],
                    time_to_animate[current_time]).setOnStart(() => startAnimation(id)).setOnComplete(auxiliarMissEnd);
                current_time += 1;
                current_position += 1;
                break;
            case 7:
                //Makes MISS walks backwards
                if (boss.GetComponent<miss_special>() != null)
                {
                    boss.GetComponent<miss_special>().DropTerminal();
                }

                LeanTween.move(boss, positions_to_animate[current_position],
                   time_to_animate[current_time]).setOnStart(() => startAnimation(id)).setOnComplete(() => stopAnimation(id));
                current_time += 1;
                current_position += 1;
                break;
            case 8:
                //Makes MISS do a sigh
                LeanTween.scale(boss, positions_to_animate[current_position],
                    time_to_animate[current_time]).setOnStart(() => startAnimation(id)).setOnComplete(() => stopAnimation(id));
                current_time += 1;
                current_position += 1;
                break;
            case 2:
                //Makes player and binary moves towards fork
                void playerAux()
                {
                    LeanTween.rotate(main, new Vector3(0, 90.0f, 0), time_to_animate[current_time]);
                    current_time += 1;
                    LeanTween.move(main, positions_to_animate[current_position], time_to_animate[current_time]).setOnComplete(() => stopAnimation(id));
                    current_time += 1;
                    current_position += 1;
                }

                LeanTween.move(main, positions_to_animate[current_position],
                    time_to_animate[current_time]).setOnStart(() => startAnimation(id)).setOnComplete(playerAux);
                current_time += 1;
                current_position += 1;
                break;
            case 3:
                //Makes binary bark
                LeanTween.scale(binary, positions_to_animate[current_position],
                    time_to_animate[current_time]).setOnStart(() => startAnimation(id)).setOnComplete(() => stopAnimation(id));
                current_time += 1;
                current_position += 1;
                break;
            case 4:
                //Makes binary's head tilt
                LeanTween.scale(binary, positions_to_animate[current_position],
                    time_to_animate[current_time]).setOnStart(() => startAnimation(id)).setOnComplete(() => stopAnimation(id));
                current_time += 1;
                current_position += 1;
                break;
            case 5:
                //Makes main character move towards left alley and cross
                LeanTween.rotate(main, new Vector3(0, 45.0f, 0), time_to_animate[current_time]);
                current_time += 1;

                void auxiliar_player()
                {
                    LeanTween.rotate(main, new Vector3(0, 90.0f, 0), time_to_animate[current_time]);
                    current_time += 1;
                    LeanTween.move(main, positions_to_animate[current_position], time_to_animate[current_time]).setOnComplete(() => stopAnimation(2));
                    current_time += 1;
                    current_position += 1;
                }

                LeanTween.move(main, positions_to_animate[current_position],
                    time_to_animate[current_time]).setOnStart(() => startAnimation(2)).setOnComplete(auxiliar_player);
                current_time += 1;
                current_position += 1;
                break;
            case -1:
                background_console.gameObject.SetActive(true);
                stopAnimation(id);
                break;
            default:
                Debug.LogError("There is a problem at pickingSpamStageAnimations(id) at boss_manager");
                break;
        }
    }

    //VIRUS stages
    void pickVirusStageAnimations(int id)
    {
        switch (id)
        {
            case 0:
                //Simple VIRUS walk
                void aux_3_0()
                {
                    stopAnimation(1);
                    nextAnim();
                }
                LeanTween.move(boss, positions_to_animate[current_position], time_to_animate[current_time])
                    .setOnStart(() => startAnimation(1)).setOnComplete(() => aux_3_0());
                current_position += 1;
                current_time += 1;
                break;
            case 1:
                //Makes binary bark
                void aux_3_1()
                {
                    stopAnimation(3);
                    nextAnim();
                }
                LeanTween.scale(binary, positions_to_animate[current_position],
                    time_to_animate[current_time]).setOnStart(() => startAnimation(3))
                    .setOnComplete(() => aux_3_1());
                current_time += 1;
                current_position += 1;
                break;
            case 2:
                //Virus rotate and fly
                void aux_3_2()
                {
                    stopAnimation(1);
                    nextAnim();
                }
                void rotateTowardsPlayerVirus()
                {
                    stopAnimation(8);
                    LeanTween.rotate(boss, positions_to_animate[current_position], time_to_animate[current_time])
                    .setOnStart(() => startAnimation(1)).setOnComplete(() => aux_3_2());
                    current_time += 1;
                    current_position += 1;
                }

                void moveTowardsPointVirus()
                {
                    stopAnimation(1);
                    LeanTween.move(boss, positions_to_animate[current_position], time_to_animate[current_time])
                    .setOnStart(() => startAnimation(8)).setOnComplete(rotateTowardsPlayerVirus);
                    current_time += 1;
                    current_position += 1;
                }

                LeanTween.rotate(boss, positions_to_animate[current_position], time_to_animate[current_time])
                    .setOnStart(() => startAnimation(1)).setOnComplete(moveTowardsPointVirus);
                current_time += 1;
                current_position += 1;
                break;
            case 3:
                void aux_3_3()
                {
                    stopAnimation(2);
                    nextAnim();
                }
                //Main rotate towards board and appear
                LeanTween.rotate(main, positions_to_animate[current_position], time_to_animate[current_time])
                   .setOnStart(() => startAnimation(2)).setOnComplete(() => aux_3_3());
                current_time += 1;
                current_position += 1;
                board.SetActive(true);
                break;
            case 4:
                //Main and board move towards virus, dismount and rotate
                void aux_3_4()
                {
                    stopAnimation(2);
                    nextAnim();
                }
                void dismountAndRotate()
                {
                    stopAnimation(9);
                    board.transform.parent = null;
                    LeanTween.rotate(main, positions_to_animate[current_position], time_to_animate[current_time]);
                    current_time += 1;
                    current_position += 1;

                    LeanTween.move(main, positions_to_animate[current_position], time_to_animate[current_time])
                        .setOnStart(() => startAnimation(2)).setOnComplete(() => aux_3_4());
                    current_time += 1;
                    current_position += 1;
                    board.SetActive(false);

                }

                void moveBoardTowardVirus()
                {
                    stopAnimation(2);
                    board.transform.parent = main.transform;
                    LeanTween.move(main, positions_to_animate[current_position], time_to_animate[current_time])
                        .setOnStart(() => startAnimation(9)).setOnComplete(dismountAndRotate);
                    current_time += 1;
                    current_position += 1;
                }

                LeanTween.move(main, positions_to_animate[current_position], time_to_animate[current_time])
                        .setOnStart(() => startAnimation(2)).setOnComplete(moveBoardTowardVirus);
                current_time += 1;
                current_position += 1;
                break;

            case 5:
                void aux_3_5()
                {
                    stopAnimation(0);
                    nextAnim();
                }
                //Virus laugh
                LeanTween.scale(boss, positions_to_animate[current_position], time_to_animate[current_time])
                    .setOnStart(() => startAnimation(0)).setOnComplete(() => aux_3_5());
                current_time += 1;
                current_position += 1;
                break;
            case 6:
                void aux_3_6()
                {
                    stopAnimation(2);
                    nextAnim();
                }

                void startMoveMainRight()
                {
                    startAnimation(10);
                    LeanTween.scale(boss, new Vector3(2, 2, 2), 0.25f).setOnComplete(() => boss.GetComponent<virus_special>().launchBall(0));
                    LeanTween.rotate(main, new Vector3(0, -90.0f, 0), 0.25f).setOnComplete(()=>stopAnimation(10));
                    LeanTween.move(main, new Vector3(161.0f, 0, -15), 0.25f)
                        .setOnStart(() => startAnimation(2)).setOnComplete(() => aux_3_6());
                }
                //Virus left launch
                LeanTween.scale(boss, positions_to_animate[current_position], time_to_animate[current_time])
                    .setOnStart(() => startMoveMainRight());

                current_time += 1;
                current_position += 1;
                break;
            case 7:
                void aux_3_7()
                {
                    stopAnimation(2);
                    LeanTween.rotate(main, new Vector3(0, 0, 0), 0.25f).setOnStart(()=>startAnimation(2));
                    LeanTween.move(main, new Vector3(160.0f, 0, -15), 0.25f).setOnComplete(() => stopAnimation(2));
                    background_console.gameObject.SetActive(true);
                    nextAnim();
                }
                void startMoveMainLeft()
                {
                    startAnimation(11);
                    LeanTween.scale(boss, new Vector3(2, 2, 2), 0.25f).setOnComplete(() => boss.GetComponent<virus_special>().launchBall(1));
                    LeanTween.rotate(main, new Vector3(0, 90.0f, 0), 0.25f).setOnComplete(() =>stopAnimation(11));
                    LeanTween.move(main, new Vector3(159.0f, 0, -15), 0.25f)
                        .setOnStart(() => startAnimation(2)).setOnComplete(() => aux_3_7());
                }
                //Virus right launch
                LeanTween.scale(boss, positions_to_animate[current_position], time_to_animate[current_time])
                 .setOnStart(() => startAnimation(11)).setOnComplete(() => startMoveMainLeft());

                current_time += 1;
                current_position += 1;
                break;
            case 8:
                //Virus multiple launch and character dodge
                StartCoroutine(specialLaunch());
                break;
            case 9:
                void aux_3_9()
                {
                    stopAnimation(2);
                    nextAnim();
                }
                void fallOffBehindVirus()
                {
                    stopAnimation(2);
                    LeanTween.move(main, positions_to_animate[current_position], time_to_animate[current_time])
                    .setOnStart(() => startAnimation(2)).setOnComplete(() => aux_3_9());
                    current_time += 1;
                    current_position += 1;
                }
                LeanTween.move(main, positions_to_animate[current_position], time_to_animate[current_time])
                    .setOnStart(() => startAnimation(2)).setOnComplete(() => fallOffBehindVirus());
                current_time += 1;
                current_position += 1;
                break;
            case 10:
                void aux_3_10()
                {
                    stopAnimation(8);
                    nextAnim();
                }
                LeanTween.move(boss, positions_to_animate[current_position], time_to_animate[current_time])
                    .setOnStart(() => startAnimation(8)).setOnComplete(() => aux_3_10());
                current_time += 1;
                current_position += 1;
                break;
            default:
                Debug.LogError("There is a problem at pickingSpamStageAnimations(id) at boss_manager");
                break;
        }
    }


    IEnumerator specialLaunch()
    {
        bool is_dodge_done = true;

        void startMoveMainLeft()
        {
            startAnimation(11);
            LeanTween.rotate(main, new Vector3(0, 90.0f, 0), 0.25f).setOnComplete(() => is_dodge_done = true);
            LeanTween.move(main, new Vector3(159.0f, 0, -15), 0.25f)
                .setOnStart(() => startAnimation(2)).setOnComplete(() => stopAnimation(2));
        }

        void startMoveMainRight()
        {
            startAnimation(10);
            LeanTween.rotate(main, new Vector3(0, -90.0f, 0), 0.25f).setOnComplete(() => is_dodge_done = true);
            LeanTween.move(main, new Vector3(161.0f, 0, -15), 0.25f)
                .setOnStart(() => startAnimation(2)).setOnComplete(() => stopAnimation(2));
        }

        void stopLaunchLeft()
        {
            stopAnimation(10);
            is_special_launch_finished = true;
        }
        void stopLaunchRight()
        {
            stopAnimation(11);
            is_special_launch_finished = true;
        }

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitUntil(() => is_dodge_done);
            is_dodge_done = false;
            if (i % 2 == 0)
            {
                LeanTween.scale(boss, new Vector3(2, 2, 2), 0.5f).
                    setOnStart(() => startMoveMainRight())
                    .setOnComplete(() => stopLaunchLeft());
            }
            else
            {
                LeanTween.scale(boss, new Vector3(2, 2, 2), 0.5f)
                    .setOnStart(() => startMoveMainLeft())
                    .setOnComplete(() => stopLaunchRight());
            }
            LeanTween.scale(boss, new Vector3(2, 2, 2), 0.25f).setOnComplete(() => boss.GetComponent<virus_special>().launchBall(i));
            yield return new WaitUntil(() => is_special_launch_finished);
            is_special_launch_finished = false;
        }
        LeanTween.rotate(main, new Vector3(0, 0, 0), 0.25f).setOnStart(() => startAnimation(2));
        LeanTween.move(main, new Vector3(160.0f, 0, -15), 0.25f).setOnComplete(() => stopAnimation(2));
        nextAnim();
        yield return null;
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
            case 5:
                boss.GetComponent<Animator>().SetBool("runwalk", true);
                break;
            case 6:
                boss.GetComponent<Animator>().SetBool("backwards", true);
                break;
            case 7:
                boss.GetComponent<Animator>().SetBool("sigh", true);
                break;
            case 8:
                boss.GetComponent<Animator>().SetBool("fly", true);
                break;
            case 9:
                main.GetComponent<Animator>().SetBool("pickingItem", true);
                break;
            case 10:
                boss.GetComponent<Animator>().SetBool("leftLaunch", true);
                break;
            case 11:
                boss.GetComponent<Animator>().SetBool("rightLaunch", true);
                break;
            case -1:
                //Just to have one that does nothing just in case is needed as the system needs an stopAnim and start to work
                break;
            default:
                Debug.LogError("There is a problem at startAnimation(id) at boss_manager or no numbers possible were selected");
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
            case 5:
                boss.GetComponent<Animator>().SetBool("runwalk", false);
                break;
            case 6:
                boss.GetComponent<Animator>().SetBool("backwards", false);
                break;
            case 7:
                boss.GetComponent<Animator>().SetBool("sigh", false);
                break;
            case 8:
                boss.GetComponent<Animator>().SetBool("fly", false);
                break;
            case 9:
                main.GetComponent<Animator>().SetBool("pickingItem", false);
                break;
            case 10:
                boss.GetComponent<Animator>().SetBool("leftLaunch", false);
                break;
            case 11:
                boss.GetComponent<Animator>().SetBool("rightLaunch", false);
                break;
            case -1:
                //Just to have one that does nothing just in case is needed as the system needs an stopAnim and start to work
                break;
            default:
                Debug.LogError("There is a problem at stopAnimation(id) at boss_manager or no numbers possible were selected");
                break;
        }
    }

    //To advance on the index and nexttext
    void nextAnim()
    {
        current_animation += 1;
        blink_reference = StartCoroutine(blinkArrow());
    }


}
