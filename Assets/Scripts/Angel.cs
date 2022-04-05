using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour
{
    public AudioSource audioSource;
    
    Rigidbody2D rigidbody;
    Animator animator;
    SpriteRenderer spriteRenderer;
    bool att = false;
    private float hlth = 30;

    private Transform playerTransform;
    
    private void Start(){
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.Find("Player").transform;
    } 

    private void FixedUpdate(){
        if(att == true){
            animator.SetInteger("Angel", 1);
            transform.position = Vector2.Lerp(transform.position, playerTransform.position, Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D a){
        if (a.gameObject.tag == "Player")
        {
            att = true;
            audioSource.Play();
        }

        if(a.gameObject.tag == "EnemyDamage"){
            Damage();
        }
    }

    public void Damage(){
        if(GameObject.Find("Player").GetComponent<CharacterAnimation>().energyBonus == true)
            hlth -= 5;
        else 
            hlth--;

        if(hlth <= 0){
            animator.SetInteger("Angel", 2);
            att = false;
            audioSource.Stop();
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX|RigidbodyConstraints2D.FreezePositionY|RigidbodyConstraints2D.FreezeRotation;
            Invoke("Destroy", 0.6f);
        }
    }

    private void Destroy() {
        gameObject.SetActive(false);
        GameObject.Find("Player").GetComponent<CharacterAnimation>().energy += 30f;
    }
}
