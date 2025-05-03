using UnityEngine;
using System.Collections;

public class FireController : MonoBehaviour
{
    private ParticleSystem firePS;

    private Coroutine colorTransitionCoroutine; // Tracks the current color transition coroutine
    private Coroutine playFireCoroutine; // Tracks the coroutine for delayed fire start

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
            // Start the coroutine to delay firePS.Play() and StartColorTransition()
            if (playFireCoroutine == null)
            {
                playFireCoroutine = StartCoroutine(DelayedFireStart());
            }
        }
        else if (!ElementManager.fireOn && firePS.isPlaying)
        {
            firePS.Stop();
            if (playFireCoroutine != null)
            {
                StopCoroutine(playFireCoroutine);
                playFireCoroutine = null;
            }
        }
    }

    private IEnumerator DelayedFireStart()
    {
        // Generate a random delay between 0 and 5 seconds
        float randomDelay = Random.Range(0f, 3f);
        yield return new WaitForSeconds(randomDelay);

        // Play the particle system and start the color transition
        firePS.Play();
        StartColorTransition();

        // Clear the coroutine reference
        playFireCoroutine = null;
    }

    private void StartColorTransition()
    {
        // Stop any ongoing color transition coroutine
        if (colorTransitionCoroutine != null)
        {
            StopCoroutine(colorTransitionCoroutine);
        }

        // Start a new coroutine to transition the materials' colors to black (if implemented)
        // colorTransitionCoroutine = StartCoroutine(TransitionMaterialsToBlack());
    }
}
