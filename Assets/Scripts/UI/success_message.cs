using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class success_message : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        show_hide_message(false);
        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Hides text
    void show_hide_message(bool show)
    {
        gameObject.SetActive(show);
    }

    //Success
    public void successMessage()
    {
        show_hide_message(true);
        text.color = Color.green;
        text.text = "Acierto";
        StartCoroutine(hideCoroutine());
    }

    //Fail
    public void failMessage()
    {
        show_hide_message(true);
        text.color = Color.red;
        text.text = "Error";
        StartCoroutine(hideCoroutine());
    }

    //Fails after a time
    IEnumerator hideCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        show_hide_message(false);
    }
}
