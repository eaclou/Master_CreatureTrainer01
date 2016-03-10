
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_CritterJointLink : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		CritterJointLink data = (CritterJointLink)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.thisNodeID);
        writer.Write(data.parentNodeID);
        writer.Write(data.attachDir);        
        //writer.Write(data.thisNode);
        writer.Write(data.jointLimitPrimary);
        writer.Write(data.jointLimitSecondary);
        writer.Write(data.jointType);
        writer.Write(data.numberOfRecursions);
        writer.Write(data.onlyAttachToTailNode);
        //if (data.parentNode != null)
        //writer.Write(data.parentNode);
        writer.Write(data.recursionForward);
        writer.Write(data.recursionScalingFactor);
        writer.Write(data.restAngleDir);
        writer.Write(data.symmetryType);
    }
	
	public override object Read(ES2Reader reader)
	{
		CritterJointLink data = new CritterJointLink();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		CritterJointLink data = (CritterJointLink)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.thisNodeID = reader.Read<System.Int32>();
            data.parentNodeID = reader.Read<System.Int32>();
            data.attachDir = reader.Read<Vector3>();
            //data.thisNode = reader.Read<CritterNode>();
            data.jointLimitPrimary = reader.Read<System.Single>();
            data.jointLimitSecondary = reader.Read<System.Single>();
            data.jointType = reader.Read<CritterJointLink.JointType>();
            data.numberOfRecursions = reader.Read<System.Int32>();
            data.onlyAttachToTailNode = reader.Read<System.Boolean>();
            //if (data.parentNode != null)
            //data.parentNode = reader.Read<CritterNode>();
            data.recursionForward = reader.Read<System.Single>();
            data.recursionScalingFactor = reader.Read<System.Single>();
            data.restAngleDir = reader.Read<Vector3>();
            data.symmetryType = reader.Read<CritterJointLink.SymmetryType>();
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_CritterJointLink():base(typeof(CritterJointLink)){}
}
