using UnityEngine;
using UnityEngine.UI;

public class FillEnergyBar : MonoBehaviour {
    
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
        CurrentValue = 0f;
    }
}