using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/*
 * Class that manages the boss enemy that appears at the end of each world
 */

public class boss : MonoBehaviour
{
    /*
     * Currently these two variables are constant privates, but they should be public 
     * if the designer wants to add different number of phases and fails to different
     * bosses 
     */
    public int levelPhases = 5; //Phases of the boss. When all of them are finished, the level ends
    public int indexOfCurrentText = 0; //Index of the text list
    public Text currentText; //Current text showing on screen
    public bool isCurrentTextQuestion; // false -> The current text is a commentary; true -> is a question
    public int remainingFails = 3; //Number of mistakes that the player can make before losing
    public Text remainingTries;
    public Text victoryText;


    // Start is called before the first frame update
    void Start()
    {
        remainingTries.text = "Intentos: " + remainingFails.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Enemy attacks when player makes a mistake
    public void attack() {
        remainingFails--;
        remainingTries.text = "Intentos: " + remainingFails.ToString();
        print("Has fallado. Te quedan: " + remainingFails);
        if (remainingFails == 0)
        {
            victoryText.text = "Derrota";
            victoryText.color = Color.red;
            victoryText.gameObject.SetActive(true);
        }
    }

    //Enemy loses life when player answers correctly
    public void lifeLoss()
    {
        levelPhases--;
        if (levelPhases == 0)
        {
            victoryText.text = "Victoria";
            victoryText.color = Color.green;
            victoryText.gameObject.SetActive(true);
        } else
        {
            indexOfCurrentText++;
        }
    }

    public float getRadius()
    {
        return GetComponent<SphereCollider>().radius;
    }
}
