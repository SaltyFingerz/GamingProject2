using UnityEngine;
using System.Collections;


public class TreeWind : MonoBehaviour
{
    private Animator aC;
    public enum WindState
    {
        NoWind,
        MediumWind,
        StrongWind
    }

    [SerializeField] private WindState currentWindState = WindState.NoWind; // Private field with SerializeField for Inspector access

    public WindState CurrentWindState // Public property to access the wind state
    {
        get => currentWindState;
        set
        {
            if (currentWindState != value)
            {
                currentWindState = value;
                StartCoroutine(TransitionWindState());
            }
        }
    }

    [SerializeField] private float mediumWindAngle = 5f; // Maximum angle for medium wind
  //  [SerializeField] private float strongWindAngle = 8f; // Maximum angle for strong wind
    [SerializeField] private float mediumWindSpeed = 1f; // Oscillation speed for medium wind
   // [SerializeField] private float strongWindSpeed = 1.5f; // Oscillation speed for strong wind

    private float currentAngleRange = 0f; // Current angle range based on wind state
    private float currentOscillationSpeed = 0f; // Current oscillation speed based on wind state

    private float previousAngleRange = 0f;
    private float previousOscillationSpeed = 0f;

    private float targetAngleRange = 0f;
    private float targetOscillationSpeed = 0f;

    private Coroutine windTransitionCoroutine; // Tracks the current wind transition coroutine

    private void Start()
    {
       aC=gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        OscillateTree();
        AnimateTree();
    }

    private void AnimateTree()
    {
        if (aC != null) 
        { 
        aC.SetBool("Fire", ElementManager.fireOn);
        aC.SetBool("Rain", ElementManager.rainOn);
        }

    }

    private IEnumerator TransitionWindState()
    {
        // Set target values based on the current wind state
        switch (currentWindState)
        {
            case WindState.NoWind:
                targetAngleRange = 0f;
               // targetOscillationSpeed = 0f;
                break;
            case WindState.MediumWind:
                targetAngleRange = mediumWindAngle;
                targetOscillationSpeed = mediumWindSpeed;
                break;
          /*  case WindState.StrongWind:
                targetAngleRange = strongWindAngle;
                targetOscillationSpeed = strongWindSpeed;
                break;*/
        }

        float duration = 2f; // Transition duration in seconds
        float elapsed = 0f;

        // Gradually interpolate the values over the duration
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentAngleRange = Mathf.Lerp(previousAngleRange, targetAngleRange, elapsed / duration);
            currentOscillationSpeed = Mathf.Lerp(previousOscillationSpeed, targetOscillationSpeed, elapsed / duration);
            yield return null;
        }
      

        // Ensure final values are set
        currentAngleRange = targetAngleRange;
        currentOscillationSpeed = targetOscillationSpeed;

        // Update previous values for the next transition
        previousAngleRange = currentAngleRange;
        previousOscillationSpeed = currentOscillationSpeed;
        yield return null;
    }

    private void OscillateTree()
    {
        if (currentAngleRange > 0f)
        {
            // Calculate oscillation angles using a sine wave
            float xRotation = Mathf.Sin(Time.time * currentOscillationSpeed) * currentAngleRange;
            float zRotation = Mathf.Cos(Time.time * currentOscillationSpeed) * currentAngleRange;

            // Apply the rotation to the tree
            transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation);
        }
        else
        {
            // Reset rotation to default when there is no wind
            transform.localRotation = Quaternion.identity;
        }
    }

    // Public method to set the wind state
    public void SetWindState(WindState newWindState)
    {
        previousAngleRange = currentAngleRange;
        previousOscillationSpeed = currentOscillationSpeed;
        CurrentWindState = newWindState;
    }
}
