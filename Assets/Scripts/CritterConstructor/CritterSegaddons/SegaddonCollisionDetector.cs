using UnityEngine;
using System.Collections;

public class SegaddonCollisionDetector : MonoBehaviour {

    int numContacts = 0;
    public SegaddonContactSensor referencedContactSensor;
    	

    void OnCollisionEnter(Collision col) {
        numContacts++;
        if(referencedContactSensor != null) {
            referencedContactSensor.contactStatus[0] = col.impulse.magnitude * referencedContactSensor.contactSensitivity[0];
            referencedContactSensor.fitnessContact[0] = 1f;
        }
        else {
            Debug.Log("referencedContactSensor == null!");
        }
        Debug.Log("COLLISION! numContacts: " + numContacts.ToString());
    }

    void OnCollisionStay(Collision col) {
        
    }

    void OnCollisionExit(Collision col) {
        numContacts--;
        if(numContacts <= 0) {   // exited all collisions
            if (referencedContactSensor != null) {
                referencedContactSensor.contactStatus[0] = 0f;
                referencedContactSensor.fitnessContact[0] = 0f;
            }
        }        
    }
}
