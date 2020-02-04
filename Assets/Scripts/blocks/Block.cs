using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Block parent class. Used to have a common Update method and atributes
 */
public class Block : MonoBehaviour
{
    public enum kinds { INPUT, PRINT, VARIABLE }; // Kind of block to check by the UI

    public kinds kind_of_block; //The variable to state the kind

    public GameObject item; //The proper item to be picked
    public GameObject player; //The player who will be picking the object
    public GameObject guide; //Where it needs to be placed once its picked

    public bool isPicked; //To check if picked

    public float tossStrength = 600.0f;
    
    protected Vector3 initial_position;

    protected void Start()
    {
        initial_position = gameObject.transform.position;
        print(initial_position);
    }

    void Update()
    {
        //Check if the player is in range to hold the item, check if the item is held and if the player has an item.
        if (Vector3.Distance(player.transform.position, item.transform.position) < 2f
           && !isPicked
           && !player.GetComponent<player_controller>().isItemHeld
           && Input.GetKeyDown(KeyCode.E))
        {
            pickUp();
        }
        else if (isPicked
            && player.GetComponent<player_controller>().isItemHeld
            && Input.GetKeyDown(KeyCode.E)
            && Time.timeScale == 1)
        {
            pickDown();
        }
        else if (isPicked
            && player.GetComponent<player_controller>().isItemHeld
            && Input.GetKeyDown(KeyCode.F)
            && Time.timeScale == 1)
        {
            toss();
        }
    }

    //Asigns values needed for the player to release the block
    private void releaseBlock()
    {
        item.GetComponent<Rigidbody>().useGravity = true;
        item.GetComponent<Rigidbody>().isKinematic = false;
        item.transform.parent = null;
        isPicked = false;
        player.GetComponent<player_controller>().isItemHeld = false;
        player.GetComponent<player_controller>().held_item = null;

    }
    public void reset_position() {
        transform.position = initial_position+Vector3.up;
    }
    

    //Ignores or enables collision between player and item.
    //Ignore -> True for ignore collisions, false for enable them.
    private void setCollisions(bool ignore)
    {
        foreach (Transform child in player.transform)
        {
            if (child.tag != "guide")
            {
                Physics.IgnoreCollision(item.GetComponent<BoxCollider>(), child.GetComponent<BoxCollider>(), ignore);
            }
        }
    }

    //Calls setCollisions after a few seconds
    IEnumerator resetCollision()
    {
        yield return new WaitForSeconds(2.2f);
        setCollisions(false);
    }

    //Picks up the block
    public void pickUp() {
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

    }

    //Picks down the block
    public void pickDown() {
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
        setCollisions(true);
        StartCoroutine(resetCollision());
        Vector3 throwDirection = item.transform.forward + new Vector3(0.0f, 0.5f, 0.0f);
        item.GetComponent<Rigidbody>().AddForce(throwDirection * tossStrength);
    }
}
