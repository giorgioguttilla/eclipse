using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public PlayerShipController shipController;
    public float tiltAmount = 0.5f;
    private Rigidbody shipRb;

    // Start is called before the first frame update
    void Start()
    {
        shipRb = shipController.gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if(Cursor.lockState == CursorLockMode.None && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(!(shipController == null))
        {
            shipController.Roll(-Input.GetAxis("Roll"));
            shipController.Pitch(-Input.GetAxis("Mouse Y"));
            shipController.Yaw(Input.GetAxis("Mouse X"));

            if (Input.GetMouseButton(0))
            {
                shipController.ShootAmmo();
            }

            if (Input.GetMouseButton(1))
            {
                shipController.ShootMissile();
            }


            shipController.Accelerate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));

            if (shipRb)
            {
                Vector3 localVelocity = shipController.transform.InverseTransformDirection(shipRb.velocity);
                Camera.main.transform.localEulerAngles = (new Vector3(localVelocity.z, 0, localVelocity.x) / shipController.speed) * -1.0f * tiltAmount;
                
            }
            
        }
    }
}
