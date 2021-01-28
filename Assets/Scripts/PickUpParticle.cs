using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PickUpParticle : MonoBehaviour
{
    private ParticleSystem particle;
    private float lifetime;
    private float elapsedTime = 0f;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        lifetime = particle.main.startLifetime.constant;
    }
    private void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
