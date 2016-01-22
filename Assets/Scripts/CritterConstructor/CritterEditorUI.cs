using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CritterEditorUI : MonoBehaviour {

    public GameObject panelRightClickSegmentMenu;
    public GameObject panelRightClickJointMenu;

    

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

    

}
