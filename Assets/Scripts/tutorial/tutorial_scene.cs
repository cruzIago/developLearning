using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorial_scene : MonoBehaviour
{
    public Image help_panel;
    public Text help_text;
    public Image help_console;
    public player_controller tutorial_player;

    private int text_id = 35; //35 is where tutorials start in lang.json

    private bool isWPressed, isAPressed, isDPressed, isSPressed;
    private bool isSpacePressed = false;
    private bool isEPressed, isFPressed, isItemHeldOnce = false;

    private float elapsed_time; //Used for Log reasons

    void Start()
    {
        elapsed_time = Time.time;
        tutorial_player.isInputBlocked = true;
        help_text.text = game_manager.getStringFromLang(text_id);
    }


    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && tutorial_player.isInputBlocked)
        {
            nextText();
        }

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
                text_id += 1;
                isEPressed = false;
                isFPressed = false;
                isItemHeldOnce = false;
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
                text_id += 1;
                isSpacePressed = false;
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

            text_id += 1;
            isWPressed = false;
            isSPressed = false;
            isAPressed = false;
            isDPressed = false;
            StartCoroutine(WaitALittle());
        }
    }

    /*
     * To not interrupt player instantly
     */
    IEnumerator WaitALittle()
    {

        yield return new WaitForSeconds(2.0f);
        tutorial_player.isInputBlocked = true;
        help_panel.gameObject.SetActive(true);
        help_text.text = game_manager.getStringFromLang(text_id);
    }

    void nextText()
    {
        if (text_id == 38)
        {
            help_panel.gameObject.SetActive(false);
            tutorial_player.isInputBlocked = false;
        }
        else if (text_id == 39)
        {
            help_panel.gameObject.SetActive(false);
            tutorial_player.isInputBlocked = false;
        }
        else if (text_id == 41)
        {
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
            elapsed_time = Time.time - elapsed_time;
            scene_manager.checkEndScreen(3,elapsed_time,0);
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
