using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;  // This is necessary for using TMP_InputField

public enum Direction
{
  Left,
  UpLeft,
  UpFacingLeft,
  UpFacingRight,
  UpRight,
  Right,
  RightDown,
  DownFacingLeft,
  DownFacingRight,
  DownLeft,
  Nothing,
}

// Define the enum outside of your class
public enum ContactQuadrant
{
  TopRight,   // 0
  TopLeft,    // 1
  BottomLeft, // 2
  BottomRight, // 3
  None
}
 
public class CharacterMovement : MonoBehaviour
{
  CharacterAnimation characterAnimation;
  CharacterMovement myCharacterMovement;

  CharacterCustomization characterCustomization;
  [HideInInspector] public int movementSpeed = 30;
  [HideInInspector] public int initialMovementSpeed;
  [HideInInspector] public Rigidbody2D myRigidbody; 

  [HideInInspector] public BoxCollider2D boxCollider;

  [HideInInspector] public string motionDirection = "normal";
  [HideInInspector] public Vector3 change;

  [HideInInspector] public bool playerOnThresh = false;
  [HideInInspector] public bool playerOnBuildingThresh = false;
  [HideInInspector] public bool movementAutopilot = false;


  [HideInInspector] public bool fixedDirectionLeftDiagonal;
  [HideInInspector] public bool fixedDirectionRightDiagonal;

  public bool playerIsOutside = false;

  [HideInInspector] public bool facingLeft;

  [HideInInspector] public bool playerOnBike = false;

  [HideInInspector] public bool spaceBarDeactivated;

  [HideInInspector] public Coroutine allowTimeForSpaceBarCoro;
  [HideInInspector] public Coroutine npcRandomMovementCoro;


  [HideInInspector] public bool playerIsCustomizing = false;
  [HideInInspector] public bool playerOnFurniture = false;
  [HideInInspector] public bool playerTouchingCollider = false;


  [HideInInspector] public TMP_InputField[] inputFields;

  Vector2 previousPosition;
  Vector2 velocity;

  // Variable to store the contact quadrant
  [HideInInspector] public ContactQuadrant currentContactQuadrant;

  // Map the angle to control directions
  Direction controlDirection = Direction.Nothing; // Default value should never be used


  // Start is called before the first frame update
  void Start()    
    {
      myRigidbody = GetComponent<Rigidbody2D>();
      myCharacterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
      boxCollider = GetComponentInChildren<BoxCollider2D>();
      // Find all input fields in the scene
      inputFields = FindObjectsOfType<TMP_InputField>();

      initialMovementSpeed = movementSpeed;

      characterAnimation = GetComponent<CharacterAnimation>();
      characterCustomization = GetComponent<CharacterCustomization>();
      
      if(this.gameObject.tag != "Player")
      {
          npcRandomMovementCoro = StartCoroutine(MoveCharacterRandomly());
          // npcRandomMovementCoro = null;
      }

      currentContactQuadrant = ContactQuadrant.BottomRight;

      StartCoroutine(LateStartSpawnInsideCoro());


    }

  // Update is called once per frame
  public void Update()
  {
    Debug.Log(movementAutopilot);
    if(this.gameObject.tag == "Player")
    {
      HandleSpaceBarReactivation();
      if(movementAutopilot == true)
      {
        HandleMovementReactivation();
      }
    }
    
  }
  void FixedUpdate()
    {
      MoveCharacter(); 
    }


