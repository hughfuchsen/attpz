﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
 [SerializeField] GameObject Player;
  public List<GameObject> playerSpriteList = new List<GameObject>();
  public List<Color> initialPlayerColorList = new List<Color>();

  CameraMovement cameraMovement;
  public int speed;
  private int initialMovementSpeed;
  private float initialAnimationSpeed;
  public Rigidbody2D myRigidbody; 

  public string motionDirection = "normal";
  public Vector3 change;
  // public Animator animator;


  // private Vector2 initialBCOffset;
  // private Vector2 boxCol2DRunLeftOffset;



  private Vector3 targetPosition;
  // private bool isMovingToClick = false;

  private bool isMoving;
  private bool isRunning;

  public bool playerOnThresh = false;

  IsoSpriteSorting IsoSpriteSorting; 

  public bool fixedDirectionLeft;
  public bool fixedDirectionRight;
  
  public bool playerIsOutside = false;


  private int animDirectionAdjustorInt = 26;

  public int rightDownAnim;
  public int leftDownAnim;
  public int rightAnim;
  public int leftAnim;
  public int upRightAnim;
  public int upLeftAnim;
  public int ladderAnimDirectionIndex;
  private int idle = 0;
  private int walk = 1;
  public int run = 5;
  private int sit = 9;
  private int climb = 11;
  public int rideBike = 15;
  



  private Sprite[] allHeadSprites; // Array to hold all 156 sprites from the sprite sheet VV
  private Sprite[] allEyeSprites; 
  private Sprite[] allThroatSprites; 
  private Sprite[] allCollarSprites;
  private Sprite[] allTorsoSprites;
  private Sprite[] allWaistSprites; 
  private Sprite[] allWaistShortsSprites; 
  private Sprite[] allKneesShinsSprites; 
  private Sprite[] allAnklesSprites; 
  private Sprite[] allFeetSprites; 
  private Sprite[] allJakettoSprites; 
  private Sprite[] allLongSleeveSprites; 
  private Sprite[] allHandSprites; 
  private Sprite[] allShortSleeveSprites; 
  private Sprite[] allMohawk5TopSprites; 
  private Sprite[] allMohawk5BottomSprites; 
  private Sprite[] allHair0TopSprites; 
  private Sprite[] allHair0BottomSprites; 
  private Sprite[] allHair1TopSprites; 
  private Sprite[] allHair7TopSprites; 
  private Sprite[] allHair8TopSprites; 
  private Sprite[] allHair1BottomSprites; 
  private Sprite[] allHair2BottomSprites; 
  private Sprite[] allHair3BottomSprites; 
  private Sprite[] allHair4BottomSprites; 
  private Sprite[] allHair6BottomSprites; 
  private Sprite[] allHair7BottomSprites; 
  private Sprite[] allHair8BottomSprites; 
  private Sprite[] allHairFringe1Sprites; 
  private Sprite[] allHairFringe2Sprites; 
  private Sprite[] allBikeSprites; 

  public float animationSpeed = 0.09f; // Time between frames
  private int movementDirection;

  private SpriteRenderer headSprite;
  private SpriteRenderer eyeSprite;
  private SpriteRenderer throatSprite;
  private SpriteRenderer collarSprite;
  private SpriteRenderer torsoSprite;
  private SpriteRenderer waistSprite;
  private SpriteRenderer waistShortsSprite;
  private SpriteRenderer kneesShinsSprite;
  private SpriteRenderer anklesSprite;
  private SpriteRenderer feetSprite;
  private SpriteRenderer jakettoSprite;
  private SpriteRenderer longSleeveSprite;
  private SpriteRenderer handSprite;
  private SpriteRenderer shortSleeveSprite;
  private SpriteRenderer mohawk5TopSprite;
  private SpriteRenderer mohawk5BottomSprite;
  private SpriteRenderer hair0TopSprite;
  private SpriteRenderer hair0BottomSprite;
  private SpriteRenderer hair1TopSprite;
  private SpriteRenderer hair7TopSprite;
  private SpriteRenderer hair8TopSprite;
  private SpriteRenderer hair1BottomSprite;
  private SpriteRenderer hair2BottomSprite;
  private SpriteRenderer hair3BottomSprite;
  private SpriteRenderer hair4BottomSprite;
  private SpriteRenderer hair6BottomSprite;
  private SpriteRenderer hair7BottomSprite;
  private SpriteRenderer hair8BottomSprite;
  private SpriteRenderer hairFringe1Sprite;
  private SpriteRenderer hairFringe2Sprite;


  BikeTransformAdjustment bikeTransformAdjustment;
  BikeScript bikeScript;

  [SerializeField] GameObject bikeStatic;
  private SpriteRenderer bikeSprite;
  public bool playerOnBike;
  private Color currentBikeColor;
  private Color bikeInitialColor;

  public bool spaceBarDeactivated;

  public Coroutine allowTimeForSpaceBarCoro;


  public bool playerSitting = false;


  private float timer;
  private int currentFrame;
  public int movementStartIndex;
  private int movementFrameCount;
  private int[] movementIndices;

  public int currentAnimationDirection;

  public int bodyTypeNumber;
  private int bodyTypeIndexMultiplier = 156;

  CharacterCustomization characterCustomization;

  public List<Color> skinColors = new List<Color>();
  public Color currentSkinColor;
  public Color currentHairColor;
  public Color currentShirtColor;
  public Color currentPantsColor;
  public Color currentShoeColor;
  public Color currentJakettoColor;






  public bool playerIsInside()
  {
    if (!playerIsOutside)
    {
      return true;
    }
    return false;
  }

  // Start is called before the first frame update
  void Start()    
    {
      Player = GameObject.FindGameObjectWithTag("Player");
      cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
      GetSpritesAndAddToLists(Player, playerSpriteList, initialPlayerColorList);
      IsoSpriteSorting = GetComponent<IsoSpriteSorting>();
      // animator = GetComponent<Animator>();
      myRigidbody = GetComponent<Rigidbody2D>();
      fixedDirectionLeft = false;
      fixedDirectionRight = false;

      characterCustomization = GameObject.FindGameObjectWithTag("CharacterCustomizationMenu").GetComponent<CharacterCustomization>();
      bikeTransformAdjustment = Player.GetComponent<BikeTransformAdjustment>();


      // initialBCOffset = Player.GetComponent<BoxCollider2D>().offset;
      // boxCol2DRunLeftOffset = new Vector2(0.5f,0f);

      initialMovementSpeed = speed;
      initialAnimationSpeed = animationSpeed;

      movementFrameCount = 4;
      // animation directions
      rightDownAnim = 0 * animDirectionAdjustorInt;
      leftDownAnim  = 1 * animDirectionAdjustorInt;
      rightAnim     = 2 * animDirectionAdjustorInt;
      leftAnim      = 3 * animDirectionAdjustorInt;
      upRightAnim   = 4 * animDirectionAdjustorInt;
      upLeftAnim    = 5 * animDirectionAdjustorInt;
      
      bodyTypeNumber = 5;
        
      allHeadSprites = Resources.LoadAll<Sprite>("head");
      allEyeSprites = Resources.LoadAll<Sprite>("eyes");
      allThroatSprites = Resources.LoadAll<Sprite>("throat");
      allCollarSprites = Resources.LoadAll<Sprite>("collar");
      allTorsoSprites = Resources.LoadAll<Sprite>("torso");
      allWaistSprites = Resources.LoadAll<Sprite>("waist");
      allWaistShortsSprites = Resources.LoadAll<Sprite>("waistShorts");
      allKneesShinsSprites = Resources.LoadAll<Sprite>("kneesShins");
      allAnklesSprites = Resources.LoadAll<Sprite>("ankles");
      allFeetSprites = Resources.LoadAll<Sprite>("feet");
      allJakettoSprites = Resources.LoadAll<Sprite>("jaketto");
      allLongSleeveSprites = Resources.LoadAll<Sprite>("longSleeve");
      allHandSprites = Resources.LoadAll<Sprite>("hands");
      allShortSleeveSprites = Resources.LoadAll<Sprite>("shortSleeve");
      allMohawk5TopSprites = Resources.LoadAll<Sprite>("mohawk5Top");
      allMohawk5BottomSprites = Resources.LoadAll<Sprite>("mohawk5Bottom");
      allHair0TopSprites = Resources.LoadAll<Sprite>("hair0Top");
      allHair0BottomSprites = Resources.LoadAll<Sprite>("hair0Bottom");
      allHair1TopSprites = Resources.LoadAll<Sprite>("hair1Top");
      allHair7TopSprites = Resources.LoadAll<Sprite>("hair7Top");
      allHair8TopSprites = Resources.LoadAll<Sprite>("hair8Top");
      allHair1BottomSprites = Resources.LoadAll<Sprite>("hair1Bottom");
      allHair2BottomSprites = Resources.LoadAll<Sprite>("hair2Bottom");
      allHair3BottomSprites = Resources.LoadAll<Sprite>("hair3Bottom");
      allHair4BottomSprites = Resources.LoadAll<Sprite>("hair4Bottom");
      allHair6BottomSprites = Resources.LoadAll<Sprite>("hair6Bottom");
      allHair7BottomSprites = Resources.LoadAll<Sprite>("hair7Bottom");
      allHair8BottomSprites = Resources.LoadAll<Sprite>("hair8Bottom");
      allHairFringe1Sprites = Resources.LoadAll<Sprite>("hairFringe1");
      allHairFringe2Sprites = Resources.LoadAll<Sprite>("hairFringe2");
      allBikeSprites = Resources.LoadAll<Sprite>("bike");


      headSprite = transform.Find("head").GetComponent<SpriteRenderer>();
      eyeSprite = transform.Find("eyes").GetComponent<SpriteRenderer>();
      throatSprite = transform.Find("throat").GetComponent<SpriteRenderer>();
      collarSprite = transform.Find("collar").GetComponent<SpriteRenderer>();
      torsoSprite = transform.Find("torso").GetComponent<SpriteRenderer>();
      waistSprite = transform.Find("waist").GetComponent<SpriteRenderer>();
      waistShortsSprite = transform.Find("waistShorts").GetComponent<SpriteRenderer>();
      kneesShinsSprite = transform.Find("kneesShins").GetComponent<SpriteRenderer>();
      anklesSprite = transform.Find("ankles").GetComponent<SpriteRenderer>();
      feetSprite = transform.Find("feet").GetComponent<SpriteRenderer>();
      jakettoSprite = transform.Find("jaketto").GetComponent<SpriteRenderer>();
      longSleeveSprite = transform.Find("longSleeve").GetComponent<SpriteRenderer>();
      handSprite = transform.Find("hands").GetComponent<SpriteRenderer>();
      shortSleeveSprite = transform.Find("shortSleeve").GetComponent<SpriteRenderer>();
      mohawk5TopSprite = transform.Find("hair").transform.Find("mohawk5Top").GetComponent<SpriteRenderer>();
      mohawk5BottomSprite = transform.Find("hair").transform.Find("mohawk5Bottom").GetComponent<SpriteRenderer>();
      hair0TopSprite = transform.Find("hair").transform.Find("hair0Top").GetComponent<SpriteRenderer>();
      hair0BottomSprite = transform.Find("hair").transform.Find("hair0Bottom").GetComponent<SpriteRenderer>();
      hair1TopSprite = transform.Find("hair").transform.Find("hair1Top").GetComponent<SpriteRenderer>();
      hair7TopSprite = transform.Find("hair").transform.Find("hair7Top").GetComponent<SpriteRenderer>();
      hair8TopSprite = transform.Find("hair").transform.Find("hair8Top").GetComponent<SpriteRenderer>();
      hair1BottomSprite = transform.Find("hair").transform.Find("hair1Bottom").GetComponent<SpriteRenderer>();
      hair2BottomSprite = transform.Find("hair").transform.Find("hair2Bottom").GetComponent<SpriteRenderer>();
      hair3BottomSprite = transform.Find("hair").transform.Find("hair3Bottom").GetComponent<SpriteRenderer>();
      hair4BottomSprite = transform.Find("hair").transform.Find("hair4Bottom").GetComponent<SpriteRenderer>();
      hair6BottomSprite = transform.Find("hair").transform.Find("hair6Bottom").GetComponent<SpriteRenderer>();
      hair7BottomSprite = transform.Find("hair").transform.Find("hair7Bottom").GetComponent<SpriteRenderer>();
      hair8BottomSprite = transform.Find("hair").transform.Find("hair8Bottom").GetComponent<SpriteRenderer>();
      hairFringe1Sprite = transform.Find("hair").transform.Find("hairFringe1").GetComponent<SpriteRenderer>();
      hairFringe2Sprite = transform.Find("hair").transform.Find("hairFringe2").GetComponent<SpriteRenderer>();
      bikeSprite = transform.Find("bike").GetComponent<SpriteRenderer>();

      bikeStatic = GameObject.FindGameObjectWithTag("Bike");
      bikeScript = bikeStatic.transform.Find("bikeTrig").GetComponent<BikeScript>();
      bikeInitialColor = bikeSprite.color;
      currentBikeColor = bikeSprite.color;
      currentBikeColor.a = 0f;



      // Set the initial state to idle left using the idleLeftIndex
      headSprite.sprite = allHeadSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      eyeSprite.sprite = allEyeSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      throatSprite.sprite = allThroatSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      collarSprite.sprite = allCollarSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      torsoSprite.sprite = allTorsoSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      waistSprite.sprite = allWaistSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      waistShortsSprite.sprite = allWaistShortsSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      kneesShinsSprite.sprite = allKneesShinsSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      anklesSprite.sprite = allAnklesSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      feetSprite.sprite = allFeetSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      jakettoSprite.sprite = allJakettoSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      longSleeveSprite.sprite = allLongSleeveSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      handSprite.sprite = allHandSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
      shortSleeveSprite.sprite = allShortSleeveSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      mohawk5TopSprite.sprite = allMohawk5TopSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      mohawk5BottomSprite.sprite = allMohawk5BottomSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hair0TopSprite.sprite = allHair0TopSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hair0BottomSprite.sprite = allHair0BottomSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hair1TopSprite.sprite = allHair1TopSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hair7TopSprite.sprite = allHair7TopSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hair8TopSprite.sprite = allHair8TopSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hair1BottomSprite.sprite = allHair1BottomSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hair2BottomSprite.sprite = allHair2BottomSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hair3BottomSprite.sprite = allHair3BottomSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hair4BottomSprite.sprite = allHair4BottomSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hair6BottomSprite.sprite = allHair6BottomSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hair7BottomSprite.sprite = allHair7BottomSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hair8BottomSprite.sprite = allHair8BottomSprites[bodyTypeNumber * bodyTypeIndexMultiplier];      
      hairFringe1Sprite.sprite = allHairFringe1Sprites[bodyTypeNumber * bodyTypeIndexMultiplier];  
      hairFringe2Sprite.sprite = allHairFringe2Sprites[bodyTypeNumber * bodyTypeIndexMultiplier];  



      currentSkinColor = (HexToColor("#F5CBA7"));
      currentShirtColor = (HexToColor("#aaaaaa"));
      currentPantsColor = (HexToColor("#00B6FF"));
      currentShoeColor = (HexToColor("#AB4918"));
      currentJakettoColor = (HexToColor("#AB4918"));
      // SetShirtToSinglet1();  // start w. singlet??
      // SetPantsToShorts();
      // SetFeetToBareFoot();
      // SetHairColor1();
      // SetHairStyle13();
      // characterCustomization.currentShirtIndex = 1; // start w. singlet??
      // characterCustomization.currentPantsIndex = 1; // start w. singlet??
      // characterCustomization.currentFeetIndex = 1; // start w. singlet??

      characterCustomization.UpdateRandom();


    }

  // Update is called once per frame
  public void Update()
  {
      // // Check if space is pressed
      // if (Input.GetKeyDown(KeyCode.Space))
      // {
      //     spaceBarDeactivated = true;
      // }

      // Check if space is released
      if (Input.GetKeyUp(KeyCode.Space))
      {
          spaceBarDeactivated = false;
      }
  }
  void FixedUpdate()
    {
      UpdateAnimationAndMove(); 
    }

