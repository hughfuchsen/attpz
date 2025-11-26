using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureScript : MonoBehaviour
{
    CharacterMovement myCharacterMovement;
    CharacterAnimation characterAnimation;
    CharacterCustomization myCharacterCustomization;
    CameraMovement cameraMovement;

    LoadCSVDataWeb characterLoader;
    public GameObject Player;
    [SerializeField] GameObject anchorPoint;
    [SerializeField] GameObject bedCoverSprite;

    public List<GameObject> bedCoverSpriteList = new List<GameObject>();

    List<float> initialAlphaFloatList = new List<float>();
    List<GameObject> playerSpriteList = new List<GameObject>();


    public Vector3 currentAnchorPoint;

    public float issOffsetY = 0f;
    public Color furnitureColor = Color.white;
    private Vector3 initialPlayerPosBeforeEngaging;

    private int currentLayerIndex;


    private Coroutine setBlankyAlphaZeroLateStartCoro;
    public BedCoverTransformAdjustmentScript bedCoverTransformAdjustmentScript;


    public enum FacingDirection
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }
    // Selectable in the Unity Editor
    [SerializeField]
    public FacingDirection currentFacing;

    public enum FurnitureType
    {
        chair,
        toilet,
        bed
    }
    // Selectable in the Unity Editor
    [SerializeField]
    public FurnitureType currentFurnitureType;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
        characterLoader = GameObject.FindGameObjectWithTag("CharacterCustomizationMenu").GetComponent<LoadCSVDataWeb>();
        anchorPoint = transform.Find("anchorPoint").gameObject;
        myCharacterMovement = Player.GetComponent<CharacterMovement>();
        characterAnimation = Player.GetComponent<CharacterAnimation>();
        bedCoverTransformAdjustmentScript = Player.GetComponent<BedCoverTransformAdjustmentScript>();
        myCharacterCustomization = Player.GetComponent<CharacterCustomization>();
        currentAnchorPoint = anchorPoint.transform.position;


        if (bedCoverSprite != null)
        {
            GetSpritesAndAddToLists(bedCoverSprite, bedCoverSpriteList);
            setBlankyAlphaZeroLateStartCoro = StartCoroutine(BlankyAlphaZeroLateStart());
        }

        GetSpritesAndAddToLists(Player, playerSpriteList);

        furnitureColor.a = 0f;
        for (int i = 1; i < bedCoverSpriteList.Count; i++)
        {
            if (bedCoverSpriteList[i].GetComponent<SpriteRenderer>() != null)
                bedCoverSpriteList[i].GetComponent<SpriteRenderer>().color = furnitureColor;
        }
        currentLayerIndex = gameObject.layer; // Get the current layer of this GameObject
    }

    IEnumerator BlankyAlphaZeroLateStart()
    {
        yield return new WaitForEndOfFrame();
        SetBedCoverAlpha(false);
    }

    // Make sure the method has the correct signature
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision object is the player
        if (collision.gameObject.CompareTag("PlayerCollider"))
        {
            characterAnimation.currentFurnitureScript = null;

            characterAnimation.currentFurnitureScript = this;

            initialPlayerPosBeforeEngaging = Player.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the collision object is the player
        if (collision.gameObject.CompareTag("PlayerCollider"))
        {
            // characterAnimation.currentFurnitureScript = null;
        }
    }

    public void StopEngaging()
    {
        myCharacterMovement.playerOnFurniture = false;
        Player.transform.position = initialPlayerPosBeforeEngaging;
        cameraMovement.freezeCamPos = false;
        myCharacterMovement.ResetPlayerMovement();

        characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos;

        SetCollisionLayer();

        Player.GetComponent<IsoSpriteSorting>().SorterPositionOffset = new Vector3(8, -28, 0);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCollider"))
        {
            if (myCharacterMovement.spaceBarDeactivated == false && characterAnimation.currentFurnitureScript == this)
            {
                if ((Input.GetKey(KeyCode.Space) ||
                        Input.GetKey(KeyCode.JoystickButton0) ||  // A button
                        Input.GetKey(KeyCode.JoystickButton1) ||  // B button
                        Input.GetKey(KeyCode.JoystickButton2)  // X button
                                                               // Input.GetKey(KeyCode.JoystickButton3)
                        ) && !myCharacterMovement.playerOnBike && !myCharacterMovement.playerOnFurniture 
                          && myCharacterMovement.change == Vector3.zero)
                {
                    gameObject.layer = LayerMask.NameToLayer("Player");
                    IgnoreCollisionLayer();

                    myCharacterMovement.StartDeactivateSpaceBar(); // Use centralized method

                    PerformActionBasedOnFacing();
                    PerformActionBasedOnFurniture();
                    // if(itsAtoilet)
                    // {
                    //     myCharacterCustomization.SetPantsTotoiletMode();
                    //     HandleWaistSpriteTransform();
                    // }


                    myCharacterMovement.playerOnFurniture = true;

                    cameraMovement.freezeCamPos = true;

                    initialPlayerPosBeforeEngaging = Player.transform.position;

                    // Move the player to the current seat anchor point
                    // Player.transform.position = currentAnchorPoint + new Vector3(-8, 22, 0) + new Vector3(-4, 1, 0);
                    HandleYTransform();
                    HandleISSTransform();

                }
            }
        }
    }

    // Method to use the selected direction conditionally
    public void PerformActionBasedOnFacing()
    {
        switch (currentFacing)
        {
            case FacingDirection.UpLeft:
                characterAnimation.currentAnimationDirection = characterAnimation.upLeftAnim;
                // itsAtoilet ? HandleWaistSpriteTransform() : break;
                // HandleWaistSpriteTransform();
                break;

            case FacingDirection.UpRight:
                characterAnimation.currentAnimationDirection = characterAnimation.upRightAnim;
                break;

            case FacingDirection.DownLeft:
                characterAnimation.currentAnimationDirection = characterAnimation.leftDownAnim;
                break;

            case FacingDirection.DownRight:
                characterAnimation.currentAnimationDirection = characterAnimation.rightDownAnim;
                break;
        }
    }

    // Method to use the selected direction conditionally
    public void PerformActionBasedOnFurniture()
    {
        switch (currentFurnitureType)
        {
            case FurnitureType.chair:
                // characterAnimation.currentAnimationDirection = characterAnimation.upLeftAnim;
                // itsAtoilet ? HandleWaistSpriteTransform() : break;
                // HandleWaistSpriteTransform();
                break;

            case FurnitureType.toilet:
                myCharacterCustomization.SetPantsToToiletMode();
                HandleWaistSpriteTransform();
                break;

            case FurnitureType.bed:
                SetBedCoverAlpha(true);
                HandlePlayerAlphaEngagement(true); // alphas of sprites!!!!!!! 
                break;
        }
    }


    public void IgnoreCollisionLayer()
    {
        // Disable collisions with all layers
        for (int i = 0; i < 32; i++)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), i, true);

            // Re-enable collision with the target layer
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), false);
        }
    }

    public void SetCollisionLayer()
    {
        // Disable collisions with all layers
        for (int i = 0; i < 32; i++)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), i, true);
        }



        // Re-enable collision with the target layer
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), currentLayerIndex, false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("InclineTrigger"), false);

        gameObject.layer = currentLayerIndex;
    }

    public void HandleWaistSpriteTransform()
    {
        if (characterAnimation.bodyTypeNumber == 1
        || characterAnimation.bodyTypeNumber == 2
        || characterAnimation.bodyTypeNumber == 3
        || characterAnimation.bodyTypeNumber == 4
        || characterAnimation.bodyTypeNumber == 5
        || characterAnimation.bodyTypeNumber == 6
        || characterAnimation.bodyTypeNumber == 6
        || characterAnimation.bodyTypeNumber == 8
        || characterAnimation.bodyTypeNumber == 13
        || characterAnimation.bodyTypeNumber == 14
        || characterAnimation.bodyTypeNumber == 15
        || characterAnimation.bodyTypeNumber == 16
        || characterAnimation.bodyTypeNumber == 17
        || characterAnimation.bodyTypeNumber == 18
        || characterAnimation.bodyTypeNumber == 19
        || characterAnimation.bodyTypeNumber == 20
        || characterAnimation.bodyTypeNumber == 25
        || characterAnimation.bodyTypeNumber == 26
        || characterAnimation.bodyTypeNumber == 27
        || characterAnimation.bodyTypeNumber == 28
        || characterAnimation.bodyTypeNumber == 29
        || characterAnimation.bodyTypeNumber == 30
        || characterAnimation.bodyTypeNumber == 31
        || characterAnimation.bodyTypeNumber == 32)
        {
            if (characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2, -3, 0);
            }

            else if (characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, -3f, 0);
            }

            else if (characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }

            else if (characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }

            else if (characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }

            else if (characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }
        }
        else if (characterAnimation.bodyTypeNumber == 9
             || characterAnimation.bodyTypeNumber == 10
             || characterAnimation.bodyTypeNumber == 11
             || characterAnimation.bodyTypeNumber == 12
             || characterAnimation.bodyTypeNumber == 21
             || characterAnimation.bodyTypeNumber == 22
             || characterAnimation.bodyTypeNumber == 23
             || characterAnimation.bodyTypeNumber == 24
             || characterAnimation.bodyTypeNumber == 33
             || characterAnimation.bodyTypeNumber == 34
             || characterAnimation.bodyTypeNumber == 35
             || characterAnimation.bodyTypeNumber == 36)
        {
            if (characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2, -4, 0);
            }

            else if (characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, -4f, 0);
            }

            else if (characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }

            else if (characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }

            else if (characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }

            else if (characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }
        }

    }

    public void HandleYTransform()
    {
        if(currentFurnitureType == FurnitureType.bed)
        {
            if (characterAnimation.bodyTypeNumber == 1
            || characterAnimation.bodyTypeNumber == 2
            || characterAnimation.bodyTypeNumber == 3
            || characterAnimation.bodyTypeNumber == 4
            || characterAnimation.bodyTypeNumber == 13
            || characterAnimation.bodyTypeNumber == 14
            || characterAnimation.bodyTypeNumber == 15
            || characterAnimation.bodyTypeNumber == 16)
            {
                Player.transform.position = currentAnchorPoint + new Vector3(-12, 27, 0);
            }
            else if(
             characterAnimation.bodyTypeNumber == 25
            || characterAnimation.bodyTypeNumber == 26
            || characterAnimation.bodyTypeNumber == 27
            || characterAnimation.bodyTypeNumber == 28)
            {
                Player.transform.position = currentAnchorPoint + new Vector3(-12, 26, 0);
            }
            else if (characterAnimation.bodyTypeNumber == 5
            || characterAnimation.bodyTypeNumber == 6
            || characterAnimation.bodyTypeNumber == 7
            || characterAnimation.bodyTypeNumber == 8
            || characterAnimation.bodyTypeNumber == 17
            || characterAnimation.bodyTypeNumber == 18
            || characterAnimation.bodyTypeNumber == 19
            || characterAnimation.bodyTypeNumber == 20)
            {
                Player.transform.position = currentAnchorPoint + new Vector3(-12, 24, 0);
            }
            else if(
             characterAnimation.bodyTypeNumber == 29
            || characterAnimation.bodyTypeNumber == 30
            || characterAnimation.bodyTypeNumber == 31
            || characterAnimation.bodyTypeNumber == 32)
            {
                Player.transform.position = currentAnchorPoint + new Vector3(-12, 23, 0);
            }
            else if (characterAnimation.bodyTypeNumber == 9
            || characterAnimation.bodyTypeNumber == 10
            || characterAnimation.bodyTypeNumber == 11
            || characterAnimation.bodyTypeNumber == 12
            || characterAnimation.bodyTypeNumber == 21
            || characterAnimation.bodyTypeNumber == 22
            || characterAnimation.bodyTypeNumber == 23
            || characterAnimation.bodyTypeNumber == 24
            || characterAnimation.bodyTypeNumber == 33
            || characterAnimation.bodyTypeNumber == 34
            || characterAnimation.bodyTypeNumber == 35
            || characterAnimation.bodyTypeNumber == 36)
            {
                Player.transform.position = currentAnchorPoint + new Vector3(-12, 23, 0);
            }

            else { Player.transform.position = currentAnchorPoint + new Vector3(-12, 25, 0); }
        }


        else // if not a bed baby!
        {
            if (characterAnimation.bodyTypeNumber == 1
            || characterAnimation.bodyTypeNumber == 2
            || characterAnimation.bodyTypeNumber == 3
            || characterAnimation.bodyTypeNumber == 4
            || characterAnimation.bodyTypeNumber == 13
            || characterAnimation.bodyTypeNumber == 14
            || characterAnimation.bodyTypeNumber == 15
            || characterAnimation.bodyTypeNumber == 16
            || characterAnimation.bodyTypeNumber == 25
            || characterAnimation.bodyTypeNumber == 26
            || characterAnimation.bodyTypeNumber == 27
            || characterAnimation.bodyTypeNumber == 28)
            {
                Player.transform.position = currentAnchorPoint + new Vector3(-12, 24, 0);
            }
            else { Player.transform.position = currentAnchorPoint + new Vector3(-12, 23, 0); }
        }


    }
    public void HandleISSTransform()
    {
        if(currentFurnitureType == FurnitureType.bed)
        {
            if (characterAnimation.bodyTypeNumber == 1
            || characterAnimation.bodyTypeNumber == 2
            || characterAnimation.bodyTypeNumber == 3
            || characterAnimation.bodyTypeNumber == 4
            || characterAnimation.bodyTypeNumber == 13
            || characterAnimation.bodyTypeNumber == 14
            || characterAnimation.bodyTypeNumber == 15
            || characterAnimation.bodyTypeNumber == 16
            || characterAnimation.bodyTypeNumber == 25
            || characterAnimation.bodyTypeNumber == 26
            || characterAnimation.bodyTypeNumber == 27
            || characterAnimation.bodyTypeNumber == 28)
            {
                Player.GetComponent<IsoSpriteSorting>().SorterPositionOffset += new Vector3(0, -4f, 0);
            }
        }
        else if(this.name.Contains("Stool"))
        {
            Player.GetComponent<IsoSpriteSorting>().SorterPositionOffset += new Vector3(0, issOffsetY, 0);
        }
        else
        {
            if (characterAnimation.bodyTypeNumber == 1
            || characterAnimation.bodyTypeNumber == 2
            || characterAnimation.bodyTypeNumber == 3
            || characterAnimation.bodyTypeNumber == 4
            || characterAnimation.bodyTypeNumber == 13
            || characterAnimation.bodyTypeNumber == 14
            || characterAnimation.bodyTypeNumber == 15
            || characterAnimation.bodyTypeNumber == 16
            || characterAnimation.bodyTypeNumber == 25
            || characterAnimation.bodyTypeNumber == 26
            || characterAnimation.bodyTypeNumber == 27
            || characterAnimation.bodyTypeNumber == 28)
            {
                Player.GetComponent<IsoSpriteSorting>().SorterPositionOffset += new Vector3(0, -1f, 0);
            }
        }
    }
    public void HandlePlayerAlphaEngagement(bool enter) // Handle alphas!!
    {
       GetSpritesAndAddToLists(Player, playerSpriteList);


        for (int i = 0; i < playerSpriteList.Count; i++)
        {
            SpriteRenderer sr = playerSpriteList[i].GetComponent<SpriteRenderer>();
            Color col = sr.color;

            // Store initial alpha only when entering
            if (enter && initialAlphaFloatList.Count <= i)
            {
                initialAlphaFloatList.Add(col.a);
            }

            // // Check if sprite should be skipped
            // if (
            //     playerSpriteList[i].name.Contains("Top") ||
            //     playerSpriteList[i].name.Contains("head") ||
            //     playerSpriteList[i].name.Contains("eyes") ||
            //     playerSpriteList[i].name.Contains("mohawk") ||
            //     playerSpriteList[i].name.Contains("bike")
            // )
            // {
            //     continue; // Skip modifying these sprites
            // }

            // Adjust alpha
            col.a = enter ? 0f : initialAlphaFloatList[i];

            sr.color = col; // Apply updated color to SpriteRenderer
        }


        for (int i = 0; i < bedCoverSpriteList.Count; i++)
        {
            SpriteRenderer bedSr = bedCoverSpriteList[i].GetComponent<SpriteRenderer>();

            if (bedCoverSpriteList[i].name.Contains("headInBedSprite"))
            {
                // Find the playerSprite with name containing "head"
                for (int j = 0; j < playerSpriteList.Count; j++)
                {
                    if (playerSpriteList[j].name.Contains("head"))
                    {
                        SpriteRenderer playerSr = playerSpriteList[j].GetComponent<SpriteRenderer>();
                        Color col = playerSr.color;
                        col.a = enter ? initialAlphaFloatList[j] : 0;
                        bedSr.color = col;
                        // sr.sprite = playerSr.sprite;
                        break;
                    }
                }
            }

            if (bedCoverSpriteList[i].name.Contains("hairTopInBedSprite"))
            {
                // Find the playerSprite with name containing "head"
                for (int j = 0; j < playerSpriteList.Count; j++)
                {
                    SpriteRenderer playerSr = playerSpriteList[j].GetComponent<SpriteRenderer>();
                    Color col = playerSr.color;
                    
                    if( initialAlphaFloatList[j] == 0 )
                    {
                        bedSr.color = playerSr.color;
                    }
                    else if (playerSpriteList[j].name.Contains("Top") &&  initialAlphaFloatList[j] != 0)
                    {
                        col.a = enter ? initialAlphaFloatList[j] : 0;
                        bedSr.color = col;
                        // sr.sprite = playerSr.sprite;
                        break;
                    }

                }
            }

            // if (bedCoverSpriteList[i].name.Contains("eyesInBedSprite"))
            // {
            //     // Find the playerSprite with name containing "head"
            //     for (int j = 0; j < playerSpriteList.Count; j++)
            //     {
            //         if (playerSpriteList[j].name.Contains("eyes"))
            //         {
            //             SpriteRenderer playerSr = playerSpriteList[j].GetComponent<SpriteRenderer>();
            //             Color col = playerSr.color;
            //             col.a = enter ? initialAlphaFloatList[j] : 0;
            //             sr.color = col;
            //             // sr.sprite = playerSr.sprite;
            //             break;
            //         }
            //     }
            // }
        }


        if (!enter)
        {
            initialAlphaFloatList.Clear(); // Clear initial alpha values when exiting
            playerSpriteList.Clear();
        }
    }



    public void SetBedCoverAlpha(bool enter)
    {
        foreach (GameObject gameObject in bedCoverSpriteList)
        {
            furnitureColor.a = enter ? 1f : 0f;
            Color objCol = gameObject.GetComponent<SpriteRenderer>().color;
            objCol.a = enter ? 1f : 0f;
            if (gameObject.name.Contains("bedCoverSprite"))
            {
                gameObject.GetComponent<SpriteRenderer>().color = furnitureColor; 
            } 
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = objCol; 
            }
        }
    }


    public void SetTreeAlpha(GameObject treeNode, float alpha)
    {
        if (treeNode == null)
        {
            return; // TODO: remove this
        }
        SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
        foreach (Transform child in treeNode.transform)
        {
            SetTreeAlpha(child.gameObject, alpha);
        }
    }

    private void GetSpritesAndAddToLists(GameObject obj, List<GameObject> spriteList)
    {
        spriteList.Clear(); // Clear the list before adding new entries

        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(obj);

        while (stack.Count > 0)
        {
            GameObject currentNode = stack.Pop();
            SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                spriteList.Add(currentNode);
            }

            foreach (Transform child in currentNode.transform)
            {
                stack.Push(child.gameObject);
            }
        }
    }


}
