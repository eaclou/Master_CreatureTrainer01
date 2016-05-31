using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrainNEAT {

    // BASE properties:
    //public string Name = "Default";
    //public List<FitnessComponent> brainFitnessComponentList;
    // Change these to List<float[]> ??
    //public float[][] layerOutput;  // first array specifies the layer, second specifies the nodes within that layer
    //public float[][] layerInput;  // first array specifies the layer, second specifies the nodes within that layer 

    /* // LayerAll2All BRAIN:
    public int numHiddenLayers = 0;
    public int[] numNodesEachLayer;
    // Layer Count doesn't include Input Layer, since it's simply a buffer that is not manipulated
    private int layerCount;  // number of 'real' layers (excluding input layer)
    private int inputSize;  // the number of input layer nodes
    private int[] layerSize;  // the number of nodes within each of the 'real' layers
    //public TransferFunction[] transferFunction;  // determines the transfer function to be used on each layer/node
    // MOVED TO BASE CLASS:
    //public float[][] layerOutput;  // first array specifies the layer, second specifies the nodes within that layer
    //public float[][] layerInput;  // first array specifies the layer, second specifies the nodes within that layer 
    private float[][] bias;  // the bias term to be added to the weights before being evaluated by the transfer function
    private TransferFunctions.TransferFunction[][] transferFunctions;
    private float[][] delta;  // the delta between the output value for that node and the target value
    private float[][] previousBiasDelta; // the delta of the bias term for the previous epoch - will be used when calculating Momentum for the back-propogation algorithm
    private float[][][] weight;   // first array is the layer, second is the source node, third is the target node
    private float[][][] previousWeightDelta;  // weights of the preceding epoch, to be used for calculating momentum
    // Fitness Components:
    public float[] fitConnectionCost = new float[1];
    */

    //private List<NeuronNEAT> neuronList;
    private GenomeNEAT sourceGenome;
    
    public List<NeuronNEAT> neuronList;
    public List<ConnectionNEAT> connectionList;

    public List<NeuronNEAT> inputNeuronList; 
    public List<NeuronNEAT> outputNeuronList;

    List<NeuronNEAT> visitedNodes;  // make this a hash instead for faster lookups?
    List<NeuronNEAT> completedNodes;

    int numNodesVisited = 0;

    public BrainNEAT() {
        visitedNodes = new List<NeuronNEAT>();
        completedNodes = new List<NeuronNEAT>();
    }
    public BrainNEAT(GenomeNEAT newGenome) {
        sourceGenome = newGenome;
        visitedNodes = new List<NeuronNEAT>();
        completedNodes = new List<NeuronNEAT>();
    }

    public void BrainMasterFunction(ref List<float[]> inputList, ref List<float[]> outputList) {
        string inputString = "inputList: ";
        string outputString = "outputList: ";
        for(int i = 0; i < inputList.Count; i++) {
            inputString += "[" + i.ToString() + "]: " + inputList[i][0].ToString() + ", ";
        }

        //============================================================================================
        // How to run the network??
        // 1) start with input nodes, calculate values one step down the tree
        // 2) start with output nodes, do a depth-first search to find all required calculations -- might run into trouble with recursion?
        // 3) start with inputs, keep going forward until all outputs are taken care of


        // 2)
        /*numNodesVisited = 0;
        visitedNodes.Clear();
        completedNodes.Clear();
        // First, zero out values for all current nodes and save previous states:
        for (int n = 0; n < neuronList.Count; n++) {
            neuronList[n].previousValue = neuronList[n].currentValue[0]; // save current status (which is the state of the brain in the previous timeStep
            neuronList[n].currentValue[0] = 0f;  // clear currentValue            
        }        
        // Fill in input node values:
        for(int m = 0; m < inputNeuronList.Count; m++) {
            inputNeuronList[m].currentValue[0] = inputList[m][0];  // revisit! might be better to do a reference pointer only once when agent is built rather than every time step value-write
            //processedNodes.Add(inputNeuronList[m]);  // if it's an inputNode, it's ready to be used, so mark as processed
        }
        for(int endNode = 0; endNode < outputNeuronList.Count; endNode++) {
            // loop through all output nodes, if all their inputs are processed and ready to go, add weights and run transfer function, else, dive deeper into unprocessed nodes
            //Debug.Log("CalculateNodeInputs endNode(" + endNode.ToString() + ")");
            
            CalculateNodeInputs(outputNeuronList[endNode]);
        }
        for (int o = 0; o < outputList.Count; o++) {
            outputList[o][0] = outputNeuronList[o].currentValue[0];
            outputString += "[" + o.ToString() + "]: " + outputList[o][0].ToString() + ", ";
        }*/
        //============================================================================================

        //@@@@@@@@@@ One-step should support recursion
        // go through every node and calculate its value from each of its inputs' PREVIOUS values

        // populate list of nodes to process
        // First, zero out values for all current nodes and save previous states:
        for (int n = 0; n < neuronList.Count; n++) {
            neuronList[n].previousValue = neuronList[n].currentValue[0]; // save current status (which is the state of the brain in the previous timeStep
            neuronList[n].currentValue[0] = 0f;  // clear currentValue            
        }
        // Fill in input node values:
        for (int m = 0; m < inputNeuronList.Count; m++) {
            inputNeuronList[m].currentValue[0] = inputList[m][0];  // revisit! might be better to do a reference pointer only once when agent is built rather than every time step value-write
            //processedNodes.Add(inputNeuronList[m]);  // if it's an inputNode, it's ready to be used, so mark as processed
        }
        for (int node = 0; node < neuronList.Count; node++) {
            // loop through all nodes, if all their inputs are processed and ready to go, add weights and run transfer function, else, dive deeper into unprocessed nodes
            //Debug.Log("CalculateNodeInputs endNode(" + endNode.ToString() + ")");

            for(int inputs = 0; inputs < neuronList[node].incomingConnectionsList.Count; inputs++) {
                neuronList[node].currentValue[0] += neuronList[neuronList[node].incomingConnectionsList[inputs].fromNodeID].previousValue * neuronList[node].incomingConnectionsList[inputs].weight[0];
            }

            neuronList[node].currentValue[0] = TransferFunctions.Evaluate(neuronList[node].activationFunction, neuronList[node].currentValue[0]);
            // OLD BELOW:
            /*if (neuronList[node].nodeType == GeneNodeNEAT.GeneNodeType.Out) {
                neuronList[node].currentValue[0] = TransferFunctions.Evaluate(TransferFunctions.TransferFunction.RationalSigmoid, neuronList[node].currentValue[0]);
            }
            else if (neuronList[node].nodeType == GeneNodeNEAT.GeneNodeType.Hid) {
                neuronList[node].currentValue[0] = TransferFunctions.Evaluate(TransferFunctions.TransferFunction.RationalSigmoid, neuronList[node].currentValue[0]);
            }*/

            
        }

        //============================================================================================

        for (int o = 0; o < outputList.Count; o++) {
            outputList[o][0] = outputNeuronList[o].currentValue[0];
            outputString += "[" + o.ToString() + "]: " + outputList[o][0].ToString() + ", ";
        }
        


        //Debug.Log("BrainNEAT: BrainMasterFunction!" + inputString);
        //Debug.Log("BrainNEAT: BrainMasterFunction!" + outputString);
    }

    private void CalculateNodeInputs(NeuronNEAT currentNeuron) {  // recursive function
        numNodesVisited++;
        if (numNodesVisited >= 100) {
            Debug.Log("CalculateNodeInputs() HIT MAX NODE COUNT!");
            return;
        }

        visitedNodes.Add(currentNeuron);  // to avoid infinite recursion
        
        for (int i = 0; i < currentNeuron.incomingConnectionsList.Count; i++) {
            //Debug.Log("CalculateNodeInputs! [" + currentNeuron.id.ToString() + "] incoming: " + currentNeuron.incomingConnectionsList[i].fromNodeID.ToString() + " -> " + currentNeuron.incomingConnectionsList[i].toNodeID.ToString());
            //if (outputNeuronList.Contains(currentNeuron)) {
                //Debug.Log("CalculateNodeInputs! visitedNodes.Clear();");                
            //}
            currentNeuron.currentValue[0] += neuronList[currentNeuron.incomingConnectionsList[i].fromNodeID].previousValue * currentNeuron.incomingConnectionsList[i].weight[0];
            if (neuronList[currentNeuron.incomingConnectionsList[i].fromNodeID].nodeType != GeneNodeNEAT.GeneNodeType.In) { // if incoming connection isn't an Input Neuron   
                if(visitedNodes.Contains(neuronList[currentNeuron.incomingConnectionsList[i].fromNodeID])) {
                    if(completedNodes.Contains(neuronList[currentNeuron.incomingConnectionsList[i].fromNodeID])) {
                        // done
                    }
                    else {
                        Debug.Log("Incoming connection has already been visited!  RECURSION?? nodeID: " + currentNeuron.id.ToString() + ", fromNode: " + neuronList[currentNeuron.incomingConnectionsList[i].fromNodeID].id.ToString());
                    }                    
                } 
                else {                    
                    CalculateNodeInputs(neuronList[currentNeuron.incomingConnectionsList[i].fromNodeID]);
                }           
                
            }
            else {
                // if it IS an input neuron:
                visitedNodes.Clear();
            }
            
        }
        if(currentNeuron.nodeType == GeneNodeNEAT.GeneNodeType.Out) {
            currentNeuron.currentValue[0] = TransferFunctions.Evaluate(TransferFunctions.TransferFunction.RationalSigmoid, currentNeuron.currentValue[0]);
        }
        else {
            currentNeuron.currentValue[0] = TransferFunctions.Evaluate(TransferFunctions.TransferFunction.Linear, currentNeuron.currentValue[0]);
        }
        completedNodes.Add(currentNeuron);

        //processedNodes.Add(currentNeuron);
        //return allInputsProcessed;
    }

    public void CopyBrainSettingsFrom(BrainBase sourceBrain) {

    }

    public GenomeNEAT InitializeRandomBrain(int numInputNodes, int numOutputNodes) {
        
        sourceGenome = new GenomeNEAT(numInputNodes, numOutputNodes); // create blank genome with no connections
        sourceGenome.CreateMinimumRandomConnections(); // Add links to each output node froma  random input node

        return sourceGenome;
    }

    public GenomeNEAT InitializeBlankBrain(int numInputNodes, int numOutputNodes) {
        
        sourceGenome = new GenomeNEAT(numInputNodes, numOutputNodes); // create blank genome with no connections
        int numNewLinks = 0;
        int numNewNodes = 0;
        for(int i = 0; i < numNewLinks; i++) {
            sourceGenome.AddNewRandomLink();
        }
        for (int j = 0; j < numNewNodes; j++) {
            sourceGenome.AddNewRandomNode();
        }

        //Debug.Log("InitializeBlankBrain #nodes: " + sourceGenome.nodeNEATList.Count.ToString() + ", #links: " + sourceGenome.linkNEATList.Count.ToString());

        return sourceGenome;
    }

    public void BuildBrainNetwork() {
        if(neuronList == null) {
            neuronList = new List<NeuronNEAT>();
        }
        else {
            neuronList.Clear();
        }
        if(connectionList == null) {
            connectionList = new List<ConnectionNEAT>();
        }
        else {
            connectionList.Clear();
        }
        if (inputNeuronList == null) {
            inputNeuronList = new List<NeuronNEAT>();
        }
        else {
            inputNeuronList.Clear();
        }
        if (outputNeuronList == null) {
            outputNeuronList = new List<NeuronNEAT>();
        }
        else {
            outputNeuronList.Clear();
        }
        int numActiveNodes = 0;
        int numActiveLinks = 0;
        // Create nodes:
        string nodesString = "BuildBrainNetwork() nodes: ";
        for (int i = 0; i < sourceGenome.nodeNEATList.Count; i++) {
            NeuronNEAT newNeuron = new NeuronNEAT(sourceGenome.nodeNEATList[i].id, sourceGenome.nodeNEATList[i].nodeType, sourceGenome.nodeNEATList[i].activationFunction);
            neuronList.Add(newNeuron);
            if(newNeuron.nodeType == GeneNodeNEAT.GeneNodeType.In) {
                inputNeuronList.Add(newNeuron);  // save reference to node 
            }
            if (newNeuron.nodeType == GeneNodeNEAT.GeneNodeType.Out) {
                outputNeuronList.Add(newNeuron);
            }
            nodesString += "[" + i.ToString() + "]: " + newNeuron.nodeType.ToString() + ", ";
            numActiveNodes++;
        }
        // Create connections:
        string connectionsString = "BuildBrainNetwork() connections: ";
        for (int o = 0; o < sourceGenome.linkNEATList.Count; o++) {
            if(sourceGenome.linkNEATList[o].enabled) {
                numActiveLinks++;
                ConnectionNEAT newConnection = new ConnectionNEAT(sourceGenome.linkNEATList[o].fromNodeID, sourceGenome.linkNEATList[o].toNodeID, sourceGenome.linkNEATList[o].weight);
                connectionList.Add(newConnection);
                neuronList[newConnection.toNodeID].incomingConnectionsList.Add(newConnection); // add this connection to its destination neuron's list
                                                                                               //Debug.Log("linkNEATList[" + o.ToString() + "], from: " + newConnection.fromNodeID.ToString() + ", to: " + newConnection.toNodeID.ToString() + ", weight: " + newConnection.weight[0]);
                connectionsString += "[" + o.ToString() + "] " + newConnection.fromNodeID.ToString() + "->" + newConnection.toNodeID.ToString() + ", w: " + newConnection.weight[0].ToString() + ", ";
            }            
        }
        //Debug.Log("BuildBrainNetwork #nodes: " + numActiveNodes.ToString() + ", #links: " + numActiveLinks.ToString());
        //Debug.Log(connectionsString);
    }
    
    public void InitializeBrainFromGenome(GenomeNEAT newGenome) {
        sourceGenome = newGenome;
    }
}
