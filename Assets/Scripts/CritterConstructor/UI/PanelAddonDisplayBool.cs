using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelAddonDisplayBool : MonoBehaviour {

    public bool[] linkedBoolValue;

    public Text textBoolName;
    public Toggle toggleBool;
    
    void Awake() {
        linkedBoolValue = new bool[1];
    }

    public void ClickToggleBool(bool value) {
        linkedBoolValue[0] = value;
        //textFloatValue.text = linkedFloatValue[0].ToString();
        //Debug.Log("Slider linkedFloatValue: " + linkedFloatValue[0].ToString());
    }
}
