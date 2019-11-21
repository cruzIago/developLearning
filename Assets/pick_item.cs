using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pick_item : MonoBehaviour
{
    public GameObject item;
    public GameObject player;
    public GameObject guide;

    private bool isPicked;
    // Start is called before the first frame update
    void Start()
    {
        isPicked = false;
        player = GameObject.FindGameObjectWithTag("Player");
        guide = GameObject.FindGameObjectWithTag("guide");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(player.transform.position,item.transform.position)<2f && !isPicked && Input.GetKeyDown(KeyCode.E))
        {
            print(Vector3.Distance(player.transform.position, item.transform.position));
           
            pickUp();
        }
        else if (isPicked && Input.GetKeyDown(KeyCode.E))
        {
            print(Vector3.Distance(player.transform.position, item.transform.position));

            pickDown();
        }
    }

    void pickUp()
    {

        print("Jugador: " + player);
        print("Guia: " + guide);

        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.position = guide.transform.position;
        item.transform.rotation = guide.transform.rotation;
        item.transform.parent = player.transform;
        isPicked = true;
    }

    void pickDown()
    {
        print("Jugador: "+player);
        print("Guia: "+guide);
        item.GetComponent<Rigidbody>().useGravity = true;
        item.GetComponent<Rigidbody>().isKinematic = false;
        item.transform.position = guide.transform.position;
        item.transform.rotation = guide.transform.rotation;
        item.transform.parent = null;
        isPicked = false;
    }
}
