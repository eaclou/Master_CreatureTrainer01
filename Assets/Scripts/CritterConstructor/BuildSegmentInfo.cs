using UnityEngine;
using System.Collections;

public class BuildSegmentInfo {
    // should this be a struct instead?
    public CritterNode sourceNode;
    public CritterSegment parentSegment;
    public bool isMirror = false;

	// Constructor!
	public BuildSegmentInfo() {
	
	}
	
}
