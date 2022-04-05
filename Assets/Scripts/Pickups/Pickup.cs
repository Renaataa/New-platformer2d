using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupType type;
    public int amount = 1;
    public AudioName audioName;
    public void GetPickup()
    {
        AudioBox.instance.AudioPlay(audioName);
        Destroy(gameObject);
    }
}
