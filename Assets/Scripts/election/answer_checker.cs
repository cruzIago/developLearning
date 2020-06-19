using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Setups the ground checkers so its known when player falls through them 
 */
public class answer_checker : MonoBehaviour
{
    public floor_manager manager;
    public Text myAnswer;
    private bool onEnterFlag;
    
    void Start()
    {
        onEnterFlag = false;
    }
    
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
