
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_FitnessComponentData : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		FitnessComponentData data = (FitnessComponentData)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 1 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.agentDataArray);
        writer.Write(data.highestScore);
        writer.Write(data.lowestScore);
        writer.Write(data.totalRawScore);
    }
	
	public override object Read(ES2Reader reader)
	{
		FitnessComponentData data = new FitnessComponentData();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		FitnessComponentData data = (FitnessComponentData)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.agentDataArray = reader.ReadArray<AgentData>();
            data.highestScore = reader.Read<float>();
            data.lowestScore = reader.Read<float>();
            data.totalRawScore = reader.Read<float>();
            if (fileVersion >= 1) {
                // new attributes
            }
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_FitnessComponentData():base(typeof(FitnessComponentData)){}
}
