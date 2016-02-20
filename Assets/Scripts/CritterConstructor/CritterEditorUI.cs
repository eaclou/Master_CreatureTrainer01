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
    public PanelSegmentSettings panelSegmentSettings;
    public GameObject panelSegmentSettingsGO;
    private bool isSegmentSettings = false;

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
        panelSegmentSettingsGO.SetActive(true);
        isSegmentSettings = true;
    }
    public void HideSegmentSettingsPanel() {
        panelSegmentSettingsGO.SetActive(false);
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
            textHoverSegment.text = "Hover Segment: " + hoverSegment.GetComponent<CritterSegment>().id.ToString() + " ( " + hoverSegment.GetComponent<CritterSegment>().sourceNode.ID.ToString() + " )";
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
        if (selectedSegment != null) {
            textSelectedSegment.text = "Selected Segment: " + selectedSegment.GetComponent<CritterSegment>().id.ToString() + " ( " + selectedSegment.GetComponent<CritterSegment>().sourceNode.ID.ToString() + " )";
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
        if(selectedSegment != null) {
            CritterNode sourceNode = selectedSegment.GetComponent<CritterSegment>().sourceNode;
            panelSegmentSettings.textSegmentID.text = "Segment ID: " + selectedSegment.GetComponent<CritterSegment>().id.ToString();
            panelSegmentSettings.textNodeID.text = "(Node: " + sourceNode.ID.ToString() + ")";
            //textSegmentID.text = "Segment ID: " + sourceNode.ID.ToString();
            if(sourceNode.parentJointLink.parentNode != null) { // if not the ROOT
                panelSegmentSettings.textParentSegmentID.text = "Parent Segment ID: " + selectedSegment.GetComponent<CritterSegment>().parentSegment.id.ToString();
                panelSegmentSettings.textParentNodeID.text = "(Node: " + sourceNode.parentJointLink.parentNode.ID.ToString() + ")";
                //textParentID.text = "Parent ID: " + sourceNode.parentJointLink.parentNode.ID.ToString();
            }
            else {
                panelSegmentSettings.textParentSegmentID.text = "Parent Segment ID: ";
                panelSegmentSettings.textParentNodeID.text = "(Root)";
                //textParentID.text = "Parent ID: (root)";
            }
            Vector3 size = sourceNode.dimensions;
            size.x = (float)Math.Round((double)size.x, 2);
            size.y = (float)Math.Round((double)size.y, 2);
            size.z = (float)Math.Round((double)size.z, 2);
            panelSegmentSettings.textDimensionX.text = "X: " + size.x.ToString();
            panelSegmentSettings.textDimensionY.text = "Y: " + size.y.ToString();
            panelSegmentSettings.textDimensionZ.text = "Z: " + size.z.ToString();
            //textSegmentSize.text = "Size: ( " + size.x.ToString() + ", " + size.y.ToString() + ", " + size.z.ToString() + " )";
            panelSegmentSettings.dropdownJointType.value = (int)sourceNode.parentJointLink.jointType;
            //dropdownJointType.value = (int)sourceNode.parentJointLink.jointType;
            Vector3 attachDir = sourceNode.parentJointLink.attachDir;
            attachDir.x = (float)Math.Round((double)attachDir.x, 2);
            attachDir.y = (float)Math.Round((double)attachDir.y, 2);
            attachDir.z = (float)Math.Round((double)attachDir.z, 2);
            panelSegmentSettings.textAttachDirX.text = "X: " + attachDir.x.ToString();
            panelSegmentSettings.textAttachDirY.text = "Y: " + attachDir.y.ToString();
            panelSegmentSettings.textAttachDirZ.text = "Z: " + attachDir.z.ToString();
            Vector3 restAngle = sourceNode.parentJointLink.restAngleDir;
            restAngle.x = (float)Math.Round((double)restAngle.x, 2);
            restAngle.y = (float)Math.Round((double)restAngle.y, 2);
            panelSegmentSettings.textRestAngleX.text = "X: " + restAngle.x.ToString();
            panelSegmentSettings.textRestAngleY.text = "Y: " + restAngle.y.ToString();
            //textAttachDir.text = "AttachDir: ( " + attachDir.x.ToString() + ", " + attachDir.y.ToString() + ", " + attachDir.z.ToString() + " )";
            panelSegmentSettings.textNumRecursions.text = "Number Of Recursions: " + sourceNode.parentJointLink.numberOfRecursions.ToString();
            //textNumRecursions.text = "Recursions: " + sourceNode.parentJointLink.numberOfRecursions.ToString();
            panelSegmentSettings.textRecursionScaleFactor.text = "Scale Factor: " + sourceNode.parentJointLink.recursionScalingFactor.ToString();
            //textRecursionScalingFactor.text = "Recursion Scaling: " + sourceNode.parentJointLink.recursionScalingFactor.ToString();
            panelSegmentSettings.sliderRecursionScaleFactor.value = sourceNode.parentJointLink.recursionScalingFactor;
            //sliderRecursionScalingFactor.value = sourceNode.parentJointLink.recursionScalingFactor;
            //textRecursionForward.text = "Recursion Forward: " + sourceNode.parentJointLink.recursionForward.ToString();
            panelSegmentSettings.textRecursionForward.text = "Forward Bias: " + sourceNode.parentJointLink.recursionForward.ToString();
            panelSegmentSettings.sliderRecursionForward.value = sourceNode.parentJointLink.recursionForward;
            //sliderRecursionForward.value = sourceNode.parentJointLink.recursionForward;
            panelSegmentSettings.dropdownSymmetryType.value = (int)sourceNode.parentJointLink.symmetryType;
            //dropdownSymmetryType.value = (int)sourceNode.parentJointLink.symmetryType;
            //toggleAttachOnlyToEnd.isOn = sourceNode.parentJointLink.onlyAttachToTailNode;
            panelSegmentSettings.toggleAttachOnlyToLast.isOn = sourceNode.parentJointLink.onlyAttachToTailNode;
            panelSegmentSettings.textJointAngleLimit.text = sourceNode.parentJointLink.jointLimitMaxTemp.ToString();
            panelSegmentSettings.sliderJointAngleLimit.value = sourceNode.parentJointLink.jointLimitMaxTemp;
        }
        else {
            panelSegmentSettings.textSegmentID.text = "Segment ID: none";
            panelSegmentSettings.textNodeID.text = "Parent ID: none";
            panelSegmentSettings.textDimensionX.text = "X: ";
            panelSegmentSettings.textDimensionY.text = "Y: ";
            panelSegmentSettings.textDimensionZ.text = "Z: ";
            panelSegmentSettings.textAttachDirX.text = "X: ";
            panelSegmentSettings.textAttachDirY.text = "Y: ";
            panelSegmentSettings.textAttachDirZ.text = "Z: ";
        }
        
    }

    public void ClickPreviewPhysics() {
        critterEditorState.TogglePhysicsPreview();      
    }
}
