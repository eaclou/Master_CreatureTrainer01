using UnityEngine;
using System.Collections;

public class CritterEditorState : MonoBehaviour {

    public CritterEditorUI critterEditorUI;
    public CritterConstructorManager critterConstructorManager;

    private Vector3 mouseRayHitPos = new Vector3(0f, 0f, 0f);
    private RaycastHit mouseRayHitInfo; // = new RaycastHit();
    private Vector3 gizmoRayHitPos = new Vector3(0f, 0f, 0f);
    private RaycastHit gizmoRayHitInfo; // = new RaycastHit();
    public Vector2 mouseInput = new Vector3(0f, 0f, 0f);
    public Vector2 mousePosition = new Vector3(0f, 0f, 0f);
    public Vector3 rightClickWorldPosition = new Vector3(0f, 0f, 0f);

    private enum CurrentToolState {
        None,
        ScaleSegment,
        MoveAttachPoint
    };
    private CurrentToolState currentToolState = CurrentToolState.None;

    public enum CurrentCameraState {
        Off,
        Static,
        Pan,
        Rotate,
        Zoom
    };
    public CurrentCameraState currentCameraState = CurrentCameraState.Off;

    public GameObject selectedSegment;  // the CritterSegment that is currently selected
    public bool isSegmentSelected = false;
    public int selectedSegmentID = -1;
    public GameObject hoverSegment;
    public bool isSegmentHover = false; // is the mouse hovering over a critterSegment on this frame?
    public int hoverSegmentID = -1;
    public GameObject engagedGizmo;  // the gizmo object/sub-object that is currently being manipulated
    public bool isGizmoEngaged = false;  // is the user currently interacting with a Gizmo?
    public GameObject hoverGizmo;  // the gizmo GameObject that the mouse is currently hovering over
    public bool isGizmoHover = false;  // is the mouse cursor currently hovering over a Gizmo?

    private bool rightClickMenuOn = false;  // is the right-click context-driven menu currently active?
    private bool rightClickMenuAddHover = false;  // is the mouse cursor currently over the 'Add Segment' button?
    private bool rightClickMenuScaleHover = false;
    private bool rightClickMenuDeleteHover = false;
    
    //   v v v (not necessarily pressed this frame, it could be being held) v v v 
    private bool mouseLeftIsDown = false;
    private bool mouseMiddleIsDown = false;
    private bool mouseRightIsDown = false;
    private bool altIsDown = false;  // is the alt key in a down state? (not necessarily pressed this frame, it could be being held)

    public CurrentCameraState GetCurrentCameraState() {
        return currentCameraState;
    }

    public void SetMouseCursorPosition(Vector3 newPos) {
        mousePosition = newPos;
    }
    public void SetMouseCursorVelocity(Vector3 newVel) {
        mouseInput = newVel;
    }

