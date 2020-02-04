using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Block parent class. Used to have a common Update method and atributes
 */
public class Block : MonoBehaviour
{
    public enum kinds {INPUT,PRINT,VARIABLE }; // Kind of block to check by the UI

    public kinds kind_of_block; //The variable to state the kind

    public GameObject item; //The proper item to be picked
    public GameObject player; //The player who will be picking the object
    public GameObject guide; //Where it needs to be placed once its picked

    public bool isPicked; //To check if picked

    private Vector3 initial_position;

    void Start()
    {
        initial_position = transform.position;    
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
            && Time.timeScale==1)
        {
            pickDown();
        }
    }

    public void reset_position() {
        transform.position = initial_position;
    }

    public virtual void pickUp() { }

    public virtual void pickDown() { }
}
