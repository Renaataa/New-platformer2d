using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterAnimation : MonoBehaviour
{
    public Joystick joystick;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D coll;
    public GameObject damage;
    int hit;
    int currLevel = 0;
    public float health = 10;
    public float energy;
    bool activeBoost = false;
    bool protect = false;
    public int coin;
    public int countHealth = 0;
    public int countJump = 0;
    public int countSpeed = 0;
    public int countProtect = 0;
    public GameObject panelBoost;
    public GameObject healthBoost;
    public GameObject jumpBoost;
    public GameObject speedBoost;
    public GameObject protectBoost;
    public bool energyBonus = false;
    public GameObject PanelGameOver;
    public GameObject PanelWin;
    public GameObject ButtonNext;

    void Start(){
        Debug.Log("character anim start");
        panelBoost = GameObject.Find("PanelBoost");
        jumpBoost = GameObject.Find("JumpBoost");
        healthBoost = GameObject.Find("HealthBoost");
        speedBoost = GameObject.Find("SpeedBoost");
        protectBoost = GameObject.Find("ProtectBoost");

        panelBoost.SetActive(false);
        jumpBoost.SetActive(false);
        healthBoost.SetActive(false);
        speedBoost.SetActive(false);
        protectBoost.SetActive(false);

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //
        UpdateProgressFromDB();
    }

    void UpdateProgressFromDB()
    {
        currLevel = int.Parse(SceneManager.GetActiveScene().name);
        coin = LoginPanel.loggedPlayerProgress.coins;
        GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>().text = coin.ToString();

        //health = LoginPanel.loggedPlayerProgress.health;
        //energy = LoginPanel.loggedPlayerProgress.energy;
        countHealth = LoginPanel.loggedPlayerProgress.boost.health;
        countJump = LoginPanel.loggedPlayerProgress.boost.jump;
        countSpeed = LoginPanel.loggedPlayerProgress.boost.speed;
        countProtect = LoginPanel.loggedPlayerProgress.boost.protect;
        ShowBoostPanelIfNeeded();
        UpdatePanelBoostValues();
    }

    void ShowBoostPanelIfNeeded()
    {
        panelBoost.SetActive(false);
        healthBoost.SetActive(false);
        jumpBoost.SetActive(false);
        speedBoost.SetActive(false);
        protectBoost.SetActive(false);

        if(countHealth >0)
        {
            panelBoost.SetActive(true);
            healthBoost.SetActive(true);
        }

        if(countJump >0)
        {
            panelBoost.SetActive(true);
            jumpBoost.SetActive(true);
        }

        if( countSpeed>0 )
        {
            panelBoost.SetActive(true);
             speedBoost.SetActive(true);
        }

        if(countProtect >0)
        {
            panelBoost.SetActive(true);
            protectBoost.SetActive(true);
        }
    }

    void UpdatePanelBoostValues()
    {
        if(!panelBoost.activeSelf) return;

        if(healthBoost.activeSelf) GameObject.Find("HealthBoostText").GetComponent<TextMeshProUGUI>().text = countHealth.ToString();
        if(jumpBoost.activeSelf) GameObject.Find("JumpBoostText").GetComponent<TextMeshProUGUI>().text = countJump.ToString();
        if(speedBoost.activeSelf) GameObject.Find("SpeedBoostText").GetComponent<TextMeshProUGUI>().text = countSpeed.ToString();
        if(protectBoost.activeSelf) GameObject.Find("ProtectBoostText").GetComponent<TextMeshProUGUI>().text = countProtect.ToString();
    }


    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "win"){
            //if(Convert.ToInt32(collision.gameObject.name) > level && level < 4){
            //    level++;
            //    PlayerPrefs.SetInt("Level", level);
           // }
            PanelWin.SetActive(true);
            ButtonNext.SetActive(LoginPanel.loggedPlayerProgress.level <11 && currLevel <10);
            Time.timeScale = 0;
        }

        Pickup pickup = collision.GetComponent<Pickup>();
        if (pickup != null)
        {
            GetPickup(pickup);
        }

        ShowBoostPanelIfNeeded();
        UpdatePanelBoostValues();
    }
    public void BoostHealth(){
        if(countHealth > 0){
            countHealth--;
            AudioBox.instance.AudioPlay(AudioName.Bonus);
            GameObject.Find("HealthBoostText").GetComponent<TextMeshProUGUI>().text = countHealth.ToString();
        
            health += 3;
            if(health > 10) health = 10;
            GameObject.Find("HealthBar").GetComponent<FillHealthBar>().CurrentValue = health*0.1f;
            GameObject.Find("Fill").GetComponent<Animation>().Play();
        }
    }
    public void BoostJump(){
        if(countJump > 0 && activeBoost == false){
            activeBoost = true;
            countJump--;
            AudioBox.instance.AudioPlay(AudioName.Bonus);
            GameObject.Find("JumpBoostText").GetComponent<TextMeshProUGUI>().text = countJump.ToString();
            GameObject.Find("JumpBoost").GetComponent<Animation>().Play();
        
            GameObject.Find("Player").GetComponent<PlayerController>().jumpForce *= 1.5f;
            Invoke("OffJump", 5);
        }
    }
    public void BoostSpeed(){
        if(countSpeed > 0 && activeBoost == false){
            activeBoost = true;
            countSpeed--;
            AudioBox.instance.AudioPlay(AudioName.Bonus);
            GameObject.Find("SpeedBoostText").GetComponent<TextMeshProUGUI>().text = countSpeed.ToString();
            GameObject.Find("SpeedBoost").GetComponent<Animation>().Play();
        
            GameObject.Find("Player").GetComponent<PlayerController>().speed *= 1.5f;
            Invoke("OffSpeed", 5);
        }
    }
    public void BoostProtect(){
        if(countProtect > 0 && activeBoost == false){
            activeBoost = true;
            countProtect--;
            AudioBox.instance.AudioPlay(AudioName.Bonus);
            GameObject.Find("ProtectBoostText").GetComponent<TextMeshProUGUI>().text = countProtect.ToString();
            GameObject.Find("ProtectBoost").GetComponent<Animation>().Play();

            protect = true;
            Invoke("OffProtect", 5);
        }
    }
    void OffJump(){
        activeBoost = false;
        GameObject.Find("Player").GetComponent<PlayerController>().jumpForce /= 1.5f;
    }
    void OffSpeed(){
        activeBoost = false;
        GameObject.Find("Player").GetComponent<PlayerController>().speed /= 1.5f;
    }
    void OffProtect(){
        activeBoost = false;
        protect = false;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Enemy" && protect == false){
            if(collision.gameObject.GetComponent<Angel>() == true){
                AudioBox.instance.AudioPlay(AudioName.AttackAngel);
                health -=3;
            }
            else if(collision.gameObject.GetComponent<Fireball>() == true)
                health -=0.5f;
            else if(collision.gameObject.GetComponent<Wizard>() == true)
                health -=2;
            else if(collision.gameObject.GetComponent<Ghoul>() == true){
                AudioBox.instance.AudioPlay(AudioName.AttackGhoul);
                health --;
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
                coll = collision.collider;
            }
            
            GameObject.Find("HealthBar").GetComponent<FillHealthBar>().CurrentValue = health*0.1f;
            GameObject.Find("Fill").GetComponent<Animation>().Play();
            GameObject.Find("Blood").GetComponent<Animation>().Play();

            rb.constraints = RigidbodyConstraints2D.FreezePositionX|RigidbodyConstraints2D.FreezeRotation;
            AudioBox.instance.AudioPlay(AudioName.Hurt);
            anim.SetInteger("hurt", 0);
            
            Invoke("AnimHurtOff", 0.25f);
            Invoke("HurtOff", 0.5f);

            if(health <= 0){
                PanelGameOver.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else if(collision.gameObject.GetComponent<Ghoul>() == true){
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
                coll = collision.collider;
                Invoke("HurtOff", 0.5f);
        }
        else if(collision.gameObject.tag == "lava"){
            Debug.Log("lava");
                health -=10;
                GameObject.Find("HealthBar").GetComponent<FillHealthBar>().CurrentValue = health*0.1f;
                GameObject.Find("Fill").GetComponent<Animation>().Play();
                GameObject.Find("Blood").GetComponent<Animation>().Play();
                 
                rb.constraints = RigidbodyConstraints2D.FreezePositionX|RigidbodyConstraints2D.FreezeRotation;
                AudioBox.instance.AudioPlay(AudioName.Hurt);
                anim.SetInteger("hurt", 0);

                Invoke("HurtOff", 0.5f);

                 if(health <= 0){
                    PanelGameOver.SetActive(true);
                    Time.timeScale = 0;
                 }
        }

    }
    void FixedUpdate(){
        GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>().text = coin.ToString();
    }
    void Update(){
        GameObject.Find("EnergyBar").GetComponent<FillEnergyBar>().CurrentValue = energy/50f;
        GameObject.Find("HealthBar").GetComponent<FillHealthBar>().CurrentValue = health*0.1f;
        
        if(joystick.Horizontal < 0 || joystick.Horizontal > 0){
            //GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().walk);
            anim.SetBool("isWalking", true);
        } else {
            anim.SetBool("isWalking", false);
        }

        if(joystick.Vertical > 0.3 && GetComponent<PlayerController>().isGrounded == true){
            if(!(joystick.Vertical <= 0)){
                HitOff();
                AudioBox.instance.AudioPlay(AudioName.Jump);
                anim.SetInteger("Jump", 0);
            }
            Invoke("AnimJumpOff", 0.15f);
        }

        if((joystick.Vertical < -0.3) && GetComponent<PlayerController>().isGrounded == true){
            Crouch();
        }
    }

    void AnimHurtOff(){
        anim.SetInteger("hurt", -1);
    }
    void HurtOff(){
        if (coll) Physics2D.IgnoreCollision(coll, GetComponent<Collider2D>(), false);
        rb.constraints = RigidbodyConstraints2D.None|RigidbodyConstraints2D.FreezeRotation;
    }

    public void ButtonHit(){
        if(GetComponent<PlayerController>().isGrounded == false){
            FlyingKick();
        }
        if(GetComponent<PlayerController>().isGrounded == true){
            Hit();
        }
    }

    void FlyingKick(){
        AudioBox.instance.AudioPlay(AudioName.FlyingKick);
        anim.SetInteger("Jump", -1);
        anim.SetInteger("hit", 3);

        DamageFlip(0.25f);

        Invoke("AnimJumpOff", 0.5f);
        Invoke("AnimHitOff", 0.1f);
    }
    void AnimJumpOff(){
        anim.SetInteger("Jump", -1);
    }

    void Hit(){
        AudioBox.instance.AudioPlay(AudioName.Kick);
        hit = UnityEngine.Random.Range(0, 2);
        anim.SetInteger("hit", hit);

        DamageFlip(0.3f);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX|RigidbodyConstraints2D.FreezePositionY|RigidbodyConstraints2D.FreezeRotation;

        Invoke("AnimHitOff", 0.1f);
        Invoke("HitOff", 0.6f);
    }
    void AnimHitOff(){
        anim.SetInteger("hit", -1);
    }
    void HitOff(){
        rb.constraints = RigidbodyConstraints2D.None|RigidbodyConstraints2D.FreezeRotation;
    }

    void Crouch(){
        AudioBox.instance.AudioPlay(AudioName.Crouch);
        anim.SetTrigger("crouch");

        GetComponent<CapsuleCollider2D>().offset = new Vector2(-0.0001967549f, -0.1565886f);
        GetComponent<CapsuleCollider2D>().size = new Vector2(0.1009637f, 0.2951798f);  
        rb.constraints = RigidbodyConstraints2D.FreezePositionX|RigidbodyConstraints2D.FreezePositionY|RigidbodyConstraints2D.FreezeRotation;

        Invoke("CrouchOff", 0.5f);
    }
    void CrouchOff(){
        GetComponent<CapsuleCollider2D>().offset = new Vector2(-0.005184233f, -0.07877457f);
        GetComponent<CapsuleCollider2D>().size = new Vector2(0.2300652f, 0.4508078f);
        rb.constraints = RigidbodyConstraints2D.None|RigidbodyConstraints2D.FreezeRotation;
    }

    void DamageFlip(float distance){
        if(transform.localScale.x < 0)
            Instantiate(damage, new Vector2(transform.position.x - distance, transform.position.y), Quaternion.identity);
        else if(transform.localScale.x > 0)
            Instantiate(damage, new Vector2(transform.position.x + distance, transform.position.y), Quaternion.identity);
    }

    public void EnergyBonus(){
        if(energy >= 50)
        {
            energy = 0;
            energyBonus = true;
            GetComponent<PlayerController>().speed += 0.1f;
            Invoke("EnergyBonusOff", 5);
        }
    }

    void EnergyBonusOff(){
        energyBonus = false;
        GetComponent<PlayerController>().speed -= 0.1f;
    }

    private void GetPickup(Pickup pickup)
    {
        switch (pickup.type)
        {
            case PickupType.Health:
                countHealth += pickup.amount; 
                break;
            case PickupType.Jump: 
                countJump += pickup.amount; 
                break;
            case PickupType.Protect:
                countProtect += pickup.amount;
                break;
            case PickupType.Speed:
                countSpeed += pickup.amount;
                break;
            case PickupType.Money:
                coin += pickup.amount;
                GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>().text = coin.ToString();
                GameObject.Find("CoinText").GetComponent<Animation>().Play();
                PlayerPrefs.SetInt("Coin", coin);
                break;
        }
        pickup.GetPickup();
    }
}
