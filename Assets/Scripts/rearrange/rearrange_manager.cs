using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class rearrange_manager : MonoBehaviour
{
    public List<Color> colors; // List of colors at manager dispossal
    public List<rearrange_checker> platforms; //List of platforms in scene
    public List<Block> blocks; //List of blocks in scene
    public List<Text> texts; //List of texts on GUI to arrange
    public List<RectTransform> pivots; //List of positions of texts to solve
    public Text messageConfirm;

    private int mistakes;
    private float elapsed_time;

    public bool is_review_stage;
    public review_manager reviewer;

    public void Start()
    {
        mistakes = 0;
        elapsed_time = Time.time;
        resetControl();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            resetControl();
        }
        if (GetComponent<confirm_button>().isReadyToConfirm)
        {
            button_pressed_compile();
            GetComponent<confirm_button>().isReadyToConfirm = false;
        }
    }

    /*
     * It checks if code would compile 
     */
    private bool checkCompile()
    {
        bool isAbleToCompile = true;
        for (int i = 0; i < platforms.Count(); i++)
        {
            if (platforms[i].getBlock() == null
                || !platforms[i].getBlock().Equals(blocks[i]))
            {
                isAbleToCompile = false;
                break;
            }
        }
        return isAbleToCompile;
    }

    /*
     * Control to compile on button 
     */
    private void button_pressed_compile()
    {
        if (checkCompile())
        {
            if (!is_review_stage)
            {
                int stars = 0;
                if (mistakes <= 2)
                {
                    stars = 3;
                }
                else if (mistakes <= 3)
                {
                    stars = 2;
                }
                else
                {
                    stars = 1;
                }
                scene_manager.checkEndScreen(stars, elapsed_time, mistakes);
            }
            else {
                foreach (Block b in blocks) {
                    b.gameObject.SetActive(false);
                }
                reviewer.nextReview(mistakes);
            }
            //print("Correcto");
            //messageConfirm.text = "¡Bien hecho!";
        }
        else
        {
            mistakes += 1;
            resetControl();
        }
    }

    /*
     * Randomize color list so they aren't the same color every reset
     * Not the most efficient if done every frame, but for this time should be fine. Testing either way  
     */
    public void resetControl()
    {
        messageConfirm.text = "Pulsa F para confirmar";
        colors = colors.OrderBy(x => Random.value).ToList();
        texts = texts.OrderBy(x => Random.value).ToList();
        List<rearrange_checker> randomPlatforms = platforms.OrderBy(x => Random.value).ToList();
        bool isRandomized = false;
        do
        {
            for (int i = 0; i < colors.Count(); i++)
            {
                texts[i].color = Color.gray;
                texts[i].color = colors[i];
            }

            for (int i = 0; i < blocks.Count(); i++)
            {
                blocks[i].GetComponent<Reorder_block>().setColor();
                blocks[i].GetComponent<Rigidbody>().isKinematic = false;
                if (blocks[i].player != null
                    && blocks[i].player.GetComponent<player_controller>().isItemHeld)
                {
                    blocks[i].pickDown();
                }
                blocks[i].transform.position = randomPlatforms[i].transform.position + (Vector3.up * 4);
            }
            isRandomized = !checkCompile(); //Check if is randomized enough to not be solved at start
        } while (!isRandomized);
    }

    /*
     * Efficient real-time shuffle brought by Smooth.Foundations. Gonna try it if simple shuffling is too demanding on slow computers
     * https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052/
     */
    void Shuffle<T>(IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
