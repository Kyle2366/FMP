using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public GameObject player;
    public float mouseSensitivity = 100f;
    public Transform playerBod;
    PlayerCombat PC;
    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        PC = GetComponent<PlayerCombat>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<PlayerCombat>().canMove)
        {
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerBod.Rotate(Vector3.up * mouseX);
        }
        
    }
}
