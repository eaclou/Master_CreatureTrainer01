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
            GeneLinkNEAT newLink = new GeneLinkNEAT(inputNodeList[inNodeID].id, outputNodeList[o].id, randomWeight, true, GetNextInnovNumber(), 0);
            linkNEATList.Add(newLink);
        }
    }

    public void RemoveRandomLink() {
        if(linkNEATList.Count > 0) {
            int randomLinkID = (int)UnityEngine.Random.Range(0f, (float)linkNEATList.Count);
            Debug.Log("Remove RandomLink #" + randomLinkID.ToString());
            linkNEATList.RemoveAt(randomLinkID);
        }        
    }

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
        int fromNodeID = (int)UnityEngine.Random.Range(0f, (float)eligibleFromNodes.Count);
        int toNodeID = (int)UnityEngine.Random.Range(0f, (float)eligibleToNodes.Count);
        if(eligibleFromNodes[fromNodeID].id == eligibleToNodes[toNodeID].id) {
            // Check if this link already exists:
            bool linkExists = false;
            for (int i = 0; i < linkNEATList.Count; i++) {
                if (linkNEATList[i].toNodeID == eligibleToNodes[toNodeID].id && linkNEATList[i].fromNodeID == eligibleFromNodes[fromNodeID].id) {
                    Debug.Log("Attempted to add link but it already exists!!! from: " + eligibleFromNodes[fromNodeID].id.ToString() + ", to: " + eligibleToNodes[toNodeID].id.ToString());
                    linkExists = true;
                }
            }
            if (!linkExists) {
                Debug.Log("New Link TO ITSELF: " + eligibleFromNodes[fromNodeID].id.ToString() + " Doing it anyway!");
                float randomWeight = Gaussian.GetRandomGaussian() * 0.2f; //0f; // start zeroed to give a chance to try both + and - //Gaussian.GetRandomGaussian();
                GeneLinkNEAT newLink = new GeneLinkNEAT(eligibleFromNodes[fromNodeID].id, eligibleToNodes[toNodeID].id, randomWeight, true, GetNextInnovNumber(), gen);
                linkNEATList.Add(newLink);
            }
            
        }
        else {
            // Check if this link already exists:
            bool linkExists = false;
            for(int i = 0; i < linkNEATList.Count; i++) {
                if(linkNEATList[i].toNodeID == eligibleToNodes[toNodeID].id && linkNEATList[i].fromNodeID == eligibleFromNodes[fromNodeID].id) {
                    Debug.Log("Attempted to add link but it already exists!!! from: " + eligibleFromNodes[fromNodeID].id.ToString() + ", to: " + eligibleToNodes[toNodeID].id.ToString());
                    linkExists = true;                    
                }
            }
            if(!linkExists) {
                float randomWeight = Gaussian.GetRandomGaussian() * 0.2f; //0f; // start zeroed to give a chance to try both + and - //Gaussian.GetRandomGaussian();
                GeneLinkNEAT newLink = new GeneLinkNEAT(eligibleFromNodes[fromNodeID].id, eligibleToNodes[toNodeID].id, randomWeight, true, GetNextInnovNumber(), gen);
                linkNEATList.Add(newLink);
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
                    if (eligibleFromNodes.Contains(nodeNEATList[linkNEATList[k].fromNodeID])) {
                        //Debug.Log("AddNewExtraLink() EligibleFromNode Already Contains Node " + linkNEATList[k].fromNodeID.ToString());
                    }
                    else {
                        eligibleFromNodes.Add(nodeNEATList[linkNEATList[k].fromNodeID]);
                    }
                }
                else { // reuse an exisitng TO node
                    if (eligibleToNodes.Contains(nodeNEATList[linkNEATList[k].toNodeID])) {
                        //Debug.Log("AddNewExtraLink() EligibleToNode Already Contains Node " + linkNEATList[k].toNodeID.ToString());
                    }
                    else {
                        eligibleToNodes.Add(nodeNEATList[linkNEATList[k].toNodeID]);
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
                    if (linkNEATList[i].toNodeID == eligibleToNodes[toNodeID].id && linkNEATList[i].fromNodeID == eligibleFromNodes[fromNodeID].id) {
                        Debug.Log("AddNewExtraLink() Attempted to add link but it already exists!!! from: " + eligibleFromNodes[fromNodeID].id.ToString() + ", to: " + eligibleToNodes[toNodeID].id.ToString());
                        linkExists = true;
                    }
                }
                if (!linkExists) {
                    Debug.Log("AddNewExtraLink() New Link TO ITSELF: " + eligibleFromNodes[fromNodeID].id.ToString() + " Doing it anyway!");
                    float randomWeight = Gaussian.GetRandomGaussian() * 0.2f; //0f; // start zeroed to give a chance to try both + and - //Gaussian.GetRandomGaussian();
                    GeneLinkNEAT newLink = new GeneLinkNEAT(eligibleFromNodes[fromNodeID].id, eligibleToNodes[toNodeID].id, randomWeight, true, GetNextInnovNumber(), gen);
                    linkNEATList.Add(newLink);
                }                
            }
            else {
                // Check if this link already exists:
                bool linkExists = false;
                for (int i = 0; i < linkNEATList.Count; i++) {
                    if (linkNEATList[i].toNodeID == eligibleToNodes[toNodeID].id && linkNEATList[i].fromNodeID == eligibleFromNodes[fromNodeID].id) {
                        Debug.Log("AddNewExtraLink() Attempted to add link but it already exists!!! from: " + eligibleFromNodes[fromNodeID].id.ToString() + ", to: " + eligibleToNodes[toNodeID].id.ToString());
                        linkExists = true;
                    }
                }
                if (!linkExists) {

                    float randomWeight = Gaussian.GetRandomGaussian() * 0.2f; //0f; // start zeroed to give a chance to try both + and - //Gaussian.GetRandomGaussian();
                    Debug.Log("AddNewExtraLink() NEW LINK!!! from: " + eligibleFromNodes[fromNodeID].id.ToString() + ", to: " + eligibleToNodes[toNodeID].id.ToString());
                    GeneLinkNEAT newLink = new GeneLinkNEAT(eligibleFromNodes[fromNodeID].id, eligibleToNodes[toNodeID].id, randomWeight, true, GetNextInnovNumber(), gen);
                    linkNEATList.Add(newLink);
                }
            }
        }        
    }

    public void AddNewRandomNode(int gen) {
        if(linkNEATList.Count > 0) {
            int linkID = (int)UnityEngine.Random.Range(0f, (float)linkNEATList.Count);
            linkNEATList[linkID].enabled = false;  // disable old connection
            GeneNodeNEAT newHiddenNode = new GeneNodeNEAT(nodeNEATList.Count, GeneNodeNEAT.GeneNodeType.Hid, TransferFunctions.TransferFunction.RationalSigmoid);
            nodeNEATList.Add(newHiddenNode);
            // add new node between old connection
            // create two new connections
            GeneLinkNEAT newLinkA = new GeneLinkNEAT(linkNEATList[linkID].fromNodeID, newHiddenNode.id, linkNEATList[linkID].weight, true, GetNextInnovNumber(), gen);
            GeneLinkNEAT newLinkB = new GeneLinkNEAT(newHiddenNode.id, linkNEATList[linkID].toNodeID, 1f, true, GetNextInnovNumber(), gen);

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