public void AnimateMovement(int movementStartIndex, int movementFrameCount, int animationDirection, int bodyTypeNumber)
{
    bikeSprite.color = currentBikeColor;
    // if (Input.GetKey(KeyCode.B) && !playerOnThresh && playerIsOutside && change != Vector3.zero)
    if (playerOnBike == true)
    {
      //RideBike
      RideBike();
    }
    else if(playerSitting == true)
    {
      Sit();
    }
    else if (motionDirection == "upDownLadder"|| motionDirection == "upLadder" || motionDirection == "downLadder")
    {
      ClimbLadder();
    }
    else if (Input.GetKey(KeyCode.Space) && !playerOnThresh && playerIsOutside && change != Vector3.zero)
    {
      Run();
    }
    else if (change != Vector3.zero)
    {
      Walk();
    }


    currentAnimationDirection = animationDirection;

    // Check if space was released before this code
    if(spaceBarDeactivated == false)
    {
        if (Input.GetKey(KeyCode.Space) && playerIsOutside && playerOnBike)
        {
            // StopBikeFunction
            StartDeactivateSpaceBar();
            playerOnBike = false;
            bikeScript.GetOffBoike();
            bikeStatic.transform.position = Player.transform.position + new Vector3(8, -22, 0);
            currentBikeColor.a = 0f;
            // StartDeactivateSpaceBar();

            // movementStartIndex = idle;
        }
    }
    

    if(playerOnThresh && playerOnBike == true)
    {
        //StopBikeFunctionWhile Entering Building

      if(change.x > 0 && change.y > 0)//up right
      {  
        bikeStatic.transform.position = Player.transform.position + new Vector3(8,-22,0) + new Vector3(-16,-8,0);
      }
      if(change.x < 0 && change.y < 0)//down left
      {  
        bikeStatic.transform.position = Player.transform.position + new Vector3(8,-22,0) + new Vector3(16,8,0);
      }
      if(change.x < 0 && change.y > 0)//up left
      {  
        bikeStatic.transform.position = Player.transform.position + new Vector3(8,-22,0) + new Vector3(16,-8,0);
      }
      if(change.x > 0 && change.y < 0)// down right
      {  
        bikeStatic.transform.position = Player.transform.position + new Vector3(8,-22,0) + new Vector3(-16,8,0);
      }
      
      playerOnBike = false;
      bikeScript.GetOffBoike();
      currentBikeColor.a = 0f;
    }

    movementIndices = Enumerable.Range(movementStartIndex + animationDirection + ((bodyTypeNumber - 1) * bodyTypeIndexMultiplier), movementFrameCount).ToArray();
    // Timer to control the animation frame rate
    timer += Time.deltaTime;





    // If enough time has passed, move to the next frame
    if (timer >= animationSpeed)
    {
        timer = 0f; // Reset timer

        // Update the current frame
        currentFrame++;

        // If we've reached the end of the movementIndices array, loop back to the first sprite
        if (currentFrame >= movementIndices.Length)
        {
            currentFrame = 0;
        }

        // Set the sprite to the current frame in the movementIndices array
        headSprite.sprite = allHeadSprites[movementIndices[currentFrame]];
        eyeSprite.sprite = allEyeSprites[movementIndices[currentFrame]];
        throatSprite.sprite = allThroatSprites[movementIndices[currentFrame]];
        collarSprite.sprite = allCollarSprites[movementIndices[currentFrame]];
        torsoSprite.sprite = allTorsoSprites[movementIndices[currentFrame]];
        waistSprite.sprite = allWaistSprites[movementIndices[currentFrame]];
        waistShortsSprite.sprite = allWaistShortsSprites[movementIndices[currentFrame]];
        kneesShinsSprite.sprite = allKneesShinsSprites[movementIndices[currentFrame]];
        anklesSprite.sprite = allAnklesSprites[movementIndices[currentFrame]];
        feetSprite.sprite = allFeetSprites[movementIndices[currentFrame]];
        jakettoSprite.sprite = allJakettoSprites[movementIndices[currentFrame]];
        longSleeveSprite.sprite = allLongSleeveSprites[movementIndices[currentFrame]];
        handSprite.sprite = allHandSprites[movementIndices[currentFrame]];
        shortSleeveSprite.sprite = allShortSleeveSprites[movementIndices[currentFrame]];
        mohawk5TopSprite.sprite = allMohawk5TopSprites[movementIndices[currentFrame]];
        mohawk5BottomSprite.sprite = allMohawk5BottomSprites[movementIndices[currentFrame]];
        hair0TopSprite.sprite = allHair0TopSprites[movementIndices[currentFrame]];
        hair0BottomSprite.sprite = allHair0BottomSprites[movementIndices[currentFrame]];
        hair1TopSprite.sprite = allHair1TopSprites[movementIndices[currentFrame]];
        hair7TopSprite.sprite = allHair7TopSprites[movementIndices[currentFrame]];
        hair8TopSprite.sprite = allHair8TopSprites[movementIndices[currentFrame]];
        hair1BottomSprite.sprite = allHair1BottomSprites[movementIndices[currentFrame]];
        hair2BottomSprite.sprite = allHair2BottomSprites[movementIndices[currentFrame]];
        hair3BottomSprite.sprite = allHair3BottomSprites[movementIndices[currentFrame]];
        hair4BottomSprite.sprite = allHair4BottomSprites[movementIndices[currentFrame]];
        hair6BottomSprite.sprite = allHair6BottomSprites[movementIndices[currentFrame]];
        hair7BottomSprite.sprite = allHair7BottomSprites[movementIndices[currentFrame]];
        hair8BottomSprite.sprite = allHair8BottomSprites[movementIndices[currentFrame]];
        hairFringe1Sprite.sprite = allHairFringe1Sprites[movementIndices[currentFrame]];
        hairFringe2Sprite.sprite = allHairFringe2Sprites[movementIndices[currentFrame]];
    }
}


  public void UpdateAnimationAndMove()
    {

        change = Vector3.zero;
        if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D))
        {
          change.x = Input.GetAxisRaw("LeftSideHoriz1");
          change.y = Input.GetAxisRaw("LeftSideVert1");
        }
        else
        {
          change.x = Input.GetAxisRaw("Horizontal");
          change.y = Input.GetAxisRaw("Vertical");
        }
      

      if(change != Vector3.zero)
      {
        if(motionDirection == "normal") {
            MoveCharacter();} 
        else if (motionDirection == "inclineLeftAway") {
            MoveCharacterVerticalInclineLeftAway();} 
        else if (motionDirection == "inclineRightAway") 
          {MoveCharacterVerticalInclineRightAway();}
        else if (motionDirection == "inclineLeftToward") 
          {MoveCharacterVerticalInclineLeftToward();}
        else if (motionDirection == "inclineRightToward") 
          {MoveCharacterVerticalInclineRightToward();}
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
        if(playerSitting == true)
        {
          // AnimateMovement(sit, 1, currentAnimationDirection, bodyTypeNumber);
        }
        else if(!playerOnBike) // not optimal
        {
          AnimateMovement(idle, 1, currentAnimationDirection, bodyTypeNumber);
        }
        else //  if player on boike
        {
          // if(currentAnimationDirection == upRightAnim)
          // {}
          AnimateMovement(rideBike, 1, currentAnimationDirection, bodyTypeNumber);
        }
      }
    }
      void MoveCharacterVerticalInclineLeftAway()
  {
    if(!fixedDirectionLeft)
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(1f,0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upRightAnim, bodyTypeNumber); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(-0.7f,1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upLeftAnim, bodyTypeNumber);}
      if (change == Vector3.up)                 { change = new Vector3(-0.7f,1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upLeftAnim, bodyTypeNumber);}
      if (change == Vector3.right)              { change = new Vector3(1f,0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upRightAnim, bodyTypeNumber);}
      if (change == Vector3.right+Vector3.down) { change = new Vector3(0.7f,-1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, rightDownAnim, bodyTypeNumber);}
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(-1f,-0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, leftDownAnim, bodyTypeNumber);}
      if (change == Vector3.down)               { change = new Vector3(0.7f,-1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, rightDownAnim, bodyTypeNumber);}
      if (change == Vector3.left)               { change = new Vector3(-1f,-0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, leftAnim, bodyTypeNumber);}
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      
    }
    else
    // fixedDirectionLeft
    {    
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(-1f,0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upRightAnim, bodyTypeNumber); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(-0.7f,1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upLeftAnim, bodyTypeNumber);}
      if (change == Vector3.up)                 { change = new Vector3(-0.7f,1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upLeftAnim, bodyTypeNumber);}
      if (change == Vector3.right)              { change = new Vector3(-1f,0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upRightAnim, bodyTypeNumber);}
      if (change == Vector3.right+Vector3.down) { change = new Vector3(0.7f,-1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, rightDownAnim, bodyTypeNumber);}
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(1f,-0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, leftDownAnim, bodyTypeNumber);}
      if (change == Vector3.down)               { change = new Vector3(0.7f,-1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, rightDownAnim, bodyTypeNumber);}
      if (change == Vector3.left)               { change = new Vector3(1f,-0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, leftAnim, bodyTypeNumber);}


      // if (change == Vector3.right+Vector3.up)   { change = new Vector3(-1f,0.5f,0f); }
      // if (change == Vector3.left+Vector3.up)    { change = new Vector3(-0.7f,1f,0f); }
      // if (change == Vector3.up)                 { change = new Vector3(-0.7f,1f,0f); }
      // if (change == Vector3.right)              { change = new Vector3(-1f,0.5f,0f); }
      // if (change == Vector3.right+Vector3.down) { change = new Vector3(0.7f,-1f,0f); }
      // if (change == Vector3.left+Vector3.down)  { change = new Vector3(1f,-0.5f,0f); }
      // if (change == Vector3.down)               { change = new Vector3(0.7f,-1f,0f); }
      // if (change == Vector3.left)               { change = new Vector3(1f,-0.5f,0f); }
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      
    } 
  }

  void MoveCharacterVerticalInclineRightAway()
  {
    if(!fixedDirectionRight)
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(0.7f,1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upRightAnim, bodyTypeNumber); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upLeftAnim, bodyTypeNumber);}
      if (change == Vector3.up)                 { change = new Vector3(-1f,0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upLeftAnim, bodyTypeNumber);}
      if (change == Vector3.right)              { change = new Vector3(0.7f,1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upRightAnim, bodyTypeNumber);}
      if (change == Vector3.right+Vector3.down) { change = new Vector3(0.7f,-1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, rightDownAnim, bodyTypeNumber);}
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(-0.7f,-1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, leftDownAnim, bodyTypeNumber);}
      if (change == Vector3.down)               { change = new Vector3(1f,-0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, rightDownAnim, bodyTypeNumber);}
      if (change == Vector3.left)               { change = new Vector3(-0.7f,-1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, leftAnim, bodyTypeNumber);}


      // if (change == Vector3.right+Vector3.up)   { change = new Vector3(0.7f,1f,0f); }
      // if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.5f,0f); }
      // if (change == Vector3.up)                 { change = new Vector3(-1f,0.5f,0f); }
      // if (change == Vector3.right)              { change = new Vector3(0.7f,1f,0f); }
      // if (change == Vector3.right+Vector3.down) { change = new Vector3(0.7f,-1f,0f); }
      // if (change == Vector3.left+Vector3.down)  { change = new Vector3(-0.7f,-1f,0f); }
      // if (change == Vector3.down)               { change = new Vector3(1f,-0.5f,0f); }
      // if (change == Vector3.left)               { change = new Vector3(-0.7f,-1f,0f);}
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      
    }
    else
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(0.7f,1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upRightAnim, bodyTypeNumber); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upLeftAnim, bodyTypeNumber);}
      if (change == Vector3.up)                 { change = new Vector3(-1f,0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upLeftAnim, bodyTypeNumber);}
      if (change == Vector3.right)              { change = new Vector3(0.7f,1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, upRightAnim, bodyTypeNumber);}
      if (change == Vector3.right+Vector3.down) { change = new Vector3(1f,-0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, rightDownAnim, bodyTypeNumber);}
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(-0.7f,-1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, leftDownAnim, bodyTypeNumber);}
      if (change == Vector3.down)               { change = new Vector3(1f,-0.5f,0f); AnimateMovement(movementStartIndex, movementFrameCount, rightDownAnim, bodyTypeNumber);}
      if (change == Vector3.left)               { change = new Vector3(-0.7f,-1f,0f); AnimateMovement(movementStartIndex, movementFrameCount, leftAnim, bodyTypeNumber);}




      // if (change == Vector3.right+Vector3.up)   { change = new Vector3(0.7f,1f,0f); }
      // if (change == Vector3.left+Vector3.up)    { change = new Vector3(0f,0f,0f); }
      // if (change == Vector3.up)                 { change = new Vector3(0f,0f,0f); }
      // if (change == Vector3.right)              { change = new Vector3(0.7f,1f,0f); }
      // if (change == Vector3.right+Vector3.down) { change = new Vector3(0f,0f,0f); }
      // if (change == Vector3.left+Vector3.down)  { change = new Vector3(-0.7f,-1f,0f); }
      // if (change == Vector3.down)               { change = new Vector3(0f,0f,0f); }
      // if (change == Vector3.left)               { change = new Vector3(-0.7f,-1f,0f); }
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      
    }
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
    myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      
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
    myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      
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
      AnimateMovement(movementStartIndex, movementFrameCount, ladderAnimDirectionIndex, bodyTypeNumber);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
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
      AnimateMovement(movementStartIndex, movementFrameCount, ladderAnimDirectionIndex, bodyTypeNumber);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
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
      AnimateMovement(movementStartIndex, movementFrameCount, ladderAnimDirectionIndex, bodyTypeNumber);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
  }

  void MoveCharacter()
  { 
    if(!fixedDirectionLeft && !fixedDirectionRight)
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(1f,0.5f,0f); currentAnimationDirection = upRightAnim; }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.5f,0f); currentAnimationDirection = upLeftAnim; }
      if (change == Vector3.up)                 { change = new Vector3(-1f,0.5f,0f); currentAnimationDirection = upLeftAnim; }
      if (change == Vector3.right)              { change = new Vector3(1f,0.5f,0f); currentAnimationDirection = upRightAnim; }
      if (change == Vector3.right+Vector3.down) { change = new Vector3(1f,-0.5f,0f); currentAnimationDirection = rightAnim; }
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(-1f,-0.5f,0f); currentAnimationDirection = leftDownAnim; }
      if (change == Vector3.down)               { change = new Vector3(1f,-0.5f,0f); currentAnimationDirection = rightDownAnim; }
      if (change == Vector3.left)               { change = new Vector3(-1f,-0.5f,0f); currentAnimationDirection = leftAnim; }
      AnimateMovement(movementStartIndex, movementFrameCount, currentAnimationDirection, bodyTypeNumber);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);

    }
    else if(fixedDirectionLeft)
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(-1f,0.5f,0f); currentAnimationDirection = upLeftAnim;}
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.5f,0f); currentAnimationDirection = upLeftAnim;}
      if (change == Vector3.up)                 { change = new Vector3(-1f,0.5f,0f); currentAnimationDirection = upLeftAnim;}
      if (change == Vector3.right)              { change = new Vector3(-1f,0.5f,0f); currentAnimationDirection = upLeftAnim;}
      if (change == Vector3.right+Vector3.down) { change = new Vector3(1f,-0.5f,0f); currentAnimationDirection = rightDownAnim;}
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(1f,-0.5f,0f); currentAnimationDirection = rightDownAnim;}
      if (change == Vector3.down)               { change = new Vector3(1f,-0.5f,0f); currentAnimationDirection = rightDownAnim;}
      if (change == Vector3.left)               { change = new Vector3(1f,-0.5f,0f); currentAnimationDirection = rightDownAnim;}
      AnimateMovement(movementStartIndex, movementFrameCount, currentAnimationDirection, bodyTypeNumber);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      
    }              
    else if (fixedDirectionRight)  
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(1f,0.5f,0f); currentAnimationDirection = upRightAnim;}
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(1f,0.5f,0f); currentAnimationDirection = upRightAnim;}
      if (change == Vector3.up)                 { change = new Vector3(1f,0.5f,0f); currentAnimationDirection = upRightAnim;}
      if (change == Vector3.right)              { change = new Vector3(1f,0.5f,0f); currentAnimationDirection = upRightAnim;}
      if (change == Vector3.right+Vector3.down) { change = new Vector3(-1f,-0.5f,0f); currentAnimationDirection = leftAnim;}
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(-1f,-0.5f,0f); currentAnimationDirection = leftAnim;}
      if (change == Vector3.down)               { change = new Vector3(-1f,-0.5f,0f); currentAnimationDirection = leftAnim;}
      if (change == Vector3.left)               { change = new Vector3(-1f,-0.5f,0f); currentAnimationDirection = leftAnim;}
      AnimateMovement(movementStartIndex, movementFrameCount, currentAnimationDirection, bodyTypeNumber);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      
    }
  }

    public void RideBike()
    {
        bikeTransformAdjustment.SetBikeTransformPosition();
        // if(cameraMovement.smoothing != 1000f)
        // {
        //   if(cameraMovement.adjustSmoothingCoro != null)
        //   {
        //     StopCoroutine(cameraMovement.adjustSmoothingCoro);
        //   }
        //   cameraMovement.adjustSmoothingCoro = StartCoroutine(cameraMovement.AdjustSmoothing(1000f)); 
        // }       
        currentBikeColor.a = 1f;
        movementStartIndex = rideBike;
        movementFrameCount = 4;
        speed = initialMovementSpeed + 50;
        animationSpeed = 0.08f;      
        if(change.x > 0)
        {
          bikeSprite.flipX = true;
        }
        else if (change.x < 0)
        {
          bikeSprite.flipX = false;
        }

        if(change.y > 0)
        {
          bikeSprite.sprite = allBikeSprites[0];
        }
        else if (change.y < 0)
        {
          bikeSprite.sprite = allBikeSprites[1];
        }
    }

    public void Sit()
    {
      // if(cameraMovement.adjustSmoothingCoro != null)
      // {
      //   StopCoroutine(cameraMovement.adjustSmoothingCoro);
      // }
      cameraMovement.smoothing = 0f;
      movementStartIndex = sit;
      movementFrameCount = 1;
    }

    public void ClimbLadder()
    {
            //ClimbLadder
      // if(cameraMovement.smoothing != cameraMovement.initialSmoothing)
      // {      
      //   if(cameraMovement.adjustSmoothingCoro != null)
      //   {
      //     StopCoroutine(cameraMovement.adjustSmoothingCoro);
      //   }
      //   cameraMovement.adjustSmoothingCoro = StartCoroutine(cameraMovement.AdjustSmoothing(3f));
      // }
      movementStartIndex = climb;
      movementFrameCount = 4;
      animationSpeed = initialAnimationSpeed;
      currentBikeColor.a = 0f;
    }

    public void Run()
    {
      //Run
      // if(cameraMovement.smoothing != 1000f)
      // {   
      //   if(cameraMovement.adjustSmoothingCoro != null)
      //   {
      //     StopCoroutine(cameraMovement.adjustSmoothingCoro);
      //   }
      //   cameraMovement.adjustSmoothingCoro = StartCoroutine(cameraMovement.AdjustSmoothing(1000f)); 
      // }     
      movementStartIndex = run;
      movementFrameCount = 4;
      speed = initialMovementSpeed + 20;
      animationSpeed = 0.11f;
      currentBikeColor.a = 0f;
    } 

    public void Walk()
    {
      //Walk
      // if(cameraMovement.smoothing != cameraMovement.initialSmoothing)
      // {         
      //   if(cameraMovement.adjustSmoothingCoro != null)
      //   {
      //     StopCoroutine(cameraMovement.adjustSmoothingCoro);
      //   }
      //   cameraMovement.adjustSmoothingCoro = StartCoroutine(cameraMovement.AdjustSmoothing(3f));
      // }      

      movementFrameCount = 4;
      movementStartIndex = walk;
      speed = initialMovementSpeed;
      animationSpeed = initialAnimationSpeed;
    }



    public void SetShirtToBirthdaySuit()
    {
      throatSprite.color = currentSkinColor;
      collarSprite.color = currentSkinColor;
      torsoSprite.color = currentSkinColor;
      
      if(characterCustomization.currentJakettoIndex == 0 || characterCustomization.currentJakettoIndex == 1) // vest or no jaketto
      {
        shortSleeveSprite.color = currentSkinColor;
        longSleeveSprite.color = currentSkinColor;
      }
      else if(characterCustomization.currentJakettoIndex == 2)
      {
        longSleeveSprite.color = currentSkinColor; // and if characterCustomization.currentJakettoIndex == 3 do nuttin!
      }
      if(characterCustomization.currentWaistIndex == 0) // if waist is set to shirt color
      {
        waistSprite.color = currentPantsColor;
      }
    }
    public void SetShirtToSinglet1()
    {
      throatSprite.color = currentSkinColor;
      collarSprite.color = currentSkinColor;
      torsoSprite.color = currentShirtColor;
      if(characterCustomization.currentJakettoIndex == 0 || characterCustomization.currentJakettoIndex == 1) // vest or no jaketto
      {
        shortSleeveSprite.color = currentSkinColor;
        longSleeveSprite.color = currentSkinColor;
      }
      else if(characterCustomization.currentJakettoIndex == 2)
      {
        longSleeveSprite.color = currentSkinColor; 
        // and if characterCustomization.currentJakettoIndex == 3 do nuttin!
      }
      if(characterCustomization.currentWaistIndex == 0) // if waist is set to shirt color
      {
        waistSprite.color = currentShirtColor;
        characterCustomization.currentWaistIndex = 0;
      }
    }
    public void SetShirtToSinglet2()
    {
      throatSprite.color = currentSkinColor;
      collarSprite.color = currentShirtColor;
      torsoSprite.color = currentShirtColor;
      if(characterCustomization.currentJakettoIndex == 0 || characterCustomization.currentJakettoIndex == 1) // vest or no jaketto
      {
        shortSleeveSprite.color = currentSkinColor;
        longSleeveSprite.color = currentSkinColor;
      }
      else if(characterCustomization.currentJakettoIndex == 2)
      {
        longSleeveSprite.color = currentSkinColor;
        // and if characterCustomization.currentJakettoIndex == 3 do nuttin!
      }
      if(waistSprite.color == currentShirtColor)
      {
        waistSprite.color = currentShirtColor;
      }
    }
    public void SetShirtToSinglet3()
    {
      throatSprite.color = currentShirtColor;
      collarSprite.color = currentShirtColor;
      torsoSprite.color = currentShirtColor;
      if(characterCustomization.currentJakettoIndex == 0 || characterCustomization.currentJakettoIndex == 1) // vest or no jaketto
      {
        shortSleeveSprite.color = currentSkinColor;
        longSleeveSprite.color = currentSkinColor;
      }
      else if(characterCustomization.currentJakettoIndex == 2)
      {
        longSleeveSprite.color = currentSkinColor;
        // and if characterCustomization.currentJakettoIndex == 3 do nuttin!
      }

      if(waistSprite.color == currentShirtColor)
      {
        waistSprite.color = currentShirtColor;
      }
    }
    public void SetShirtToTShirt1()
    {
      throatSprite.color = currentSkinColor;
      collarSprite.color = currentShirtColor;
      torsoSprite.color = currentShirtColor;
     
      if(characterCustomization.currentJakettoIndex == 0 || characterCustomization.currentJakettoIndex == 1) // vest or no jaketto
      {
        shortSleeveSprite.color = currentShirtColor;
        longSleeveSprite.color = currentSkinColor;
      }
      else if(characterCustomization.currentJakettoIndex == 2)
      {
        longSleeveSprite.color = currentSkinColor;
        // and if characterCustomization.currentJakettoIndex == 3 do nuttin!
      }

      if(waistSprite.color == currentShirtColor)
      {
        waistSprite.color = currentShirtColor;
      }
    }
    public void SetShirtToTShirt2()
    {
      throatSprite.color = currentSkinColor;
      collarSprite.color = currentSkinColor;
      torsoSprite.color = currentShirtColor;

      if(characterCustomization.currentJakettoIndex == 0 || characterCustomization.currentJakettoIndex == 1) // vest or no jaketto
      {
        shortSleeveSprite.color = currentShirtColor;
        longSleeveSprite.color = currentSkinColor;
      }
      else if(characterCustomization.currentJakettoIndex == 2)
      {
        longSleeveSprite.color = currentSkinColor;
        // and if characterCustomization.currentJakettoIndex == 3 do nuttin!
      }

      if(waistSprite.color == currentShirtColor)
      {
        waistSprite.color = currentShirtColor;
      }
    }
    public void SetShirtToTShirt3()
    {
      throatSprite.color = currentShirtColor;
      collarSprite.color = currentShirtColor;
      torsoSprite.color = currentShirtColor;

      if(characterCustomization.currentJakettoIndex == 0 || characterCustomization.currentJakettoIndex == 1) // vest or no jaketto
      {
        shortSleeveSprite.color = currentShirtColor;
        longSleeveSprite.color = currentSkinColor;
      }
      else if(characterCustomization.currentJakettoIndex == 2)
      {
        longSleeveSprite.color = currentSkinColor;
        // and if characterCustomization.currentJakettoIndex == 3 do nuttin!
      }

      if(waistSprite.color == currentShirtColor)
      {
        waistSprite.color = currentShirtColor;
      }
    }
    public void SetShirtToLSShirt1()
    {
      throatSprite.color = currentSkinColor;
      collarSprite.color = currentShirtColor;
      torsoSprite.color = currentShirtColor;

      if(characterCustomization.currentJakettoIndex == 0 || characterCustomization.currentJakettoIndex == 1
          || characterCustomization.currentJakettoIndex == 2) // vest or no jaketto
      {
        shortSleeveSprite.color = currentShirtColor;
        longSleeveSprite.color = currentShirtColor;
      }


      if(waistSprite.color == currentShirtColor)
      {
        waistSprite.color = currentShirtColor;
      }
    }
    public void SetShirtToLSShirt2()
    {
      throatSprite.color = currentSkinColor;
      collarSprite.color = currentSkinColor;
      torsoSprite.color = currentShirtColor;

      if(characterCustomization.currentJakettoIndex == 0 || characterCustomization.currentJakettoIndex == 1
          || characterCustomization.currentJakettoIndex == 2) // vest or no jaketto
      {
        shortSleeveSprite.color = currentShirtColor;
        longSleeveSprite.color = currentShirtColor;
      }


      if(waistSprite.color == currentShirtColor)
      {
        waistSprite.color = currentShirtColor;
      }
    }
    public void SetShirtToLSShirt3()
    {
      throatSprite.color = currentShirtColor;
      collarSprite.color = currentShirtColor;
      torsoSprite.color = currentShirtColor;
     
      if(characterCustomization.currentJakettoIndex == 0 || characterCustomization.currentJakettoIndex == 1
          || characterCustomization.currentJakettoIndex == 2) // vest or no jaketto
      {
        shortSleeveSprite.color = currentShirtColor;
        longSleeveSprite.color = currentShirtColor;
      }

      if(waistSprite.color == currentShirtColor)
      {
        waistSprite.color = currentShirtColor;
      }
    }



    public void SetPantsToShorts()
    {
      waistShortsSprite.color = currentPantsColor;
      kneesShinsSprite.color = currentSkinColor;
      if(anklesSprite.color != currentShoeColor)
      {
        anklesSprite.color = currentSkinColor;
      }
    }
    public void SetPantsTo3QuarterPants()
    {
      if(anklesSprite.color != currentShoeColor)
      {
        waistShortsSprite.color = currentPantsColor;
        kneesShinsSprite.color = currentPantsColor;
        anklesSprite.color = currentSkinColor;
      }
      else
      {
        characterCustomization.NextPants();
      }
    }
    public void SetPantsToPants()
    {
      waistShortsSprite.color = currentPantsColor;
      kneesShinsSprite.color = currentPantsColor;
      if(anklesSprite.color != currentShoeColor)
      {
        anklesSprite.color = currentPantsColor;
      }    
    }


    public void SetWaistToShirt()
    {
      if(characterCustomization.currentShirtIndex == 0) // if no shirt is on
      {
        characterCustomization.currentWaistIndex = 1;// set waist to pants 
        waistSprite.color = currentPantsColor;
      }
      else
      {
        waistSprite.color = currentShirtColor;
      }  
    }
    public void SetWaistToPants()
    {
      waistSprite.color = currentPantsColor;  
    }


    public void SetFeetToShoes()
    {
      feetSprite.color = currentShoeColor;
      if(characterCustomization.currentPantsIndex == 2)
      {
        anklesSprite.color = currentPantsColor;
      }
      else
      {
        anklesSprite.color = currentSkinColor;
      }

    }
    public void SetFeetToBareFoot()
    {
      feetSprite.color = currentSkinColor;
      if(characterCustomization.currentPantsIndex == 2)
      {
        anklesSprite.color = currentPantsColor;
      }
      else
      {
        anklesSprite.color = currentSkinColor;
      }
    }
    public void SetFeetToBoots()
    {
      feetSprite.color = currentShoeColor;
      anklesSprite.color = currentShoeColor;
    }


    public void SetNoJaketto()
    {
      Color color = currentJakettoColor;
      color.a = 0f;
      jakettoSprite.color = color;
      characterCustomization.UpdateShirt();
    }
    public void SetJakettoToVest()
    {
      Color color = currentJakettoColor;
      color.a = 1f;
      jakettoSprite.color = color;
      characterCustomization.UpdateShirt();
    }
    public void SetJaketto1()
    {
      Color color = currentJakettoColor;
      shortSleeveSprite.color = color;
      jakettoSprite.color = color;
      color.a = 1f;
    }
    public void SetJaketto2()
    {
      Color color = currentJakettoColor;
      shortSleeveSprite.color = currentJakettoColor;
      longSleeveSprite.color = currentJakettoColor;
      color.a = 1f;
      jakettoSprite.color = color;
    }



  public void SetSkinColor1()
  {
    if(headSprite.color == currentSkinColor)
    {
      headSprite.color = HexToColor("#F5CBA7");
    };
    if(throatSprite.color == currentSkinColor)
    {
      throatSprite.color = HexToColor("#F5CBA7");
    };
    if(collarSprite.color == currentSkinColor)
    {
      collarSprite.color = HexToColor("#F5CBA7");
    };
    if(torsoSprite.color == currentSkinColor)
    {
      torsoSprite.color = HexToColor("#F5CBA7");
    };
    if(waistShortsSprite.color == currentSkinColor)
    {
      waistShortsSprite.color = HexToColor("#F5CBA7");
    };
    if(waistSprite.color == currentSkinColor)
    {
      waistSprite.color = HexToColor("#F5CBA7");
    };
    if(kneesShinsSprite.color ==currentSkinColor)
    {
      kneesShinsSprite.color = HexToColor("#F5CBA7");
    };
    if(anklesSprite.color == currentSkinColor)
    {
      anklesSprite.color = HexToColor("#F5CBA7");
    };
    if(feetSprite.color == currentSkinColor)
    {
      feetSprite.color = HexToColor("#F5CBA7");
    };
    if(longSleeveSprite.color == currentSkinColor)
    {
      longSleeveSprite.color = HexToColor("#F5CBA7");
    };
    if(handSprite.color == currentSkinColor)
    {
      handSprite.color = HexToColor("#F5CBA7");
    };
    if(shortSleeveSprite.color == currentSkinColor)
    {
      shortSleeveSprite.color = HexToColor("#F5CBA7");
    };
    currentSkinColor = HexToColor("#F5CBA7");
  }
  

  public void SetSkinColor2()
  {
    if(headSprite.color == currentSkinColor)
    {
      headSprite.color = HexToColor("#F6B883");
    };
    if(throatSprite.color == currentSkinColor)
    {
      throatSprite.color = HexToColor("#F6B883");
    };
    if(collarSprite.color == currentSkinColor)
    {
      collarSprite.color = HexToColor("#F6B883");
    };
    if(torsoSprite.color == currentSkinColor)
    {
      torsoSprite.color = HexToColor("#F6B883");
    };
    if(waistShortsSprite.color == currentSkinColor)
    {
      waistShortsSprite.color = HexToColor("#F6B883");
    };
    if(waistSprite.color == currentSkinColor)
    {
      waistSprite.color = HexToColor("#F6B883");
    };
    if(kneesShinsSprite.color ==currentSkinColor)
    {
      kneesShinsSprite.color = HexToColor("#F6B883");
    };
    if(anklesSprite.color == currentSkinColor)
    {
      anklesSprite.color = HexToColor("#F6B883");
    };
    if(feetSprite.color == currentSkinColor)
    {
      feetSprite.color = HexToColor("#F6B883");
    };
    if(longSleeveSprite.color == currentSkinColor)
    {
      longSleeveSprite.color = HexToColor("#F6B883");
    };
    if(handSprite.color == currentSkinColor)
    {
      handSprite.color = HexToColor("#F6B883");
    };
    if(shortSleeveSprite.color == currentSkinColor)
    {
      shortSleeveSprite.color = HexToColor("#F6B883");
    };
    currentSkinColor = HexToColor("#F6B883");
  }

  public void SetSkinColor3()
    {
      if(headSprite.color == currentSkinColor)
      {
        headSprite.color = HexToColor("#E1A95F");
      };
      if(throatSprite.color == currentSkinColor)
      {
        throatSprite.color = HexToColor("#E1A95F");
      };
      if(collarSprite.color == currentSkinColor)
      {
        collarSprite.color = HexToColor("#E1A95F");
      };
      if(torsoSprite.color == currentSkinColor)
      {
        torsoSprite.color = HexToColor("#E1A95F");
      };
      if(waistSprite.color == currentSkinColor)
      {
        waistSprite.color = HexToColor("#E1A95F");
      };
      if(waistShortsSprite.color == currentSkinColor)
      {
        waistShortsSprite.color = HexToColor("#E1A95F");
      };
      if(kneesShinsSprite.color == currentSkinColor)
      {
        kneesShinsSprite.color = HexToColor("#E1A95F");
      };
      if(anklesSprite.color == currentSkinColor)
      {
        anklesSprite.color = HexToColor("#E1A95F");
      };
      if(feetSprite.color == currentSkinColor)
      {
        feetSprite.color = HexToColor("#E1A95F");
      };
      if(longSleeveSprite.color == currentSkinColor)
      {
        longSleeveSprite.color = HexToColor("#E1A95F");
      };
      if(handSprite.color == currentSkinColor)
      {
        handSprite.color = HexToColor("#E1A95F");
      };
      if(shortSleeveSprite.color == currentSkinColor)
      {
        shortSleeveSprite.color = HexToColor("#E1A95F");
      };
      currentSkinColor = HexToColor("#E1A95F");
    }

  public void SetSkinColor4()
    {
      if(headSprite.color == currentSkinColor)
      {
        headSprite.color = HexToColor("#C68642");
      };
      if(throatSprite.color == currentSkinColor)
      {
        throatSprite.color = HexToColor("#C68642");
      };
      if(collarSprite.color == currentSkinColor)
      {
        collarSprite.color = HexToColor("#C68642");
      };
      if(torsoSprite.color == currentSkinColor)
      {
        torsoSprite.color = HexToColor("#C68642");
      };
      if(waistSprite.color == currentSkinColor)
      {
        waistSprite.color = HexToColor("#C68642");
      };
      if(waistShortsSprite.color == currentSkinColor)
      {
        waistShortsSprite.color = HexToColor("#C68642");
      };
      if(kneesShinsSprite.color == currentSkinColor)
      {
        kneesShinsSprite.color = HexToColor("#C68642");
      };
      if(anklesSprite.color == currentSkinColor)
      {
        anklesSprite.color = HexToColor("#C68642");
      };
      if(feetSprite.color == currentSkinColor)
      {
        feetSprite.color = HexToColor("#C68642");
      };
      if(longSleeveSprite.color == currentSkinColor)
      {
        longSleeveSprite.color = HexToColor("#C68642");
      };
      if(handSprite.color == currentSkinColor)
      {
        handSprite.color = HexToColor("#C68642");
      };
      if(shortSleeveSprite.color == currentSkinColor)
      {
        shortSleeveSprite.color = HexToColor("#C68642");
      };
      currentSkinColor = HexToColor("#C68642");
    }

  public void SetSkinColor5()
    {
      if(headSprite.color == currentSkinColor)
      {
        headSprite.color = HexToColor("#8D5524");
      };
      if(throatSprite.color == currentSkinColor)
      {
        throatSprite.color = HexToColor("#8D5524");
      };
      if(collarSprite.color == currentSkinColor)
      {
        collarSprite.color = HexToColor("#8D5524");
      };
      if(torsoSprite.color == currentSkinColor)
      {
        torsoSprite.color = HexToColor("#8D5524");
      };
      if(waistSprite.color == currentSkinColor)
      {
        waistSprite.color = HexToColor("#8D5524");
      };
      if(waistShortsSprite.color == currentSkinColor)
      {
        waistShortsSprite.color = HexToColor("#8D5524");
      };
      if(kneesShinsSprite.color == currentSkinColor)
      {
        kneesShinsSprite.color = HexToColor("#8D5524");
      };
      if(anklesSprite.color == currentSkinColor)
      {
        anklesSprite.color = HexToColor("#8D5524");
      };
      if(feetSprite.color == currentSkinColor)
      {
        feetSprite.color = HexToColor("#8D5524");
      };
      if(longSleeveSprite.color == currentSkinColor)
      {
        longSleeveSprite.color = HexToColor("#8D5524");
      };
      if(handSprite.color == currentSkinColor)
      {
        handSprite.color = HexToColor("#8D5524");
      };
      if(shortSleeveSprite.color == currentSkinColor)
      {
        shortSleeveSprite.color = HexToColor("#8D5524");
      };
      currentSkinColor = HexToColor("#8D5524");
    }

  public void SetSkinColor6()
    {
      if(headSprite.color == currentSkinColor)
      {
        headSprite.color = HexToColor("#5D4037");
      };
      if(throatSprite.color == currentSkinColor)
      {
        throatSprite.color = HexToColor("#5D4037");
      };
      if(collarSprite.color == currentSkinColor)
      {
        collarSprite.color = HexToColor("#5D4037");
      };
      if(torsoSprite.color == currentSkinColor)
      {
        torsoSprite.color = HexToColor("#5D4037");
      };
      if(waistSprite.color == currentSkinColor)
      {
        waistSprite.color = HexToColor("#5D4037");
      };
      if(waistShortsSprite.color == currentSkinColor)
      {
        waistShortsSprite.color = HexToColor("#5D4037");
      };
      if(kneesShinsSprite.color == currentSkinColor)
      {
        kneesShinsSprite.color = HexToColor("#5D4037");
      };
      if(anklesSprite.color == currentSkinColor)
      {
        anklesSprite.color = HexToColor("#5D4037");
      };
      if(feetSprite.color == currentSkinColor)
      {
        feetSprite.color = HexToColor("#5D4037");
      };
      if(longSleeveSprite.color == currentSkinColor)
      {
        longSleeveSprite.color = HexToColor("#5D4037");
      };
      if(handSprite.color == currentSkinColor)
      {
        handSprite.color = HexToColor("#5D4037");
      };
      if(shortSleeveSprite.color == currentSkinColor)
      {
        shortSleeveSprite.color = HexToColor("#5D4037");
      };
      currentSkinColor = HexToColor("#5D4037");
    }

  public void SetSkinColor7()
    {
      if(headSprite.color == currentSkinColor)
      {
        headSprite.color = HexToColor("#3B2F2F");
      };
      if(throatSprite.color == currentSkinColor)
      {
        throatSprite.color = HexToColor("#3B2F2F");
      };
      if(collarSprite.color == currentSkinColor)
      {
        collarSprite.color = HexToColor("#3B2F2F");
      };
      if(torsoSprite.color == currentSkinColor)
      {
        torsoSprite.color = HexToColor("#3B2F2F");
      };
      if(waistSprite.color == currentSkinColor)
      {
        waistSprite.color = HexToColor("#3B2F2F");
      };
      if(waistShortsSprite.color == currentSkinColor)
      {
        waistShortsSprite.color = HexToColor("#3B2F2F");
      };
      if(kneesShinsSprite.color == currentSkinColor)
      {
        kneesShinsSprite.color = HexToColor("#3B2F2F");
      };
      if(anklesSprite.color == currentSkinColor)
      {
        anklesSprite.color = HexToColor("#3B2F2F");
      };
      if(feetSprite.color == currentSkinColor)
      {
        feetSprite.color = HexToColor("#3B2F2F");
      };
      if(longSleeveSprite.color == currentSkinColor)
      {
        longSleeveSprite.color = HexToColor("#3B2F2F");
      };
      if(handSprite.color == currentSkinColor)
      {
        handSprite.color = HexToColor("#3B2F2F");
      };
      if(shortSleeveSprite.color == currentSkinColor)
      {
        shortSleeveSprite.color = HexToColor("#3B2F2F");
      };
      currentSkinColor = HexToColor("#3B2F2F");
    }



    public void SetHairColor1()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                Color color = spriteRenderer.color;
                if (spriteRenderer != null && color.a != 0)
                {
                    spriteRenderer.color = HexToColor("#4E342E");  // Brunette (Brown)
                }
            }
          currentHairColor = HexToColor("#4E342E"); 
        }
    }

    public void SetHairColor2()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                Color color = spriteRenderer.color;
                if (spriteRenderer != null && color.a != 0)
                {
                    spriteRenderer.color = HexToColor("#1C1C1C");  // Black
                }
            }
          currentHairColor = HexToColor("#1C1C1C"); 
        }
    }

    public void SetHairColor3()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                Color color = spriteRenderer.color;
                if (spriteRenderer != null && color.a != 0)
                {
                    spriteRenderer.color = HexToColor("#F5D76E");  // Blonde
                }
            }
          currentHairColor = HexToColor("#F5D76E");  
        }
    }

    public void SetHairColor4()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                Color color = spriteRenderer.color;
                if (spriteRenderer != null && color.a != 0)
                {
                    spriteRenderer.color = HexToColor("#A52A2A");  // Auburn (Red-Brown)
                }
            }
          currentHairColor = HexToColor("#A52A2A");  
        }
    }

    public void SetHairColor5()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                Color color = spriteRenderer.color;
                if (spriteRenderer != null && color.a != 0)
                {
                    spriteRenderer.color = HexToColor("#A9A9A9");  // Gray
                }
            }
        }
      currentHairColor = HexToColor("#A9A9A9");  
    }

    public void SetHairColor6()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                Color color = spriteRenderer.color;
                if (spriteRenderer != null && color.a != 0)
                {
                    spriteRenderer.color = HexToColor("#00BFFF");  // Electric Blue
                }
            }
        }
      currentHairColor = HexToColor("#00BFFF");  
    }

    public void SetHairColor7()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                Color color = spriteRenderer.color;
                if (spriteRenderer != null && color.a != 0)
                {
                    spriteRenderer.color = HexToColor("#FFB6C1");  // Pastel Pink
                }
            }
        }
      currentHairColor = HexToColor("#FFB6C1");  
    }

    public void SetHairColor8()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                Color color = spriteRenderer.color;
                if (spriteRenderer != null && color.a != 0)
                {
                    spriteRenderer.color = HexToColor("#E6E6FA");  // Lavender
                }
            }
        }
      currentHairColor = HexToColor("#E6E6FA");  
    }

    public void SetHairColor9()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                Color color = spriteRenderer.color;
                if (spriteRenderer != null && color.a != 0)
                {
                    spriteRenderer.color = HexToColor("#98FF98");  // Mint Green
                }
            }
        }
      currentHairColor = HexToColor("#98FF98");  
    }

    public void SetHairColor10()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                Color color = spriteRenderer.color;
                if (spriteRenderer != null && color.a != 0)
                {
                    spriteRenderer.color = HexToColor("#8A2BE2");  // Vibrant Purple
                }
            }
        }
      currentHairColor = HexToColor("#8A2BE2");  
    }


    public void SetHairStyle1()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair0Top") || child.gameObject.name.Contains("hair0Bottom"))
                  {
                      spriteRenderer.color = currentHairColor;  
                      Color color = spriteRenderer.color;
                      color.a = 1f;  // Set alpha to 1 (fully visible)
                      spriteRenderer.color = color;
                  }
                  else
                  {
                      Color color = spriteRenderer.color;
                      color.a = 0f;  // Set alpha to 0 (invisible)
                      spriteRenderer.color = color;
                  }
                }
            }
        }
    }

    public void SetHairStyle2()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair0Top") 
                      || child.gameObject.name.Contains("hair0Bottom")
                      || child.gameObject.name.Contains("hairFringe1"))
                  {
                      spriteRenderer.color = currentHairColor;                    
                      Color color = spriteRenderer.color;
                      color.a = 1f;  // Set alpha to 1 (fully visible)
                      spriteRenderer.color = color;
                  }
                  else
                  {
                      Color color = spriteRenderer.color;
                      color.a = 0f;  // Set alpha to 0 (invisible)
                      spriteRenderer.color = color;
                  }
                }
            }
        }
    }

    public void SetHairStyle3()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") 
                      || child.gameObject.name.Contains("hair1Bottom")
                      || child.gameObject.name.Contains("hairFringe1"))
                  {
                      spriteRenderer.color = currentHairColor;                    
                      Color color = spriteRenderer.color;
                      color.a = 1f;  // Set alpha to 1 (fully visible)
                      spriteRenderer.color = color;
                  }
                  else
                  {
                      Color color = spriteRenderer.color;
                      color.a = 0f;  // Set alpha to 0 (invisible)
                      spriteRenderer.color = color;
                  }
                }
            }
        }
    }

    public void SetHairStyle4()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") 
                      || child.gameObject.name.Contains("hair1Bottom"))
                  {
                      spriteRenderer.color = currentHairColor;  
                      Color color = spriteRenderer.color;
                      color.a = 1f;  // Set alpha to 1 (fully visible)
                      spriteRenderer.color = color;
                  }
                  else
                  {
                      Color color = spriteRenderer.color;
                      color.a = 0f;  // Set alpha to 0 (invisible)
                      spriteRenderer.color = color;
                  }
                }
            }
        }
    }

    public void SetHairStyle5()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("mohawk"))
                  {
                      spriteRenderer.color = currentHairColor;  
                      Color color = spriteRenderer.color;
                      color.a = 1f;  // Set alpha to 1 (fully visible)
                      spriteRenderer.color = color;
                  }
                  else
                  {
                      Color color = spriteRenderer.color;
                      color.a = 0f;  // Set alpha to 0 (invisible)
                      spriteRenderer.color = color;
                  }
                }
            }
        }
    }

    public void SetHairStyle6()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") || child.gameObject.name.Contains("hair2Bottom") )
                  {
                      spriteRenderer.color = currentHairColor;  
                      Color color = spriteRenderer.color;
                      color.a = 1f;  // Set alpha to 1 (fully visible)
                      spriteRenderer.color = color;
                  }
                  else
                  {
                      Color color = spriteRenderer.color;
                      color.a = 0f;  // Set alpha to 0 (invisible)
                      spriteRenderer.color = color;
                  }
                }
            }
        }
    }

    public void SetHairStyle7()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") || child.gameObject.name.Contains("hair3Bottom") )
                  {
                      spriteRenderer.color = currentHairColor;  
                      Color color = spriteRenderer.color;
                      color.a = 1f;  // Set alpha to 1 (fully visible)
                      spriteRenderer.color = color;
                  }
                  else
                  {
                      Color color = spriteRenderer.color;
                      color.a = 0f;  // Set alpha to 0 (invisible)
                      spriteRenderer.color = color;
                  }
                }
            }
        }
    }

    public void SetHairStyle8()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") || child.gameObject.name.Contains("hair4Bottom") )
                  {
                      spriteRenderer.color = currentHairColor;  
                      Color color = spriteRenderer.color;
                      color.a = 1f;  // Set alpha to 1 (fully visible)
                      spriteRenderer.color = color;
                  }
                  else
                  {
                      Color color = spriteRenderer.color;
                      color.a = 0f;  // Set alpha to 0 (invisible)
                      spriteRenderer.color = color;
                  }
                }
            }
        }
    }
    public void SetHairStyle9()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") || child.gameObject.name.Contains("hair1Bottom") || child.gameObject.name.Contains("hair6Bottom") )
                  {
                      spriteRenderer.color = currentHairColor;  
                      Color color = spriteRenderer.color;
                      color.a = 1f;  // Set alpha to 1 (fully visible)
                      spriteRenderer.color = color;
                  }
                  else
                  {
                      Color color = spriteRenderer.color;
                      color.a = 0f;  // Set alpha to 0 (invisible)
                      spriteRenderer.color = color;
                  }
                }
            }
        }
    }
    public void SetHairStyle10()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair7Top")
                  || child.gameObject.name.Contains("hair7Bottom"))
                  {
                      spriteRenderer.color = currentHairColor;  
                      Color color = spriteRenderer.color;
                      color.a = 1f;  // Set alpha to 1 (fully visible)
                      spriteRenderer.color = color;
                  }
                  else
                  {
                      Color color = spriteRenderer.color;
                      color.a = 0f;  // Set alpha to 0 (invisible)
                      spriteRenderer.color = color;
                  }
                }
            }
        }
    }
    public void SetHairStyle11()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair8Top") || child.gameObject.name.Contains("hair8Bottom"))
                  {
                      spriteRenderer.color = currentHairColor;  
                      Color color = spriteRenderer.color;
                      color.a = 1f;  // Set alpha to 1 (fully visible)
                      spriteRenderer.color = color;
                  }
                  else
                  {
                      Color color = spriteRenderer.color;
                      color.a = 0f;  // Set alpha to 0 (invisible)
                      spriteRenderer.color = color;
                  }
                }
            }
        }
    }
    public void SetHairStyle12()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair0Bottom"))
                  {
                      spriteRenderer.color = currentHairColor;  
                      Color color = spriteRenderer.color;
                      color.a = 1f;  // Set alpha to 1 (fully visible)
                      spriteRenderer.color = color;
                  }
                  else
                  {
                      Color color = spriteRenderer.color;
                      color.a = 0f;  // Set alpha to 0 (invisible)
                      spriteRenderer.color = color;
                  }
                }
            }
        }
    }
    public void SetHairStyle13()
    {
        Transform hair = transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                    Color color = spriteRenderer.color;
                    color.a = 0f;  // Set alpha to 0 (invisible)
                    spriteRenderer.color = color;
                }
            }
        }
    }




  public void SetShirtColor1()
  {
    if(throatSprite.color == currentShirtColor)
    {
      throatSprite.color = HexToColor("#FF6347");
    };
    if(collarSprite.color == currentShirtColor)
    {
      collarSprite.color = HexToColor("#FF6347");
    };
    if(torsoSprite.color == currentShirtColor)
    {
      torsoSprite.color = HexToColor("#FF6347");
    };
    if(waistSprite.color == currentShirtColor)
    {
      waistSprite.color = HexToColor("#FF6347");
    };
    if(longSleeveSprite.color == currentShirtColor)
    {
      longSleeveSprite.color = HexToColor("#FF6347");
    };
    if(shortSleeveSprite.color == currentShirtColor)
    {
      shortSleeveSprite.color = HexToColor("#FF6347");
    };
    currentShirtColor = HexToColor("#FF6347");
  }

  public void SetShirtColor2()
  {
    if(throatSprite.color == currentShirtColor)
    {
      throatSprite.color = HexToColor("#4682B4");
    };
    if(collarSprite.color == currentShirtColor)
    {
      collarSprite.color = HexToColor("#4682B4");
    };
    if(torsoSprite.color == currentShirtColor)
    {
      torsoSprite.color = HexToColor("#4682B4");
    };
    if(waistSprite.color == currentShirtColor)
    {
      waistSprite.color = HexToColor("#4682B4");
    };
    if(longSleeveSprite.color == currentShirtColor)
    {
      longSleeveSprite.color = HexToColor("#4682B4");
    };
    if(shortSleeveSprite.color == currentShirtColor)
    {
      shortSleeveSprite.color = HexToColor("#4682B4");
    };
    currentShirtColor = HexToColor("#4682B4");
  }

  public void SetShirtColor3()
  {
    if(throatSprite.color == currentShirtColor)
    {
      throatSprite.color = HexToColor("#32CD32");
    };
    if(collarSprite.color == currentShirtColor)
    {
      collarSprite.color = HexToColor("#32CD32");
    };
    if(torsoSprite.color == currentShirtColor)
    {
      torsoSprite.color = HexToColor("#32CD32");
    };
    if(waistSprite.color == currentShirtColor)
    {
      waistSprite.color = HexToColor("#32CD32");
    };
    if(longSleeveSprite.color == currentShirtColor)
    {
      longSleeveSprite.color = HexToColor("#32CD32");
    };
    if(shortSleeveSprite.color == currentShirtColor)
    {
      shortSleeveSprite.color = HexToColor("#32CD32");
    };
    currentShirtColor = HexToColor("#32CD32");
  }

  public void SetShirtColor4()
  {
    if(throatSprite.color == currentShirtColor)
    {
      throatSprite.color = HexToColor("#FFD700");
    };
    if(collarSprite.color == currentShirtColor)
    {
      collarSprite.color = HexToColor("#FFD700");
    };
    if(torsoSprite.color == currentShirtColor)
    {
      torsoSprite.color = HexToColor("#FFD700");
    };
    if(waistSprite.color == currentShirtColor)
    {
      waistSprite.color = HexToColor("#FFD700");
    };
    if(longSleeveSprite.color == currentShirtColor)
    {
      longSleeveSprite.color = HexToColor("#FFD700");
    };
    if(shortSleeveSprite.color == currentShirtColor)
    {
      shortSleeveSprite.color = HexToColor("#FFD700");
    };
    currentShirtColor = HexToColor("#FFD700");
  }

  public void SetShirtColor5()
  {
    if(throatSprite.color == currentShirtColor)
    {
      throatSprite.color = HexToColor("#8A2BE2");
    };
    if(collarSprite.color == currentShirtColor)
    {
      collarSprite.color = HexToColor("#8A2BE2");
    };
    if(torsoSprite.color == currentShirtColor)
    {
      torsoSprite.color = HexToColor("#8A2BE2");
    };
    if(waistSprite.color == currentShirtColor)
    {
      waistSprite.color = HexToColor("#8A2BE2");
    };
    if(longSleeveSprite.color == currentShirtColor)
    {
      longSleeveSprite.color = HexToColor("#8A2BE2");
    };
    if(shortSleeveSprite.color == currentShirtColor)
    {
      shortSleeveSprite.color = HexToColor("#8A2BE2");
    };
    currentShirtColor = HexToColor("#8A2BE2");
  }

  public void SetShirtColor6()
  {
    if(throatSprite.color == currentShirtColor)
    {
      throatSprite.color = HexToColor("#DC143C");
    };
    if(collarSprite.color == currentShirtColor)
    {
      collarSprite.color = HexToColor("#DC143C");
    };
    if(torsoSprite.color == currentShirtColor)
    {
      torsoSprite.color = HexToColor("#DC143C");
    };
    if(waistSprite.color == currentShirtColor)
    {
      waistSprite.color = HexToColor("#DC143C");
    };
    if(longSleeveSprite.color == currentShirtColor)
    {
      longSleeveSprite.color = HexToColor("#DC143C");
    };
    if(shortSleeveSprite.color == currentShirtColor)
    {
      shortSleeveSprite.color = HexToColor("#DC143C");
    };
    currentShirtColor = HexToColor("#DC143C");
  }
  
  public void SetShirtColor7()
  {
    if(throatSprite.color == currentShirtColor)
    {
      throatSprite.color = HexToColor("#20B2AA");
    };
    if(collarSprite.color == currentShirtColor)
    {
      collarSprite.color = HexToColor("#20B2AA");
    };
    if(torsoSprite.color == currentShirtColor)
    {
      torsoSprite.color = HexToColor("#20B2AA");
    };
    if(waistSprite.color == currentShirtColor)
    {
      waistSprite.color = HexToColor("#20B2AA");
    };
    if(longSleeveSprite.color == currentShirtColor)
    {
      longSleeveSprite.color = HexToColor("#20B2AA");
    };
    if(shortSleeveSprite.color == currentShirtColor)
    {
      shortSleeveSprite.color = HexToColor("#20B2AA");
    };
    currentShirtColor = HexToColor("#20B2AA");
  }

  public void SetShirtColor8()
  {
    if(throatSprite.color == currentShirtColor)
    {
      throatSprite.color = HexToColor("#D3D3D3");
    };
    if(collarSprite.color == currentShirtColor)
    {
      collarSprite.color = HexToColor("#D3D3D3");
    };
    if(torsoSprite.color == currentShirtColor)
    {
      torsoSprite.color = HexToColor("#D3D3D3");
    };
    if(waistSprite.color == currentShirtColor)
    {
      waistSprite.color = HexToColor("#D3D3D3");
    };
    if(longSleeveSprite.color == currentShirtColor)
    {
      longSleeveSprite.color = HexToColor("#D3D3D3");
    };
    if(shortSleeveSprite.color == currentShirtColor)
    {
      shortSleeveSprite.color = HexToColor("#D3D3D3");
    };
    currentShirtColor = HexToColor("#D3D3D3");
  }

  public void SetShirtColor9()
  {
    if(throatSprite.color == currentShirtColor)
    {
      throatSprite.color = HexToColor("#F08080");
    };
    if(collarSprite.color == currentShirtColor)
    {
      collarSprite.color = HexToColor("#F08080");
    };
    if(torsoSprite.color == currentShirtColor)
    {
      torsoSprite.color = HexToColor("#F08080");
    };
    if(waistSprite.color == currentShirtColor)
    {
      waistSprite.color = HexToColor("#F08080");
    };
    if(longSleeveSprite.color == currentShirtColor)
    {
      longSleeveSprite.color = HexToColor("#F08080");
    };
    if(shortSleeveSprite.color == currentShirtColor)
    {
      shortSleeveSprite.color = HexToColor("#F08080");
    };
    currentShirtColor = HexToColor("#F08080");
  }

  public void SetShirtColor10()
  {
    if(throatSprite.color == currentShirtColor)
    {
      throatSprite.color = HexToColor("#2F4F4F");
    };
    if(collarSprite.color == currentShirtColor)
    {
      collarSprite.color = HexToColor("#2F4F4F");
    };
    if(torsoSprite.color == currentShirtColor)
    {
      torsoSprite.color = HexToColor("#2F4F4F");
    };
    if(waistSprite.color == currentShirtColor)
    {
      waistSprite.color = HexToColor("#2F4F4F");
    };
    if(longSleeveSprite.color == currentShirtColor)
    {
      longSleeveSprite.color = HexToColor("#2F4F4F");
    };
    if(shortSleeveSprite.color == currentShirtColor)
    {
      shortSleeveSprite.color = HexToColor("#2F4F4F");
    };
    currentShirtColor = HexToColor("#2F4F4F");
  }

  public void SetShirtColor11()
  {
    if(throatSprite.color == currentShirtColor)
    {
      throatSprite.color = HexToColor("#FF34F4");
    };
    if(collarSprite.color == currentShirtColor)
    {
      collarSprite.color = HexToColor("#FF34F4");
    };
    if(torsoSprite.color == currentShirtColor)
    {
      torsoSprite.color = HexToColor("#FF34F4");
    };
    if(waistSprite.color == currentShirtColor)
    {
      waistSprite.color = HexToColor("#FF34F4");
    };
    if(longSleeveSprite.color == currentShirtColor)
    {
      longSleeveSprite.color = HexToColor("#FF34F4");
    };
    if(shortSleeveSprite.color == currentShirtColor)
    {
      shortSleeveSprite.color = HexToColor("#FF34F4");
    };
    currentShirtColor = HexToColor("#FF34F4");
  }





