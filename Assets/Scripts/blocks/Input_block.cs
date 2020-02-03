using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_block : Block
{
   
    void Start()
    {
        item = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        guide = GameObject.FindGameObjectWithTag("guide");
        kind_of_block = kinds.INPUT;
        isPicked = false;

    }
}
