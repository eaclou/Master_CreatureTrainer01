using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelSegmentSettings : MonoBehaviour {

    public GameObject panelOverlay;

    public Text textSegmentID;
    public Text textParentSegmentID;
    public Text textNodeID;
    public Text textParentNodeID;

    public Text textDimensionX;
    public Text textDimensionY;
    public Text textDimensionZ;
    public Slider sliderDimensionX;
    public Slider sliderDimensionY;
    public Slider sliderDimensionZ;
    public Text textInheritedScaleFactor;

    public Text textAttachDirX;
    public Text textAttachDirY;
    public Text textAttachDirZ;
    public Slider sliderAttachDirX;
    public Slider sliderAttachDirY;
    public Slider sliderAttachDirZ;

    public Text textRestAngleX;
    public Text textRestAngleY;
    public Text textRestAngleZ;
    public Slider sliderRestAngleX;
    public Slider sliderRestAngleY;
    public Slider sliderRestAngleZ;

    public Dropdown dropdownJointType;

    public Text textNumRecursions;
    public Button buttonRecursionPlus;
    public Button buttonRecursionMinus;
    public Text textRecursionScaleFactor;
    public Slider sliderRecursionScaleFactor;
    public Text textRecursionForward;
    public Slider sliderRecursionForward;
    public Toggle toggleAttachOnlyToLast;

    public Dropdown dropdownSymmetryType;

    public Text textJointAngleLimit;
    public Text textJointAngleLimitSecondary;
    public Slider sliderJointAngleLimit;
    public Slider sliderJointAngleLimitSecondary;



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
