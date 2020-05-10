using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class end_game_screen : MonoBehaviour
{
    public Button retry_button;
    public Button menu_button;

    
    public void onRetryButtonClick() {
        scene_manager.retryCurrentLevel();
    }

    public void onMenuButtonClick() {
        scene_manager.backToMenu();
    }
}
