using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 12f;
    public Animator animator;

    public CharacterController controller;
    // Start is called before the first frame update
 

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        animator.SetFloat("x", x);
        animator.SetFloat("y", z);
    }
    private void FixedUpdate()
    {
        controller.Move(Vector3.up * -9.81f);
    }
}
