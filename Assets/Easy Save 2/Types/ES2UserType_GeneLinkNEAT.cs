
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_GeneLinkNEAT : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		GeneLinkNEAT data = (GeneLinkNEAT)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.enabled);
        writer.Write(data.fromNodeID);
        writer.Write(data.innov);
        writer.Write(data.toNodeID);
        writer.Write(data.weight);
        writer.Write(data.birthGen);
    }

    public override object Read(ES2Reader reader)
	{
		GeneLinkNEAT data = new GeneLinkNEAT();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		GeneLinkNEAT data = (GeneLinkNEAT)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.enabled = reader.Read<bool>();
            data.fromNodeID = reader.Read<int>();
            data.innov = reader.Read<int>();
            data.toNodeID = reader.Read<int>();
            data.weight = reader.Read<float>();
            data.birthGen = reader.Read<int>();
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_GeneLinkNEAT():base(typeof(GeneLinkNEAT)){}
}
