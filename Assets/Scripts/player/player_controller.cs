using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    public int speed;
    public float thrust;

    private Rigidbody rigidbody;
    public bool isJumping; //To check jumping
    public bool isItemHeld; // To avoid picking two items

    public Block held_item; //Item held to check

    void Start()
    {

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY 
            | RigidbodyConstraints.FreezeRotationX 
            | RigidbodyConstraints.FreezeRotationY 
            | RigidbodyConstraints.FreezeRotationZ;
        //This is done to avoid tumbling of the player and changing Y.
        isJumping = false;
        isItemHeld = false;
    }
    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        player_movement();

        if (!isJumping)
        {
            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                rigidbody.constraints = RigidbodyConstraints.FreezeRotationX 
                    | RigidbodyConstraints.FreezeRotationY 
                    | RigidbodyConstraints.FreezeRotationZ;
                //Frees the Y position so it can jump.
                rigidbody.AddForce(Vector3.up * thrust);
            }
        }
    }

    
    /* Checks player controlled movement and jumping*/
    private void player_movement()
    {
        float hMovement = Input.GetAxis("Horizontal") * speed;
        float vMovement = Input.GetAxis("Vertical") * speed;

        Vector3 tempVect = vMovement * transform.forward;
        tempVect = tempVect.normalized * speed * Time.deltaTime;

        Vector3 tempRot = new Vector3(0, hMovement, 0);
        tempRot = tempRot * speed * Time.deltaTime;

        rigidbody.MovePosition(transform.position + tempVect);

        transform.Rotate(tempRot);

        
    }

    /*Checks collision on real time*/
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("wall"))
        {

        }
        else if (collision.gameObject.name.Contains("floor"))
        {//To check if player is still airborne
            isJumping = false;

            rigidbody.constraints = RigidbodyConstraints.FreezePositionY 
                | RigidbodyConstraints.FreezeRotationX
                | RigidbodyConstraints.FreezeRotationY 
                | RigidbodyConstraints.FreezeRotationZ;
        }
        else if (collision.gameObject.tag == "item")
        {
            foreach (Transform child in transform)
            {
                if (child.name == "right_arm")
                {
                    Physics.IgnoreCollision(collision.collider, child.GetComponent<BoxCollider>());
                }
                if (child.name == "left_arm")
                {
                    Physics.IgnoreCollision(collision.collider, child.GetComponent<BoxCollider>());

                }
            }
        }
    }
}
