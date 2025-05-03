using UnityEngine;

public class FireController : MonoBehaviour
{
    private ParticleSystem firePS;
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
        }
        else if (!ElementManager.fireOn && firePS.isPlaying)
        {
            firePS.Stop();

        }
    }
}
