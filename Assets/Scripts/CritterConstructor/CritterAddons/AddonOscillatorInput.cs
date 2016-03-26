using UnityEngine;
using System.Collections;

public class AddonOscillatorInput {

    public int critterNodeID;
    public float[] frequency;
    public float[] amplitude;
    public float[] offset;

    public AddonOscillatorInput() {
        Debug.Log("Constructor AddonOscillatorInput()");
        frequency = new float[1];
        frequency[0] = 1f;
        amplitude = new float[1];
        amplitude[0] = 1f;
        offset = new float[1];
        offset[0] = 0f;
    }

    public AddonOscillatorInput(int id) {
        Debug.Log("Constructor AddonOscillatorInput(" + id.ToString() + ")");
        critterNodeID = id;
        frequency = new float[1];
        frequency[0] = 1f;
        amplitude = new float[1];
        amplitude[0] = 1f;
        offset = new float[1];
        offset[0] = 0f;
    }
}
