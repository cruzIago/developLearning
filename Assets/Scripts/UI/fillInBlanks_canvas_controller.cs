using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/**
 * Used to controll UI console for kids to see how they are filling it
 */
public class fillInBlanks_canvas_controller : MonoBehaviour
{
    public int[] game_solution; // To modify on editor for each minigame. It stablish how blocks should be stored to complete the level

    private List<int> user_solution; // The array that the user will fill throwing cubes to the trigger zone

    public Text[] blanks_to_fill; // Text where user will see the changes 

    public Block[] blocks_in_scene;

    private void Start()
    {
        user_solution = new List<int>();
    }

    private void Update()
    {
        if (user_solution.Count == game_solution.Length)
        {
            if (checkSolutions())
            {
                print("Conseguido");
            }
            else
            {
                user_solution.Clear();
                blanks_to_default();
                blocks_to_default();
                print("No conseguido");
            }
        }
    }

    //Reset blocks to initial position
    private void blocks_to_default() {
        foreach (Block b in blocks_in_scene) {
            b.reset_position();
            b.gameObject.SetActive(true);
        }
    }

    //Fill the blanks with underscore again
    private void blanks_to_default() {
        foreach(Text t in blanks_to_fill) {
            t.text = "_______";
        }
    }

    // Check if the solution provided by the user is the same as the required
    private bool checkSolutions()
    {
        if (game_solution.SequenceEqual<int>(user_solution))
        {
            return true;
        }
        return false;
    }

    // Checks which blocks enters on the box
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag.Equals("item")
            && user_solution.Count < game_solution.Length)
        {
            if (other.gameObject.GetComponent<Block>().kind_of_block == Block.kinds.VARIABLE)
            {
                user_solution.Add((int)Block.kinds.VARIABLE);
                blanks_to_fill[user_solution.Count - 1].text = "var";

            }
            else if (other.gameObject.GetComponent<Block>().kind_of_block == Block.kinds.INPUT)
            {
                user_solution.Add((int)Block.kinds.INPUT);
                blanks_to_fill[user_solution.Count - 1].text = "input()";
            }
            else if (other.gameObject.GetComponent<Block>().kind_of_block == Block.kinds.PRINT)
            {
                user_solution.Add((int)Block.kinds.PRINT);
                blanks_to_fill[user_solution.Count - 1].text = "print";
            }
            else
            {
                print("ERROR ON CANVAS CONTROLLER, NO KIND OF ITEM ALLOWED");
            }

            other.gameObject.GetComponent<Block>().setCollisions(true);
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Block>().textGuide.SetActive(false);
            other.gameObject.SetActive(false);
        }
    }
}
