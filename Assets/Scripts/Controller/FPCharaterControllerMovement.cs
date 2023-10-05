using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCharaterControllerMovement : MonoBehaviour
{

    CharacterController characterController;
    Animator characteranimator;
    public float WalkSpeed;
    public float RunSpeed;
    public float SquatSpeed;
    public float JumpHeight;
    private bool isSquated;
    private float velocity;
    private Vector3 movementDirection;
    public Transform childTr;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characteranimator = GetComponentInChildren<Animator>();
        isSquated = false;
    }

    // Update is called once per frame
    void Update()
    {

        float currentSpeed = WalkSpeed;
        if (characterController.isGrounded)
        {
            var horizontal = Input.GetAxis("Horizontal");
            var Vertical = Input.GetAxis("Vertical");
            movementDirection.y = childTr.transform.rotation.y;
            movementDirection = transform.TransformDirection(new Vector3(horizontal, 0, Vertical));
            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;
            if (Input.GetButtonDown("Jump"))
            {
                movementDirection.y = JumpHeight;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                float t = 0;
                float tempSpeed = currentSpeed;
                t += 0.5f * Time.deltaTime;
                if (!isSquated)
                {
                    characterController.height = Mathf.Lerp(2, 1, t);
                    currentSpeed = SquatSpeed;
                }
                else
                {
                    characterController.height = Mathf.Lerp(1, 2, t);
                    currentSpeed = tempSpeed;
                }
                if (t > 1.0f)
                {
                    t = 1.0f;
                }
                isSquated = !isSquated;
            }
            var tmp_velocity = characterController.velocity;
            tmp_velocity.y = 0;
            velocity = tmp_velocity.magnitude;
            // Debug.Log(velocity);
            // characteranimator.SetFloat("Velocity", velocity);
            characteranimator.SetFloat("Velocity", velocity, 0.1f, Time.deltaTime);

            //  characteranimator.SetFloat("velocity", velocity, 0.25f, Time.deltaTime);
        }

        movementDirection.y -= 9.8f * Time.deltaTime;
        characterController.Move(movementDirection * Time.deltaTime * currentSpeed);
    }
}
