using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelAddonDisplayVector3 : MonoBehaviour {

    public Vector3[] linkedVector3Value;

    public Text textVector3Name;
    public Slider sliderX;
    public Text textXValue;
    public Slider sliderY;
    public Text textYValue;
    public Slider sliderZ;
    public Text textZValue;

    void Awake() {
        linkedVector3Value = new Vector3[1];
    }

    public void ClickSliderX(float value) {
        linkedVector3Value[0].x = value;
        textXValue.text = linkedVector3Value[0].x.ToString();
        //Debug.Log("Slider linkedFloatValue: " + linkedFloatValue[0].ToString());
    }
    public void ClickSliderY(float value) {
        linkedVector3Value[0].y = value;
        textYValue.text = linkedVector3Value[0].y.ToString();
        //Debug.Log("Slider linkedFloatValue: " + linkedFloatValue[0].ToString());
    }
    public void ClickSliderZ(float value) {
        linkedVector3Value[0].z = value;
        textZValue.text = linkedVector3Value[0].z.ToString();
        //Debug.Log("Slider linkedFloatValue: " + linkedFloatValue[0].ToString());
    }
}
