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
    public Sprite[] star_rating;
    public GameObject end_game_screen;
    public GameObject pause_game_screen;
    public GameObject first_time_screen;

    public static bool is_pause_menu_on;

    private static List<int> stages;
    private Button[] stage_buttons; //Just to not lose the reference to the buttons. Could be changed later

    private static bool is_back_from_game;
    private static GameObject end_game_reference;

    private void Awake()
    {
        if (instance == null)
        {
            stages = new List<int>();
            instance = this;
            // Its done by this system so its known which scene has which rating 0 being can be played and no score gotten and -1 can't access
            //Tutorial
            end_game_reference = end_game_screen;
            PlayerPrefs.DeleteAll(); // To debug

            //Tutorial
            if (!PlayerPrefs.HasKey(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1))))
            {
                PlayerPrefs.SetInt(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1)), 0); //TODO CAMBIAR
                stages.Add(0);

            }
            else
            {
                stages.Add(PlayerPrefs.GetInt(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1))));
            }
            for (int i = 2; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                if (!PlayerPrefs.HasKey(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i))))
                {
                    PlayerPrefs.SetInt(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)), -1); //TODO CAMBIAR

                    stages.Add(-1);
                }
                else
                {
                    stages.Add(PlayerPrefs.GetInt(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i))));
                }
            }

            is_back_from_game = false;
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
            stage_buttons = GameObject.Find("startingMenu").GetComponent<menu_references>().stage_buttons;
            checkAvaliableStages();
            if (is_back_from_game)
            {
                is_back_from_game = false;
                GameObject.Find("mainMenu").GetComponent<back_from_game>().deactivate();

            }
        }
        else if(scene.buildIndex!=1) {
            Instantiate(pause_game_screen, Vector3.zero, Quaternion.identity);
            if (PlayerPrefs.GetInt(scene.name)<=0 && !scene.name.Contains("boss")) {
                Instantiate(first_time_screen, Vector3.zero, Quaternion.identity);
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
        stages[SceneManager.GetActiveScene().buildIndex-1] = stars;
        if (GameObject.Find("Main").GetComponent<player_controller>() != null)
        {
            GameObject.Find("Main").GetComponent<player_controller>().isInputBlocked = true;
        }
        GameObject temp_end_game=(GameObject) Instantiate(end_game_reference, Vector3.zero, Quaternion.identity);
        temp_end_game.GetComponentInChildren<end_game_screen>().changeStars(stars);

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
        is_back_from_game = true;
        SceneManager.LoadScene("scene_menu");
    }

    /*
     * For pause menu to avoid player movement
     */ 
    public static void checkPause(bool paused) {
        is_pause_menu_on = paused;

        if (GameObject.Find("Main").GetComponent<player_controller>() != null)
        {
            GameObject.Find("Main").GetComponent<player_controller>().isInputBlocked = paused;
        }   
    }

}