public void SetPantsColor1()
{
    if(waistShortsSprite.color == currentPantsColor)
    {
        waistShortsSprite.color = HexToColor("#2C3E50");
    }
    if(waistSprite.color == currentPantsColor)
    {
        waistSprite.color = HexToColor("#2C3E50");
    }
    if(kneesShinsSprite.color == currentPantsColor)
    {
        kneesShinsSprite.color = HexToColor("#2C3E50");
    }
    if(anklesSprite.color == currentPantsColor)
    {
        anklesSprite.color = HexToColor("#2C3E50");
    }
    currentPantsColor = HexToColor("#2C3E50");
}

public void SetPantsColor2()
{
    if(waistShortsSprite.color == currentPantsColor)
    {
        waistShortsSprite.color = HexToColor("#A0522D");
    }
    if(waistSprite.color == currentPantsColor)
    {
        waistSprite.color = HexToColor("#A0522D");
    }
    if(kneesShinsSprite.color == currentPantsColor)
    {
        kneesShinsSprite.color = HexToColor("#A0522D");
    }
    if(anklesSprite.color == currentPantsColor)
    {
        anklesSprite.color = HexToColor("#A0522D");
    }
    currentPantsColor = HexToColor("#A0522D");
}

public void SetPantsColor3()
{
    if(waistShortsSprite.color == currentPantsColor)
    {
        waistShortsSprite.color = HexToColor("#808080");
    }
    if(waistSprite.color == currentPantsColor)
    {
        waistSprite.color = HexToColor("#808080");
    }
    if(kneesShinsSprite.color == currentPantsColor)
    {
        kneesShinsSprite.color = HexToColor("#808080");
    }
    if(anklesSprite.color == currentPantsColor)
    {
        anklesSprite.color = HexToColor("#808080");
    }
    currentPantsColor = HexToColor("#808080");
}

