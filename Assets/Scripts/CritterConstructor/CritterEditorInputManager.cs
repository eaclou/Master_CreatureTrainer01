using UnityEngine;
using System.Collections;

public class CritterEditorInputManager : MonoBehaviour {

    public CritterConstructorManager critterConstructorManager;
    public CritterConstructorCameraController critterConstructorCameraController;
    public CritterEditorUI critterEditorUI;

    private float horizontalInput;
    private float verticalInput;

    private bool mouseLeftIsDown = false;
    private bool mouseMiddleIsDown = false;
    private bool mouseRightIsDown = false;
    private bool altIsDown = false;

    private bool rightClickSegmentMenuOn = false;
    private bool SegmentMenuAddActive = false;

    private Vector3 rayHitPos = new Vector3(0f, 0f, 0f);

    /*private enum EditorState {
        Idle,
        CameraTransform
    };
    private EditorState editorState = EditorState.Idle;
    */

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        CheckInputs();
    }

    private void CheckInputs() {
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        bool leftClick = false;
        bool rightClick = false;

        if (Input.GetKeyDown("left alt") || Input.GetKeyDown("right alt")) {
            altIsDown = true;
        }
        if (Input.GetKeyUp("left alt") || Input.GetKeyUp("right alt")) {
            altIsDown = false;
        }
        if (Input.GetMouseButtonDown(0)) {
            mouseLeftIsDown = true;
            leftClick = true;            
        }
        else if (Input.GetMouseButtonDown(2)) {
            mouseMiddleIsDown = true;
        }
        else if (Input.GetMouseButtonDown(1)) {
            mouseRightIsDown = true;
            rightClick = true;
        }

        if (Input.GetMouseButtonUp(0)) {
            mouseLeftIsDown = false;
        }
        else if (Input.GetMouseButtonUp(2)) {
            mouseMiddleIsDown = false;
        }
        else if (Input.GetMouseButtonUp(1)) {
            mouseRightIsDown = false;
            RightClickUp();
        }        

        // ====================================== Perform Actions: ====================================
        if (Input.GetKeyDown("f")) {
            critterConstructorCameraController.SetFocalPoint(Vector3.zero);
            critterConstructorCameraController.ReframeCamera();
        }
        //UpdateCameraTransform();
        if (altIsDown) { // If in Camera Mode:
            if (mouseMiddleIsDown) {  // PAN
                critterConstructorCameraController.PanCamera(mouseInput);
            }
            if (mouseLeftIsDown) {  // Rotate:
                critterConstructorCameraController.RotateCamera(mouseInput);
            }
            if (mouseRightIsDown) {  // Zoom:
                critterConstructorCameraController.ZoomCamera(mouseInput);
            }
        }
        else {
            if(leftClick) { // mouse was clicked this frame
                LeftClick();  // raycast to see what user clicked on
            }
            if(rightClick) {
                RightClick(mousePosition);
            }
        }
    }

    private void ClickCheckSelected() {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit) {
            Debug.Log("Hit " + hitInfo.transform.gameObject.name);
            if (hitInfo.transform.gameObject != null) {
                critterConstructorManager.selectedObject = hitInfo.transform.gameObject;
                Debug.Log("critterConstructorManager.selectedSegment = " + critterConstructorManager.selectedObject.ToString());
                hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = critterConstructorManager.critterSelectedSegmentMaterial;
                rayHitPos = hitInfo.point;
            }
        }
        else {
            if (critterConstructorManager.selectedObject != null) {
                critterConstructorManager.selectedObject.GetComponent<MeshRenderer>().material = critterConstructorManager.critterSegmentMaterial;
            }
            critterConstructorManager.selectedObject = null;
            Debug.Log("No hit| critterConstructorManager.selectedSegment = " + "critterConstructorManager.selectedObject.ToString()");
        }
    }

    private void LeftClick() {
        ClickCheckSelected();
        critterConstructorManager.UpdateSegmentSelectionVis();
    }

    private void RightClick(Vector2 mousePos) {
        ClickCheckSelected();
        critterConstructorManager.UpdateSegmentSelectionVis();
        Debug.Log("Context Right-Click Menu");
        if (critterConstructorManager.selectedObject != null) {  // Re-Evaluate!
            rightClickSegmentMenuOn = true;
            critterEditorUI.panelRightClickSegmentMenu.GetComponent<RectTransform>().position = new Vector3(mousePos.x, mousePos.y, 0f);
            critterEditorUI.ShowSegmentMenu();

        }            
    }

    private void RightClickUp() {
        //ClickCheckSelected();
        Debug.Log("Context Right-Click Menu OFF");
        rightClickSegmentMenuOn = false;
        if(SegmentMenuAddActive) {
            MenuSegmentAdd();
        }
        critterEditorUI.HideSegmentMenu();
        SegmentMenuAddActive = false;
    }

    public void MenuSegmentAddEnter() {
        Debug.Log("MenuSegmentAddEnter");
        SegmentMenuAddActive = true;
    }
    public void MenuSegmentAddExit() {
        Debug.Log("MenuSegmentAddExit");
        SegmentMenuAddActive = false;
    }

    private void MenuSegmentAdd() {
        Debug.Log("MenuSegmentAdd()");
        critterConstructorManager.AddNewCritterNode(rayHitPos);
    } 
}
