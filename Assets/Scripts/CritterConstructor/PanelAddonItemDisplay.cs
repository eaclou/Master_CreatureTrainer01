using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelAddonItemDisplay : MonoBehaviour {

    public PanelAddonsList panelAddonsList;

    public Text textHeader;
    public Button buttonRemove;
    public CritterNodeAddonBase sourceAddon;

    public int index;

    public GameObject floatDisplayPrefab;

	// Use this for initialization
	void Start () {
        
	}

	// Update is called once per frame
	void Update () {
	
	}

    public void Prime() {
        textHeader.text = sourceAddon.GetType().ToString();

        if(sourceAddon is AddonJointMotor) {
            AddonJointMotor jointMotor = (AddonJointMotor)sourceAddon;
            GameObject floatDisplayGO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay = floatDisplayGO.GetComponent<PanelAddonDisplayFloat>();
            
            floatDisplay.textFloatName.text = "Motor Force:";            
            floatDisplay.sliderFloat.minValue = jointMotor.motorForceMin;
            floatDisplay.sliderFloat.maxValue = jointMotor.motorForceMax;            
            floatDisplay.linkedFloatValue[0] = jointMotor.motorForce[0];
            //Debug.Log("0 jointMotor force: " + jointMotor.motorForce[0].ToString() + ", linkedFloatValue: " + floatDisplay.linkedFloatValue[0].ToString());
            jointMotor.motorForce = floatDisplay.linkedFloatValue;            
            //Debug.Log("1 jointMotor force: " + jointMotor.motorForce[0].ToString() + ", linkedFloatValue: " + floatDisplay.linkedFloatValue[0].ToString());
            floatDisplay.sliderFloat.value = jointMotor.motorForce[0];
            floatDisplay.textFloatValue.text = jointMotor.motorForce[0].ToString();

            floatDisplayGO.transform.SetParent(this.transform);
            //Debug.Log("2 jointMotor force: " + jointMotor.motorForce[0].ToString() + ", linkedFloatValue: " + floatDisplay.linkedFloatValue[0].ToString());
        }
    }

    public void ClickRemoveAddon() {
        //if (sourceAddon is AddonJointMotor) {
        //    AddonJointMotor jointMotor = (AddonJointMotor)sourceAddon;
        //    Debug.Log("jointMotor force: " + jointMotor.motorForce[0].ToString());
        //}
        panelAddonsList.panelNodeAddons.critterEditorState.RemoveAddon(index);
    }
}
