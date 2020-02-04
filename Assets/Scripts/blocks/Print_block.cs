using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Print_block : Block
{

    void Start()
    {
        base.Start();
        item = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        guide = GameObject.FindGameObjectWithTag("guide");
        kind_of_block = kinds.PRINT;
        isPicked = false;

    }
    
}
