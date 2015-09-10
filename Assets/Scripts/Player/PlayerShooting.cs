using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20; //Amount of damage every shot does
    public float timeBetweenBullets = 0.15f; //Time limit between how quickly the gun can fire
    public float range = 100f; //How far the bullets may travel


    float timer; //Keeps everything in sync, can only attack when time is right
    Ray shootRay; //When we fire we use raycasts to figure out what we hit with bullets
    RaycastHit shootHit; //Returns what we hit
    int shootableMask; //Makes it so we can only shoot shootable things
    ParticleSystem gunParticles; //Reference to the gun particles
    LineRenderer gunLine; //References the line for the gun
    AudioSource gunAudio; //References gun audio
    Light gunLight; //References the light for the gun
    float effectsDisplayTime = 0.2f; //The amount of time our effects are displayed for


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable"); //Can shoot anything that is shootable
        gunParticles = GetComponent<ParticleSystem> (); //Setup component references
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update () //Control whether it is time to shoot or not
    {
        timer += Time.deltaTime; //Increase in size as time progresses

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0) //Fire1 by default is the left click on the mouse, left ctrl on keyboard
        { //If the player fires, and the time limit b etween bullets is over, shoot
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        { //If we fired and the displayed effects time is over, stop them
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    { //Stop the effects
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f; //We're firing now so reset the amount of time between firing

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop (); //If the particles are still playing stop them then allow them to be used again
        gunParticles.Play ();

        gunLine.enabled = true; //Turns on the line renderer
        gunLine.SetPosition (0, transform.position); //Place the line at the start of the gunbarrel, transform.position is on the barrel of the gun

        shootRay.origin = transform.position; //Start the ray at the beginning of the gun
        shootRay.direction = transform.forward; //Make it point forward from the gunbarrel in the Z axis

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask)) //Shoot a ray and hit something in our range, giving shootHit as what we hit
        {																	//can only hit things in shootableMask
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> (); //Get the enemy health script so if we hit an enemy we can reduce its health	
            if(enemyHealth != null) //If the enemy health script actually exists for the hit object (if the object IS an enemy)
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point); //Make the enemy take damage and place the effects exactly where it was hit
            }
            gunLine.SetPosition (1, shootHit.point); //The point where we hit to draw our line is drawn towards the point that we hit on the target
        }
        else
        { //If we don't hit anything with our line, set the line to go to the end of the range in the direction of the line
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
