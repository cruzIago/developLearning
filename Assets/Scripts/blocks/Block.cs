using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum kinds {INPUT,PRINT,VARIABLE };

    public kinds kind_of_block;

    public GameObject item;
    public GameObject player;
    public GameObject guide;

    public bool isPicked;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Vector3.Distance(player.transform.position, item.transform.position) < 2f
           && !isPicked
           && !player.GetComponent<player_controller>().isItemHeld
           && Input.GetKeyDown(KeyCode.E))
        {
            print(Vector3.Distance(player.transform.position, item.transform.position));

            pickUp();
        }
        else if (isPicked
            && player.GetComponent<player_controller>().isItemHeld
            && Input.GetKeyDown(KeyCode.E))
        {
            print(Vector3.Distance(player.transform.position, item.transform.position));

            pickDown();
        }
    }

    public virtual void pickUp() { }

    public virtual void pickDown() { }
}
