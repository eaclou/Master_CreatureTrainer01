
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_CreatureBodySegmentGenome : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		CreatureBodySegmentGenome data = (CreatureBodySegmentGenome)obj;
        // Add your writer.Write calls here.
        writer.Write(1); // Version 1 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.id);
        writer.Write(data.parentID);
        writer.Write(data.size);
        writer.Write(data.attachPointParent);
        writer.Write(data.attachPointChild);
        writer.Write(data.parentAttachSide);
        writer.Write(data.jointType);
        writer.Write(data.jointLimitsMin);
        writer.Write(data.jointLimitsMax);
        writer.Write(data.jointSpeed);
        writer.Write(data.jointStrength);
        // Version 1:
        writer.Write(data.addOn1);
        writer.Write(data.addOn2);        
}

    public override object Read(ES2Reader reader)
	{
		CreatureBodySegmentGenome data = new CreatureBodySegmentGenome();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		CreatureBodySegmentGenome data = (CreatureBodySegmentGenome)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();
        
        // VERSION 0:
        if(fileVersion >= 0) { 
            data.id = reader.Read<System.Int32>();
            data.parentID = reader.Read<System.Int32>();
            data.size = reader.Read<UnityEngine.Vector3>();
            data.attachPointParent = reader.Read<UnityEngine.Vector3>();
            data.attachPointChild = reader.Read<UnityEngine.Vector3>();
            data.parentAttachSide = reader.Read<CreatureBodySegmentGenome.ParentAttachSide>();
            data.jointType = reader.Read<CreatureBodySegmentGenome.JointType>();
            data.jointLimitsMin = reader.Read<UnityEngine.Vector3>();
            data.jointLimitsMax = reader.Read<UnityEngine.Vector3>();
            data.jointSpeed = reader.Read<System.Single>();
            data.jointStrength = reader.Read<System.Single>();

            if (fileVersion >= 1)
            {
                data.addOn1 = reader.Read<CreatureBodySegmentGenome.AddOns>();
                data.addOn2 = reader.Read<CreatureBodySegmentGenome.AddOns>();
            }
        }        
    }

    /* ! Don't modify anything below this line ! */
    public ES2UserType_CreatureBodySegmentGenome():base(typeof(CreatureBodySegmentGenome)){}
}
