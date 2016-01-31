using UnityEngine;
using System.Collections;

public class CritterEditorInputManager : MonoBehaviour {

    public CritterConstructorManager critterConstructorManager;
    public CritterConstructorCameraController critterConstructorCameraController;
    public CritterEditorUI critterEditorUI;
    public CritterEditorState critterEditorState;

    private float horizontalInput;
    private float verticalInput;

    public bool mouseLeftClickDown = false;
    public bool mouseMiddleClickDown = false;
    public bool mouseRightClickDown = false;
    public bool mouseLeftClickUp = false;
    public bool mouseMiddleClickUp = false;
    public bool mouseRightClickUp = false;
    public bool keyAltDown = false;
    public bool keyAltUp = false;
    public bool keyFDown = false;

    public void InitKeyPressBools() {
        mouseLeftClickDown = false;
        mouseMiddleClickDown = false;
        mouseRightClickDown = false;
        mouseLeftClickUp = false;
        mouseMiddleClickUp = false;
        mouseRightClickUp = false;
        keyAltDown = false;
        keyAltUp = false;
        keyFDown = false;
    }

    public void CheckInputs() {
        InitKeyPressBools();  // reset key and mouse press bools to false
        // Update Mouse Cursor Information!!
        critterEditorState.SetMouseCursorVelocity(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
        critterEditorState.SetMouseCursorPosition(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        //  Is the alt-key currently ON?
        if (Input.GetKeyDown("left alt") || Input.GetKeyDown("right alt")) {
            keyAltDown = true;
            //critterEditorState.UpdateStateAltDown();
        }
        if (Input.GetKeyUp("left alt") || Input.GetKeyUp("right alt")) {
            keyAltUp = true;
            //critterEditorState.UpdateStateAltUp();
        }        
        if (Input.GetMouseButtonDown(0)) {
            mouseLeftClickDown = true;
            //critterEditorState.UpdateStateLeftClickDown();
        }
        else if (Input.GetMouseButtonDown(2)) {
            mouseMiddleClickDown = true;
            //critterEditorState.UpdateStateMiddleClickDown();
        }        
        else if (Input.GetMouseButtonDown(1)) {
            mouseRightClickDown = true;
            
            //critterEditorState.UpdateStateRightClickDown();
        }
        if (Input.GetMouseButtonUp(0)) {
            mouseLeftClickUp = true;
            //critterEditorState.UpdateStateLeftClickUp();
        }
        else if (Input.GetMouseButtonUp(2)) {
            mouseMiddleClickUp = true;
            //critterEditorState.UpdateStateMiddleClickUp();
        }
        else if (Input.GetMouseButtonUp(1)) {
            mouseRightClickUp = true;
            //critterEditorState.UpdateStateRightClickUp();
        }

        // ====================================== Perform Actions: ====================================
        if (Input.GetKeyDown("f")) {
            //critterConstructorCameraController.SetFocalPoint(Vector3.zero);
            //critterConstructorCameraController.ReframeCamera();
            keyFDown = true;
            //critterEditorState.UpdateStateFDown();
        }

        critterEditorState.UpdateStatesFromInput(this);  // Send keypress information to EditorState to figure out what to do with it
    }

    /*
        //UpdateCameraTransform();
        if (altIsDown) { // If in Camera Mode:
            critterConstructorManager.UpdateSegmentShaderStates();
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
            if (leftClick) { // mouse was clicked this frame
                LeftClick();  // raycast to see what user clicked on
            }
            if (rightClick) {
                RightClick(mousePosition);
            }

            if (!leftClick && !rightClick && !rightClickSegmentMenuOn) {
                //if (hitInfo.transform.gameObject != null) { // turn off previous onMouseOver
                //    hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 0f);
                //}
                critterConstructorManager.UpdateSegmentShaderStates();
                bool hit = CheckMouseRayCast();
                if (hit) {
                    if (hitInfo.transform.gameObject != null) {
                        rayHitPos = hitInfo.point;
                        Vector3 segmentLocalHitPos = new Vector3(0f, 0f, 0f);
                        segmentLocalHitPos.x = Vector3.Dot(rayHitPos - hitInfo.transform.gameObject.transform.position, hitInfo.transform.gameObject.transform.right);
                        segmentLocalHitPos.y = Vector3.Dot(rayHitPos - hitInfo.transform.gameObject.transform.position, hitInfo.transform.gameObject.transform.up);
                        segmentLocalHitPos.z = Vector3.Dot(rayHitPos - hitInfo.transform.gameObject.transform.position, hitInfo.transform.gameObject.transform.forward);
                        hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosX", segmentLocalHitPos.x);
                        hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosY", segmentLocalHitPos.y);
                        hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosZ", segmentLocalHitPos.z);
                        hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 1f);
                        //Debug.Log("Hit SetFloat(_DisplayTarget, 1f)" + rayHitPos.ToString());
                    }
                }
            }
        }
    }
    */

    /*

    private void ClickCheckSelected() {
        //RaycastHit hitInfo = new RaycastHit();
        bool hit = CheckMouseRayCast();
        if (hit) {
            Debug.Log("Hit " + hitInfo.transform.gameObject.name);
            if (hitInfo.transform.gameObject != null) {
                critterConstructorManager.selectedObject = hitInfo.transform.gameObject;
                Debug.Log("critterConstructorManager.selectedSegment = " + critterConstructorManager.selectedObject.ToString());
                //hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = critterConstructorManager.critterSelectedSegmentMaterial;
                hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 1f);
                rayHitPos = hitInfo.point;
            }
        }
        else {
            if (critterConstructorManager.selectedObject != null) {
                critterConstructorManager.selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 0f);
            }
            critterConstructorManager.selectedObject = null;
            Debug.Log("No hit| critterConstructorManager.selectedSegment = " + "critterConstructorManager.selectedObject.ToString()");
        }
    }

    private bool CheckMouseRayCast() {        
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        return hit;
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
            //critterConstructorManager.selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosX", rayHitPos.x - critterConstructorManager.selectedObject.transform.position.x);
            //critterConstructorManager.selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosY", rayHitPos.y - critterConstructorManager.selectedObject.transform.position.y);
            //critterConstructorManager.selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosZ", rayHitPos.z - critterConstructorManager.selectedObject.transform.position.z);
            //critterConstructorManager.selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 1f);
            //Debug.Log("CCC: " + critterConstructorManager.selectedObject.GetComponent<MeshRenderer>().material.GetFloat("_TargetX").ToString());
        }            
    }

    private void RightClickUp() {
        //ClickCheckSelected();
        Debug.Log("Context Right-Click Menu OFF");
        rightClickSegmentMenuOn = false;
        if(SegmentMenuAddActive) {            
            MenuSegmentAdd();
        }
        if (SegmentMenuScaleActive) {
            MenuSegmentScale();
        }
        critterEditorUI.HideSegmentMenu();
        if (critterConstructorManager.selectedObject != null) {  // Re-Evaluate!
            //critterConstructorManager.selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 0f);
        }
        SegmentMenuAddActive = false;
        SegmentMenuScaleActive = false;
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

    public void MenuSegmentScaleEnter() {
        Debug.Log("MenuSegmentScaleEnter");
        SegmentMenuScaleActive = true;
    }
    public void MenuSegmentScaleExit() {
        Debug.Log("MenuSegmentScaleExit");
        SegmentMenuScaleActive = false;
    }

    private void MenuSegmentScale() {
        Debug.Log("MenuSegmentScale()");
        critterConstructorManager.ScaleCritterNode();
    }
    */
}
