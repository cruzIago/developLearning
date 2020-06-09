using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu_references : MonoBehaviour
{
    public Button[] stage_buttons;
    public Button[] world_buttons;

    public GameObject play_menu;
    public Sprite menu_standard;
    public Sprite menu_blurred;
    public Image background;

    public void Start()
    {
        if (play_menu.active)
        {
            background.sprite = menu_blurred;
        }
        else {
            background.sprite = menu_standard;
        }
    }
    public void OnClickChangeBG() {
        background.sprite = menu_blurred;
    }

    public void OnClickStandardBG() {
        background.sprite = menu_standard;
    }
}
