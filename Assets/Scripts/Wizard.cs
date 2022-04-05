using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody;
    Animator animator;
    public GameObject fireball;
    float hlth = 20;

    private void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate(){
        if(GameObject.Find("Player").transform.position.x < transform.position.x)
            spriteRenderer.flipX = false;
        else if(GameObject.Find("Player").transform.position.x > transform.position.x)
            spriteRenderer.flipX = true;
    }
    private void OnTriggerEnter2D(Collider2D a){
        if(a.gameObject.tag == "Player"){
            AudioBox.instance.AudioPlay(AudioName.Wizard);
            animator.SetInteger("Wizard", 1);
            Progress.StartNewProgress(this.gameObject, 0.9f, SpawnFireball);
        }
        
        if(a.gameObject.tag == "EnemyDamage"){
            Damage();
        }
    }
    private void OnTriggerExit2D(Collider2D a){
        if(a.gameObject.tag == "Player"){
            animator.SetInteger("Wizard", 0);
            Progress.StartNewProgress(this.gameObject, 0, SpawnFireball);
        }
    }

    void SpawnFireball(){
        if(spriteRenderer.flipX == false){
            fireball.GetComponent<SpriteRenderer>().flipX = false;
            Instantiate(fireball, new Vector2(transform.position.x - 0.3f, transform.position.y), Quaternion.identity);
        }
        else if(spriteRenderer.flipX == true){
            fireball.GetComponent<SpriteRenderer>().flipX = true;
            Instantiate(fireball, new Vector2(transform.position.x + 0.3f, transform.position.y), Quaternion.identity);
        }
    }

    public void Damage(){
        if(GameObject.Find("Player").GetComponent<CharacterAnimation>().energyBonus == true)
            hlth -= 5;
        else 
            hlth--;
            
        if(hlth <= 0){
            animator.SetInteger("Wizard", 2); 
            Invoke("Destroy", 0.5f);
        }
    }

    private void Destroy() {
        gameObject.SetActive(false);
        GameObject.Find("Player").GetComponent<CharacterAnimation>().energy += 20f;
    }
}
