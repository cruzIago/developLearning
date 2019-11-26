using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_canvas_controll : MonoBehaviour
{
    public InputField variable_input;
    public player_controller player;
    void Start()
    {
        variable_input.gameObject.SetActive(false);
        var submiter = new InputField.SubmitEvent();
        submiter.AddListener(SubmitVariableName);
        variable_input.onEndEdit = submiter;
        player = gameObject.GetComponent<player_controller>();
    }

    void Update()
    {

        if (player.isItemHeld)
        {
            if (player.held_item != null)
            {
                if (player.held_item.kind_of_block == Block.kinds.INPUT)
                {
                    variable_input.gameObject.SetActive(true);
                    print(variable_input.onEndEdit);
                }
            }

        }
        else
        {
            variable_input.gameObject.SetActive(false);
        }
    }

    void SubmitVariableName(string args0) {
        print(variable_input.text);
    }
}
