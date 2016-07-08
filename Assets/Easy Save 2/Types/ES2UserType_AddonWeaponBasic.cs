
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_AddonWeaponBasic : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		AddonWeaponBasic data = (AddonWeaponBasic)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.critterNodeID);
        writer.Write(data.innov);
        writer.Write(data.strength);
    }
	
	public override object Read(ES2Reader reader)
	{
		AddonWeaponBasic data = new AddonWeaponBasic();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		AddonWeaponBasic data = (AddonWeaponBasic)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.critterNodeID = reader.Read<System.Int32>();
            data.innov = reader.Read<System.Int32>();
            data.strength = reader.ReadArray<float>();
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_AddonWeaponBasic():base(typeof(AddonWeaponBasic)){}
}
