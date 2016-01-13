using UnityEngine;
using System.Collections;

public class GameOptionChannel {

	public float[] channelValue; // See if I can figure out how to make this dataType generic
	//public bool on; // This means on/off selection choice is stored in the GameMoveToTarget1D instance
	public string channelName;

	public float minValue;
	public float maxValue;
	
	// Constructor Methods!
	public GameOptionChannel(ref float[] value, float min, float max, string name) {
		channelValue = value;
		channelName = name;

		minValue = min;
		maxValue = max;
	}
}
