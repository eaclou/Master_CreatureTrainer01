
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_SpeciesBreedingPool : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		SpeciesBreedingPool data = (SpeciesBreedingPool)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 1 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.agentList);
        writer.Write(data.nextAgentIndex);
        writer.Write(data.speciesID);
        writer.Write(data.templateGenome);
    }
	
	public override object Read(ES2Reader reader)
	{
		SpeciesBreedingPool data = new SpeciesBreedingPool();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		SpeciesBreedingPool data = (SpeciesBreedingPool)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.agentList = reader.ReadList<Agent>();
            data.nextAgentIndex = reader.Read<int>();
            data.speciesID = reader.Read<int>();
            data.templateGenome = reader.Read<GenomeNEAT>();
            if (fileVersion >= 1) {
                // new attributes
            }
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_SpeciesBreedingPool():base(typeof(SpeciesBreedingPool)){}
}
