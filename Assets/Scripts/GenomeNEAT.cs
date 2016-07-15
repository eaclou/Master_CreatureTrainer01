using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenomeNEAT {

    public List<GeneNodeNEAT> nodeNEATList;
    public List<GeneLinkNEAT> linkNEATList;

    public static int nextAvailableInnovationNumber = 0;
    
	public GenomeNEAT() {
        // Constructor
    }
    public GenomeNEAT(int numInputs, int numOutputs) {
        // Constructor
        if(numInputs < 1 || numOutputs < 1) {
            Debug.LogError("New NEAT Genome requires at least 1 input and output node!! [" + numInputs.ToString() + "," + numOutputs.ToString() + "]");
        }
        else {
            nodeNEATList = new List<GeneNodeNEAT>();
            int currentID = 0;
            for (int i = 0; i < numInputs; i++) {
                GeneNodeNEAT newInputNode = new GeneNodeNEAT(currentID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear);
                nodeNEATList.Add(newInputNode);
                currentID++;
            }
            for (int o = 0; o < numOutputs; o++) {
                GeneNodeNEAT newOutputNode = new GeneNodeNEAT(currentID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid);
                nodeNEATList.Add(newOutputNode);
                currentID++;
            }

            linkNEATList = new List<GeneLinkNEAT>();
            // Empty for now
        }        
    }
    public GenomeNEAT(int numInputs, int numHidden, int numOutputs) {
        // Constructor
        if (numInputs < 1 || numOutputs < 1) {
            Debug.LogError("New NEAT Genome requires at least 1 input and output node!! [" + numInputs.ToString() + "," + numOutputs.ToString() + "]");
        }
        else {
            nodeNEATList = new List<GeneNodeNEAT>();
            int currentID = 0;
            for (int i = 0; i < numInputs; i++) {
                GeneNodeNEAT newInputNode = new GeneNodeNEAT(currentID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear);
                nodeNEATList.Add(newInputNode);
                currentID++;
            }
            for (int h = 0; h < numHidden; h++) {
                GeneNodeNEAT newHiddenNode = new GeneNodeNEAT(currentID, GeneNodeNEAT.GeneNodeType.Hid, TransferFunctions.TransferFunction.RationalSigmoid);
                nodeNEATList.Add(newHiddenNode);
                currentID++;
            }
            for (int o = 0; o < numOutputs; o++) {
                GeneNodeNEAT newOutputNode = new GeneNodeNEAT(currentID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid);
                nodeNEATList.Add(newOutputNode);
                currentID++;
            }

            linkNEATList = new List<GeneLinkNEAT>();
            // Empty for now
        }
    }
    public GenomeNEAT(CritterGenome critterBodyGenome, int numHidden) {
        // Constructor
        nodeNEATList = critterBodyGenome.GetBlankBrainNodesFromBody();  // returns a list of nodeNEAT's based on bodyGenome
        
        int currentID = nodeNEATList.Count;
        // Add in hidden nodes:
        for (int h = 0; h < numHidden; h++) {
            GeneNodeNEAT newHiddenNode = new GeneNodeNEAT(currentID, GeneNodeNEAT.GeneNodeType.Hid, TransferFunctions.TransferFunction.RationalSigmoid, -1, 0, -1);
            nodeNEATList.Add(newHiddenNode);
            currentID++;
        }
        // do I need to save body-part&add-on information on the Brain NODES? or only on Neurons?
        linkNEATList = new List<GeneLinkNEAT>();

        string newGenomeNEATDisplay = "NEW GenomeNEAT! #nodes: " + nodeNEATList.Count.ToString() + "\n";
        for(int i = 0; i < nodeNEATList.Count; i++) {
            newGenomeNEATDisplay += "Node[" + i.ToString() + "] (" + nodeNEATList[i].id.ToString() + ", " + nodeNEATList[i].sourceAddonInno.ToString() + ", " + nodeNEATList[i].sourceAddonRecursionNum.ToString() + ", " + nodeNEATList[i].sourceAddonChannelNum.ToString() + ")\n";
        }
        Debug.Log(newGenomeNEATDisplay);

        /*if (numInputs < 1 || numOutputs < 1) {
            Debug.LogError("New NEAT Genome requires at least 1 input and output node!! [" + numInputs.ToString() + "," + numOutputs.ToString() + "]");
        }
        else {
            nodeNEATList = new List<GeneNodeNEAT>();
            int currentID = 0;
            for (int i = 0; i < numInputs; i++) {
                GeneNodeNEAT newInputNode = new GeneNodeNEAT(currentID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear);
                nodeNEATList.Add(newInputNode);
                currentID++;
            }
            for (int h = 0; h < numHidden; h++) {
                GeneNodeNEAT newHiddenNode = new GeneNodeNEAT(currentID, GeneNodeNEAT.GeneNodeType.Hid, TransferFunctions.TransferFunction.RationalSigmoid);
                nodeNEATList.Add(newHiddenNode);
                currentID++;
            }
            for (int o = 0; o < numOutputs; o++) {
                GeneNodeNEAT newOutputNode = new GeneNodeNEAT(currentID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid);
                nodeNEATList.Add(newOutputNode);
                currentID++;
            }

            linkNEATList = new List<GeneLinkNEAT>();
            // Empty for now
        }*/
    }

    public int GetNodeIndexFromInt3(Int3 identifierTag) {
        int nodeIndex = -1;
        string debugNodes = "GetNodeIndexFromVector3 ERROR!!!Can't Find Node " + identifierTag.ToString() + "\n";
        for (int i = 0; i < nodeNEATList.Count; i++) {
            debugNodes += i.ToString() + ", " + nodeNEATList[i].sourceAddonInno.ToString() + ", " + nodeNEATList[i].sourceAddonRecursionNum.ToString() + ", " + nodeNEATList[i].sourceAddonChannelNum.ToString() + "\n";
            if (nodeNEATList[i].sourceAddonInno == identifierTag.x) {
                if (nodeNEATList[i].sourceAddonRecursionNum == identifierTag.y) {
                    if (nodeNEATList[i].sourceAddonChannelNum == identifierTag.z) {
                        // MATCH!!
                        nodeIndex = i;
                        break;
                    }
                }
            }
        }
        if(nodeIndex == -1) {
            //Debug.Log(debugNodes);
        }

        return nodeIndex;
    }

    public Int3 GetInt3FromNodeIndex(int nodeIndex) {
        return new Int3(nodeNEATList[nodeIndex].sourceAddonInno, nodeNEATList[nodeIndex].sourceAddonRecursionNum, nodeNEATList[nodeIndex].sourceAddonChannelNum);
    }

    public void AdjustBrainAfterBodyChange(ref CritterGenome bodyGenome) {
        // Try to do it assuming this function only took a CritterNode as input, rather than full bodyGenome:
        //CritterNode sourceNode = bodyGenome.CritterNodeList[bodyNodeID];
        string beforeBrain = "AdjustBrain Before: \n";
        string afterBrain = "AdjustBrain After: \n";

        List<GeneNodeNEAT> newBrainNodeList = bodyGenome.GetBlankBrainNodesFromBody();
        //List<GeneLinkNEAT> newBrainLinkList = new List<GeneLinkNEAT>();  // need to make a new copy so that the old link-list stays static while searching for matching from/toID's
        // Find number of Input+Output nodes in the old list:
        int numOriginalInOutNodes = 0;
        for (int i = 0; i < nodeNEATList.Count; i++) {
            if(nodeNEATList[i].nodeType != GeneNodeNEAT.GeneNodeType.Hid) {
                numOriginalInOutNodes++;
            }            
        }
        // Compare this to the number of nodes in the new list (which doesn't contain any hidden nodes)
        int netNewNodes = newBrainNodeList.Count - numOriginalInOutNodes;

        //Debug.Log("AdjustBrainAddedRecursion! numOriginalInOutNodes: " + numOriginalInOutNodes.ToString() + ", netNewNodes: " + netNewNodes.ToString() + ", bodyNodeID: " + bodyNodeID.ToString() + ", recursionNum: " + recursionNum.ToString());

        for (int i = 0; i < nodeNEATList.Count; i++) {
            beforeBrain += "Node " + nodeNEATList[i].id.ToString() + " (" + nodeNEATList[i].sourceAddonInno.ToString() + ", " + nodeNEATList[i].sourceAddonRecursionNum.ToString() + ", " + nodeNEATList[i].sourceAddonChannelNum.ToString() + ")\n";

            if (nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.Hid) {
                GeneNodeNEAT clonedNode = new GeneNodeNEAT(nodeNEATList[i].id, nodeNEATList[i].nodeType, nodeNEATList[i].activationFunction, nodeNEATList[i].sourceAddonInno, nodeNEATList[i].sourceAddonRecursionNum, nodeNEATList[i].sourceAddonChannelNum);
                newBrainNodeList.Add(clonedNode);
            }
        }
        
        if (netNewNodes != 0) {  // if the node that was recursed had any Add-ons/Brain channels on it:
            // Copy all the hidden nodes into the newNodeList, adjust their id's based on the numNewNodes found in previous comment
            
            // Go through all links and if a connectionID is greater than the cutoff-point (was a link to hidden node) then adjust its id by the Offset to match the new listIndex/ID
            /*   // Links SHOULD be able to stay the same now that they use fully unique identifiers for their From-To connections:
            for (int j = 0; j < linkNEATList.Count; j++) {
                beforeBrain += "Link " + linkNEATList[j].innov.ToString() + " (" + linkNEATList[j].fromNodeID.ToString() + ", " + linkNEATList[j].toNodeID.ToString() + ") weight: " + linkNEATList[j].weight.ToString() + ")\n";

                GeneLinkNEAT clonedLink = new GeneLinkNEAT(linkNEATList[j].fromNodeID, linkNEATList[j].toNodeID, linkNEATList[j].weight, linkNEATList[j].enabled, linkNEATList[j].innov, linkNEATList[j].birthGen);
                newBrainLinkList.Add(clonedLink);
                if (linkNEATList[j].fromNodeID > numOriginalInOutNodes) {
                    clonedLink.fromNodeID += netNewNodes;
                    Debug.Log("HIDLink [" + j.ToString() + "] FromNode Re-Number! fromNodeID: " + linkNEATList[j].fromNodeID.ToString());
                }
                if (linkNEATList[j].toNodeID > numOriginalInOutNodes) {
                    clonedLink.toNodeID += netNewNodes;
                    Debug.Log("HIDLink [" + j.ToString() + "] ToNode Re-Number! toNodeID: " + linkNEATList[j].toNodeID.ToString());
                }                
            }*/

            // Go through existing nodeList, searching for matches, based on sourceInno#, recursion#, and channel#        
            /*for (int i = 0; i < nodeNEATList.Count; i++) {
                for (int k = 0; k < newBrainNodeList.Count; k++) {
                    // If so, copy attributes from old node to new node  
                    if (nodeNEATList[i].sourceAddonInno == newBrainNodeList[k].sourceAddonInno) {
                        if (nodeNEATList[i].sourceAddonRecursionNum == newBrainNodeList[k].sourceAddonRecursionNum) {
                            if (nodeNEATList[i].sourceAddonChannelNum == newBrainNodeList[k].sourceAddonChannelNum) {
                                int origID = i;  // or nodeNEATList[i].id???
                                int newID = k;
                                //Debug.Log("NEURON MATCH! i: " + i.ToString() + ", id: " + nodeNEATList[i].id.ToString() + ", orig: " + origID.ToString() + ", new: " + newID.ToString() + " sourceAddonInno: " + nodeNEATList[i].sourceAddonInno.ToString() + " sourceAddonRecursionNum: " + nodeNEATList[i].sourceAddonRecursionNum.ToString() + ", sourceAddonChannelNum: " + nodeNEATList[i].sourceAddonChannelNum.ToString());
                                // Keep track of originalID<=>newID so it can be changed on all existing links:
                                // and re-wire and links that referenced that old node
                                for (int l = 0; l < linkNEATList.Count; l++) {
                                    if (linkNEATList[l].fromNodeID == origID) {
                                        newBrainLinkList[l].fromNodeID = newID;
                                        //Debug.Log("Link [" + l.ToString() + "] FromNode Re-Number! orig: " + origID.ToString() + ", new: " + newID.ToString());
                                    }
                                    if (linkNEATList[l].toNodeID == origID) {
                                        newBrainLinkList[l].toNodeID = newID;
                                        //Debug.Log("Link [" + l.ToString() + "] ToNode Re-Number! orig: " + origID.ToString() + ", new: " + newID.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }*/

            
            //for(int j = 0; j < newBrainLinkList.Count; j++) {
            //    afterBrain += "Link " + newBrainLinkList[j].innov.ToString() + " (" + newBrainLinkList[j].fromNodeID.ToString() + ", " + newBrainLinkList[j].toNodeID.ToString() + ") weight: " + newBrainLinkList[j].weight.ToString() + ")\n";
            //}
        }
        for (int i = 0; i < newBrainNodeList.Count; i++) {
            afterBrain += "Node " + newBrainNodeList[i].id.ToString() + " (" + newBrainNodeList[i].sourceAddonInno.ToString() + ", " + newBrainNodeList[i].sourceAddonRecursionNum.ToString() + ", " + newBrainNodeList[i].sourceAddonChannelNum.ToString() + ")\n";
        }
        // Once complete, replace oldNodeList with the new one:
        nodeNEATList = newBrainNodeList;
        //linkNEATList = newBrainLinkList;

        // Make sure that there aren't any links pointing to non-existent nodes:
        for (int i = 0; i < linkNEATList.Count; i++) {
            bool linkSevered = false;
            if (GetNodeIndexFromInt3(linkNEATList[i].fromNodeID) == -1) {
                linkSevered = true;
            }
            if (GetNodeIndexFromInt3(linkNEATList[i].toNodeID) == -1) {
                linkSevered = true;
            }

            if (linkSevered) {
                linkNEATList[i].enabled = false;
                //Debug.Log("LINK " + i.ToString() + " SEVERED!! " + linkNEATList[i].fromNodeID.ToString() + ", " + linkNEATList[i].toNodeID.ToString());
            }
        }

        //Debug.Log(beforeBrain);
        //Debug.Log(afterBrain + "\n deltaNodes: " + netNewNodes.ToString());
    }
    
    public void CreateInitialConnections(float connectedness, bool randomWeights) {
        int numInputs = 0;
        int numHidden = 0;
        int numOutputs = 0;
        List<GeneNodeNEAT> inputNodeList = new List<GeneNodeNEAT>();
        List<GeneNodeNEAT> hiddenNodeList = new List<GeneNodeNEAT>();
        List<GeneNodeNEAT> outputNodeList = new List<GeneNodeNEAT>();
        for (int i = 0; i < nodeNEATList.Count; i++) {
            if (nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.In) {
                numInputs++;
                inputNodeList.Add(nodeNEATList[i]);
            }
            if (nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.Hid) {
                numHidden++;
                hiddenNodeList.Add(nodeNEATList[i]);
            }
            if (nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.Out) {
                numOutputs++;
                outputNodeList.Add(nodeNEATList[i]);
            }
        }

        if(numHidden > 0) {  // if there is a hidden layer:
            float accumulatedLinkChance = UnityEngine.Random.Range(0f, 1f);  // random offset
            for (int i = 0; i < inputNodeList.Count; i++) {  // input layer to hidden layer
                for (int h = 0; h < hiddenNodeList.Count; h++) {
                    accumulatedLinkChance += connectedness;  // currently deterministic.... is this ok?
                    if (accumulatedLinkChance >= 1f) {  // if connectedness is 0.34, link will be created every third cycle, for example
                        float initialWeight = 0f;
                        if (randomWeights) {
                            initialWeight = Gaussian.GetRandomGaussian();
                        }
                        GeneLinkNEAT newLink = new GeneLinkNEAT(GetInt3FromNodeIndex(inputNodeList[i].id), GetInt3FromNodeIndex(hiddenNodeList[h].id), initialWeight, true, GetNextInnovNumber(), 0);
                        linkNEATList.Add(newLink);
                        accumulatedLinkChance -= 1f; 
                    }
                }
            }
            for (int h = 0; h < hiddenNodeList.Count; h++) {  // hidden layer to output layer
                for (int o = 0; o < outputNodeList.Count; o++) {
                    accumulatedLinkChance += connectedness;  // currently deterministic.... is this ok?
                    if (accumulatedLinkChance >= 1f) {  // if connectedness is 0.34, link will be created every third cycle, for example
                        float initialWeight = 0f;
                        if (randomWeights) {
                            initialWeight = Gaussian.GetRandomGaussian();
                        }
                        GeneLinkNEAT newLink = new GeneLinkNEAT(GetInt3FromNodeIndex(hiddenNodeList[h].id), GetInt3FromNodeIndex(outputNodeList[o].id), initialWeight, true, GetNextInnovNumber(), 0);
                        linkNEATList.Add(newLink);
                        accumulatedLinkChance -= 1f; 
                    }
                }
            }
        }
        else {  // no hidden layer:
            float accumulatedLinkChance = UnityEngine.Random.Range(0f, 1f);  // random offset
            for (int i = 0; i < inputNodeList.Count; i++) {
                
                for(int o = 0; o < outputNodeList.Count; o++) {
                    accumulatedLinkChance += connectedness;  // currently deterministic.... is this ok?
                    if(accumulatedLinkChance >= 1f) {  // if connectedness is 0.34, link will be created every third cycle, for example
                        float initialWeight = 0f;
                        if (randomWeights) {
                            initialWeight = Gaussian.GetRandomGaussian();
                        }
                        GeneLinkNEAT newLink = new GeneLinkNEAT(GetInt3FromNodeIndex(inputNodeList[i].id), GetInt3FromNodeIndex(outputNodeList[o].id), initialWeight, true, GetNextInnovNumber(), 0);
                        linkNEATList.Add(newLink);

                        accumulatedLinkChance -= 1f; 
                    }                    
                }                
            }
        }        
    }

    // loops through all output nodes andcreates a connection from a random input node to the output, so that all output nodes are hooked up
    // should result in the minimum amount of connections required to have 'full' functionality
    public void CreateMinimumRandomConnections() {
        int numInputs = 0;
        int numOutputs = 0;
        List<GeneNodeNEAT> inputNodeList = new List<GeneNodeNEAT>();
        List<GeneNodeNEAT> outputNodeList = new List<GeneNodeNEAT>();
        for (int i = 0; i < nodeNEATList.Count; i++) {
            if(nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.In) {
                numInputs++;
                inputNodeList.Add(nodeNEATList[i]);
            }
            if (nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.Out) {
                numOutputs++;
                outputNodeList.Add(nodeNEATList[i]);
            }
        }

        for(int o = 0; o < outputNodeList.Count; o++) {
            int inNodeID = (int)UnityEngine.Random.Range(0f, (float)inputNodeList.Count);
            float randomWeight = Gaussian.GetRandomGaussian();
            GeneLinkNEAT newLink = new GeneLinkNEAT(GetInt3FromNodeIndex(inputNodeList[inNodeID].id), GetInt3FromNodeIndex(outputNodeList[o].id), randomWeight, true, GetNextInnovNumber(), 0);
            linkNEATList.Add(newLink);
        }
    }

    public void RemoveRandomLink() {
        if(linkNEATList.Count > 0) {
            int randomLinkID = (int)UnityEngine.Random.Range(0f, (float)linkNEATList.Count);
            //Debug.Log("Remove RandomLink #" + randomLinkID.ToString());
            linkNEATList.RemoveAt(randomLinkID);
        }        
    }

    /*public bool AreEqual(Vector3 vec1, Vector3 vec2) {
        bool isEqual = false;
        if(vec1.x == vec2.x) {
            if (vec1.y == vec2.y) {
                if (vec1.z == vec2.z) {
                    isEqual = true;
                }
            }
        }
        return isEqual;
    }
    */
    public void AddNewRandomLink(int gen) {
        
        List<GeneNodeNEAT> eligibleFromNodes = new List<GeneNodeNEAT>();
        List<GeneNodeNEAT> eligibleToNodes = new List<GeneNodeNEAT>();
        for (int i = 0; i < nodeNEATList.Count; i++) {
            if (nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.In) {
                eligibleFromNodes.Add(nodeNEATList[i]);
            }
            else if (nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.Hid) {
                eligibleFromNodes.Add(nodeNEATList[i]);
                eligibleToNodes.Add(nodeNEATList[i]);
            }
            if (nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.Out) {
                eligibleToNodes.Add(nodeNEATList[i]);
            }
        }
        if(eligibleFromNodes.Count > 0 && eligibleToNodes.Count > 0) {
            int fromNodeID = (int)UnityEngine.Random.Range(0f, (float)eligibleFromNodes.Count);
            int toNodeID = (int)UnityEngine.Random.Range(0f, (float)eligibleToNodes.Count);
            if (eligibleFromNodes[fromNodeID].id == eligibleToNodes[toNodeID].id) {
                // Check if this link already exists:
                bool linkExists = false;
                for (int i = 0; i < linkNEATList.Count; i++) {
                    if (GetNodeIndexFromInt3(linkNEATList[i].toNodeID) == eligibleToNodes[toNodeID].id && GetNodeIndexFromInt3(linkNEATList[i].fromNodeID) == eligibleFromNodes[fromNodeID].id) {
                        Debug.Log("Attempted to add link but it already exists!!! from: " + eligibleFromNodes[fromNodeID].id.ToString() + ", to: " + eligibleToNodes[toNodeID].id.ToString());
                        linkExists = true;
                    }
                }
                if (!linkExists) {
                    Debug.Log("New Link TO ITSELF: " + eligibleFromNodes[fromNodeID].id.ToString() + " Doing it anyway!");
                    float randomWeight = Gaussian.GetRandomGaussian() * 0.2f; //0f; // start zeroed to give a chance to try both + and - //Gaussian.GetRandomGaussian();
                    GeneLinkNEAT newLink = new GeneLinkNEAT(GetInt3FromNodeIndex(eligibleFromNodes[fromNodeID].id), GetInt3FromNodeIndex(eligibleToNodes[toNodeID].id), randomWeight, true, GetNextInnovNumber(), gen);
                    linkNEATList.Add(newLink);
                }

            }
            else {
                // Check if this link already exists:
                bool linkExists = false;
                for (int i = 0; i < linkNEATList.Count; i++) {
                    if (GetNodeIndexFromInt3(linkNEATList[i].toNodeID) == eligibleToNodes[toNodeID].id && GetNodeIndexFromInt3(linkNEATList[i].fromNodeID) == eligibleFromNodes[fromNodeID].id) {
                        //Debug.Log("Attempted to add link but it already exists!!! from: " + eligibleFromNodes[fromNodeID].id.ToString() + ", to: " + eligibleToNodes[toNodeID].id.ToString());
                        linkExists = true;
                    }
                }
                if (!linkExists) {
                    float randomWeight = Gaussian.GetRandomGaussian() * 0.2f; //0f; // start zeroed to give a chance to try both + and - //Gaussian.GetRandomGaussian();
                    GeneLinkNEAT newLink = new GeneLinkNEAT(GetInt3FromNodeIndex(eligibleFromNodes[fromNodeID].id), GetInt3FromNodeIndex(eligibleToNodes[toNodeID].id), randomWeight, true, GetNextInnovNumber(), gen);
                    linkNEATList.Add(newLink);
                }
            }
        }                
    }

    public void AddNewExtraLink(float fromBias, int gen) {  // has a higher-than-random chance to create a new link with at least one node that is already connected

        List<GeneNodeNEAT> eligibleFromNodes = new List<GeneNodeNEAT>();
        List<GeneNodeNEAT> eligibleToNodes = new List<GeneNodeNEAT>();

        bool reuseFromNode;  // true = use an existing FROM node,  false = use an existing TO node
        float randToOrFrom = UnityEngine.Random.Range(0f, 1f);
        if(randToOrFrom < fromBias) {
            reuseFromNode = true;
        }
        else {
            reuseFromNode = false;
        }
        for (int k = 0; k < linkNEATList.Count; k++) {
            // Populate eligible node lists with those nodes that are already connected:
            if(linkNEATList[k].enabled) {
                if(reuseFromNode) {  // reuse an exisiting fromNode
                    if (eligibleFromNodes.Contains(nodeNEATList[GetNodeIndexFromInt3(linkNEATList[k].fromNodeID)])) {
                        //Debug.Log("AddNewExtraLink() EligibleFromNode Already Contains Node " + linkNEATList[k].fromNodeID.ToString());
                    }
                    else {
                        eligibleFromNodes.Add(nodeNEATList[GetNodeIndexFromInt3(linkNEATList[k].fromNodeID)]);
                    }
                }
                else { // reuse an exisitng TO node
                    if (eligibleToNodes.Contains(nodeNEATList[GetNodeIndexFromInt3(linkNEATList[k].toNodeID)])) {
                        //Debug.Log("AddNewExtraLink() EligibleToNode Already Contains Node " + linkNEATList[k].toNodeID.ToString());
                    }
                    else {
                        eligibleToNodes.Add(nodeNEATList[GetNodeIndexFromInt3(linkNEATList[k].toNodeID)]);
                    }
                }                
            }
        }
        for (int i = 0; i < nodeNEATList.Count; i++) {
            if (reuseFromNode) {  // if re-using an exisitng FROM node, then get a TO node from random:
                if (nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.Hid) {
                    eligibleToNodes.Add(nodeNEATList[i]);
                }
                else if (nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.Out) {
                    eligibleToNodes.Add(nodeNEATList[i]);
                }                
            }
            else {   // if re-using an exisitng TO node, then get a FROM node from random:
                if (nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.In) {
                    eligibleFromNodes.Add(nodeNEATList[i]);
                }
                else if (nodeNEATList[i].nodeType == GeneNodeNEAT.GeneNodeType.Hid) {
                    eligibleFromNodes.Add(nodeNEATList[i]);
                }
            }
        }

        if (eligibleFromNodes.Count > 0 && eligibleToNodes.Count > 0) {
            // make sure that there is at least 1 from and to node that is possible
            int fromNodeID = (int)UnityEngine.Random.Range(0f, (float)eligibleFromNodes.Count);
            int toNodeID = (int)UnityEngine.Random.Range(0f, (float)eligibleToNodes.Count);
            if (eligibleFromNodes[fromNodeID].id == eligibleToNodes[toNodeID].id) {
                // Check if this link already exists:
                bool linkExists = false;
                for (int i = 0; i < linkNEATList.Count; i++) {
                    if (GetNodeIndexFromInt3(linkNEATList[i].toNodeID) == eligibleToNodes[toNodeID].id && GetNodeIndexFromInt3(linkNEATList[i].fromNodeID) == eligibleFromNodes[fromNodeID].id) {
                        Debug.Log("AddNewExtraLink() Attempted to add link but it already exists!!! from: " + eligibleFromNodes[fromNodeID].id.ToString() + ", to: " + eligibleToNodes[toNodeID].id.ToString());
                        linkExists = true;
                    }
                }
                if (!linkExists) {
                    Debug.Log("AddNewExtraLink() New Link TO ITSELF: " + eligibleFromNodes[fromNodeID].id.ToString() + " Doing it anyway!");
                    float randomWeight = Gaussian.GetRandomGaussian() * 0.2f; //0f; // start zeroed to give a chance to try both + and - //Gaussian.GetRandomGaussian();
                    GeneLinkNEAT newLink = new GeneLinkNEAT(GetInt3FromNodeIndex(eligibleFromNodes[fromNodeID].id), GetInt3FromNodeIndex(eligibleToNodes[toNodeID].id), randomWeight, true, GetNextInnovNumber(), gen);
                    linkNEATList.Add(newLink);
                }                
            }
            else {
                // Check if this link already exists:
                bool linkExists = false;
                for (int i = 0; i < linkNEATList.Count; i++) {
                    if (GetNodeIndexFromInt3(linkNEATList[i].toNodeID) == eligibleToNodes[toNodeID].id && GetNodeIndexFromInt3(linkNEATList[i].fromNodeID) == eligibleFromNodes[fromNodeID].id) {
                        Debug.Log("AddNewExtraLink() Attempted to add link but it already exists!!! from: " + eligibleFromNodes[fromNodeID].id.ToString() + ", to: " + eligibleToNodes[toNodeID].id.ToString());
                        linkExists = true;
                    }
                }
                if (!linkExists) {

                    float randomWeight = Gaussian.GetRandomGaussian() * 0.2f; //0f; // start zeroed to give a chance to try both + and - //Gaussian.GetRandomGaussian();
                    Debug.Log("AddNewExtraLink() NEW LINK!!! from: " + eligibleFromNodes[fromNodeID].id.ToString() + ", to: " + eligibleToNodes[toNodeID].id.ToString());
                    GeneLinkNEAT newLink = new GeneLinkNEAT(GetInt3FromNodeIndex(eligibleFromNodes[fromNodeID].id), GetInt3FromNodeIndex(eligibleToNodes[toNodeID].id), randomWeight, true, GetNextInnovNumber(), gen);
                    linkNEATList.Add(newLink);
                }
            }
        }        
    }

    public void AddNewRandomNode(int gen) {
        if(linkNEATList.Count > 0) {
            int linkID = (int)UnityEngine.Random.Range(0f, (float)linkNEATList.Count);
            linkNEATList[linkID].enabled = false;  // disable old connection
            GeneNodeNEAT newHiddenNode = new GeneNodeNEAT(nodeNEATList.Count, GeneNodeNEAT.GeneNodeType.Hid, TransferFunctions.TransferFunction.RationalSigmoid, -1, 0, -1);
            nodeNEATList.Add(newHiddenNode);
            // add new node between old connection
            // create two new connections
            GeneLinkNEAT newLinkA = new GeneLinkNEAT(linkNEATList[linkID].fromNodeID, GetInt3FromNodeIndex(newHiddenNode.id), linkNEATList[linkID].weight, true, GetNextInnovNumber(), gen);
            GeneLinkNEAT newLinkB = new GeneLinkNEAT(GetInt3FromNodeIndex(newHiddenNode.id), linkNEATList[linkID].toNodeID, 1f, true, GetNextInnovNumber(), gen);

            linkNEATList.Add(newLinkA);
            linkNEATList.Add(newLinkB);

            //Debug.Log("AddNewRandomNode() linkID: " + linkID.ToString() + ", ");
        }
        else {
            Debug.Log("No connections! Can't create new node!!!");
        }
    }

    private int GetNextInnovNumber() {
        int nextNum = nextAvailableInnovationNumber;
        nextAvailableInnovationNumber++;
        return nextNum;
    }

    public int ReadMaxInno() {
        return nextAvailableInnovationNumber;
    }

    public static float MeasureGeneticDistance(GenomeNEAT genomeA, GenomeNEAT genomeB, float excessCoefficient, float disjointCoefficient, float weightCoefficient, float normExcess, float normDisjoint, float normWeight) {
        
        int indexA = 0;
        int indexB = 0;

        float weightDeltaTotal = 0f;
        float maxGenes = Mathf.Max((float)genomeA.linkNEATList.Count, (float)genomeB.linkNEATList.Count);
        float matchingGenes = 0f;
        float disjointGenes = 0f;
        float excessGenes = 0f;

        // matching Links
        // matching nodes/neurons
        // matching activation functions?
        // difference in weights
        // difference in add-on settings?
        // -- NEED TO GO BY BODY GENOME?????

        for(int i = 0; i < genomeA.linkNEATList.Count + genomeB.linkNEATList.Count; i++) {
            if(indexA < genomeA.linkNEATList.Count) {

                if (indexB < genomeB.linkNEATList.Count) { // both good

                    if (genomeA.linkNEATList[indexA].innov < genomeB.linkNEATList[indexB].innov) {
                        // disjoint A
                        disjointGenes++;
                        indexA++;
                    }
                    else if (genomeA.linkNEATList[indexA].innov > genomeB.linkNEATList[indexB].innov) {
                        // disjoint B
                        disjointGenes++;
                        indexB++;
                    }
                    else if (genomeA.linkNEATList[indexA].innov == genomeB.linkNEATList[indexB].innov) {
                        // match!
                        
                        weightDeltaTotal += Mathf.Abs(genomeA.linkNEATList[indexA].weight - genomeB.linkNEATList[indexB].weight);
                        matchingGenes++;
                        //Debug.Log("!@$#!$#!@$#!@$# MeasureGeneticDistance! innov MATCHING GENE: weightA: " + genomeA.linkNEATList[indexA].weight.ToString() + ", weightB: " + genomeB.linkNEATList[indexB].weight.ToString());
                        indexA++;
                        indexB++;
                    }

                }
                else { // A is good, B is finished
                    excessGenes++;
                    indexA++;
                }
            }
            else { // A done
                if (indexB < genomeB.linkNEATList.Count) {  // A is finished, B is good
                    excessGenes++;
                    indexB++;

                }
                else { // both done
                    break;
                }
            }            
        }

        float distance = 0f;
        if(maxGenes > 0f) {
            float excessComponent = excessCoefficient * Mathf.Lerp(excessGenes, excessGenes / maxGenes, normExcess);
            float disjointComponent = disjointCoefficient * Mathf.Lerp(disjointGenes, disjointGenes / maxGenes, normDisjoint);
            float weightComponent = weightCoefficient * Mathf.Lerp(weightDeltaTotal, weightDeltaTotal / Mathf.Max((float)matchingGenes, 1f), normWeight);
            distance = excessComponent + disjointComponent + weightComponent;
            //Debug.Log("MeasureGeneticDistance! disjoint: " + (disjointGenes).ToString() + ", excess: " + (excessGenes).ToString() + ", weight: " + (weightDeltaTotal / Mathf.Max((float)matchingGenes, 1f)).ToString() + ", distance: " + distance.ToString());
        }
        // else stays 0f;
        return distance;
    }
}
