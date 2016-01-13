using UnityEngine;
using System.Collections;

public class CreatureBodySegmentGenome { // holds the information for each body segment a creature has

	public int id;
	public int parentID = -1;
	//public Vector3 centerPos;	
	public Vector3 size = new Vector3(1f, 1f, 1f);	
	public Vector3 attachPointParent = new Vector3(0f, 0f, 0f);
	public Vector3 attachPointChild = new Vector3(0f, 0f, 0f);
	
	public ParentAttachSide parentAttachSide;
	public enum ParentAttachSide {
		xPos,
		yPos,
		zPos,
		xNeg,
		yNeg,
		zNeg
	};
	
	public JointType jointType;
	public enum JointType {
		Fixed,
		HingeX,
		HingeY,
		HingeZ,
		DualXY,
		DualYZ,
		DualXZ,
		Full
	};

	public AddOns addOn1;
	public AddOns addOn2;
	public enum AddOns {
		None,
		ContactSensor,
		CompassSensor1D,
		CompassSensor3D,
		RangeSensor,
		JetEffector,
		GrabEffector,
        Mouth
	};
	
	public Vector3 jointLimitsMin = new Vector3(-45f, -45f, -45f);
	public Vector3 jointLimitsMax = new Vector3(45f, 45f, 45f);
	public float jointSpeed = 100f;
	public float jointStrength = 1000f;

	public void CopySettingsFromGraphNode(CreatureSegmentNode graphNode) {
		id = graphNode.id;
		parentID = graphNode.parentID;
		size = graphNode.size;
		attachPointParent = graphNode.attachPointParent;
		attachPointChild = graphNode.attachPointChild;

		if(graphNode.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.xNeg) {
			parentAttachSide = ParentAttachSide.xNeg;
		}
		else if(graphNode.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.yNeg) {
			parentAttachSide = ParentAttachSide.yNeg;
		}
		else if(graphNode.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.zNeg) {
			parentAttachSide = ParentAttachSide.zNeg;
		}
		else if(graphNode.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.xPos) {
			parentAttachSide = ParentAttachSide.xPos;
		}
		else if(graphNode.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.yPos) {
			parentAttachSide = ParentAttachSide.yPos;
		}
		else if(graphNode.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.zPos) {
			parentAttachSide = ParentAttachSide.zPos;
		}

		if(graphNode.jointPresetType == CreatureSegmentNode.JointPresetType.HingeX) {
			jointType = JointType.HingeX;
		}
		else if(graphNode.jointPresetType == CreatureSegmentNode.JointPresetType.HingeY) {
			jointType = JointType.HingeY;
		}
		else if(graphNode.jointPresetType == CreatureSegmentNode.JointPresetType.HingeZ) {
			jointType = JointType.HingeZ;
		}
		else if(graphNode.jointPresetType == CreatureSegmentNode.JointPresetType.DualXY) {
			jointType = JointType.DualXY;
		}
		else if(graphNode.jointPresetType == CreatureSegmentNode.JointPresetType.DualYZ) {
			jointType = JointType.DualYZ;
		}
		else if(graphNode.jointPresetType == CreatureSegmentNode.JointPresetType.DualXZ) {
			jointType = JointType.DualXZ;
		}
		else if(graphNode.jointPresetType == CreatureSegmentNode.JointPresetType.Full) {
			jointType = JointType.Full;
		}
		else if(graphNode.jointPresetType == CreatureSegmentNode.JointPresetType.Fixed) {
			jointType = JointType.Fixed;
		}

		if(graphNode.nodeAddOn1 == CreatureSegmentNode.NodeAddOns.None) {
			addOn1 = AddOns.None;
		}
		else if(graphNode.nodeAddOn1 == CreatureSegmentNode.NodeAddOns.CompassSensor1D) {
			addOn1 = AddOns.CompassSensor1D;
		}
		else if(graphNode.nodeAddOn1 == CreatureSegmentNode.NodeAddOns.CompassSensor3D) {
			addOn1 = AddOns.CompassSensor3D;
		}
		else if(graphNode.nodeAddOn1 == CreatureSegmentNode.NodeAddOns.ContactSensor) {
			addOn1 = AddOns.ContactSensor;
		}
		else if(graphNode.nodeAddOn1 == CreatureSegmentNode.NodeAddOns.RangeSensor) {
			addOn1 = AddOns.RangeSensor;
		}
		else if(graphNode.nodeAddOn1 == CreatureSegmentNode.NodeAddOns.GrabEffector) {
			addOn1 = AddOns.GrabEffector;
		}
		else if(graphNode.nodeAddOn1 == CreatureSegmentNode.NodeAddOns.JetEffector) {
			addOn1 = AddOns.JetEffector;
		}

		if(graphNode.nodeAddOn2 == CreatureSegmentNode.NodeAddOns.None) {
			addOn2 = AddOns.None;
		}
		else if(graphNode.nodeAddOn2 == CreatureSegmentNode.NodeAddOns.CompassSensor1D) {
			addOn2 = AddOns.CompassSensor1D;
		}
		else if(graphNode.nodeAddOn2 == CreatureSegmentNode.NodeAddOns.CompassSensor3D) {
			addOn2 = AddOns.CompassSensor3D;
		}
		else if(graphNode.nodeAddOn2 == CreatureSegmentNode.NodeAddOns.ContactSensor) {
			addOn2 = AddOns.ContactSensor;
		}
		else if(graphNode.nodeAddOn2 == CreatureSegmentNode.NodeAddOns.RangeSensor) {
			addOn2 = AddOns.RangeSensor;
		}
		else if(graphNode.nodeAddOn2 == CreatureSegmentNode.NodeAddOns.GrabEffector) {
			addOn2 = AddOns.GrabEffector;
		}
		else if(graphNode.nodeAddOn2 == CreatureSegmentNode.NodeAddOns.JetEffector) {
			addOn2 = AddOns.JetEffector;
		}

		//jointType = graphNode.jointPresetType;
		jointLimitsMin = graphNode.jointLimitsMin;
		jointLimitsMax = graphNode.jointLimitsMax;
		jointSpeed = graphNode.jointSpeed;
		jointStrength = graphNode.jointStrength;
	}
}
