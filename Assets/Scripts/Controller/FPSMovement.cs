using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    private float horizontal, vertical;
    public float MoveSpeed;
    public float JumpHeight;
    private bool isInGround;
    Rigidbody rg;
    private void Start()
    {
        rg = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (isInGround)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            Vector3 tmp_CurrentDirection = new Vector3(horizontal, 0, vertical);
            tmp_CurrentDirection = transform.TransformDirection(tmp_CurrentDirection);
            tmp_CurrentDirection *= MoveSpeed;
            var tmp_CurrentVelocity = rg.velocity;
            var tmp_VelocityChange = tmp_CurrentDirection - tmp_CurrentVelocity;
            tmp_VelocityChange.y = 0;

            rg.AddForce(tmp_VelocityChange, ForceMode.VelocityChange);
            if (Input.GetButtonDown("Jump"))
            {
                rg.velocity = new Vector3(tmp_CurrentVelocity.x, Calculate(), tmp_CurrentVelocity.z);
            }
        }
        rg.AddForce(new Vector3(0, -9.8f * rg.mass, 0));

    }
    private float Calculate()
    {
        return Mathf.Sqrt(9.8f * JumpHeight * 2f);
    }
    private void OnCollisionStay(Collision other)
    {
        isInGround = true;

    }

    private void OnCollisionExit(Collision other)
    {
        isInGround = false;
    }
}
