using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Used so if clicked in a concept on Help menu, menu is hidden and concept is shown to read it.
 */
public class help_concepts : MonoBehaviour { 

    public GameObject thePanel;
    public int definition;
    public Text targetText;
    
    void Start()
    {
    }
    
    void Update()
    {
        
    }

    public void OnClickConcept()
    {
        transform.parent.gameObject.SetActive(false);
        OpenPanel();
        
    }

    public void OnClickBack()
    {
        //Places the scene the way it has to be placed for the next time that the user clicks on Help button
        ClosePanel();
    }

    public void ClosePanel()
    {
        LeanTween.scale(thePanel.gameObject, new Vector3(0, 0, 0), 0.2f).setOnComplete(CloseDefPanel);
        
    }

    private void CloseDefPanel() {
        transform.parent.gameObject.SetActive(true);
        thePanel.gameObject.SetActive(false);
        targetText.gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        thePanel.gameObject.SetActive(true);

        thePanel.transform.localScale = Vector3.zero;
        LeanTween.scale(thePanel, new Vector3(1, 1, 1), 0.3f); //Scale up UI by tweening
        targetText.text = game_manager.getStringFromLang(definition);
        targetText.gameObject.SetActive(true);
    }
}
