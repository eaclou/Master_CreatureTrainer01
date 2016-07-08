using UnityEngine;
using System.Collections;

public class AddonRaycastSensor {

    public int critterNodeID;
    public int innov;
    public Vector3[] forwardVector;
    public float[] maxDistance;

    public AddonRaycastSensor() {
        Debug.Log("Constructor AddonRaycastSensor()");
        forwardVector = new Vector3[1];
        forwardVector[0] = new Vector3(0f, 0f, 1f);
        maxDistance = new float[1];
        maxDistance[0] = 10f;
    }

    public AddonRaycastSensor(int id, int inno) {
        Debug.Log("Constructor AddonRaycastSensor(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        forwardVector = new Vector3[1];
        forwardVector[0] = new Vector3(0f, 0f, 1f);
        maxDistance = new float[1];
        maxDistance[0] = 10f;
    }
}
