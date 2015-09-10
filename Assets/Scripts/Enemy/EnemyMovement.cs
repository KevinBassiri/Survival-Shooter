using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player; //What the enemy moves towards
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav; //Component we put onto the enemy previously


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform; //Finds the  tag "Player" by looking through all the tags in a scene
        playerHealth = player.GetComponent <PlayerHealth> (); //Reference player health
        enemyHealth = GetComponent <EnemyHealth> (); //Reference enemy health
        nav = GetComponent <NavMeshAgent> (); //Pulls a reference to the component we have in editor
    }


    void Update () //Not keeping in time with physics
    {
        if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0) //Check if both the player and enemy are alive before we set destination
        {
		 nav.SetDestination (player.position); //Tells enemy where to go, towards the player
        }
        else
        {
            nav.enabled = false; //The enemy or player has died and is now unable to move
        }
    }
}
