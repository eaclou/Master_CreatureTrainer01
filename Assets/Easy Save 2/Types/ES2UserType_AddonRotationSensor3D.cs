
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_AddonRotationSensor3D : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		AddonRotationSensor3D data = (AddonRotationSensor3D)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.critterNodeID);
        writer.Write(data.sensitivity);
    }
	
	public override object Read(ES2Reader reader)
	{
		AddonRotationSensor3D data = new AddonRotationSensor3D();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		AddonRotationSensor3D data = (AddonRotationSensor3D)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.critterNodeID = reader.Read<System.Int32>();
            data.sensitivity = reader.ReadArray<System.Single>();
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_AddonRotationSensor3D():base(typeof(AddonRotationSensor3D)){}
}
