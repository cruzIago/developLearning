using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class parse the contents on lang.json to c# objects. It eases language selection, so instead
 * of having switchs on every string, you only have to retrieve one and get from it
 */

[System.Serializable]
public class language_parser
{
    //Need to be public so the parser can assign them
    public List<language> spanish;
    public List<language> english;

   
    public language_parser()
    {
    }

    public language_parser(List<language> _spanish, List<language> _english)
    {
        this.spanish = _spanish;
        this.english = _english;
    }

    //This method is the one to retrieve things from lang.json
    public static language_parser CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<language_parser>(jsonString);
    }


    public void setSpanish(List<language> _spanish)
    {
        this.spanish = _spanish;
    }

    public void setEnglish(List<language> _english)
    {
        this.english = _english;
    }

    public List<language> getSpanish()
    {
        return this.spanish;
    }

    public List<language> getEnglish()
    {
        return this.english;
    }

    
}
