using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    private Light directionalLight; // Reference to the directional light component
    private Coroutine intensityCoroutine; // Tracks the current intensity, temperature, and skybox exposure transition coroutine

    [SerializeField] private float defaultIntensity = 2f; // Default light intensity
    [SerializeField] private float fireIntensity = 16f; // Intensity when fireOn is true
    [SerializeField] private float rainIntensity = 0f; // Intensity when rainOn is true
    [SerializeField] private float defaultTemperature = 7590f; // Default light temperature (Kelvin)
    [SerializeField] private float fireTemperature = 1500f; // Temperature when fireOn is true (Kelvin)
    [SerializeField] private float rainTemperature = 20000f; // Temperature when rainOn is true (Kelvin)
    [SerializeField] private float defaultExposure = 1f; // Default skybox exposure
    [SerializeField] private float fireExposure = 2f; // Skybox exposure when fireOn is true
    [SerializeField] private float rainExposure = 0.5f; // Skybox exposure when rainOn is true
    [SerializeField] private float transitionDuration = 3f; // Duration for intensity, temperature, and exposure transitions

    private Material skyboxMaterial; // Reference to the skybox material

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Light component attached to the GameObject
        directionalLight = GetComponent<Light>();
        if (directionalLight == null)
        {
            Debug.LogError("No Light component found on this GameObject.");
        }

        // Get the skybox material
        skyboxMaterial = RenderSettings.skybox;
        if (skyboxMaterial == null)
        {
            Debug.LogError("No Skybox material found in RenderSettings.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ElementManager.fireOn)
        {
            // Gradually increase intensity, adjust temperature, and set skybox exposure for fire
            StartTransition(fireIntensity, fireTemperature, fireExposure);
        }
        else if (ElementManager.rainOn)
        {
            // Gradually decrease intensity, adjust temperature, and set skybox exposure for rain
            StartTransition(rainIntensity, rainTemperature, rainExposure);
        }
        else
        {
            // Gradually return to default intensity, temperature, and skybox exposure
            StartTransition(defaultIntensity, defaultTemperature, defaultExposure);
        }
    }

    private void StartTransition(float targetIntensity, float targetTemperature, float targetExposure)
    {
        // Stop any ongoing intensity, temperature, and exposure transition coroutine
        if (intensityCoroutine != null)
        {
            StopCoroutine(intensityCoroutine);
        }

        // Start a new coroutine to transition the light intensity, temperature, and skybox exposure
        intensityCoroutine = StartCoroutine(TransitionLightAndSkybox(targetIntensity, targetTemperature, targetExposure));
    }

    private IEnumerator TransitionLightAndSkybox(float targetIntensity, float targetTemperature, float targetExposure)
    {
        float initialIntensity = directionalLight.intensity; // Current intensity of the light
        float initialTemperature = directionalLight.colorTemperature; // Current temperature of the light
        float initialExposure = skyboxMaterial.GetFloat("_Exposure"); // Current skybox exposure
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;

            // Lerp the intensity, temperature, and exposure from their initial values to the target values
            directionalLight.intensity = Mathf.Lerp(initialIntensity, targetIntensity, t);
            directionalLight.colorTemperature = Mathf.Lerp(initialTemperature, targetTemperature, t);
            skyboxMaterial.SetFloat("_Exposure", Mathf.Lerp(initialExposure, targetExposure, t));

            yield return null;
        }

        // Ensure the final intensity, temperature, and exposure are set to the target values
        directionalLight.intensity = targetIntensity;
        directionalLight.colorTemperature = targetTemperature;
        skyboxMaterial.SetFloat("_Exposure", targetExposure);
    }
}
