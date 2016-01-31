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
    public GameObject hoverSegment;
    public bool isSegmentHover = false; // is the mouse hovering over a critterSegment on this frame?
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

        if (critterEditorInputManager.keyFDown) {  // pressing 'f' centers the camera on the currently selected segment, or entire critter is nothing selected
            critterEditorInputManager.critterConstructorCameraController.SetFocalPoint(selectedSegment);
            critterEditorInputManager.critterConstructorCameraController.ReframeCamera();
        }
        if (critterEditorInputManager.keyAltDown) {  // pressing 'alt' enters camera mode  -- any clicks will control camera and nothing else
            altIsDown = true;
            currentCameraState = CurrentCameraState.Static;  // alt is down -- but no mouse-click -- 
        }
        if (critterEditorInputManager.keyAltUp) {  // letting go of alt exits camera mode
            altIsDown = false;
            currentCameraState = CurrentCameraState.Off;
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
        }
        else {  // not in camera mode:
            if (mouseMiddleIsDown) {  // middle w/e

            }
            if (mouseLeftIsDown) {  // Left-Click:
                // Check For Selecting
            }
            if (mouseRightIsDown) {  // Right-Click:

            }
        }
        critterEditorInputManager.critterConstructorCameraController.UpdateCamera(this);  // pan, zoom, rotate, or do nothing
        // Check for updated States:
        if (currentCameraState == CurrentCameraState.Off) {  // if Not in CAMERA mode, then check for other stuff:
            // Check Right-click menu state:
            if (critterEditorInputManager.mouseRightClickUp) {  // right click menu was exited this frame:
                // Check to see if any buttons were selected:
                if(rightClickMenuAddHover) {
                    RightClickMenuSegmentAdd();
                }
                if (rightClickMenuScaleHover) {
                    RightClickMenuSegmentScale();
                }
                // If so, perform actions:

                CloseRightClickMenu(); 
            }
            if (critterEditorInputManager.mouseRightClickDown) {  // right click menu was entered this frame:
                Debug.Log("Hit critterEditorInputManager.mouseRightClickDown");
                // Check to see if any segments are selected:
                // Check for Segment:
                ShootSegmentRaycast();
                if (isSegmentHover) {  //if left-Click on a segment
                    selectedSegment = hoverSegment;
                    isSegmentSelected = true;
                    critterConstructorManager.UpdateSegmentShaderStates();
                    critterConstructorManager.UpdateSegmentSelectionVis();
                }
                if (isSegmentSelected) {
                    rightClickWorldPosition = mouseRayHitPos;
                }
                //  display menu; init buttons
                OpenRightClickMenu();
            }
            //======= If Right-click menu is on: ======================================================
            if (rightClickMenuOn) {
                // don't need to check for raycasts, just check if mouse is over a button
            }
            else {   // right-click menu OFF
                
                // right-click menu has been off for at least a frame
                    // Check for Gizmos:
                ShootGizmoRaycast();
                // Check for Segment:
                ShootSegmentRaycast();

                if (critterEditorInputManager.mouseLeftClickDown) {  // left-clicked this frame
                    if(isSegmentHover) {  //if left-Click on a segment
                        selectedSegment = hoverSegment;
                        isSegmentSelected = true;
                    }
                    else {  // left-clicked on emptiness:
                        selectedSegment = null;
                        isSegmentSelected = false;
                    }
                    critterConstructorManager.UpdateSegmentShaderStates();
                    critterConstructorManager.UpdateSegmentSelectionVis();
                }
                if (critterEditorInputManager.mouseMiddleClickDown) {  // middle-clicked this frame

                }
                if (critterEditorInputManager.mouseRightClickDown) {  // right-clicked this frame

                }
                                
            }            
        }
        else {
            //Debug.Log("Camera Mode ON");
        }
    }

    /*   OLD OLD OLD OLD:::::::::::::::::   
    
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
    }*/

    private void ShootGizmoRaycast() {
        //RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out gizmoRayHitInfo);  // ADD LAYERMASK FOR GIZMO OBJECTS
        if (hit) {
            Debug.Log("Hit " + gizmoRayHitInfo.transform.gameObject.name);
            if (gizmoRayHitInfo.transform.gameObject != null) {
                gizmoRayHitPos = gizmoRayHitInfo.point;
                isGizmoHover = true;
            }
        }
        else {
            isGizmoHover = false;
        }
    }

    private void ShootSegmentRaycast() {
        //RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseRayHitInfo);  // ADD LAYERMASK FOR CRITTERSEGMENT OBJECTS
        if (hit) {
            Debug.Log("ShootSegmentRaycast()Hit " + mouseRayHitInfo.transform.gameObject.name);
            if (mouseRayHitInfo.transform.gameObject != null) {
                hoverSegment = mouseRayHitInfo.transform.gameObject;
                isSegmentHover = true;
                //mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = critterConstructorManager.critterSelectedSegmentMaterial;
                //mouseRayHitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 1f);
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
            hoverSegment = null;
            isSegmentHover = false;
            //Debug.Log("No hit| critterConstructorManager.selectedSegment = " + "ShootSegmentRaycast()");
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
        Debug.Log("MenuSegmentAdd()" + critterConstructorManager.masterCritter.masterCritterGenome.ToString());
        //critterConstructorManager.AddNewCritterNode(rayHitPos);
        critterConstructorManager.masterCritter.masterCritterGenome.AddNewNode(selectedSegment.GetComponent<CritterSegment>().sourceNode, rightClickWorldPosition);
        critterConstructorManager.masterCritter.RebuildCritterFromGenome();
        rightClickMenuAddHover = false;
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
    }
    
}
