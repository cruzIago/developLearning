using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Print_block : Block
{
    public Text editor_text; //Used if user doesn't need to write text
    private string pre_text { get; set; } //Text written before variables
    private string true_text { get; set; } //Text acquired from variable

    void Start()
    {
        base.Start();
        item = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        guide = GameObject.FindGameObjectWithTag("guide");
        kind_of_block = kinds.PRINT;
        isPicked = false;

        if (editor_text != null) {
            pre_text = editor_text.text;
        }
    }

    
    
}
