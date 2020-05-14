using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class end_game_screen : MonoBehaviour
{
    public Button retry_button;
    public Button menu_button;
    public Image star_rating;
    public Sprite[] star_images; //Needed to have endgame screen with a reference for the sprites
    public void onRetryButtonClick() {
        scene_manager.retryCurrentLevel();
    }

    public void onMenuButtonClick() {
        scene_manager.backToMenu();
    }

    public void changeStars(int stars) {
        star_rating.sprite = star_images[stars];
    }
}
