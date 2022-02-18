using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;

    private void Start(){
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject.Find("AudioBoxEnemy").GetComponent<AudioBoxEnemy>().AudioPlay(GameObject.Find("AudioBoxEnemy").GetComponent<AudioBoxEnemy>().flyingFireball);
        Destroy(gameObject, 1);
    }

    private void FixedUpdate(){
        if(spriteRenderer.flipX == true)
            rigidbody.AddForce(transform.right * 2, ForceMode2D.Force);
        else if(spriteRenderer.flipX == false)
            rigidbody.AddForce(-transform.right * 2, ForceMode2D.Force);
    }
    private void OnCollisionEnter2D(Collision2D a){
        if(a.gameObject.tag == "Player" || a.gameObject.tag == "ground"){
            GameObject.Find("AudioBoxEnemy").GetComponent<AudioBoxEnemy>().AudioPlay(GameObject.Find("AudioBoxEnemy").GetComponent<AudioBoxEnemy>().explosingFireball);
            Destroy(gameObject);
        }
            
    }
}