    public void UpdateStatesFromInput(CritterEditorInputManager critterEditorInputManager) {
        //Debug.Log("UpdateStatesFromInput(CritterEditorInputManager critterEditorInputManager)");
        
        CheckKeyPresses(critterEditorInputManager);
        UpdateCameraState(critterEditorInputManager);

        // Check for updated States:
        if (currentCameraState == CurrentCameraState.Off) {  // if Not in CAMERA mode, then check for other stuff:
            
            // Check Right-click menu state:
            //======= If Right-click menu is on: ======================================================
            if (rightClickMenuOn) {
                // don't need to check for raycasts, just check if mouse is over a button
                if (critterEditorInputManager.mouseRightClickUp) {  // right click menu was exited this frame:
                    CommandRCMenuOff();
                    critterEditorUI.buttonDisplayRCMenuMode.interactable = true;
                }
            }
            else {   // right-click menu OFF
                // Check for Gizmos:
                ShootGizmoRaycast();
                // Check for Segment:
                ShootSegmentRaycast();
                UpdateHoverState();  // Using raycast information, determine the status of the hover state -- did mouse cursor enter, exit, or switch hover targets?

                if (critterEditorInputManager.mouseRightClickDown) {  // right-clicked this frame
                    // right click menu was entered this frame:
                    CommandRCMenuOn();
                    critterEditorUI.buttonDisplayRCMenuMode.interactable = false;
                }

                if (critterEditorInputManager.mouseLeftClickDown) {  // left-clicked this frame
                    if(isSegmentHover) {  //if left-Click on a segment
                        if(isSegmentSelected) {
                            SetSegmentMaterialSelected(selectedSegment.GetComponent<CritterSegment>(), false);  // De-select old segment!!
                        }
                        selectedSegment = hoverSegment;
                        Debug.Log("new selectedSegment = " + selectedSegment.GetComponent<CritterSegment>().ToString());
                        selectedSegmentID = selectedSegment.GetComponent<CritterSegment>().id;
                        isSegmentSelected = true;
                        SetSegmentMaterialSelected(selectedSegment.GetComponent<CritterSegment>(), true);
                    }
                    else {  // left-clicked on emptiness:
                        
                        if (isSegmentSelected) {
                            Debug.Log("OLD selectedSegment = " + selectedSegment.GetComponent<CritterSegment>().ToString());
                            SetSegmentMaterialSelected(selectedSegment.GetComponent<CritterSegment>(), false);  // De-select old segment!!
                        }
                        selectedSegment = null;
                        isSegmentSelected = false;
                    }
                }
            }            
        }
        else {
            //Debug.Log("Camera Mode ON");
        }

        if(currentToolState == CurrentToolState.None) {
            critterEditorUI.buttonDisplayView.interactable = false;
        }
        else {
            critterEditorUI.buttonDisplayView.interactable = true;
        }
        if (currentToolState == CurrentToolState.ScaleSegment) {
            critterEditorUI.buttonDisplayScale.interactable = false;
        }
        else {
            critterEditorUI.buttonDisplayScale.interactable = true;
        }
        if (currentToolState == CurrentToolState.MoveAttachPoint) {
            critterEditorUI.buttonDisplayMoveJoint.interactable = false;
        }
        else {
            critterEditorUI.buttonDisplayMoveJoint.interactable = true;
        }
    }

    private void SetHoverAndSelectedFromID() {
        //Debug.Log(critterConstructorManager.masterCritter.critterSegmentList.ToString() + "#: " + critterConstructorManager.masterCritter.critterSegmentList.Count.ToString());
        if (isSegmentSelected) {
            selectedSegment = critterConstructorManager.masterCritter.critterSegmentList[selectedSegmentID];
        }
        if (isSegmentHover) {
            hoverSegment = critterConstructorManager.masterCritter.critterSegmentList[hoverSegmentID];
        }
    }

    private void CheckKeyPresses(CritterEditorInputManager critterEditorInputManager) {
        if (critterEditorInputManager.keyFDown) {  // pressing 'f' centers the camera on the currently selected segment, or entire critter is nothing selected
            critterEditorInputManager.critterConstructorCameraController.SetFocalPoint(selectedSegment);
            critterEditorInputManager.critterConstructorCameraController.ReframeCamera();
        }
        if (critterEditorInputManager.keyAltDown) {  // pressing 'alt' enters camera mode  -- any clicks will control camera and nothing else
            altIsDown = true;
            //currentCameraState = CurrentCameraState.Static;  // alt is down -- but no mouse-click -- 
        }
        if (critterEditorInputManager.keyAltUp) {  // letting go of alt exits camera mode
            altIsDown = false;
        }
        if (critterEditorInputManager.mouseLeftClickDown) {  // left-clicked this frame
            mouseLeftIsDown = true;
        }
        if (critterEditorInputManager.mouseLeftClickUp) {  // released the left-click
            mouseLeftIsDown = false;
        }
        if (critterEditorInputManager.mouseMiddleClickDown) {  // Middle-clicked this frame
            mouseMiddleIsDown = true;
        }
        if (critterEditorInputManager.mouseMiddleClickUp) {  // released the Middle-click
            mouseMiddleIsDown = false;
        }
        if (critterEditorInputManager.mouseRightClickDown) {  // Right-clicked this frame
            mouseRightIsDown = true;

        }
        if (critterEditorInputManager.mouseRightClickUp) {  // released the Right-click
            mouseRightIsDown = false;
        }
    }

