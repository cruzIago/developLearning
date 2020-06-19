using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Manages whole tutorial level 
 */
public class tutorial_scene : MonoBehaviour
{
    public Image help_panel;
    public Text help_text;
    public Image help_console;
    public player_controller tutorial_player;
    public Image blinker;

    public Image wasd_image;
    public Image space_image;
    public Image ef_image;
    public Image nexus_image;

    private int text_id = 35; //35 is where tutorials start in lang.json

    private bool isWPressed, isAPressed, isDPressed, isSPressed;
    private bool isSpacePressed;
    private bool isEPressed, isFPressed, isItemHeldOnce;
    private bool isAbleToContinue; //Used to not skip texts on tutorial and let players read it
    private float elapsed_time; //Used for Log reasons

    private bool isTutorialEnded;
    private Coroutine blink_reference; //Used to be able to stop coroutine

    void Start()
    {
        elapsed_time = Time.time;
        tutorial_player.isInputBlocked = true;
        help_text.text = game_manager.getStringFromLang(text_id);
        blink_reference = StartCoroutine(blinkArrow());
        checkStyle();
    }

    void checkStyle()
    {
        if (GameObject.Find("game_manager"))
        {
            GameObject.Find("game_manager").GetComponent<game_manager>().changeStyle(help_panel, help_console);
        }
    }

    void Update()
    {
        if (isAbleToContinue && !scene_manager.is_pause_menu_on)
        {
            if (!isTutorialEnded && ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))) && tutorial_player.isInputBlocked)
            {
                StopCoroutine(blink_reference);
                isAbleToContinue = true;
                blink_reference = StartCoroutine(blinkArrow());
                nextText();
            }
        }

        if (!isTutorialEnded)
        {
            
            if (text_id == 38)
            {
                WASDTutorial();
            }
            if (text_id == 39)
            {
                SpaceTutorial();
            }
            if (text_id == 41)
            {
                EFTutorial();
            }
            if (text_id == 35) {
                nexus_image.gameObject.SetActive(true);
            }
            if (text_id == 36) {
                nexus_image.gameObject.SetActive(false);
            }
        }


    }

    /*
     * To avoid that the player skips a text
     */
    IEnumerator blinkArrow()
    {

        yield return new WaitForSeconds(0.5f);
        blinker.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
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
    /*
     * Used for Pick and throw of tutorial 
     */
    void EFTutorial()
    {
        if (!tutorial_player.isInputBlocked)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isEPressed = true;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                isFPressed = true;
            }
            if (tutorial_player.isItemHeld)
            {
                isItemHeldOnce = true;
            }
            if (isEPressed && isFPressed && isItemHeldOnce && !tutorial_player.isItemHeld)
            {
                isEPressed = false;
                isFPressed = false;
                isItemHeldOnce = false;
                ef_image.gameObject.SetActive(false);
                StartCoroutine(WaitALittle());
            }
        }
    }

    /*
     * Used for jump segment of tutorial 
     */
    void SpaceTutorial()
    {
        if (!tutorial_player.isInputBlocked)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSpacePressed = true;
            }
            if (isSpacePressed)
            {
                isSpacePressed = false;
                space_image.gameObject.SetActive(false);
                StartCoroutine(WaitALittle());
            }
        }
    }

    /*
     * Code belonging to WASD segment of tutorial
     */
    void WASDTutorial()
    {
        if (!tutorial_player.isInputBlocked)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                isWPressed = true;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                isSPressed = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.D))
            {
                isDPressed = true;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A))
            {
                isAPressed = true;
            }
        }
        if (isWPressed && isAPressed && isDPressed && isSPressed)
        {
            
            isWPressed = false;
            isSPressed = false;
            isAPressed = false;
            isDPressed = false;
            wasd_image.gameObject.SetActive(false);
            StartCoroutine(WaitALittle());
        }
    }

    /*
     * To not interrupt player instantly
     */
    IEnumerator WaitALittle()
    {
        yield return new WaitForSeconds(2.0f);
        text_id += 1;
        tutorial_player.isInputBlocked = true;
        help_panel.gameObject.SetActive(true);
        help_text.text = game_manager.getStringFromLang(text_id);
    }

    void nextText()
    {
        if (text_id == 38)
        {
            wasd_image.gameObject.SetActive(true);
            help_panel.gameObject.SetActive(false);
            tutorial_player.isInputBlocked = false;
        }
        else if (text_id == 39)
        {
            space_image.gameObject.SetActive(true);
            help_panel.gameObject.SetActive(false);
            tutorial_player.isInputBlocked = false;
        }
        else if (text_id == 41)
        {
            ef_image.gameObject.SetActive(true);
            help_panel.gameObject.SetActive(false);
            tutorial_player.isInputBlocked = false;
        }
        else if (text_id == 46)
        {
            help_console.gameObject.SetActive(true);
            text_id += 1;
            help_text.text = game_manager.getStringFromLang(text_id);
        }
        else if (text_id == 56)
        {
            isTutorialEnded = true;
            elapsed_time = Time.time - elapsed_time;
            scene_manager.checkEndScreen(3, elapsed_time, 0);
        }
        else
        {
            text_id += 1;
            if (!tutorial_player.isInputBlocked)
            {
                tutorial_player.isInputBlocked = true;
                help_panel.gameObject.SetActive(true);
            }
            help_text.text = game_manager.getStringFromLang(text_id);
        }
    }
}
