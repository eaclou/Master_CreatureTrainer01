using UnityEngine;
using System.Collections;

public class SegaddonGravitySensor {

    public int segmentID;
    public float[] gravityDot;

    public SegaddonGravitySensor() {
        gravityDot = new float[1];
        gravityDot[0] = 0f;
    }

    public SegaddonGravitySensor(AddonGravitySensor sourceNode) {
        gravityDot = new float[1];
        gravityDot[0] = 0f;
    }
}
