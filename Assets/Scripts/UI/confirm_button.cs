using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Rearrange minigame button, but could be used in some more scenes 
 */
public class confirm_button : MonoBehaviour
{
    public GameObject target_button; //Button in scene to confirm. Should only be needed in rearrange minigame
    public GameObject player; //Player in scene that is going to interact with the button
    public bool isReadyToConfirm; //Bool for manager to check if it needs to be checked the code

    private void Update()
    {
        if (!isReadyToConfirm
            && Input.GetKeyDown(KeyCode.F)
            && Vector3.Distance(player.transform.position, target_button.transform.position) < 2.5f)
        {
            isReadyToConfirm = true;
        }

    }
}
