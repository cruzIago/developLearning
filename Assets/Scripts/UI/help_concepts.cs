using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class help_concepts : MonoBehaviour { 

    public GameObject thePanel;
    public string definition;
    public Text targetText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
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
        print("Pinchaste en close");
        LeanTween.scale(thePanel.gameObject, new Vector3(0, 0, 0), 0.2f).setOnComplete(ClosePanel).setOnComplete(CloseDefPanel);
        
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
        targetText.text = definition;
        targetText.gameObject.SetActive(true);
    }
}
