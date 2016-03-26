using UnityEngine;
using System.Collections;

public class SegaddonValueInput {

    public int segmentID;  // move to base class?
    public float[] value;

    public SegaddonValueInput() {
        value = new float[1];
        value[0] = 1f;
    }

    public SegaddonValueInput(AddonValueInput sourceNode) {
        value = new float[1];
        value[0] = 1f;
    }
}
