using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

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
    public Vector3 gizmoPivotPoint = new Vector3(0f, 0f, 0f);
    public Quaternion gizmoOrientation = Quaternion.identity;

    public enum CurrentToolState {
        None,
        ScaleSegment,
        MoveAttachPoint
    };
    private CurrentToolState currentToolState = CurrentToolState.None;
    public bool changeStateFromEditorUI = false;
    public CurrentToolState pendingToolStateFromEditorUI = CurrentToolState.None;

    public enum CurrentCameraState {
        Off,
        Static,
        Pan,
        Rotate,
        Zoom
    };
    public CurrentCameraState currentCameraState = CurrentCameraState.Off;

    // GIZMOS!!! -- eventually move to their own class/script!!!
    GameObject gizmoGroupGO;
    GameObject gizmoScaleCoreGO;
    GameObject gizmoScaleXGO;
    GameObject gizmoScaleYGO;
    GameObject gizmoScaleZGO;
    GameObject gizmoAxisXGO;
    GameObject gizmoAxisYGO;
    GameObject gizmoAxisZGO;
    GameObject gizmoMoveCoreGO;
    GameObject gizmoMoveXGO;
    GameObject gizmoMoveYGO;
    GameObject gizmoMoveZGO;
    GameObject gizmoForceShaftGO;
    GameObject gizmoForceArrowGO;

    public GameObject selectedSegment;  // the CritterSegment that is currently selected
    public bool isSegmentSelected = false;
    public int selectedSegmentID = -1;
    public int selectedNodeID = -1;
    public CritterNode selectedSourceNode;
    public List<GameObject> selectedSegmentList = new List<GameObject>();
    public GameObject selectedSegmentGizmo;
    public GameObject hoverSegment;
    public bool isSegmentHover = false; // is the mouse hovering over a critterSegment on this frame?
    public int hoverSegmentID = -1;
    public Vector3 raycastSegmentLocalHitPos = new Vector3(0f, 0f, 0f);
    public GameObject engagedGizmo;  // the gizmo object/sub-object that is currently being manipulated
    public bool isGizmoEngaged = false;  // is the user currently interacting with a Gizmo?
    public GameObject hoverGizmo;  // the gizmo GameObject that the mouse is currently hovering over
    public bool isGizmoHover = false;  // is the mouse cursor currently hovering over a Gizmo?
    public Vector2 gizmoEngagedInitialMouseClickPos = new Vector2(0f, 0f);
    public Vector2 gizmoEngagedInitialMouseDelta = new Vector2(0f, 0f);
    public Vector2 gizmoEngagedInitialScreenPos = new Vector2(0f, 0f);
    public Vector2 gizmoEngagedInitialScreenDir = new Vector2(0f, 0f);
    public Vector3 gizmoEngagedInitialLocalPos = new Vector3(0f, 0f, 0f);
    public Vector3 gizmoSegmentInitialLocalScale = new Vector3(1f, 1f, 1f);
    public Vector3 gizmoSegmentInitialJointAttachDir = new Vector3(1f, 1f, 1f);

    private bool rightClickMenuOn = false;  // is the right-click context-driven menu currently active?
    private bool rightClickMenuAddHover = false;  // is the mouse cursor currently over the 'Add Segment' button?
    private bool rightClickMenuDeleteHover = false;
    private bool rightClickMenuViewHover = false;
    private bool rightClickMenuScaleHover = false;
    private bool rightClickMenuMoveHover = false;
    
    //   v v v (not necessarily pressed this frame, it could be being held) v v v 
    private bool mouseLeftIsDown = false;
    private bool mouseMiddleIsDown = false;
    private bool mouseRightIsDown = false;
    private bool altIsDown = false;  // is the alt key in a down state? (not necessarily pressed this frame, it could be being held)

    private Vector3 gravityStrength0 = new Vector3(0f, 0f, 0f);
    private Vector3 gravityStrength1 = new Vector3(0f, 0f, 0f);
    private int floorValue = -1; // used to track how many cycles of gravity-random
    private bool recalcGravityRandom = true;
    private bool isPhysicsPreview = false;
    

    public bool mouseOverUI = false;

    public void Awake() {
        InitGizmos();
        UpdateGizmos();
        critterConstructorManager.ResetToBlankCritter();
    }
    public void Start() {
        if (Debug.isDebugBuild) Debug.Log(" CritterEditorState Start()!");
        
        

        // TEMP:
        //Debug.Log(GetDistanceLineLine3D(new Vector3(-5f, 0f, 0f), new Vector3(-4f, 0f, 0f), new Vector3(0f, -6f, 0f), new Vector3(0f, 1f, 0f)).ToString());

        //Debug.Log(GetDistanceLinePoint2D(new Vector2(1f, 1f), new Vector2(0f, 0f), new Vector2(0f, 1f)).ToString());
    }

    public CurrentCameraState GetCurrentCameraState() {
        return currentCameraState;
    }

    public void SetMouseCursorPosition(Vector3 newPos) {
        mousePosition = newPos;
    }
    public void SetMouseCursorVelocity(Vector3 newVel) {
        mouseInput = newVel;
    }

    private void UpdateGravityStrength() { // !!
        Time.timeScale = 3.3f;
        Physics.solverIterationCount = 12;
        float currentTime = Time.fixedTime;
        float gravityMaxAmplitude = 4f;
        float lerpFrequency = 2.6f;
        float lerpValue = (currentTime % lerpFrequency) / lerpFrequency;
        int curFloorValue = (int)Mathf.Floor(currentTime / lerpFrequency);
        if(curFloorValue == floorValue) {  // still on same cycle

        }
        else {
            gravityStrength0 = gravityStrength1;
            gravityStrength1 = new Vector3(Random.Range(-gravityMaxAmplitude, gravityMaxAmplitude), Random.Range(-gravityMaxAmplitude, gravityMaxAmplitude), Random.Range(-gravityMaxAmplitude, gravityMaxAmplitude));
            floorValue = curFloorValue;
            recalcGravityRandom = true;
        }
        if (recalcGravityRandom) {

        }
        Physics.gravity = Vector3.Lerp(gravityStrength0, gravityStrength1, lerpValue);
        //Debug.Log("curTime: " + currentTime.ToString() + ", lerpValue: " + lerpValue.ToString() + ", g1: " + gravityStrength1.ToString());
        Vector3 gravityDir = Physics.gravity.normalized;
        float gravityMag = 2f;
        gizmoForceArrowGO.transform.position = gravityDir * gravityMag;
        gizmoForceArrowGO.transform.rotation = Quaternion.LookRotation(gravityDir);
        gizmoForceShaftGO.transform.position = gravityDir * gravityMag * 0.5f;
        gizmoForceShaftGO.transform.localScale = new Vector3(0.05f, 0.05f, gravityMag); ;
        gizmoForceShaftGO.transform.rotation = Quaternion.LookRotation(gravityDir);
        
        
    }

    public void UpdateStatesFromInput(CritterEditorInputManager critterEditorInputManager) {
        UpdateGravityStrength();
        //Debug.Log("UpdateStatesFromInput(CritterEditorInputManager critterEditorInputManager)");
        
        CheckKeyPresses(critterEditorInputManager);
        UpdateCameraState(critterEditorInputManager);

        // Check for updated States:
        if (currentCameraState == CurrentCameraState.Off) {  // if Not in CAMERA mode, then check for other stuff:

            // Check for commands from EditorUI buttons:
            if(changeStateFromEditorUI) {  // if a button has been pressed in the EditorUI GUI
                SetToolState(pendingToolStateFromEditorUI); // update Tool State
                changeStateFromEditorUI = false;  // reset trigger
            }

            // Check Right-click menu state:
            //======= If Right-click menu is on: ======================================================
            if (rightClickMenuOn) {
                // don't need to check for raycasts, just check if mouse is over a button
                if (critterEditorInputManager.mouseRightClickUp) {  // right click menu was exited this frame:
                    CommandRCMenuOff();                    
                }
            }
            else {   // right-click menu OFF
                // Check for Gizmos:
                //ShootGizmoRaycast();
                // Check for Segment:
                //ShootSegmentRaycast();
                if(mouseOverUI) {

                }
                else {
                    ShootEditorRaycasts();
                    UpdateHoverState();  // Using raycast information, determine the status of the hover state -- did mouse cursor enter, exit, or switch hover targets?

                    if (critterEditorInputManager.mouseRightClickDown) {  // right-clicked this frame
                                                                          // right click menu was entered this frame:
                        CommandRCMenuOn();
                    }

                    if (critterEditorInputManager.mouseLeftClickDown) {  // left-clicked this frame
                        if (isGizmoHover) {
                            CommandLeftClickGizmoDown();
                        }
                        else {
                            if (isSegmentHover) {  //if left-Click on a segment
                                CommandLeftClickSegment();
                            }
                            else {  // left-clicked on emptiness:
                                CommandLeftClickNone();
                            }
                        }
                    }
                    if (isGizmoEngaged) {
                        if (critterEditorInputManager.mouseLeftClickUp) {
                            CommandLeftClickGizmoUp();
                        }
                        else {
                            CommandGizmoOn();
                        }
                    }
                }                
            }            
        }
        else {
            //Debug.Log("Camera Mode ON");
        }
        // VVVVVVV The code below is duplicated within the Set Tool State() method -- look into consolidating and cleaning this up!!!
        if(currentToolState == CurrentToolState.None) {
            critterEditorUI.buttonDisplayView.interactable = false;
            critterEditorUI.textButtonView.fontStyle = FontStyle.Bold;
        }
        else {
            critterEditorUI.buttonDisplayView.interactable = true;
            critterEditorUI.textButtonView.fontStyle = FontStyle.Normal;
        }
        if (currentToolState == CurrentToolState.ScaleSegment) {
            critterEditorUI.buttonDisplayScale.interactable = false;
            critterEditorUI.textButtonScale.fontStyle = FontStyle.Bold;
        }
        else {
            critterEditorUI.buttonDisplayScale.interactable = true;
            critterEditorUI.textButtonScale.fontStyle = FontStyle.Normal;
        }
        if (currentToolState == CurrentToolState.MoveAttachPoint) {
            critterEditorUI.buttonDisplayMoveJoint.interactable = false;
            critterEditorUI.textButtonMove.fontStyle = FontStyle.Bold;
        }
        else {
            critterEditorUI.buttonDisplayMoveJoint.interactable = true;
            critterEditorUI.textButtonMove.fontStyle = FontStyle.Normal;
        }

        critterEditorUI.UpdateSelectedDisplayText(isSegmentSelected, selectedSegment, isGizmoEngaged, engagedGizmo, selectedSegmentGizmo);
        if(isPhysicsPreview) {
            critterEditorUI.UpdateSegmentSettingsPanel(isSegmentSelected, selectedSegment, false);
            critterEditorUI.UpdateNodeAddonsPanel(isSegmentSelected, selectedSegment, false);
        }
        else {
            if(isSegmentSelected) {
                critterEditorUI.UpdateSegmentSettingsPanel(isSegmentSelected, selectedSegment, true);
                critterEditorUI.UpdateNodeAddonsPanel(isSegmentSelected, selectedSegment, true);
            }
            else {
                critterEditorUI.UpdateSegmentSettingsPanel(isSegmentSelected, selectedSegment, false);
                critterEditorUI.UpdateNodeAddonsPanel(isSegmentSelected, selectedSegment, false);
            }
        }
        
    }

    private void SetHoverFromID() {
        //Debug.Log("SetHoverAndSelectedFromID ID: " + selectedSegmentID.ToString() + ", " + critterConstructorManager.masterCritter.critterSegmentList.ToString() + "#: " + critterConstructorManager.masterCritter.critterSegmentList.Count.ToString());
        /*if (isSegmentSelected) {
            if(selectedSegmentID >= critterConstructorManager.masterCritter.critterSegmentList.Count) {
                isSegmentSelected = false;
            }
            else {
                selectedSegment = critterConstructorManager.masterCritter.critterSegmentList[selectedSegmentID];
            }            
        }*/
        if (isSegmentHover) {
            if(hoverSegmentID >= critterConstructorManager.masterCritter.critterSegmentList.Count) {
                isSegmentHover = false;
            }
            else {
                hoverSegment = critterConstructorManager.masterCritter.critterSegmentList[hoverSegmentID];
            }            
        }

        //if (selectedSegmentID < critterConstructorManager.masterCritter.critterSegmentList.Count) {
        //    CommandSetSelected(critterConstructorManager.masterCritter.critterSegmentList[selectedSegmentID]);
        //}
    }

    private void CheckKeyPresses(CritterEditorInputManager critterEditorInputManager) {
        if (critterEditorInputManager.keyFDown) {  // pressing 'f' centers the camera on the currently selected segment, or entire critter is nothing selected
            critterEditorInputManager.critterConstructorCameraController.SetFocalPoint(selectedSegment);
            //critterEditorInputManager.critterConstructorCameraController.ReframeCamera();
        }
        if (critterEditorInputManager.keyQDown) {
            SetToolState(CurrentToolState.None);
        }
        if (critterEditorInputManager.keyWDown) {
            SetToolState(CurrentToolState.MoveAttachPoint);
        }
        if (critterEditorInputManager.keyRDown) {
            //Debug.Log("key r down SETTOOLSTATE");
            SetToolState(CurrentToolState.ScaleSegment);
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
        UpdateGizmos();
    }



    #region Command Functions
    
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

    private void CommandHoverGizmoEnter() {
        hoverGizmo = gizmoRayHitInfo.transform.gameObject;  // Set as new hoverSegment
        isGizmoHover = true;
        //SetSegmentMaterialHover(hoverSegment.GetComponent<CritterSegment>(), true);
    }
    private void CommandHoverGizmoSwitch() {
        //SetSegmentMaterialHover(hoverSegment.GetComponent<CritterSegment>(), false);  // Set OLD hoverSegment hover to false
        hoverGizmo = gizmoRayHitInfo.transform.gameObject;  // Set NEW hoverSegment
        isGizmoHover = true;
        //SetSegmentMaterialHover(hoverSegment.GetComponent<CritterSegment>(), true);  // Set NEW hoverSegment hover to true
    }
    private void CommandHoverGizmoExit() {
        // Went from over Segment to over Nothing:
        // Clean up current hoverSegment:
        //Debug.Log("CommandHoverSegmentExit()" + hoverSegment.ToString());
        //SetSegmentMaterialHover(hoverSegment.GetComponent<CritterSegment>(), false);
        // update hover segment info:
        hoverGizmo = null;  // Mouse is not hovering over anything
        isGizmoHover = false;
    }

    /*private void CommandSelectSegmentEnter() {
        selectedSegment = hoverSegment;
        Debug.Log("new selectedSegment = " + selectedSegment.GetComponent<CritterSegment>().ToString());
        selectedSegmentID = selectedSegment.GetComponent<CritterSegment>().id;
        isSegmentSelected = true;
        SetSegmentMaterialSelected(selectedSegment.GetComponent<CritterSegment>(), true);
        UpdateGizmos();
    }
    private void CommandSelectSegmentSwitch() {
        SetSegmentMaterialSelected(selectedSegment.GetComponent<CritterSegment>(), false);  // De-select old segment!!
        selectedSegment = hoverSegment;
        Debug.Log("new selectedSegment = " + selectedSegment.GetComponent<CritterSegment>().ToString());
        selectedSegmentID = selectedSegment.GetComponent<CritterSegment>().id;
        isSegmentSelected = true;
        SetSegmentMaterialSelected(selectedSegment.GetComponent<CritterSegment>(), true);
        UpdateGizmos();
    }
    private void CommandSelectSegmentExit() {
        Debug.Log("OLD selectedSegment = " + selectedSegment.GetComponent<CritterSegment>().ToString());
        SetSegmentMaterialSelected(selectedSegment.GetComponent<CritterSegment>(), false);  // De-select old segment!!
    
        selectedSegment = null;
        isSegmentSelected = false;
        UpdateGizmos();
    }*/
    private void CommandSetSelected(GameObject clickedObject) {
        
        if (clickedObject == null) {
            // Deselect all
            CommandDeselectAll();
        }
        else {   // Clicked on a GameObject!
            selectedSegment = clickedObject;
            selectedSegmentID = selectedSegment.GetComponent<CritterSegment>().id;            
            isSegmentSelected = true;
            int sourceNodeID = selectedSegment.GetComponent<CritterSegment>().sourceNode.ID;
            selectedNodeID = sourceNodeID;
            int lowestMatchID = critterConstructorManager.masterCritter.critterSegmentList.Count;
            // Brute force:
            selectedSegmentList.Clear();  // clear list and repopulate it:
            for (int i = 0; i < critterConstructorManager.masterCritter.critterSegmentList.Count; i++) {
                // iterate through all of the current segments of the critter, add to selectionList
                // check if they match the id of the clicked -- save the lowest index number for the segment that the Gizmo should be positioned at:
                SetSegmentMaterialSelected(critterConstructorManager.masterCritter.critterSegmentList[i].GetComponent<CritterSegment>(), false);
                if (critterConstructorManager.masterCritter.critterSegmentList[i].GetComponent<CritterSegment>().sourceNode.ID == sourceNodeID) {
                    // it's a match!!
                    if(i < lowestMatchID) {  // if this segment has a lower ID:
                        lowestMatchID = i;
                    }
                    selectedSegmentList.Add(critterConstructorManager.masterCritter.critterSegmentList[i]);
                    SetSegmentMaterialSelected(critterConstructorManager.masterCritter.critterSegmentList[i].GetComponent<CritterSegment>(), true);
                    //critterConstructorManager.masterCritter.critterSegmentList[i].GetComponent<MeshRenderer>().material.SetFloat("_Selected", 1f);  // turn on selected
                    //Debug.Log("CommandSetSelectedSegments: " + selectedSegmentList.Count.ToString() + ", lowestID: " + lowestMatchID.ToString() + ", i: " + i.ToString());
                }
                else {
                    // who gives a shit
                }
            }
            
            selectedSegmentGizmo = critterConstructorManager.masterCritter.critterSegmentList[lowestMatchID];

            critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(selectedSegment.GetComponent<CritterSegment>().sourceNode);  // REVISIT?
            //Debug.Log("CommandSetSelectedSegments: selectedSegment.GetComponent<CritterSegment>().id " + selectedSegment.GetComponent<CritterSegment>().id.ToString() + ", co: " + clickedObject.ToString() + ", selNodeID: " + selectedNodeID.ToString());
        }
    }
    private void CommandSetSelected(int nodeID) {

        if(nodeID < critterConstructorManager.masterCritter.masterCritterGenome.CritterNodeList.Count) {  // valid ID
            int lowestMatchID = critterConstructorManager.masterCritter.critterSegmentList.Count;
            // Brute force:
            selectedSegmentList.Clear();  // clear list and repopulate it:
            for (int i = 0; i < critterConstructorManager.masterCritter.critterSegmentList.Count; i++) {
                // iterate through all of the current segments of the critter, add to selectionList
                // check if they match the id of the clicked -- save the lowest index number for the segment that the Gizmo should be positioned at:
                SetSegmentMaterialSelected(critterConstructorManager.masterCritter.critterSegmentList[i].GetComponent<CritterSegment>(), false);
                if (critterConstructorManager.masterCritter.critterSegmentList[i].GetComponent<CritterSegment>().sourceNode.ID == nodeID) {
                    // it's a match!!
                    if (i < lowestMatchID) {  // if this segment has a lower ID:
                        lowestMatchID = i;
                    }
                    selectedSegmentList.Add(critterConstructorManager.masterCritter.critterSegmentList[i]);
                    SetSegmentMaterialSelected(critterConstructorManager.masterCritter.critterSegmentList[i].GetComponent<CritterSegment>(), true);
                    //critterConstructorManager.masterCritter.critterSegmentList[i].GetComponent<MeshRenderer>().material.SetFloat("_Selected", 1f);  // turn on selected
                    //Debug.Log("CommandSetSelectedSegments: " + selectedSegmentList.Count.ToString() + ", lowestID: " + lowestMatchID.ToString() + ", i: " + i.ToString());
                }
                else {
                    // who gives a shit
                }
            }
            isSegmentSelected = true;
            selectedSegmentID = lowestMatchID;
            selectedNodeID = nodeID;
            selectedSegment = critterConstructorManager.masterCritter.critterSegmentList[lowestMatchID];

            selectedSegmentGizmo = critterConstructorManager.masterCritter.critterSegmentList[lowestMatchID];
            critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(selectedSegment.GetComponent<CritterSegment>().sourceNode);  // REVISIT?
        }
        else {
            // Deselect all
            CommandDeselectAll();
        }
    }
    private void CommandDeselectAll() {
        // Deselect ALL:
        // Handle Multi-Select:
        if(critterConstructorManager.masterCritter != null) {
            for (int i = 0; i < critterConstructorManager.masterCritter.critterSegmentList.Count; i++) {  // Deselect old segments:
                SetSegmentMaterialSelected(critterConstructorManager.masterCritter.critterSegmentList[i].GetComponent<CritterSegment>(), false);
            }
            selectedSegmentList.Clear();

            selectedSegment = null;
            selectedSegmentGizmo = null;
            isSegmentSelected = false;
            selectedSegmentID = -1;  //  ??? should this be -1?
            selectedNodeID = -1;
            UpdateGizmos();
        }        
    }

    private void CommandGizmoEnter() {
        isGizmoEngaged = true;
        engagedGizmo = hoverGizmo;
        SetGizmoStartConditions();
        
    }
    private void CommandGizmoOn() {
        // Check current mouse position
        // Compare mouse position to initialMousePos
        // Figure out how gizmo has been moved
        Vector2 mousePositionDelta = mousePosition - gizmoEngagedInitialMouseClickPos;  // where mouse is NOW, compared to where it was first clicked

        float multiplier = GetGizmoMultiplier();
        Vector3 newSegmentScale = gizmoSegmentInitialLocalScale;

        if (engagedGizmo.GetComponent<EditorGizmoObject>().gizmoType != EditorGizmoObject.GizmoType.none) {
            if (engagedGizmo.GetComponent<EditorGizmoObject>().gizmoType == EditorGizmoObject.GizmoType.axisX) {
                if (currentToolState == CurrentToolState.ScaleSegment) {                    
                    newSegmentScale.x *= multiplier;
                    selectedSegment.GetComponent<CritterSegment>().sourceNode.dimensions = newSegmentScale;
                    gizmoAxisXGO.transform.localScale = new Vector3(gizmoEngagedInitialLocalPos.x * multiplier, 0.02f, 0.02f);
                    gizmoAxisXGO.transform.localPosition = gizmoEngagedInitialLocalPos * multiplier * 0.5f;
                }
                else if (currentToolState == CurrentToolState.MoveAttachPoint) {
                    if (selectedNodeID != 0) {
                        Vector3 newAttachDir = new Vector3(0f, 0f, 0f);
                        float effectMagnitude = Vector2.Dot(mousePositionDelta, gizmoEngagedInitialScreenDir); // amount of mouse movement projected onto gizmo screen vector
                        float effectSign = 1f;
                        if (effectMagnitude != 0f) {
                            effectSign = effectMagnitude / Mathf.Abs(effectMagnitude);  // either 1f or -1f
                        }
                        newAttachDir = Vector3.Lerp(gizmoSegmentInitialJointAttachDir, new Vector3(1f, 0f, 0f) * effectSign, Mathf.Abs(effectMagnitude) * 0.01f);
                        selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.attachDir = newAttachDir;
                    }
                }                
            }
            else if (engagedGizmo.GetComponent<EditorGizmoObject>().gizmoType == EditorGizmoObject.GizmoType.axisY) {
                if (currentToolState == CurrentToolState.ScaleSegment) {
                    newSegmentScale.y *= multiplier;
                    selectedSegment.GetComponent<CritterSegment>().sourceNode.dimensions = newSegmentScale;
                    gizmoAxisYGO.transform.localScale = new Vector3(0.02f, gizmoEngagedInitialLocalPos.y * multiplier, 0.02f);
                    gizmoAxisYGO.transform.localPosition = gizmoEngagedInitialLocalPos * multiplier * 0.5f;
                }
                else if (currentToolState == CurrentToolState.MoveAttachPoint) {
                    if (selectedNodeID != 0) {
                        Vector3 newAttachDir = new Vector3(0f, 0f, 0f);
                        float effectMagnitude = Vector2.Dot(mousePositionDelta, gizmoEngagedInitialScreenDir); // amount of mouse movement projected onto gizmo screen vector
                        float effectSign = 1f;
                        if (effectMagnitude != 0f) {
                            effectSign = effectMagnitude / Mathf.Abs(effectMagnitude);  // either 1f or -1f
                        }
                        newAttachDir = Vector3.Lerp(gizmoSegmentInitialJointAttachDir, new Vector3(0f, 1f, 0f) * effectSign, Mathf.Abs(effectMagnitude) * 0.01f);
                        selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.attachDir = newAttachDir;
                    }
                }                
            }
            else if (engagedGizmo.GetComponent<EditorGizmoObject>().gizmoType == EditorGizmoObject.GizmoType.axisZ) {
                if (currentToolState == CurrentToolState.ScaleSegment) {
                    newSegmentScale.z *= multiplier;
                    selectedSegment.GetComponent<CritterSegment>().sourceNode.dimensions = newSegmentScale;
                    gizmoAxisZGO.transform.localScale = new Vector3(0.02f, 0.02f, gizmoEngagedInitialLocalPos.z * multiplier);
                    gizmoAxisZGO.transform.localPosition = gizmoEngagedInitialLocalPos * multiplier * 0.5f;
                }
                else if (currentToolState == CurrentToolState.MoveAttachPoint) {
                    if (selectedNodeID != 0) {
                        Vector3 newAttachDir = new Vector3(0f, 0f, 0f);
                        float effectMagnitude = Vector2.Dot(mousePositionDelta, gizmoEngagedInitialScreenDir); // amount of mouse movement projected onto gizmo screen vector
                        float effectSign = 1f;
                        if (effectMagnitude != 0f) {
                            effectSign = effectMagnitude / Mathf.Abs(effectMagnitude);  // either 1f or -1f
                        }
                        newAttachDir = Vector3.Lerp(gizmoSegmentInitialJointAttachDir, new Vector3(0f, 0f, 1f) * effectSign, Mathf.Abs(effectMagnitude) * 0.01f);
                        selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.attachDir = newAttachDir;
                    }                    
                }                
            }
            else if (engagedGizmo.GetComponent<EditorGizmoObject>().gizmoType == EditorGizmoObject.GizmoType.axisAll) {                
                if (currentToolState == CurrentToolState.ScaleSegment) {
                    multiplier = Mathf.Max(1f + Vector2.Dot(mousePositionDelta, new Vector2(1f, 1f)) * 0.02f, 0.01f);
                    newSegmentScale.x *= multiplier;
                    newSegmentScale.y *= multiplier;
                    newSegmentScale.z *= multiplier;
                    gizmoScaleXGO.transform.localPosition = new Vector3(multiplier, 0f, 0f);
                    gizmoScaleYGO.transform.localPosition = new Vector3(0f, multiplier, 0f);
                    gizmoScaleZGO.transform.localPosition = new Vector3(0f, 0f, multiplier);
                    gizmoAxisXGO.transform.localScale = new Vector3(multiplier, 0.02f, 0.02f);
                    gizmoAxisXGO.transform.localPosition = new Vector3(1f, 0f, 0f) * multiplier * 0.5f;
                    gizmoAxisYGO.transform.localScale = new Vector3(0.02f, multiplier, 0.02f);
                    gizmoAxisYGO.transform.localPosition = new Vector3(0f, 1f, 0f) * multiplier * 0.5f;
                    gizmoAxisZGO.transform.localScale = new Vector3(0.02f, 0.02f, multiplier);
                    gizmoAxisZGO.transform.localPosition = new Vector3(0f, 0f, 1f) * multiplier * 0.5f;
                    selectedSegment.GetComponent<CritterSegment>().sourceNode.dimensions = newSegmentScale;
                }
                else if (currentToolState == CurrentToolState.MoveAttachPoint) {
                    if(selectedNodeID != 0) { // if NOT the root node!
                        if (isSegmentHover) {
                            Collider coll = selectedSegmentGizmo.GetComponent<CritterSegment>().parentSegment.gameObject.GetComponent<Collider>();
                            Ray ray = Camera.main.ScreenPointToRay(mousePosition - gizmoEngagedInitialMouseDelta);
                            RaycastHit hit;
                            Vector3 rayHitPos = new Vector3(0f, 0f, 0f);
                            if (coll.Raycast(ray, out hit, 100.0F)) {
                                rayHitPos = hit.point;

                                Vector3 newAttachDir = ConvertWorldSpaceToAttachDir(selectedSegmentGizmo.GetComponent<CritterSegment>().parentSegment.gameObject, rayHitPos);
                                Camera mainCam = critterConstructorManager.critterEditorInputManager.critterConstructorCameraController.gameObject.GetComponent<Camera>();
                                Vector2 mouseDir = mousePositionDelta.normalized;
                                Vector3 mouseDragWorldDir = Vector3.Normalize(mainCam.transform.right * mouseDir.x + mainCam.transform.up * mouseDir.y);
                                //Debug.Log("newAttachDir: " + newAttachDir.ToString() + ", mouseDir: " + mouseDir.ToString() + ", mouseDragWorldDir: " + mouseDragWorldDir.ToString() + ", rayHitPos: " + rayHitPos.ToString());
                                selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.attachDir = newAttachDir;
                            }
                        }
                    }                    
                }
            }

            if (currentToolState == CurrentToolState.ScaleSegment) {
                Vector3 newGizmoPos = gizmoEngagedInitialLocalPos * multiplier; // update gizmo
                engagedGizmo.transform.localPosition = newGizmoPos;
            }
            

            critterConstructorManager.masterCritter.UpdateCritterFromGenome();
        }
    }
    private void CommandGizmoExit() {
        // Figure out current Gizmo position
        // Set CritterSegment genome dimensions!
        ResetGizmos();
        UpdateGizmos();
        isGizmoEngaged = false;
        engagedGizmo = null;
    }
    private void SetGizmoStartConditions() {
        gizmoEngagedInitialMouseClickPos = mousePosition; // where the user actually clicked when engagint he gizmo        
        gizmoEngagedInitialScreenPos = critterConstructorManager.critterEditorInputManager.critterConstructorCameraController.gameObject.GetComponent<Camera>().WorldToScreenPoint(engagedGizmo.transform.position);  // center of the gizmo that was clicked
        gizmoEngagedInitialMouseDelta = mousePosition - gizmoEngagedInitialScreenPos;
        gizmoEngagedInitialLocalPos = engagedGizmo.transform.localPosition;
        gizmoSegmentInitialLocalScale = selectedSegmentGizmo.GetComponent<CritterSegment>().sourceNode.dimensions;  // change to sourceNode.dimensions ???
        Camera mainCam = critterConstructorManager.critterEditorInputManager.critterConstructorCameraController.gameObject.GetComponent<Camera>();
        Vector2 gizmoOriginScreenPos = new Vector2(mainCam.WorldToScreenPoint(gizmoGroupGO.transform.position).x, mainCam.WorldToScreenPoint(gizmoGroupGO.transform.position).y);  // 'group' = gizmo group
        Vector2 gizmoHandleScreenPos = new Vector2(mainCam.WorldToScreenPoint(engagedGizmo.transform.position).x, mainCam.WorldToScreenPoint(engagedGizmo.transform.position).y);
        gizmoEngagedInitialScreenDir = (gizmoHandleScreenPos - gizmoOriginScreenPos).normalized;
        gizmoSegmentInitialJointAttachDir = selectedSegmentGizmo.GetComponent<CritterSegment>().sourceNode.jointLink.attachDir;
    }
    private float GetGizmoMultiplier() {
        //Vector2 mousePositionDelta = mousePosition - gizmoEngagedInitialMouseClickPos;  // where mouse is NOW, compared to where it was first clicked
        //Vector3 gizmoHandleWorldDirection = new Vector3(0f, 0f, 0f);
        Camera mainCam = critterConstructorManager.critterEditorInputManager.critterConstructorCameraController.gameObject.GetComponent<Camera>();
        Vector2 gizmoOriginScreenPos = new Vector2(mainCam.WorldToScreenPoint(gizmoGroupGO.transform.position).x, mainCam.WorldToScreenPoint(gizmoGroupGO.transform.position).y);  // 'group' = gizmo group
        Vector3 gizmoHandleScreenPos = mainCam.WorldToScreenPoint(engagedGizmo.transform.position);
        Vector2 cursorToGizmoOriginScreen = new Vector2(mousePosition.x - gizmoOriginScreenPos.x, mousePosition.y - gizmoOriginScreenPos.y);
        Vector2 handleToGizmoOriginScreen = new Vector2(gizmoEngagedInitialMouseClickPos.x - gizmoOriginScreenPos.x, gizmoEngagedInitialMouseClickPos.y - gizmoOriginScreenPos.y);
        //Vector2 gizmoScreenVector = handleToGizmoOriginScreen;
        //Vector3 handleWorldDirection = new Vector3(0f, 0f, 0f);

        float multiplier = 1f * Vector2.Dot(handleToGizmoOriginScreen / handleToGizmoOriginScreen.magnitude, cursorToGizmoOriginScreen / handleToGizmoOriginScreen.magnitude); // ORIGINAL!
        multiplier = Mathf.Max(multiplier, 0.01f);  // minimum multiplier
       
        return multiplier;
        //Debug.Log("gizmoVectorToScreenProjection() " + gizmoVectorToScreenProjection.ToString() + ", multiplier= " + multiplier.ToString() + ", gizT: " + gizT.ToString() + ", targetHandleScreenPos: " + targetHandleScreenPos.ToString());
    }
    private float GetDistanceLineLine3D(Vector3 lineOrigin1, Vector3 lineEnd1, Vector3 lineOrigin2, Vector3 lineEnd2) {
        //float pa;  // point on line a that is closest
        //float pb;  // point on line b that is closest
        float t = 0f;  // paramter on line 1 that is closest
        float s = 0f;
        float epsilon = 0.00001f;

        Vector3 p13, p43, p21;
        float d1343, d4321, d1321, d4343, d2121;
        float numer, denom;

        p13.x = lineOrigin1.x - lineOrigin2.x;
        p13.y = lineOrigin1.y - lineOrigin2.y;
        p13.z = lineOrigin1.z - lineOrigin2.z;
        p43.x = lineEnd2.x - lineOrigin2.x;
        p43.y = lineEnd2.y - lineOrigin2.y;
        p43.z = lineEnd2.z - lineOrigin2.z;
        if (Mathf.Abs(p43.x) < epsilon && Mathf.Abs(p43.y) < epsilon && Mathf.Abs(p43.z) < epsilon) {
            //Debug.Log("CASE1");
        }
            //return (FALSE);
        p21.x = lineEnd1.x - lineOrigin1.x;
        p21.y = lineEnd1.y - lineOrigin1.y;
        p21.z = lineEnd1.z - lineOrigin1.z;
        if (Mathf.Abs(p21.x) < epsilon && Mathf.Abs(p21.y) < epsilon && Mathf.Abs(p21.z) < epsilon) {
            //Debug.Log("CASE2");
        }
            //return (FALSE);

        d1343 = p13.x * p43.x + p13.y * p43.y + p13.z * p43.z;
        d4321 = p43.x * p21.x + p43.y * p21.y + p43.z * p21.z;
        d1321 = p13.x * p21.x + p13.y * p21.y + p13.z * p21.z;
        d4343 = p43.x * p43.x + p43.y * p43.y + p43.z * p43.z;
        d2121 = p21.x * p21.x + p21.y * p21.y + p21.z * p21.z;

        denom = d2121 * d4343 - d4321 * d4321;
        if (Mathf.Abs(denom) < epsilon) {
            //Debug.Log("CASE3");
        }
            //return (FALSE);
        numer = d1343 * d4321 - d1321 * d4343;

        t = numer / denom;
        s = (d1343 + d4321 * (t)) / d4343;

        Vector3 closestPoint1 = new Vector3(lineOrigin1.x + t * p21.x, lineOrigin1.y + t * p21.y, lineOrigin1.z + t * p21.z);
        //pa->x = lineOrigin1.x + t * p21.x;
        //pa->y = lineOrigin1.y + t * p21.y;
        //pa->z = lineOrigin1.z + t * p21.z;
        //pb->x = lineOrigin2.x + s * p43.x;
        //pb->y = lineOrigin2.y + s * p43.y;
        //pb->z = lineOrigin2.z + s * p43.z;

        //return (TRUE);
        //Debug.Log("V1: " + lineOrigin1.ToString() + lineEnd1.ToString() + ", V2: " + lineOrigin2.ToString() + lineEnd2.ToString() + ", t: " + t.ToString() + ", s: " + s.ToString() + ", numer: " + numer.ToString() + ", denom: " + denom.ToString());
        return t;
    }
    private float GetDistanceLinePoint2D(Vector2 point, Vector2 lineOrigin, Vector2 lineEnd) {
        // Calculate the distance between
        // point pt and the segment p1 --> p2.
        //private double FindDistanceToSegment(
        //PointF pt, PointF p1, PointF p2, out PointF closest) {
        Vector2 closest;
        float dx = lineEnd.x - lineOrigin.x;
        float dy = lineEnd.y - lineOrigin.y;
        if ((dx == 0) && (dy == 0)) {
            // It's a point not a line segment.
            closest = lineOrigin;
            dx = point.x - lineOrigin.x;
            dy = point.y - lineOrigin.y;
            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        // Calculate the t that minimizes the distance.
        float t = ((point.x - lineOrigin.x) * dx + (point.y - lineOrigin.y) * dy) /
            (dx * dx + dy * dy);

        // See if this represents one of the segment's
        // end points or a point in the middle.
        if (t < 0) {
            closest = new Vector2(lineOrigin.x, lineOrigin.y);
            dx = point.x - lineOrigin.x;
            dy = point.y - lineOrigin.y;
        }
        else if (t > 1) {
            closest = new Vector2(lineEnd.x, lineEnd.y);
            dx = point.x - lineEnd.x;
            dy = point.y - lineEnd.y;
        }
        else {
            closest = new Vector2(lineOrigin.x + t * dx, lineOrigin.y + t * dy);
            dx = point.x - closest.x;
            dy = point.y - closest.y;
        }

        //return Mathf.Sqrt(dx * dx + dy * dy);
        //Debug.Log("GetDistanceLinePoint2D | t: " + t.ToString() + ", closest: " + closest.ToString());
        return t;
        /*import numpy as np

def closestDistanceBetweenLines(a0, a1, b0, b1, clampAll= False, clampA0 = False, clampA1 = False, clampB0 = False, clampB1 = False):
    ''' Given two lines defined by numpy.array pairs (a0,a1,b0,b1)
        Return distance, the two closest points, and their average
    '''

    # If clampAll=True, set all clamps to True
        if clampAll:
        clampA0 = True
        clampA1 = True
        clampB0 = True
        clampB1 = True

    # Calculate denomitator
        A = a1 - a0
    B = b1 - b0

    _A = A / np.linalg.norm(A)
    _B = B / np.linalg.norm(B)
    cross = np.cross(_A, _B);

        denom = np.linalg.norm(cross) * *2


    # If denominator is 0, lines are parallel: Calculate distance with a projection
# and evaluate clamp edge cases
        if (denom == 0):
        d0 = np.dot(_A, (b0 - a0))
        d = np.linalg.norm(((d0 * _A) + a0) - b0)

        # If clamping: the only time we'll get closest points will be when lines don't overlap at all.
# Find if segments overlap using dot products.
        if clampA0 or clampA1 or clampB0 or clampB1:
            d1 = np.dot(_A, (b1 - a0))

            # Is segment B before A?
        if d0 <= 0 >= d1:
                if clampA0 == True and clampB1 == True:
                    if np.absolute(d0) < np.absolute(d1):
                        return b0,a0,np.linalg.norm(b0 - a0)
                    return b1,a0,np.linalg.norm(b1 - a0)

            # Is segment B after A?
        elif d0 >= np.linalg.norm(A) <= d1:
                if clampA1 == True and clampB0 == True:
                    if np.absolute(d0) < np.absolute(d1):
                        return b0,a1,np.linalg.norm(b0 - a1)
                    return b1,a1,np.linalg.norm(b1, a1)

        # If clamping is off, or segments overlapped, we have infinite results, just return position.
        return None,None,d



# Lines criss-cross: Calculate the dereminent and return points
    t = (b0 - a0);
        det0 = np.linalg.det([t, _B, cross])
    det1 = np.linalg.det([t, _A, cross])

    t0 = det0 / denom;
        t1 = det1 / denom;

        pA = a0 + (_A * t0);
        pB = b0 + (_B * t1);

# Clamp results to line segments if needed
        if clampA0 or clampA1 or clampB0 or clampB1:

        if t0 < 0 and clampA0:
            pA = a0
        elif t0 > np.linalg.norm(A) and clampA1:
            pA = a1

        if t1 < 0 and clampB0:
            pB = b0
        elif t1 > np.linalg.norm(B) and clampB1:
            pB = b1

    d = np.linalg.norm(pA - pB)

    return pA,pB,d*/
    }

    private void CommandLeftClickNone() {
        /*  OLD:
        if (isSegmentSelected) {  // something was selected at the beginning of this frame, so now we need to de-select it
            CommandSelectSegmentExit();            
        }
        else {
            // Do nothing???  
        }*/
        CommandSetSelected(hoverSegment);
    }

    private void CommandLeftClickSegment() {
        /* OLD:
        if (isSegmentSelected) {
            if(selectedSegment == hoverSegment) {   // clicked on the same segment that is already selected!!
                // do nothing???
            }
            else {  // clicked ona  DIFFERENT segment!
                CommandSelectSegmentSwitch(); // there was already a segment selected, so this is a switch!
            }
        }
        else {
            CommandSelectSegmentEnter();
        }
        */
        CommandSetSelected(hoverSegment);
    }

    private void CommandLeftClickGizmoDown() {
        CommandGizmoEnter();
    }
    private void CommandLeftClickGizmoUp() {
        CommandGizmoExit();
    }

    private void CommandRCMenuOn() {
        if(!isPhysicsPreview) {
            if (isSegmentHover) {
                CommandSetSelected(hoverSegment);
                rightClickWorldPosition = mouseRayHitPos;
                critterEditorUI.panelRightClickSegmentMenu.GetComponent<PanelRightClickMenu>().buttonSegmentAdd.interactable = true;
                if (selectedNodeID != 0) {
                    critterEditorUI.panelRightClickSegmentMenu.GetComponent<PanelRightClickMenu>().buttonSegmentDelete.interactable = true;
                }
                else {
                    critterEditorUI.panelRightClickSegmentMenu.GetComponent<PanelRightClickMenu>().buttonSegmentDelete.interactable = false;
                }
            }
            else {
                // Disable Add and Delete options -- not selecting a segment!
                critterEditorUI.panelRightClickSegmentMenu.GetComponent<PanelRightClickMenu>().buttonSegmentAdd.interactable = false;
                critterEditorUI.panelRightClickSegmentMenu.GetComponent<PanelRightClickMenu>().buttonSegmentDelete.interactable = false;
            }
            //  display menu; init buttons
            OpenRightClickMenu();
            critterEditorUI.buttonDisplayRCMenuMode.interactable = false;
        }        
    }

    private void CommandRCMenuOff() {
        //Debug.Log("CommandRCMenuOff()" + rightClickMenuDeleteHover.ToString());
        // Check to see if any buttons were selected:
        if (rightClickMenuAddHover) {
            RightClickMenuSegmentAdd();        
        }
        if (rightClickMenuDeleteHover) {
            if (selectedNodeID != 0) {
                RightClickMenuSegmentDelete();
            }            
        }
        if (rightClickMenuViewHover) {
            RightClickMenuSegmentView();
        }
        if (rightClickMenuScaleHover) {
            RightClickMenuSegmentScale();
        }
        if(rightClickMenuMoveHover) {
            RightClickMenuSegmentMove();
        }
        // If so, perform actions:

        CloseRightClickMenu();
        critterEditorUI.buttonDisplayRCMenuMode.interactable = true;
        UpdateGizmos();
    }
    #endregion

    private void SetSegmentMaterialHover(CritterSegment segment, bool on) {
        // bool on true=is hovering;   false=not hovering
        if(on) {
            segment.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 1f);  // turn on crosshairs
        }
        else {
            segment.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 0f);  // turn off crosshairs
        }
    }
    private void SetSegmentMaterialSelected(CritterSegment segment, bool on) {
        if(on) {
            segment.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 1f);  // turn on selected
        }
        else {
            segment.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 0f);  // de-select
        }
    }

    private void ShootGizmoRaycast() {
        //RaycastHit hitInfo = new RaycastHit();
        int layer = 9;  // ignore editorSegments!
        int layermask = ~(1 << layer);
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(cameraRay, out gizmoRayHitInfo, Mathf.Infinity, layermask);  // ADD LAYERMASK FOR GIZMO OBJECTS
        if (hit) {
            //Debug.Log("ShootGizmoRaycast()Hit " + gizmoRayHitInfo.transform.gameObject.name + ", layer: " + gizmoRayHitInfo.transform.gameObject.layer.ToString() + ", pos: " + gizmoRayHitInfo.point.ToString());
            //Debug.Log("Hit " + gizmoRayHitInfo.transform.gameObject.name);
            if (gizmoRayHitInfo.transform.gameObject != null) {

                if(gizmoRayHitInfo.transform.gameObject.layer != LayerMask.NameToLayer("gizmo")) {
                    // keep trying!
                    //Debug.Log(" keep trying! ");
                    Ray extendedRay = new Ray(gizmoRayHitInfo.point, cameraRay.direction);
                    hit = Physics.Raycast(cameraRay, out gizmoRayHitInfo);
                    if (hit) {
                        //Debug.Log("We hit something!" + gizmoRayHitInfo.transform.gameObject.name + ", layer: " + gizmoRayHitInfo.transform.gameObject.layer.ToString() + ", pos: " + gizmoRayHitInfo.point.ToString());
                    }
                }
                else {
                    gizmoRayHitPos = gizmoRayHitInfo.point;
                }
            }
        }
        else {
            //isGizmoHover = false;
        }
    }
    private void ShootSegmentRaycast() {
        //RaycastHit hitInfo = new RaycastHit();
        //int layerAntiGizmo = ~(1 << 8);   // ignore gizmos!
        //int layerProSegment = ~(1 << 8);   // CheckSegments!
        int layermask = (1 << 9); // Or, (1 << layer1) | (1 << layer2)
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseRayHitInfo, layermask); 
        if (hit) {
            //Debug.Log("ShootSegmentRaycast()Hit " + mouseRayHitInfo.transform.gameObject.name + ", layer: " + mouseRayHitInfo.transform.gameObject.layer.ToString());
            if (mouseRayHitInfo.transform.gameObject != null) {
                mouseRayHitPos = mouseRayHitInfo.point;
                
                raycastSegmentLocalHitPos = new Vector3(0f, 0f, 0f);
                raycastSegmentLocalHitPos.x = Vector3.Dot(mouseRayHitPos - mouseRayHitInfo.transform.gameObject.transform.position, mouseRayHitInfo.transform.gameObject.transform.right);
                raycastSegmentLocalHitPos.y = Vector3.Dot(mouseRayHitPos - mouseRayHitInfo.transform.gameObject.transform.position, mouseRayHitInfo.transform.gameObject.transform.up);
                raycastSegmentLocalHitPos.z = Vector3.Dot(mouseRayHitPos - mouseRayHitInfo.transform.gameObject.transform.position, mouseRayHitInfo.transform.gameObject.transform.forward);
                mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosX", raycastSegmentLocalHitPos.x);
                mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosY", raycastSegmentLocalHitPos.y);
                mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosZ", raycastSegmentLocalHitPos.z);
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
    private void ShootEditorRaycasts() {
        int maxRays = 12;
        int rays = 0;
        bool hitSegment = false;
        bool hitGizmo = false;
        //bool hitNeither = false;
        bool keepCasting = true;

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray currentRay = new Ray(cameraRay.origin, cameraRay.direction);
        Vector3 offset = currentRay.direction * 0.0001f;
        RaycastHit editorRayHitInfo; // = new RaycastHit();
        int layerGizmo = (1 << 8);  // check for gizmo!
        int layerSegment = (1 << 9);  // check for editorSegments!
        int layermask = layerGizmo | layerSegment;

        while (keepCasting) {            
            rays++;
            bool hit = Physics.Raycast(currentRay, out editorRayHitInfo, Mathf.Infinity);
            //Debug.Log(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1).ToString()); // returns false if not on a UI (GO w/ an EventSystem), true if hovering
            //Debug.Log("while(!hitNeither) { " + rays.ToString() + hit.ToString() + ", " + hitSegment.ToString() + ", " + hitGizmo.ToString() + "origin: " + currentRay.origin.ToString());
            if (hit) {  // ray hit SOMETHING
                //Debug.Log("editorRayHitInfo: " + editorRayHitInfo.ToString());
                if (editorRayHitInfo.transform.gameObject != null) {  // make sure it hit a gameObject and logged its information
                    Vector3 newOrigin = editorRayHitInfo.point + offset;  // update start of ray
                    currentRay.origin = newOrigin;
                    //Debug.Log("currentRay.origin " + newOrigin.ToString());
                    if (editorRayHitInfo.transform.gameObject.layer == LayerMask.NameToLayer("gizmo")) {  // if it hit a gizmo!
                        if (!hitGizmo) {  // first time hitting gizmo
                            gizmoRayHitPos = editorRayHitInfo.point;
                            gizmoRayHitInfo = editorRayHitInfo;
                            
                            hitGizmo = true;
                            
                        }
                        else {  // Then we're done with Gizmos!  -- i.e we already hit one, and we're only interested in one
                            keepCasting = false; // <--- NOPE
                        }                        
                    }
                    else if (editorRayHitInfo.transform.gameObject.layer == LayerMask.NameToLayer("editorSegment")) {  // if it hit a CritterSegment!
                        if (!hitSegment) {
                            currentRay.origin = editorRayHitInfo.point + offset;  // update start of ray
                            mouseRayHitInfo = editorRayHitInfo; // CHECK that it's not a reference!
                            mouseRayHitPos = mouseRayHitInfo.point;


                            //raycastSegmentLocalHitPos = new Vector3(0f, 0f, 0f);
                            raycastSegmentLocalHitPos.x = Vector3.Dot(mouseRayHitPos - mouseRayHitInfo.transform.gameObject.transform.position, mouseRayHitInfo.transform.gameObject.transform.right) / mouseRayHitInfo.transform.gameObject.GetComponent<CritterSegment>().sourceNode.dimensions.x;
                            raycastSegmentLocalHitPos.y = Vector3.Dot(mouseRayHitPos - mouseRayHitInfo.transform.gameObject.transform.position, mouseRayHitInfo.transform.gameObject.transform.up) / mouseRayHitInfo.transform.gameObject.GetComponent<CritterSegment>().sourceNode.dimensions.y;
                            raycastSegmentLocalHitPos.z = Vector3.Dot(mouseRayHitPos - mouseRayHitInfo.transform.gameObject.transform.position, mouseRayHitInfo.transform.gameObject.transform.forward) / mouseRayHitInfo.transform.gameObject.GetComponent<CritterSegment>().sourceNode.dimensions.z;
                            mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosX", raycastSegmentLocalHitPos.x);
                            mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosY", raycastSegmentLocalHitPos.y);
                            mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosZ", raycastSegmentLocalHitPos.z);
                            mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_SizeX", mouseRayHitInfo.transform.gameObject.GetComponent<CritterSegment>().sourceNode.dimensions.x);
                            mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_SizeY", mouseRayHitInfo.transform.gameObject.GetComponent<CritterSegment>().sourceNode.dimensions.y);
                            mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_SizeZ", mouseRayHitInfo.transform.gameObject.GetComponent<CritterSegment>().sourceNode.dimensions.z);
                            if(!isGizmoEngaged) {
                                mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 1f);
                            }
                            else {
                                mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 0f);
                            }
                            

                            //Debug.Log("ShootSegmentRaycast()Hit " + editorRayHitInfo.transform.gameObject.name + ", layer: " + editorRayHitInfo.transform.gameObject.layer.ToString() + ", pos: " + currentRay.origin.ToString());
                            hitSegment = true;
                        }
                        else {  // Then we're done with Segments!  -- i.e we already hit one, and we're only interested in one
                            if (hitGizmo) {
                                keepCasting = false; // <--- NOPE
                            }                           
                        }                        
                    }
                    else {
                        Debug.Log("ray hit a GO but not segment or gizmo");
                    }
                }
                else {
                    Debug.Log("ray hit SOMETHING but not a gameObject");
                }
            }
            else {  // hit NOTHING!!
                keepCasting = false;
                if (!hitGizmo) {
                    gizmoRayHitInfo = editorRayHitInfo;
                }
                if (!hitSegment) {
                    mouseRayHitInfo = editorRayHitInfo;
                }
            }

            if(hitGizmo && hitSegment) {
                keepCasting = false;
            }
            if(rays >= maxRays) {
                keepCasting = false;
            }
            //keepCasting = false; // safeguard

            if (keepCasting == false) {
                
            }            
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
                //Debug.Log("CommandHoverSegmentExit()" + isSegmentHover.ToString() + ", " + hoverSegment.ToString());
                CommandHoverSegmentExit();
            }
        }
        else {  // last frame, the cursor was hovering over nothing
            //Debug.Log(mouseRayHitInfo.transform.ToString());
            if (mouseRayHitInfo.transform != null) {  // // THIS frame, cursor is hovering over a SEGMENT!! 
                //Debug.Log("CommandHoverSegmentEnter()" + isSegmentHover.ToString());
                CommandHoverSegmentEnter();
            }
            else {  // THIS frame, cursor is hovering over nothing!!.... nothing changes ...
                
            }                        
        }
        if (isGizmoHover) {  // last frame, the cursor was hovering over a gizmo
            if (gizmoRayHitInfo.transform != null) {  // // THIS frame, cursor is hovering over a Gizmo!! 

                if (hoverGizmo == gizmoRayHitInfo.transform.gameObject) {  // It's still on the SAME Gizmo.... nothing changes ...
                    //CommandHoverGizmoStay();
                }
                else {  //  Cursor has switched to a NEW Gizmo!!!!
                    CommandHoverGizmoSwitch();
                }
            }
            else {   // THIS frame, cursor is hovering over nothing!!  It Left the Gizmo!
                //Debug.Log("CommandHoverGizmoExit()" + isGizmoHover.ToString() + ", " + hoverGizmo.ToString());
                CommandHoverGizmoExit();
            }
        }
        else {  // last frame, the cursor was hovering over nothing
            if (gizmoRayHitInfo.transform != null) {  // // THIS frame, cursor is hovering over a Gizmo!! 
                //Debug.Log("CommandHoverGizmoEnter()" + isGizmoHover.ToString());
                CommandHoverGizmoEnter();
            }
            else {  // THIS frame, cursor is hovering over nothing!!.... nothing changes ...

            }
        }

        critterEditorUI.UpdateHoverDisplayText(isSegmentHover, hoverSegment, isGizmoHover, hoverGizmo);
    }

    private void OpenRightClickMenu() {
        // Right click menu was opened on this frame, due to a right-click
        //Debug.Log("OpenRightClickMenu()");
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
        //Debug.Log("MenuSegmentAddEnter");
        rightClickMenuAddHover = true;
    }
    public void RightClickMenuSegmentAddExit() {
        //Debug.Log("MenuSegmentAddExit");
        rightClickMenuAddHover = false;
    }
    private void RightClickMenuSegmentAdd() {
        if(isSegmentSelected) {            
            // Determine attachCoords:
            Vector3 attachDir = ConvertWorldSpaceToAttachDir(selectedSegment, rightClickWorldPosition);            
            int nextID = critterConstructorManager.masterCritter.masterCritterGenome.CritterNodeList.Count;
            critterConstructorManager.masterCritter.masterCritterGenome.AddNewNode(selectedSegment.GetComponent<CritterSegment>().sourceNode, attachDir, new Vector3(0f, 0f, 0f), nextID);            
            critterConstructorManager.masterCritter.RebuildCritterFromGenomeRecursive(false);
            
            // TEMPORARY:  -- DUE to critter being fully destroyed and re-built, the references to selected/hoverSegments are broken:
            CommandSetSelected(nextID);
            SetHoverFromID();
            critterConstructorManager.UpdateSegmentSelectionVis();
            critterConstructorManager.UpdateSegmentShaderStates();
        }
        rightClickMenuAddHover = false;
    }

    public void RightClickMenuDeleteEnter() {
        //Debug.Log("MenuSegmentDeleteEnter");
        rightClickMenuDeleteHover = true;
    }
    public void RightClickMenuSegmentDeleteExit() {
        //Debug.Log("MenuSegmentDeleteExit");
        rightClickMenuDeleteHover = false;
    }
    private void RightClickMenuSegmentDelete() {
        //Debug.Log("MenuSegmentDelete");
        
        if (isSegmentSelected) {
            //selectedSegmentID = selectedSegment.GetComponent<CritterSegment>().parentSegment.id;
            critterConstructorManager.masterCritter.DeleteNode(selectedSegment.GetComponent<CritterSegment>().sourceNode);
            critterConstructorManager.masterCritter.RenumberNodes();
            critterConstructorManager.masterCritter.DeleteSegments();
            critterConstructorManager.masterCritter.RebuildCritterFromGenomeRecursive(false);
            CommandSetSelected(0); // CHANGE THIS!!!!
            SetHoverFromID();
            //critterConstructorManager.UpdateSegmentSelectionVis();
            //critterConstructorManager.UpdateSegmentShaderStates();
        }
        rightClickMenuDeleteHover = false;
    }

    private Vector3 ConvertWorldSpaceToAttachDir(GameObject segment, Vector3 worldSpacePos) {
        
        Vector3 relativePos = (worldSpacePos - segment.transform.position);
        relativePos.Normalize();
        float x = Vector3.Dot(relativePos, segment.transform.right);
        float y = Vector3.Dot(relativePos, segment.transform.up);
        float z = Vector3.Dot(relativePos, segment.transform.forward);
        x /= segment.GetComponent<CritterSegment>().sourceNode.dimensions.x;
        y /= segment.GetComponent<CritterSegment>().sourceNode.dimensions.y;
        z /= segment.GetComponent<CritterSegment>().sourceNode.dimensions.z;
        Vector3 attachDir = new Vector3(x, y, z).normalized;
        //Debug.Log("WorldSpace2attachDir: " + attachDir.ToString());
        return attachDir;
    }

    public void RightClickMenuSegmentViewEnter() {
        //Debug.Log("MenuSegmentViewEnter");
        rightClickMenuViewHover = true;
    }
    public void RightClickMenuSegmentViewExit() {
        //Debug.Log("MenuSegmentViewExit");
        rightClickMenuViewHover = false;
    }
    private void RightClickMenuSegmentView() {
        //Debug.Log("MenuSegmentView()");
        rightClickMenuViewHover = false;  // Right-click menu has been exited, no longer hovering
        SetToolState(CurrentToolState.None);
    }

    public void RightClickMenuSegmentScaleEnter() {
        //Debug.Log("MenuSegmentScaleEnter");
        rightClickMenuScaleHover = true;
    }
    public void RightClickMenuSegmentScaleExit() {
        //Debug.Log("MenuSegmentScaleExit");
        rightClickMenuScaleHover = false;
    }

    private void RightClickMenuSegmentScale() {
        //Debug.Log("MenuSegmentScale()");
        rightClickMenuScaleHover = false;  // Right-click menu has been exited, no longer hovering
        SetToolState(CurrentToolState.ScaleSegment);        
    }

    public void RightClickMenuSegmentMoveEnter() {
        //Debug.Log("MenuSegmentMoveEnter");
        rightClickMenuMoveHover = true;
    }
    public void RightClickMenuSegmentMoveExit() {
        //Debug.Log("MenuSegmentMoveExit");
        rightClickMenuMoveHover = false;
    }
    private void RightClickMenuSegmentMove() {
        //Debug.Log("MenuSegmentMove()");
        rightClickMenuMoveHover = false;  // Right-click menu has been exited, no longer hovering
        SetToolState(CurrentToolState.MoveAttachPoint);
    }

    public void SetToolState(CurrentToolState toolState) {
        //Debug.Log("SetToolState( " + toolState.ToString());
        currentToolState = toolState;
        if(currentToolState == CurrentToolState.None) {
            SetToolView();
        }
        else if (currentToolState == CurrentToolState.ScaleSegment) {
            SetToolScale();
        }
        else if (currentToolState == CurrentToolState.MoveAttachPoint) {
            SetToolMove();
        }
        UpdateGizmos();
    }

    public void SetToolView() {
        critterEditorUI.buttonDisplayView.interactable = false; // update GUI display
        critterEditorUI.textButtonView.fontStyle = FontStyle.Bold;
        critterEditorUI.textButtonScale.fontStyle = FontStyle.Normal;
        critterEditorUI.textButtonMove.fontStyle = FontStyle.Normal;
    }
    public void SetToolScale() {
        critterEditorUI.buttonDisplayScale.interactable = false; // update GUI display
        critterEditorUI.textButtonView.fontStyle = FontStyle.Normal;
        critterEditorUI.textButtonScale.fontStyle = FontStyle.Bold;
        critterEditorUI.textButtonMove.fontStyle = FontStyle.Normal;
    }
    public void SetToolMove() {
        critterEditorUI.buttonDisplayMoveJoint.interactable = false; // update GUI display
        critterEditorUI.textButtonView.fontStyle = FontStyle.Normal;
        critterEditorUI.textButtonScale.fontStyle = FontStyle.Normal;
        critterEditorUI.textButtonMove.fontStyle = FontStyle.Bold;
    }

    public void UpdateGizmos() {
        //group.transform.position = gizmoPivotPoint;
        //group.transform.rotation = gizmoOrientation;
        /*if (isSegmentSelected) {
            selectedSegmentGizmo = selectedSegment;
            if (currentToolState == CurrentToolState.MoveAttachPoint) {
                //selectedSegmentGizmo = selectedSegment.GetComponent<CritterSegment>().parentSegment.gameObject;
            }
        } */       
        if (currentToolState != CurrentToolState.None) {
            if (isSegmentSelected) {
                if(!isPhysicsPreview) {
                    // Turn off Physics Arrow
                    gizmoForceShaftGO.SetActive(false);
                    gizmoForceArrowGO.SetActive(false);

                    gizmoAxisXGO.SetActive(true);
                    gizmoAxisYGO.SetActive(true);
                    gizmoAxisZGO.SetActive(true);
                    if (currentToolState == CurrentToolState.ScaleSegment) {
                        gizmoScaleCoreGO.SetActive(true);
                        gizmoScaleXGO.SetActive(true);
                        gizmoScaleYGO.SetActive(true);
                        gizmoScaleZGO.SetActive(true);
                        gizmoMoveCoreGO.SetActive(false);
                        gizmoMoveXGO.SetActive(false);
                        gizmoMoveYGO.SetActive(false);
                        gizmoMoveZGO.SetActive(false);
                        gizmoOrientation = selectedSegmentGizmo.transform.rotation;
                    }
                    else if (currentToolState == CurrentToolState.MoveAttachPoint) {
                        gizmoMoveCoreGO.SetActive(true);
                        gizmoMoveXGO.SetActive(true);
                        gizmoMoveYGO.SetActive(true);
                        gizmoMoveZGO.SetActive(true);
                        gizmoScaleCoreGO.SetActive(false);
                        gizmoScaleXGO.SetActive(false);
                        gizmoScaleYGO.SetActive(false);
                        gizmoScaleZGO.SetActive(false);
                        if (selectedSegmentGizmo.GetComponent<CritterSegment>().sourceNode.ID != 0) {  // if not the Root Node:
                            gizmoOrientation = selectedSegmentGizmo.GetComponent<CritterSegment>().parentSegment.transform.rotation;
                        }
                        else {
                            gizmoOrientation = selectedSegmentGizmo.transform.rotation;
                        }
                    }
                    gizmoGroupGO.transform.rotation = gizmoOrientation;
                    if(selectedNodeID == 0) {  // IF ROOT
                        gizmoPivotPoint = selectedSegmentGizmo.transform.position;
                    }
                    else {
                        gizmoPivotPoint = selectedSegmentGizmo.transform.position - selectedSegmentGizmo.transform.forward * selectedSegmentGizmo.GetComponent<CritterSegment>().sourceNode.dimensions.z * selectedSegmentGizmo.GetComponent<CritterSegment>().scalingFactor * 0.5f;
                    }                    
                    //gizmoOrientation = critterConstructorManager.masterCritter.critterSegmentList[selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.parentNode.ID].gameObject.transform.rotation;
                    gizmoGroupGO.transform.position = gizmoPivotPoint;
                    
                    float distToCamera = Vector3.Distance(critterConstructorManager.critterEditorInputManager.critterConstructorCameraController.gameObject.transform.position, gizmoGroupGO.transform.position);
                    gizmoGroupGO.transform.localScale = Vector3.one * distToCamera * 0.12f;
                    gizmoGroupGO.SetActive(true);  // a segment is selected and there is a tool active -- display the Gizmos!!!
                }
                else {
                    gizmoGroupGO.SetActive(false); // during physics preview, so do not display gizmos!
                }
            }
            else {
                gizmoGroupGO.SetActive(false); // nothing selected, so do not display gizmos!
            }
        }
        else {
            gizmoGroupGO.SetActive(false);  // current tool is set to View, so do not display Gizmos!
            if (isPhysicsPreview) {
                gizmoForceShaftGO.SetActive(true);
                gizmoForceArrowGO.SetActive(true);
                gizmoGroupGO.SetActive(true);
                gizmoMoveCoreGO.SetActive(false);
                gizmoMoveXGO.SetActive(false);
                gizmoMoveYGO.SetActive(false);
                gizmoMoveZGO.SetActive(false);
                gizmoScaleCoreGO.SetActive(false);
                gizmoScaleXGO.SetActive(false);
                gizmoScaleYGO.SetActive(false);
                gizmoScaleZGO.SetActive(false);
                gizmoAxisXGO.SetActive(false);
                gizmoAxisYGO.SetActive(false);
                gizmoAxisZGO.SetActive(false);
            }
            else {
                gizmoForceShaftGO.SetActive(false);
                gizmoForceArrowGO.SetActive(false);
                gizmoGroupGO.SetActive(false);
            }
        }
        
    }

    public void ResetGizmos() {
        
        gizmoScaleCoreGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        
        gizmoScaleXGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        gizmoScaleXGO.transform.localPosition = new Vector3(1f, 0f, 0f);
        
        gizmoScaleYGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        gizmoScaleYGO.transform.localPosition = new Vector3(0f, 1f, 0f);
        
        gizmoScaleZGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        gizmoScaleZGO.transform.localPosition = new Vector3(0f, 0f, 1f);
        
        gizmoAxisXGO.transform.localScale = new Vector3(gizmoScaleXGO.transform.localPosition.x - 0.04f, 0.02f, 0.02f);
        gizmoAxisXGO.transform.localPosition = gizmoScaleXGO.transform.localPosition * 0.5f;
        
        gizmoAxisYGO.transform.localScale = new Vector3(0.02f, gizmoScaleYGO.transform.localPosition.y - 0.04f, 0.02f);
        gizmoAxisYGO.transform.localPosition = gizmoScaleYGO.transform.localPosition * 0.5f;
        
        gizmoAxisZGO.transform.localScale = new Vector3(0.02f, 0.02f, gizmoScaleZGO.transform.localPosition.z - 0.04f);
        gizmoAxisZGO.transform.localPosition = gizmoScaleZGO.transform.localPosition * 0.5f;

        gizmoMoveCoreGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        gizmoMoveXGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        gizmoMoveXGO.transform.localPosition = new Vector3(1f, 0f, 0f);

        gizmoMoveYGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        gizmoMoveYGO.transform.localPosition = new Vector3(0f, 1f, 0f);

        gizmoMoveZGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        gizmoMoveZGO.transform.localPosition = new Vector3(0f, 0f, 1f);
    }

    public void InitGizmos() {
        if (gizmoGroupGO == null) {  // gizmo has already been created
            // SHARED:
            gizmoGroupGO = new GameObject("gizmoGroupGO");  // initialize gizmo objects
            gizmoAxisXGO = new GameObject("gizmoAxisXGO");
            gizmoAxisYGO = new GameObject("gizmoAxisYGO");
            gizmoAxisZGO = new GameObject("gizmoAxisZGO");
            // SCALE:
            gizmoScaleCoreGO = new GameObject("gizmoScaleCoreGO");
            gizmoScaleXGO = new GameObject("gizmoScaleXGO");
            gizmoScaleYGO = new GameObject("gizmoScaleYGO");
            gizmoScaleZGO = new GameObject("gizmoScaleZGO");
            // MOVE:
            gizmoMoveCoreGO = new GameObject("gizmoMoveCoreGO");
            gizmoMoveXGO = new GameObject("gizmoMoveXGO");
            gizmoMoveYGO = new GameObject("gizmoMoveYGO");
            gizmoMoveZGO = new GameObject("gizmoMoveZGO");
            // FORCE:
            gizmoForceShaftGO = new GameObject("gizmoForceShaftGO");
            gizmoForceArrowGO = new GameObject("gizmoForceArrowGO");

            gizmoScaleCoreGO.transform.SetParent(gizmoGroupGO.transform);
            gizmoScaleCoreGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoScaleCoreGO.AddComponent<EditorGizmoObject>();
            gizmoScaleCoreGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.Cube, EditorGizmoObject.GizmoType.axisAll);
            gizmoScaleCoreGO.GetComponent<EditorGizmoObject>().gizmoType = EditorGizmoObject.GizmoType.axisAll;
            gizmoScaleCoreGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f, 1f, 0f));
            gizmoScaleCoreGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            gizmoScaleXGO.transform.SetParent(gizmoGroupGO.transform);
            gizmoScaleXGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoScaleXGO.AddComponent<EditorGizmoObject>();
            gizmoScaleXGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.Cube, EditorGizmoObject.GizmoType.axisX);
            gizmoScaleXGO.GetComponent<EditorGizmoObject>().gizmoType = EditorGizmoObject.GizmoType.axisX;
            gizmoScaleXGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f, 0f, 0f));
            gizmoScaleXGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            gizmoScaleXGO.transform.localPosition = new Vector3(1f, 0f, 0f);

            gizmoScaleYGO.transform.SetParent(gizmoGroupGO.transform);
            gizmoScaleYGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoScaleYGO.AddComponent<EditorGizmoObject>();
            gizmoScaleYGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.Cube, EditorGizmoObject.GizmoType.axisY);
            gizmoScaleYGO.GetComponent<EditorGizmoObject>().gizmoType = EditorGizmoObject.GizmoType.axisY;
            gizmoScaleYGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0f, 1f, 0f));
            gizmoScaleYGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            gizmoScaleYGO.transform.localPosition = new Vector3(0f, 1f, 0f);

            gizmoScaleZGO.transform.SetParent(gizmoGroupGO.transform);
            gizmoScaleZGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoScaleZGO.AddComponent<EditorGizmoObject>();
            gizmoScaleZGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.Cube, EditorGizmoObject.GizmoType.axisZ);
            gizmoScaleZGO.GetComponent<EditorGizmoObject>().gizmoType = EditorGizmoObject.GizmoType.axisZ;
            gizmoScaleZGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0f, 0f, 1f));
            gizmoScaleZGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            gizmoScaleZGO.transform.localPosition = new Vector3(0f, 0f, 1f);

            gizmoAxisXGO.transform.SetParent(gizmoGroupGO.transform);
            gizmoAxisXGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoAxisXGO.AddComponent<EditorGizmoObject>();
            gizmoAxisXGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.Cube, EditorGizmoObject.GizmoType.none);
            gizmoAxisXGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0.75f, 0.75f, 0.75f));
            gizmoAxisXGO.transform.localScale = new Vector3(gizmoScaleXGO.transform.localPosition.x, 0.02f, 0.02f);
            gizmoAxisXGO.transform.localPosition = gizmoScaleXGO.transform.localPosition * 0.5f;

            gizmoAxisYGO.transform.SetParent(gizmoGroupGO.transform);
            gizmoAxisYGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoAxisYGO.AddComponent<EditorGizmoObject>();
            gizmoAxisYGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.Cube, EditorGizmoObject.GizmoType.none);
            gizmoAxisYGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0.75f, 0.75f, 0.75f));
            gizmoAxisYGO.transform.localScale = new Vector3(0.02f, gizmoScaleYGO.transform.localPosition.y, 0.02f);
            gizmoAxisYGO.transform.localPosition = gizmoScaleYGO.transform.localPosition * 0.5f;

            gizmoAxisZGO.transform.SetParent(gizmoGroupGO.transform);
            gizmoAxisZGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoAxisZGO.AddComponent<EditorGizmoObject>();
            gizmoAxisZGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.Cube, EditorGizmoObject.GizmoType.none);
            gizmoAxisZGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0.75f, 0.75f, 0.75f));
            gizmoAxisZGO.transform.localScale = new Vector3(0.02f, 0.02f, gizmoScaleZGO.transform.localPosition.z);
            gizmoAxisZGO.transform.localPosition = gizmoScaleZGO.transform.localPosition * 0.5f;

            gizmoMoveCoreGO.transform.SetParent(gizmoGroupGO.transform);
            gizmoMoveCoreGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoMoveCoreGO.AddComponent<EditorGizmoObject>();
            gizmoMoveCoreGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.OmniArrow, EditorGizmoObject.GizmoType.axisAll);
            gizmoMoveCoreGO.GetComponent<EditorGizmoObject>().gizmoType = EditorGizmoObject.GizmoType.axisAll;
            gizmoMoveCoreGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f, 1f, 0f));
            gizmoMoveCoreGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            gizmoMoveXGO.transform.SetParent(gizmoGroupGO.transform);
            gizmoMoveXGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoMoveXGO.AddComponent<EditorGizmoObject>();
            gizmoMoveXGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.Arrow, EditorGizmoObject.GizmoType.axisX);
            gizmoMoveXGO.GetComponent<EditorGizmoObject>().gizmoType = EditorGizmoObject.GizmoType.axisX;
            gizmoMoveXGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f, 0f, 0f));
            gizmoMoveXGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            gizmoMoveXGO.transform.localPosition = new Vector3(1f, 0f, 0f);
            gizmoMoveXGO.transform.rotation = Quaternion.LookRotation(new Vector3(1f, 0f, 0f), new Vector3(0f, 1f, 0f));

            gizmoMoveYGO.transform.SetParent(gizmoGroupGO.transform);
            gizmoMoveYGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoMoveYGO.AddComponent<EditorGizmoObject>();
            gizmoMoveYGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.Arrow, EditorGizmoObject.GizmoType.axisY);
            gizmoMoveYGO.GetComponent<EditorGizmoObject>().gizmoType = EditorGizmoObject.GizmoType.axisY;
            gizmoMoveYGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0f, 1f, 0f));
            gizmoMoveYGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            gizmoMoveYGO.transform.localPosition = new Vector3(0f, 1f, 0f);
            gizmoMoveYGO.transform.rotation = Quaternion.LookRotation(new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, -1f));

            gizmoMoveZGO.transform.SetParent(gizmoGroupGO.transform);
            gizmoMoveZGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoMoveZGO.AddComponent<EditorGizmoObject>();
            gizmoMoveZGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.Arrow, EditorGizmoObject.GizmoType.axisZ);
            gizmoMoveZGO.GetComponent<EditorGizmoObject>().gizmoType = EditorGizmoObject.GizmoType.axisZ;
            gizmoMoveZGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0f, 0f, 1f));
            gizmoMoveZGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            gizmoMoveZGO.transform.localPosition = new Vector3(0f, 0f, 1f);
            //gizmoMoveZGO.transform.rotation = Quaternion.LookRotation(new Vector3(1f, 0f, 0f), new Vector3(0f, 1f, 0f));
            
            gizmoForceShaftGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoForceShaftGO.AddComponent<EditorGizmoObject>();
            gizmoForceShaftGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.Cube, EditorGizmoObject.GizmoType.none);
            gizmoForceShaftGO.GetComponent<EditorGizmoObject>().gizmoType = EditorGizmoObject.GizmoType.none;
            gizmoForceShaftGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f, 1f, 0.5f));
            gizmoForceShaftGO.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
            gizmoForceShaftGO.transform.localPosition = new Vector3(0f, 0f, 0f);

            gizmoForceArrowGO.layer = LayerMask.NameToLayer("gizmo");  // set segmentGO layer to gizmo, to distinguish it from segments
            gizmoForceArrowGO.AddComponent<EditorGizmoObject>();
            gizmoForceArrowGO.GetComponent<EditorGizmoObject>().CreateMesh(EditorGizmoObject.GizmoMeshShape.Arrow, EditorGizmoObject.GizmoType.none);
            gizmoForceArrowGO.GetComponent<EditorGizmoObject>().gizmoType = EditorGizmoObject.GizmoType.none;
            gizmoForceArrowGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f, 1f, 0.5f));
            gizmoForceArrowGO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            gizmoForceArrowGO.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    public void PreviewPhysicsEnter() {
        critterConstructorManager.masterCritter.RebuildCritterFromGenomeRecursive(true);
        isPhysicsPreview = true;
        critterEditorUI.textPreviewPhysics.text = "Stop Preview";
        critterEditorUI.buttonPreviewPhysics.image.color = new Color(1f, 0.75f, 0.75f);
        critterEditorUI.buttonReset.interactable = false;
        critterEditorUI.buttonSave.interactable = false;
        critterEditorUI.buttonLoad.interactable = false;
        //SetHoverAndSelectedFromID();
        SetHoverFromID();
        if(isSegmentSelected) {
            CommandSetSelected(selectedNodeID);
            critterConstructorManager.UpdateSegmentSelectionVis();
            critterConstructorManager.UpdateSegmentShaderStates();
        }        
    }
    public void PreviewPhysicsExit() {
        critterConstructorManager.masterCritter.RebuildCritterFromGenomeRecursive(false);
        isPhysicsPreview = false;
        critterEditorUI.textPreviewPhysics.text = "Preview Physics";
        critterEditorUI.buttonPreviewPhysics.image.color = critterEditorUI.colorButtonNormal;
        critterEditorUI.buttonReset.interactable = true;
        critterEditorUI.buttonSave.interactable = true;
        critterEditorUI.buttonLoad.interactable = true;
        //SetHoverAndSelectedFromID();
        SetHoverFromID();
        if(isSegmentSelected) {
            CommandSetSelected(selectedNodeID);
            critterConstructorManager.UpdateSegmentSelectionVis();
            critterConstructorManager.UpdateSegmentShaderStates();
        }        
    }

    public void ClickSliderDimensionX(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.dimensions.x = value;
            RebuildCritterStatic();
        }
    }
    public void ClickSliderDimensionY(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.dimensions.y = value;
            RebuildCritterStatic();
        }
    }
    public void ClickSliderDimensionZ(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.dimensions.z = value;
            RebuildCritterStatic();
        }
    }
    public void ClickSliderAttachDirX(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.attachDir.x = value;
            RebuildCritterStatic();
        }
    }
    public void ClickSliderAttachDirY(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.attachDir.y = value;
            RebuildCritterStatic();
        }
    }
    public void ClickSliderAttachDirZ(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.attachDir.z = value;
            RebuildCritterStatic();
        }
    }
    public void ClickDropdownJointType(int val) {
        
        if(isSegmentSelected) {
            CritterJointLink.JointType type = (CritterJointLink.JointType)val;
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.jointType = type;
            //Debug.Log("val: " + val.ToString() + ", type: " + type.ToString());
            RebuildCritterStatic();
        }
    }
    public void ClickDropdownSymmetryType(int val) {

        if (isSegmentSelected) {
            CritterJointLink.SymmetryType type = (CritterJointLink.SymmetryType)val;
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.symmetryType = type;
            //Debug.Log("val: " + val.ToString() + ", type: " + type.ToString());
            RebuildCritterStatic();
        }
    }
    public void ClickRecursionPlus() {
        int maxRecursion = 8;
        if(isSegmentSelected) {
            if(selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.numberOfRecursions >= maxRecursion) {

            }
            else {
                selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.numberOfRecursions++;
                RebuildCritterStatic();
            }
        }
    }
    public void ClickRecursionMinus() {
        if (isSegmentSelected) {
            if (selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.numberOfRecursions <= 0) {

            }
            else {
                selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.numberOfRecursions--;
                RebuildCritterStatic();
            }
        }
    }
    public void ClickSliderRecursionScaling(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.recursionScalingFactor = value;
            RebuildCritterStatic();
        }
    }
    public void ClickSliderRecursionForward(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.recursionForward = value;
            RebuildCritterStatic();
        }
    }
    public void ClickSliderRestAngleX(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.restAngleDir.x = value;
            RebuildCritterStatic();
        }
    }
    public void ClickSliderRestAngleY(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.restAngleDir.y = value;
            RebuildCritterStatic();
        }
    }
    public void ClickSliderRestAngleZ(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.restAngleDir.z = value;
            RebuildCritterStatic();
        }
    }
    public void ClickSliderJointAngleLimit(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.jointLimitPrimary = value;
            RebuildCritterStatic();
        }
    }
    public void ClickSliderJointAngleLimitSecondary(float value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.jointLimitSecondary = value;
            RebuildCritterStatic();
        }
    }
    public void ClickToggleAttachToTail(bool value) {
        if (isSegmentSelected) {
            selectedSegment.GetComponent<CritterSegment>().sourceNode.jointLink.onlyAttachToTailNode = value;
            RebuildCritterStatic();
        }
    }

    public void ClickAttachAddon() {
        //Debug.Log("Attach Addon");
        CritterNodeAddonBase.CritterNodeAddonTypes addonType = (CritterNodeAddonBase.CritterNodeAddonTypes)critterEditorUI.panelNodeAddons.dropdownAddonType.value;
        CritterNode sourceNode = selectedSegment.GetComponent<CritterSegment>().sourceNode;

        //CritterNodeAddonBase newAddon;
        if(addonType == CritterNodeAddonBase.CritterNodeAddonTypes.JointMotor) {
            AddonJointMotor newJointMotor = new AddonJointMotor();      // Figure out how to get this to not create a new instance everytime -- check class type!      
            //newAddon = newJointMotor;
            if(!CheckListForAddon(newJointMotor, sourceNode.addonsList)) {  // only allows 1 instance of the JointMotor type
                sourceNode.addonsList.Add(newJointMotor);
                critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
            }            
        }
        else if (addonType == CritterNodeAddonBase.CritterNodeAddonTypes.JointAngleSensor) {
            AddonJointAngleSensor newJointAngleSensor = new AddonJointAngleSensor();
            if (!CheckListForAddon(newJointAngleSensor, sourceNode.addonsList)) {  // only allows 1 instance of the newJointAngleSensor type
                sourceNode.addonsList.Add(newJointAngleSensor);
                critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
            }
        }
        else if (addonType == CritterNodeAddonBase.CritterNodeAddonTypes.ContactSensor) {
            AddonContactSensor newContactSensor = new AddonContactSensor();
            if (!CheckListForAddon(newContactSensor, sourceNode.addonsList)) {  // only allows 1 instance of the newContactSensor type
                sourceNode.addonsList.Add(newContactSensor);
                critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
            }
        }
        else if (addonType == CritterNodeAddonBase.CritterNodeAddonTypes.RaycastSensor) {
            AddonRaycastSensor newRaycastSensor = new AddonRaycastSensor();
            sourceNode.addonsList.Add(newRaycastSensor);
            critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
        }
        else if (addonType == CritterNodeAddonBase.CritterNodeAddonTypes.CompassSensor1D) {
            AddonCompassSensor1D newCompassSensor1D = new AddonCompassSensor1D();
            sourceNode.addonsList.Add(newCompassSensor1D);
            critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
        }
        else if (addonType == CritterNodeAddonBase.CritterNodeAddonTypes.CompassSensor3D) {
            AddonCompassSensor3D newCompassSensor3D = new AddonCompassSensor3D();
            if (!CheckListForAddon(newCompassSensor3D, sourceNode.addonsList)) {  // only allows 1 instance of the newCompassSensor3D type
                sourceNode.addonsList.Add(newCompassSensor3D);
                critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
            }
        }
        else if (addonType == CritterNodeAddonBase.CritterNodeAddonTypes.ThrusterEffector1D) {
            AddonThrusterEffector1D newThrusterEffector1D = new AddonThrusterEffector1D();
            sourceNode.addonsList.Add(newThrusterEffector1D);
            critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
        }
        else if (addonType == CritterNodeAddonBase.CritterNodeAddonTypes.ThrusterEffector3D) {
            AddonThrusterEffector3D newThrusterEffector3D = new AddonThrusterEffector3D();
            if (!CheckListForAddon(newThrusterEffector3D, sourceNode.addonsList)) {  // only allows 1 instance of the newThrusterEffector3D type
                sourceNode.addonsList.Add(newThrusterEffector3D);
                critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
            }
        }
        else if (addonType == CritterNodeAddonBase.CritterNodeAddonTypes.StickyEffector) {
            AddonStickyEffector newStickyEffector = new AddonStickyEffector();
            if (!CheckListForAddon(newStickyEffector, sourceNode.addonsList)) {  // only allows 1 instance of the newThrusterEffector3D type
                sourceNode.addonsList.Add(newStickyEffector);
                critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
            }
        }
        else if (addonType == CritterNodeAddonBase.CritterNodeAddonTypes.OscillatorInput) {
            AddonOscillatorInput newOscillatorInput = new AddonOscillatorInput();
            sourceNode.addonsList.Add(newOscillatorInput);
            critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
        }
        else if (addonType == CritterNodeAddonBase.CritterNodeAddonTypes.ValueInput) {
            AddonValueInput newValueInput = new AddonValueInput();
            sourceNode.addonsList.Add(newValueInput);
            critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
        }
        //sourceNode.addonsList.Add(newAddon);
        //critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
    }
    public bool CheckListForAddon(CritterNodeAddonBase pendingAddon, List<CritterNodeAddonBase> list) {
        bool listContainsType = false;
        for(int i = 0; i < list.Count; i++) {
            if(pendingAddon.GetType() == list[i].GetType()) {
                // there already exists a jointMotor!!
                listContainsType = true;
            }
        }
        return listContainsType;
    }
    public void RemoveAddon(int index) {
        CritterNode sourceNode = selectedSegment.GetComponent<CritterSegment>().sourceNode;
        //Destroy(sourceNode.addonsList[index]);
        sourceNode.addonsList.RemoveAt(index);
        critterEditorUI.panelNodeAddons.panelAddonsList.GetComponent<PanelAddonsList>().RepopulateList(sourceNode);
    }

    private void RebuildCritterStatic() {
        if(!isPhysicsPreview) {  // hack to prevent changing UI to interrupt physics preview simulation -- eventually this will be fixed with dedicated UI handler classes
            critterConstructorManager.masterCritter.RebuildCritterFromGenomeRecursive(false);
            //SetHoverAndSelectedFromID();
            SetHoverFromID();
            CommandSetSelected(selectedNodeID);
            critterConstructorManager.UpdateSegmentSelectionVis();
            critterConstructorManager.UpdateSegmentShaderStates();
        }        
    }

    public void TogglePhysicsPreview() {
        if(isPhysicsPreview) {
            PreviewPhysicsExit();
        }
        else {
            PreviewPhysicsEnter();
        }
    }

    public void SaveCritterGenome() {
        Debug.Log("SaveCritterGenome;");
        /*
        bool save = false;
        bool overwriteFiles = true;
        string pendingCritterGenomeName = "";
        CritterGenome genomeToSave = critterConstructorManager.masterCritter.masterCritterGenome;
        // Open file explorer window to choose asset filename:
        string absPath = EditorUtility.SaveFilePanel("Select Critter Genome", "Assets/SaveFiles/CritterEditorGenomes", "critter", "txt");
        Debug.Log("absPath: " + absPath);
        if (absPath.StartsWith(Application.dataPath)) {
            Debug.Log("absPath starts with Application.dataPath");
            pendingCritterGenomeName = absPath.Substring(Application.dataPath.Length + "/SaveFiles/CritterEditorGenomes/".Length);
        }
        pendingCritterGenomeName = pendingCritterGenomeName.Substring(0, pendingCritterGenomeName.Length - ".txt".Length); // clips extension  // Revisit Extension type!
        DebugBot.DebugFunctionCall("pendingCritterGenomeName; " + pendingCritterGenomeName, true);
        if(genomeToSave != null) {
            if(pendingCritterGenomeName != "") {
                if (System.IO.File.Exists(absPath)) {
                    if(overwriteFiles) {
                        save = true;
                    }
                    else {
                        Debug.Log("File Already Exists! Can't Overwrite!!!");
                    }
                }
                else {
                    save = true;
                }
            }
            else {
                Debug.Log("No filename specified!");
            }
        }
        else {
            Debug.Log("Genome doesn't exist!!!");
        }

        if (save) {   // SAVE AGENT:
            ES2.Save(genomeToSave, absPath);
            //Population populationToLoad = ES2.Load<Population>(populationRootPath + fileName);
            //Debug.Log("genomeBiases.Length: " + populationToLoad.masterAgentArray[0].genome.genomeBiases.Length.ToString());
            //Debug.Log("genomeBias[0]: " + populationToLoad.masterAgentArray[0].genome.genomeBiases[0].ToString());
            //Debug.Log("ref genomeBias[0]: " + populationRef.masterAgentArray[0].genome.genomeBiases[0].ToString());
            //Debug.Log("root_body_size: " + populationToLoad.masterAgentArray[0].bodyGenome.creatureBodySegmentGenomeList[0].size.ToString());
        }
        else {
            Debug.Log("couldn't save!!!");
        }*/
    }

    public void LoadCritterGenome() {
        Debug.Log("LoadCritterGenome;");
        /*
        string pendingCritterGenomeName = "";
        //bool load = false;
        // Open file explorer window to choose asset filename:
        string absPath = EditorUtility.OpenFilePanel("Select Critter Genome", "Assets/SaveFiles/CritterEditorGenomes", "");
        Debug.Log("absPath: " + absPath);
        if (absPath.StartsWith(Application.dataPath)) {
            Debug.Log("absPath starts with Application.dataPath");
            pendingCritterGenomeName = absPath.Substring(Application.dataPath.Length + "/SaveFiles/CritterEditorGenomes/".Length);
        }
        pendingCritterGenomeName = pendingCritterGenomeName.Substring(0, pendingCritterGenomeName.Length - ".txt".Length); // clips extension  // Revisit Extension type!
        DebugBot.DebugFunctionCall("pendingCritterGenomeName; " + pendingCritterGenomeName, true);

        if (System.IO.File.Exists(absPath)) {
            CritterGenome genomeToLoad = ES2.Load<CritterGenome>(absPath);
            Debug.Log("genomeToLoad.Length: " + genomeToLoad.CritterNodeList.Count.ToString());
            critterConstructorManager.masterCritter.LoadCritterGenome(genomeToLoad);
            
            //Debug.Log("genomeBias[0]: " + populationToLoad.masterAgentArray[0].genome.genomeBiases[0].ToString());
            //Debug.Log("root_body_size: " + populationToLoad.masterAgentArray[0].bodyGenome.creatureBodySegmentGenomeList[0].size.ToString());

            // Leap of Faith:
            //currentPlayer.masterPopulation = populationToLoad;
            //currentPlayer.masterPopulation.InitializeLoadedMasterAgentArray(); // <-- somewhat hacky, re-assess later, but this is where the brains are created from genome
            //currentPlayer.masterPopulation.isFunctional = true;
            //currentPlayer.hasValidPopulation = true;
            //trainerModuleScript.SetAllPanelsFromTrainerData();
        }
        else {
            Debug.LogError("No CritterGenome File Exists!");
        }        
        */
    }
}
