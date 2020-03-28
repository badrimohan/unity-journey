using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementController : MonoBehaviour
{
    // public GameObject character;
    public float moveSpeed = 50;
    [SerializeField]
    private float jumpVelocity = 4;
    private Rigidbody rb;
    private float pitch = 0.0f;
    private float yaw = 0.0f;
    public float rotateSpeed = 10;
    public Transform cam;

    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody>();
        // cam = gameObject.transform.parent.GetChild(0);
    }
    private void Update() {
        // pitch -= Input.GetAxis("Mouse X");
        // yaw += Input.GetAxis("Mouse X");
        pitch = Input.GetAxis("Mouse Y");
        yaw = Input.GetAxis("Mouse X");
        rb.transform.Rotate(new Vector3(0.0f, yaw, 0.0f) * rotateSpeed, Space.World);
    }
    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        

        // gameObject.transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime);
        //Move the object to XYZ coordinates defined as horizontalInput, 0, and verticalInput respectively.
        // Debug.Log((pitch, yaw));
        rb.AddForce(cam.forward * moveSpeed * verticalInput, ForceMode.Impulse);
        rb.AddForce(cam.right * moveSpeed * horizontalInput, ForceMode.Impulse);
        // rb.AddForce(new Vector3(horizontalInput,0,verticalInput) * moveSpeed, ForceMode.Impulse);
        // Debug.Log(cam);
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
        }

    }
    // private void FixedUpdate() {
    //      if (Input.GetButtonDown("Jump"))
    //     {
    //         // IEnumerator jumpCoroutine = jumpAction(2.0f);
    //         // StartCoroutine(jumpCoroutine);
    //         Debug.Log("Entered jump");
    //         rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
    //     }
        
    // }

    // private IEnumerator jumpAction(float waitTime)
    // {
    //     character.transform.Translate(new Vector3(0, jumpHeight, 0) * moveSpeed * Time.deltaTime);
    //     yield return new WaitForSeconds(waitTime);
    // }
}
