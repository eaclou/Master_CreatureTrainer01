
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_AddonOscillatorInput : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		AddonOscillatorInput data = (AddonOscillatorInput)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.critterNodeID);
        writer.Write(data.innov);
        writer.Write(data.frequency);
        writer.Write(data.amplitude);
        writer.Write(data.offset);
    }
	
	public override object Read(ES2Reader reader)
	{
		AddonOscillatorInput data = new AddonOscillatorInput();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		AddonOscillatorInput data = (AddonOscillatorInput)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.critterNodeID = reader.Read<System.Int32>();
            data.innov = reader.Read<System.Int32>();
            data.frequency = reader.ReadArray<System.Single>();
            data.amplitude = reader.ReadArray<System.Single>();
            data.offset = reader.ReadArray<System.Single>();
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_AddonOscillatorInput():base(typeof(AddonOscillatorInput)){}
}
