using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelAddonDisplayInt : MonoBehaviour {

    public int[] linkedIntValue;
    
    public Text textIntName;
    public Slider sliderInt;
    public Text textIntValue;
    
    void Awake() {
        linkedIntValue = new int[1];
    }

    public void ClickSliderInt(float value) {
        linkedIntValue[0] = (int)value;
        textIntValue.text = linkedIntValue[0].ToString();
        //Debug.Log("Slider linkedFloatValue: " + linkedFloatValue[0].ToString());
    }
}
