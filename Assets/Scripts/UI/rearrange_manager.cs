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
    public List<Text> answerTexts; //List of user given answers
    public List<RectTransform> pivots; //List of positions of texts to solve

    public void Start()
    {
        resetControl();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            resetControl();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            checkCompile();
        }
    }

    /*
     * It checks if code would compile or if there are errors 
     */
    private void checkCompile()
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

        if (isAbleToCompile)
        {
            print("Correcto");
        }
        else
        {
            resetControl();
        }
    }

    /*
     * Randomize color list so they aren't the same color every reset
     * Not the most efficient if done every frame, but for this time should be fine. Testing either way  
     */
    public void resetControl()
    {
        colors = colors.OrderBy(x => Random.value).ToList();
        texts = texts.OrderBy(x => Random.value).ToList();
        List<rearrange_checker> randomPlatforms = platforms.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < colors.Count(); i++)
        {
            texts[i].color = Color.gray;
            texts[i].color = colors[i];
        }

        for (int i = 0; i < blocks.Count(); i++)
        {
            blocks[i].GetComponent<Control_block>().setColor();
            blocks[i].GetComponent<Rigidbody>().isKinematic = false;
            if (blocks[i].player!=null 
                && blocks[i].player.GetComponent<player_controller>().isItemHeld)
            {
                blocks[i].pickDown();
            }
            blocks[i].transform.position = randomPlatforms[i].transform.position + (Vector3.up * 4);
        }
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
