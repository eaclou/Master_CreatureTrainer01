
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Genome : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Genome data = (Genome)obj;
        // Add your writer.Write calls here.
        writer.Write(1); // Version 1 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.genomeWeights);
		writer.Write(data.genomeBiases);
		writer.Write(data.layerSizes);
        // VERSION 1:
    }

    public override object Read(ES2Reader reader)
	{
		Genome data = new Genome();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		Genome data = (Genome)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0)
        {
            data.genomeWeights = reader.ReadArray<System.Single>();
            data.genomeBiases = reader.ReadArray<System.Single>();
            data.layerSizes = reader.ReadArray<System.Int32>();
            if (fileVersion >= 1)
            {
                // new attributes
            }
        }
	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_Genome():base(typeof(Genome)){}
}
