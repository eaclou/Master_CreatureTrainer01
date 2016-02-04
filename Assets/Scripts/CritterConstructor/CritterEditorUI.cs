using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public Text textHoverSegment;
    public Text textSelectedSegment;
    public Text textHoverGizmo;
    public Text textSelectedGizmo;


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

    public void UpdateHoverDisplayText(bool segmentOver, GameObject hoverSegment, bool gizmoOver, GameObject hoverGizmo) {
        if(segmentOver) {
            textHoverSegment.text = "Hover Segment: " + hoverSegment.GetComponent<CritterSegment>().sourceNode.ID.ToString();
        }
        else {
            textHoverSegment.text = "Hover Segment: NONE!";
        }

        if (gizmoOver) {
            string axis = "";
            if(hoverGizmo.GetComponent<EditorGizmoObject>().affectsAxisX) {
                axis += "X";
            }
            if (hoverGizmo.GetComponent<EditorGizmoObject>().affectsAxisY) {
                axis += "Y";
            }
            if (hoverGizmo.GetComponent<EditorGizmoObject>().affectsAxisZ) {
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
            if (selectedGizmo.GetComponent<EditorGizmoObject>().affectsAxisX) {
                axis += "X";
            }
            if (selectedGizmo.GetComponent<EditorGizmoObject>().affectsAxisY) {
                axis += "Y";
            }
            if (selectedGizmo.GetComponent<EditorGizmoObject>().affectsAxisZ) {
                axis += "Z";
            }
            textSelectedGizmo.text = "Selected Gizmo: " + axis;
        }
        else {
            textSelectedGizmo.text = "Selected Gizmo: NONE!";
        }
    }
}
