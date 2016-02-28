using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelAddonDisplayFloat : MonoBehaviour {

    public float[] linkedFloatValue;
    public float sliderMinValue = 0f;
    public float sliderMaxValue = 1f;

    public Text textFloatName;
    public Slider sliderFloat;
    public Text textFloatValue;
    
    void Awake() {
        linkedFloatValue = new float[1];
    }

    public void ClickSliderFloat(float value) {
        linkedFloatValue[0] = value;
        textFloatValue.text = linkedFloatValue[0].ToString();
        //Debug.Log("Slider linkedFloatValue: " + linkedFloatValue[0].ToString());
    }
}
