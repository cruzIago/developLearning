using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * Used to change between scenes, end minigame screen 
 */
public class scene_manager : MonoBehaviour
{
    public static scene_manager instance; //To satisfy singleton needs

    public Button[] stage_buttons;
    private static List<int> stages;

    private void Awake()
    {
        stages = new List<int>();
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Its done by this system so its known which scene has which rating 0 being can be played and no score gotten and -1 can't access
        //Tutorial
        PlayerPrefs.DeleteAll(); // To debug

        if (!PlayerPrefs.HasKey(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1))))
        {
            PlayerPrefs.SetInt(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1)), 0);
            stages.Add(1);
        }
        else {
            stages.Add(1);
        }
        for (int i = 2; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (!PlayerPrefs.HasKey(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i))))
            {
                PlayerPrefs.SetInt(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)), -1);

                stages.Add(-1);
            }
            else {
                stages.Add(PlayerPrefs.GetInt(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i))));
            }
        }


        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //If the scene loaded is the menu scene...
        if (scene.buildIndex == 0)
        {
            checkAvaliableStages();
        }
    }

    /*
     * This will change how user interacts with stage selection so if they didn't solve stage 1_1, they can't play stage 1_2 and so on 
     */
    void checkAvaliableStages()
    {
        for (int i = 0; i < stages.Count; i++)
        {
            if (stages[i]>=0) {
                stage_buttons[i].interactable = true;
                if (stages[i] > 0) {
                    if (i + 1 < stages.Count && stages[i+1]==-1) {
                        stages[i + 1] = 0;
                        stage_buttons[i + 1].interactable = true;
                    }
                }
            }
        }
    }
    /**
     * To modify the end game screen and score 
     */
    public static void checkEndScreen(int stars)
    {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, stars);
        stages[SceneManager.GetActiveScene().buildIndex] = stars;
    }




}
