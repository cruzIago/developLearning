using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reorder_block : Block
{
    public Text reorder_text;
    public Color block_color;

    void Start()
    {
        base.Start();
        item = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        guide = GameObject.FindGameObjectWithTag("guide");
        kind_of_block = kinds.REORDER;
        isPicked = false;

    }


    /*
     * To set random color at start or reset of rearrange game so players are discouraged to try at random
     */
    public void setColor()
    {
        Renderer matRender = this.GetComponent<Renderer>();
        block_color = reorder_text.color;
        matRender.material.SetColor("_Color", block_color);
    }
}
