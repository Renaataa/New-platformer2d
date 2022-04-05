using UnityEngine;

public class Ghoul : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody;
    float spd = 1f;
    float hlth = 10;

    private void Start (){
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate(){
        transform.Translate(transform.right * spd * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D c){
        if(c.gameObject.tag == "Enemy"){
            if(c.gameObject.GetComponent<Ghoul>() == true){
                Physics2D.IgnoreCollision(c.collider, GetComponent<Collider2D>(), true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D a){
        if(a.gameObject.tag == "EnemyDamage"){
            Damage();
        }
        if(a.gameObject.tag == "ground"){
            spd = -spd;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    public void Damage(){
        if(GameObject.Find("Player").GetComponent<CharacterAnimation>().energyBonus == true)
            hlth -= 5;
        else 
            hlth--;

        if(hlth <= 0){
            animator.SetTrigger("Ghoul");
            spd = 0;
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX|RigidbodyConstraints2D.FreezeRotation;
            Invoke("Destroy", 0.6f);
        }
    }

    private void Destroy() {
        gameObject.SetActive(false);
        GameObject.Find("Player").GetComponent<CharacterAnimation>().energy += 10f;
    }
}
