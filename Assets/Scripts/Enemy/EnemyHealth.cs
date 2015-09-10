using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100; //Works same as player health
    public int currentHealth;
    public float sinkSpeed = 2.5f; //When enemies die they will sink through the floor at this speed
    public int scoreValue = 10; //Players are worth points for every kill
    public AudioClip deathClip; //Audioclip for when enemy dies


    Animator anim; //Reference to animator
    AudioSource enemyAudio; //Reference to audiosource
    ParticleSystem hitParticles; //Reference for the hit particles
    CapsuleCollider capsuleCollider; //Reference to the capsule collider
    bool isDead; //Works same as player
    bool isSinking;


    void Awake () //Sets up references
    {
        anim = GetComponent <Animator> (); //Gets components for animator and audiosource
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> (); //Returns the first particle system to all children
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)
        { //Check if the enemy is supposed to be sinking, make them go in a negative-up direction (down) per second
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint) //When the enemy takes damage, hitpoint is used for where the particle effect goes
    {
        if(isDead)
            return; //If the enemy is dead don't do anything here

        enemyAudio.Play (); //Play the hurt audio

        currentHealth -= amount; //Remove health
            
        hitParticles.transform.position = hitPoint; //Make the particles appear at the hitpoint where the enemy was hit
        hitParticles.Play(); //Play the particle system so the fluff appears

        if(currentHealth <= 0)
        { //If the enemy runs out of health, it dies
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true; //When enemies are dead they are no longer obstacles

        anim.SetTrigger ("Dead"); //Perform the dead animation

        enemyAudio.clip = deathClip; //Prepare the deathclip
        enemyAudio.Play (); //Make the deathclip play
    }


    public void StartSinking ()
    {
        GetComponent <NavMeshAgent> ().enabled = false; //Find references to the navmeshagent and disable it, use .enabled since it's a component instead of the entire gameobject
        GetComponent <Rigidbody> ().isKinematic = true; //Set it to kinematic so unity ignores it and doesn't reset it up
        isSinking = true; //Start to sink
        ScoreManager.score += scoreValue; //When the monster sinks the player gets a point
        Destroy (gameObject, 2f); //Destroy the gameobject after 2 seconds to save space
    }
}
