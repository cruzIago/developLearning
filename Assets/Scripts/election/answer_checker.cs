using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class answer_checker : MonoBehaviour
{
    public floor_manager manager;
    public Text myAnswer;
    private bool onEnterFlag;

    // Start is called before the first frame update
    void Start()
    {
        onEnterFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void checkCollision()
    {
        manager.chosenAnswer = myAnswer;
        manager.checkAnswer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.tag == "Player" && !onEnterFlag)
        {
            onEnterFlag = true;
            checkCollision();
        }
    }

    public void resetCollision()
    {
        onEnterFlag = false;
    }

    public void move(Vector3 destination)
    {
        transform.position = destination;
        myAnswer.rectTransform.position = destination + new Vector3(0, 7, 0);
    }
}
