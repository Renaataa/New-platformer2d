using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchPlayer : MonoBehaviour
{
    Transform player; 
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime+0.02f);
    }
}
