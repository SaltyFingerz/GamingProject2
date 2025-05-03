using UnityEngine;

public class ElementManager : MonoBehaviour
{
    // Particle systems for each element
    [SerializeField] private ParticleSystem waterParticleSystem;
    [SerializeField] private ParticleSystem fireParticleSystem;
    [SerializeField] private ParticleSystem windParticleSystem;

    private ParticleSystem.EmissionModule fireEmission;
    private ParticleSystem.ShapeModule waterShape;

    private bool isWindActive = false; // Tracks if wind is active
    private float windOscillationSpeed = 2f; // Speed of oscillation
    private float windOscillationAngle = 15f; // Maximum angle of oscillation

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
        if (isWindActive && waterParticleSystem != null && waterParticleSystem.isPlaying)
        {
            OscillateWaterAngle();
        }
    }

    public void Water()
    {
        if (waterParticleSystem != null)
        {
            waterParticleSystem.Play();
        }

        // Gradually reduce fire emission rate
        if (fireParticleSystem != null)
        {
            StartCoroutine(GraduallyReduceFireEmission());
        }
    }

    public void Fire()
    {
        if (fireParticleSystem != null)
        {
            fireParticleSystem.Play();
        }
    }

    public void Wind()
    {
        if (windParticleSystem != null)
        {
            windParticleSystem.Play();
        }

        // Increase fire emission rate
        if (fireParticleSystem != null)
        {
            AmplifyFireEmission();
        }

        // Activate wind effect on water
        if (waterParticleSystem != null && waterParticleSystem.isPlaying)
        {
            isWindActive = true;
        }
    }

    private System.Collections.IEnumerator GraduallyReduceFireEmission()
    {
        float targetRate = 0f;
        float duration = 2f; // Time in seconds to fully stop emission
        float initialRate = fireEmission.rateOverTime.constant;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newRate = Mathf.Lerp(initialRate, targetRate, elapsed / duration);
            fireEmission.rateOverTime = newRate;
            yield return null;
        }

        fireEmission.rateOverTime = targetRate;
    }

    private void AmplifyFireEmission()
    {
        float amplificationFactor = 2f; // Increase emission rate by this factor
        float currentRate = fireEmission.rateOverTime.constant;
        fireEmission.rateOverTime = currentRate * amplificationFactor;
    }

    private void OscillateWaterAngle()
    {
        float angle = Mathf.Sin(Time.time * windOscillationSpeed) * windOscillationAngle;
        waterShape.rotation = new Vector3(angle, 0f, 0f);
    }
}
