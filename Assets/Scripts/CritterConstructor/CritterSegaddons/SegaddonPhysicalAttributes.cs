using UnityEngine;
using System.Collections;

public class SegaddonPhysicalAttributes {

    public int segmentID;  // move to base class?
    public float[] dynamicFriction;
    public float[] staticFriction;
    public float[] bounciness;
    public bool[] freezePositionX;
    public bool[] freezePositionY;
    public bool[] freezePositionZ;
    public bool[] freezeRotationX;
    public bool[] freezeRotationY;
    public bool[] freezeRotationZ;

    public SegaddonPhysicalAttributes() {
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

    public SegaddonPhysicalAttributes(AddonPhysicalAttributes sourceNode) {
        dynamicFriction = new float[1];
        dynamicFriction[0] = sourceNode.dynamicFriction[0];
        staticFriction = new float[1];
        staticFriction[0] = sourceNode.staticFriction[0];
        bounciness = new float[1];
        bounciness[0] = sourceNode.bounciness[0];
        freezePositionX = new bool[1];
        freezePositionX[0] = sourceNode.freezePositionX[0];
        freezePositionY = new bool[1];
        freezePositionY[0] = sourceNode.freezePositionY[0];
        freezePositionZ = new bool[1];
        freezePositionZ[0] = sourceNode.freezePositionZ[0];
        freezeRotationX = new bool[1];
        freezeRotationX[0] = sourceNode.freezeRotationX[0];
        freezeRotationY = new bool[1];
        freezeRotationY[0] = sourceNode.freezeRotationY[0];
        freezeRotationZ = new bool[1];
        freezeRotationZ[0] = sourceNode.freezeRotationZ[0];
    }
}
