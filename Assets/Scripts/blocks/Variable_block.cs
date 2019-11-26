using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variable_block : Block
{

    void Start()
    {
        item = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        guide = GameObject.FindGameObjectWithTag("guide");
        kind_of_block = kinds.VARIABLE;
        isPicked = false;
    }
    

    override public void pickUp() {

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

    public override void pickDown()
    {
        print("Jugador: " + player);
        print("Guia: " + guide);
        item.GetComponent<Rigidbody>().useGravity = true;
        item.GetComponent<Rigidbody>().isKinematic = false;
        item.transform.position = guide.transform.position;
        item.transform.rotation = guide.transform.rotation;
        item.transform.parent = null;
        isPicked = false;
        player.GetComponent<player_controller>().isItemHeld = false;
        player.GetComponent<player_controller>().held_item = null;
    }
}
