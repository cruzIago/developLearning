using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class input_textbox : MonoBehaviour
{
    public InputField input;
    public bossLevelManager manager;


    private void Awake()
    {
        input = gameObject.GetComponent<InputField>();
    }
    // Start is called before the first frame update
    void Start()
    {  
        setupInputTextbox();
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
            gameObject.GetComponent<InputField>().ActivateInputField();
        } else
        {
            gameObject.GetComponent<InputField>().DeactivateInputField();
        }
    }

    private void setupInputTextbox()
    {
        var submitter = new InputField.SubmitEvent();
        submitter.AddListener(manager.checkIfCorrect);
        input.onEndEdit = submitter;

        setTextBoxActive(true);
    }

    
}