public void SetPantsColor4()
{
    if(waistShortsSprite.color == currentPantsColor)
    {
        waistShortsSprite.color = HexToColor("#556B2F");
    }
    if(waistSprite.color == currentPantsColor)
    {
        waistSprite.color = HexToColor("#556B2F");
    }
    if(kneesShinsSprite.color == currentPantsColor)
    {
        kneesShinsSprite.color = HexToColor("#556B2F");
    }
    if(anklesSprite.color == currentPantsColor)
    {
        anklesSprite.color = HexToColor("#556B2F");
    }
    currentPantsColor = HexToColor("#556B2F");
}

public void SetPantsColor5()
{
    if(waistShortsSprite.color == currentPantsColor)
    {
        waistShortsSprite.color = HexToColor("#B0C4DE");
    }
    if(waistSprite.color == currentPantsColor)
    {
        waistSprite.color = HexToColor("#B0C4DE");
    }
    if(kneesShinsSprite.color == currentPantsColor)
    {
        kneesShinsSprite.color = HexToColor("#B0C4DE");
    }
    if(anklesSprite.color == currentPantsColor)
    {
        anklesSprite.color = HexToColor("#B0C4DE");
    }
    currentPantsColor = HexToColor("#B0C4DE");
}

public void SetPantsColor6()
{
    if(waistShortsSprite.color == currentPantsColor)
    {
        waistShortsSprite.color = HexToColor("#4B0082");
    }
    if(waistSprite.color == currentPantsColor)
    {
        waistSprite.color = HexToColor("#4B0082");
    }
    if(kneesShinsSprite.color == currentPantsColor)
    {
        kneesShinsSprite.color = HexToColor("#4B0082");
    }
    if(anklesSprite.color == currentPantsColor)
    {
        anklesSprite.color = HexToColor("#4B0082");
    }
    currentPantsColor = HexToColor("#4B0082");
}

