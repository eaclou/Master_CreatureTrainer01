
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_CritterNodeAddonBase : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{        
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        Debug.Log("ES2UserType_CritterNodeAddonBase obj.Type: " + obj.GetType().ToString());

        // The WRITE method for each derived Class is done inside its individual ES2UserType class,
        // But the READ method for all inherited addon classes is done within THIS file's Read() method below:
        if (obj is AddonJointMotor) {
            Debug.Log("ES2UserType_CritterNodeAddonBase Write Data JOINT");
            AddonJointMotor data = (AddonJointMotor)obj;
            writer.Write(data);
        }
        else {
            Debug.Log("ES2UserType_CritterNodeAddonBase Write Data Base ERROR DO NOT SAVE!!!");
            //CritterNodeAddonBase data = (CritterNodeAddonBase)obj;
            //writer.Write(data.addonType);
        }
    }

    public override object Read(ES2Reader reader)
	{
        Debug.Log("ES2UserType_CritterNodeAddonBase reader: " + reader.Read<CritterNodeAddonBase.CritterNodeAddonTypes>().ToString());
        if(reader.Read<CritterNodeAddonBase.CritterNodeAddonTypes>() == CritterNodeAddonBase.CritterNodeAddonTypes.JointMotor) {
            Debug.Log("ES2UserType_CritterNodeAddonBase READ JOINT");
            AddonJointMotor data = new AddonJointMotor();
            data.addonType = reader.Read<CritterNodeAddonBase.CritterNodeAddonTypes>();
            data.motorForce = reader.ReadArray<System.Single>();
            //Read(reader, data);
            return data;
        }
        else {
            Debug.Log("ES2UserType_CritterNodeAddonBase READ BASE ERROR EMPTY!!!");
            CritterNodeAddonBase data = new CritterNodeAddonBase();
            //Read(reader, data);
            return data;
        }        
	}
	
	public override void Read(ES2Reader reader, object c)
	{
        // Add your reader.Read calls here to read the data into the object.
        // This method is not used
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_CritterNodeAddonBase():base(typeof(CritterNodeAddonBase)){}
}
