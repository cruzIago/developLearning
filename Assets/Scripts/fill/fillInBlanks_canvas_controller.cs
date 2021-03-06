﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/**
 * Used to controll UI console for kids to see how they are filling it
 */
public class fillInBlanks_canvas_controller : MonoBehaviour
{
    public player_controller main;
    public Block.kinds[] game_solution; // To modify on editor for each minigame. It stablish how blocks should be stored to complete the level

    private List<Block> user_solution; // The array that the user will fill throwing cubes to the trigger zone

    public Text[] blanks_to_fill; // Text where user will see the changes 

    public Block[] blocks_in_scene;

    public bool[] is_number_compared; // Used to know which array is going to be checked everytime. Size should be both arrays below combined
    public int[] numbers_to_compare; //Used when control blocks need something to compare. Array just in case it needs more than one comparison
    public string[] strings_to_compare;


    public InputField gui_fill_input; //For the user to make inputs to simulate code running

    public Text pressToContinue;

    public string filled_text; //To write on editor what will prompt at result

    public GameObject background; //To animate

    private Block.kinds last_kind_block; // To check last block checked
    private int mistakes; //To check what rating will have the player
    private float elapsed_time;

    //Review variables
    public review_manager reviewer;
    public bool is_review_stage;

    public Image background_texts;
    public Image background_console;
    
    private void Start()
    {
        elapsed_time = Time.time;
        user_solution = new List<Block>();
        last_kind_block = Block.kinds.DEFAULT;
        var submiter = new InputField.SubmitEvent();
        submiter.AddListener(SubmitInputVariable);
        gui_fill_input.onEndEdit = submiter;
        checkStyle();
    }

    void checkStyle()
    {
        if (GameObject.Find("game_manager"))
        {
            GameObject.Find("game_manager").GetComponent<game_manager>().changeStyle(background_texts, background_console);
        }
    }
    private void Update()
    {
    }

    //Reset blocks to initial position
    private void blocks_to_default()
    {
        foreach (Block b in blocks_in_scene)
        {
            b.reset_position();
            b.gameObject.SetActive(true);
        }
    }

    //Fill the blanks with underscore again
    private void blanks_to_default()
    {
        foreach (Text t in blanks_to_fill)
        {
            t.color = Color.black;
            t.text = "_______";
        }
    }

