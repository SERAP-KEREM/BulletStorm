using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;
    public float maxVelocityChange = 10f;
    public float airControl = 0.5f;
    public float jumpHeight = 10f;
    public float currentSpeed;

    private Vector2 input;
    private Rigidbody rb;
    private bool sprinting;
    private bool jumping;
    private bool grounded = false;

    private Animator playerAnim;

   
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        GameObject weaponObject = GameObject.FindGameObjectWithTag("Weapon");
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();

        sprinting = Input.GetButton("Sprint");
        jumping = Input.GetButton("Jump");

        // Speed parametresini ayarla
         currentSpeed = rb.velocity.magnitude;
      //  playerAnim.SetFloat("Speed", currentSpeed);

        // Jump tetikleyici
        if (jumping && grounded)
        {
        //    playerAnim.SetBool("isJump",true);
        }

     

        if (Input.GetKeyDown(KeyCode.K)) // Ölüm animasyonu için bir tetikleyici, oyun mekaniklerine göre değiştirin
        {
           // playerAnim.SetBool("isDie", true);
        }


    }

    private void OnTriggerStay(Collider other)
    {
        grounded = true;
        Debug.Log("Yerde");
    }

    private void FixedUpdate()
    {
        if (grounded)
        {
            if (jumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight / 2, rb.velocity.z);
            }
            else if (input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed : walkSpeed), ForceMode.VelocityChange);
            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }
        else
        {
            if (input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed * airControl : walkSpeed * airControl), ForceMode.VelocityChange);
            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }

        grounded = false;
    }

    Vector3 CalculateMovement(float _speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y).normalized;
        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocity = rb.velocity;

        if (input.magnitude > 0.5f)
        {
            Vector3 velocityChange = (targetVelocity * _speed) - velocity;

            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            return velocityChange;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
