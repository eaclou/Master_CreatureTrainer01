using UnityEngine;
using System.Collections;

public class SegaddonTriggerDetector : MonoBehaviour {

    int numTriggers = 0;
    public SegaddonMouthBasic referencedMouth;
    	

    void OnTriggerEnter(Collider otherCollider) {
        numTriggers++;
        if(referencedMouth != null) {
            referencedMouth.contactStatus[0] += 1f;
        }
        else {
            //Debug.Log("referencedContactSensor == null!");
        }
        Debug.Log("TRIGGER! numContacts: " + numTriggers.ToString());
    }

    void OnTriggerStay(Collider otherCollider) {
        
    }

    void OnTriggerExit(Collider otherCollider) {
        numTriggers--;
        if(numTriggers <= 0) {   // exited all collisions
            if (referencedMouth != null) {
                referencedMouth.contactStatus[0] = 0f;
            }
        }        
    }
}
