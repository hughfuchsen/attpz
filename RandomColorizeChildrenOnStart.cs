using System.Collections.Generic;
using UnityEngine;

public class RandomColorizeChildrenOnStart : MonoBehaviour
{
    private List<SpriteRenderer> childSpriteRenderers = new List<SpriteRenderer>();
    private IsoSpriteSorting isoSpriteSorting;

    private void Start()
    {
        isoSpriteSorting = GetComponent<IsoSpriteSorting>();
        // Populate the list with SpriteRenderers from all child objects
        foreach (Transform child in transform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                childSpriteRenderers.Add(spriteRenderer);
            }
        }

        // Apply a random color with fixed V to each child SpriteRenderer
        foreach (SpriteRenderer spriteRenderer in childSpriteRenderers)
        {
            SetRandomColor(spriteRenderer);
        }

        // Find and move the FlowerStem object (child)
        Transform flowerStem = transform.Find("FlowerStem");
        if (flowerStem != null)
        {
            // Randomly move the flowerStem in the negative X direction by 0, -1, -2, or -3
            float randomXOffset = Random.Range(0, 4) * -1;
            flowerStem.position += new Vector3(randomXOffset, 0, 0);

            // Randomly set the Y scale to be either 3, 4, 5, or 6
            int randomYScale = Random.Range(3, 7);  // This will give values between 3 and 6 (inclusive)
            flowerStem.localScale = new Vector3(flowerStem.localScale.x, randomYScale, flowerStem.localScale.z);

            if(randomYScale == 4)
            {
                isoSpriteSorting.SorterPositionOffset.y -= 1;
            }
            else if(randomYScale == 5)
            {
                isoSpriteSorting.SorterPositionOffset.y -= 2;
            }
            else if(randomYScale == 6)
            {
                isoSpriteSorting.SorterPositionOffset.y -= 3;
            }
            else if(randomYScale == 7)
            {
                isoSpriteSorting.SorterPositionOffset.y -= 4;
            }
        }


    }

    private void SetRandomColor(SpriteRenderer spriteRenderer)
    {
        float hue = Random.Range(0f, 1f);
        float saturation = 0.68f;
        float value = 1f;

        Color randomColor = Color.HSVToRGB(hue, saturation, value);
        spriteRenderer.color = randomColor;
    }
}
