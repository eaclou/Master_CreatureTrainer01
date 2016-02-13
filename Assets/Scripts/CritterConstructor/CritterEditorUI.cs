using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CritterEditorUI : MonoBehaviour {

    public CritterEditorState critterEditorState;

    public GameObject panelRightClickSegmentMenu;
    public GameObject panelRightClickJointMenu;    

    public Button buttonDisplayCameraMode;
    public Button buttonDisplayRCMenuMode;
    public Button buttonDisplayView;
    public Button buttonDisplayScale;
    public Button buttonDisplayMoveJoint;
    public Button buttonDisplayJointSettings;

    public Button buttonPreviewPhysics;

    public Text textHoverSegment;
    public Text textSelectedSegment;
    public Text textHoverGizmo;
    public Text textSelectedGizmo;

    // SEGMENT SETTINGS MENU:
    public GameObject panelSegmentSettings;
    public Text textSegmentID;
    public Text textParentID;
    public Text textSegmentSize;
    public Dropdown dropdownJointType;
    public Text textAttachDir;

    private bool isSegmentSettings = true;

    // ======= UI Inputs ???:
    public bool isClickButtonView = false;
    public bool isClickButtonScale = false;
    public bool isClickButtonMove = false;
    public bool isClickButtonSegmentSettings = false;
    public bool isClickButtonPreviewPhysics = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowSegmentMenu() {
        panelRightClickSegmentMenu.SetActive(true);
    }
    public void HideSegmentMenu() {
        panelRightClickSegmentMenu.SetActive(false);        
    }

    public void ShowJointMenu() {
        panelRightClickJointMenu.SetActive(true);
    }
    public void HideJointMenu() {
        panelRightClickJointMenu.SetActive(false);
    }

    public void ShowSegmentSettingsPanel() {
        panelSegmentSettings.SetActive(true);
        isSegmentSettings = true;
    }
    public void HideSegmentSettingsPanel() {
        panelSegmentSettings.SetActive(false);
        isSegmentSettings = false;
    }

    public void ClickButtonToolView() {
        critterEditorState.changeStateFromEditorUI = true;
        critterEditorState.pendingToolStateFromEditorUI = CritterEditorState.CurrentToolState.None;
    }
    public void ClickButtonToolScale() {
        critterEditorState.changeStateFromEditorUI = true;
        critterEditorState.pendingToolStateFromEditorUI = CritterEditorState.CurrentToolState.ScaleSegment;
    }
    public void ClickButtonToolMove() {
        critterEditorState.changeStateFromEditorUI = true;
        critterEditorState.pendingToolStateFromEditorUI = CritterEditorState.CurrentToolState.MoveAttachPoint;
    }
    public void ClickButtonSegmentSettings() {
        if(isSegmentSettings) {
            HideSegmentSettingsPanel();
        }
        else {
            ShowSegmentSettingsPanel();
        }
    }

    public void UpdateHoverDisplayText(bool segmentOver, GameObject hoverSegment, bool gizmoOver, GameObject hoverGizmo) {
        if(segmentOver) {
            textHoverSegment.text = "Hover Segment: " + hoverSegment.GetComponent<CritterSegment>().sourceNode.ID.ToString();
        }
        else {
            textHoverSegment.text = "Hover Segment: NONE!";
        }

        if (gizmoOver) {
            string axis = "";
            if(hoverGizmo.GetComponent<EditorGizmoObject>().gizmoType == EditorGizmoObject.GizmoType.axisX) {
                axis += "X";
            }
            if (hoverGizmo.GetComponent<EditorGizmoObject>().gizmoType == EditorGizmoObject.GizmoType.axisY) {
                axis += "Y";
            }
            if (hoverGizmo.GetComponent<EditorGizmoObject>().gizmoType == EditorGizmoObject.GizmoType.axisZ) {
                axis += "Z";
            }
            textHoverGizmo.text = "Hover Gizmo: " + axis;
        }
        else {
            textHoverGizmo.text = "Hover Gizmo: NONE!";
        }
    }

    public void UpdateSelectedDisplayText(bool segmentOn, GameObject selectedSegment, bool gizmoOn, GameObject selectedGizmo) {
        if (segmentOn) {
            textSelectedSegment.text = "Selected Segment: " + selectedSegment.GetComponent<CritterSegment>().sourceNode.ID.ToString();
        }
        else {
            textSelectedSegment.text = "Selected Segment: NONE!";
        }

        if (gizmoOn) {
            string axis = "";
            if (selectedGizmo.GetComponent<EditorGizmoObject>().gizmoType == EditorGizmoObject.GizmoType.axisX) {
                axis += "X";
            }
            else if (selectedGizmo.GetComponent<EditorGizmoObject>().gizmoType == EditorGizmoObject.GizmoType.axisY) {
                axis += "Y";
            }
            else if (selectedGizmo.GetComponent<EditorGizmoObject>().gizmoType == EditorGizmoObject.GizmoType.axisZ) {
                axis += "Z";
            }
            else if (selectedGizmo.GetComponent<EditorGizmoObject>().gizmoType == EditorGizmoObject.GizmoType.axisAll) {
                axis += "ALL";
            }
            textSelectedGizmo.text = "Selected Gizmo: " + axis;
        }
        else {
            textSelectedGizmo.text = "Selected Gizmo: NONE!";
        }
    }

    public void UpdateSegmentSettingsPanel(bool segmentOn, GameObject selectedSegment) {
        if(segmentOn) {
            textSegmentID.text = "Segment ID: " + selectedSegment.GetComponent<CritterSegment>().sourceNode.ID.ToString();
            if(selectedSegment.GetComponent<CritterSegment>().sourceNode.parentJointLink.parentNode != null) { // if not the ROOT
                textParentID.text = "Parent ID: " + selectedSegment.GetComponent<CritterSegment>().sourceNode.parentJointLink.parentNode.ID.ToString();
            }
            else {
                textParentID.text = "Parent ID: (root)";
            }
            Vector3 size = selectedSegment.GetComponent<CritterSegment>().sourceNode.dimensions;
            size.x = (float)Math.Round((double)size.x, 2);
            size.y = (float)Math.Round((double)size.y, 2);
            size.z = (float)Math.Round((double)size.z, 2);
            textSegmentSize.text = "Size: ( " + size.x.ToString() + ", " + size.y.ToString() + ", " + size.z.ToString() + " )";
            dropdownJointType.value = (int)selectedSegment.GetComponent<CritterSegment>().sourceNode.parentJointLink.jointType;
            Vector3 attachDir = selectedSegment.GetComponent<CritterSegment>().sourceNode.parentJointLink.attachDir;
            attachDir.x = (float)Math.Round((double)attachDir.x, 2);
            attachDir.y = (float)Math.Round((double)attachDir.y, 2);
            attachDir.z = (float)Math.Round((double)attachDir.z, 2);
            textAttachDir.text = "AttachDir: ( " + attachDir.x.ToString() + ", " + attachDir.y.ToString() + ", " + attachDir.z.ToString() + " )";
        }
        else {
            textSegmentID.text = "Segment ID: none";
            textParentID.text = "Parent ID: none";
            textSegmentSize.text = "Size:";
            textAttachDir.text = "AttachDir:";
        }
        
    }

    public void ClickPreviewPhysics() {
        critterEditorState.TogglePhysicsPreview();      
    }
}
