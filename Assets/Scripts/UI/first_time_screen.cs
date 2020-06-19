using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Manages the first time screens when new level is loaded. May need brute force of some degree. 
 */
public class first_time_screen : MonoBehaviour
{
    public Text script;
    public Image blinker;

    public Image background_texts;
    public Image background_console;

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
            if (SceneManager.GetActiveScene().name.Contains("descriptionIO"))
            {
                next_text = 57;
                script.text = game_manager.getStringFromLang(57);
            }
            else if (SceneManager.GetActiveScene().name.Contains("descriptionC"))
            {
                next_text = 100;
                script.text = game_manager.getStringFromLang(100);
            }
            else {
                next_text = 142;
                script.text = game_manager.getStringFromLang(142);
            }
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

        if (GameObject.Find("Main").GetComponent<player_controller>() != null)
        {
            GameObject.Find("Main").GetComponent<player_controller>().isInputBlocked = true;
        }
        checkStyle();
        blinker_reference = StartCoroutine(blinkArrow());
    }

    void checkStyle()
    {
        if (GameObject.Find("game_manager"))
        {
            GameObject.Find("game_manager").GetComponent<game_manager>().changeStyle(background_texts, background_console);
        }
    }

    void Update()
    {
        if (isAbleToContinue && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            if (scene != 0 || next_text == 60)
            {
                if (GameObject.Find("Main").GetComponent<player_controller>() != null)
                {
                    GameObject.Find("Main").GetComponent<player_controller>().isInputBlocked = false;
                }
                Destroy(gameObject);
            } else if (next_text==102 || next_text==144) {
                next_text = 59;
            }
            else
            {
                isAbleToContinue = false;
                StopCoroutine(blinker_reference);
                first_world();
            }
        }
    }

    void first_world()
    {
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
