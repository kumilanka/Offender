using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to an object with Unity UI Image component to create a flashing color cycle effect. 
/// Set color saturation for the text to >0 to see the effect. The more saturation, the more pronounced the effect.
/// Set initial hue of color in inspector to phase shift.
/// </summary>

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class FlashyImage : MonoBehaviour
{
    [Range(0.1f, 3f)]
    public float cycleTime = 1f;

    private float time;
    private float h, s, v;
    private UnityEngine.UI.Image image;

    void Start ()
    {
        // get the text component reference (that must exist)
        image = GetComponent<UnityEngine.UI.Image>();

        if (image != null) 
        {
            Color.RGBToHSV(image.color, out h, out s, out v);
            // initially, set the cycle timer to current color phase (to allow for phase shift by editing color in inspector)
            time = cycleTime * h;
        }
    }
	
	void Update ()
    {
        if (image != null)
        {
            Color.RGBToHSV(image.color, out h, out s, out v);

            // run a simple timer
            time -= Time.deltaTime;
            if (time <= 0f)
                time = cycleTime;

            // cycle color saturation over time for that cool retro rainbow thing
            h = Mathf.Lerp(0f, 1f, time / cycleTime);
            image.color = Color.HSVToRGB(h, s, v);
        }
	}
}
