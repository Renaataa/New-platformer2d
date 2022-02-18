using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    public float speed;
    public float jumpForce;
    private float inputMove;

    private Rigidbody2D rigidbody;
    private bool faceRight = true;
    public bool isGrounded;
    

    private void Start(){
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate(){
        inputMove = joystick.Horizontal;
        rigidbody.velocity = new Vector2(inputMove*speed, rigidbody.velocity.y);

        if(faceRight == false && inputMove > 0){
            Flip();
        } else if(faceRight == true && inputMove < 0){
            Flip();
        }
    }

    private void Update(){
        if(isGrounded == true && joystick.Vertical > 0.3){
            rigidbody.velocity = Vector2.up * jumpForce;
        }
    }
    void Flip(){
        faceRight = !faceRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
