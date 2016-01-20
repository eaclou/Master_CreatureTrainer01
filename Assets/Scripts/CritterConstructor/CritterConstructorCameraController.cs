using UnityEngine;
using System.Collections;

public class CritterConstructorCameraController : MonoBehaviour {

    public float turningSpeed = 0.1f;
    public float movementSpeed = 0.2f;

    private float horizontalInput;
    private float verticalInput;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        horizontalInput = Input.GetAxis("Mouse X");
        transform.Rotate(0, horizontalInput * turningSpeed * Time.deltaTime, 0);

        verticalInput = Input.GetAxis("Mouse Y");
        transform.Translate(0, 0, verticalInput * movementSpeed * Time.deltaTime);

        Debug.Log("Horizontal= " + horizontalInput.ToString() + ", Vertical= " + verticalInput.ToString());
    }
}
