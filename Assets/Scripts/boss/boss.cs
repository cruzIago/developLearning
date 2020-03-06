using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class boss : MonoBehaviour
{
    private const int HITS_TO_DEFEAT = 3;

    private List<string> texts;
    public TextAsset bossPhrases;
    public Text currentText;
    private int indexOfCurrentText = 0;
    private int lifeTotal = 5;
    private int currentHits = 0;
     
    // Start is called before the first frame update
    void Start()
    {
        texts = new List<string>();
        appendText();//Change when needed!
        currentText.text = texts[indexOfCurrentText];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Enemy attacks when player makes a mistake
    public void attack() {
        currentHits++;
        if (currentHits == HITS_TO_DEFEAT)
        {
            //Game Over: Lose TODO
            print("Fin de la partida");
        }
    }

    //Enemy loses life when player answers correctly
    public void lifeLoss()
    {
        lifeTotal--;
        if (lifeTotal == 0)
        {
            //Game Over: Win TODO
            print("Has ganado");
        } else
        {
            nextText();
        }
    }

    //Changes to next text
    private void nextText() {
        indexOfCurrentText++;
        if (indexOfCurrentText < texts.Count)
        {
            currentText.text = texts[indexOfCurrentText];
        }
    }

    //Loads texts from TextAsset
    public void appendText()
    {
        string[] lines = bossPhrases.text.Split(';');
        for (int i = 0; i < lines.Length; i++)
        {
            texts.Add(lines[i]);
            print(texts[i]);
        }
    }
}
