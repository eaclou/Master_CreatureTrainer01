
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_MiniGameSettingsSaves : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		MiniGameSettingsSaves data = (MiniGameSettingsSaves)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 1 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.angleSensorSensitivity);
        writer.Write(data.gravityStrength);
        writer.Write(data.groundBounce);
        writer.Write(data.groundFriction);
        writer.Write(data.groundPositionY);
        writer.Write(data.jointMotorForce);
        writer.Write(data.jointMotorSpeed);
        writer.Write(data.maxScoreDistance);
        writer.Write(data.maxTargetX);
        writer.Write(data.maxTargetY);
        writer.Write(data.maxTargetZ);
        writer.Write(data.minScoreDistance);
        writer.Write(data.minTargetX);
        writer.Write(data.minTargetY);
        writer.Write(data.minTargetZ);
        writer.Write(data.targetRadius);
        writer.Write(data.viscosityDrag);
        writer.Write(data.initForceMax);
        writer.Write(data.initForceMin);
        writer.Write(data.useRandomTargetPos);
        writer.Write(data.variableMass);
    }
	
	public override object Read(ES2Reader reader)
	{
		MiniGameSettingsSaves data = new MiniGameSettingsSaves();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		MiniGameSettingsSaves data = (MiniGameSettingsSaves)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.angleSensorSensitivity = reader.Read<float>();
            data.gravityStrength = reader.Read<float>();
            data.groundBounce = reader.Read<float>();
            data.groundFriction = reader.Read<float>();
            data.groundPositionY = reader.Read<float>();
            data.jointMotorForce = reader.Read<float>();
            data.jointMotorSpeed = reader.Read<float>();
            data.maxScoreDistance = reader.Read<float>();
            data.maxTargetX = reader.Read<float>();
            data.maxTargetY = reader.Read<float>();
            data.maxTargetZ = reader.Read<float>();
            data.minScoreDistance = reader.Read<float>();
            data.minTargetX = reader.Read<float>();
            data.minTargetY = reader.Read<float>();
            data.minTargetZ = reader.Read<float>();
            data.targetRadius = reader.Read<float>();
            data.viscosityDrag = reader.Read<float>();
            data.initForceMax = reader.Read<float>();
            data.initForceMin = reader.Read<float>();
            data.useRandomTargetPos = reader.Read<float>();
            data.variableMass = reader.Read<float>();
            if (fileVersion >= 1) {
                // new attributes
            }
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_MiniGameSettingsSaves():base(typeof(MiniGameSettingsSaves)){}
}
