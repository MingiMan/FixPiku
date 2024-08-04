using System.Collections;
using UnityEngine;

public class Particle : MonoBehaviour
{
    ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        StartCoroutine(PlayParticle());
    }

    IEnumerator PlayParticle()
    {
        particle.Play();
        yield return new WaitForSeconds(particle.main.duration);
        Destroy(gameObject);
    }
}
