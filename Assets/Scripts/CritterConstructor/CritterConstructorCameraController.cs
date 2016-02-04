using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CritterConstructorCameraController : MonoBehaviour {

    public float defaultMoveSpeed = 20f;
    public float defaultTurningSpeed = 400f;    
    private float maxMouseSpeed = 100f;    
    private Vector3 cameraFocalPoint = new Vector3(0f, 0f, 0f);

    // Use this for initialization
    void Start () {
        //movementSpeed = defaultMoveSpeed;
        
    }
	
	// Update is called once per frame
	void Update () {        

        CheckForInput();
        //Debug.Log("Horizontal= " + horizontalInput.ToString() + ", Vertical= " + verticalInput.ToString());
    }

    private void CheckForInput() {
        
        
    }

    public void ReframeCamera() {
        //Debug.Log("Reframe Camera()");
        Vector3 forward = this.gameObject.transform.forward;
        Vector3 cameraPosition = cameraFocalPoint - forward * 5f;
        transform.position = cameraPosition;
    }

    public void SetFocalPoint(Vector3 focus) {
        cameraFocalPoint = focus;
    }
    public void SetFocalPoint(GameObject selectedSegment) {
        if(selectedSegment != null) {  // if something selected
            cameraFocalPoint = selectedSegment.transform.position;  // focus cam on selectedSegment
        }
        else {  // nothing selected, default to origin
            cameraFocalPoint = new Vector3(0f, 0f, 0f);
        }
        ReframeCamera(); // reframe the camera
    }

    public void UpdateCamera(CritterEditorState editorState) {
        if (editorState.GetCurrentCameraState() == CritterEditorState.CurrentCameraState.Pan) {  // PAN
            PanCamera(editorState.mouseInput);
        }
        if (editorState.GetCurrentCameraState() == CritterEditorState.CurrentCameraState.Rotate) {  // Rotate:
            RotateCamera(editorState.mouseInput);
        }
        if (editorState.GetCurrentCameraState() == CritterEditorState.CurrentCameraState.Zoom) {  // Zoom:
            ZoomCamera(editorState.mouseInput);
        }
    }

    public void PanCamera(Vector2 mouseInput) {
        Vector2 mouseDirection = mouseInput.normalized;
        float mouseSpeed = Mathf.Min(mouseInput.magnitude, maxMouseSpeed);
        
        Vector3 moveDirection = Vector3.Normalize(this.gameObject.transform.right * -mouseDirection.x + this.gameObject.transform.up * -mouseDirection.y);
        Vector3 moveOffset = moveDirection * defaultMoveSpeed * mouseSpeed * Time.deltaTime;
        Vector3 cameraPosition = transform.position += moveOffset;
        transform.position = cameraPosition;
        cameraFocalPoint += moveOffset;            
    }

    public void RotateCamera(Vector2 mouseInput) {
        Vector2 mouseDirection = mouseInput.normalized;
        float mouseSpeed = Mathf.Min(mouseInput.magnitude, maxMouseSpeed);

        Vector3 axisToFocalPoint = cameraFocalPoint - transform.position;
        Vector3 mouseRotationTangent = Vector3.Normalize(this.gameObject.transform.right * mouseDirection.x + this.gameObject.transform.up * mouseDirection.y);
        Vector3 axisOfRotation = Vector3.Cross(axisToFocalPoint, mouseRotationTangent).normalized;
        Quaternion cameraRotation = Quaternion.AngleAxis(defaultTurningSpeed * mouseSpeed * Time.deltaTime, axisOfRotation);
        Vector3 cameraPosition = cameraFocalPoint + cameraRotation * (this.transform.position - cameraFocalPoint);
        transform.position = cameraPosition;
        transform.LookAt(cameraFocalPoint);
    }

    public void ZoomCamera(Vector2 mouseInput) {
        Vector2 mouseDirection = mouseInput.normalized;
        float mouseSpeed = Mathf.Min(mouseInput.magnitude, maxMouseSpeed);

        Vector3 moveDirection = Vector3.Normalize(this.gameObject.transform.forward * Vector2.Dot(mouseDirection, new Vector2(1f, 1f)));
        Vector3 moveOffset = moveDirection * defaultMoveSpeed * mouseSpeed * Time.deltaTime;
        Vector3 cameraPosition = transform.position += moveOffset;
        transform.position = cameraPosition;
        //cameraFocalPoint += moveOffset;
    }

    private void UpdateCameraTransform() {
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector2 mouseDirection = mouseInput.normalized;
        float mouseSpeed = Mathf.Min(mouseInput.magnitude, maxMouseSpeed);
        /*
        if (altIsDown) { // If in Camera Mode:
            if (mouseMiddleIsDown) {  // PAN
                Vector3 moveDirection = Vector3.Normalize(this.gameObject.transform.right * -mouseDirection.x + this.gameObject.transform.up * -mouseDirection.y);
                Vector3 moveOffset = moveDirection * defaultMoveSpeed * mouseSpeed * Time.deltaTime;
                Vector3 cameraPosition = transform.position += moveOffset;
                transform.position = cameraPosition;
                cameraFocalPoint += moveOffset;
            }
            if(mouseLeftIsDown) {  // Rotate:
                Vector3 axisToFocalPoint = cameraFocalPoint - transform.position;
                Vector3 mouseRotationTangent = Vector3.Normalize(this.gameObject.transform.right * mouseDirection.x + this.gameObject.transform.up * mouseDirection.y);
                Vector3 axisOfRotation = Vector3.Cross(axisToFocalPoint, mouseRotationTangent).normalized;
                Quaternion cameraRotation = Quaternion.AngleAxis(defaultTurningSpeed * mouseSpeed * Time.deltaTime, axisOfRotation);
                Vector3 cameraPosition = cameraFocalPoint + cameraRotation * (this.transform.position - cameraFocalPoint);
                transform.position = cameraPosition;
                transform.LookAt(cameraFocalPoint);
            }
            if (mouseRightIsDown) {  // Zoom:
                Vector3 moveDirection = Vector3.Normalize(this.gameObject.transform.forward * Vector2.Dot(mouseDirection, new Vector2(1f, 1f)));
                Vector3 moveOffset = moveDirection * defaultMoveSpeed * mouseSpeed * Time.deltaTime;
                Vector3 cameraPosition = transform.position += moveOffset;
                transform.position = cameraPosition;
                cameraFocalPoint += moveOffset;
                //Debug.Log(Time.time.ToString() + ", moveDirection: " + moveDirection.ToString());
            }
        }
        */
    }
}
