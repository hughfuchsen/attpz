using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureScript : MonoBehaviour
{
    CharacterMovement myCharacterMovement;
    CharacterAnimation characterAnimation;
    CharacterCustomization myCharacterCustomization;
    CameraMovement cameraMovement;
    public GameObject Player;
    [SerializeField] GameObject anchorPoint;
    public Vector3 currentAnchorPoint;
    public bool itsAtoilet = false;
    public bool itsAbed = false;

    public Color furnitureColor = Color.white;
    private Vector3 initialPlayerPosBeforeEngaging;

    private int currentLayerIndex;


    public enum FacingDirection
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }
        // Selectable in the Unity Editor
        [SerializeField]
        private FacingDirection currentFacing;

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
        anchorPoint = transform.Find("anchorPoint").gameObject;
        myCharacterMovement = Player.GetComponent<CharacterMovement>();
        characterAnimation = Player.GetComponent<CharacterAnimation>();
        myCharacterCustomization = Player.GetComponent<CharacterCustomization>();
        currentAnchorPoint = anchorPoint.transform.position;

        currentLayerIndex = gameObject.layer; // Get the current layer of this GameObject
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
                        ) && !myCharacterMovement.playerOnBike && !myCharacterMovement.playerOnFurniture)
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
                HandleBedEngagement(Player, 0);                
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
        
        gameObject.layer = currentLayerIndex;
    }

    public void HandleWaistSpriteTransform()
    {
        if(characterAnimation.bodyTypeNumber == 1 
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
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2, -3, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, -3f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }
        }
        else if(characterAnimation.bodyTypeNumber == 9
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
                  if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2, -4, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, -4f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0, 0);
            }
        }

    }


    //     // else if(characterAnimation.bodyTypeNumber == 3 || characterAnimation.bodyTypeNumber == 4)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, 0, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, 0, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, 0, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, 0, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2f, 0, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, 0, 0);
    //     //     }
    //     // }
        

    //     // else if(characterAnimation.bodyTypeNumber == 5)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2f, -1, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, -1f, 0);
    //     //     }
    //     // }

    //     // else if(characterAnimation.bodyTypeNumber == 6)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, -1, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, -1f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 7 || characterAnimation.bodyTypeNumber == 8)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, -1f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 9)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-3f, -3f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2f, 1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, 1f, 0);
    //     //     }
    //     // }

    //     // else if(characterAnimation.bodyTypeNumber == 10  || characterAnimation.bodyTypeNumber == 11 || characterAnimation.bodyTypeNumber == 12)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, 1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, 1f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 13)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, 0f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 14 || characterAnimation.bodyTypeNumber == 15 || characterAnimation.bodyTypeNumber == 16)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, 0f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 17)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, -2f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 18 || characterAnimation.bodyTypeNumber == 19 || characterAnimation.bodyTypeNumber == 20)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, -2f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 21)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, 1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, 1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, 1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, 1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2f, 1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, 1f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 22 || characterAnimation.bodyTypeNumber == 23 || characterAnimation.bodyTypeNumber == 24)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, 1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, 1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, 1f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 25)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, -1f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 26 || characterAnimation.bodyTypeNumber == 27 || characterAnimation.bodyTypeNumber == 28)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, -1f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 29)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, -1f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 30 || characterAnimation.bodyTypeNumber == 31 || characterAnimation.bodyTypeNumber == 32)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -2f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, -1f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 33)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(2f, 0f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-2f, 0f, 0);
    //     //     }
    //     // }
        
    //     // else if(characterAnimation.bodyTypeNumber == 34 || characterAnimation.bodyTypeNumber == 35 || characterAnimation.bodyTypeNumber == 36)
    //     // {
    //     //     if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(0f, -1f, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(1f, 0, 0);
    //     //     }

    //     //     else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
    //     //     {
    //     //         characterAnimation.waistSprite.transform.localPosition = characterAnimation.initialWaistTransformPos + new Vector3(-1f, 0, 0);
    //     //     }
    //     // }
        
        

    // }
    public void HandleYTransform()
    {
        if(characterAnimation.bodyTypeNumber == 1 
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
        else{ Player.transform.position = currentAnchorPoint + new Vector3(-12, 23, 0); }


    }
    public void HandleISSTransform()
    {
        if(characterAnimation.bodyTypeNumber == 1 
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
            Player.GetComponent<IsoSpriteSorting>().SorterPositionOffset += new Vector3(0, -0.75f, 0);
        }
    }
    public void HandleBedEngagement(GameObject treeNode, float alpha)
        {
            if (treeNode == null)
            {
                return; // TODO: remove this
            }
            else if (treeNode.name.Contains("hair") || treeNode.name.Contains("head") 
                || treeNode.name.Contains("eyes") || treeNode.name.Contains("mohawk")
                || treeNode.name.Contains("bike"))
            {
                return; // TODO: remove this
            }

            SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();
            if (sr != null )
            {
                if(!treeNode.name.Contains("bedCoverSprite"))
                {
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
                }
                else
                {
                    sr.color = furnitureColor;
                }
            }

            foreach (Transform child in treeNode.transform)
            {
                HandleBedEngagement(child.gameObject, alpha);
            }



            // characterAnimation.headSprite
            // characterAnimation.eyeSprite 
            // characterAnimation.throatSprite
            // characterAnimation.collarSprite
            // characterAnimation.torsoSprite
            // characterAnimation.waistSprite
            // characterAnimation.waistShortsSprite
            // characterAnimation.kneesShinsSprite
            // characterAnimation.anklesSprite
            // characterAnimation.feetSprite 
            // characterAnimation.jakettoSprite
            // characterAnimation.dressSprite
            // characterAnimation.longSleeveSprite
            // characterAnimation.handSprite
            // characterAnimation.shortSleeveSprite
            // characterAnimation.mohawk5TopSprite
            // characterAnimation.mohawk5BottomSprite 
            // characterAnimation.hair0TopSprite
            // characterAnimation.hair0BottomSprite
            // characterAnimation.hair1TopSprite
            // characterAnimation.hair7TopSprite
            // characterAnimation.hair8TopSprite
            // characterAnimation.hair1BottomSprite
            // characterAnimation.hair3BottomSprite
            // characterAnimation.hair4BottomSprite
            // characterAnimation.hair6BottomSprite
            // characterAnimation.hair7BottomSprite
            // characterAnimation.hair8BottomSprite
            // characterAnimation.hairFringe1Sprite
            // characterAnimation.hairFringe2Sprite
            // characterAnimation.bikeSprite 
        }

}
