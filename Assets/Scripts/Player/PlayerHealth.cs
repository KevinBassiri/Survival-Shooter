using UnityEngine;
using UnityEngine.UI; //Needed to access the slider UI element
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100; //How much health the player has when the level starts
    public int currentHealth; //How much health the player has after a condition
    public Slider healthSlider; //Reference to the slider UI element
    public Image damageImage; //Reference to the damageImage we made
    public AudioClip deathClip; //Sound made when player loses
    public float flashSpeed = 5f; //Speed at which the damage flash is displayed
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f); //Completely red, 10th of the way opaque


    Animator anim; //Reference to animator components
    AudioSource playerAudio; //Reference to audiosource
    PlayerMovement playerMovement; //Reference to the playermovement SCRIPT, can be used to stop player movement
    PlayerShooting playerShooting; //Reference to the playerShooting script
    bool isDead;
    bool damaged;


    void Awake ()
    {
        anim = GetComponent <Animator> (); //Gets a reference to the animator
        playerAudio = GetComponent <AudioSource> (); //Gets a reference to the audiosource
        playerMovement = GetComponent <PlayerMovement> (); //Gets a reference to the playermovement script
        playerShooting = GetComponentInChildren <PlayerShooting> (); //Get the components of the shooting script since it fires on the end of the gunbarrel
        currentHealth = startingHealth; //Sets current health to starter health
    }


    void Update () //Concerns whether or not the player is damaged + flashing or not
    {
        if(damaged)
        { //If we're damaged change the screen to flash red
            damageImage.color = flashColour;
        }
        else
        { //Otherwise transition the color back to clear
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        } 
        damaged = false; //The moment we take damage, set damage back to false
    }


    public void TakeDamage (int amount) //Can be called by other functions for it to takedamage
    {
        damaged = true; //Make the screen flash

        currentHealth -= amount; //Reduce current health by currenthealth - amount

        healthSlider.value = currentHealth; //Make the slider shrink as player takes more damage

        playerAudio.Play (); //Use the player hurt audio

        if(currentHealth <= 0 && !isDead) //If the player is dead then go to Death()
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        playerShooting.DisableEffects (); //Player cannot create particle effects while dead

        anim.SetTrigger ("Die"); //Play death animation

        playerAudio.clip = deathClip; //Play death sound
        playerAudio.Play ();

        playerMovement.enabled = false; //Player can no longer move
        playerShooting.enabled = false; //Player cannot shoot gun anymore
    }


    public void RestartLevel ()
    {
        Application.LoadLevel (Application.loadedLevel);
    }
}
