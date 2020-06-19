using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Gives player the ability to change variable names. May be used on sandbox if done.
 */
public class player_canvas_controll : MonoBehaviour
{
    public InputField variable_input;
    public player_controller player;

    private bool givenName; // to check if the item was given a name once picked up

    void Start()
    {
        //variable_input.gameObject.SetActive(false);
        var submiter = new InputField.SubmitEvent();
        submiter.AddListener(SubmitVariableName);
        variable_input.onEndEdit = submiter;
        player = gameObject.GetComponent<player_controller>();
        givenName = false;
    }

    void Update()
    {

        if (player.isItemHeld)
        {
            if (player.held_item != null)
            {
                if (player.held_item.kind_of_block == Block.kinds.VARIABLE
                    && player.held_item.GetComponent<Variable_block>().variable_written == null
                    && !givenName)
                {
                    variable_input.gameObject.SetActive(true);
                    variable_input.ActivateInputField();
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                    variable_input.gameObject.SetActive(false);
                }
            }

        }
        else
        {
            givenName = false;
            variable_input.gameObject.SetActive(false);
        }
    }

    void SubmitVariableName(string args0)
    {
        if (player.isItemHeld
            && player.held_item != null)
        {
            player.held_item.GetComponent<Variable_block>().setVariableName(variable_input.text);
            givenName = true;
            Time.timeScale = 1;
        }
    }
}
