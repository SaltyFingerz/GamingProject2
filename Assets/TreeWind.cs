using UnityEngine;

public class TreeWind : MonoBehaviour
{
    public enum WindState
    {
        NoWind,
        MediumWind,
        StrongWind
    }

    [SerializeField] private WindState currentWindState = WindState.NoWind; // Public variable to set wind state
    [SerializeField] private float mediumWindAngle = 5f; // Maximum angle for medium wind
    [SerializeField] private float strongWindAngle = 15f; // Maximum angle for strong wind
    [SerializeField] private float mediumWindSpeed = 2f; // Oscillation speed for medium wind
    [SerializeField] private float strongWindSpeed = 4f; // Oscillation speed for strong wind

    private float currentAngleRange = 0f; // Current angle range based on wind state
    private float currentOscillationSpeed = 0f; // Current oscillation speed based on wind state

    // Update is called once per frame
    void Update()
    {
        UpdateWindState();
        OscillateTree();
    }

    private void UpdateWindState()
    {
        // Adjust the angle range and oscillation speed based on the current wind state
        switch (currentWindState)
        {
            case WindState.NoWind:
                currentAngleRange = 0f;
                currentOscillationSpeed = 0f;
                break;
            case WindState.MediumWind:
                currentAngleRange = mediumWindAngle;
                currentOscillationSpeed = mediumWindSpeed;
                break;
            case WindState.StrongWind:
                currentAngleRange = strongWindAngle;
                currentOscillationSpeed = strongWindSpeed;
                break;
        }
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

    // Public method to set the wind state (optional, can also be set via the Inspector)
    public void SetWindState(WindState newWindState)
    {
        currentWindState = newWindState;
    }
}
