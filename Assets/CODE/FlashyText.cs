using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to an object with Unity UI Text component to create a flashing color cycle text effect. 
/// Set color saturation for the text to >0 to see the effect. The more saturation, the more pronounced the effect.
/// Set initial hue of text color in inspector to phase shift.
/// </summary>

[RequireComponent(typeof(UnityEngine.UI.Text))]
public class FlashyText : MonoBehaviour
{
    [Range(0.1f, 3f)]
    public float cycleTime = 1f;

    private float time;
    private float h, s, v;
    private UnityEngine.UI.Text text;

    void Start ()
    {
        // get the text component reference (that must exist)
        text = GetComponent<UnityEngine.UI.Text>();

        if (text != null) 
        {
            Color.RGBToHSV(text.color, out h, out s, out v);
            // initially, set the cycle timer to current color phase (to allow for phase shift by editing color in inspector)
            time = cycleTime * h;
        }
    }
	
	void Update ()
    {
        if (text != null)
        {
            Color.RGBToHSV(text.color, out h, out s, out v);

            // run a simple timer
            time -= Time.deltaTime;
            if (time <= 0f)
                time = cycleTime;

            // cycle color saturation over time for that cool retro rainbow thing
            h = Mathf.Lerp(0f, 1f, time / cycleTime);
            text.color = Color.HSVToRGB(h, s, v);
        }
	}
}
