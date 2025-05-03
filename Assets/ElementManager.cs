using UnityEngine;
using System.Collections;
public class ElementManager : MonoBehaviour
{
    // Particle systems for each element
    [SerializeField] private ParticleSystem waterParticleSystem;
    [SerializeField] private ParticleSystem fireParticleSystem;
    [SerializeField] private ParticleSystem windParticleSystem;

    private ParticleSystem.EmissionModule fireEmission;
    private ParticleSystem.ShapeModule waterShape;

    private float lastWaterTime = 0f; // Tracks the last time Water() was called
    private float lastFireTime = 0f; // Tracks the last time Fire() was called
    private float lastWindTime = 0f; // Tracks the last time Wind() was called
    private bool isWindStrong = false; // Tracks if wind is in the strong state
    private bool isWindCooldownActive = false; // Tracks if the cooldown is active
    private bool isMediumWindActive = false; // Tracks if medium wind is active

    public static bool fireOn = false;
    public static bool rainOn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Optional: Ensure particle systems are stopped at the start
        if (waterParticleSystem != null)
        {
            waterParticleSystem.Stop();
            waterShape = waterParticleSystem.shape;
        }
        if (fireParticleSystem != null)
        {
            fireParticleSystem.Stop();
            fireEmission = fireParticleSystem.emission;
        }
        if (windParticleSystem != null) windParticleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for key presses to trigger element functions
        if (Input.GetKeyDown(KeyCode.W))
        {
            Wind();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Fire();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Water();
        }

        // Stop water if inactive for 5 seconds
        if (Time.time - lastWaterTime > 5f && waterParticleSystem != null && waterParticleSystem.isPlaying)
        {
            waterParticleSystem.Stop();
            rainOn=false;
        }

        // Stop fire if inactive for 5 seconds
        if (Time.time - lastFireTime > 5f && fireParticleSystem != null && fireParticleSystem.isPlaying)
        {
            fireOn = false;
        }

        // Handle wind state transitions and stop wind if inactive for 5 seconds
        if (Time.time - lastWindTime > 5f && isMediumWindActive)
        {
            StopWind();
        }
    }

    public void Water()
    {
        lastWaterTime = Time.time;

        if (waterParticleSystem != null)
        {
            waterParticleSystem.Play();
        }

        fireOn = false;
        rainOn = true;
    }

    public void Fire()
    {
        lastFireTime = Time.time;

        fireOn = true;
    }

    public void Wind()
    {
        float currentTime = Time.time;

    
        
        
        
            // Enable medium wind and start cooldown
            lastWindTime = currentTime;
           

            EnableMediumWind();
        
        
    }

    private void EnableMediumWind()
    {
       
        isMediumWindActive = true; // Medium wind is now active
        TreeWind[] treeWinds = FindObjectsByType<TreeWind>(FindObjectsSortMode.None);
        foreach (var treeWind in treeWinds)
        {
            treeWind.SetWindState(TreeWind.WindState.MediumWind);
        }
    }

  

    

    private void StopWind()
    {
        isWindStrong = false;
        isMediumWindActive = false;

        TreeWind[] treeWinds = FindObjectsByType<TreeWind>(FindObjectsSortMode.None);
        foreach (var treeWind in treeWinds)
        {
            treeWind.SetWindState(TreeWind.WindState.NoWind);
        }

        if (windParticleSystem != null)
        {
            windParticleSystem.Stop();
        }
    }
}
