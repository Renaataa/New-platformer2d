using UnityEngine;
using UnityEngine.UI;

public class FillHealthBar : MonoBehaviour {
    
    public Slider slider;
    
    private float cv = 0f;
    public float CurrentValue {
        get {
            return cv;
        }
        set {
            cv = value;
            slider.value = cv;
        }
    }

    void Start () {
        CurrentValue = 1f;
    }
}