  public void MoveCharacter()
  {
      // get user inputs
    if(this.gameObject.tag == "Player")
    {
      change = Vector3.zero;
      if (IsInputFieldFocused() || playerOnFurniture)
      {
        change = Vector3.zero;
      }
      else if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D))
      {
        change.x = Input.GetAxisRaw("LeftSideHoriz1");
        change.y = Input.GetAxisRaw("LeftSideVert1");
      }
      else if (Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.RightArrow))
      {
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
      }
      else
      {
        change.x = Input.GetAxis("Horizontal");
        change.y = Input.GetAxis("Vertical");
      }


      // Calculate velocity based on position change
      Vector2 currentPosition = transform.position;
      velocity = (currentPosition - previousPosition) / Time.deltaTime;

      // Update previous position for the next frame
      previousPosition = currentPosition;

      // Optional: Log velocity for debugging
      // Debug.Log("Velocity: " + velocity);
      // Debug.Log("Input: " + change);
      // CheckDirection(velocity);
    }
      

    if(change != Vector3.zero)
    {
      if(motionDirection == "normal") 
      {
          MoveCharacterNormalDirection();
          // characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.currentAnimationDirection, characterAnimation.bodyTypeNumber);
      } 
      else if (motionDirection == "inclineLeftAway") {
          MoveCharacterVerticalInclineLeftAway();} 
      else if (motionDirection == "inclineRightAway") 
        {MoveCharacterVerticalInclineRightAway();}
      // else if (motionDirection == "inclineLeftToward") 
      //   {MoveCharacterVerticalInclineLeftToward();}
      // else if (motionDirection == "inclineRightToward") 
      //   {MoveCharacterVerticalInclineRightToward();}
      else if (motionDirection == "upDownLadder") 
        {MoveCharacterUpDownLadder();}
      else if (motionDirection == "upLadder") 
        {MoveCharacterUpLadder();}
      else if (motionDirection == "downLadder") 
        {MoveCharacterDownLadder();}
    }
    else if (motionDirection == "upLadder")
    {
      motionDirection = "upDownLadder";
    }
    else if (motionDirection == "downLadder")
    {
      motionDirection = "upDownLadder";
    }
    else // if change == 0 :^)
    {
      if(playerOnFurniture == true)
      {
        characterAnimation.Animate(characterAnimation.sit, 1, characterAnimation.currentAnimationDirection, characterAnimation.bodyTypeNumber);
      }
      else if(!playerOnBike ) // not optimal
      {
        characterAnimation.Animate(characterAnimation.idle, 1, characterAnimation.currentAnimationDirection, characterAnimation.bodyTypeNumber);
      }
      else //  if player on boike
      {
        characterAnimation.Animate(characterAnimation.rideBike, 1, characterAnimation.currentAnimationDirection, characterAnimation.bodyTypeNumber);
      }
    }
  }
  void MoveCharacterVerticalInclineLeftAway()
  {
  // Calculate the angle in degrees (-180 to 180 degrees)
  float angle = Mathf.Atan2(change.y, change.x) * Mathf.Rad2Deg;

  // Adjust by 90 degrees to align 0 degrees with "up"
  angle -= 90f;

  // Convert negative angles to positive angles (0 to 360 degrees)
  if (angle < 0) angle += 360;  

  // Map the angle to movement directions and animations
  if (angle > 0f && angle <= 90f)           { change = new Vector3(-0.7f,1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upLeftAnim; facingLeft = true; } // Inverted right-up to left-up      
  else if (angle > 90f && angle <= 135f)    { change = new Vector3(-1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.leftAnim; facingLeft = true; } // Inverted right to left
  else if (angle > 135f && angle < 180)    { change = new Vector3(-1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.leftDownAnim; facingLeft = true; } // Inverted right-down to left-down
  else if (angle > 180f && angle <= 225f)   { change = new Vector3(0.7f,-1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.rightDownAnim; facingLeft = false; }  // Inverted left-down to right-down
  else if ((angle == 180f)) { change = new Vector3(0.7f,-1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.rightDownAnim; facingLeft = false; }

  else if (angle > 225f && angle <= 270f)   { change = new Vector3(0.7f,-1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.rightAnim; facingLeft = false; }  // Inverted left-down to right-down
  else if ((angle > 270f && angle <= 360f)) { change = new Vector3(1f,0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upRightAnim; facingLeft = false; } // Up
  else if ((angle == 0f)) { change = new Vector3(-0.7f,1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upLeftAnim; facingLeft = true; }

  characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.currentAnimationDirection, characterAnimation.bodyTypeNumber);
  myRigidbody.MovePosition(transform.position + change * movementSpeed * Time.deltaTime);
  }

  void MoveCharacterVerticalInclineRightAway() // needs updating
  {

    // Calculate the angle in degrees (-180 to 180 degrees)
    float angle = Mathf.Atan2(change.y, change.x) * Mathf.Rad2Deg;

    // Adjust by 90 degrees to align 0 degrees with "up"
    angle -= 90f;

    // Convert negative angles to positive angles (0 to 360 degrees)
    if (angle < 0) angle += 360;  

    // Map the angle to movement directions and animations
    if (angle > 0f && angle < 90f)           { change = new Vector3(-1f,0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upLeftAnim; facingLeft = true; } // Inverted right-up to left-up      
    else if (angle >= 90f && angle <= 135f)    { change = new Vector3(-0.7f,-1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.leftAnim; facingLeft = true; } // Inverted right to left
    else if (angle > 135f && angle < 180)    { change = new Vector3(-0.7f,-1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.leftDownAnim; facingLeft = true; } // Inverted right-down to left-down
    else if (angle > 180f && angle <= 225f)   { change = new Vector3(1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.rightAnim; facingLeft = false; }  // Inverted left-down to right-down
    else if ((angle == 180f)) { change = new Vector3(-0.7f,-1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.leftDownAnim; facingLeft = true; }

    else if (angle > 225f && angle < 270f)   { change = new Vector3(1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.rightDownAnim; facingLeft = false; }  // Inverted left-down to right-down
    else if ((angle >= 270f && angle <= 360f)) { change = new Vector3(0.7f,1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upRightAnim; facingLeft = false; } // Up
    else if ((angle == 0f)) { change = new Vector3(0.7f,1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upRightAnim; facingLeft = false; }

    characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.currentAnimationDirection, characterAnimation.bodyTypeNumber);
    myRigidbody.MovePosition(transform.position + change * movementSpeed * Time.deltaTime);    
    
    // if(!fixedDirectionRightDiagonal)
    // {
    //   if (change == Vector3.right+Vector3.up)   { change = new Vector3(0.7f,1f,0f); characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.upRightAnim, characterAnimation.bodyTypeNumber); }
    //   if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.5f,0f); characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.upLeftAnim, characterAnimation.bodyTypeNumber);}
    //   if (change == Vector3.up)                 { change = new Vector3(-1f,0.5f,0f); characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.upLeftAnim, characterAnimation.bodyTypeNumber);}
    //   if (change == Vector3.right)              { change = new Vector3(0.7f,1f,0f); characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.upRightAnim, characterAnimation.bodyTypeNumber);}
    //   if (change == Vector3.right+Vector3.down) { change = new Vector3(0.7f,-1f,0f); characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.rightDownAnim, characterAnimation.bodyTypeNumber);}
    //   if (change == Vector3.left+Vector3.down)  { change = new Vector3(-0.7f,-1f,0f); characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.leftDownAnim, characterAnimation.bodyTypeNumber);}
    //   if (change == Vector3.down)               { change = new Vector3(1f,-0.5f,0f); characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.rightDownAnim, characterAnimation.bodyTypeNumber);}
    //   if (change == Vector3.left)               { change = new Vector3(-0.7f,-1f,0f); characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.leftAnim, characterAnimation.bodyTypeNumber);}


    //   // if (change == Vector3.right+Vector3.up)   { change = new Vector3(0.7f,1f,0f); }
    //   // if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.5f,0f); }
    //   // if (change == Vector3.up)                 { change = new Vector3(-1f,0.5f,0f); }
    //   // if (change == Vector3.right)              { change = new Vector3(0.7f,1f,0f); }
    //   // if (change == Vector3.right+Vector3.down) { change = new Vector3(0.7f,-1f,0f); }
    //   // if (change == Vector3.left+Vector3.down)  { change = new Vector3(-0.7f,-1f,0f); }
    //   // if (change == Vector3.down)               { change = new Vector3(1f,-0.5f,0f); }
    //   // if (change == Vector3.left)               { change = new Vector3(-0.7f,-1f,0f);}
    //   myRigidbody.MovePosition(transform.position + change * movementSpeed * Time.deltaTime);
      
    // }
    // else
    // {
    //   if (change == Vector3.right+Vector3.up)   { change = new Vector3(0.7f,1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upRightAnim, bodyTypeNumber); }
    //   if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upLeftAnim, bodyTypeNumber);}
    //   if (change == Vector3.up)                 { change = new Vector3(-1f,0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upLeftAnim, bodyTypeNumber);}
    //   if (change == Vector3.right)              { change = new Vector3(0.7f,1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upRightAnim, bodyTypeNumber);}
    //   if (change == Vector3.right+Vector3.down) { change = new Vector3(1f,-0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, rightDownAnim, bodyTypeNumber);}
    //   if (change == Vector3.left+Vector3.down)  { change = new Vector3(-0.7f,-1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, leftDownAnim, bodyTypeNumber);}
    //   if (change == Vector3.down)               { change = new Vector3(1f,-0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, rightDownAnim, bodyTypeNumber);}
    //   if (change == Vector3.left)               { change = new Vector3(-0.7f,-1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, leftAnim, bodyTypeNumber);}




    //   // if (change == Vector3.right+Vector3.up)   { change = new Vector3(0.7f,1f,0f); }
    //   // if (change == Vector3.left+Vector3.up)    { change = new Vector3(0f,0f,0f); }
    //   // if (change == Vector3.up)                 { change = new Vector3(0f,0f,0f); }
    //   // if (change == Vector3.right)              { change = new Vector3(0.7f,1f,0f); }
    //   // if (change == Vector3.right+Vector3.down) { change = new Vector3(0f,0f,0f); }
    //   // if (change == Vector3.left+Vector3.down)  { change = new Vector3(-0.7f,-1f,0f); }
    //   // if (change == Vector3.down)               { change = new Vector3(0f,0f,0f); }
    //   // if (change == Vector3.left)               { change = new Vector3(-0.7f,-1f,0f); }
    //   myRigidbody.MovePosition(transform.position + change * movementSpeed * Time.deltaTime);
      
    // }
  }
  void MoveCharacterVerticalInclineLeftToward()
  {
    if (change == Vector3.right+Vector3.up)   { change = new Vector3(1f,-0.2f,0f); }
    if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.2f,0f); }
    if (change == Vector3.up)                 { change = new Vector3(-1f,0.2f,0f); }
    if (change == Vector3.right)              { change = new Vector3(1f,-0.2f,0f); }
    if (change == Vector3.right+Vector3.down) { change = new Vector3(1f,-0.2f,0f); }
    if (change == Vector3.left+Vector3.down)  { change = new Vector3(-1f,0.2f,0f); }
    if (change == Vector3.down)               { change = new Vector3(1f,-0.2f,0f); }
    if (change == Vector3.left)               { change = new Vector3(-1f,0.2f,0f); }
    // AnimateMovement(movementStartIndex, movementFrameCount, ladderAnimDirectionIndex, bodyTypeNumber);
    myRigidbody.MovePosition(transform.position + change * movementSpeed * Time.deltaTime);
      
  }

  void MoveCharacterVerticalInclineRightToward()
  {
    if (change == Vector3.right+Vector3.up)   { change = new Vector3(1f,0.2f,0f); }
    if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,-0.2f,0f); }
    if (change == Vector3.up)                 { change = new Vector3(-1f,-0.2f,0f); }
    if (change == Vector3.right)              { change = new Vector3(1f,0.2f,0f); }
    if (change == Vector3.right+Vector3.down) { change = new Vector3(1f,0.2f,0f); }
    if (change == Vector3.left+Vector3.down)  { change = new Vector3(-1f,-0.2f,0f); }
    if (change == Vector3.down)               { change = new Vector3(1f,0.2f,0f); }
    if (change == Vector3.left)               { change = new Vector3(-1f,-0.2f,0f); }
    // AnimateMovement(movementStartIndex, movementFrameCount, ladderAnimDirectionIndex, bodyTypeNumber);
    myRigidbody.MovePosition(transform.position + change * movementSpeed * Time.deltaTime);
      
  } 
  void MoveCharacterUpDownLadder()
  { 
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(0f,1f,0f);}
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(0f,1f,0f);}
      if (change == Vector3.up)                 { change = new Vector3(0f,1f,0f);}
      if (change == Vector3.right)              { change = new Vector3(0f,1f,0f);}
      if (change == Vector3.right+Vector3.down) { change = new Vector3(0f,-1f,0f);}
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(0f,-1f,0f);}
      if (change == Vector3.down)               { change = new Vector3(0f,-1f,0f);}
      if (change == Vector3.left)               { change = new Vector3(0f,-1f,0f);}
      characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.ladderAnimDirectionIndex, characterAnimation.bodyTypeNumber);
      myRigidbody.MovePosition(transform.position + change * movementSpeed * Time.deltaTime);
  }
  void MoveCharacterUpLadder()
  { 
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(0f,1f,0f);}
      else if (change == Vector3.left+Vector3.up)    { change = new Vector3(0f,1f,0f);}
      else if (change == Vector3.up)                 { change = new Vector3(0f,1f,0f);}
      else if (change == Vector3.right)              { change = new Vector3(0f,1f,0f);}
      else if (change == Vector3.right+Vector3.down) { change = new Vector3(0f,1f,0f);}
      else if (change == Vector3.left+Vector3.down)  { change = new Vector3(0f,1f,0f);}
      else if (change == Vector3.down)               { change = new Vector3(0f,1f,0f);}
      else if (change == Vector3.left)               { change = new Vector3(0f,1f,0f);}
      characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.ladderAnimDirectionIndex, characterAnimation.bodyTypeNumber);
      myRigidbody.MovePosition(transform.position + change * movementSpeed * Time.deltaTime);
  }
  void MoveCharacterDownLadder()
  { 
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(0f,-1f,0f);}
      else if (change == Vector3.left+Vector3.up)    { change = new Vector3(0f,-1f,0f);}
      else if (change == Vector3.up)                 { change = new Vector3(0f,-1f,0f);}
      else if (change == Vector3.right)              { change = new Vector3(0f,-1f,0f);}
      else if (change == Vector3.right+Vector3.down) { change = new Vector3(0f,-1f,0f);}
      else if (change == Vector3.left+Vector3.down)  { change = new Vector3(0f,-1f,0f);}
      else if (change == Vector3.down)               { change = new Vector3(0f,-1f,0f);}
      else if (change == Vector3.left)               { change = new Vector3(0f,-1f,0f);}
      characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.ladderAnimDirectionIndex, characterAnimation.bodyTypeNumber);
      myRigidbody.MovePosition(transform.position + change * movementSpeed * Time.deltaTime);
  }



  public Dictionary<Direction, Direction> controlDirectionToPlayerDirection = new Dictionary<Direction, Direction>(){
    {Direction.Left, Direction.Left},
    {Direction.UpLeft, Direction.UpLeft},
    {Direction.UpFacingLeft, Direction.UpFacingLeft},
    {Direction.UpFacingRight, Direction.UpFacingRight},
    {Direction.UpRight, Direction.UpRight},
    {Direction.Right, Direction.Right},
    {Direction.RightDown, Direction.RightDown},
    {Direction.DownFacingLeft, Direction.DownFacingLeft},
    {Direction.DownFacingRight, Direction.DownFacingRight},
    {Direction.DownLeft, Direction.DownLeft},
    {Direction.Nothing, Direction.Nothing},
  };


  public void ResetPlayerMovement()
  {
    controlDirectionToPlayerDirection[Direction.Left] = Direction.Left;
    controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpLeft;
    controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpFacingLeft;
    controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpFacingRight;
    controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpRight;
    controlDirectionToPlayerDirection[Direction.Right] = Direction.Right;
    controlDirectionToPlayerDirection[Direction.RightDown] = Direction.RightDown;
    controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.DownFacingRight;
    controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.DownFacingLeft;
    controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.DownLeft;
    controlDirectionToPlayerDirection[Direction.Nothing] = Direction.Nothing;
  }

  public void MoveCharacterNormalDirection()
  { 
    // Calculate the angle in degrees (-180 to 180 degrees)
    float angle = Mathf.Atan2(change.y, change.x) * Mathf.Rad2Deg;

    // Adjust by 90 degrees to align 0 degrees with "up"
    angle -= 90f;

    // Debug.Log(playerTouchingCollider);

    // Convert negative angles to positive angles (0 to 360 degrees)
    if (angle < 0) angle += 360;

    if(playerOnBike)
    {
        // Map the angle to movement directions and animations
        if (angle > 0f && angle <= 90f)           { change = new Vector3(-1f,0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upLeftAnim; facingLeft = true; } // Inverted right-up to left-up      
        else if (angle > 90f && angle <= 135f)    { change = new Vector3(-1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.leftAnim; facingLeft = true; } // Inverted right to left
        else if (angle > 135f && angle < 180)    { change = new Vector3(-1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.leftDownAnim; facingLeft = true; } // Inverted right-down to left-down
        else if (angle > 180f && angle <= 225f)   { change = new Vector3(1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.rightDownAnim; facingLeft = false; }  // Inverted left-down to right-down
        else if ((angle == 180f))   
          { 
            if(facingLeft == true)
            {
              change = new Vector3(-1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.leftDownAnim; facingLeft = true;  // Up
            }
            else
            {
              change = new Vector3(1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.rightDownAnim; facingLeft = false; // Up
            }
          }
        else if (angle > 225f && angle <= 270f)   { change = new Vector3(1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.rightAnim; facingLeft = false; }  // Inverted left-down to right-down
        else if ((angle > 270f && angle <= 360f)) { change = new Vector3(1f,0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upRightAnim; facingLeft = false; } // Up
        else if ((angle == 0f))   
                { 
                  if(facingLeft == true)
                  {
                    change = new Vector3(-1f,0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upLeftAnim; facingLeft = true;  // Up
                  }
                  else
                  {
                    change = new Vector3(1f,0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upRightAnim; facingLeft = false; // Up
                  }
                }
        
        else if (angle > 225f && angle <= 270f) { change = new Vector3(1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.rightAnim; facingLeft = false; } // Inverted left to right

        // Handle animation and movement
        characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.currentAnimationDirection, characterAnimation.bodyTypeNumber);
        myRigidbody.MovePosition(transform.position + change * movementSpeed * Time.deltaTime);
    }
    else
    {
        if ((angle > 0f && angle <= 22.5f))   
        { 
          if(currentContactQuadrant == ContactQuadrant.TopRight && playerTouchingCollider == true)
          {
            controlDirection = Direction.UpLeft; 
          }
          else if(currentContactQuadrant == ContactQuadrant.TopLeft && playerTouchingCollider == true)
          {
            controlDirection = Direction.UpRight; 
          }
          else
          {
            controlDirection = Direction.UpFacingLeft;
          }        
        } // Up
        else if ((angle == 0f))   
                {
                  if(facingLeft == true)
                  {
                    // if(movementAutopilot == true)
                    // {
                    //   HandleAutoPilot();
                    // }
                    // else if(activeCollisions.Count > 1 && currentContactQuadrant == ContactQuadrant.TopLeft && playerTouchingCollider == true)
                    // {
                    //   controlDirection = Direction.Left;
                    //   movementAutopilot = true;
                    // }
                    // else 
                    if(currentContactQuadrant == ContactQuadrant.TopRight && playerTouchingCollider == true)
                    {
                      controlDirection = Direction.UpLeft; 
                    }
                    else if(currentContactQuadrant == ContactQuadrant.TopLeft && playerTouchingCollider == true)
                    {
                      controlDirection = Direction.UpRight; 
                    }
                    else
                    {
                      controlDirection = Direction.UpFacingLeft;
                    }
                  }
                  else //facing right
                  {
                    // if(movementAutopilot == true)
                    // {
                    //   HandleAutoPilot();
                    // }
                    // else if(activeCollisions.Count > 1 && currentContactQuadrant ==  ContactQuadrant.TopRight && playerTouchingCollider == true)
                    // {
                    //   controlDirection = Direction.Right;
                    //   movementAutopilot = true;
                    // }
                    // else 
                    if(currentContactQuadrant == ContactQuadrant.TopLeft && playerTouchingCollider == true)
                    {
                      controlDirection = Direction.UpRight; 
                    }
                    else if(currentContactQuadrant == ContactQuadrant.TopRight && playerTouchingCollider == true)
                    {
                      controlDirection = Direction.UpLeft; 
                    }
                    else
                    {
                      controlDirection = Direction.UpFacingRight;
                    }                  
                  }
                }
        
        else if (angle > 22.5f && angle <= 67.5f)   
          { 
            if(currentContactQuadrant == ContactQuadrant.TopLeft && playerTouchingCollider == true && playerOnThresh == false)
            {
              controlDirection = Direction.DownLeft; 
            }
            else
            {
              controlDirection = Direction.UpLeft; 
            }
          } // Inverted right-up to left-up
        else if (angle > 67.5f && angle <= 112.5f)  
          { 
            // if(activeCollisions.Count > 1 
            //   && (currentContactQuadrant ==  ContactQuadrant.TopLeft || currentContactQuadrant ==  ContactQuadrant.BottomLeft)
            //   && playerTouchingCollider == true)
            // {
            //   controlDirection = Direction.Nothing;
            //   movementAutopilot = true;
            // }
            // else 
            if(currentContactQuadrant == ContactQuadrant.TopLeft && playerTouchingCollider == true)
            {
              controlDirection = Direction.DownLeft;
            }
            else if(currentContactQuadrant == ContactQuadrant.BottomLeft && playerTouchingCollider == true)
            {
              controlDirection = Direction.UpLeft;
            }
            else
            {
              controlDirection = Direction.Left; 
            }
          } // Inverted right to left
        else if (angle > 112.5f && angle <= 157.5f) 
        { 
          if(currentContactQuadrant == ContactQuadrant.BottomLeft && playerTouchingCollider == true)
          {
            controlDirection = Direction.UpLeft; 
          }
          else
          {
            controlDirection = Direction.DownLeft; 
          }
        } // Inverted right-down to left-down
        else if (angle > 157.5f && angle < 180f)   
        { 
          if(currentContactQuadrant == ContactQuadrant.BottomRight && playerTouchingCollider == true)
          {
            controlDirection = Direction.DownLeft; 
          }
          else if(currentContactQuadrant == ContactQuadrant.BottomLeft && playerTouchingCollider == true)
          {
            controlDirection = Direction.RightDown; 
          }
          else
          {
            controlDirection = Direction.DownFacingLeft;
          }  
        } // Down
        else if (angle == 180f)  
          {
            if(facingLeft == true)
            {
              // if(activeCollisions.Count > 1 && currentContactQuadrant ==  ContactQuadrant.BottomLeft && playerTouchingCollider == true)
              // {
              //   controlDirection = Direction.Nothing;
              //   movementAutopilot = true;
              // }
              // else 
              if(currentContactQuadrant == ContactQuadrant.BottomRight && playerTouchingCollider == true)
              {
                controlDirection = Direction.DownLeft; 
              }
              else if(currentContactQuadrant == ContactQuadrant.BottomLeft && playerTouchingCollider == true)
              {
                controlDirection = Direction.RightDown; 
              }
              else
              {
                controlDirection = Direction.DownFacingLeft;
              }                  
            }
            else
            {
              // if(activeCollisions.Count > 1 && currentContactQuadrant ==  ContactQuadrant.BottomRight && playerTouchingCollider == true)
              // {
              //   controlDirection = Direction.Nothing;
              //   movementAutopilot = true;
              // }
              // else 
              if(currentContactQuadrant == ContactQuadrant.BottomLeft && playerTouchingCollider == true)
              {
                controlDirection = Direction.RightDown; 
              }
              else if(currentContactQuadrant == ContactQuadrant.BottomRight && playerTouchingCollider == true)
              {
                controlDirection = Direction.DownLeft; 
              }
              else
              {
                controlDirection = Direction.DownFacingRight;
              }                   
            }
          }
        else if (angle > 180f && angle <= 202.5f)   
        { 
          if(currentContactQuadrant == ContactQuadrant.BottomLeft && playerTouchingCollider == true)
          {
            controlDirection = Direction.RightDown; 
          }
          else if(currentContactQuadrant == ContactQuadrant.BottomRight && playerTouchingCollider == true)
          {
            controlDirection = Direction.DownLeft; 
          }
          else
          {
            controlDirection = Direction.DownFacingRight;
          } 
        } // Down
        else if (angle > 202.5f && angle <= 247.5f) 
        { 
          if(currentContactQuadrant == ContactQuadrant.BottomRight && playerTouchingCollider == true)
          {
            controlDirection = Direction.UpRight; 
          }
          else
          {
            controlDirection = Direction.RightDown;
          } 
        }  // Inverted left-down to right-down
        else if (angle > 247.5f && angle <= 292.5f) 
        { 
          // if(activeCollisions.Count > 1 
          //     && (currentContactQuadrant ==  ContactQuadrant.TopRight || currentContactQuadrant ==  ContactQuadrant.BottomRight)
          //     && playerTouchingCollider == true)
          // {
          //   controlDirection = Direction.Nothing;
          //   movementAutopilot = true;
          // }
          // else 
          if(currentContactQuadrant == ContactQuadrant.TopRight && playerTouchingCollider == true)
          {
            controlDirection = Direction.RightDown;
          }
          else if(currentContactQuadrant == ContactQuadrant.BottomRight && playerTouchingCollider == true)
          {
            controlDirection = Direction.UpRight;
          }
          else
          {
            controlDirection = Direction.Right;         
          }
        } // Inverted left to right
        else if (angle > 292.5f && angle <= 337.5f) 
        { 
          if(currentContactQuadrant == ContactQuadrant.TopRight && playerTouchingCollider == true && playerOnThresh == false)
          {
            controlDirection = Direction.RightDown; 
          }
          else
          {
            controlDirection = Direction.UpRight;
          }
        }  // Inverted left-up to right-up
        else if ((angle > 337.5f && angle <= 360f)) 
        { 
          if(currentContactQuadrant == ContactQuadrant.TopLeft && playerTouchingCollider == true)
          {
            controlDirection = Direction.UpRight; 
          }
          else if(currentContactQuadrant == ContactQuadrant.TopRight && playerTouchingCollider == true)
          {
            controlDirection = Direction.UpLeft; 
          }
          else
          {
            controlDirection = Direction.UpFacingRight;
          }                  
        } // Up
        // Map control directions to player directions and animations
        UpdateCharacterDirection(controlDirection);
        // Handle animation and movement
        characterAnimation.Animate(characterAnimation.movementStartIndex, characterAnimation.movementFrameCount, characterAnimation.currentAnimationDirection, characterAnimation.bodyTypeNumber);
        myRigidbody.MovePosition(transform.position + change * movementSpeed * Time.deltaTime);
    };




  } 

  public void UpdateCharacterDirection(Direction controlDirection)
  {
    Direction playerDirection = controlDirectionToPlayerDirection[controlDirection];
    if      (playerDirection == Direction.UpFacingLeft) { change = new Vector3(0f,1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upLeftAnim; facingLeft = true; }
    else if (playerDirection == Direction.UpFacingRight) { change = new Vector3(0f,1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upRightAnim; facingLeft = false; }
    else if (playerDirection == Direction.UpRight) { change = new Vector3(1f,0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upRightAnim; facingLeft = false; }
    else if (playerDirection == Direction.Right) { change = new Vector3(1f,0f,0f); characterAnimation.currentAnimationDirection = characterAnimation.rightAnim; facingLeft = false; }
    else if (playerDirection == Direction.RightDown) { change = new Vector3(1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.rightDownAnim; facingLeft = false; }
    else if (playerDirection == Direction.DownFacingRight) { change = new Vector3(0f,-1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.rightDownAnim; facingLeft = false; }
    else if (playerDirection == Direction.DownFacingLeft) { change = new Vector3(0f,-1f,0f); characterAnimation.currentAnimationDirection = characterAnimation.leftDownAnim; facingLeft = true; }
    else if (playerDirection == Direction.DownLeft) { change = new Vector3(-1f,-0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.leftDownAnim; facingLeft = true; }
    else if (playerDirection == Direction.Left) { change = new Vector3(-1f,0f,0f); characterAnimation.currentAnimationDirection = characterAnimation.leftAnim; facingLeft = true; }
    else if (playerDirection == Direction.UpLeft) { change = new Vector3(-1f,0.5f,0f); characterAnimation.currentAnimationDirection = characterAnimation.upLeftAnim; facingLeft = true; }
    else if (playerDirection == Direction.Nothing) { change = Vector3.zero; }
  }

  public bool IsInputFieldFocused()
  {
      foreach (TMP_InputField inputField in inputFields)
      {
          if (inputField.isFocused)
          {
              return true;
          }
      }
      return false;
  }

  public void SetAlpha(GameObject treeNode, float alpha) // opening cupboards
  {
    SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();

    if(treeNode != transform.Find("bike"))
    {
      if(sr.color.a != 0)
      {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
      }
    }
  }  

  public IEnumerator MoveCharacterRandomly()
  {
    yield return new WaitForSeconds(0.3f);

    change = Vector3.zero;

    // if(!myCharacterMovement.playerOnThresh)
    // {
      yield return new WaitForSeconds(Random.Range(5,7));

      change.x = Random.Range(-10,10);
      change.y = Random.Range(-10,10);

      yield return new WaitForSeconds(Random.Range(1,4));
    // }

    change = Vector3.zero;

    yield return new WaitForSeconds(Random.Range(5,8));

    npcRandomMovementCoro = StartCoroutine(MoveCharacterRandomly()); 
    // npcRandomMovementCoro = null;
  }

  // private void OnCollisionEnter2D(Collision2D collision)
  // {
  //     if (!this.gameObject.CompareTag("Player"))
  //     {
  //         if (playerIsOutside && collision.otherCollider == boxCollider)
  //         {
  //             ReverseDirection(false); // isTrigger is false
  //         }
  //         else
  //         {
  //             change = Vector3.zero;
  //         }
  //     }
  //     else if (this.gameObject.CompareTag("Player"))
  //     {
  //         // Get the player's box collider (child)
  //         BoxCollider2D boxCollider = GetComponentInChildren<BoxCollider2D>();
  //         if (boxCollider == null)
  //         {
  //             Debug.LogError("Player BoxCollider2D not found in child objects!");
  //             return;
  //         }

  //         // Get bounds and contact point relative to the player's box collider
  //         Bounds playerBounds = boxCollider.bounds;
  //         Vector2 contactPoint = collision.GetContact(0).point;

  //         // Determine quadrant based on the player's box collider
  //         currentContactQuadrant = DetermineContactQuadrant(contactPoint, playerBounds);
  //         Debug.Log("Contact Quadrant: " + currentContactQuadrant);

  //         playerTouchingCollider = true;
  //     }
  // }

  // private ContactQuadrant DetermineContactQuadrant(Vector2 contactPoint, Bounds playerBounds)
  // {
  //     // Get the local center of the player's box collider
  //     Vector2 center = playerBounds.center;

  //     // Determine the quadrant in world space
  //     if (contactPoint.x >= center.x && contactPoint.y >= center.y)
  //         return ContactQuadrant.TopRight;
  //     else if (contactPoint.x <= center.x && contactPoint.y >= center.y)
  //         return ContactQuadrant.TopLeft;
  //     else if (contactPoint.x <= center.x && contactPoint.y <= center.y)
  //         return ContactQuadrant.BottomLeft;
  //     else
  //         return ContactQuadrant.BottomRight;
  // }

  //   private HashSet<Collider2D> activeCollisions = new HashSet<Collider2D>(); // Keeps track of active collisions


  // private void OnCollisionEnter2D(Collision2D collision)
  // {
  //     if (!this.gameObject.CompareTag("Player"))
  //     {
  //         if (playerIsOutside && collision.otherCollider == boxCollider)
  //         {
  //             ReverseDirection(false); // isTrigger is false
  //         }
  //         else
  //         {
  //             change = Vector3.zero;
  //         }
  //     }
  //     else if (this.gameObject.CompareTag("Player"))
  //     {
  //         // Get the player's box collider (child)
  //         BoxCollider2D boxCollider = GetComponentInChildren<BoxCollider2D>();

  //         // Get bounds and contact point
  //         Bounds playerBounds = boxCollider.bounds;
  //         Vector2 contactPoint = collision.GetContact(0).point;


  //         // Transform the contact point and bounds center to the local space of the root GameObject
  //         Vector2 localContactPoint = transform.InverseTransformPoint(contactPoint);
  //         Vector2 localCenter = transform.InverseTransformPoint(playerBounds.center);

  //         // Determine quadrant using local space
  //         currentContactQuadrant = DetermineContactQuadrant(localContactPoint, localCenter);
  //         Debug.Log($"Contact Quadrant: {currentContactQuadrant}, Contact Point (local): {localContactPoint}, Center (local): {localCenter}");


  //         activeCollisions.Add(collision.collider); // Add the collider to active collisions


  //         if(playerOnThresh)
  //         {
  //           playerTouchingCollider = false;
  //         }
  //         else
  //         {
  //           playerTouchingCollider = true;
  //         }
  //     }
  // }

  // private ContactQuadrant DetermineContactQuadrant(Vector2 contactPoint, Vector2 center)
  // {
  //     // Determine the quadrant in local space
  //     if (contactPoint.x >= center.x && contactPoint.y >= center.y)
  //         return ContactQuadrant.TopRight;
  //     else if (contactPoint.x <= center.x && contactPoint.y >= center.y)
  //         return ContactQuadrant.TopLeft;
  //     else if (contactPoint.x <= center.x && contactPoint.y <= center.y)
  //         return ContactQuadrant.BottomLeft;
  //     else
  //         return ContactQuadrant.BottomRight;
  // }





  // private void OnCollisionExit2D(Collision2D collision)
  //   {
  //       // Ensure collision is detected by this NPC’s BoxCollider2D and not self-triggered
  //       if (this.gameObject.CompareTag("Player"))
  //       {
  //           activeCollisions.Remove(collision.collider); // Remove the collider from active collisions

  //           // If there are no more active collisions, unlock velocity
  //           if (activeCollisions.Count == 0)
  //           {
  //               playerTouchingCollider = false;
  //           }
  //       }
  //   }

  private HashSet<Collider2D> activeCollisions = new HashSet<Collider2D>(); // Keeps track of active collisions

  private ContactQuadrant lastContactQuadrant = ContactQuadrant.None;

  private void OnCollisionEnter2D(Collision2D collision)
  {
      if (this.gameObject.CompareTag("Player"))
      {
          // Get the player's box collider (child)

          // Get bounds and contact point
          Bounds playerBounds = boxCollider.bounds;
          Vector2 contactPoint = collision.GetContact(0).point;

          // Transform the contact point and bounds center to the local space of the root GameObject
          Vector2 localContactPoint = transform.InverseTransformPoint(contactPoint);
          Vector2 localCenter = transform.InverseTransformPoint(playerBounds.center);

          // Update the lastContactQuadrant
          lastContactQuadrant = currentContactQuadrant;

          // Determine quadrant using local space
          currentContactQuadrant = DetermineContactQuadrant(localContactPoint, localCenter);
          // Debug.Log($"Contact Quadrant: {currentContactQuadrant}, Contact Point (local): {localContactPoint}, Center (local): {localCenter}");

          activeCollisions.Add(collision.collider); // Add the collider to active collisions

          if (playerOnThresh)
          {
              playerTouchingCollider = false;
          }
          else
          {
              playerTouchingCollider = true;
          }
      }
  }

  private void OnCollisionExit2D(Collision2D collision)
  {
      if (this.gameObject.CompareTag("Player"))
      {
          activeCollisions.Remove(collision.collider);

          if (activeCollisions.Count > 0)
          {
              Collider2D remainingCollider = null;
              foreach (var col in activeCollisions)
              {
                  remainingCollider = col;
                  break;
              }

              if (remainingCollider != null)
              {
                  Bounds playerBounds = boxCollider.bounds;
                  Vector2 newContactPoint = remainingCollider.ClosestPoint(playerBounds.center);
                  Vector2 localNewContactPoint = transform.InverseTransformPoint(newContactPoint);
                  Vector2 localCenter = transform.InverseTransformPoint(playerBounds.center);

                  currentContactQuadrant = DetermineContactQuadrant(localNewContactPoint, localCenter);
              }
          }
          else
          {
              // No more active collisions
              // currentContactQuadrant = ContactQuadrant.None;
              playerTouchingCollider = false;
          }
      }
  }


  private ContactQuadrant DetermineContactQuadrant(Vector2 contactPoint, Vector2 center)
  {
      // Determine the quadrant in local space
      if (contactPoint.x >= center.x && contactPoint.y >= center.y)
          return ContactQuadrant.TopRight;
      else if (contactPoint.x <= center.x && contactPoint.y >= center.y)
          return ContactQuadrant.TopLeft;
      else if (contactPoint.x <= center.x && contactPoint.y <= center.y)
          return ContactQuadrant.BottomLeft;
      else
          return ContactQuadrant.BottomRight;
  }


  public void ReverseDirection(bool isTrigger = false)
  {
    // Reverse the direction when a collision occurs
    change *= -1;
    if(isTrigger)
    {
      if(this.npcRandomMovementCoro != null)
      {
          StopCoroutine(this.npcRandomMovementCoro);
      }
      npcRandomMovementCoro = StartCoroutine(MoveCharacterRandomly()); 
      // npcRandomMovementCoro = null;
    }
  }

  public IEnumerator DeactivateSpaceBar()
  {
    while ((Input.GetKey(KeyCode.Space) || 
          Input.GetKey(KeyCode.JoystickButton0) ||  // A button
          Input.GetKey(KeyCode.JoystickButton1) ||  // B button
          Input.GetKey(KeyCode.JoystickButton2) ||  // X button
          Input.GetKey(KeyCode.JoystickButton3)))
    {
      spaceBarDeactivated = true;

      yield return null;
    }
      spaceBarDeactivated = false;
  }

  public void StartDeactivateSpaceBar()
  {
      if (allowTimeForSpaceBarCoro != null)
      {
          StopCoroutine(allowTimeForSpaceBarCoro);
      }
      allowTimeForSpaceBarCoro = StartCoroutine(DeactivateSpaceBar());
  }

  public void StopDeactivateSpaceBar()
  {
      if (allowTimeForSpaceBarCoro != null)
      {
          StopCoroutine(allowTimeForSpaceBarCoro);
          allowTimeForSpaceBarCoro = null;
          spaceBarDeactivated = false;
      }
  }

  Color HexToColor(string hex)
  {
      Color newCol;
      if (ColorUtility.TryParseHtmlString(hex, out newCol))
      {
          return newCol;
      }
      else
      {
          Debug.LogError("Invalid hex color string: " + hex);
          return Color.black; // Return black if conversion fails
      }
  }  

  IEnumerator LateStartSpawnInsideCoro()
  {
      // Wait until the end of the current frame
      yield return new WaitForEndOfFrame();
      // SetAsChild();

  }

  // public void SetAsChild()
  //   {
  //       if (roomToSpawnIn != null)
  //       {
  //           // Set this GameObject as a child of the specified parent
  //           transform.SetParent(roomToSpawnIn.transform);
  //           Debug.Log($"{gameObject.name} is now a child of {roomToSpawnIn.name}");
  //       }
  //   }


  public static void SetTreeSortingLayer(GameObject gameObject, string sortingLayerName)
  {
      if(gameObject.GetComponent<SpriteRenderer>() != null) {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
      }
      foreach (Transform child in gameObject.transform)
      {
          CharacterMovement.SetTreeSortingLayer(child.gameObject, sortingLayerName);
      }
  }
  private Vector2 previousJoystickDirection;
  public void HandleMovementReactivation()
  {
    // Detect keyboard input
    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || 
        Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
        Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || 
        Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
    {
        movementAutopilot = false;
    }

    // Detect joystick direction change
    Vector2 currentJoystickDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    if (currentJoystickDirection != previousJoystickDirection && currentJoystickDirection.magnitude > 0.1f)
    {
      movementAutopilot = false;
    }

    // Update the previous joystick direction
    previousJoystickDirection = currentJoystickDirection; 
  }
  public void HandleSpaceBarReactivation()
  {
    if ((Input.GetKeyUp(KeyCode.Space) || 
        Input.GetKeyUp(KeyCode.JoystickButton0) ||  // A button
        Input.GetKeyUp(KeyCode.JoystickButton1) ||  // B button
        Input.GetKeyUp(KeyCode.JoystickButton2) ||  // X button
        Input.GetKeyUp(KeyCode.JoystickButton3)))
    {
      spaceBarDeactivated = false;   
    }
  }

  // public void HandleAutoPilot()
  // {
  //   Debug.Log("handling");
  //   if(activeCollisions.Count > 0)
  //   {
  //     // Direction controlDirection = Direction.Nothing; // Default value should never be used
  //     if(controlDirection == Direction.Left)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.TopLeft)
  //             {
  //               controlDirection = Direction.DownLeft;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.BottomLeft)
  //             {
  //               controlDirection = Direction.UpLeft;
  //             }
  //     }
  //     else if (controlDirection == Direction.DownLeft)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.TopLeft)
  //             {
  //               controlDirection = Direction.DownLeft;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.BottomLeft)
  //             {
  //               controlDirection = Direction.Left;
  //             }
  //     }
  //     else if (controlDirection == Direction.DownFacingLeft)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.BottomLeft)
  //             {
  //               controlDirection = Direction.RightDown;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.BottomRight)
  //             {
  //               controlDirection = Direction.DownLeft;
  //             }
  //     }
  //     else if (controlDirection == Direction.DownFacingRight)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.BottomLeft)
  //             {
  //               controlDirection = Direction.RightDown;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.BottomRight)
  //             {
  //               controlDirection = Direction.DownLeft;
  //             }
  //     }
  //     else if (controlDirection == Direction.RightDown)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.BottomRight)
  //             {
  //               controlDirection = Direction.Right;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.TopRight)
  //             {
  //               controlDirection = Direction.UpFacingRight;
  //             }
  //     }
  //     else if (controlDirection == Direction.Right)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.BottomRight)
  //             {
  //               controlDirection = Direction.UpRight;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.TopRight)
  //             {
  //               controlDirection = Direction.RightDown;
  //             }
  //     }
  //     else if (controlDirection == Direction.UpRight)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.TopRight)
  //             {
  //               controlDirection = Direction.UpFacingRight;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.TopLeft)
  //             {
  //               controlDirection = Direction.Left;
  //             }
  //     }
  //     else if (controlDirection == Direction.UpFacingRight)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.TopRight)
  //             {
  //               controlDirection = Direction.UpFacingLeft;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.TopLeft)
  //             {
  //               controlDirection = Direction.UpLeft;
  //             }
  //     }
  //     else if (controlDirection == Direction.UpFacingLeft)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.TopRight)
  //             {
  //               controlDirection = Direction.UpLeft;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.TopLeft)
  //             {
  //               controlDirection = Direction.UpFacingRight;
  //             }
  //     }
  //     else if (controlDirection == Direction.UpLeft)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.TopRight)
  //             {
  //               controlDirection = Direction.UpLeft;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.TopLeft)
  //             {
  //               controlDirection = Direction.UpFacingRight;
  //             }
  //     }
  //   }
  //   else if(activeCollisions.Count < 1)
  //   {
  //     Debug.Log(currentContactQuadrant);
  //     if(controlDirection == Direction.Left)
  //     {
  //             controlDirection = Direction.UpLeft;
  //     }
  //     else 
  //     if (controlDirection == Direction.DownLeft)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.TopLeft)
  //             {
  //               controlDirection = Direction.Left;
  //             }
  //             else if(lastContactQuadrant == ContactQuadrant.BottomRight)
  //             {

  //             }
  //     }
  //     // else if (controlDirection == Direction.DownFacingLeft)
  //     // {
  //     //         if(currentContactQuadrant == ContactQuadrant.BottomLeft)
  //     //         {
  //     //           controlDirection = Direction.RightDown;
  //     //         }
  //     //         else if(currentContactQuadrant == ContactQuadrant.BottomRight)
  //     //         {
  //     //           controlDirection = Direction.DownLeft;
  //     //         }
  //     // }
  //     // else if (controlDirection == Direction.DownFacingRight)
  //     // {
  //     //         if(currentContactQuadrant == ContactQuadrant.BottomLeft)
  //     //         {
  //     //           controlDirection = Direction.RightDown;
  //     //         }
  //     //         else if(currentContactQuadrant == ContactQuadrant.BottomRight)
  //     //         {
  //     //           controlDirection = Direction.DownLeft;
  //     //         }
  //     // }
  //     else if (controlDirection == Direction.RightDown)
  //     { 
  //             // if(lastContactQuadrant == ContactQuadrant.)
  //               controlDirection = Direction.UpRight;
  //     }
  //     else if (controlDirection == Direction.Right)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.BottomRight)
  //             {
  //               controlDirection = Direction.UpRight;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.TopRight)
  //             {
  //               controlDirection = Direction.RightDown;
  //             }
  //     }
  //     else if (controlDirection == Direction.UpRight)
  //     {
  //             // controlDirection = Direction.
  //     }
  //     else if (controlDirection == Direction.UpFacingRight)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.TopRight)
  //             {
  //               controlDirection = Direction.UpFacingLeft;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.TopLeft)
  //             {
  //               controlDirection = Direction.UpLeft;
  //             }
  //     }
  //     else if (controlDirection == Direction.UpFacingLeft)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.TopRight)
  //             {
  //               controlDirection = Direction.UpLeft;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.TopLeft)
  //             {
  //               controlDirection = Direction.UpFacingRight;
  //             }
  //     }
  //     else if (controlDirection == Direction.UpLeft)
  //     {
  //             if(currentContactQuadrant == ContactQuadrant.TopRight)
  //             {
  //               controlDirection = Direction.UpFacingLeft;
  //             }
  //             else if(currentContactQuadrant == ContactQuadrant.TopLeft)
  //             {
  //               controlDirection = Direction.UpFacingRight;
  //             }
  //     }
  //   }
  // }
  
}

