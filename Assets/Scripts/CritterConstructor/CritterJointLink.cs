using UnityEngine;
using System.Collections;

public class CritterJointLink {

    public CritterNode childNode;
    public CritterNode parentNode;

    public Vector3 attachCoords;

    public CritterJointLink() {
        Debug.Log("CritterJointLink Constructor()!");
    }

    public void MoveAttachCoords(Vector3 newAttachCoords) { // sets the position where this joint attaches to its parent

    }
}
