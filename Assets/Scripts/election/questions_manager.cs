using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questions_manager : MonoBehaviour
{
    public List<Text> questions;
    public List<Text> possibleAnswers;
    private int indexOfCorrectAnswer;

    private void shuffleAnswers()
    {
        for (int i = 0; i < possibleAnswers.Count; i++)
        {
            Text tempAns = possibleAnswers[i];        
            int randomIndex = Random.Range(i, possibleAnswers.Count);
            possibleAnswers[i] = possibleAnswers[randomIndex];
            possibleAnswers[randomIndex] = tempAns;
        }
        //Shuffle list:https://answers.unity.com/questions/486626/how-can-i-shuffle-alist.html 
    }

    //For Debugging
    private void printAnswers()
    {
        int i = 0;
        foreach (Text t in possibleAnswers)
        {
            print("Respuesta " + i + ": " + t.text);
            i++;
        }
    }

    //For Debugging
    private void printQuestions()
    {
        int i = 0;
        foreach (Text t in questions)
        {
            print("Pregunta " + i + ": " + t.text);
            i++;
        }
    }

    public void checkCorrect()
    {
        //TODO
    }

    // Start is called before the first frame update
    void Start()
    {
        printQuestions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
