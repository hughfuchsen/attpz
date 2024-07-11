using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipAndTransform : MonoBehaviour
{
    public float yOffsetMultiplier = 1f; // Multiplier to adjust the transformation based on sprite name
    public GameObject objectToFlipAndTransform; // Public field to assign the GameObject in the Unity Editor
    public IsoSpriteSorting scriptToToggle; // Script to set y-value

    private SpriteRenderer spriteRenderer;
    private bool isPlayerInside = false;

    private void Start()
    {
        if (objectToFlipAndTransform == null)
        {
            Debug.LogError("Please assign Object To Flip And Transform in the Inspector!");
            return;
        }

        spriteRenderer = objectToFlipAndTransform.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the assigned GameObject!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Change the tag as per your trigger object
        {
            isPlayerInside = true;
            FlipSpriteYAxis();
            TransformBySpriteName();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Change the tag as per your trigger object
        {
            isPlayerInside = false;
            FlipSpriteYAxis();
            TransformBySpriteName();
        }
    }

    private void FlipSpriteYAxis()
    {
        spriteRenderer.flipY = !spriteRenderer.flipY;
    }

    private void TransformBySpriteName()
    {
        string spriteName = spriteRenderer.sprite.name;
        int yOffset = ExtractLastNumber(spriteName);

        if (isPlayerInside)
        {
            objectToFlipAndTransform.transform.Translate(0f, yOffset * yOffsetMultiplier, 0f);
        }
        else
        {
            objectToFlipAndTransform.transform.Translate(0f, -yOffset * yOffsetMultiplier, 0f); // Revert the transformation
        }
    }

    private int ExtractLastNumber(string input)
    {
        string numberString = "";
        for (int i = input.Length - 1; i >= 0; i--)
        {
            if (char.IsDigit(input[i]))
            {
                numberString = input[i] + numberString;
            }
            else
            {
                break;
            }
        }

        return int.Parse(numberString);
    }
}
