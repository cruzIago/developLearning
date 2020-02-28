using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control_block : Block
{
    public Text control_text;
    public Color block_color;
    void Start()
    {
        base.Start();
        item = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        guide = GameObject.FindGameObjectWithTag("guide");
        kind_of_block = kinds.CONTROL;
        isPicked = false;

    }

    /*
     * To set random color at start or reset so players are discouraged to try at random
     */
    public void setColor()
    {
        Renderer matRender = this.GetComponent<Renderer>();
        block_color = control_text.color;
        matRender.material.SetColor("_Color", block_color);
    }
}
