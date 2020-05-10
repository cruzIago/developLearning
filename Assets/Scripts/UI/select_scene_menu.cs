using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/**
 * Used to select scenes on the menu.  
 */
public class select_scene_menu : MonoBehaviour
{
    
    public void OnClickStage() {
        print(gameObject.name);
        if (PlayerPrefs.HasKey(gameObject.name)) {
            if (PlayerPrefs.GetInt(gameObject.name) >= 0f) {
                SceneManager.LoadScene(gameObject.name, LoadSceneMode.Single);
            }
        }
    }
}
