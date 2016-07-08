using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CritterEditorUI : MonoBehaviour {

    public CritterEditorState critterEditorState;

    public GameObject panelRightClickSegmentMenu;
    public GameObject panelRightClickJointMenu;

    public Button buttonReset;
    public Button buttonSave;
    public Button buttonLoad;   

    public Button buttonDisplayCameraMode;
    public Button buttonDisplayRCMenuMode;
    public Button buttonDisplayView;
    public Text textButtonView;
    public Button buttonDisplayScale;
    public Text textButtonScale;
    public Button buttonDisplayMoveJoint;
    public Text textButtonMove;
    public Button buttonDisplayJointSettings;
    public Text textJointSettingsTab;
    public Button buttonDisplayAddons;
    public Text textAddonsTab;

    public Button buttonPreviewPhysics;
    public Text textPreviewPhysics;

    public Text textHoverSegment;
    public Text textSelectedSegment;
    public Text textHoverGizmo;
    public Text textSelectedGizmo;

    // SEGMENT SETTINGS MENU:
    public PanelSegmentSettings panelSegmentSettings;
    public GameObject panelSegmentSettingsGO;
    public PanelNodeAddons panelNodeAddons;
    public GameObject panelNodeAddonsGO;
    private bool isSegmentSettings = false;
    private bool isNodeAddons = false;

    // ======= UI Inputs ???:
    public bool isClickButtonView = false;
    public bool isClickButtonScale = false;
    public bool isClickButtonMove = false;
    public bool isClickButtonSegmentSettings = false;
    public bool isClickButtonPreviewPhysics = false;

    public Color colorButtonTransparent = new Color(1f, 1f, 1f, 0f);
    public Color colorButtonNormal = new Color(1f, 1f, 1f, 1f);

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

    public void ShowNodeAddonsPanel() {
        panelNodeAddonsGO.SetActive(true);
        isNodeAddons = true;
    }
    public void HideNodeAddonsPanel() {
        panelNodeAddonsGO.SetActive(false);
        isNodeAddons = false;
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
            textJointSettingsTab.fontStyle = FontStyle.Normal;
            buttonDisplayJointSettings.image.color = colorButtonNormal;
        }
        else {
            ShowSegmentSettingsPanel();
            textJointSettingsTab.fontStyle = FontStyle.Bold;
            buttonDisplayJointSettings.image.color = colorButtonTransparent;
            HideNodeAddonsPanel();
            textAddonsTab.fontStyle = FontStyle.Normal;
            buttonDisplayAddons.image.color = colorButtonNormal;
        }
    }
    public void ClickButtonNodeAddons() {
        if (isNodeAddons) {
            HideNodeAddonsPanel();
            textAddonsTab.fontStyle = FontStyle.Normal;
            buttonDisplayAddons.image.color = colorButtonNormal;
        }
        else {
            ShowNodeAddonsPanel();
            textAddonsTab.fontStyle = FontStyle.Bold;
            buttonDisplayAddons.image.color = colorButtonTransparent;
            HideSegmentSettingsPanel();
            textJointSettingsTab.fontStyle = FontStyle.Normal;
            buttonDisplayJointSettings.image.color = colorButtonNormal;
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

    public void UpdateSelectedDisplayText(bool segmentOn, GameObject selectedSegment, bool gizmoOn, GameObject selectedGizmo, GameObject selectedSegmentGizmo) {
        if (selectedSegment != null) {
            if(selectedSegmentGizmo != null) {
                textSelectedSegment.text = "Selected Segment: " + selectedSegment.GetComponent<CritterSegment>().id.ToString() + " ( " + selectedSegment.GetComponent<CritterSegment>().sourceNode.ID.ToString() + " )" + " giz: " + selectedSegmentGizmo.GetComponent<CritterSegment>().id.ToString();
            }
            
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

    public void UpdateSegmentSettingsPanel(bool segmentOn, GameObject selectedSegment, bool active) {
        if(active) {
            panelSegmentSettings.panelOverlay.SetActive(false);
        }
        else {
            panelSegmentSettings.panelOverlay.SetActive(true);
        }
        if(selectedSegment != null) {
            CritterNode sourceNode = selectedSegment.GetComponent<CritterSegment>().sourceNode;
            panelSegmentSettings.textSegmentID.text = "Segment ID: " + selectedSegment.GetComponent<CritterSegment>().id.ToString();
            panelSegmentSettings.textNodeID.text = "(Node: " + sourceNode.ID.ToString() + ")";
            if(sourceNode.ID != 0) { // if not the ROOT
                panelSegmentSettings.textParentSegmentID.text = "Parent Segment ID: " + selectedSegment.GetComponent<CritterSegment>().parentSegment.id.ToString();
                panelSegmentSettings.textParentNodeID.text = "(Node: " + sourceNode.jointLink.parentNodeID.ToString() + ")";
            }
            else {
                panelSegmentSettings.textParentSegmentID.text = "Parent Segment ID: ";
                panelSegmentSettings.textParentNodeID.text = "(Root)";
            }
            Vector3 size = sourceNode.dimensions;
            size.x = (float)Math.Round((double)size.x, 2);
            size.y = (float)Math.Round((double)size.y, 2);
            size.z = (float)Math.Round((double)size.z, 2);
            panelSegmentSettings.textDimensionX.text = "X: " + size.x.ToString();
            panelSegmentSettings.textDimensionY.text = "Y: " + size.y.ToString();
            panelSegmentSettings.textDimensionZ.text = "Z: " + size.z.ToString();
            panelSegmentSettings.sliderDimensionX.value = size.x;
            panelSegmentSettings.sliderDimensionY.value = size.y;
            panelSegmentSettings.sliderDimensionZ.value = size.z;
            panelSegmentSettings.textInheritedScaleFactor.text = "Inherited Scale Factor: " + selectedSegment.GetComponent<CritterSegment>().scalingFactor.ToString();

            Vector3 attachDir = sourceNode.jointLink.attachDir;
            attachDir.x = (float)Math.Round((double)attachDir.x, 2);
            attachDir.y = (float)Math.Round((double)attachDir.y, 2);
            attachDir.z = (float)Math.Round((double)attachDir.z, 2);
            panelSegmentSettings.textAttachDirX.text = "X: " + attachDir.x.ToString();
            panelSegmentSettings.textAttachDirY.text = "Y: " + attachDir.y.ToString();
            panelSegmentSettings.textAttachDirZ.text = "Z: " + attachDir.z.ToString();
            panelSegmentSettings.sliderAttachDirX.value = attachDir.x;
            panelSegmentSettings.sliderAttachDirY.value = attachDir.y;
            panelSegmentSettings.sliderAttachDirZ.value = attachDir.z;

            Vector3 restAngle = sourceNode.jointLink.restAngleDir;
            restAngle.x = (float)Math.Round((double)restAngle.x, 2);
            restAngle.y = (float)Math.Round((double)restAngle.y, 2);
            restAngle.z = (float)Math.Round((double)restAngle.z, 2);
            panelSegmentSettings.textRestAngleX.text = "X: " + restAngle.x.ToString();
            panelSegmentSettings.textRestAngleY.text = "Y: " + restAngle.y.ToString();
            panelSegmentSettings.textRestAngleZ.text = "Z: " + restAngle.z.ToString();
            panelSegmentSettings.sliderRestAngleX.value = restAngle.x;
            panelSegmentSettings.sliderRestAngleY.value = restAngle.y;
            panelSegmentSettings.sliderRestAngleZ.value = restAngle.z;

            panelSegmentSettings.dropdownJointType.value = (int)sourceNode.jointLink.jointType;

            panelSegmentSettings.textNumRecursions.text = "Recursion Number: " + sourceNode.jointLink.numberOfRecursions.ToString();
            panelSegmentSettings.textRecursionScaleFactor.text = "Scale Factor: " + sourceNode.jointLink.recursionScalingFactor.ToString();
            panelSegmentSettings.sliderRecursionScaleFactor.value = sourceNode.jointLink.recursionScalingFactor;
            panelSegmentSettings.textRecursionForward.text = "Forward Bias: " + sourceNode.jointLink.recursionForward.ToString();
            panelSegmentSettings.sliderRecursionForward.value = sourceNode.jointLink.recursionForward;
            panelSegmentSettings.toggleAttachOnlyToLast.isOn = sourceNode.jointLink.onlyAttachToTailNode;

            panelSegmentSettings.dropdownSymmetryType.value = (int)sourceNode.jointLink.symmetryType;

            panelSegmentSettings.textJointAngleLimit.text = "Primary: " + sourceNode.jointLink.jointLimitPrimary.ToString();
            panelSegmentSettings.textJointAngleLimitSecondary.text = "Secondary: " + sourceNode.jointLink.jointLimitSecondary.ToString();
            panelSegmentSettings.sliderJointAngleLimit.value = sourceNode.jointLink.jointLimitPrimary;
            panelSegmentSettings.sliderJointAngleLimitSecondary.value = sourceNode.jointLink.jointLimitSecondary;
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
            panelSegmentSettings.textRestAngleX.text = "X: ";
            panelSegmentSettings.textRestAngleY.text = "Y: ";
            panelSegmentSettings.textRestAngleZ.text = "Z: ";

        }        
    }

    public void UpdateNodeAddonsPanel(bool segmentOn, GameObject selectedSegment, bool active) {
        if (active) {
            panelNodeAddons.panelOverlay.SetActive(false);
        }
        else {
            panelNodeAddons.panelOverlay.SetActive(true);
        }
        if (selectedSegment != null) {
            CritterNode sourceNode = selectedSegment.GetComponent<CritterSegment>().sourceNode;
            panelNodeAddons.textNodeID.text = "Selected Node ID: " + sourceNode.ID.ToString() + " [" + sourceNode.innov.ToString() + "]";            
        }
        else {
            panelNodeAddons.textNodeID.text = "Selected Node ID: none";
        }
    }

    public void ClickPreviewPhysics() {
        critterEditorState.TogglePhysicsPreview();      
    }
}
