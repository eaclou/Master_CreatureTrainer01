using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrainNetworkVisualizer : MonoBehaviour {

    //public GameObject displayGameObject;
    //public Mesh displayMesh;
    //public float scale = 1f;
    private MeshBuilder meshBuilder;

    public List<List<int>> nodeVertexList;  // outer List keeps track of a group of vertices that make up each node.  inner List is a list of vertex indices
    public List<List<int>> connectionVertexList;

    public List<Vector3> nodePositionsList; // keeps track of each node's 2d position
    public List<BezierCurve> bezierCurveList;
    public List<int> hiddenNodeIndicesList;

    public float neuronRadiusMin = 0.01f;
    public float neuronRadiusMax = 0.15f;
    public float neuronRadiusMaxValue = 0.5f;
    public float connectionWidthMin = 0.005f;
    public float connectionWidthMax = 0.09f;
    public float attractToCritterPos = 0.1f;
    public float attractToCenterPos = 0.01f;
    public float attractByConnections = 0.01f;
    public float attractToControlPoints = 0.01f;
    public float neuronRepelForce = 0.001f;
    public float bezierRepelForce = 0.001f;
    public float neuronRepelMaxDistance = 0.5f;
    public float bezierRepelMaxDistance = 0.5f;

    public Texture2D neuronPositionsTexture;
    public Vector3 centerPos;
    public Critter sourceCritter;

    public BrainNetworkVisualizer() {
        //if(displayGameObject == null) {
        //    displayGameObject = new GameObject("GOBrainNetworkVisualization");
        //    displayGameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        

        //}
    }

    public void SetNeuronSegmentPositions(BrainNEAT brain) {
        
        int currentInputIndex = 0;
        int currentOutputIndex = 0;
        // Check for ANGLE SENSORS:
        for(int i = 0; i < sourceCritter.segaddonJointAngleSensorList.Count; i++) {            
            if (sourceCritter.critterSegmentList[sourceCritter.segaddonJointAngleSensorList[i].segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.DualXY) {
                // Add an extra one if it's a dual Joint
                brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonJointAngleSensorList[i].segmentID].transform.position;
                currentInputIndex++;
            }
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonJointAngleSensorList[i].segmentID].transform.position;
            currentInputIndex++;            
        }
        for(int i = 0; i < sourceCritter.segaddonContactSensorList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonContactSensorList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonRaycastSensorList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonRaycastSensorList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonCompassSensor1DList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonCompassSensor1DList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonCompassSensor3DList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonCompassSensor3DList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonPositionSensor1DList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonPositionSensor1DList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonPositionSensor3DList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonPositionSensor3DList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonRotationSensor1DList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonRotationSensor1DList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonRotationSensor3DList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonRotationSensor3DList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonVelocitySensor1DList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonVelocitySensor1DList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonVelocitySensor3DList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonVelocitySensor3DList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonAltimeterList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonAltimeterList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        // Outputs:
        for (int i = 0; i < sourceCritter.segaddonJointMotorList.Count; i++) {
            if (sourceCritter.critterSegmentList[sourceCritter.segaddonJointMotorList[i].segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.DualXY) {
                // Add an extra one if it's a dual Joint
                brain.outputNeuronList[currentOutputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonJointMotorList[i].segmentID].transform.position - sourceCritter.critterSegmentList[sourceCritter.segaddonJointMotorList[i].segmentID].transform.forward * sourceCritter.critterSegmentList[sourceCritter.segaddonJointMotorList[i].segmentID].transform.localScale.z * 0.5f;

                currentOutputIndex++;
            }
            brain.outputNeuronList[currentOutputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonJointMotorList[i].segmentID].transform.position - sourceCritter.critterSegmentList[sourceCritter.segaddonJointMotorList[i].segmentID].transform.forward * sourceCritter.critterSegmentList[sourceCritter.segaddonJointMotorList[i].segmentID].transform.localScale.z * 0.5f;
            currentOutputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonThrusterEffector1DList.Count; i++) {
            brain.outputNeuronList[currentOutputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonThrusterEffector1DList[i].segmentID].transform.position;
            currentOutputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonThrusterEffector3DList.Count; i++) {
            brain.outputNeuronList[currentOutputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonThrusterEffector3DList[i].segmentID].transform.position;
            currentOutputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonTorqueEffector1DList.Count; i++) {
            brain.outputNeuronList[currentOutputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonTorqueEffector1DList[i].segmentID].transform.position;
            currentOutputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonTorqueEffector3DList.Count; i++) {
            brain.outputNeuronList[currentOutputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonTorqueEffector3DList[i].segmentID].transform.position;
            currentOutputIndex++;
        }

        // More Inputs:
        for (int i = 0; i < sourceCritter.segaddonOscillatorInputList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonOscillatorInputList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonValueInputList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonValueInputList[i].segmentID].transform.position;
            currentInputIndex++;
        }
        for (int i = 0; i < sourceCritter.segaddonTimerInputList.Count; i++) {
            brain.inputNeuronList[currentInputIndex].worldPos = sourceCritter.critterSegmentList[sourceCritter.segaddonTimerInputList[i].segmentID].transform.position;
            currentInputIndex++;
        }        
    }

    public void InitShaderTexture(BrainNEAT brain) {
        if (neuronPositionsTexture == null) {
            neuronPositionsTexture = new Texture2D(brain.neuronList.Count, 1, TextureFormat.RGBAHalf, false);
        }
        else {
            neuronPositionsTexture.Resize(brain.neuronList.Count, 1);
        }
    }

    public Mesh BuildNewMesh(BrainNEAT brain) {
        if(sourceCritter == null) {
            Debug.Log("BuildNewMesh(BrainNEAT brain) SOURCE CRITTER NULL!!!");
        }
        else {
            SetNeuronSegmentPositions(brain);
        }

        if (nodeVertexList == null) {
            nodeVertexList = new List<List<int>>();
        }
        else {
            nodeVertexList.Clear();
        }
        if (connectionVertexList == null) {
            connectionVertexList = new List<List<int>>();
        }
        else {
            connectionVertexList.Clear();
        }
        
        if(nodePositionsList == null) {
            nodePositionsList = new List<Vector3>();
        }
        else {
            nodePositionsList.Clear();
        }
        if(bezierCurveList == null) {
            bezierCurveList = new List<BezierCurve>();
        }
        else {
            bezierCurveList.Clear();
        }
        if(hiddenNodeIndicesList == null) {
            hiddenNodeIndicesList = new List<int>();  // keep track of hidden nodes for later use
        }
        else {
            hiddenNodeIndicesList.Clear();
        }
        
        meshBuilder = new MeshBuilder();
        //Debug.Log("PRE ERROR: brain: " + brain.ToString());
        //neuronRadius = 1.4f / Mathf.Max((float)brain.inputNeuronList.Count, (float)brain.outputNeuronList.Count);
        //Vector3 offset = new Vector3(neuronRadius * 0.5f, neuronRadius * 0.5f, 0f);

        int currentInputIndex = 0;
        int currentOutputIndex = 0; 

        for(int i = 0; i < brain.neuronList.Count; i++) {
            // go through all nodes and place them in proper spot
            float xpos = 0f;
            float ypos = 0f;
            float zpos = 0f;

            //float size = Mathf.Min((neuronRadius * brain.neuronList[i].currentValue[0] * 0.2f + 0.02f), 2.0f);
            float size = Mathf.Max(Mathf.Min((Mathf.Abs(brain.neuronList[i].currentValue[0])) * neuronRadiusMaxValue, neuronRadiusMax), neuronRadiusMin);

            if (brain.neuronList[i].nodeType == GeneNodeNEAT.GeneNodeType.In) {
                xpos = (float)(currentInputIndex + 1) / (float)(brain.inputNeuronList.Count + 1) - 0.5f;
                ypos = 0f;
                zpos = UnityEngine.Random.Range(-0.5f, 0.5f);
                currentInputIndex++;
                BuildNodeSphere(meshBuilder, new Vector3(xpos, ypos, zpos), size, GetColorFromNeuron(brain.neuronList[i]));
            }
            else if(brain.neuronList[i].nodeType == GeneNodeNEAT.GeneNodeType.Hid) {
                xpos = 0.0f; // UnityEngine.Random.Range(neuronRadius, 1f - neuronRadius); //0.5f;
                ypos = 0.0f; // UnityEngine.Random.Range(neuronRadius, 1f - neuronRadius); //0.5f;
                zpos = UnityEngine.Random.Range(-0.5f, 0.5f);
                hiddenNodeIndicesList.Add(i);
                //BuildNode(meshBuilder, new Vector3(xpos, ypos, 0f), new Vector3(radius, 0f, 0f), new Vector3(0f, radius, 0f), GetColorFromValue(brain.neuronList[i].currentValue[0]));
            }
            else {  // output
                xpos = (float)(currentOutputIndex + 1) / (float)(brain.outputNeuronList.Count + 1) - 0.5f;
                ypos = 1f - neuronRadiusMax;
                zpos = UnityEngine.Random.Range(-0.5f, 0.5f);
                currentOutputIndex++;
                BuildNodeSphere(meshBuilder, new Vector3(xpos, ypos, zpos), size, GetColorFromNeuron(brain.neuronList[i]));
            }
            nodePositionsList.Add(new Vector3(xpos, ypos, zpos));
        }

        // position HiddenNodes!!
        
        // BUILD hidden nodes:
        for (int h = 0; h < hiddenNodeIndicesList.Count; h++) {
            float size = Mathf.Max(Mathf.Min((Mathf.Abs(brain.neuronList[hiddenNodeIndicesList[h]].currentValue[0])) * neuronRadiusMaxValue, neuronRadiusMax), neuronRadiusMin);
            BuildNodeSphere(meshBuilder, new Vector3(nodePositionsList[hiddenNodeIndicesList[h]].x, nodePositionsList[hiddenNodeIndicesList[h]].y, nodePositionsList[hiddenNodeIndicesList[h]].z), size, GetColorFromNeuron(brain.neuronList[hiddenNodeIndicesList[h]]));
        }

        // CONNECTIONS!
        //connectionWidth = Mathf.Max(neuronRadius * 0.1f, 0.01f);
        //Debug.Log("BuildNew BrainMesh: numConnections: " + brain.connectionList.Count);
        for(int c = 0; c < brain.connectionList.Count; c++) {
            BezierCurve connectionBezier = new BezierCurve();
            connectionBezier.points[0] = nodePositionsList[brain.connectionList[c].fromNodeID];
            connectionBezier.points[1] = Vector3.Lerp(nodePositionsList[brain.connectionList[c].fromNodeID], nodePositionsList[brain.connectionList[c].toNodeID], 0.333f);
            connectionBezier.points[2] = Vector3.Lerp(nodePositionsList[brain.connectionList[c].fromNodeID], nodePositionsList[brain.connectionList[c].toNodeID], 0.667f);
            connectionBezier.points[3] = nodePositionsList[brain.connectionList[c].toNodeID];
            bezierCurveList.Add(connectionBezier);
            float startWidth = Mathf.Max(Mathf.Min((Mathf.Abs(brain.neuronList[brain.connectionList[c].fromNodeID].currentValue[0]) * connectionWidthMax * 0.4f), connectionWidthMax), connectionWidthMin);
            float endWidth = Mathf.Max(Mathf.Min((Mathf.Abs(brain.neuronList[brain.connectionList[c].toNodeID].currentValue[0])) * connectionWidthMax * 0.4f, connectionWidthMax), connectionWidthMin);
            BuildLinkBezier(meshBuilder, connectionBezier, startWidth, endWidth, GetColorFromNeuron(brain.neuronList[brain.connectionList[c].fromNodeID]), GetColorFromNeuron(brain.neuronList[brain.connectionList[c].toNodeID]));
            //Debug.Log("BuildNew BrainMesh: from: " + nodePositionsList[brain.connectionList[c].fromNodeID].ToString() + ", to: " + nodePositionsList[brain.connectionList[c].toNodeID].ToString());
        }
        
        //MoveHiddenNodes(brain, 1);
        return meshBuilder.CreateMesh();
    }

    public void MoveNeurons(BrainNEAT brain, int numIter) {

        // position HiddenNodes!!
        int numIterations = numIter;
        List<Vector3> newNodePositionsList = new List<Vector3>();
        for(int i = 0; i < nodePositionsList.Count; i++) {  // Initialize new positions list
            Vector3 newPositionVector = new Vector3();
            newPositionVector = nodePositionsList[i];
            newNodePositionsList.Add(newPositionVector);
        }
        
        //float attract = 0.007f;
        //float repel = 0.0025f;
        for (int phase = 0; phase < numIterations; phase++) {
            // Attract NEURONS:
            for (int c = 0; c < brain.connectionList.Count; c++) {
                // go trhough all connections and treat them as springs
                int fromNode = brain.connectionList[c].fromNodeID;
                int toNode = brain.connectionList[c].toNodeID;
                
                Vector3 linkVector = nodePositionsList[toNode] - nodePositionsList[fromNode];
                if (brain.neuronList[fromNode].nodeType == GeneNodeNEAT.GeneNodeType.In) {
                    //linkVector += new Vector2(0f, -attract * 2.7f);
                }
                else if (brain.neuronList[fromNode].nodeType == GeneNodeNEAT.GeneNodeType.Out) {
                    //linkVector += new Vector2(0f, attract * 2.7f);
                }
                float dist = Mathf.Max(linkVector.sqrMagnitude, 0.02f);
                newNodePositionsList[fromNode] += linkVector * attractByConnections * dist;  // move fromNode towards toNode proportionally based on its distance
                newNodePositionsList[toNode] -= linkVector * attractByConnections * dist;  // move fromNode towards toNode proportionally based on its distance  
                
            }
            // REPEL NEURONS:
            for (int j = 0; j < nodePositionsList.Count; j++) {  // for all nodes
                for (int k = 0; k < nodePositionsList.Count; k++) {   // go through all nodes
                    if (j != k) {  // if not comparing to itself
                        Vector3 vectorToNode = (nodePositionsList[k] - nodePositionsList[j]);
                        float dist = Mathf.Max(vectorToNode.magnitude, 0.01f);
                        // repel from each node:
                        if (dist < neuronRepelMaxDistance) {
                            newNodePositionsList[j] -= vectorToNode * neuronRepelForce / dist;
                            newNodePositionsList[k] += vectorToNode * neuronRepelForce / dist;
                        }                        
                    }
                }
                // For each node, if not hidden, move towards its worldPos:
                if (brain.neuronList[j].nodeType != GeneNodeNEAT.GeneNodeType.Hid) {
                    Vector3 vectorToSegmentPos = (brain.neuronList[j].worldPos - nodePositionsList[j]);
                    float dist = Mathf.Max(vectorToSegmentPos.sqrMagnitude, 0.01f);
                    newNodePositionsList[j] += vectorToSegmentPos * attractToCritterPos;
                    //Debug.Log("brain.neuronList[" + j.ToString() + "] worldPos: " + brain.neuronList[j].worldPos.ToString() + ", nodePos: " + nodePositionsList[j].ToString());
                }
                // build texture to pass to shader:
                Vector3 position = nodePositionsList[j];
                neuronPositionsTexture.SetPixel(j, 1, new Color(position.x, position.y, position.z, brain.neuronList[j].currentValue[0]));
            }

            // Apply new Neuron positions to master list:
            for (int p = 0; p < nodePositionsList.Count; p++) {  // Initialize new positions list
                //Vector3 newPositionVector = new Vector3();
                nodePositionsList[p] = newNodePositionsList[p];
            }

            // BEZIER CURVES:
            for (int c = 0; c < brain.connectionList.Count; c++) {
                bezierCurveList[c].points[0] = nodePositionsList[brain.connectionList[c].fromNodeID];
                //bezierCurveList[c].points[1] = Vector3.Lerp(nodePositionsList[brain.connectionList[c].fromNodeID], nodePositionsList[brain.connectionList[c].toNodeID], 0.333f);
                //bezierCurveList[c].points[2] = Vector3.Lerp(nodePositionsList[brain.connectionList[c].fromNodeID], nodePositionsList[brain.connectionList[c].toNodeID], 0.667f);
                bezierCurveList[c].points[3] = nodePositionsList[brain.connectionList[c].toNodeID];
                

                for (int b = 0; b < bezierCurveList.Count; b++) {
                    // Go through all other bezierCurves and repel from others  
                    if (b != c) {  // if not comparing to itself:
                        for (int control = 0; control < 4; control++) {  // go through each of the 4 points in the other bezierCurve connection:
                            Vector3 vectorToControlPoint1 = bezierCurveList[b].points[control] - bezierCurveList[c].points[1];
                            float distance = Mathf.Max(vectorToControlPoint1.sqrMagnitude, 0.02f);
                            // The middle control points in the current Beziercurve are repelled from ALL OTHER bezier curve control points!
                            if (distance < neuronRepelMaxDistance) {
                                bezierCurveList[c].points[1] -= vectorToControlPoint1 * bezierRepelForce * distance;
                                bezierCurveList[c].points[2] -= vectorToControlPoint1 * bezierRepelForce * distance;
                            }
                        }
                    }
                }
                // ATTRACT to rest position!
                Vector3 vectorControl1ToRest = Vector3.Lerp(bezierCurveList[c].points[0], bezierCurveList[c].points[3], 0.333f) - bezierCurveList[c].points[1];
                float dist1 = Mathf.Max(vectorControl1ToRest.sqrMagnitude, 0.02f);
                Vector3 vectorControl2ToRest = Vector3.Lerp(bezierCurveList[c].points[0], bezierCurveList[c].points[3], 0.667f) - bezierCurveList[c].points[2];
                float dist2 = Mathf.Max(vectorControl2ToRest.sqrMagnitude, 0.02f);
                bezierCurveList[c].points[1] += vectorControl1ToRest * attractToControlPoints * dist1;
                bezierCurveList[c].points[2] += vectorControl2ToRest * attractToControlPoints * dist2;
            }   
            

            neuronPositionsTexture.Apply();
        }
        //meshBuilder.UpdateMeshVertices();
    }  

    public void UpdateVertexColors(BrainNEAT brain) {
        //Debug.Log("UpdateVertexColors(BrainNEAT brain) " + GetColorFromValue(brain.neuronList[0].currentValue[0]).ToString());
        for (int i = 0; i < brain.neuronList.Count; i++) {
            ChangeNodeColor(i, GetColorFromNeuron(brain.neuronList[i]));
        }
        for (int j = 0; j < brain.connectionList.Count; j++) {
            //ChangeConnectionColor(j, GetColorFromConnection(brain.connectionList[j], brain));
        }
        meshBuilder.UpdateMeshColors();
    }

    public void ChangeNodeColor(int index, Color newColor) {
        for(int i = 0; i < nodeVertexList[index].Count; i++) {
            meshBuilder.Colors[nodeVertexList[index][i]] = newColor;
        }
    }
    public void ChangeConnectionColor(int index, Color newColor) {
        for (int i = 0; i < connectionVertexList[index].Count; i++) {
            //meshBuilder.Colors[connectionVertexList[index][i]] = newColor;
        }
    }

    private Color GetColorFromNeuron(NeuronNEAT neuron) {
        //black = -1, white = 1
        float val01 = neuron.currentValue[0] * neuronRadiusMaxValue + 0.5f;
        Color newColor = new Color(val01, val01, val01);
        if (neuron.nodeType == GeneNodeNEAT.GeneNodeType.In) {
            newColor = Color.Lerp(newColor, new Color(0f, 1f, 0f), 0.25f);
        }
        if (neuron.nodeType == GeneNodeNEAT.GeneNodeType.Hid) {
            newColor = Color.Lerp(newColor, new Color(1f, 1f, 0f), 0.25f);
        }
        if (neuron.nodeType == GeneNodeNEAT.GeneNodeType.Out) {
            newColor = Color.Lerp(newColor, new Color(1f, 0f, 0f), 0.25f);
        }

        return newColor;
    }
    private Color GetColorFromConnection(ConnectionNEAT connection, BrainNEAT brain) {
        //black = -1, white = 1
        //float val01 = connection.weight[0] * 0.5f + 0.5f;
        float val01 = brain.neuronList[connection.fromNodeID].currentValue[0] * connection.weight[0] * 0.2f + 0.5f;
        Color newColor = new Color(val01, val01, val01);
        newColor = Color.Lerp(newColor, new Color(0f, 0f, 1f), 0f);
        //if (connection.nodeType == GeneNodeNEAT.GeneNodeType.In) {
        //    newColor = Color.Lerp(newColor, new Color(0f, 1f, 0f), 0.75f);
        //}
        //if (connection.nodeType == GeneNodeNEAT.GeneNodeType.Out) {
        //    newColor = Color.Lerp(newColor, new Color(1f, 0f, 0f), 0.75f);
        //}
        //Debug.Log(val01.ToString());
        return newColor;
    }

    public void BuildLinkBezier(MeshBuilder meshBuilder, BezierCurve connectionBezier, float widthStart, float widthEnd, Color colorStart, Color colorEnd) {
        List<int> verticesList = new List<int>();

        float m_splineStartRadius = widthStart;
        float m_splineEndRadius = widthEnd;
        int m_splineHeightSegmentCount = 8;
        int m_splineRadialSegmentCount = 4;

        float tInc = 1f / m_splineHeightSegmentCount; // How many subdivisions along the length of the spline

        for (int i = 0; i <= m_splineHeightSegmentCount; i++) {
            float t = tInc * (float)i;
            Vector3 ringCenter = connectionBezier.GetPoint(t);
            //Quaternion rot = Quaternion.identity;
            Vector3 dir = connectionBezier.GetDirection(t);
            Quaternion rot = Quaternion.identity;
            if (dir != Vector3.zero) {
                rot.SetLookRotation(dir);
            }
            float radius = ((1f - t) * m_splineStartRadius) + (t * m_splineEndRadius);
            // Construct the mesh Ring!
            //BuildBezierCurveRing(meshBuilder, m_splineRadialSegmentCount, ringCenter, radius, t, i > 0, rot); 
            //protected void BuildBezierCurveRing(MeshBuilder meshBuilder, int segmentCount, Vector3 center, float radius, float v, bool buildTriangles, Quaternion rotation) {
            float angleInc = (Mathf.PI * 2.0f) / m_splineRadialSegmentCount;

            for (int j = 0; j <= m_splineRadialSegmentCount; j++) {
                float angle = angleInc * j;

                Vector3 unitPosition = Vector3.zero;
                unitPosition.x = Mathf.Cos(angle);
                unitPosition.y = Mathf.Sin(angle);

                unitPosition = rot * unitPosition;
                Vector3 normal = unitPosition;

                meshBuilder.Vertices.Add(ringCenter + unitPosition * radius);
                meshBuilder.Normals.Add(normal);
                //meshBuilder.UVs.Add(new Vector2((float)i / segmentCount, v));
                meshBuilder.UVs.Add(new Vector2((float)j / m_splineRadialSegmentCount, t));
                verticesList.Add(meshBuilder.Vertices.Count - 1);
                meshBuilder.Colors.Add(Color.Lerp(colorStart, colorEnd, t));

                if (j > 0 && i > 0) {
                    //Debug.Log ("buildTriangles!");
                    int baseIndex = meshBuilder.Vertices.Count - 1;

                    int vertsPerRow = m_splineRadialSegmentCount + 1;

                    int index0 = baseIndex;
                    int index1 = baseIndex - 1;
                    int index2 = baseIndex - vertsPerRow;
                    int index3 = baseIndex - vertsPerRow - 1;

                    meshBuilder.AddTriangle(index1, index2, index0);
                    meshBuilder.AddTriangle(index1, index3, index2);
                }
            }            
        }
        connectionVertexList.Add(verticesList);  // keep track of Connection's vertices so their color can be changed without rebuilding every frame
    }
    public void BuildLink(MeshBuilder meshBuilder, Vector3 fromPos, Vector3 toPos, float width, Color color) {
        List<int> verticesList = new List<int>();

        Vector3 normal = new Vector3(0f, 0f, 1f); // Vector3.Cross(lengthDir, widthDir).normalized;
        Vector3 linkVector = new Vector3(toPos.x - fromPos.x, toPos.y - fromPos.y, toPos.z - fromPos.z);
        Vector3 widthVector = new Vector3(new Vector2(linkVector.y, -linkVector.x).normalized.x, new Vector2(linkVector.y, -linkVector.x).normalized.y, 0f) * width;        
        Vector3 cornerPos = new Vector3(fromPos.x - widthVector.x * 0.5f, fromPos.y - widthVector.y * 0.5f, -0.01f);
        //Vector3 offset = new Vector3(cornerPos.x, cornerPos.y, 0f);

        meshBuilder.Vertices.Add(cornerPos);
        meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
        meshBuilder.Normals.Add(normal);
        verticesList.Add(meshBuilder.Vertices.Count - 1);
        meshBuilder.Colors.Add(color);

        meshBuilder.Vertices.Add(cornerPos + linkVector);
        meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(normal);
        verticesList.Add(meshBuilder.Vertices.Count - 1);
        meshBuilder.Colors.Add(color);

        meshBuilder.Vertices.Add(cornerPos + linkVector + widthVector);
        meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(normal);
        verticesList.Add(meshBuilder.Vertices.Count - 1);
        meshBuilder.Colors.Add(color);

        meshBuilder.Vertices.Add(cornerPos + widthVector);
        meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
        meshBuilder.Normals.Add(normal);
        verticesList.Add(meshBuilder.Vertices.Count - 1);
        meshBuilder.Colors.Add(color);

        connectionVertexList.Add(verticesList);  // keep track of Connection's vertices so their color can be changed without rebuilding every frame

        //we don't know how many verts the meshBuilder is up to, but we only care about the four we just added:
        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
        
    }

    public void UpdateNodeVertexPositions(BrainNEAT brain) {
        for(int i = 0; i < nodeVertexList.Count; i++) {
            // for all neurons:
            //Debug.Log("index0: " + nodePositionsList[i].x.ToString() + ", " + nodePositionsList[i].y.ToString() + " index1: " + nodePositionsList[i].x.ToString() + ", " + (nodePositionsList[i].y + neuronRadius).ToString() + " index2: " + (nodePositionsList[i].x + neuronRadius).ToString() + ", " + (nodePositionsList[i].y + neuronRadius).ToString() + " index3: " + (nodePositionsList[i].x + neuronRadius).ToString() + ", " + nodePositionsList[i].y.ToString());
            //float size = neuronRadius + Mathf.Abs(brain.neuronList[i].previousValue) * neuronRadius * 0.12f;
            float size = Mathf.Max(Mathf.Min((Mathf.Abs(brain.neuronList[i].currentValue[0])) * neuronRadiusMaxValue, neuronRadiusMax), neuronRadiusMin);
            meshBuilder.Vertices[nodeVertexList[i][0]] = nodePositionsList[i];
            meshBuilder.Vertices[nodeVertexList[i][1]] = nodePositionsList[i] + new Vector3(0f, size, 0f);
            meshBuilder.Vertices[nodeVertexList[i][2]] = nodePositionsList[i] + new Vector3(size, 0f, 0f) + new Vector3(0f, size, 0f);
            meshBuilder.Vertices[nodeVertexList[i][3]] = nodePositionsList[i] + new Vector3(size, 0f, 0f);
        }
        meshBuilder.UpdateMeshVertices();
    }
    public void UpdateNodeVertexPositionsSphere(BrainNEAT brain) {
        for (int n = 0; n < nodeVertexList.Count; n++) {
            // for all neurons:
            //float size = Mathf.Min((neuronRadius * brain.neuronList[n].currentValue[0] * 0.2f + 0.02f), 2.0f);
            float size = Mathf.Max(Mathf.Min((Mathf.Abs(brain.neuronList[n].currentValue[0])) * neuronRadiusMaxValue, neuronRadiusMax), neuronRadiusMin);
            int currentVertexIndex = 0;
            if(nodePositionsList == null) {
                //Debug.Log("if(nodePositionsList == null) {");
            }
            else {
                //Debug.Log("if(nodePositionsList != null) {" + nodePositionsList.Count.ToString() + ", n: " + n.ToString() + ", fromNode: " + brain.connectionList[n].fromNodeID.ToString());
            }
            Vector3 offset = nodePositionsList[n] - new Vector3(0f, size, 0f);
            int m_HeightSegmentCount = 4;
            int m_RadialSegmentCount = 4;
            float m_VerticalScale = 1f;
            Quaternion rotation = Quaternion.identity;
            //the angle increment per height segment:
            float angleInc = Mathf.PI / m_HeightSegmentCount;
            //the vertical (scaled) radius of the sphere:
            float verticalRadius = size * m_VerticalScale;
            //build the rings:
            for (int i = 0; i <= m_HeightSegmentCount; i++) {
                Vector3 centrePos = Vector3.zero;
                //calculate a height offset and radius based on a vertical circle calculation:
                centrePos.y = -Mathf.Cos(angleInc * i);
                float radius = Mathf.Sin(angleInc * i);
                //calculate the slope of the shpere at this ring based on the height and radius:
                Vector2 slope = new Vector3(-centrePos.y / m_VerticalScale, radius);
                slope.Normalize();
                //multiply the unit height by the vertical radius, and then add the radius to the height to make this sphere originate from its base rather than its centre:
                centrePos.y = centrePos.y * verticalRadius + verticalRadius;
                //scale the radius by the one stored in the partData:
                radius *= size;
                //calculate the final position of the ring centre:
                Vector3 finalRingCentre = rotation * centrePos + offset;
                //V coordinate:
                float v = (float)i / m_HeightSegmentCount;
                //build the ring:
                //BuildRing(meshBuilder, m_RadialSegmentCount, finalRingCentre, radius, v, i > 0, rotation, slope);
                float angleIncrement = (Mathf.PI * 2.0f) / m_RadialSegmentCount;

                for (int j = 0; j <= m_RadialSegmentCount; j++) {
                    float angle = angleIncrement * j;
                    Vector3 unitPosition = Vector3.zero;
                    unitPosition.x = Mathf.Cos(angle);
                    unitPosition.z = Mathf.Sin(angle);
                    float normalVertical = -slope.x;
                    float normalHorizontal = slope.y;
                    Vector3 normal = unitPosition * normalHorizontal;
                    normal.y = normalVertical;
                    normal = rotation * normal;
                    unitPosition = rotation * unitPosition;

                    meshBuilder.Vertices[nodeVertexList[n][currentVertexIndex]] = finalRingCentre + unitPosition * radius;
                    currentVertexIndex++;
                }
            }

        }
        meshBuilder.UpdateMeshVertices();
    }
    public void UpdateConnectionVertexPositions(BrainNEAT brain) {
        //for (int c = 0; c < brain.connectionList.Count; c++) {

            //BuildLink(meshBuilder, nodePositionsList[brain.connectionList[c].fromNodeID] + offset, nodePositionsList[brain.connectionList[c].toNodeID] + offset, connectionWidth, GetColorFromValue(brain.connectionList[c].weight[0]));
            //Debug.Log("BuildNew BrainMesh: from: " + nodePositionsList[brain.connectionList[c].fromNodeID].ToString() + ", to: " + nodePositionsList[brain.connectionList[c].toNodeID].ToString());
        //}
        for (int i = 0; i < connectionVertexList.Count; i++) {
            // for all connections:
            
            Vector3 linkVector = new Vector3(nodePositionsList[brain.connectionList[i].toNodeID].x - nodePositionsList[brain.connectionList[i].fromNodeID].x, nodePositionsList[brain.connectionList[i].toNodeID].y - nodePositionsList[brain.connectionList[i].fromNodeID].y, nodePositionsList[brain.connectionList[i].toNodeID].z - nodePositionsList[brain.connectionList[i].fromNodeID].z);
            Vector3 widthVector = new Vector3(new Vector2(linkVector.y, -linkVector.x).normalized.x, new Vector2(linkVector.y, -linkVector.x).normalized.y, 0f) * (connectionWidthMin + Mathf.Abs(brain.connectionList[i].weight[0]) * connectionWidthMin * 0.62f);
            Vector3 cornerPos = new Vector3(nodePositionsList[brain.connectionList[i].fromNodeID].x - widthVector.x * 0.5f + neuronRadiusMin * 0.5f, nodePositionsList[brain.connectionList[i].fromNodeID].y - widthVector.y * 0.5f + neuronRadiusMin * 0.5f, nodePositionsList[brain.connectionList[i].fromNodeID].z);
            //Debug.Log("index0: " + nodePositionsList[i].x.ToString() + ", " + nodePositionsList[i].y.ToString() + " index1: " + nodePositionsList[i].x.ToString() + ", " + (nodePositionsList[i].y + neuronRadius).ToString() + " index2: " + (nodePositionsList[i].x + neuronRadius).ToString() + ", " + (nodePositionsList[i].y + neuronRadius).ToString() + " index3: " + (nodePositionsList[i].x + neuronRadius).ToString() + ", " + nodePositionsList[i].y.ToString());
            meshBuilder.Vertices[connectionVertexList[i][0]] = cornerPos;
            meshBuilder.Vertices[connectionVertexList[i][1]] = cornerPos + linkVector;
            meshBuilder.Vertices[connectionVertexList[i][2]] = cornerPos + linkVector + widthVector;
            meshBuilder.Vertices[connectionVertexList[i][3]] = cornerPos + widthVector;
        }
        meshBuilder.UpdateMeshVertices();
    }
    public void UpdateConnectionVertexPositionsBezier(BrainNEAT brain) {

        for (int c = 0; c < connectionVertexList.Count; c++) {
            // for all connections:

            /*Vector3 linkVector = new Vector3(nodePositionsList[brain.connectionList[i].toNodeID].x - nodePositionsList[brain.connectionList[i].fromNodeID].x, nodePositionsList[brain.connectionList[i].toNodeID].y - nodePositionsList[brain.connectionList[i].fromNodeID].y, nodePositionsList[brain.connectionList[i].toNodeID].z - nodePositionsList[brain.connectionList[i].fromNodeID].z);
            Vector3 widthVector = new Vector3(new Vector2(linkVector.y, -linkVector.x).normalized.x, new Vector2(linkVector.y, -linkVector.x).normalized.y, 0f) * (connectionWidth + Mathf.Abs(brain.connectionList[i].weight[0]) * connectionWidth * 0.62f);
            Vector3 cornerPos = new Vector3(nodePositionsList[brain.connectionList[i].fromNodeID].x - widthVector.x * 0.5f + neuronRadius * 0.5f, nodePositionsList[brain.connectionList[i].fromNodeID].y - widthVector.y * 0.5f + neuronRadius * 0.5f, nodePositionsList[brain.connectionList[i].fromNodeID].z);
            //Debug.Log("index0: " + nodePositionsList[i].x.ToString() + ", " + nodePositionsList[i].y.ToString() + " index1: " + nodePositionsList[i].x.ToString() + ", " + (nodePositionsList[i].y + neuronRadius).ToString() + " index2: " + (nodePositionsList[i].x + neuronRadius).ToString() + ", " + (nodePositionsList[i].y + neuronRadius).ToString() + " index3: " + (nodePositionsList[i].x + neuronRadius).ToString() + ", " + nodePositionsList[i].y.ToString());
            meshBuilder.Vertices[connectionVertexList[i][0]] = cornerPos;
            meshBuilder.Vertices[connectionVertexList[i][1]] = cornerPos + linkVector;
            meshBuilder.Vertices[connectionVertexList[i][2]] = cornerPos + linkVector + widthVector;
            meshBuilder.Vertices[connectionVertexList[i][3]] = cornerPos + widthVector;
            */
            int currentVertexIndex = 0;
            //float width = connectionWidth; // brain.neuronList[brain.connectionList[c].fromNodeID].currentValue[0];
            Color colorStart = GetColorFromNeuron(brain.neuronList[brain.connectionList[c].fromNodeID]);
            Color colorEnd = GetColorFromNeuron(brain.neuronList[brain.connectionList[c].toNodeID]);
            float startWidth = Mathf.Max(Mathf.Min((Mathf.Abs(brain.neuronList[brain.connectionList[c].fromNodeID].currentValue[0])) * neuronRadiusMaxValue, connectionWidthMax), connectionWidthMin);
            float endWidth = Mathf.Max(Mathf.Min((Mathf.Abs(brain.neuronList[brain.connectionList[c].toNodeID].currentValue[0])) * neuronRadiusMaxValue, connectionWidthMax), connectionWidthMin);
            float m_splineStartRadius = startWidth;
            float m_splineEndRadius = endWidth;
            int m_splineHeightSegmentCount = 8;
            int m_splineRadialSegmentCount = 4;

            float tInc = 1f / m_splineHeightSegmentCount; // How many subdivisions along the length of the spline

            for (int i = 0; i <= m_splineHeightSegmentCount; i++) {
                float t = tInc * (float)i;
                Vector3 ringCenter = bezierCurveList[c].GetPoint(t);
                
                Vector3 dir = bezierCurveList[c].GetDirection(t);
                Quaternion rot = Quaternion.identity;
                if(dir != Vector3.zero) {
                    rot.SetLookRotation(dir);
                }                
                float radius = ((1f - t) * m_splineStartRadius) + (t * m_splineEndRadius);
                // Construct the mesh Ring!
                //BuildBezierCurveRing(meshBuilder, m_splineRadialSegmentCount, ringCenter, radius, t, i > 0, rot); 
                //protected void BuildBezierCurveRing(MeshBuilder meshBuilder, int segmentCount, Vector3 center, float radius, float v, bool buildTriangles, Quaternion rotation) {
                float angleInc = (Mathf.PI * 2.0f) / m_splineRadialSegmentCount;

                for (int j = 0; j <= m_splineRadialSegmentCount; j++) {
                    float angle = angleInc * j;

                    Vector3 unitPosition = Vector3.zero;
                    unitPosition.x = Mathf.Cos(angle);
                    unitPosition.y = Mathf.Sin(angle);

                    unitPosition = rot * unitPosition;
                    Vector3 normal = unitPosition;

                    //meshBuilder.Vertices[] = ringCenter + unitPosition * radius;
                    meshBuilder.Vertices[connectionVertexList[c][currentVertexIndex]] = ringCenter + unitPosition * radius;
                    meshBuilder.Colors[connectionVertexList[c][currentVertexIndex]] = Color.Lerp(colorStart, colorEnd, t);
                    currentVertexIndex++;
                    //meshBuilder.Vertices.Add(ringCenter + unitPosition * radius);
                    //meshBuilder.Normals.Add(normal);
                    //meshBuilder.UVs.Add(new Vector2((float)i / segmentCount, v));
                    //meshBuilder.UVs.Add(new Vector2((float)j / m_splineRadialSegmentCount, t));
                    //verticesList.Add(meshBuilder.Vertices.Count - 1);
                    //meshBuilder.Colors.Add(color);

                    /*if (j > 0 && i > 0) {
                        //Debug.Log ("buildTriangles!");
                        int baseIndex = meshBuilder.Vertices.Count - 1;

                        int vertsPerRow = m_splineRadialSegmentCount + 1;

                        int index0 = baseIndex;
                        int index1 = baseIndex - 1;
                        int index2 = baseIndex - vertsPerRow;
                        int index3 = baseIndex - vertsPerRow - 1;

                        meshBuilder.AddTriangle(index1, index2, index0);
                        meshBuilder.AddTriangle(index1, index3, index2);
                    }*/
                }
            }
        }

        meshBuilder.UpdateMeshVertices();
    }

    public void BuildNodeSphere(MeshBuilder meshBuilder, Vector3 offset, float m_Radius, Color color) {
        List<int> verticesList = new List<int>();

        int m_HeightSegmentCount = 4;
        int m_RadialSegmentCount = 4;
        //float m_Radius = 1f;
        float m_VerticalScale = 1f;
        Quaternion rotation = Quaternion.identity;
        
        //the angle increment per height segment:
        float angleInc = Mathf.PI / m_HeightSegmentCount;

        //the vertical (scaled) radius of the sphere:
        float verticalRadius = m_Radius * m_VerticalScale;

        //build the rings:
        for (int i = 0; i <= m_HeightSegmentCount; i++) {
            Vector3 centrePos = Vector3.zero;

            //calculate a height offset and radius based on a vertical circle calculation:
            centrePos.y = -Mathf.Cos(angleInc * i);
            float radius = Mathf.Sin(angleInc * i);

            //calculate the slope of the shpere at this ring based on the height and radius:
            Vector2 slope = new Vector3(-centrePos.y / m_VerticalScale, radius);
            slope.Normalize();

            //multiply the unit height by the vertical radius, and then add the radius to the height to make this sphere originate from its base rather than its centre:
            centrePos.y = centrePos.y * verticalRadius + verticalRadius;

            //scale the radius by the one stored in the partData:
            radius *= m_Radius;

            //calculate the final position of the ring centre:
            Vector3 finalRingCentre = rotation * centrePos + offset;

            //V coordinate:
            float v = (float)i / m_HeightSegmentCount;

            //build the ring:
            //BuildRing(meshBuilder, m_RadialSegmentCount, finalRingCentre, radius, v, i > 0, rotation, slope);
            float angleIncrement = (Mathf.PI * 2.0f) / m_RadialSegmentCount;

            for (int j = 0; j <= m_RadialSegmentCount; j++) {
                float angle = angleIncrement * j;

                Vector3 unitPosition = Vector3.zero;
                unitPosition.x = Mathf.Cos(angle);
                unitPosition.z = Mathf.Sin(angle);

                float normalVertical = -slope.x;
                float normalHorizontal = slope.y;

                Vector3 normal = unitPosition * normalHorizontal;
                normal.y = normalVertical;

                normal = rotation * normal;

                unitPosition = rotation * unitPosition;

                meshBuilder.Vertices.Add(finalRingCentre + unitPosition * radius);
                meshBuilder.Normals.Add(normal);
                meshBuilder.UVs.Add(new Vector2((float)j / m_RadialSegmentCount, v));                
                verticesList.Add(meshBuilder.Vertices.Count - 1);
                meshBuilder.Colors.Add(color);

                if (j > 0 && i > 0) {
                    int baseIndex = meshBuilder.Vertices.Count - 1;

                    int vertsPerRow = m_RadialSegmentCount + 1;

                    int index0 = baseIndex;
                    int index1 = baseIndex - 1;
                    int index2 = baseIndex - vertsPerRow;
                    int index3 = baseIndex - vertsPerRow - 1;

                    meshBuilder.AddTriangle(index0, index2, index1);
                    meshBuilder.AddTriangle(index2, index3, index1);
                }
            }
        }
        nodeVertexList.Add(verticesList);  // keep track of Node's vertices so their color can be changed without rebuilding every frame
    }
    public void BuildNode(MeshBuilder meshBuilder, Vector3 offset, Vector3 widthDir, Vector3 lengthDir, Color color) {
        List<int> verticesList = new List<int>();
        Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;
        //Color red = new Color(1f, 0f, 0f, 1f);

        meshBuilder.Vertices.Add(offset);
        meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
        meshBuilder.Normals.Add(normal);
        verticesList.Add(meshBuilder.Vertices.Count - 1);
        meshBuilder.Colors.Add(color);

        meshBuilder.Vertices.Add(offset + lengthDir);
        meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(normal);
        verticesList.Add(meshBuilder.Vertices.Count - 1);
        meshBuilder.Colors.Add(color);

        meshBuilder.Vertices.Add(offset + lengthDir + widthDir);
        meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(normal);
        verticesList.Add(meshBuilder.Vertices.Count - 1);
        meshBuilder.Colors.Add(color);

        meshBuilder.Vertices.Add(offset + widthDir);
        meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
        meshBuilder.Normals.Add(normal);
        verticesList.Add(meshBuilder.Vertices.Count - 1);
        meshBuilder.Colors.Add(color);

        nodeVertexList.Add(verticesList);  // keep track of Node's vertices so their color can be changed without rebuilding every frame

        //we don't know how many verts the meshBuilder is up to, but we only care about the four we just added:
        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }

}
