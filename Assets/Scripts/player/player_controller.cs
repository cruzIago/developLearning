using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    public int speed;
    public int rotSpeed;
    public float thrust;

    //Jump controllers, depending on button hold
    private float fallMultiplierFloat = 2.5f;
    private float lowJumpMultiplierFloat = 2f;

    private Rigidbody rigidbody;
    public bool isJumping; //To check jumping
    public bool isItemHeld; // To avoid picking two items

    public Block held_item; //Item held to check

    public bool isInputBlocked;

    void Start()
    {

        rigidbody = GetComponent<Rigidbody>();
        /*
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY
            | RigidbodyConstraints.FreezeRotationX
            | RigidbodyConstraints.FreezeRotationY
            | RigidbodyConstraints.FreezeRotationZ;*/

        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX
            | RigidbodyConstraints.FreezeRotationY
            | RigidbodyConstraints.FreezeRotationZ;
        //This is done to avoid tumbling of the player and changing Y.
        isJumping = false;
        isItemHeld = false;
        isInputBlocked = false;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        player_movement();

        if (!isJumping)
        {
            if (Input.GetButtonDown("Jump") && !isInputBlocked)
            {
                rigidbody.velocity = Vector3.up * thrust;
                isJumping = true;
            }
        }

        
        if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplierFloat - 1) * Time.deltaTime;
        }
        
        if (rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigidbody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplierFloat - 1) * Time.deltaTime;
        }

    }


    /* Checks player controlled movement and jumping*/
    private void player_movement()
    {
        float hMovement = 0.0f;
        float vMovement = 0.0f; 
        if (!isInputBlocked)
        {
            hMovement = Input.GetAxis("Horizontal") * speed;
            vMovement = Input.GetAxis("Vertical") * speed;

        }
        Vector3 tempVect = vMovement * transform.forward;
        tempVect = tempVect.normalized * speed * Time.deltaTime;

        Vector3 tempRot = new Vector3(0, hMovement, 0);
        tempRot = tempRot * rotSpeed * Time.deltaTime;

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

            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX
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
