//Colin Klein - MovementScript
//This script is used in 2D platformers to enable gradual horizontal movement and allow a "Mario"
//  style jumping system. Assign this script to the player object and assign the platforms to 
//  the correct layer/desigination

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Announcing Variables
    private Rigidbody2D body;
    public int speed;
    public int jumpPower;
    private float horizontalInput;
    public LayerMask layer;
    public float maxDistance;
    public Vector3 boxSize;
    private bool Falling = false;

    private void Awake() //Gather RigidBody from the player
    {
        body = GetComponent<Rigidbody2D>();
    }
    void Start() // Assign gravity to the player from the start
    {
        body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f);
    }
    private void FixedUpdate() // Assign the x-axis movement to the horizontal input from the player
    {
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        Run();
        Debug.Log(IsGrounded());
    }
    private void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded()) //Jump if the player presses a jump button and the player is touching the ground
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower); //The player is moved on the x axis as normal and moved on the y axis based on the jumpPower value that has been assigned
        }
        if (Input.GetButtonUp("Jump") && !Falling) //Begin gravity/falling if the player lets go of the button and they are not already falling
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f); //Assigns the players y-axis to an exponentially increasing value to simulate gravity
            Debug.Log("Falling now.");
            Falling = true; //Tick falling true to disable the statement from retriggering
        }
        if (IsGrounded())
        {
            Falling = false; //Ticks falling false to reenable the statement after landing
        }
    }
    private void Run() //Pulls the horizontal input to allow universal control support
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }
    private bool IsGrounded()
    {
        if(Physics2D.BoxCast(transform.position,boxSize,0,-transform.up,maxDistance,layer)) //Fires a box cast of the users size/distance selected to detect the layer specified
        {
            return true; //Detects a collision of 'layer'
        }
        else{
            return false; //No longer detects a collision of 'layer'
        }
    }
    void OnDrawGizmos() //Simply draws a box of equal size to the box cast above
    {
        Gizmos.color=Color.red;
        Gizmos.DrawCube(transform.position-transform.up*maxDistance,boxSize);
    }
}
