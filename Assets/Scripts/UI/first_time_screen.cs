using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class first_time_screen : MonoBehaviour
{
    public Text script;
    public Image blinker;

    private bool isAbleToContinue;
    private int scene;
    private int next_text;
    private Coroutine blinker_reference;

    void Start()
    {
        
        isAbleToContinue = false;
        if (SceneManager.GetActiveScene().name.Contains("description"))
        {
            
                scene = 0;
                next_text = 57;
                script.text = game_manager.getStringFromLang(57);
            
        }
        else if (SceneManager.GetActiveScene().name.Contains("election"))
        {
            scene = 1;
            script.text = game_manager.getStringFromLang(61);
        }
        else if (SceneManager.GetActiveScene().name.Contains("rearrange"))
        {
            scene = 2;
            script.text = game_manager.getStringFromLang(62);
        }
        else if (SceneManager.GetActiveScene().name.Contains("fill"))
        {
            scene = 3;
            script.text = game_manager.getStringFromLang(63);
        }
        else if (SceneManager.GetActiveScene().name.Contains("review"))
        {
            scene = 4;
            script.text = game_manager.getStringFromLang(64);
        }

        if (GameObject.Find("Main").GetComponent<player_controller>() != null) {
            GameObject.Find("Main").GetComponent<player_controller>().isInputBlocked = true;
        }
        blinker_reference = StartCoroutine(blinkArrow());
    }

    void Update()
    {
        if (isAbleToContinue && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))) {
            if (scene != 0 || next_text==60||next_text==102||next_text==143)
            {
                if (GameObject.Find("Main").GetComponent<player_controller>() != null)
                {
                    GameObject.Find("Main").GetComponent<player_controller>().isInputBlocked = false;
                }
                Destroy(gameObject);
            }
            else {
                isAbleToContinue = false;
                StopCoroutine(blinker_reference);
                first_world();
            }
        }
    }

    void first_world() {
        next_text += 1;
        script.text = game_manager.getStringFromLang(next_text);
        blinker_reference = StartCoroutine(blinkArrow());
    }
    
    /*
     * To avoid that the player skips a text
     */
    IEnumerator blinkArrow()
    {

        yield return new WaitForSeconds(0.2f);
        blinker.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        blinker.gameObject.SetActive(true);

        isAbleToContinue = true;

        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            blinker.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            blinker.gameObject.SetActive(true);
        }

    }

}
