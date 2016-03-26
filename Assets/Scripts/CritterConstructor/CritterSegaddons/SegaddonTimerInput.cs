using UnityEngine;
using System.Collections;

public class SegaddonTimerInput {

    public int segmentID;  // move to base class?
    public float[] value;

    public SegaddonTimerInput() {
        value = new float[1];
        value[0] = 1f;
    }

    public SegaddonTimerInput(AddonTimerInput sourceNode) {
        value = new float[1];
        value[0] = 1f;
    }
}
