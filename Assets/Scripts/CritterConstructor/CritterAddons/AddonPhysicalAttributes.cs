using UnityEngine;
using System.Collections;

public class AddonPhysicalAttributes {

    public int critterNodeID;
    public int innov;
    //public bool[] isKinematic;
    public float[] dynamicFriction;
    public float[] staticFriction;
    public float[] bounciness;
    public bool[] freezePositionX;
    public bool[] freezePositionY;
    public bool[] freezePositionZ;
    public bool[] freezeRotationX;
    public bool[] freezeRotationY;
    public bool[] freezeRotationZ;

    public AddonPhysicalAttributes() {
        Debug.Log("Constructor AddonPhysicalAttributes()");
        dynamicFriction = new float[1];
        dynamicFriction[0] = 0.6f;
        staticFriction = new float[1];
        staticFriction[0] = 0.6f;
        bounciness = new float[1];
        bounciness[0] = 0f;
        freezePositionX = new bool[1];
        freezePositionX[0] = false;
        freezePositionY = new bool[1];
        freezePositionY[0] = false;
        freezePositionZ = new bool[1];
        freezePositionZ[0] = false;
        freezeRotationX = new bool[1];
        freezeRotationX[0] = false;
        freezeRotationY = new bool[1];
        freezeRotationY[0] = false;
        freezeRotationZ = new bool[1];
        freezeRotationZ[0] = false;
    }

    public AddonPhysicalAttributes(int id, int inno) {
        Debug.Log("Constructor AddonPhysicalAttributes(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        dynamicFriction = new float[1];
        dynamicFriction[0] = 0.6f;
        staticFriction = new float[1];
        staticFriction[0] = 0.6f;
        bounciness = new float[1];
        bounciness[0] = 0f;
        freezePositionX = new bool[1];
        freezePositionX[0] = false;
        freezePositionY = new bool[1];
        freezePositionY[0] = false;
        freezePositionZ = new bool[1];
        freezePositionZ[0] = false;
        freezeRotationX = new bool[1];
        freezeRotationX[0] = false;
        freezeRotationY = new bool[1];
        freezeRotationY[0] = false;
        freezeRotationZ = new bool[1];
        freezeRotationZ[0] = false;
    }
}
