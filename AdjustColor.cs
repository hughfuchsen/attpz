// using UnityEngine;

// public class AdjustSpriteRendererColor : MonoBehaviour
// {
//     void AdjustColor()
//     {
//         // Find all game objects with SpriteRenderer components
//         SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();

//         // Iterate through each sprite renderer
//         foreach (SpriteRenderer renderer in spriteRenderers)
//         {
//             // Get the current color of the sprite renderer
//             Color color = renderer.color;

//             // Convert the color to HSV
//             Color.RGBToHSV(color, out float h, out float s, out float v);

//             // Adjust the value (brightness) by -60
//             v -= 0.6f;
//             if (v < 0f) v = 0f; // Ensure the value stays within valid range [0,1]

//             // Convert the HSV color back to RGB
//             color = Color.HSVToRGB(h, s, v);

//             // Apply the new color to the sprite renderer
//             renderer.color = color;
//         }
//     }

//     void Awake()
//     {
//         // Call the AdjustColor function when the script starts
//         AdjustColor();
//     }
// }


using UnityEngine;

public class AdjustColor : MonoBehaviour
{
    public float cycleDuration = 2f; // Duration of one full cycle in seconds

    [SerializeField] public Material spriteMaterial;
    private float elapsedTime;

    void Update()
    {
        if (spriteMaterial != null)
        {
            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the oscillation factor using a sine wave to oscillate between 0 and 1
            float oscillationFactor = (Mathf.Sin((elapsedTime / cycleDuration) * Mathf.PI * 2) + 1) / 2;

            // Get the current color of the material
            Color originalColor = spriteMaterial.color;

            // Convert the original color to HSV
            Color.RGBToHSV(originalColor, out float h, out float s, out float v);

            // Adjust the v (value/brightness) to oscillate between original v and a darker v (e.g., 50% of original v)
            float minValue = v * 0.5f; // Change this value to control how dark it gets
            float newV = Mathf.Lerp(minValue, v, oscillationFactor);

            // Ensure the v value is clamped between 0 and 1
            newV = Mathf.Clamp(newV, 0f, 1f);

            // Convert the HSV color back to RGB
            Color adjustedColor = Color.HSVToRGB(h, s, newV);

            // Ensure the alpha value remains the same
            adjustedColor.a = originalColor.a;

            // Apply the adjusted color to the material
            spriteMaterial.color = adjustedColor;
        }
    }
}
