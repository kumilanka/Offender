using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is another copy of the same script for a different type of component because Unity doesn't allow for interfaces to existing types. IColorable would be awesome here, but no.
/// This one is used for the background twinkling stars.
/// </summary>

    [RequireComponent(typeof(SpriteRenderer))]
public class FlashySprite : MonoBehaviour
{
    [Range(0.1f, 3f)]
    public float colorCycleTime = 1f;
    public float colorCycleStart = 0f;
    private float currentColorCycleTime;
    private float h, s, v;
    private SpriteRenderer spriteRenderer;

    [Range(0.1f, 3f)]
    public float alphaCycleTime = 1f;

    [Range(0f, 1f)]
    public float alphaCycleStart = 0f; // 0-1
    private float currentAlphaCycleTime;

    void Start ()
    {
        // get the text component reference (that must exist)
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null) 
        {
            Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);
            // initially, set the cycle timer to current color phase (to allow for phase shift by editing color in inspector)
            currentColorCycleTime = colorCycleTime * colorCycleStart;
            currentAlphaCycleTime = alphaCycleTime * alphaCycleStart;
        }
    }
	
	void Update ()
    {
        if (spriteRenderer != null)
        {
            Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

            // run a simple timer
            currentColorCycleTime -= Time.deltaTime;
            if (currentColorCycleTime <= 0f)
                currentColorCycleTime = colorCycleTime;

            currentAlphaCycleTime -= Time.deltaTime;
            if (currentAlphaCycleTime <= 0f)
                currentAlphaCycleTime = alphaCycleTime;

            // cycle color saturation over time for that cool retro rainbow thing
            h = Mathf.Lerp(0f, 1f, currentColorCycleTime / colorCycleTime);
            Color c = Color.HSVToRGB(h, s, v);
            // alpha cycle is a sine wave between 0 and 1, so the stars kind of "pulse"
            float sineValue = 0.5f * (1f + Mathf.Sin(2 * Mathf.PI * 1f * currentAlphaCycleTime / alphaCycleTime));
            c.a = 0.1f + 0.5f * sineValue;
            spriteRenderer.color = c;
        }
	}
}
