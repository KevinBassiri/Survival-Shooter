using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f; //A limit between how many times an enemy can attack
    public int attackDamage = 10; //How much each attack does to the player


    Animator anim; //Reference to animator
    GameObject player; //Reference to player's gameobject so enemy can attack player
    PlayerHealth playerHealth; //Enemy has a reference to the player's health
    EnemyHealth enemyHealth; //Reference to the enemy's health in the enemy health script
    bool playerInRange; //If the enemy is close enough to attack they'll attack
    float timer; //Timer is used to keep everything in sync


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player"); //Locates the player for us, stores reference locally to save performance
        playerHealth = player.GetComponent <PlayerHealth> (); //Pulls playerhealth component off of player and stores it
        enemyHealth = GetComponent<EnemyHealth>(); //Pulls the enemyhealth component from enemyhealth
        anim = GetComponent <Animator> (); //Reference to animator component
    }


    void OnTriggerEnter (Collider other) //Called when anything goes in a trigger, other is whatever is collided
    {
        if(other.gameObject == player) //If it's actually the player prepare to attack it
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other) //Called when anything was in the trigger and is gone
    {
        if(other.gameObject == player)
        {
            playerInRange = false; //Player isn't close enough anymore
        }
    }


    void Update () //Where the attack method happens
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        { //If the time has been long enough between attacks AND player has more than 0 health, attack the player
            Attack ();
        }

        if(playerHealth.currentHealth <= 0)
        { //If the player has died transition to the idle state
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack ()
    {
        timer = 0f; //Reset the timer back to 0

        if(playerHealth.currentHealth > 0)
        { //If the player still has health remove it from them depending upon our damage
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
