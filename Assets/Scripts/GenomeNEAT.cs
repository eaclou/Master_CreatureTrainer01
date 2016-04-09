using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenomeNEAT {

    public List<GeneNodeNEAT> nodeNEATList;
    public List<GeneLinkNEAT> linkNEATList;

    private int nextAvailableInnovationNumber = 0;

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
                GeneNodeNEAT newInputNode = new GeneNodeNEAT(currentID, GeneNodeNEAT.GeneNodeType.In);
                nodeNEATList.Add(newInputNode);
                currentID++;
            }
            for (int o = 0; o < numOutputs; o++) {
                GeneNodeNEAT newOutputNode = new GeneNodeNEAT(currentID, GeneNodeNEAT.GeneNodeType.Out);
                nodeNEATList.Add(newOutputNode);
                currentID++;
            }

            linkNEATList = new List<GeneLinkNEAT>();
            // Empty for now
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
            GeneLinkNEAT newLink = new GeneLinkNEAT(inputNodeList[inNodeID].id, outputNodeList[o].id, randomWeight, true, GetNextInnovNumber());
            linkNEATList.Add(newLink);
        }
    }

    public void AddNewRandomLink() {
        
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
        int fromNodeID = (int)UnityEngine.Random.Range(0f, (float)eligibleFromNodes.Count);
        int toNodeID = (int)UnityEngine.Random.Range(0f, (float)eligibleToNodes.Count);
        if(eligibleFromNodes[fromNodeID].id == eligibleToNodes[toNodeID].id) {
            Debug.Log("New Link TO ITSELF: " + fromNodeID.ToString() + " Doing it anyway!");
            float randomWeight = Gaussian.GetRandomGaussian();
            GeneLinkNEAT newLink = new GeneLinkNEAT(eligibleFromNodes[fromNodeID].id, eligibleToNodes[toNodeID].id, randomWeight, true, GetNextInnovNumber());
            linkNEATList.Add(newLink);
        }
        else {
            float randomWeight = Gaussian.GetRandomGaussian();
            GeneLinkNEAT newLink = new GeneLinkNEAT(eligibleFromNodes[fromNodeID].id, eligibleToNodes[toNodeID].id, randomWeight, true, GetNextInnovNumber());
            linkNEATList.Add(newLink);
        }
        
    }

    public void AddNewRandomNode() {
        if(linkNEATList.Count > 0) {
            int linkID = (int)UnityEngine.Random.Range(0f, (float)linkNEATList.Count);
            linkNEATList[linkID].enabled = false;  // disable old connection
            GeneNodeNEAT newHiddenNode = new GeneNodeNEAT(nodeNEATList.Count, GeneNodeNEAT.GeneNodeType.Hid);
            nodeNEATList.Add(newHiddenNode);
            // add new node between old connection
            // create two new connections
            GeneLinkNEAT newLinkA = new GeneLinkNEAT(linkNEATList[linkID].fromNodeID, newHiddenNode.id, linkNEATList[linkID].weight, true, GetNextInnovNumber());
            GeneLinkNEAT newLinkB = new GeneLinkNEAT(newHiddenNode.id, linkNEATList[linkID].toNodeID, 1f, true, GetNextInnovNumber());

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
}
