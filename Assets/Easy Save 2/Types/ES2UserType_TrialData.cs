
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_TrialData : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		TrialData data = (TrialData)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 1 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.fitnessComponentDataArray);
        writer.Write(data.totalSumOfWeights);
    }
	
	public override object Read(ES2Reader reader)
	{
		TrialData data = new TrialData();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		TrialData data = (TrialData)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.fitnessComponentDataArray = reader.ReadArray<FitnessComponentData>();
            data.totalSumOfWeights = reader.Read<float>();
            if (fileVersion >= 1) {
                // new attributes
            }
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_TrialData():base(typeof(TrialData)){}
}
