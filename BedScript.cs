// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BedScript : MonoBehaviour
// {
//     CharacterMovement myCharacterMovement;
//     CharacterAnimation characterAnimation;
//     CharacterCustomization myCharacterCustomization;
//     CameraMovement cameraMovement;
//     [SerializeField] GameObject Player;
//     [SerializeField] GameObject bedScriptAnchorPoint;
//     public Vector3 currentSeatAnchorPoint;
//     public bool itsAToilet = false;
//     private Vector3 initialPlayerPosBeforeLaying;

//     private int currentLayerIndex;


//     public enum FacingDirection
//     {
//         UpLeft,
//         UpRight,
//         DownLeft,
//         DownRight
//     }

//     // Selectable in the Unity Editor
//     [SerializeField]
//     private FacingDirection currentFacing;


//     // Start is called before the first frame update
//     void Start()
//     {
//         Player = GameObject.FindGameObjectWithTag("Player");
//         cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
//         bedScriptAnchorPoint = transform.Find("bedScriptAnchorPoint").gameObject;
//         myCharacterMovement = Player.GetComponent<CharacterMovement>();
//         characterAnimation = Player.GetComponent<CharacterAnimation>();
//         myCharacterCustomization = Player.GetComponent<CharacterCustomization>();
//         currentSeatAnchorPoint = bedScriptAnchorPoint.transform.position;

//         currentLayerIndex = gameObject.layer; // Get the current layer of this GameObject
//     }

//     // Make sure the method has the correct signature
//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         // Check if the collision object is the player
//         if (collision.gameObject.CompareTag("PlayerCollider"))
//         {
//             characterAnimation.currentChairScript = null;

//             characterAnimation.currentChairScript = this;

//             initialPlayerPosBeforeLaying = Player.transform.position;
//         }
//     }
//     private void OnTriggerExit2D(Collider2D collision)
//     {
//         // Check if the collision object is the player
//         if (collision.gameObject.CompareTag("PlayerCollider"))
//         {
//             // characterAnimation.currentChairScript = null;
//         }
//     }

//     public void StopLaying()
//     {
//         myCharacterMovement.playerSitting = false;
//         Player.transform.position = initialPlayerPosBeforeLaying;
//         cameraMovement.freezeCamPos = false;
//         myCharacterMovement.ResetPlayerMovement();

        
//         SetCollisionLayer();
        
//         Player.GetComponent<IsoSpriteSorting>().SorterPositionOffset = new Vector3(8, -28, 0);
//     }

//     private void OnTriggerStay2D(Collider2D collision)
//     {
//         if (collision.gameObject.CompareTag("PlayerCollider"))
//         {
//             if (myCharacterMovement.spaceBarDeactivated == false && characterAnimation.currentChairScript == this)
//             {
//                 if ((Input.GetKey(KeyCode.Space) || 
//                         Input.GetKey(KeyCode.JoystickButton0) ||  // A button
//                         Input.GetKey(KeyCode.JoystickButton1) ||  // B button
//                         Input.GetKey(KeyCode.JoystickButton2)  // X button
//                         // Input.GetKey(KeyCode.JoystickButton3)
//                         ) && !myCharacterMovement.playerOnBike && !myCharacterMovement.playerSitting)
//                 {
//                     gameObject.layer = LayerMask.NameToLayer("Player");
//                     IgnoreCollisionLayer();

//                     myCharacterMovement.StartDeactivateSpaceBar(); // Use centralized method

//                     PerformActionBasedOnFacing();
  
//                     myCharacterCustomization.SetPlayerToSleepMode();


//                     myCharacterMovement.playerSitting = true;

//                     cameraMovement.freezeCamPos = true;

//                     initialPlayerPosBeforeLaying = Player.transform.position;

//                     // Move the player to the current seat anchor point
//                     // Player.transform.position = currentSeatAnchorPoint + new Vector3(-8, 22, 0) + new Vector3(-4, 1, 0);
//                     HandleYTransform();
//                     HandleISSTransform();

//                 }
//             }
//         }   
//     }

//     // Method to use the selected direction conditionally
//     public void PerformActionBasedOnFacing()
//     {
//         switch (currentFacing)
//         {
//             case FacingDirection.UpLeft:
//                 characterAnimation.currentAnimationDirection = characterAnimation.upLeftAnim;
//                 // itsAToilet ? HandleWaistSpriteTransform() : break;
//                 // HandleWaistSpriteTransform();
//                 break;

//             case FacingDirection.UpRight:
//                 characterAnimation.currentAnimationDirection = characterAnimation.upRightAnim;
//                 break;

//             case FacingDirection.DownLeft:
//                 characterAnimation.currentAnimationDirection = characterAnimation.leftDownAnim;
//                 break;

//             case FacingDirection.DownRight:
//                 characterAnimation.currentAnimationDirection = characterAnimation.rightDownAnim;
//                 break;
//         }
//     }
    

//     public void IgnoreCollisionLayer()
//     {
//         // Disable collisions with all layers
//         for (int i = 0; i < 32; i++)
//         {
//                 Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), i, true);

//                 // Re-enable collision with the target layer
//                 Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), false); 
//         }
//     }

//     public void SetCollisionLayer()
//     {
//         // Disable collisions with all layers
//         for (int i = 0; i < 32; i++)
//         {
//             Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), i, true);
//         }



//         // Re-enable collision with the target layer
//         Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), currentLayerIndex, false); 
        
//         gameObject.layer = currentLayerIndex;
//     }

 
//     public void HandleYTransform()
//     {
//         if(characterAnimation.bodyTypeNumber == 1 
//         || characterAnimation.bodyTypeNumber == 2
//         || characterAnimation.bodyTypeNumber == 3
//         || characterAnimation.bodyTypeNumber == 4
//         || characterAnimation.bodyTypeNumber == 13
//         || characterAnimation.bodyTypeNumber == 14
//         || characterAnimation.bodyTypeNumber == 15
//         || characterAnimation.bodyTypeNumber == 16
//         || characterAnimation.bodyTypeNumber == 25
//         || characterAnimation.bodyTypeNumber == 26
//         || characterAnimation.bodyTypeNumber == 27
//         || characterAnimation.bodyTypeNumber == 28)
//         {
//             Player.transform.position = currentSeatAnchorPoint + new Vector3(-12, 24, 0);
//         }
//         else{ Player.transform.position = currentSeatAnchorPoint + new Vector3(-12, 23, 0); }
//     }
//     public void HandleISSTransform()
//     {
//         if(characterAnimation.bodyTypeNumber == 1 
//         || characterAnimation.bodyTypeNumber == 2
//         || characterAnimation.bodyTypeNumber == 3
//         || characterAnimation.bodyTypeNumber == 4
//         || characterAnimation.bodyTypeNumber == 13
//         || characterAnimation.bodyTypeNumber == 14
//         || characterAnimation.bodyTypeNumber == 15
//         || characterAnimation.bodyTypeNumber == 16
//         || characterAnimation.bodyTypeNumber == 25
//         || characterAnimation.bodyTypeNumber == 26
//         || characterAnimation.bodyTypeNumber == 27
//         || characterAnimation.bodyTypeNumber == 28)
//         {
//             Player.GetComponent<IsoSpriteSorting>().SorterPositionOffset += new Vector3(0, -0.75f, 0);
//         }
//     }


// }
