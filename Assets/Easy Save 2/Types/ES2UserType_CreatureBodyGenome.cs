
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_CreatureBodyGenome : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		CreatureBodyGenome data = (CreatureBodyGenome)obj;
        // Add your writer.Write calls here.
        writer.Write(2); // Version 2 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.creatureBodySegmentGenomeList);
        // VERSION 1:
        // VERSION 2:
        writer.Write(data.initialTotalVolume);
    }

    public override object Read(ES2Reader reader)
	{
		CreatureBodyGenome data = new CreatureBodyGenome();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		CreatureBodyGenome data = (CreatureBodyGenome)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0)
        {
            data.creatureBodySegmentGenomeList = reader.ReadList<CreatureBodySegmentGenome>();
            if (fileVersion >= 1)
            {
                // new attributes
            }
            if (fileVersion >= 2)
            {
                // new attributes:
                data.initialTotalVolume = reader.Read<float>();
            }
        }

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_CreatureBodyGenome():base(typeof(CreatureBodyGenome)){}
}
