using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public Sprite[] allHeadSprites; // Array to hold all 156 sprites from the sprite sheet VV
    public Sprite[] allEyeSprites; 
    public Sprite[] allThroatSprites; 
    public Sprite[] allCollarSprites;
    public Sprite[] allTorsoSprites;
    public Sprite[] allWaistShortsSprites; 
    public Sprite[] allKneesShinsSprites; 
    public Sprite[] allAnklesSprites; 
    public Sprite[] allFeetSprites; 
    public Sprite[] allLongSleeveSprites; 
    public Sprite[] allHandSprites; 
    public Sprite[] allShortSleeveSprites; 

    public float animationSpeed = 0.01f; // Time between frames
    private int movementDirection;
    private SpriteRenderer headSprite;
    private SpriteRenderer eyeSprite;
    private SpriteRenderer throatSprite;
    private SpriteRenderer collarSprite;
    private SpriteRenderer torsoSprite;
    private SpriteRenderer waistShortsSprite;
    private SpriteRenderer kneesShinsSprite;
    private SpriteRenderer anklesSprite;
    private SpriteRenderer feetSprite;
    private SpriteRenderer longSleeveSprite;
    private SpriteRenderer handSprite;
    private SpriteRenderer shortSleeveSprite;
    private float timer;
    private int currentFrame;
    private bool isWalking;

    public int movementStartIndex;
    public int movementFrameCount;
    private int[] movementIndices;

    public int bodyTypeNumber;
    public int bodyTypeIndexMultiplier = 156;


    void Start()
    {
        bodyTypeNumber = 0;
        
        allHeadSprites = Resources.LoadAll<Sprite>("head");
        allEyeSprites = Resources.LoadAll<Sprite>("eyes");
        allThroatSprites = Resources.LoadAll<Sprite>("throat");
        allCollarSprites = Resources.LoadAll<Sprite>("collar");
        allTorsoSprites = Resources.LoadAll<Sprite>("torso");
        allWaistShortsSprites = Resources.LoadAll<Sprite>("waistShorts");
        allKneesShinsSprites = Resources.LoadAll<Sprite>("kneesShins");
        allAnklesSprites = Resources.LoadAll<Sprite>("ankles");
        allFeetSprites = Resources.LoadAll<Sprite>("feet");
        allLongSleeveSprites = Resources.LoadAll<Sprite>("longSleeve");
        allHandSprites = Resources.LoadAll<Sprite>("hands");
        allShortSleeveSprites = Resources.LoadAll<Sprite>("shortSleeve");




        headSprite = transform.Find("head").GetComponent<SpriteRenderer>();
        eyeSprite = transform.Find("eyes").GetComponent<SpriteRenderer>();
        throatSprite = transform.Find("throat").GetComponent<SpriteRenderer>();
        collarSprite = transform.Find("collar").GetComponent<SpriteRenderer>();
        torsoSprite = transform.Find("torso").GetComponent<SpriteRenderer>();
        waistShortsSprite = transform.Find("waistShorts").GetComponent<SpriteRenderer>();
        kneesShinsSprite = transform.Find("kneesShins").GetComponent<SpriteRenderer>();
        anklesSprite = transform.Find("ankles").GetComponent<SpriteRenderer>();
        feetSprite = transform.Find("feet").GetComponent<SpriteRenderer>();
        longSleeveSprite = transform.Find("longSleeve").GetComponent<SpriteRenderer>();
        handSprite = transform.Find("hands").GetComponent<SpriteRenderer>();
        shortSleeveSprite = transform.Find("shortSleeve").GetComponent<SpriteRenderer>();

        // Set the initial state to idle left using the idleLeftIndex
        headSprite.sprite = allHeadSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
        eyeSprite.sprite = allEyeSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
        throatSprite.sprite = allThroatSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
        collarSprite.sprite = allCollarSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
        torsoSprite.sprite = allTorsoSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
        waistShortsSprite.sprite = allWaistShortsSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
        kneesShinsSprite.sprite = allKneesShinsSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
        anklesSprite.sprite = allAnklesSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
        feetSprite.sprite = allFeetSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
        longSleeveSprite.sprite = allLongSleeveSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
        handSprite.sprite = allHandSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
        shortSleeveSprite.sprite = allShortSleeveSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
    }

    public void AnimateMovement(int movementStartIndex, int movementFrameCount, int animationDirection)
    {
        movementIndices = Enumerable.Range(movementStartIndex + animationDirection, movementFrameCount).ToArray();
        // Timer to control the animation frame rate
        timer += Time.deltaTime;

        // If enough time has passed, move to the next frame
        if (timer >= animationSpeed)
        {
            timer = 0f; // Reset timer

            // Update the current frame
            currentFrame++;

            // If we've reached the end of the walkLeftIndices array, loop back to the first sprite
            if (currentFrame >= movementIndices.Length)
            {
                currentFrame = 0;
            }

            // Set the sprite to the current frame in the walkLeftIndices array
            headSprite.sprite = allHeadSprites[movementIndices[currentFrame]];
            eyeSprite.sprite = allEyeSprites[movementIndices[currentFrame]];
            throatSprite.sprite = allThroatSprites[movementIndices[currentFrame]];
            collarSprite.sprite = allCollarSprites[movementIndices[currentFrame]];
            torsoSprite.sprite = allTorsoSprites[movementIndices[currentFrame]];
            waistShortsSprite.sprite = allWaistShortsSprites[movementIndices[currentFrame]];
            kneesShinsSprite.sprite = allKneesShinsSprites[movementIndices[currentFrame]];
            anklesSprite.sprite = allAnklesSprites[movementIndices[currentFrame]];
            feetSprite.sprite = allFeetSprites[movementIndices[currentFrame]];
            longSleeveSprite.sprite = allLongSleeveSprites[movementIndices[currentFrame]];
            handSprite.sprite = allHandSprites[movementIndices[currentFrame]];
            shortSleeveSprite.sprite = allShortSleeveSprites[movementIndices[currentFrame]];
        }
    }
}
