using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorGround : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D a){
        if(a.gameObject.tag == "ground")
            GetComponentInParent<PlayerController>().isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D a){
        if(a.gameObject.tag == "ground")
            GetComponentInParent<PlayerController>().isGrounded = false;
    }
}
