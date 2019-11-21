using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    public int speed;
    public float thrust;

    private Rigidbody rigidbody;
    public bool isJumping;

    private bool isItemPicked;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        isJumping = false;
        isItemPicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            pickUpItem();
        }
    }

    private void FixedUpdate()
    {
        player_movement();

        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            rigidbody.AddForce(Vector3.up * thrust);
        }
    }

    private void pickUpItem()
    {

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
        }
    }
}
