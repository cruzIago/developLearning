using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Block parent class. Used to have a common Update method and atributes
 */
public class Block : MonoBehaviour
{
    public enum kinds { INPUT, PRINT, VARIABLE, RESET, CONTROL, REORDER, DEFAULT }; // Kind of block to check by the UI

    public kinds kind_of_block; //The variable to state the kind

    public GameObject item; //The proper item to be picked
    public GameObject player; //The player who will be picking the object
    public GameObject guide; //Where it needs to be placed once its picked
    public GameObject textGuide; //Text to name the block
    public GameObject default_parent; //Used for blocks to go back to parent once they detach from player

    public int lines = 1; //Number of lines in canvas text

    public bool isPicked; //To check if picked

    public float tossStrength = 600.0f;

    protected Vector3 initial_position;

    private float dumpTime = 0.5f; //Used to set animations of player

    protected void Start()
    {
        initial_position = transform.position;
    }

    //Provisional, will change later TODO
    protected void Awake()
    {
        initial_position = transform.position;
    }

    void Update()
    {
        if (!player.GetComponent<player_controller>().isInputBlocked)
        {
            //Check if the player is in range to hold the item, check if the item is held and if the player has an item.
            if (Vector3.Distance(player.transform.position, item.transform.position) < 2.5f
               && !isPicked
               && !player.GetComponent<player_controller>().isItemHeld
               && Input.GetKeyDown(KeyCode.E))
            {
                pickUp();
            }
            else if (isPicked
                && player.GetComponent<player_controller>().isItemHeld
                && Input.GetKeyDown(KeyCode.E)
                && !player.GetComponent<player_controller>().isInputBlocked)
            {
                pickDown();
            }
            else if (isPicked
                && player.GetComponent<player_controller>().isItemHeld
                && Input.GetKeyDown(KeyCode.F)
                && !player.GetComponent<player_controller>().isInputBlocked)
            {
                toss();
            }
        }

    }

    private void LateUpdate()
    {
        //Update parent position
        if (textGuide != null)
        {
            textGuide.transform.position = transform.position + Vector3.up*(lines);
        }
    }

    //Asigns values needed for the player to release the block
    private void releaseBlock()
    {
        item.GetComponent<Rigidbody>().useGravity = true;
        item.GetComponent<Rigidbody>().isKinematic = false;
        if (default_parent != null)
        {
            item.transform.parent = default_parent.transform;
        }
        else
        {
            item.transform.parent = null;
        }
        isPicked = false;
        StartCoroutine(dropingItem());
    }

    IEnumerator dropingItem()
    {
        yield return new WaitForSeconds(0.2f);
        if (player.GetComponent<player_controller>().held_item == this)
        {
            player.GetComponent<player_controller>().isItemHeld = false;
            player.GetComponent<player_controller>().held_item = null;
            player.GetComponent<player_controller>().player_animator.SetFloat("Blend", 0.0f);
            player.GetComponent<player_controller>().player_animator.SetBool("pickingItem", false);
        }


    }

    public void reset_position()
    {
        transform.position = initial_position + Vector3.up;
    }


    //Ignores or enables collision between player and item.
    //Ignore -> True for enable collisions, False for ignore them.
    public void setCollisions(bool isActive)
    {
        foreach (Transform child in player.transform)
        {
            if (child.tag != "guide")
            {
                if (child.GetComponent<BoxCollider>() != null)
                {
                    Physics.IgnoreCollision(item.GetComponent<BoxCollider>(), child.GetComponent<BoxCollider>(), !isActive);
                }
            }
        }
    }

    //Calls setCollisions after a few seconds
    IEnumerator resetCollision()
    {
        yield return new WaitForSeconds(2.2f);
        setCollisions(true);
        print("Corrutina ok");
    }

    //Picks up the block
    public void pickUp()
    {
        print("Jugador: " + player);
        print("Guia: " + guide);

        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.position = guide.transform.position;
        item.transform.rotation = guide.transform.rotation;
        item.transform.parent = player.transform;
        isPicked = true;
        player.GetComponent<player_controller>().isItemHeld = true;
        player.GetComponent<player_controller>().held_item = this; //To check which item player is carrying



        player.GetComponent<player_controller>().player_animator.SetFloat("Blend", 1.0f);


        player.GetComponent<player_controller>().player_animator.SetBool("pickingItem", true);


    }

    //Picks down the block
    public void pickDown()
    {
        print("Jugador: " + player);
        print("Guia: " + guide);
        releaseBlock();
        item.transform.position = guide.transform.position;
        item.transform.rotation = guide.transform.rotation;
    }

    //Throws the block
    public void toss()
    {
        releaseBlock();
        item.transform.rotation = guide.transform.rotation;
        item.transform.position = guide.transform.position;
        setCollisions(false);
        StartCoroutine(resetCollision());
        Vector3 throwDirection = item.transform.forward + new Vector3(0.0f, 0.5f, 0.0f);
        item.GetComponent<Rigidbody>().AddForce(throwDirection * tossStrength);
    }


}
