
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Agent : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Agent data = (Agent)obj;
        // Add your writer.Write calls here.
        writer.Write(1); // Version 1 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.genome);
		writer.Write(data.bodyGenome);
        // VERSION 1:

    }

    public override object Read(ES2Reader reader)
	{
		Agent data = new Agent();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		Agent data = (Agent)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0)
        {
            data.genome = reader.Read<Genome>();
            data.bodyGenome = reader.Read<CreatureBodyGenome>();
            if (fileVersion >= 1)
            {
                // new attributes
            }
        }
	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_Agent():base(typeof(Agent)){}
}
