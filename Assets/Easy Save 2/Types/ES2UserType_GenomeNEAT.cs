
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_GenomeNEAT : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		GenomeNEAT data = (GenomeNEAT)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.nodeNEATList);
        writer.Write(data.linkNEATList);

    }
	
	public override object Read(ES2Reader reader)
	{
		GenomeNEAT data = new GenomeNEAT();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		GenomeNEAT data = (GenomeNEAT)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.nodeNEATList = reader.ReadList<GeneNodeNEAT>();
            data.linkNEATList = reader.ReadList<GeneLinkNEAT>();
        }
    }

    /* ! Don't modify anything below this line ! */
    public ES2UserType_GenomeNEAT():base(typeof(GenomeNEAT)){}
}
