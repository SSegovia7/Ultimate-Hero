using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float currentHealth = 100;
    [SerializeField] int maxHealth = 100;
    
    public HealthBar healthBar;
    public UnityEvent onDie = new UnityEvent();
    public UnityEvent onDamaged = new UnityEvent();

    public bool isLiving = true;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(20);
        }

        if (currentHealth <= 0 && isLiving == true){
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        onDamaged.Invoke();
        healthBar.SetHealth((int) currentHealth);
    }

    public void EnemyDamaged(float damage)
    {
        this.currentHealth -= damage;
        this.healthBar.SetHealth((int) currentHealth);
    }
    
    public void Die()
    {
        Revive();
        Invoke("Revive", 2.0f);
        onDie.Invoke();
    }

    public void Revive()
    {
        if (transform.tag == "Player"){
            Debug.Log($"{transform.name}: is Dead:{isLiving}");
            Transform player_sprite_holder = this.transform.Find("Main Character"); //sr in child gameObject
            SpriteRenderer player_renderer = player_sprite_holder.GetComponent<SpriteRenderer>(); //gets player sprite renderer
            player_renderer.enabled = !player_renderer.enabled; //flips player sprite renderer
        }
        else if (transform.tag == "Enemy"){
            Debug.Log($"{transform.name}: is Dead: {isLiving}");
            this.GetComponent<SpriteRenderer>().enabled = !this.GetComponent<SpriteRenderer>().enabled; //flips sr
        }
        if (currentHealth <= 0){ //resets health
            healthBar.SetHealth(maxHealth);
            currentHealth = maxHealth;
        }
        isLiving = !isLiving; //flips isLiving bool


    }
}
