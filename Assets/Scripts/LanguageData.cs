using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/*
 * Reads the file and through language_parser.cs, it creates the dictionary with the strings with the script 
 */
public class LanguageData
{
    private Dictionary<string, string> spanish_strings;
    private Dictionary<string, string> english_strings;

    public LanguageData()
    {
        string filepath = Path.Combine(Application.streamingAssetsPath, "lang.json");
        string file="";
        if (filepath.Contains("://") || filepath.Contains(":///"))
        {
            WWW www = new WWW(filepath);
            
            file = www.url;
        }
        else
        {
            file = File.ReadAllText(filepath,System.Text.Encoding.GetEncoding("Windows-1252"));
        }

        /*file = File.ReadAllText(Application.dataPath + "/Assets/Languages/lang.json");
         file = File.ReadAllText(Application.dataPath + "/Languages/lang.json");*/

        language_parser language_strings = language_parser.CreateFromJSON(file);
        spanish_strings = new Dictionary<string, string>();
        english_strings = new Dictionary<string, string>();
        toDictionaries(language_strings);
    }
    

    private void toDictionaries(language_parser l_p)
    {

        foreach (language l in l_p.english)
        {
            english_strings.Add(l.id, l.text);
        }

        foreach (language l in l_p.spanish)
        {
            spanish_strings.Add(l.id, l.text);
        }
    }

    public string getString(string id)
    {
        string text;
        int selector = Global.language;
        switch (selector)
        {
            case 0:
                if (!spanish_strings.TryGetValue(id, out text))
                {
                    text = "El id no se corresponde";
                }
                break;
            case 1:
                if (!english_strings.TryGetValue(id, out text))
                {
                    text = "Id is not found";
                }
                break;
            default:
                if (selector == 0)
                {
                    text = "Idioma no disponible";
                }
                else
                {
                    text = "Language not avaliable";
                }
                break;
        }
        return text;
    }
}