    // Check if the solution provided by the user is the same as the required
    private IEnumerator checkSolutions()
    {
        bool is_control_filled = false;
        bool is_loop_filled = false;
        int number_comp_index = 0;
        int string_comp_index = 0;
        int compared_index = 0;
        last_kind_block = Block.kinds.DEFAULT;
        //animator.SetBool("checking_conditions", true);
        Vector3 tmp_original_position = background.transform.localPosition;
        LeanTween.moveLocal(background.gameObject, Vector3.zero, 1.0f);
        LeanTween.scale(background.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
        yield return new WaitForSeconds(1.0f);
        bool isCorrect = true;

        for (int i = 0; i < user_solution.Count; i++)
        {
            if (user_solution[i].kind_of_block == game_solution[i])
            {
                blanks_to_fill[i].color = Color.green;
                //If input is found
                if (game_solution[i] == Block.kinds.INPUT)
                {
                    gui_fill_input.gameObject.SetActive(true);
                    gui_fill_input.ActivateInputField();
                    yield return new WaitUntil(() => !main.isInputBlocked);
                    main.isInputBlocked = true;
                    if (i - 1 >= 0)
                    {
                        user_solution[i - 1].GetComponent<Variable_block>().setVariableValue(gui_fill_input.text);
                    }
                    gui_fill_input.text = "";
                    gui_fill_input.gameObject.SetActive(false);
                    //if control block is found
                }
                else if (game_solution[i] == Block.kinds.CONTROL)
                {
                    is_control_filled = true;
                    if (user_solution[i].GetComponent<Control_block>().state == Control_block.Statement.IF)
                    {
                        if (is_number_compared[compared_index])
                        {
                            filled_text = user_solution[i].GetComponent<Control_block>().DoComparison(numbers_to_compare[number_comp_index]);
                            number_comp_index += 1;
                        }
                        else
                        {
                            filled_text = user_solution[i].GetComponent<Control_block>().DoComparison(strings_to_compare[string_comp_index]);
                            string_comp_index += 1;
                        }
                    }
                }
                //If loop block is found
                else if (game_solution[i] == Block.kinds.LOOP)
                {
                    if (user_solution[i].gameObject.GetComponent<Loop_block>().repetitions == -1)
                    {
                        bool is_good_number = false;
                        int number_to_loop;
                        while (!is_good_number)
                        {
                            gui_fill_input.gameObject.SetActive(true);
                            gui_fill_input.ActivateInputField();
                            yield return new WaitUntil(() => !main.isInputBlocked);
                            main.isInputBlocked = true;
                            if (int.TryParse(gui_fill_input.text, out number_to_loop))
                            {
                                user_solution[i].gameObject.GetComponent<Loop_block>().repetitions = number_to_loop;
                                gui_fill_input.text = "";
                                gui_fill_input.gameObject.SetActive(false);
                                is_good_number = true;
                            }
                            else {
                                gui_fill_input.text = "Solo números";
                            }
                            
                        }
                    }

                    if (i <= blanks_to_fill.Length - 1)
                    {
                        if (user_solution[i].gameObject.GetComponent<Loop_block>().state == Loop_block.Statement.FINITE)
                        {
                            blanks_to_fill[i].text += " " + user_solution[i].gameObject.GetComponent<Loop_block>().repetitions + " VECES{";
                        }
                        else if (user_solution[i].gameObject.GetComponent<Loop_block>().state == Loop_block.Statement.UNTIL)
                        {
                            if (user_solution[i].gameObject.GetComponent<Loop_block>().when == Loop_block.Until.EQUAL)
                            {
                                blanks_to_fill[i].text += " HASTA numero==" + user_solution[i].gameObject.GetComponent<Loop_block>().repetitions + "{";
                            }
                            else if (user_solution[i].gameObject.GetComponent<Loop_block>().when == Loop_block.Until.GREATER)
                            {
                                blanks_to_fill[i].text += " HASTA numero>" + user_solution[i].gameObject.GetComponent<Loop_block>().repetitions + "{";
                            }
                            else if (user_solution[i].gameObject.GetComponent<Loop_block>().when == Loop_block.Until.LESSER)
                            {
                                blanks_to_fill[i].text += " HASTA numero<" + user_solution[i].gameObject.GetComponent<Loop_block>().repetitions + "{";
                            }
                        }
                    }

                    is_loop_filled = true;
                    string temp_text = "";
                    for (int j = 0; j < user_solution[i].GetComponent<Loop_block>().repetitions; j++)
                    {
                        temp_text = temp_text + filled_text + j;
                        if (j < user_solution[i].GetComponent<Loop_block>().repetitions - 1)
                        {
                            temp_text = temp_text + "\n";
                        }
                    }
                    filled_text = temp_text;
                }
            }
            else
            {
                blanks_to_fill[i].color = Color.red;
                mistakes += 1;
                isCorrect = false;
                yield return new WaitForSeconds(1.0f);
                pressToContinue.gameObject.SetActive(true);
                yield return WaitForKeyPress(KeyCode.Space);
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
        if (isCorrect)
        {
            if (!is_loop_filled)
            {
                if (!is_control_filled)
                {
                    blanks_to_fill[blanks_to_fill.Length - 1].text = filled_text + user_solution[0].GetComponent<Variable_block>().getVariableValue();
                }
                else
                {
                    blanks_to_fill[blanks_to_fill.Length - 1].text = user_solution[0].GetComponent<Variable_block>().getVariableValue() + filled_text;
                }
            }
            else
            {
                blanks_to_fill[blanks_to_fill.Length - 1].text = filled_text;
            }
            yield return new WaitForSeconds(1.0f);
            pressToContinue.gameObject.SetActive(true);
            yield return WaitForKeyPress(KeyCode.Space);

            if (!is_review_stage)
            {
                int stars = 0;
                if (mistakes <= 1)
                {
                    stars = 3;
                }
                else if (mistakes <= 4)
                {
                    stars = 2;
                }
                else
                {
                    stars = 1;
                }
                elapsed_time = Time.time - elapsed_time;
                scene_manager.checkEndScreen(stars, elapsed_time, mistakes);
            }
            else
            {
                reviewer.nextReview(mistakes);
            }

        }



        LeanTween.moveLocal(background.gameObject, tmp_original_position, 0.2f);
        LeanTween.scale(background.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.2f);
        blanks_to_default();
        user_solution.Clear();
        main.isInputBlocked = false;

    }

    /*
     * Waits for the player to press space to continue playing after examination
     */
    private IEnumerator WaitForKeyPress(KeyCode key)
    {
        bool isPressed = false;
        while (!isPressed)
        {
            if (Input.GetKeyDown(key))
            {
                isPressed = true;
                pressToContinue.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    /*
     * Waits for the player to input a variable value
     */
    void SubmitInputVariable(string args0)
    {
        main.isInputBlocked = false;
    }

    // Checks which blocks enters on the box and how to fill the GUI
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag.Equals("item")
            && user_solution.Count < game_solution.Length)
        {

            user_solution.Add(other.GetComponent<Block>());
            if (other.gameObject.GetComponent<Block>().kind_of_block == Block.kinds.VARIABLE)
            {

                if (last_kind_block == Block.kinds.PRINT)
                {
                    blanks_to_fill[user_solution.Count - 1].text = other.gameObject.GetComponent<Variable_block>().getVariableName();
                }
                else if (last_kind_block == Block.kinds.CONTROL)
                {
                    user_solution[user_solution.Count - 2].GetComponent<Control_block>().SetVariableToCompare(other.gameObject.GetComponent<Variable_block>());
                    blanks_to_fill[user_solution.Count - 1].text = other.gameObject.GetComponent<Variable_block>().getVariableName();
                }
                else
                {
                    blanks_to_fill[user_solution.Count - 1].text = "VAR";
                }

            }
            else if (other.gameObject.GetComponent<Block>().kind_of_block == Block.kinds.INPUT)
            {

                blanks_to_fill[user_solution.Count - 1].text = "ENTRADA()";
            }
            else if (other.gameObject.GetComponent<Block>().kind_of_block == Block.kinds.PRINT)
            {

                blanks_to_fill[user_solution.Count - 1].text = "SALIDA";
            }
            else if (other.gameObject.GetComponent<Block>().kind_of_block == Block.kinds.CONTROL)
            {

                if (other.gameObject.GetComponent<Control_block>().state == Control_block.Statement.IF)
                {
                    blanks_to_fill[user_solution.Count - 1].text = "SI ";
                }
                else if (other.gameObject.GetComponent<Control_block>().state == Control_block.Statement.ELSEIF)
                {
                    blanks_to_fill[user_solution.Count - 1].text = "SI NO, SI ";
                }
                else
                {
                    blanks_to_fill[user_solution.Count - 1].text = "SI NO ";
                }
            }
            else if (other.gameObject.GetComponent<Block>().kind_of_block == Block.kinds.LOOP)
            {
                blanks_to_fill[user_solution.Count - 1].text = "REPETIR";
            }
            else if (other.gameObject.GetComponent<Block>().kind_of_block == Block.kinds.RESET)
            {
                last_kind_block = Block.kinds.DEFAULT;
                user_solution.Clear();
                blanks_to_default();
                blocks_to_default();
            }
            else
            {
                Debug.Log("ERROR ON CANVAS CONTROLLER, NO KIND OF ITEM ALLOWED");
            }

            last_kind_block = other.gameObject.GetComponent<Block>().kind_of_block;
            other.gameObject.GetComponent<Block>().setCollisions(true);
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            other.gameObject.GetComponent<Block>().reset_position();

            if (other.gameObject.GetComponent<Block>().player.GetComponent<player_controller>().isItemHeld)
            {
                other.gameObject.GetComponent<Block>().pickDown();
                other.gameObject.GetComponent<Block>().reset_position();
            }

            if (user_solution.Count == game_solution.Length)
            {
                main.isInputBlocked = true;
                StartCoroutine(checkSolutions());
            }
        }
    }
}
