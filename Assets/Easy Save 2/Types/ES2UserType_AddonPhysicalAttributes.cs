
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_AddonPhysicalAttributes : ES2Type {
    public override void Write(object obj, ES2Writer writer) {
        AddonPhysicalAttributes data = (AddonPhysicalAttributes)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 0 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.critterNodeID);
        writer.Write(data.dynamicFriction);
        writer.Write(data.staticFriction);
        writer.Write(data.bounciness);
        writer.Write(data.freezePositionX);
        writer.Write(data.freezePositionY);
        writer.Write(data.freezePositionZ);
        writer.Write(data.freezeRotationX);
        writer.Write(data.freezeRotationY);
        writer.Write(data.freezeRotationZ);
    }

    public override object Read(ES2Reader reader) {
        AddonPhysicalAttributes data = new AddonPhysicalAttributes();
        Read(reader, data);
        return data;
    }

    public override void Read(ES2Reader reader, object c) {
        AddonPhysicalAttributes data = (AddonPhysicalAttributes)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.critterNodeID = reader.Read<System.Int32>();
            data.dynamicFriction = reader.ReadArray<System.Single>();
            data.staticFriction = reader.ReadArray<System.Single>();
            data.bounciness = reader.ReadArray<System.Single>();
            data.freezePositionX = reader.ReadArray<System.Boolean>();
            data.freezePositionY = reader.ReadArray<System.Boolean>();
            data.freezePositionZ = reader.ReadArray<System.Boolean>();
            data.freezeRotationX = reader.ReadArray<System.Boolean>();
            data.freezeRotationY = reader.ReadArray<System.Boolean>();
            data.freezeRotationZ = reader.ReadArray<System.Boolean>();
        }
    }

    /* ! Don't modify anything below this line ! */
    public ES2UserType_AddonPhysicalAttributes() : base(typeof(AddonPhysicalAttributes)) { }
}
