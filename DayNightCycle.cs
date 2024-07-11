using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Material sharedMaterial; // Reference to the shared material used by all houses
    public float cycleDuration = 10f;
    public float horizontalSkewStart = -1.5f; // Starting value of horizontal skew
    public float horizontalSkewEnd = 1.5f; // Ending value of horizontal skew

    private float timer = 0f;
    private bool isDay = true;

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        float horizontalSkew;
        if (isDay)
        {
            horizontalSkew = Mathf.Lerp(horizontalSkewStart, horizontalSkewEnd, timer / cycleDuration);
            if (timer >= cycleDuration)
            {
                timer = 0f;
                isDay = false;
            }
        }
        else
        {
            horizontalSkew = Mathf.Lerp(horizontalSkewEnd, horizontalSkewStart, timer / cycleDuration);
            if (timer >= cycleDuration)
            {
                timer = 0f;
                isDay = true;
            }
        }

        // Get the scale of the object along the y-axis
        float verticalScale = transform.localScale.y;

        // Adjust the horizontal skew based on the vertical scale
        horizontalSkew *= verticalScale;

        // Set the horizontal skew for all houses using the shared material
        sharedMaterial.SetFloat("_HorizontalSkew", horizontalSkew);
    }
    void OnApplicationQuit()
    {
        sharedMaterial.SetFloat("_HorizontalSkew", 1f);
    }
}
