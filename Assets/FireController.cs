using UnityEngine;
using System.Collections;

public class FireController : MonoBehaviour
{
    private ParticleSystem firePS;

  
    private Coroutine colorTransitionCoroutine; // Tracks the current color transition coroutine

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firePS = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ElementManager.fireOn && !firePS.isPlaying)
        {
            firePS.Play();
            StartColorTransition(); // Start transitioning materials to black
        }
        else if (!ElementManager.fireOn && firePS.isPlaying)
        {
            firePS.Stop();
        }
    }

    private void StartColorTransition()
    {
        // Stop any ongoing color transition coroutine
        if (colorTransitionCoroutine != null)
        {
            StopCoroutine(colorTransitionCoroutine);
        }

  
    }

   
}
