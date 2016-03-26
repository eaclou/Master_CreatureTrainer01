using UnityEngine;
using System.Collections;

public class SegaddonOscillatorInput {

    public int segmentID;  // move to base class?
    public float[] value;
    public float[] frequency;
    public float[] amplitude;
    public float[] offset;

    public SegaddonOscillatorInput() {
        value = new float[1];
        value[0] = 1f;
        frequency = new float[1];
        frequency[0] = 1f;
        amplitude = new float[1];
        amplitude[0] = 1f;
        offset = new float[1];
        offset[0] = 0f;
    }

    public SegaddonOscillatorInput(AddonOscillatorInput sourceNode) {
        value = new float[1];
        value[0] = 1f;
        frequency = new float[1];
        frequency[0] = sourceNode.frequency[0];
        amplitude = new float[1];
        amplitude[0] = sourceNode.amplitude[0];
        offset = new float[1];
        offset[0] = sourceNode.offset[0];
    }
}