public void SetPantsColor7()
{
    if(waistShortsSprite.color == currentPantsColor)
    {
        waistShortsSprite.color = HexToColor("#8B4513");
    }
    if(waistSprite.color == currentPantsColor)
    {
        waistSprite.color = HexToColor("#8B4513");
    }
    if(kneesShinsSprite.color == currentPantsColor)
    {
        kneesShinsSprite.color = HexToColor("#8B4513");
    }
    if(anklesSprite.color == currentPantsColor)
    {
        anklesSprite.color = HexToColor("#8B4513");
    }
    currentPantsColor = HexToColor("#8B4513");
}

public void SetPantsColor8()
{
    if(waistShortsSprite.color == currentPantsColor)
    {
        waistShortsSprite.color = HexToColor("#696969");
    }
    if(waistSprite.color == currentPantsColor)
    {
        waistSprite.color = HexToColor("#696969");
    }
    if(kneesShinsSprite.color == currentPantsColor)
    {
        kneesShinsSprite.color = HexToColor("#696969");
    }
    if(anklesSprite.color == currentPantsColor)
    {
        anklesSprite.color = HexToColor("#696969");
    }
    currentPantsColor = HexToColor("#696969");
}

public void SetPantsColor9()
{
    if(waistShortsSprite.color == currentPantsColor)
    {
        waistShortsSprite.color = HexToColor("#4682B4");
    }
    if(waistSprite.color == currentPantsColor)
    {
        waistSprite.color = HexToColor("#4682B4");
    }
    if(kneesShinsSprite.color == currentPantsColor)
    {
        kneesShinsSprite.color = HexToColor("#4682B4");
    }
    if(anklesSprite.color == currentPantsColor)
    {
        anklesSprite.color = HexToColor("#4682B4");
    }
    currentPantsColor = HexToColor("#4682B4");
}

