using UnityEngine;
using System.Collections;

public class AddonGravitySensor {

    public int critterNodeID;

    public AddonGravitySensor() {
        Debug.Log("Constructor AddonGravitySensor()");
    }

    public AddonGravitySensor(int id) {
        Debug.Log("Constructor AddonGravitySensor(" + id.ToString() + ")");
        critterNodeID = id;
    }
}
