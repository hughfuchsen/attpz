using System.Collections.Generic;
using UnityEngine;

public class RandomColorizeFlowerOnStart : MonoBehaviour
{
    private List<Vector3> initialPositionsList = new List<Vector3>(); 
    private List<Transform> childTransforms = new List<Transform>();
    private IsoSpriteSorting isoSpriteSorting;

    [SerializeField] private GameObject petal;
    [SerializeField] private GameObject bud;
    [SerializeField] private GameObject trig;
    [SerializeField] private GameObject stem;

    public int stemRangeMin = 1;
    public int stemRangeMax = 6;
    public int minOffsetX = 0;
    public int maxOffsetX = 3;

    private void Start()
    {
        isoSpriteSorting = this.transform.parent.GetComponent<IsoSpriteSorting>();

        // Store each child and its starting position
        Transform parent = transform.parent;

        if (parent != null)
        {
            foreach (Transform child in parent)
            {
                childTransforms.Add(child);
                initialPositionsList.Add(child.position);
            }
        }

        SetRandomColor();
        SetRandomGreenColor();

        // Randomly move the stem
        int randomXOffset = Random.Range(minOffsetX, maxOffsetX + 1);
        stem.transform.position += new Vector3(randomXOffset, 0, 0);

        int randomYScale = Random.Range(stemRangeMin, stemRangeMax + 1);
        stem.transform.localScale = new Vector3(stem.transform.localScale.x, randomYScale, stem.transform.localScale.z);

        for (int i = 1; i <= stemRangeMax; i++)
        {
            if (randomYScale == stemRangeMin + i)
            {
                isoSpriteSorting.SorterPositionOffset.y -= i;
            }
        }

        trig.transform.position = new Vector3(trig.transform.position.x + randomXOffset,
                                              trig.transform.position.y - randomYScale + 1,
                                              0);
    }

    private void SetRandomColor()
    {
        float hue;
        float randomValue = Random.value;

        if (randomValue < 0.75f)
            hue = randomValue * 0.25f;
        else
            hue = 0.45f + (randomValue - 0.75f) * 0.55f;

        float saturation = 0.68f;
        float value = 1f;
        Color randomColor = Color.HSVToRGB(hue, saturation, value);
        petal.GetComponent<SpriteRenderer>().color = randomColor;
    }

    private void SetRandomGreenColor()
    {
        float hue = Random.Range(0.22f, 0.42f);
        float saturation = 0.68f;
        float value = 1f;

        Color randomColor = Color.HSVToRGB(hue, saturation, value);
        stem.GetComponent<SpriteRenderer>().color = randomColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCollider" || collision.gameObject.tag == "NPCCollider" )
        {
            Bounds playerBounds = collision.bounds;
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            Vector2 center = playerBounds.center;

            // Move petal & bud safely by constructing new vectors
            if (contactPoint.x >= center.x && contactPoint.y >= center.y) // bottom left collisionnnn
            {
                petal.transform.position += new Vector3(1, -1, 0);
                bud.transform.position += new Vector3(1, -1, 0);
            }
            else if (contactPoint.x <= center.x && contactPoint.y >= center.y) // bottom right collison
            {
                petal.transform.position += new Vector3(-1, -1, 0);
                bud.transform.position += new Vector3(-1, -1, 0);
            }
            else if (contactPoint.x <= center.x && contactPoint.y <= center.y) // top right collisionnnn
            {
                petal.transform.position += new Vector3(-1, -2, 0);
                bud.transform.position += new Vector3(-1, -2, 0);
            }
            else if (contactPoint.x >= center.x && contactPoint.y <= center.y) // top left collisionnnn
            {
                petal.transform.position += new Vector3(1, -2, 0);
                bud.transform.position += new Vector3(1, -2, 0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Reset all child transforms to their original positions
        for (int i = 0; i < childTransforms.Count; i++)
        {
            if(childTransforms[i].gameObject != stem && childTransforms[i].gameObject != trig)
            {
                childTransforms[i].position = initialPositionsList[i];
            }
        }
    }
}
