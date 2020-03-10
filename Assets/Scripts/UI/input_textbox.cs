using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class input_textbox : MonoBehaviour
{

    public player_controller player;
    public boss boss;
    private InputField input;
    private string currentCorrectAnswer;


    private void Awake()
    {
        input = gameObject.GetComponent<InputField>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentCorrectAnswer = "be";//TODO
        var submitter = new InputField.SubmitEvent();
        submitter.AddListener(checkIfCorrect);
        input.onEndEdit = submitter;

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
            gameObject.GetComponent<InputField>().DeactivateInputField();
        }
    }

    //Check if the answer is correct. TODO->A good checking of the input
     void checkIfCorrect(string args0)
    {        
        gameObject.GetComponent<InputField>().DeactivateInputField();
        if (input.text.Equals(currentCorrectAnswer))
        {
            boss.lifeLoss();
        } else
        {
            boss.attack();
        }
    }

    public void setCorrectAnswer(string answer)
    {
        currentCorrectAnswer = answer;
    }
}
