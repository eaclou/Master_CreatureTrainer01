
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_FitnessComponent : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		FitnessComponent data = (FitnessComponent)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 1 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.bigIsBetter);
        writer.Write(data.componentName);
        writer.Write(data.divideByTimeSteps);
        writer.Write(data.on);
        writer.Write(data.power);
        writer.Write(data.weight);
    }
	
	public override object Read(ES2Reader reader)
	{
		FitnessComponent data = new FitnessComponent();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		FitnessComponent data = (FitnessComponent)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.bigIsBetter = reader.Read<bool>();
            data.componentName = reader.Read<string>();
            data.divideByTimeSteps = reader.Read<bool>();
            data.on = reader.Read<bool>();
            data.power = reader.Read<float>();
            data.weight = reader.Read<float>();
            if (fileVersion >= 1) {
                // new attributes
            }
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_FitnessComponent():base(typeof(FitnessComponent)){}
}
