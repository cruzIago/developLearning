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

    public Button[] stage_editor_buttons;
    public Sprite[] star_rating;

    private static List<int> stages;
    private Button[] stage_buttons; //Just to not lose the reference to the buttons. Could be changed later

    private static bool isBackFromGame;

    private void Awake()
    {
        stages = new List<int>();
        if (instance == null)
        {
            instance = this;
            // Its done by this system so its known which scene has which rating 0 being can be played and no score gotten and -1 can't access
            //Tutorial
            stage_buttons = new Button[stage_editor_buttons.Length];
            stage_editor_buttons.CopyTo(stage_buttons, 0);
            PlayerPrefs.DeleteAll(); // To debug

            if (!PlayerPrefs.HasKey(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1))))
            {
                PlayerPrefs.SetInt(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1)), 0);
                stages.Add(1);

            }
            else
            {
                stages.Add(1);
            }
            for (int i = 2; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                if (!PlayerPrefs.HasKey(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i))))
                {
                    PlayerPrefs.SetInt(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)), -1);

                    stages.Add(-1);
                }
                else
                {
                    stages.Add(PlayerPrefs.GetInt(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i))));
                }
            }

            isBackFromGame = false;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
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
            if (isBackFromGame)
            {
                GameObject.Find("mainMenu").GetComponent<back_from_game>().deactivate();
               
            }
        }
    }
    

    /*
     * This will change how user interacts with stage selection so if they didn't solve stage 1_1, they can't play stage 1_2 and so on 
     */
    void checkAvaliableStages()
    {
        for (int i = 0; i < stages.Count; i++)
        {
            if (stages[i] >= 0)
            {
                stage_buttons[i].interactable = true;
                applyStarRating(stages[i], stage_buttons[i]);
                if (stages[i] > 0)
                {
                    if (i + 1 < stages.Count && stages[i + 1] == -1)
                    {
                        stages[i + 1] = 0;
                        PlayerPrefs.SetInt(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i + 2)), 0);
                        stage_buttons[i + 1].interactable = true;
                    }
                }
            }
        }
    }

    /*
     * To change star rating images as player completes levels 
     */
    private void applyStarRating(int stars, Button button)
    {
        switch (stars)
        {
            case 0:
                button.transform.GetChild(1).GetComponent<Image>().sprite = star_rating[0];
                break;
            case 1:
                button.transform.GetChild(1).GetComponent<Image>().sprite = star_rating[1];
                break;
            case 2:
                button.transform.GetChild(1).GetComponent<Image>().sprite = star_rating[2];
                break;
            case 3:
                button.transform.GetChild(1).GetComponent<Image>().sprite = star_rating[3];
                break;
            default:
                button.transform.GetChild(1).GetComponent<Image>().sprite = star_rating[0];
                string debug = "This shouldn't be happening, star rating not working at scene_manager";
                Debug.Log(debug);
                game_manager.writeOnFile(debug);
                break;
        }
    }

    /**
     * To modify the end game screen and score 
     */
    public static void checkEndScreen(int stars, float elapsed_time, int fails)
    {
        string log = "Level: " + SceneManager.GetActiveScene().name + ", Stars: " + stars + ", Failures: " + fails + ", Time spent: " + elapsed_time;
        game_manager.writeOnFile(log);

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, stars);
        stages[SceneManager.GetActiveScene().buildIndex] = stars;

        //TODO Show endgame screen

    }

    /*
     * To try again the stage 
     */
    public static void retryCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /*
     * To go back to the Play Menu 
     */
    public static void backToMenu()
    {
        isBackFromGame = true;
        SceneManager.LoadScene("scene_menu");
    }



}
