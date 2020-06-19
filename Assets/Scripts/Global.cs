using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * Stores some useful information that would be needed through the project 
 */
public class Global
{
    /* List of languages:
     * ESP: Spanish
     * ENG: English
     */
    enum languageSelector { ESP = 0, ENG = 1 };

    //Default language on start
    public static int language = (int)languageSelector.ESP;

    public LanguageData languages;

    public Global()
    {
        loadLanguages();
    }

    
    private void loadLanguages()
    {
        languages = new LanguageData();
    }
    
}

