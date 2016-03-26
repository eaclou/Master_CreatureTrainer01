using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelAddonsList : MonoBehaviour {

    public PanelNodeAddons panelNodeAddons;
    public GameObject addonDisplayPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RepopulateList(CritterGenome sourceGenome, int nodeID) {
        var children = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        int addonDisplayIndex = 0;
        // Go through all the Addon lists and check if there are any addons for the current NodeID:

        // Physical Attributes:
        for (int physicalAttributesIndex = 0; physicalAttributesIndex < sourceGenome.addonPhysicalAttributesList.Count; physicalAttributesIndex++) {
            if(sourceGenome.addonPhysicalAttributesList[physicalAttributesIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++; 
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.PhysicalAttributes; 
                itemDisplay.sourceAddonIndex = physicalAttributesIndex; 
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }

        // Joint Angle Sensors:
        for (int jointAngleSensorIndex = 0; jointAngleSensorIndex < sourceGenome.addonJointAngleSensorList.Count; jointAngleSensorIndex++) {
            //Debug.Log("jointAngleSensorIndex Addon# " + jointAngleSensorIndex.ToString() + ", ");
            // Check if this node contains a jointAngleSensor Add-on:
            if (sourceGenome.addonJointAngleSensorList[jointAngleSensorIndex].critterNodeID == nodeID) {
                //Debug.Log("FOUND JOINTAngleSensor! " + jointAngleSensorIndex.ToString() + ", nodeID: " + nodeID.ToString());
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.JointAngleSensor;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = jointAngleSensorIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Contact Sensors:
        for (int contactSensorIndex = 0; contactSensorIndex < sourceGenome.addonContactSensorList.Count; contactSensorIndex++) {
            if (sourceGenome.addonContactSensorList[contactSensorIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.ContactSensor;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = contactSensorIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Raycast Sensors:
        for (int raycastSensorIndex = 0; raycastSensorIndex < sourceGenome.addonRaycastSensorList.Count; raycastSensorIndex++) {
            if (sourceGenome.addonRaycastSensorList[raycastSensorIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.RaycastSensor;
                itemDisplay.sourceAddonIndex = raycastSensorIndex;
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Compass Sensor 1D's:
        for (int compassSensor1DIndex = 0; compassSensor1DIndex < sourceGenome.addonCompassSensor1DList.Count; compassSensor1DIndex++) {
            if (sourceGenome.addonCompassSensor1DList[compassSensor1DIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.CompassSensor1D;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = compassSensor1DIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Compass Sensor 3D's:
        for (int compassSensor3DIndex = 0; compassSensor3DIndex < sourceGenome.addonCompassSensor3DList.Count; compassSensor3DIndex++) {
            //Debug.Log("CompassSensor3D Addon# " + compassSensor3DIndex.ToString() + ", ");
            // Check if this node contains a CompassSensor1D Add-on:
            if (sourceGenome.addonCompassSensor3DList[compassSensor3DIndex].critterNodeID == nodeID) {
                //Debug.Log("FOUND CompassSensor3D! " + compassSensor3DIndex.ToString() + ", nodeID: " + nodeID.ToString());
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.CompassSensor3D;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = compassSensor3DIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Position Sensor 1D's:
        for (int positionSensor1DIndex = 0; positionSensor1DIndex < sourceGenome.addonPositionSensor1DList.Count; positionSensor1DIndex++) {
            if (sourceGenome.addonPositionSensor1DList[positionSensor1DIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.PositionSensor1D;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = positionSensor1DIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Position Sensor 3D's:
        for (int positionSensor3DIndex = 0; positionSensor3DIndex < sourceGenome.addonPositionSensor3DList.Count; positionSensor3DIndex++) {
            if (sourceGenome.addonPositionSensor3DList[positionSensor3DIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.PositionSensor3D;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = positionSensor3DIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Rotation Sensor 1D's:
        for (int rotationSensor1DIndex = 0; rotationSensor1DIndex < sourceGenome.addonRotationSensor1DList.Count; rotationSensor1DIndex++) {
            if (sourceGenome.addonRotationSensor1DList[rotationSensor1DIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.RotationSensor1D;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = rotationSensor1DIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Rotation Sensor 3D's:
        for (int rotationSensor3DIndex = 0; rotationSensor3DIndex < sourceGenome.addonRotationSensor3DList.Count; rotationSensor3DIndex++) {
            if (sourceGenome.addonRotationSensor3DList[rotationSensor3DIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.RotationSensor3D;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = rotationSensor3DIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Velocity Sensor 1D's:
        for (int velocitySensor1DIndex = 0; velocitySensor1DIndex < sourceGenome.addonVelocitySensor1DList.Count; velocitySensor1DIndex++) {
            if (sourceGenome.addonVelocitySensor1DList[velocitySensor1DIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.VelocitySensor1D;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = velocitySensor1DIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Velocity Sensor 3D's:
        for (int velocitySensor3DIndex = 0; velocitySensor3DIndex < sourceGenome.addonVelocitySensor3DList.Count; velocitySensor3DIndex++) {
            if (sourceGenome.addonVelocitySensor3DList[velocitySensor3DIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.VelocitySensor3D;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = velocitySensor3DIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Altimeters:
        for (int altimeterIndex = 0; altimeterIndex < sourceGenome.addonAltimeterList.Count; altimeterIndex++) {
            if (sourceGenome.addonAltimeterList[altimeterIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.Altimeter;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = altimeterIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }

        // Joint Motors:
        for (int jointMotorIndex = 0; jointMotorIndex < sourceGenome.addonJointMotorList.Count; jointMotorIndex++) {
            //Debug.Log("jointMotor Addon# " + jointMotorIndex.ToString() + ", ");
            // Check if this node contains a jointMotor Add-on:
            if (sourceGenome.addonJointMotorList[jointMotorIndex].critterNodeID == nodeID) {
                //Debug.Log("FOUND JOINTMOTOR! " + jointMotorIndex.ToString() + ", nodeID: " + nodeID.ToString());
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;

                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                //itemDisplay.sourceAddon = sourceNode.addonsList[jointMotorIndex];
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.JointMotor;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = jointMotorIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.

                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Thruster Effector 1D's:
        for (int thrusterEffector1DIndex = 0; thrusterEffector1DIndex < sourceGenome.addonThrusterEffector1DList.Count; thrusterEffector1DIndex++) {
            if (sourceGenome.addonThrusterEffector1DList[thrusterEffector1DIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.ThrusterEffector1D;
                itemDisplay.sourceAddonIndex = thrusterEffector1DIndex;
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Thruster Effector 3D's:
        for (int thrusterEffector3DIndex = 0; thrusterEffector3DIndex < sourceGenome.addonThrusterEffector3DList.Count; thrusterEffector3DIndex++) {
            if (sourceGenome.addonThrusterEffector3DList[thrusterEffector3DIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.ThrusterEffector3D;
                itemDisplay.sourceAddonIndex = thrusterEffector3DIndex;
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Torque Effector 1D's:
        for (int torqueEffector1DIndex = 0; torqueEffector1DIndex < sourceGenome.addonTorqueEffector1DList.Count; torqueEffector1DIndex++) {
            if (sourceGenome.addonTorqueEffector1DList[torqueEffector1DIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.TorqueEffector1D;
                itemDisplay.sourceAddonIndex = torqueEffector1DIndex;
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Torque Effector 3D's:
        for (int torqueEffector3DIndex = 0; torqueEffector3DIndex < sourceGenome.addonTorqueEffector3DList.Count; torqueEffector3DIndex++) {
            if (sourceGenome.addonTorqueEffector3DList[torqueEffector3DIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.TorqueEffector3D;
                itemDisplay.sourceAddonIndex = torqueEffector3DIndex;
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }

        // Oscillator Inputs:
        for (int oscillatorInputIndex = 0; oscillatorInputIndex < sourceGenome.addonOscillatorInputList.Count; oscillatorInputIndex++) {
            //Debug.Log("OscillatorInput Addon# " + oscillatorInputIndex.ToString() + ", ");
            // Check if this node contains a OscillatorInput Add-on:
            if (sourceGenome.addonOscillatorInputList[oscillatorInputIndex].critterNodeID == nodeID) {
                //Debug.Log("FOUND OscillatorInput! " + oscillatorInputIndex.ToString() + ", nodeID: " + nodeID.ToString());
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.OscillatorInput;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = oscillatorInputIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Value Inputs:
        for (int valueInputIndex = 0; valueInputIndex < sourceGenome.addonValueInputList.Count; valueInputIndex++) {
            if (sourceGenome.addonValueInputList[valueInputIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;  // keeps track of position of this itemDisplay within the UI display addon List
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.ValueInput;  // tells this display to look in masterGenome.AddonJointMotorList[]
                itemDisplay.sourceAddonIndex = valueInputIndex;  // the index within masterGenome.AddonJointMotorList[] where the source Add-On is.
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
        // Timer Inputs:
        for (int timerInputIndex = 0; timerInputIndex < sourceGenome.addonTimerInputList.Count; timerInputIndex++) {
            if (sourceGenome.addonTimerInputList[timerInputIndex].critterNodeID == nodeID) {
                GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
                PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
                itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++; 
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.TimerInput; 
                itemDisplay.sourceAddonIndex = timerInputIndex; 
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
            }
        }
    }
}
