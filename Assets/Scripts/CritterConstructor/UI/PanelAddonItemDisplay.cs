using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelAddonItemDisplay : MonoBehaviour {

    public PanelAddonsList panelAddonsList;

    public Text textHeader;
    public Button buttonRemove;
    //public CritterNodeAddonBase sourceAddon;
    public CritterNodeAddonBase.CritterNodeAddonTypes sourceAddonType;  
    public int sourceAddonIndex;

    public int index; // this is the index of the UI DISPLAY list 
    public int addonInno;

    public GameObject floatDisplayPrefab;
    public GameObject boolDisplayPrefab;
    public GameObject intDisplayPrefab;
    public GameObject vector3DisplayPrefab;

    // Use this for initialization
    void Start () {
        
	}

	// Update is called once per frame
	void Update () {
	
	}

    public void Prime(CritterGenome sourceGenome) {
        textHeader.text = sourceAddonType.ToString() + " [" + addonInno.ToString() + "]";

        if(sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.PhysicalAttributes) {
            AddonPhysicalAttributes physicalAttributes = sourceGenome.addonPhysicalAttributesList[sourceAddonIndex];

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();            
            floatDisplay1.textFloatName.text = "Dynamic Friction:";            
            floatDisplay1.sliderFloat.minValue = 0f;
            floatDisplay1.sliderFloat.maxValue = 1f;            
            floatDisplay1.linkedFloatValue[0] = physicalAttributes.dynamicFriction[0];
            physicalAttributes.dynamicFriction = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = physicalAttributes.dynamicFriction[0];
            floatDisplay1.textFloatValue.text = physicalAttributes.dynamicFriction[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);

            GameObject floatDisplay2GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay2 = floatDisplay2GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay2.textFloatName.text = "Static Friction:";
            floatDisplay2.sliderFloat.minValue = 0f;
            floatDisplay2.sliderFloat.maxValue = 1f;
            floatDisplay2.linkedFloatValue[0] = physicalAttributes.staticFriction[0];
            physicalAttributes.staticFriction = floatDisplay2.linkedFloatValue;
            floatDisplay2.sliderFloat.value = physicalAttributes.staticFriction[0];
            floatDisplay2.textFloatValue.text = physicalAttributes.staticFriction[0].ToString();
            floatDisplay2GO.transform.SetParent(this.transform);

            GameObject floatDisplay3GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay3 = floatDisplay3GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay3.textFloatName.text = "Bounciness:";
            floatDisplay3.sliderFloat.minValue = 0f;
            floatDisplay3.sliderFloat.maxValue = 1f;
            floatDisplay3.linkedFloatValue[0] = physicalAttributes.bounciness[0];
            physicalAttributes.bounciness = floatDisplay3.linkedFloatValue;
            floatDisplay3.sliderFloat.value = physicalAttributes.bounciness[0];
            floatDisplay3.textFloatValue.text = physicalAttributes.bounciness[0].ToString();
            floatDisplay3GO.transform.SetParent(this.transform);

            GameObject boolDisplay1GO = (GameObject)Instantiate(boolDisplayPrefab);
            PanelAddonDisplayBool boolDisplay1 = boolDisplay1GO.GetComponent<PanelAddonDisplayBool>();
            boolDisplay1.textBoolName.text = "Freeze Position X:";
            boolDisplay1.linkedBoolValue[0] = physicalAttributes.freezePositionX[0];
            physicalAttributes.freezePositionX = boolDisplay1.linkedBoolValue;
            boolDisplay1.toggleBool.isOn = physicalAttributes.freezePositionX[0];
            boolDisplay1GO.transform.SetParent(this.transform);

            GameObject boolDisplay2GO = (GameObject)Instantiate(boolDisplayPrefab);
            PanelAddonDisplayBool boolDisplay2 = boolDisplay2GO.GetComponent<PanelAddonDisplayBool>();
            boolDisplay2.textBoolName.text = "Freeze Position Y:";
            boolDisplay2.linkedBoolValue[0] = physicalAttributes.freezePositionY[0];
            physicalAttributes.freezePositionY = boolDisplay2.linkedBoolValue;
            boolDisplay2.toggleBool.isOn = physicalAttributes.freezePositionY[0];
            boolDisplay2GO.transform.SetParent(this.transform);

            GameObject boolDisplay3GO = (GameObject)Instantiate(boolDisplayPrefab);
            PanelAddonDisplayBool boolDisplay3 = boolDisplay3GO.GetComponent<PanelAddonDisplayBool>();
            boolDisplay3.textBoolName.text = "Freeze Position Z:";
            boolDisplay3.linkedBoolValue[0] = physicalAttributes.freezePositionZ[0];
            physicalAttributes.freezePositionZ = boolDisplay3.linkedBoolValue;
            boolDisplay3.toggleBool.isOn = physicalAttributes.freezePositionZ[0];
            boolDisplay3GO.transform.SetParent(this.transform);

            GameObject boolDisplay4GO = (GameObject)Instantiate(boolDisplayPrefab);
            PanelAddonDisplayBool boolDisplay4 = boolDisplay4GO.GetComponent<PanelAddonDisplayBool>();
            boolDisplay4.textBoolName.text = "Freeze Rotation X:";
            boolDisplay4.linkedBoolValue[0] = physicalAttributes.freezeRotationX[0];
            physicalAttributes.freezeRotationX = boolDisplay4.linkedBoolValue;
            boolDisplay4.toggleBool.isOn = physicalAttributes.freezeRotationX[0];
            boolDisplay4GO.transform.SetParent(this.transform);

            GameObject boolDisplay5GO = (GameObject)Instantiate(boolDisplayPrefab);
            PanelAddonDisplayBool boolDisplay5 = boolDisplay5GO.GetComponent<PanelAddonDisplayBool>();
            boolDisplay5.textBoolName.text = "Freeze Rotation Y:";
            boolDisplay5.linkedBoolValue[0] = physicalAttributes.freezeRotationY[0];
            physicalAttributes.freezeRotationY = boolDisplay5.linkedBoolValue;
            boolDisplay5.toggleBool.isOn = physicalAttributes.freezeRotationY[0];
            boolDisplay5GO.transform.SetParent(this.transform);

            GameObject boolDisplay6GO = (GameObject)Instantiate(boolDisplayPrefab);
            PanelAddonDisplayBool boolDisplay6 = boolDisplay6GO.GetComponent<PanelAddonDisplayBool>();
            boolDisplay6.textBoolName.text = "Freeze Rotation Z:";
            boolDisplay6.linkedBoolValue[0] = physicalAttributes.freezeRotationZ[0];
            physicalAttributes.freezeRotationZ = boolDisplay6.linkedBoolValue;
            boolDisplay6.toggleBool.isOn = physicalAttributes.freezeRotationZ[0];
            boolDisplay6GO.transform.SetParent(this.transform);
        }

        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.JointAngleSensor) {
            AddonJointAngleSensor jointAngleSensor = sourceGenome.addonJointAngleSensorList[sourceAddonIndex];
            GameObject floatDisplayGO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay = floatDisplayGO.GetComponent<PanelAddonDisplayFloat>();

            floatDisplay.textFloatName.text = "Angle Sensitivity:";
            floatDisplay.sliderFloat.minValue = 0.01f;
            floatDisplay.sliderFloat.maxValue = 2f;
            floatDisplay.linkedFloatValue[0] = jointAngleSensor.sensitivity[0];
            jointAngleSensor.sensitivity = floatDisplay.linkedFloatValue;
            floatDisplay.sliderFloat.value = jointAngleSensor.sensitivity[0];
            floatDisplay.textFloatValue.text = jointAngleSensor.sensitivity[0].ToString();

            GameObject boolDisplay1GO = (GameObject)Instantiate(boolDisplayPrefab);
            PanelAddonDisplayBool boolDisplay1 = boolDisplay1GO.GetComponent<PanelAddonDisplayBool>();
            boolDisplay1.textBoolName.text = "Measure Vel:";
            boolDisplay1.linkedBoolValue[0] = jointAngleSensor.measureVel[0];
            jointAngleSensor.measureVel = boolDisplay1.linkedBoolValue;
            boolDisplay1.toggleBool.isOn = jointAngleSensor.measureVel[0];
            boolDisplay1GO.transform.SetParent(this.transform);

            floatDisplayGO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.ContactSensor) {
            AddonContactSensor contactSensor = sourceGenome.addonContactSensorList[sourceAddonIndex];
            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Sensitivity:";
            floatDisplay1.sliderFloat.minValue = 0.05f;
            floatDisplay1.sliderFloat.maxValue = 20f;
            floatDisplay1.linkedFloatValue[0] = contactSensor.contactSensitivity[0];
            contactSensor.contactSensitivity = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = contactSensor.contactSensitivity[0];
            floatDisplay1.textFloatValue.text = contactSensor.contactSensitivity[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.RaycastSensor) {
            AddonRaycastSensor raycastSensor = sourceGenome.addonRaycastSensorList[sourceAddonIndex];
            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Max Distance:";
            floatDisplay1.sliderFloat.minValue = 0.1f;
            floatDisplay1.sliderFloat.maxValue = 25f;
            floatDisplay1.linkedFloatValue[0] = raycastSensor.maxDistance[0];
            raycastSensor.maxDistance = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = raycastSensor.maxDistance[0];
            floatDisplay1.textFloatValue.text = raycastSensor.maxDistance[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);

            GameObject vector3DisplayGO = (GameObject)Instantiate(vector3DisplayPrefab);
            PanelAddonDisplayVector3 vector3Display = vector3DisplayGO.GetComponent<PanelAddonDisplayVector3>();
            vector3Display.textVector3Name.text = "Forward Vector:";
            vector3Display.sliderX.minValue = -1f;
            vector3Display.sliderX.maxValue = 1f;
            vector3Display.sliderX.value = raycastSensor.forwardVector[0].x;
            vector3Display.textXValue.text = raycastSensor.forwardVector[0].x.ToString();
            vector3Display.sliderY.minValue = -1f;
            vector3Display.sliderY.maxValue = 1f;
            vector3Display.sliderY.value = raycastSensor.forwardVector[0].y;
            vector3Display.textYValue.text = raycastSensor.forwardVector[0].y.ToString();
            vector3Display.sliderZ.minValue = -1f;
            vector3Display.sliderZ.maxValue = 1f;
            vector3Display.sliderZ.value = raycastSensor.forwardVector[0].z;
            vector3Display.textZValue.text = raycastSensor.forwardVector[0].z.ToString();
            vector3Display.linkedVector3Value[0] = raycastSensor.forwardVector[0];
            raycastSensor.forwardVector = vector3Display.linkedVector3Value;
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.CompassSensor1D) {
            AddonCompassSensor1D compassSensor1D = sourceGenome.addonCompassSensor1DList[sourceAddonIndex];

            GameObject vector3DisplayGO = (GameObject)Instantiate(vector3DisplayPrefab);
            PanelAddonDisplayVector3 vector3Display = vector3DisplayGO.GetComponent<PanelAddonDisplayVector3>();
            vector3Display.textVector3Name.text = "Forward Vector:";
            vector3Display.sliderX.minValue = -1f;
            vector3Display.sliderX.maxValue = 1f;
            vector3Display.sliderX.value = compassSensor1D.forwardVector[0].x;
            vector3Display.textXValue.text = compassSensor1D.forwardVector[0].x.ToString();
            vector3Display.sliderY.minValue = -1f;
            vector3Display.sliderY.maxValue = 1f;
            vector3Display.sliderY.value = compassSensor1D.forwardVector[0].y;
            vector3Display.textYValue.text = compassSensor1D.forwardVector[0].y.ToString();
            vector3Display.sliderZ.minValue = -1f;
            vector3Display.sliderZ.maxValue = 1f;
            vector3Display.sliderZ.value = compassSensor1D.forwardVector[0].z;
            vector3Display.textZValue.text = compassSensor1D.forwardVector[0].z.ToString();
            vector3Display.linkedVector3Value[0] = compassSensor1D.forwardVector[0];
            compassSensor1D.forwardVector = vector3Display.linkedVector3Value;

            vector3DisplayGO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.CompassSensor3D) {
            AddonCompassSensor3D compassSensor3D = sourceGenome.addonCompassSensor3DList[sourceAddonIndex];            
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.PositionSensor1D) {
            AddonPositionSensor1D positionSensor1D = sourceGenome.addonPositionSensor1DList[sourceAddonIndex];

            GameObject vector3DisplayGO = (GameObject)Instantiate(vector3DisplayPrefab);
            PanelAddonDisplayVector3 vector3Display = vector3DisplayGO.GetComponent<PanelAddonDisplayVector3>();
            vector3Display.textVector3Name.text = "Forward Vector:";
            vector3Display.sliderX.minValue = -1f;
            vector3Display.sliderX.maxValue = 1f;
            vector3Display.sliderX.value = positionSensor1D.forwardVector[0].x;
            vector3Display.textXValue.text = positionSensor1D.forwardVector[0].x.ToString();
            vector3Display.sliderY.minValue = -1f;
            vector3Display.sliderY.maxValue = 1f;
            vector3Display.sliderY.value = positionSensor1D.forwardVector[0].y;
            vector3Display.textYValue.text = positionSensor1D.forwardVector[0].y.ToString();
            vector3Display.sliderZ.minValue = -1f;
            vector3Display.sliderZ.maxValue = 1f;
            vector3Display.sliderZ.value = positionSensor1D.forwardVector[0].z;
            vector3Display.textZValue.text = positionSensor1D.forwardVector[0].z.ToString();
            vector3Display.linkedVector3Value[0] = positionSensor1D.forwardVector[0];
            positionSensor1D.forwardVector = vector3Display.linkedVector3Value;
            vector3DisplayGO.transform.SetParent(this.transform);

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Sensitivity:";
            floatDisplay1.sliderFloat.minValue = 0.01f;
            floatDisplay1.sliderFloat.maxValue = 10f;
            floatDisplay1.linkedFloatValue[0] = positionSensor1D.sensitivity[0];
            positionSensor1D.sensitivity = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = positionSensor1D.sensitivity[0];
            floatDisplay1.textFloatValue.text = positionSensor1D.sensitivity[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);

            GameObject boolDisplay1GO = (GameObject)Instantiate(boolDisplayPrefab);
            PanelAddonDisplayBool boolDisplay1 = boolDisplay1GO.GetComponent<PanelAddonDisplayBool>();
            boolDisplay1.textBoolName.text = "Relative:";
            boolDisplay1.linkedBoolValue[0] = positionSensor1D.relative[0];
            positionSensor1D.relative = boolDisplay1.linkedBoolValue;
            boolDisplay1.toggleBool.isOn = positionSensor1D.relative[0];
            boolDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.PositionSensor3D) {
            AddonPositionSensor3D positionSensor3D = sourceGenome.addonPositionSensor3DList[sourceAddonIndex];

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Sensitivity:";
            floatDisplay1.sliderFloat.minValue = 0.01f;
            floatDisplay1.sliderFloat.maxValue = 10f;
            floatDisplay1.linkedFloatValue[0] = positionSensor3D.sensitivity[0];
            positionSensor3D.sensitivity = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = positionSensor3D.sensitivity[0];
            floatDisplay1.textFloatValue.text = positionSensor3D.sensitivity[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);

            GameObject boolDisplay1GO = (GameObject)Instantiate(boolDisplayPrefab);
            PanelAddonDisplayBool boolDisplay1 = boolDisplay1GO.GetComponent<PanelAddonDisplayBool>();
            boolDisplay1.textBoolName.text = "Relative:";
            boolDisplay1.linkedBoolValue[0] = positionSensor3D.relative[0];
            positionSensor3D.relative = boolDisplay1.linkedBoolValue;
            boolDisplay1.toggleBool.isOn = positionSensor3D.relative[0];
            boolDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.RotationSensor1D) {
            AddonRotationSensor1D rotationSensor1D = sourceGenome.addonRotationSensor1DList[sourceAddonIndex];

            GameObject vector3DisplayGO = (GameObject)Instantiate(vector3DisplayPrefab);
            PanelAddonDisplayVector3 vector3Display = vector3DisplayGO.GetComponent<PanelAddonDisplayVector3>();
            vector3Display.textVector3Name.text = "Local Axis:";
            vector3Display.sliderX.minValue = -1f;
            vector3Display.sliderX.maxValue = 1f;
            vector3Display.sliderX.value = rotationSensor1D.localAxis[0].x;
            vector3Display.textXValue.text = rotationSensor1D.localAxis[0].x.ToString();
            vector3Display.sliderY.minValue = -1f;
            vector3Display.sliderY.maxValue = 1f;
            vector3Display.sliderY.value = rotationSensor1D.localAxis[0].y;
            vector3Display.textYValue.text = rotationSensor1D.localAxis[0].y.ToString();
            vector3Display.sliderZ.minValue = -1f;
            vector3Display.sliderZ.maxValue = 1f;
            vector3Display.sliderZ.value = rotationSensor1D.localAxis[0].z;
            vector3Display.textZValue.text = rotationSensor1D.localAxis[0].z.ToString();
            vector3Display.linkedVector3Value[0] = rotationSensor1D.localAxis[0];
            rotationSensor1D.localAxis = vector3Display.linkedVector3Value;
            vector3DisplayGO.transform.SetParent(this.transform);

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Sensitivity:";
            floatDisplay1.sliderFloat.minValue = 0.01f;
            floatDisplay1.sliderFloat.maxValue = 10f;
            floatDisplay1.linkedFloatValue[0] = rotationSensor1D.sensitivity[0];
            rotationSensor1D.sensitivity = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = rotationSensor1D.sensitivity[0];
            floatDisplay1.textFloatValue.text = rotationSensor1D.sensitivity[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.RotationSensor3D) {
            AddonRotationSensor3D rotationSensor3D = sourceGenome.addonRotationSensor3DList[sourceAddonIndex];

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Sensitivity:";
            floatDisplay1.sliderFloat.minValue = 0.01f;
            floatDisplay1.sliderFloat.maxValue = 10f;
            floatDisplay1.linkedFloatValue[0] = rotationSensor3D.sensitivity[0];
            rotationSensor3D.sensitivity = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = rotationSensor3D.sensitivity[0];
            floatDisplay1.textFloatValue.text = rotationSensor3D.sensitivity[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.VelocitySensor1D) {
            AddonVelocitySensor1D velocitySensor1D = sourceGenome.addonVelocitySensor1DList[sourceAddonIndex];

            GameObject vector3DisplayGO = (GameObject)Instantiate(vector3DisplayPrefab);
            PanelAddonDisplayVector3 vector3Display = vector3DisplayGO.GetComponent<PanelAddonDisplayVector3>();
            vector3Display.textVector3Name.text = "Forward Vector:";
            vector3Display.sliderX.minValue = -1f;
            vector3Display.sliderX.maxValue = 1f;
            vector3Display.sliderX.value = velocitySensor1D.forwardVector[0].x;
            vector3Display.textXValue.text = velocitySensor1D.forwardVector[0].x.ToString();
            vector3Display.sliderY.minValue = -1f;
            vector3Display.sliderY.maxValue = 1f;
            vector3Display.sliderY.value = velocitySensor1D.forwardVector[0].y;
            vector3Display.textYValue.text = velocitySensor1D.forwardVector[0].y.ToString();
            vector3Display.sliderZ.minValue = -1f;
            vector3Display.sliderZ.maxValue = 1f;
            vector3Display.sliderZ.value = velocitySensor1D.forwardVector[0].z;
            vector3Display.textZValue.text = velocitySensor1D.forwardVector[0].z.ToString();
            vector3Display.linkedVector3Value[0] = velocitySensor1D.forwardVector[0];
            velocitySensor1D.forwardVector = vector3Display.linkedVector3Value;
            vector3DisplayGO.transform.SetParent(this.transform);

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Sensitivity:";
            floatDisplay1.sliderFloat.minValue = 0.01f;
            floatDisplay1.sliderFloat.maxValue = 10f;
            floatDisplay1.linkedFloatValue[0] = velocitySensor1D.sensitivity[0];
            velocitySensor1D.sensitivity = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = velocitySensor1D.sensitivity[0];
            floatDisplay1.textFloatValue.text = velocitySensor1D.sensitivity[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);

            GameObject boolDisplay1GO = (GameObject)Instantiate(boolDisplayPrefab);
            PanelAddonDisplayBool boolDisplay1 = boolDisplay1GO.GetComponent<PanelAddonDisplayBool>();
            boolDisplay1.textBoolName.text = "Relative:";
            boolDisplay1.linkedBoolValue[0] = velocitySensor1D.relative[0];
            velocitySensor1D.relative = boolDisplay1.linkedBoolValue;
            boolDisplay1.toggleBool.isOn = velocitySensor1D.relative[0];
            boolDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.VelocitySensor3D) {
            AddonVelocitySensor3D velocitySensor3D = sourceGenome.addonVelocitySensor3DList[sourceAddonIndex];

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Sensitivity:";
            floatDisplay1.sliderFloat.minValue = 0.01f;
            floatDisplay1.sliderFloat.maxValue = 10f;
            floatDisplay1.linkedFloatValue[0] = velocitySensor3D.sensitivity[0];
            velocitySensor3D.sensitivity = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = velocitySensor3D.sensitivity[0];
            floatDisplay1.textFloatValue.text = velocitySensor3D.sensitivity[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);

            GameObject boolDisplay1GO = (GameObject)Instantiate(boolDisplayPrefab);
            PanelAddonDisplayBool boolDisplay1 = boolDisplay1GO.GetComponent<PanelAddonDisplayBool>();
            boolDisplay1.textBoolName.text = "Relative:";
            boolDisplay1.linkedBoolValue[0] = velocitySensor3D.relative[0];
            velocitySensor3D.relative = boolDisplay1.linkedBoolValue;
            boolDisplay1.toggleBool.isOn = velocitySensor3D.relative[0];
            boolDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.Altimeter) {
            AddonAltimeter altimeter = sourceGenome.addonAltimeterList[sourceAddonIndex];
        }
        else if(sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.EarBasic) {
            AddonEarBasic earBasic = sourceGenome.addonEarBasicList[sourceAddonIndex];

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Sensitivity:";
            floatDisplay1.sliderFloat.minValue = 0.01f;
            floatDisplay1.sliderFloat.maxValue = 10f;
            floatDisplay1.linkedFloatValue[0] = earBasic.sensitivity[0];
            earBasic.sensitivity = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = earBasic.sensitivity[0];
            floatDisplay1.textFloatValue.text = earBasic.sensitivity[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.Gravity) {
            AddonGravitySensor gravitySensor = sourceGenome.addonGravitySensorList[sourceAddonIndex];
        }

        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.JointMotor) {
            AddonJointMotor jointMotor = sourceGenome.addonJointMotorList[sourceAddonIndex];
            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Motor Force:";
            floatDisplay1.sliderFloat.minValue = 1f;
            floatDisplay1.sliderFloat.maxValue = 500f;
            floatDisplay1.linkedFloatValue[0] = jointMotor.motorForce[0];
            jointMotor.motorForce = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = jointMotor.motorForce[0];
            floatDisplay1.textFloatValue.text = jointMotor.motorForce[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);

            GameObject floatDisplay2GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay2 = floatDisplay2GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay2.textFloatName.text = "Motor Speed:";
            floatDisplay2.sliderFloat.minValue = 1f;
            floatDisplay2.sliderFloat.maxValue = 500f;
            floatDisplay2.linkedFloatValue[0] = jointMotor.motorSpeed[0];
            jointMotor.motorSpeed = floatDisplay2.linkedFloatValue;
            floatDisplay2.sliderFloat.value = jointMotor.motorSpeed[0];
            floatDisplay2.textFloatValue.text = jointMotor.motorSpeed[0].ToString();
            floatDisplay2GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.ThrusterEffector1D) {
            AddonThrusterEffector1D thrusterEffector1D = sourceGenome.addonThrusterEffector1DList[sourceAddonIndex];

            GameObject vector3DisplayGO = (GameObject)Instantiate(vector3DisplayPrefab);
            PanelAddonDisplayVector3 vector3Display = vector3DisplayGO.GetComponent<PanelAddonDisplayVector3>();
            vector3Display.textVector3Name.text = "Forward Vector:";
            vector3Display.sliderX.minValue = -1f;
            vector3Display.sliderX.maxValue = 1f;
            vector3Display.sliderX.value = thrusterEffector1D.forwardVector[0].x;
            vector3Display.textXValue.text = thrusterEffector1D.forwardVector[0].x.ToString();
            vector3Display.sliderY.minValue = -1f;
            vector3Display.sliderY.maxValue = 1f;
            vector3Display.sliderY.value = thrusterEffector1D.forwardVector[0].y;
            vector3Display.textYValue.text = thrusterEffector1D.forwardVector[0].y.ToString();
            vector3Display.sliderZ.minValue = -1f;
            vector3Display.sliderZ.maxValue = 1f;
            vector3Display.sliderZ.value = thrusterEffector1D.forwardVector[0].z;
            vector3Display.textZValue.text = thrusterEffector1D.forwardVector[0].z.ToString();
            vector3Display.linkedVector3Value[0] = thrusterEffector1D.forwardVector[0];
            thrusterEffector1D.forwardVector = vector3Display.linkedVector3Value;
            vector3DisplayGO.transform.SetParent(this.transform);

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Max Force:";
            floatDisplay1.sliderFloat.minValue = 0.1f;
            floatDisplay1.sliderFloat.maxValue = 100f;
            floatDisplay1.linkedFloatValue[0] = thrusterEffector1D.maxForce[0];
            thrusterEffector1D.maxForce = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = thrusterEffector1D.maxForce[0];
            floatDisplay1.textFloatValue.text = thrusterEffector1D.maxForce[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.ThrusterEffector3D) {
            AddonThrusterEffector3D thrusterEffector3D = sourceGenome.addonThrusterEffector3DList[sourceAddonIndex];

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Max Force:";
            floatDisplay1.sliderFloat.minValue = 0.1f;
            floatDisplay1.sliderFloat.maxValue = 100f;
            floatDisplay1.linkedFloatValue[0] = thrusterEffector3D.maxForce[0];
            thrusterEffector3D.maxForce = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = thrusterEffector3D.maxForce[0];
            floatDisplay1.textFloatValue.text = thrusterEffector3D.maxForce[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.TorqueEffector1D) {
            AddonTorqueEffector1D torqueEffector1D = sourceGenome.addonTorqueEffector1DList[sourceAddonIndex];

            GameObject vector3DisplayGO = (GameObject)Instantiate(vector3DisplayPrefab);
            PanelAddonDisplayVector3 vector3Display = vector3DisplayGO.GetComponent<PanelAddonDisplayVector3>();
            vector3Display.textVector3Name.text = "Axis:";
            vector3Display.sliderX.minValue = -1f;
            vector3Display.sliderX.maxValue = 1f;
            vector3Display.sliderX.value = torqueEffector1D.axis[0].x;
            vector3Display.textXValue.text = torqueEffector1D.axis[0].x.ToString();
            vector3Display.sliderY.minValue = -1f;
            vector3Display.sliderY.maxValue = 1f;
            vector3Display.sliderY.value = torqueEffector1D.axis[0].y;
            vector3Display.textYValue.text = torqueEffector1D.axis[0].y.ToString();
            vector3Display.sliderZ.minValue = -1f;
            vector3Display.sliderZ.maxValue = 1f;
            vector3Display.sliderZ.value = torqueEffector1D.axis[0].z;
            vector3Display.textZValue.text = torqueEffector1D.axis[0].z.ToString();
            vector3Display.linkedVector3Value[0] = torqueEffector1D.axis[0];
            torqueEffector1D.axis = vector3Display.linkedVector3Value;
            vector3DisplayGO.transform.SetParent(this.transform);

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Max Torque:";
            floatDisplay1.sliderFloat.minValue = 0.1f;
            floatDisplay1.sliderFloat.maxValue = 100f;
            floatDisplay1.linkedFloatValue[0] = torqueEffector1D.maxTorque[0];
            torqueEffector1D.maxTorque = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = torqueEffector1D.maxTorque[0];
            floatDisplay1.textFloatValue.text = torqueEffector1D.maxTorque[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.TorqueEffector3D) {
            AddonTorqueEffector3D torqueEffector3D = sourceGenome.addonTorqueEffector3DList[sourceAddonIndex];

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Max Torque:";
            floatDisplay1.sliderFloat.minValue = 0.1f;
            floatDisplay1.sliderFloat.maxValue = 100f;
            floatDisplay1.linkedFloatValue[0] = torqueEffector3D.maxTorque[0];
            torqueEffector3D.maxTorque = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = torqueEffector3D.maxTorque[0];
            floatDisplay1.textFloatValue.text = torqueEffector3D.maxTorque[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.MouthBasic) {
            AddonMouthBasic mouthBasic = sourceGenome.addonMouthBasicList[sourceAddonIndex];
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.NoiseMakerBasic) {
            AddonNoiseMakerBasic noiseMakerBasic = sourceGenome.addonNoiseMakerBasicList[sourceAddonIndex];

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Amplitude:";
            floatDisplay1.sliderFloat.minValue = 0.1f;
            floatDisplay1.sliderFloat.maxValue = 10f;
            floatDisplay1.linkedFloatValue[0] = noiseMakerBasic.amplitude[0];
            noiseMakerBasic.amplitude = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = noiseMakerBasic.amplitude[0];
            floatDisplay1.textFloatValue.text = noiseMakerBasic.amplitude[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.Sticky) {
            AddonSticky sticky = sourceGenome.addonStickyList[sourceAddonIndex];
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.WeaponBasic) {
            AddonWeaponBasic weaponBasic = sourceGenome.addonWeaponBasicList[sourceAddonIndex];

            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Strength:";
            floatDisplay1.sliderFloat.minValue = 0.1f;
            floatDisplay1.sliderFloat.maxValue = 10f;
            floatDisplay1.linkedFloatValue[0] = weaponBasic.strength[0];
            weaponBasic.strength = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = weaponBasic.strength[0];
            floatDisplay1.textFloatValue.text = weaponBasic.strength[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);
        }

        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.OscillatorInput) {
            AddonOscillatorInput oscillatorInput = sourceGenome.addonOscillatorInputList[sourceAddonIndex];
            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Frequency:";
            floatDisplay1.sliderFloat.minValue = 0.05f;
            floatDisplay1.sliderFloat.maxValue = 20f;
            floatDisplay1.linkedFloatValue[0] = oscillatorInput.frequency[0];
            oscillatorInput.frequency = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = oscillatorInput.frequency[0];
            floatDisplay1.textFloatValue.text = oscillatorInput.frequency[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);

            GameObject floatDisplay2GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay2 = floatDisplay2GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay2.textFloatName.text = "Amplitude:";
            floatDisplay2.sliderFloat.minValue = 0.05f;
            floatDisplay2.sliderFloat.maxValue = 20f;
            floatDisplay2.linkedFloatValue[0] = oscillatorInput.amplitude[0];
            oscillatorInput.amplitude = floatDisplay2.linkedFloatValue;
            floatDisplay2.sliderFloat.value = oscillatorInput.amplitude[0];
            floatDisplay2.textFloatValue.text = oscillatorInput.amplitude[0].ToString();
            floatDisplay2GO.transform.SetParent(this.transform);

            GameObject floatDisplay3GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay3 = floatDisplay3GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay3.textFloatName.text = "Offset:";
            floatDisplay3.sliderFloat.minValue = -20f;
            floatDisplay3.sliderFloat.maxValue = 20f;
            floatDisplay3.linkedFloatValue[0] = oscillatorInput.offset[0];
            oscillatorInput.offset = floatDisplay3.linkedFloatValue;
            floatDisplay3.sliderFloat.value = oscillatorInput.offset[0];
            floatDisplay3.textFloatValue.text = oscillatorInput.offset[0].ToString();
            floatDisplay3GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.ValueInput) {
            AddonValueInput valueInput = sourceGenome.addonValueInputList[sourceAddonIndex];
            GameObject floatDisplay1GO = (GameObject)Instantiate(floatDisplayPrefab);
            PanelAddonDisplayFloat floatDisplay1 = floatDisplay1GO.GetComponent<PanelAddonDisplayFloat>();
            floatDisplay1.textFloatName.text = "Value:";
            floatDisplay1.sliderFloat.minValue = -5f;
            floatDisplay1.sliderFloat.maxValue = 5f;
            floatDisplay1.linkedFloatValue[0] = valueInput.value[0];
            valueInput.value = floatDisplay1.linkedFloatValue;
            floatDisplay1.sliderFloat.value = valueInput.value[0];
            floatDisplay1.textFloatValue.text = valueInput.value[0].ToString();
            floatDisplay1GO.transform.SetParent(this.transform);
        }
        else if (sourceAddonType == CritterNodeAddonBase.CritterNodeAddonTypes.TimerInput) {
            AddonTimerInput timerInput = sourceGenome.addonTimerInputList[sourceAddonIndex];
        }

    }

    public void ClickRemoveAddon() {
        
        panelAddonsList.panelNodeAddons.critterEditorState.RemoveAddon(sourceAddonType, sourceAddonIndex);
    }
}
