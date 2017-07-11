using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to a particle system to hack out a rainbow effect for the particles while they're flying around
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class FlashyParticles : MonoBehaviour
{
    private ParticleSystem ps;

    public float cycleTime = 1f;

    private float time;
    private float h, s, v;

    void Start ()
    {
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mod = ps.main;
        ParticleSystem.ColorOverLifetimeModule col = ps.colorOverLifetime;
        col.enabled = true;

        // start color cycle from 0 (red)
        h = 0f;
        s = 1f;
        v = 1f;
    }
	
	void Update ()
    {
        // run a simple timer
        time -= Time.deltaTime;
        if (time <= 0f)
            time = cycleTime;

        // rotate hue with timer
        h = Mathf.Lerp(0f, 1f, time / cycleTime);

        // create color gradient where start color is the desired color and the end color has 0 alpha (so particles fade out nicely toward end of lifetime)
        Color startColor = Color.HSVToRGB(h, s, v);
        Color endColor = Color.HSVToRGB(h, s, v);
        startColor.a = 0.5f; // make start color have alpha as well
        endColor.a = 0f;

        // set up the gradient manually
        Gradient g = new Gradient();
        GradientColorKey[] gck = new GradientColorKey[2];
        GradientAlphaKey[] gak = new GradientAlphaKey[2];
        gck[0].color = startColor;
        gck[0].time = 0.0F;
        gck[1].color = endColor;
        gck[1].time = 1.0F;
        gak[0].alpha = startColor.a;
        gak[0].time = 0.0F;
        gak[1].alpha = endColor.a;
        gak[1].time = 1.0F;
        g.SetKeys(gck, gak);
        g.mode = GradientMode.Blend;

        // finally, assign the gradient to the emitter's color over lifetime, to affect all the particles the same way
        ParticleSystem.MainModule mod = ps.main;
        ParticleSystem.ColorOverLifetimeModule col = ps.colorOverLifetime;
        col.color = new ParticleSystem.MinMaxGradient(g);
    }
}
