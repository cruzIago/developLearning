using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class input_textbox : MonoBehaviour
{

    public player_controller player;

    // Start is called before the first frame update
    void Start()
    {
        setTextBoxActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Shows or hides textBox and blocks/unblocks player controller
    public void setTextBoxActive(bool active)
    {
        gameObject.SetActive(active);
        if (active)
        {
            player.isInputBlocked = true;
            gameObject.GetComponent<InputField>().ActivateInputField();
        } else
        {
            player.isInputBlocked = false;
        }
    }
}
