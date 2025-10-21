using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour
{
    private List<Transform> initialPositionsList; 
    private List<SpriteRenderer> childSpriteRenderers = new List<SpriteRenderer>();
    private IsoSpriteSorting isoSpriteSorting;

    [SerializeField] GameObject petal;
    [SerializeField] GameObject bud;
    [SerializeField] GameObject trig;
    [SerializeField] GameObject stem;

    public int trigPosX;
    public int trigPosY;

    public int stemRangeMin = 1;
    public int stemRangeMax = 6;
    public int minOffsetX = 0;
    public int maxOffsetX = 3;


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
                initialPositionsList.Add(child);
            }
        }

        // Apply a random color with fixed V to each child SpriteRenderer
        foreach (SpriteRenderer spriteRenderer in childSpriteRenderers)
        {
            if(!spriteRenderer.name.Equals("FlowerStem") && !spriteRenderer.name.Equals("DontChangeColor"))
            {
                SetRandomColor(spriteRenderer);
            }
            else if(spriteRenderer.name.Equals("FlowerStem"))
            {
                SetRandomGreenColor(spriteRenderer);
            }
        }

        // Find and move the FlowerStem object (child)
        Transform flowerStem = transform.Find("FlowerStem");
        if (flowerStem != null)
        {
            // Randomly move the flowerStem  
            int randomXOffset = Random.Range(minOffsetX, maxOffsetX+1);
            flowerStem.position += new Vector3(randomXOffset, 0, 0);
            
        

            int randomYScale = Random.Range(stemRangeMin, stemRangeMax+1);  // This will give values between 3 and 6 (inclusive)
            flowerStem.localScale = new Vector3(flowerStem.localScale.x, randomYScale, flowerStem.localScale.z);



            for (int i = 1; i <= stemRangeMax; i++)
            {            
                if(randomYScale == stemRangeMin + i)
                {
                    isoSpriteSorting.SorterPositionOffset.y -= i;
                }
            }

            trig.transform.position = new Vector3((trig.transform.position.x + randomXOffset), (trig.transform.position.y - randomYScale + 1), 0);


            // if(randomYScale == stemRangeMin + 1)
            // {
            //     isoSpriteSorting.SorterPositionOffset.y -= 1;
            // }
            // else if(randomYScale == stemRangeMin + 2)
            // {
            //     isoSpriteSorting.SorterPositionOffset.y -= 2;
            // }
            // else if(randomYScale == stemRangeMin + 3)
            // {
            //     isoSpriteSorting.SorterPositionOffset.y -= 3;
            // }
            // else if(randomYScale == stemRangeMin + 4)
            // {
            //     isoSpriteSorting.SorterPositionOffset.y -= 4;
            // }
        }


    }

    private void SetRandomColor(SpriteRenderer spriteRenderer)
    {
        float hue;

        // Generate hue while avoiding the green range (0.25 to 0.45)
        float randomValue = Random.value; // Random value between 0 and 1
        if (randomValue < 0.75f)
        {
            hue = randomValue * 0.25f; // Map 0 to 0.75 to 0 to 0.25
        }
        else
        {
            hue = 0.45f + (randomValue - 0.75f) * 0.55f; // Map 0.75 to 1 to 0.45 to 1
        }

        float saturation = 0.68f;
        float value = 1f;

        Color randomColor = Color.HSVToRGB(hue, saturation, value);
        spriteRenderer.color = randomColor;
    }

    private void SetRandomGreenColor(SpriteRenderer spriteRenderer)
    {
        float hue = Random.Range(0.22f, 0.42f);

        // Generate hue while avoiding the green range (0.25 to 0.45)

        float saturation = 0.68f;
        float value = 1f;

        Color randomColor = Color.HSVToRGB(hue, saturation, value);
        spriteRenderer.color = randomColor;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Bounds playerBounds = collision.bounds;

        // Find the point of contact
        Vector2 contactPoint = collision.ClosestPoint(transform.position);

        // Define regions within the player's collider
        Vector2 center = playerBounds.center;
        Vector2 max = playerBounds.max;
        Vector2 min = playerBounds.min;

        // Check if the contact point is within the top-right quarter
        if (contactPoint.x >= center.x && contactPoint.y >= center.y)
        {
            // Execute your desired code here for top-right 

        }
        // Check if the contact point is within the top-left quarter
        else if (contactPoint.x <= center.x && contactPoint.y >= center.y)
        {
            // Execute your desired code here for top-left

        }
        // Check if the contact point is within the bottom-left quarter
        else if (contactPoint.x <= center.x && contactPoint.y <= center.y)
        {
            // Execute your desired code here for bottom-left

        }
        // Check if the contact point is within the bottom-right quarter
        else if (contactPoint.x >= center.x && contactPoint.y <= center.y)
        {
            // Execute your desired code here for bottom-right

        }
    }
}

