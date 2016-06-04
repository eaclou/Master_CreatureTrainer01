
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_GeneNodeNEAT : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		GeneNodeNEAT data = (GeneNodeNEAT)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.activationFunction);
        writer.Write(data.id);
        writer.Write(data.nodeType);
    }
	
	public override object Read(ES2Reader reader)
	{
		GeneNodeNEAT data = new GeneNodeNEAT();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		GeneNodeNEAT data = (GeneNodeNEAT)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.activationFunction = reader.Read<TransferFunctions.TransferFunction>();
            data.id = reader.Read<int>();
            data.nodeType = reader.Read<GeneNodeNEAT.GeneNodeType>();
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_GeneNodeNEAT():base(typeof(GeneNodeNEAT)){}
}
