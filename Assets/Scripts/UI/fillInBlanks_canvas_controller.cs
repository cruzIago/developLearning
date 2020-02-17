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

    public Animator animator; //Animator to enlarge the console screen and give feedback to the user

    private Block.kinds last_kind_block; // To check last block checked

    private void Start()
    {
        user_solution = new List<int>();
        last_kind_block = Block.kinds.DEFAULT;
    }

    private void Update()
    {
    }

    //Reset blocks to initial position
    private void blocks_to_default()
    {
        foreach (Block b in blocks_in_scene)
        {
            b.reset_position();
            b.gameObject.SetActive(true);
        }
    }

    //Fill the blanks with underscore again
    private void blanks_to_default()
    {
        foreach (Text t in blanks_to_fill)
        {
            t.color = Color.black;
            t.text = "_______";
        }
    }

    // Check if the solution provided by the user is the same as the required
    private IEnumerator checkSolutions()
    {
        Time.timeScale = 0;
        animator.SetBool("checking_conditions", true);
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < user_solution.Count; i++)
        {
            if (user_solution[i] == game_solution[i])
            {
                blanks_to_fill[i].color = Color.green;
            }
            else
            {
                blanks_to_fill[i].color = Color.red;
                print("Fallo");
                yield return new WaitForSeconds(1.0f);
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
        if (user_solution.SequenceEqual<int>(game_solution))
        {
            print("Acierto");
        }
        animator.SetBool("checking_conditions", false);
        blanks_to_default();
        user_solution.Clear();
        Time.timeScale = 1;
        print("done");

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
                if (last_kind_block == Block.kinds.PRINT)
                {
                    blanks_to_fill[user_solution.Count - 1].text = other.gameObject.GetComponent<Variable_block>().getVariableName();
                }
                else
                {
                    blanks_to_fill[user_solution.Count - 1].text = "var";
                }
                
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
            else if (other.gameObject.GetComponent<Block>().kind_of_block == Block.kinds.RESET)
            {
                user_solution.Clear();
                blanks_to_default();
                blocks_to_default();
            }
            else
            {
                print("ERROR ON CANVAS CONTROLLER, NO KIND OF ITEM ALLOWED");
            }

            last_kind_block = other.gameObject.GetComponent<Block>().kind_of_block;
            other.gameObject.GetComponent<Block>().setCollisions(true);
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Block>().reset_position();
            print(user_solution.Count);
            if (user_solution.Count == game_solution.Length) {
                StartCoroutine(checkSolutions());
            }
        }
    }
}
