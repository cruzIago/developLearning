using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pause_menu : MonoBehaviour
{
    public GameObject help_pause_menu;
    public GameObject pause_game_panel;
    public GameObject pause_menu_window;
    

    private bool is_game_paused;

    private void Start()
    {
        is_game_paused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            changePause();
            if (is_game_paused)
            {
                pause_menu_window.SetActive(true);
            }
            else {
                pause_menu_window.SetActive(false);
            }
        }
        
    }

    private void changePause() {
        is_game_paused = !is_game_paused;
        scene_manager.checkPause(is_game_paused);
    }

    /*
     * To go back to game 
     */
    public void onClickResumeButton()
    {
        changePause();
        pause_menu_window.SetActive(false);
    }

    /*
     * To restar the stage
     */
    public void onClickRetryButton()
    {
        changePause();
        scene_manager.retryCurrentLevel();
    }

    /*
     * To go back to menu screen 
     */
    public void onClickMenuButton()
    {
        changePause();
        scene_manager.backToMenu();
    }


    /*
     * To show the help menu so player is able to check descriptions in game 
     */
    public void onClickHelpButton()
    {
        help_pause_menu.SetActive(true);
        pause_game_panel.SetActive(false);
    }

    /*
     * To close help panel 
     */
    public void onClickBackButton() {
        help_pause_menu.SetActive(false);
        pause_game_panel.SetActive(true);
    }

    
}
