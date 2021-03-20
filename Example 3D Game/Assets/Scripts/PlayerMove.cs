using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speedNormal = 12f; // Normal Speed
    public float speedSprint = 20f; // Super Speed
    public float gravity = -19.62f;
    public float jumpHeight = 3f;

    public float turnSmoothTime = 0.2f;

    public CharacterController controller;

    public Transform cam;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;

    private float turnSmoothVelocity;
    private float speed;

    private bool isGrounded;

    private bool isAttacking;

    private Animator anim;

    private void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && !isAttacking)
        {
            speed = speedSprint;
            anim.SetBool("isRunning", false);
            anim.SetBool("isSprinting", true);
        }
        else if (!isAttacking)
        {
            speed = speedNormal;
            anim.SetBool("isSprinting", false);
            anim.SetBool("isRunning", true);
        }
        else
        {
            speed = 0f;
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            anim.SetBool("isIdle", false);
        }
        else
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isSprinting", false);
            anim.SetBool("isIdle", true);
        }

        if(Input.GetButtonDown("Jump") && isGrounded && !isAttacking)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime); // v = (1/2)*g * t^2.
    }

    public void MeeleAttack()
    {
        if (!isAttacking)
        {
            anim.SetTrigger("meeleAttack");
            StartAttack();
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
        Invoke("FinishAttack", 1.2f);
    }
    public void FinishAttack()
    {
        isAttacking = false;
    }
}
