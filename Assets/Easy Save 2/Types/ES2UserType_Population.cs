
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Population : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Population data = (Population)obj;
        // Add your writer.Write calls here.
        writer.Write(1); // Version 1 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.brainType);
		writer.Write(data.templateGenome);
		writer.Write(data.numInputNodes);
		writer.Write(data.numOutputNodes);
		writer.Write(data.populationMaxSize);
		writer.Write(data.numAgents);
		writer.Write(data.masterAgentArray);
        // VERSION 1:
    }

    public override object Read(ES2Reader reader)
	{
		Population data = new Population();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		Population data = (Population)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0)
        {
            data.brainType = reader.Read<Population.BrainType>();
            data.templateGenome = reader.Read<CritterGenome>();
            data.numInputNodes = reader.Read<System.Int32>();
            data.numOutputNodes = reader.Read<System.Int32>();
            data.populationMaxSize = reader.Read<System.Int32>();
            data.numAgents = reader.Read<System.Int32>();
            data.masterAgentArray = reader.ReadArray<Agent>();
            if (fileVersion >= 1)
            {
                // new attributes
            }
        }
	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_Population():base(typeof(Population)){}
}
