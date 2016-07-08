
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_AddonPositionSensor1D : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		AddonPositionSensor1D data = (AddonPositionSensor1D)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.critterNodeID);
        writer.Write(data.innov);
        writer.Write(data.forwardVector);
        writer.Write(data.relative);
        writer.Write(data.sensitivity);
    }
	
	public override object Read(ES2Reader reader)
	{
		AddonPositionSensor1D data = new AddonPositionSensor1D();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		AddonPositionSensor1D data = (AddonPositionSensor1D)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.critterNodeID = reader.Read<System.Int32>();
            data.innov = reader.Read<System.Int32>();
            data.forwardVector = reader.ReadArray<Vector3>();
            data.relative = reader.ReadArray<System.Boolean>();
            data.sensitivity = reader.ReadArray<System.Single>();
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_AddonPositionSensor1D():base(typeof(AddonPositionSensor1D)){}
}
