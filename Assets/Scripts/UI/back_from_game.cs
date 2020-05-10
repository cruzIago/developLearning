using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Bit of an overkill, just to switch screen when player comes back from game 
 */
public class back_from_game : MonoBehaviour
{
    public GameObject playMenu;

    public void deactivate() {
        playMenu.SetActive(true);
        gameObject.SetActive(false);
        
    }
}
