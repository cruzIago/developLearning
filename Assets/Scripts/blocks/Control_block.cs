using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_block : Block
{
    public enum Statement {IF,ELSEIF,ELSE}
    public Statement state;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        item = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        guide = GameObject.FindGameObjectWithTag("guide");
        kind_of_block = kinds.CONTROL;
        isPicked = false;
    }

    

    
}
