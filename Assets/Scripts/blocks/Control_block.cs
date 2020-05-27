using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control_block : Block
{
    public enum Statement { IF, ELSEIF, ELSE }
    public Statement state;
    public enum Compare { EVEN, GREATER, LESSER, EQUAL, STARTS_AS, ENDS_IN, EQUALTEXT }
    public Compare comparison;

    public string default_name;
    public string default_value;

    public bool is_default_able;

    private int numbered_variable;
    private string string_variable;


    public Variable_block setted_variable;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        item = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        guide = GameObject.FindGameObjectWithTag("guide");
        kind_of_block = kinds.CONTROL;
        isPicked = false;

        if (is_default_able)
        {
            bool is_number = false;
            is_number = int.TryParse(default_value, out numbered_variable);
            if (!is_number)
            {
                string_variable = default_value;
            }
        }

    }


    public string DoComparison(int number_to_compare)
    {
        string fill = "";
        if (setted_variable != null && !is_default_able) {
            numbered_variable = int.Parse(setted_variable.getVariableValue());
        }
        switch (comparison)
        {
            case Compare.EVEN:
                if ((numbered_variable % 2) == 0)
                {
                    fill = " es par";
                }
                else
                {
                    fill = " es impar";
                }
                break;
            case Compare.GREATER:
                if (numbered_variable > number_to_compare)
                {
                    fill = " es mayor que " + number_to_compare;
                }
                else
                {
                    fill = " no es mayor que " + number_to_compare;
                }
                break;
            case Compare.LESSER:
                if (numbered_variable < number_to_compare)
                {
                    fill = " es menor que " + number_to_compare;
                }
                else
                {
                    fill = " no es menor que " + number_to_compare;
                }
                break;
            case Compare.EQUAL:
                if (numbered_variable == number_to_compare)
                {
                    fill = " es igual que " + number_to_compare;
                }
                else {
                    fill = " no es igual que " + number_to_compare;
                }
                break;
            default:
                Debug.LogError("No Compare. found. Control_block.cs, line 40");
                break;
        }
        return fill;
    }

    public string DoComparison(string string_to_compare)
    {
        string fill = "";
        if (setted_variable != null && !is_default_able)
        {
            string_variable = setted_variable.getVariableValue();
        }
        switch (comparison)
        {
            case Compare.STARTS_AS:
                if (string_variable.StartsWith(string_to_compare))
                {
                    fill = " empieza por " + string_to_compare;
                }
                else
                {
                    fill = " no empieza por " + string_to_compare;
                }
                break;
            case Compare.ENDS_IN:
                if (string_variable.EndsWith(string_to_compare))
                {
                    fill = " termina por " + string_to_compare;
                }
                else
                {
                    fill = " no termina por " + string_to_compare;
                }
                break;
            case Compare.EQUALTEXT:
                if (string_variable.ToUpper().Equals(string_to_compare.ToUpper()))
                {
                    fill = default_name+ " es " + string_to_compare;
                }
                else {
                    fill = default_name + " no es " + string_to_compare;
                }
                break;
            default:
                Debug.LogError("No Compare. found. Control_block.cs, line 40");
                break;
        }
        return fill;
    }

   

    public void SetVariableToCompare(Variable_block variable)
    {
        setted_variable = variable;
        if (is_default_able) {
            setted_variable.setVariableName(default_name);
            setted_variable.setVariableValue(default_value);
        }
    }





}
