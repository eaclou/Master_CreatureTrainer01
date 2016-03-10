
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_AddonJointMotor : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{       
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        Debug.Log("ES2UserType_AddonJointMotor obj.Type: " + obj.GetType().ToString());
        AddonJointMotor data = (AddonJointMotor)obj;
        writer.Write(data.addonType);
        writer.Write(data.motorForce);
    }

    public override object Read(ES2Reader reader)
	{
        Debug.Log("ES2UserType_AddonJointMotor READ READ");
        AddonJointMotor data = new AddonJointMotor();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
        Debug.Log("ES2UserType_AddonJointMotor c.Type: " + c.GetType().ToString());
        //AddonJointMotor data = (AddonJointMotor)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        //int fileVersion = reader.Read<int>();

        // VERSION 0:
        //if (fileVersion >= 0) {
        //    data.addonType = reader.Read<CritterNodeAddonBase.CritterNodeAddonTypes>();
        //    Debug.Log("ES2UserType_AddonJointMotor data.addonType: " + data.addonType.ToString());
        //}
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_AddonJointMotor():base(typeof(AddonJointMotor)){}
}
