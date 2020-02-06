using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Variable_block : Block
{
    public Text variable_written; //Default name to give it on editor via text on console

    private string variable_name;
    private string variable_value;

    void Start()
    {
        base.Start();
        item = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        guide = GameObject.FindGameObjectWithTag("guide");
        kind_of_block = kinds.VARIABLE;
        isPicked = false;
        {
        }
        if (variable_written != null)
        {
            variable_name = variable_written.text;
        }
    }

    public string getVariableName()
    {
        return variable_name;
    }

    public void setVariableName(string name)
    {
        if (variable_written != null)
        {
            variable_name = name;
        }
    }

    public string getVariableValue()
    {
        return variable_value;
    }

    public void setVariableValue(string value) {
        variable_value = value;
    }

}
