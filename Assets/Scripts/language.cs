using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class contains the id and text of strings in the game.
 */

[System.Serializable]
public class language
{
    //Need to be public so the parser can assign them
    public string id;
    public string text;

    public language() {

    }

    public language(string _id,string _text)
    {
        this.id = _id;
        this.text = _text;
    }

    public void setId(string _id) {
        this.id = _id;
    }

    public string getId() { return this.id; }

    public void setText(string _text) {
        this.text = _text;
    }

    public string getText() { return this.text; }
}
