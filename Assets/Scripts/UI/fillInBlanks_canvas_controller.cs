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
    public int[] game_solution; // To modify on editor for each minigame. It stablish how blocks should be stored to complete the level

    private List<Block> user_solution; // The array that the user will fill throwing cubes to the trigger zone

    public Text[] blanks_to_fill; // Text where user will see the changes 

    public Block[] blocks_in_scene;

    public Animator animator; //Animator to enlarge the console screen and give feedback to the user

    private Block.kinds last_kind_block; // To check last block checked

    public InputField gui_fill_input; //For the user to make inputs to simulate code running

    public Text[] result_text;

    private void Start()
    {
        user_solution = new List<Block>();
        last_kind_block = Block.kinds.DEFAULT;
        var submiter = new InputField.SubmitEvent();
        submiter.AddListener(SubmitInputVariable);
        gui_fill_input.onEndEdit = submiter;
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

        last_kind_block = Block.kinds.DEFAULT;
        Time.timeScale = 0;
        animator.SetBool("checking_conditions", true);
        yield return new WaitForSeconds(1.0f);
        bool isCorrect = true;
        for (int i = 0; i < user_solution.Count; i++)
        {
            if ((int)user_solution[i].kind_of_block == game_solution[i])
            {
                blanks_to_fill[i].color = Color.green;
                
                if (game_solution[i] == 0)
                {
                    gui_fill_input.gameObject.SetActive(true);
                    gui_fill_input.ActivateInputField();
                    yield return new WaitUntil(() => Time.timeScale == 1);
                    user_solution[i - 1].GetComponent<Variable_block>().setVariableValue(gui_fill_input.text);
                    gui_fill_input.text = "";
                    gui_fill_input.gameObject.SetActive(false);
                }
            }
            else
            {
                blanks_to_fill[i].color = Color.red;
                print("Fallo");
                isCorrect = false;
                yield return new WaitForSeconds(1.0f);
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
        if (isCorrect)
        {
            foreach (Text t in result_text) {

            }
            print("Acierto");
        }
        animator.SetBool("checking_conditions", false);
        blanks_to_default();
        user_solution.Clear();
        Time.timeScale = 1;
        print("done");

    }

    void SubmitInputVariable(string args0)
    {
        print(gui_fill_input.text);
        Time.timeScale = 1;

    }

    // Checks which blocks enters on the box
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
                else
                {
                    blanks_to_fill[user_solution.Count - 1].text = "var";
                }

            }
            else if (other.gameObject.GetComponent<Block>().kind_of_block == Block.kinds.INPUT)
            {
                
                blanks_to_fill[user_solution.Count - 1].text = "input()";
            }
            else if (other.gameObject.GetComponent<Block>().kind_of_block == Block.kinds.PRINT)
            {
                
                blanks_to_fill[user_solution.Count - 1].text = "print";
            }
            else
            {
                print("ERROR ON CANVAS CONTROLLER, NO KIND OF ITEM ALLOWED");
            }

            last_kind_block = other.gameObject.GetComponent<Block>().kind_of_block;
            other.gameObject.GetComponent<Block>().setCollisions(true);
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            if (other.gameObject.GetComponent<Block>().player.GetComponent<player_controller>().isItemHeld)
            {
                other.gameObject.GetComponent<Block>().pickDown();
            }
            other.gameObject.GetComponent<Block>().reset_position();

            print(user_solution.Count);
            if (user_solution.Count == game_solution.Length)
            {
                StartCoroutine(checkSolutions());
            }
        }
    }
}
