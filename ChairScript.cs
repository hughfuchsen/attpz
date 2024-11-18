using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairScript : MonoBehaviour
{
    CharacterMovement myCharacterMovement;
    CharacterAnimation myCharacterAnimation;
    CameraMovement cameraMovement;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject chairScriptAnchorPoint;
    public Vector3 currentSeatAnchorPoint;
    private Vector3 initialPlayerPosBeforeSitting;

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


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
        chairScriptAnchorPoint = transform.Find("chairScriptAnchorPoint").gameObject;
        myCharacterMovement = Player.GetComponent<CharacterMovement>();
        myCharacterAnimation = Player.GetComponent<CharacterAnimation>();
        currentSeatAnchorPoint = chairScriptAnchorPoint.transform.position;

        currentLayerIndex = gameObject.layer; // Get the current layer of this GameObject
    }

    // Make sure the method has the correct signature
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision object is the player
        if (collision.gameObject.CompareTag("PlayerCollider"))
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            IgnoreCollisionLayer();


            initialPlayerPosBeforeSitting = Player.transform.position;

            cameraMovement.freezeCamPos = true;

            // if ((Input.GetKey(KeyCode.Space) || 
            //           Input.GetKey(KeyCode.JoystickButton0) ||  // A button
            //           Input.GetKey(KeyCode.JoystickButton1) ||  // B button
            //           Input.GetKey(KeyCode.JoystickButton2)  // X button
            //           // Input.GetKey(KeyCode.JoystickButton3)
            //           ) && !myCharacterMovement.playerOnBike && !myCharacterMovement.playerSitting)
            // {
                // myCharacterMovement.StartDeactivateSpaceBar(); // Use centralized method
                // collision.gameObject.SetActive(false); // turn off collider

                PerformActionBasedOnFacing();

                // IgnoreCollisionLayer();
                // Player sits

                // Freeze the camera position


                myCharacterMovement.playerSitting = true;

                // Move the player to the current seat anchor point
                Player.transform.position = currentSeatAnchorPoint + new Vector3(-8, 22, 0) + new Vector3(-4, 1, 0);
            // }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the collision object is the player
        if (collision.gameObject.CompareTag("PlayerCollider"))
        {
                myCharacterMovement.playerSitting = false;
                Player.transform.position = initialPlayerPosBeforeSitting;
                cameraMovement.freezeCamPos = false;
                myCharacterMovement.ResetPlayerMovement();
                
                SetCollisionLayer();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCollider"))
        {
            cameraMovement.freezeCamPos = true;

            if ((Input.GetKey(KeyCode.Space) || 
                      Input.GetKey(KeyCode.JoystickButton0) ||  // A button
                      Input.GetKey(KeyCode.JoystickButton1) ||  // B button
                      Input.GetKey(KeyCode.JoystickButton2)  // X button
                      // Input.GetKey(KeyCode.JoystickButton3)
                      ) && !myCharacterMovement.playerOnBike && !myCharacterMovement.playerSitting)
            {
                myCharacterMovement.StartDeactivateSpaceBar(); // Use centralized method
                // collision.gameObject.SetActive(false); // turn off collider

                PerformActionBasedOnFacing();

                // IgnoreCollisionLayer();
                // Player sits

                // Freeze the camera position


                myCharacterMovement.playerSitting = true;

                // Move the player to the current seat anchor point
                Player.transform.position = currentSeatAnchorPoint + new Vector3(-8, 22, 0) + new Vector3(-4, 1, 0);
            }
            // else if (Input.GetKeyUp(KeyCode.Space))
            // {
            //     // Player stands up
            //     myCharacterMovement.playerSitting = false;

            //     // Unfreeze the camera and let it follow the player again
            //     // cameraMovement.freezeCamPos = false;

            //     Player.transform.position = initialPlayerPosBeforeSitting;

            // }
            else
            {

                // cameraMovement.adjustSmoothingCoro = StartCoroutine(cameraMovement.AdjustSmoothing()); 

                // cameraMovement.freezeCamPos = false;

            }
        }
    }

    // Method to use the selected direction conditionally
    public void PerformActionBasedOnFacing()
    {
        switch (currentFacing)
        {
            case FacingDirection.UpLeft:
                Debug.Log("The couch is facing up-left.");
                // Add specific behavior here
                break;

            case FacingDirection.UpRight:
                Debug.Log("The couch is facing up-right.");
                // Add specific behavior here
                break;

            case FacingDirection.DownLeft:
                Debug.Log("The couch is facing down-left.");
                myCharacterAnimation.currentAnimationDirection = myCharacterAnimation.leftDownAnim;
                myCharacterMovement.controlDirectionToPlayerDirection[Direction.Left] = Direction.Nothing;
                myCharacterMovement.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.Nothing;
                myCharacterMovement.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.Nothing;
                myCharacterMovement.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.Nothing;
                myCharacterMovement.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.Nothing;
                myCharacterMovement.controlDirectionToPlayerDirection[Direction.Right] = Direction.Nothing;
                myCharacterMovement.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.Nothing;
                myCharacterMovement.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.DownFacingRight;
                myCharacterMovement.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.DownFacingLeft;
                myCharacterMovement.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.DownLeft;
                // Add specific behavior here
                break;

            case FacingDirection.DownRight:
                Debug.Log("The couch is facing down-right.");
                // Add specific behavior here
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



}