public void SetPantsColor10()
{
    if(waistShortsSprite.color == currentPantsColor)
    {
        waistShortsSprite.color = HexToColor("#000000");
    }
    if(waistSprite.color == currentPantsColor)
    {
        waistSprite.color = HexToColor("#000000");
    }
    if(kneesShinsSprite.color == currentPantsColor)
    {
        kneesShinsSprite.color = HexToColor("#000000");
    }
    if(anklesSprite.color == currentPantsColor)
    {
        anklesSprite.color = HexToColor("#000000");
    }
    currentPantsColor = HexToColor("#000000");
}

public void SetJakettoColor1()
{
    currentJakettoColor = HexToColor("#556B2F");  // Dark Olive
    characterCustomization.UpdateJaketto();
}

public void SetJakettoColor2()
{
    currentJakettoColor = HexToColor("#800020");  // Deep Burgundy
    characterCustomization.UpdateJaketto();
}

public void SetJakettoColor3()
{
    currentJakettoColor = HexToColor("#6A5ACD");  // Slate Blue
    characterCustomization.UpdateJaketto();
}

public void SetJakettoColor4()
{
    currentJakettoColor = HexToColor("#FFDB58");  // Mustard Yellow
    characterCustomization.UpdateJaketto();
}

public void SetJakettoColor5()
{
    currentJakettoColor = HexToColor("#36454F");  // Charcoal
    characterCustomization.UpdateJaketto();
}