    private void UpdateCameraState(CritterEditorInputManager critterEditorInputManager) {
        // CAMERA:
        // Determine Camera Mode:
        if (altIsDown) {
            currentCameraState = CurrentCameraState.Static; // default
            if (mouseMiddleIsDown) {  // PAN
                currentCameraState = CurrentCameraState.Pan;
            }
            if (mouseLeftIsDown) {  // Rotate:
                currentCameraState = CurrentCameraState.Rotate;
            }
            if (mouseRightIsDown) {  // Zoom:
                currentCameraState = CurrentCameraState.Zoom;
            }
            critterEditorUI.buttonDisplayCameraMode.interactable = false;
            if (isSegmentHover) {                
                hoverSegment.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 0f);  // turn on selected vis
            }
        }
        else {  // not in camera mode:
            currentCameraState = CurrentCameraState.Off;
            if (mouseMiddleIsDown) {  // middle w/e

            }
            if (mouseLeftIsDown) {  // Left-Click:
                // Check For Selecting
            }
            if (mouseRightIsDown) {  // Right-Click:

            }
            critterEditorUI.buttonDisplayCameraMode.interactable = true;
        }
        critterEditorInputManager.critterConstructorCameraController.UpdateCamera(this);  // pan, zoom, rotate, or do nothing
    }

    #region Command Functions

    private void CommandLeftClickNone() {

    }

    private void CommandRightClickNone() {

    }

    private void CommandHoverSegmentEnter() {
        hoverSegment = mouseRayHitInfo.transform.gameObject;  // Set as new hoverSegment
        isSegmentHover = true;
        hoverSegmentID = hoverSegment.GetComponent<CritterSegment>().id;
        SetSegmentMaterialHover(hoverSegment.GetComponent<CritterSegment>(), true);
    }
    private void CommandHoverSegmentStay() {

    }
    private void CommandHoverSegmentSwitch() {
        SetSegmentMaterialHover(hoverSegment.GetComponent<CritterSegment>(), false);  // Set OLD hoverSegment hover to false
        hoverSegment = mouseRayHitInfo.transform.gameObject;  // Set NEW hoverSegment
        isSegmentHover = true;
        hoverSegmentID = hoverSegment.GetComponent<CritterSegment>().id;
        SetSegmentMaterialHover(hoverSegment.GetComponent<CritterSegment>(), true);  // Set NEW hoverSegment hover to true
    }
    private void CommandHoverSegmentExit() {
        // Went from over Segment to over Nothing:
        // Clean up current hoverSegment:
        //Debug.Log("CommandHoverSegmentExit()" + hoverSegment.ToString());
        SetSegmentMaterialHover(hoverSegment.GetComponent<CritterSegment>(), false);
        // update hover segment info:
        hoverSegment = null;  // Mouse is not hovering over anything
        isSegmentHover = false;
    }

    private void CommandLeftClickSegment() {

    }

    private void CommandRightClickSegment() {

    }

    private void CommandLeftClickGizmo() {

    }

    private void CommandRCMenuOn() {
        
        if (isSegmentHover) {
            if (isSegmentSelected) {  // DE-SELECT OLD if another segment was selected:
                //Debug.Log("OLD selectedSegment = " + selectedSegment.GetComponent<CritterSegment>().ToString());
                SetSegmentMaterialSelected(selectedSegment.GetComponent<CritterSegment>(), false);  // De-select old segment!!
            }
            selectedSegment = hoverSegment;
            isSegmentSelected = true;
            selectedSegmentID = selectedSegment.GetComponent<CritterSegment>().id;
            selectedSegment.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 1f);  // turn on selected vis
            rightClickWorldPosition = mouseRayHitPos;
        }
        else {  // mouse is positioned over Nothing at all!!
            if (isSegmentSelected) {  // DE-SELECT
                //Debug.Log("OLD selectedSegment = " + selectedSegment.GetComponent<CritterSegment>().ToString());
                SetSegmentMaterialSelected(selectedSegment.GetComponent<CritterSegment>(), false);  // De-select old segment!!
            }
            selectedSegment = null;
            isSegmentSelected = false;
        }        
        //  display menu; init buttons
        OpenRightClickMenu();
    }

    private void CommandRCMenuOff() {
        //Debug.Log("CommandRCMenuOff()" + hoverSegment.ToString());
        // Check to see if any buttons were selected:
        if (rightClickMenuAddHover) {
            RightClickMenuSegmentAdd();
        }
        if (rightClickMenuScaleHover) {
            RightClickMenuSegmentScale();
        }
        // If so, perform actions:

        CloseRightClickMenu(); 
        
    }
    #endregion

    private void SetSegmentMaterialHover(CritterSegment segment, bool on) {
        // bool on true=is hovering;   false=not hovering
        if(on) {
            hoverSegment.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 1f);  // turn on crosshairs
        }
        else {
            hoverSegment.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 0f);  // turn off crosshairs
        }
    }
    private void SetSegmentMaterialSelected(CritterSegment segment, bool on) {
        if(on) {
            selectedSegment.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 1f);  // turn on selected
        }
        else {
            selectedSegment.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 0f);  // de-select
        }
    }

    private void ShootGizmoRaycast() {
        //RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out gizmoRayHitInfo);  // ADD LAYERMASK FOR GIZMO OBJECTS
        if (hit) {
            //Debug.Log("Hit " + gizmoRayHitInfo.transform.gameObject.name);
            if (gizmoRayHitInfo.transform.gameObject != null) {
                gizmoRayHitPos = gizmoRayHitInfo.point;
                //isGizmoHover = true;
            }
        }
        else {
            //isGizmoHover = false;
        }
    }

    private void ShootSegmentRaycast() {
        //RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseRayHitInfo);  // ADD LAYERMASK FOR CRITTERSEGMENT OBJECTS
        if (hit) {
            //Debug.Log("ShootSegmentRaycast()Hit " + mouseRayHitInfo.transform.gameObject.name);
            if (mouseRayHitInfo.transform.gameObject != null) {
                mouseRayHitPos = mouseRayHitInfo.point;
                
                Vector3 segmentLocalHitPos = new Vector3(0f, 0f, 0f);
                segmentLocalHitPos.x = Vector3.Dot(mouseRayHitPos - mouseRayHitInfo.transform.gameObject.transform.position, mouseRayHitInfo.transform.gameObject.transform.right);
                segmentLocalHitPos.y = Vector3.Dot(mouseRayHitPos - mouseRayHitInfo.transform.gameObject.transform.position, mouseRayHitInfo.transform.gameObject.transform.up);
                segmentLocalHitPos.z = Vector3.Dot(mouseRayHitPos - mouseRayHitInfo.transform.gameObject.transform.position, mouseRayHitInfo.transform.gameObject.transform.forward);
                mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosX", segmentLocalHitPos.x);
                mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosY", segmentLocalHitPos.y);
                mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosZ", segmentLocalHitPos.z);
                mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 1f);
            }
        }
        else {
            if (selectedSegment != null) {
                //selectedSegment.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 0f);
            }
            //Debug.Log("No hit| critterConstructorManager.selectedSegment = " + "ShootSegmentRaycast()");
        }        
    }

    private void UpdateHoverState() {
        
        if(isSegmentHover) {  // last frame, the cursor was hovering over a segment
            if(mouseRayHitInfo.transform != null) {  // // THIS frame, cursor is hovering over a SEGMENT!! 
                
                if (hoverSegment == mouseRayHitInfo.transform.gameObject) {  // It's still on the SAME segment.... nothing changes ...
                    CommandHoverSegmentStay();
                }
                else {  //  Cursor has switched to a NEW segment!!!!
                    CommandHoverSegmentSwitch();
                }
            }
            else {   // THIS frame, cursor is hovering over nothing!!  It Left the Segment!
                Debug.Log("CommandHoverSegmentExit()" + isSegmentHover.ToString() + ", " + hoverSegment.ToString());
                CommandHoverSegmentExit();
            }
        }
        else {  // last frame, the cursor was hovering over nothing
            //Debug.Log(mouseRayHitInfo.transform.ToString());
            if (mouseRayHitInfo.transform != null) {  // // THIS frame, cursor is hovering over a SEGMENT!! 
                Debug.Log("CommandHoverSegmentEnter()" + isSegmentHover.ToString());
                CommandHoverSegmentEnter();
            }
            else {  // THIS frame, cursor is hovering over nothing!!.... nothing changes ...
                
            }
                        
        }
    }

    private void OpenRightClickMenu() {
        // Right click menu was opened on this frame, due to a right-click
        Debug.Log("OpenRightClickMenu()");
        critterEditorUI.panelRightClickSegmentMenu.GetComponent<RectTransform>().position = new Vector3(mousePosition.x, mousePosition.y, 0f);
        critterEditorUI.ShowSegmentMenu();
        rightClickMenuOn = true;
    }

    private void CloseRightClickMenu() {
        // Right click menu was opened on this frame, due to a right-click
        //critterEditorUI.panelRightClickSegmentMenu.GetComponent<RectTransform>().position = new Vector3(mousePosition.x, mousePosition.y, 0f);
        critterEditorUI.HideSegmentMenu();
        rightClickMenuOn = false;
    }

    public void RightClickMenuAddEnter() {
        Debug.Log("MenuSegmentAddEnter");
        rightClickMenuAddHover = true;
    }
    public void RightClickMenuSegmentAddExit() {
        Debug.Log("MenuSegmentAddExit");
        rightClickMenuAddHover = false;
    }

    private void RightClickMenuSegmentAdd() {
        Debug.Log("MenuSegmentAdd()" + critterConstructorManager.masterCritter.masterCritterGenome.ToString() + ", ss: " + selectedSegment.GetComponent<CritterSegment>().sourceNode.ID.ToString());
        //critterConstructorManager.AddNewCritterNode(rayHitPos);
        // Determine attachCoords:
        Vector3 attachDir = ConvertWorldSpaceToAttachDir(selectedSegment, rightClickWorldPosition);
        int nextID = critterConstructorManager.masterCritter.masterCritterGenome.CritterNodeList.Count;
        critterConstructorManager.masterCritter.masterCritterGenome.AddNewNode(selectedSegment.GetComponent<CritterSegment>().sourceNode, attachDir, nextID);
        selectedSegmentID = nextID;
        critterConstructorManager.masterCritter.RebuildCritterFromGenome();
        rightClickMenuAddHover = false;

        // TEMPORARY:  -- DUE to critter being fully destroyed and re-built, the references to selected/hoverSegments are broken:
        SetHoverAndSelectedFromID();
        critterConstructorManager.UpdateSegmentSelectionVis();
        critterConstructorManager.UpdateSegmentShaderStates();
    }

    private Vector3 ConvertWorldSpaceToAttachDir(GameObject segment, Vector3 worldSpacePos) {
        
        Vector3 relativePos = worldSpacePos - segment.transform.position;
        float x = Vector3.Dot(relativePos, segment.transform.right);
        float y = Vector3.Dot(relativePos, segment.transform.up);
        float z = Vector3.Dot(relativePos, segment.transform.forward);
        Vector3 attachDir = new Vector3(x, y, z).normalized;
        Debug.Log("WorldSpace2attachDir: " + attachDir.ToString());
        return attachDir;
    }

    public void RightClickMenuSegmentScaleEnter() {
        Debug.Log("MenuSegmentScaleEnter");
        rightClickMenuScaleHover = true;
    }
    public void RightClickMenuSegmentScaleExit() {
        Debug.Log("MenuSegmentScaleExit");
        rightClickMenuScaleHover = false;
    }

    private void RightClickMenuSegmentScale() {
        Debug.Log("MenuSegmentScale()");
        //critterConstructorManager.ScaleCritterNode();
        rightClickMenuScaleHover = false;
        currentToolState = CurrentToolState.ScaleSegment;
    }
    
}
