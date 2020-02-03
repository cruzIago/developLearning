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

    public float tossStrength = 0.0f;

    void Start()
    {

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
        Vector3 throwDirection = item.transform.forward + new Vector3(0.0f, 2.2f, 0.0f);
        item.GetComponent<Rigidbody>().AddForce(throwDirection * tossStrength);
    }
}
