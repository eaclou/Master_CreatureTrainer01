using UnityEngine;
using System.Collections;

public class SegaddonContactSensor {

    public int segmentID;
    public float[] contactSensitivity;
    public float[] contactStatus;
    public float[] fitnessContact;

    public SegaddonContactSensor() {
        contactSensitivity = new float[1];
        contactSensitivity[0] = 0f;
        contactStatus = new float[1];
        contactStatus[0] = -1f;
        fitnessContact = new float[1];
        fitnessContact[0] = 0f;
    }

    public SegaddonContactSensor(AddonContactSensor sourceNode) {
        contactSensitivity = new float[1];
        contactSensitivity[0] = sourceNode.contactSensitivity[0];
        contactStatus = new float[1];
        contactStatus[0] = -1f;
        fitnessContact = new float[1];
        fitnessContact[0] = 0f;
    }
}
