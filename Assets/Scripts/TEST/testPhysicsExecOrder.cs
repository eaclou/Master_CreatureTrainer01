using UnityEngine;
using System.Collections;

public class testPhysicsExecOrder : MonoBehaviour
{
    public GameObject segment1;
    public GameObject segment2;
    public int timeSteps = 0;

    void Awake()
    {
        Debug.Log("Awake()");
        Physics.gravity = new Vector3(0f, -100f, 0f);
        Time.timeScale = 0.1f;
    }
    // Use this for initialization
    void Start()
    {
        Debug.Log("Start()" + timeSteps.ToString());
        //segment1 = new GameObject("segment1");
        //segment1.AddComponent<MeshFilter>();
        //segment1.AddComponent<MeshRenderer>();
        //segment1.AddComponent<Rigidbody>();
        //segment1.AddComponent<BoxCollider>().isTrigger = true;
        //segment1.AddComponent<testPhysicsTrigger>().testPhysicsExecOrderScript = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        timeSteps++;
        Debug.Log("FixedUpdate() Start: " + timeSteps.ToString());
        
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        //Debug.Log("OnTriggerEnter() " + testPhysicsExecOrderScript.timeSteps.ToString());
        //Debug.Log("Segment RigidBodPos OnTriggerEnter(): ( " + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + " )");
    }

    void OnTriggerStay(Collider otherCollider)
    {        
        if(timeSteps <= 1)
        {
            Debug.Log("TESTPHYS OnTriggerStay triggered by: " + otherCollider.gameObject.name);
            segment1 = new GameObject("segment1");
            segment1.AddComponent<MeshFilter>();
            segment1.AddComponent<MeshRenderer>();
            segment1.AddComponent<Rigidbody>();
            segment1.AddComponent<SphereCollider>().isTrigger = true;
            segment1.AddComponent<testPhysicsTrigger>().testPhysicsExecOrderScript = this;
            //segment1.GetComponent<Rigidbody>().AddForce(Vector3.right * 10f);
            //segment1.GetComponent<Rigidbody>().velocity = new Vector3(1f, 0f, 0f);
            this.GetComponent<BoxCollider>().enabled = false;

            segment2 = new GameObject("segment2");
            segment2.transform.position = new Vector3(1f, 0f, 0f);
            segment2.AddComponent<Rigidbody>();
            segment2.AddComponent<SphereCollider>().isTrigger = true;
            segment2.AddComponent<testPhysicsTrigger>().testPhysicsExecOrderScript = this;
            ConfigurableJoint configJoint;
            configJoint = segment2.AddComponent<ConfigurableJoint>();
            configJoint.connectedBody = segment1.GetComponent<Rigidbody>();  // Set parent
            Vector3 anchorVec = new Vector3(-0.5f, 0f, 0f); // anchor to its left            
            configJoint.anchor = anchorVec;
            Vector3 connectedAnchorVec = new Vector3(0.5f, 0f, 0f);
            configJoint.connectedAnchor = connectedAnchorVec;

            // HINGE Z:
            configJoint.axis = new Vector3(0f, 1f, 0f);
            configJoint.secondaryAxis = new Vector3(1f, 0f, 0f);
            JointDrive jointDrive = configJoint.angularYZDrive;
            jointDrive.mode = JointDriveMode.Velocity;
            jointDrive.positionDamper = 1f;
            jointDrive.positionSpring = 1f;
            configJoint.angularYZDrive = jointDrive;
            // Lock mobility:
            configJoint.xMotion = ConfigurableJointMotion.Locked;
            configJoint.yMotion = ConfigurableJointMotion.Locked;
            configJoint.zMotion = ConfigurableJointMotion.Locked;
            configJoint.angularXMotion = ConfigurableJointMotion.Locked;
            configJoint.angularYMotion = ConfigurableJointMotion.Locked;
            configJoint.angularZMotion = ConfigurableJointMotion.Limited;
            // Joint Limits:
            SoftJointLimit limitZ = configJoint.angularZLimit;
            limitZ.limit = 90f;
            configJoint.angularZLimit = limitZ;

            //segment1.GetComponent<Rigidbody>().MovePosition(Vector3.forward * 1f);
            //segment2.GetComponent<Rigidbody>().MovePosition(Vector3.down * 0.2f + Vector3.right);
            //segment1.GetComponent<Rigidbody>().AddForce(Vector3.forward * 100f);
        }        
    }

}