
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Int3 : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Int3 data = (Int3)obj;
        // Add your writer.Write calls here.
        //writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.x);
        writer.Write(data.y);
        writer.Write(data.z);
    }
	
	public override object Read(ES2Reader reader)
	{
		Int3 data = new Int3();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		Int3 data = (Int3)c;
        // Add your reader.Read calls here to read the data into the object.
        data.x = reader.Read<int>();
        data.y = reader.Read<int>();
        data.z = reader.Read<int>();
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_Int3():base(typeof(Int3)){}
}
