using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop_block : Block
{
    public enum Statement { UNTIL,FINITE,INFINITE}
    public Statement state;

    public enum Until { EQUAL,GREATER,LESSER}
    public Until when;

    public int repetitions; //Variable used if its a finite statement
    public int compare_to; //Variable used if it an until statement

    void Start()
    {
        base.Start();
        item = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        guide = GameObject.FindGameObjectWithTag("guide");
        kind_of_block = kinds.LOOP;
        isPicked = false;
    }

}
