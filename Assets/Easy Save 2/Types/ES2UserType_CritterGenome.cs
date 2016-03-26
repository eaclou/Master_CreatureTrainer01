
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_CritterGenome : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		CritterGenome data = (CritterGenome)obj;
        // Add your writer.Write calls here.
        writer.Write(1); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.CritterNodeList);

        writer.Write(data.addonPhysicalAttributesList);
        
        writer.Write(data.addonJointAngleSensorList);
        writer.Write(data.addonContactSensorList);
        writer.Write(data.addonRaycastSensorList);
        writer.Write(data.addonCompassSensor1DList);
        writer.Write(data.addonCompassSensor3DList);
        writer.Write(data.addonPositionSensor1DList);
        writer.Write(data.addonPositionSensor3DList);
        writer.Write(data.addonRotationSensor1DList);
        writer.Write(data.addonRotationSensor3DList);
        writer.Write(data.addonVelocitySensor1DList);
        writer.Write(data.addonVelocitySensor3DList);
        writer.Write(data.addonAltimeterList);

        writer.Write(data.addonJointMotorList);
        writer.Write(data.addonThrusterEffector1DList);
        writer.Write(data.addonThrusterEffector3DList);
        writer.Write(data.addonTorqueEffector1DList);
        writer.Write(data.addonTorqueEffector3DList);

        writer.Write(data.addonOscillatorInputList);
        writer.Write(data.addonValueInputList);
        writer.Write(data.addonTimerInputList);
    }
	
	public override object Read(ES2Reader reader)
	{
		CritterGenome data = new CritterGenome();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		CritterGenome data = (CritterGenome)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.CritterNodeList = reader.ReadList<CritterNode>();

            data.addonPhysicalAttributesList = reader.ReadList<AddonPhysicalAttributes>();

            data.addonJointAngleSensorList = reader.ReadList<AddonJointAngleSensor>();
            data.addonContactSensorList = reader.ReadList<AddonContactSensor>();
            data.addonRaycastSensorList = reader.ReadList<AddonRaycastSensor>();
            data.addonCompassSensor1DList = reader.ReadList<AddonCompassSensor1D>();
            data.addonCompassSensor3DList = reader.ReadList<AddonCompassSensor3D>();
            data.addonPositionSensor1DList = reader.ReadList<AddonPositionSensor1D>();
            data.addonPositionSensor3DList = reader.ReadList<AddonPositionSensor3D>();
            data.addonRotationSensor1DList = reader.ReadList<AddonRotationSensor1D>();
            data.addonRotationSensor3DList = reader.ReadList<AddonRotationSensor3D>();
            data.addonVelocitySensor1DList = reader.ReadList<AddonVelocitySensor1D>();
            data.addonVelocitySensor3DList = reader.ReadList<AddonVelocitySensor3D>();
            data.addonAltimeterList = reader.ReadList<AddonAltimeter>();

            data.addonJointMotorList = reader.ReadList<AddonJointMotor>();
            data.addonThrusterEffector1DList = reader.ReadList<AddonThrusterEffector1D>();
            data.addonThrusterEffector3DList = reader.ReadList<AddonThrusterEffector3D>();
            data.addonTorqueEffector1DList = reader.ReadList<AddonTorqueEffector1D>();
            data.addonTorqueEffector3DList = reader.ReadList<AddonTorqueEffector3D>();

            data.addonOscillatorInputList = reader.ReadList<AddonOscillatorInput>();
            data.addonValueInputList = reader.ReadList<AddonValueInput>();
            data.addonTimerInputList = reader.ReadList<AddonTimerInput>();
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_CritterGenome():base(typeof(CritterGenome)){}
}
