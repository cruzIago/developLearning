using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bossLevelManager : MonoBehaviour
{
    private string currentCorrectAnswer;
    private List<string> answers;
    public TextAsset answersFile;
    private int indexOfAnswers = 0;

    public boss boss;
    private List<string> bossComments;
    public TextAsset bossFile;
    public bool bossStarted;
    public bool gameOver;

    public player_controller player;
    public input_textbox input;

    public Text completeCode;

    // Start is called before the first frame update
    void Start()
    {
        bossStarted = false;
        gameOver = false;
        answers = appendText(answersFile);
        bossComments = appendText(bossFile);
        //beginBossFight();
    }

    // Update is called once per frame
    void Update()
    {
        inputController();
        if (!bossStarted && (Vector3.Distance(player.transform.position, boss.transform.position) - boss.getRadius()) <= 9)
        {
            print("COMIENSA");
            bossStarted = true;
            beginBossFight();
        }
    }

    //Check if the answer is correct. TODO->A good checking of the input
    public void checkIfCorrect(string args0)
    {
        input.gameObject.GetComponent<InputField>().DeactivateInputField();
        if (!gameOver)
        {
            if (input.input.text.Equals(currentCorrectAnswer))
            {
                boss.lifeLoss();
                nextBossText();
                indexOfAnswers++;
                addNewLineToProgram(currentCorrectAnswer);
                if (indexOfAnswers < answers.Count)
                {
                    nextCurrentAnswer();
                    print("Actual: " + currentCorrectAnswer);
                }
            }
            else
            {
                boss.attack();
                if (boss.remainingFails <= 0)
                {
                    gameOver = true;
                }
            }
        }     
    }

    //Unlocks/Blocks InputField and/or Player
    public void setTextBoxActive(bool active)
    {
        input.setTextBoxActive(active);
        player.isInputBlocked = active;
    }

    //Input Management
    private void inputController()
    {
        if (!boss.isCurrentTextQuestion && bossStarted && !gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                boss.indexOfCurrentText++;
                nextBossText();
            }
        }
    }

   
    //Loads text from TextAsset
    public List<string> appendText(TextAsset textFile)
    {
        List<string> returningList = new List<string>();
        string[] lines = textFile.text.Split('/');
        for (int i = 0; i < lines.Length; i++)
        {
            string newLine = lines[i].Substring(lines[i].IndexOf("\n") + 1);
            returningList.Add(newLine);
        }
        return returningList;
    }

    public void nextCurrentAnswer()
    {
        currentCorrectAnswer = answers[indexOfAnswers];
    }

    //Changes to next text
    private void nextBossText()
    {
        string newLine;
        if (boss.indexOfCurrentText < bossComments.Count)
        {
            if (bossComments[boss.indexOfCurrentText].Contains("@"))
            {
                print("Es comentario"); //Debug
                boss.isCurrentTextQuestion = false;
                newLine = bossComments[boss.indexOfCurrentText].Substring(bossComments[boss.indexOfCurrentText].IndexOf("@") + 1);
            }
            else if (bossComments[boss.indexOfCurrentText].Contains("#"))
            {
                print("Es pregunta"); //Debug
                boss.isCurrentTextQuestion = true;
                newLine = bossComments[boss.indexOfCurrentText].Substring(bossComments[boss.indexOfCurrentText].IndexOf("#") + 1);
            }
            else
            {
                newLine = "[ERROR] Check file, there's been a mistake. All lines must start with @ or #";
            }
            boss.currentText.text = newLine;
            if (!boss.isCurrentTextQuestion)
            {
                setTextBoxActive(false);
            } else
            {
                setTextBoxActive(true);
            }
        }
    }

    public void OnClickButtonConfirm()
    {
        if (bossStarted && !gameOver)
        {
            if (!boss.isCurrentTextQuestion)
            {
                boss.indexOfCurrentText++;
                nextBossText();
            }
            else
            {
                //TODO Correctly
                string args = "";
                checkIfCorrect(args);
            }
        }
    }

    private void beginBossFight()
    {
        input.gameObject.SetActive(true);
        boss.currentText.gameObject.SetActive(true);
        nextCurrentAnswer();
        nextBossText();
    }

    private void addNewLineToProgram(string newLine)
    {
        if (!completeCode.text.Equals(""))
        {
            print("salto de linea");
            completeCode.text = completeCode.text + "\n" + newLine;
        } else
        {
            print("PASO POR AQUI");
            completeCode.text = completeCode.text + newLine;
        }
    }
}
