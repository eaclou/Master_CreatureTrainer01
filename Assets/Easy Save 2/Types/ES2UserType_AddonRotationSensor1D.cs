
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_AddonRotationSensor1D : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		AddonRotationSensor1D data = (AddonRotationSensor1D)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.critterNodeID);
        writer.Write(data.innov);
        writer.Write(data.localAxis);
        writer.Write(data.sensitivity);
    }
	
	public override object Read(ES2Reader reader)
	{
		AddonRotationSensor1D data = new AddonRotationSensor1D();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		AddonRotationSensor1D data = (AddonRotationSensor1D)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.critterNodeID = reader.Read<System.Int32>();
            data.innov = reader.Read<System.Int32>();
            data.localAxis = reader.ReadArray<Vector3>();
            data.sensitivity = reader.ReadArray<System.Single>();
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_AddonRotationSensor1D():base(typeof(AddonRotationSensor1D)){}
}
