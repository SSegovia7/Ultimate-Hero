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
    MainMenu mainMenu;

    public int PoseOnDeath = 20;

    GameObject player; //PLAYER OBJECT

    public bool isLiving = true;

    void Start()
    {
        mainMenu = new MainMenu();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        player = GameObject.Find("Player Character"); //Finding player GO using strintg "Player Character" if err check name

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(20);
        }

        if (currentHealth <= 0 && isLiving == true){
            if(this.gameObject.name == "Player Character")
            {
                mainMenu.GoToLoseScreen();
            }
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
        Revive(); //GAMEOBJECT DIES / TURNS INVISIBLE
        player.GetComponent<Pose>().IncreasePose(PoseOnDeath); //PLAYER POSE BAR IS INCREASE ON ENEMY DEATH
        Invoke("Revive", 2.0f); //GAMEOBJECT IS REVIVED / VISIBLE / VALUES RESET
        onDie.Invoke();
        
    }

    public void Revive() //CHECK GAME OBJECT TAG IN INSPECTOR ---NEEDS TO BE SET TO PLAYER OR ENEMEY
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
