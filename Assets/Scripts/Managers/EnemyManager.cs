using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f; //Time it takes to spawn an enemy
    public Transform[] spawnPoints;


    void Start ()
    {
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
        if(playerHealth.currentHealth <= 0f)
        {
            return; //If the player has no health stop spawning
        }

        int spawnPointIndex = Random.Range (0, spawnPoints.Length); //If the player has health continue spawning at any spawn points

        Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation); //Spawn a kind of enemy at a specific position with a specific rotation
    }
}