public void SetJakettoColor6()
{
    currentJakettoColor = HexToColor("#228B22");  // Forest Green
    characterCustomization.UpdateJaketto();
}

public void SetJakettoColor7()
{
    currentJakettoColor = HexToColor("#000080");  // Navy Blue
    characterCustomization.UpdateJaketto();
}

public void SetJakettoColor8()
{
    currentJakettoColor = HexToColor("#8B4513");  // Chocolate Brown
    characterCustomization.UpdateJaketto();
}

public void SetJakettoColor9()
{
    currentJakettoColor = HexToColor("#008080");  // Teal
    characterCustomization.UpdateJaketto();
}

public void SetJakettoColor10()
{
    currentJakettoColor = HexToColor("#800000");  // Maroon
    characterCustomization.UpdateJaketto();
}

  

    private void GetSpritesAndAddToLists(GameObject obj, List<GameObject> spriteList, List<Color> colorList)
    {
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(obj);

        while (stack.Count > 0)
        {
            GameObject currentNode = stack.Pop();
            SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                Color col = sr.color;
                spriteList.Add(currentNode);
                colorList.Add(col);
            }

            foreach (Transform child in currentNode.transform)
            {
                    stack.Push(child.gameObject);
            }
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

    public IEnumerator DeactivateSpaceBar()
    {
        spaceBarDeactivated = true;

        yield return new WaitForSeconds(5f);

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
            // spaceBarDeactivated = false;
        }
    }
    // public void ModifyBlendTree(int n, float amountX, float amountY)
    // {
    //         // animator.baseLayer.walkingTree
    //         animator.baseLayer.walkingTree.children[n].position = new Vector2(blendTree.children[n].position.x, amountX);
    //         animator.baseLayer.walkingTree.children[n].position = new Vector2(blendTree.children[n].position.y, amountY);
    //         Debug.Log("Modified BlendTree motion position.");
    // }


    // void HandlePointAndClickMovement()
    // {
    //     // Detect left mouse button click (0 = left mouse button)
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         // Get the position of the mouse click in world coordinates
    //         Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         targetPosition = new Vector3(mousePosition.x, mousePosition.y, 0f);

    //         // Enable point-and-click movement
    //         isMovingToClick = true;
    //     }

    //     // If point-and-click is enabled, move toward the clicked position
    //     if (isMovingToClick)
    //     {
    //         change = (targetPosition - transform.position).normalized;

    //         // Stop moving if the player reaches the target position
    //         if (Vector3.Distance(transform.position, targetPosition) < 1f)
    //         {
    //             isMovingToClick = false;
    //             change = Vector3.zero;
    //             // animator.SetBool("moving", false);            
    //         }
    //     }
    // }
}



