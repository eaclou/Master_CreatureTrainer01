
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_CritterNode : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		CritterNode data = (CritterNode)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        //writer.Write(data.attachedJointLinkList);
        //writer.Write(data.jointLink); 
        writer.Write(data.attachedChildNodesIdList);
        writer.Write(data.jointLink);       
        writer.Write(data.dimensions);
        writer.Write(data.ID);
    }
	
	public override object Read(ES2Reader reader)
	{
		CritterNode data = new CritterNode();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		CritterNode data = (CritterNode)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            //data.attachedJointLinkList = reader.ReadList<CritterJointLink>();
            //if (data.jointLink != null) 
            data.attachedChildNodesIdList = reader.ReadList<System.Int32>();           
            data.jointLink = reader.Read<CritterJointLink>();           
            data.dimensions = reader.Read<Vector3>();
            data.ID = reader.Read<System.Int32>();
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_CritterNode():base(typeof(CritterNode)){}
}
