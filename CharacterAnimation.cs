using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;  // This is necessary for using TMP_InputField


public class CharacterAnimation : MonoBehaviour
{
    [HideInInspector] public List<GameObject> characterSpriteList = new List<GameObject>();
    [HideInInspector] public List<Color> initialChrctrColorList = new List<Color>();

    [HideInInspector] public float initialAnimationSpeed;


    IsoSpriteSorting IsoSpriteSorting; 

    [HideInInspector] public int animDirectionAdjustorInt = 26;

    [HideInInspector] public int rightDownAnim;
    [HideInInspector] public int leftDownAnim;
    [HideInInspector] public int rightAnim;
    [HideInInspector] public int leftAnim;
    [HideInInspector] public int upRightAnim;
    [HideInInspector] public int upLeftAnim;
    [HideInInspector] public int ladderAnimDirectionIndex;
    [HideInInspector] public int idle = 0;
    [HideInInspector] public int walk = 1;
    [HideInInspector] public int run = 5;
    [HideInInspector] public int sit = 9;
    [HideInInspector] public int climb = 11;
    [HideInInspector] public int rideBike = 15;




    [HideInInspector] public Sprite[] allHeadSprites; // Array to hold all 156 sprites from the sprite sheet VV
    [HideInInspector] public Sprite[] allEyeSprites; 
    [HideInInspector] public Sprite[] allThroatSprites; 
    [HideInInspector] public Sprite[] allCollarSprites;
    [HideInInspector] public Sprite[] allTorsoSprites;
    [HideInInspector] public Sprite[] allWaistSprites; 
    [HideInInspector] public Sprite[] allWaistShortsSprites; 
    [HideInInspector] public Sprite[] allKneesShinsSprites; 
    [HideInInspector] public Sprite[] allAnklesSprites; 
    [HideInInspector] public Sprite[] allFeetSprites; 
    [HideInInspector] public Sprite[] allDressSprites; 
    [HideInInspector] public Sprite[] allJakettoSprites; 
    [HideInInspector] public Sprite[] allLongSleeveSprites; 
    [HideInInspector] public Sprite[] allHandSprites; 
    [HideInInspector] public Sprite[] allShortSleeveSprites; 
    [HideInInspector] public Sprite[] allMohawk5TopSprites; 
    [HideInInspector] public Sprite[] allMohawk5BottomSprites; 
    [HideInInspector] public Sprite[] allHair0TopSprites; 
    [HideInInspector] public Sprite[] allHair0BottomSprites; 
    [HideInInspector] public Sprite[] allHair1TopSprites; 
    [HideInInspector] public Sprite[] allHair7TopSprites; 
    [HideInInspector] public Sprite[] allHair8TopSprites; 
    [HideInInspector] public Sprite[] allHair1BottomSprites; 
    [HideInInspector] public Sprite[] allHair2BottomSprites; 
    [HideInInspector] public Sprite[] allHair3BottomSprites; 
    [HideInInspector] public Sprite[] allHair4BottomSprites; 
    [HideInInspector] public Sprite[] allHair6BottomSprites; 
    [HideInInspector] public Sprite[] allHair7BottomSprites; 
    [HideInInspector] public Sprite[] allHair8BottomSprites; 
    [HideInInspector] public Sprite[] allHairFringe1Sprites; 
    [HideInInspector] public Sprite[] allHairFringe2Sprites; 
    [HideInInspector] public Sprite[] allBikeSprites; 

    [HideInInspector] public float animationSpeed = 0.09f; // Time between frames

    [HideInInspector] public SpriteRenderer headSprite;
    [HideInInspector] public SpriteRenderer eyeSprite;
    [HideInInspector] public SpriteRenderer throatSprite;
    [HideInInspector] public SpriteRenderer collarSprite;
    [HideInInspector] public SpriteRenderer torsoSprite;
    [HideInInspector] public SpriteRenderer waistSprite;
    [HideInInspector] public SpriteRenderer waistShortsSprite;
    [HideInInspector] public SpriteRenderer kneesShinsSprite;
    [HideInInspector] public SpriteRenderer anklesSprite;
    [HideInInspector] public SpriteRenderer feetSprite;
    [HideInInspector] public SpriteRenderer jakettoSprite;
    [HideInInspector] public SpriteRenderer dressSprite;
    [HideInInspector] public SpriteRenderer longSleeveSprite;
    [HideInInspector] public SpriteRenderer handSprite;
    [HideInInspector] public SpriteRenderer shortSleeveSprite;
    [HideInInspector] public SpriteRenderer mohawk5TopSprite;
    [HideInInspector] public SpriteRenderer mohawk5BottomSprite;
    [HideInInspector] public SpriteRenderer hair0TopSprite;
    [HideInInspector] public SpriteRenderer hair0BottomSprite;
    [HideInInspector] public SpriteRenderer hair1TopSprite;
    [HideInInspector] public SpriteRenderer hair7TopSprite;
    [HideInInspector] public SpriteRenderer hair8TopSprite;
    [HideInInspector] public SpriteRenderer hair1BottomSprite;
    [HideInInspector] public SpriteRenderer hair2BottomSprite;
    [HideInInspector] public SpriteRenderer hair3BottomSprite;
    [HideInInspector] public SpriteRenderer hair4BottomSprite;
    [HideInInspector] public SpriteRenderer hair6BottomSprite;
    [HideInInspector] public SpriteRenderer hair7BottomSprite;
    [HideInInspector] public SpriteRenderer hair8BottomSprite;
    [HideInInspector] public SpriteRenderer hairFringe1Sprite;
    [HideInInspector] public SpriteRenderer hairFringe2Sprite;


    BikeTransformAdjustment bikeTransformAdjustment;
    BikeScript bikeScript;

    [SerializeField] GameObject bikeStatic;
    [HideInInspector] public SpriteRenderer bikeSprite;
    [HideInInspector] public Color currentBikeColor;
    [HideInInspector] public Color bikeInitialColor;

    [HideInInspector] public bool spaceBarDeactivated;

    [HideInInspector] public Coroutine allowTimeForSpaceBarCoro;


    [HideInInspector] public bool playerSitting = false;


    [HideInInspector] public float timer;
    [HideInInspector] public int currentFrame;
    [HideInInspector] public int movementStartIndex;
    [HideInInspector] public int movementFrameCount;
    [HideInInspector] public int[] movementIndices;

    [HideInInspector] public int currentAnimationDirection = 0;

    [HideInInspector] public int bodyTypeNumber;
    [HideInInspector] public int bodyTypeIndexMultiplier = 156;

    CharacterCustomization characterCustomization;

    CharacterMovement characterMovement;

    [HideInInspector] public Color currentSkinColor;
    [HideInInspector] public Color currentHairColor;
    [HideInInspector] public Color currentShirtColor;
    [HideInInspector] public Color currentPantsColor;
    [HideInInspector] public Color currentShoeColor;
    [HideInInspector] public Color currentJakettoColor;


  [HideInInspector] public TMP_InputField[] inputFields;





  public bool playerIsInside()
  {
  if (!characterMovement.playerIsOutside)
  {
      return true;
  }
  return false;
  }

  // Start is called before the first frame update
  void Awake()    
  {
    characterMovement = GetComponent<CharacterMovement>();
    // cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
    GetSpritesAndAddToLists(this.gameObject, characterSpriteList, initialChrctrColorList);
    IsoSpriteSorting = GetComponent<IsoSpriteSorting>();
    // animator = GetComponent<Animator>();


    characterCustomization = GetComponent<CharacterCustomization>();
    bikeTransformAdjustment = GetComponent<BikeTransformAdjustment>();


    // Find all input fields in the scene
    inputFields = FindObjectsOfType<TMP_InputField>();


    // initialBCOffset = character.GetComponent<BoxCollider2D>().offset;
    // boxCol2DRunLeftOffset = new Vector2(0.5f,0f);

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
    allDressSprites = Resources.LoadAll<Sprite>("dress");
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
    dressSprite = transform.Find("dress").GetComponent<SpriteRenderer>();
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
    dressSprite.sprite = allDressSprites[bodyTypeNumber * bodyTypeIndexMultiplier];
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
    currentHairColor = (HexToColor("#F5CBA7"));
    currentShirtColor = (HexToColor("#aaaaaa"));
    currentPantsColor = (HexToColor("#00B6FF"));
    currentShoeColor = (HexToColor("#AB4918"));
    currentJakettoColor = (HexToColor("#AB4918"));

  }


    void Start()
    {
      // if(this.gameObject.tag == "Player")
      // {
      //   characterCustomization.UpdateRandom();
      // }
    }

  // Update is called once per frame
    public void Update()
    {
        // if ((Input.GetKeyUp(KeyCode.Space) || 
        //     Input.GetKeyUp(KeyCode.JoystickButton0) ||  // A button
        //     Input.GetKeyUp(KeyCode.JoystickButton1) ||  // B button
        //     Input.GetKeyUp(KeyCode.JoystickButton2) ||  // X button
        //     Input.GetKeyUp(KeyCode.JoystickButton3)))
        // {
        //     spaceBarDeactivated = false;
        // }
    }

    public void Animate(int movementStartIndex, int movementFrameCount, int animationDirection, int bodyTypeNumber)
    {
      if(this.gameObject.tag == "Player")
      {        
        bikeSprite.color = currentBikeColor;
        // if (Input.GetKey(KeyCode.B) && !playerOnThresh && playerIsOutside && change != Vector3.zero)
        if (characterMovement.playerOnBike == true)
        {
        //RideBike
        RideBike();
        }
        else if(playerSitting == true)
        {
        Sit();
        }
        else if (characterMovement.motionDirection == "upDownLadder"|| characterMovement.motionDirection == "upLadder" || characterMovement.motionDirection == "downLadder")
        {
        ClimbLadder();
        }
        else if ((Input.GetKey(KeyCode.Space) || 
        Input.GetKey(KeyCode.JoystickButton0) ||  // A button
        Input.GetKey(KeyCode.JoystickButton1) ||  // B button
        Input.GetKey(KeyCode.JoystickButton2) ||  // X button
        Input.GetKey(KeyCode.JoystickButton3))  && !characterMovement.playerOnThresh && characterMovement.playerIsOutside && characterMovement.change != Vector3.zero)
        {
        Run();
        }
        else if (characterMovement.change != Vector3.zero)
        {
        Walk();
        }


        currentAnimationDirection = animationDirection;

        // Check if space was released before this code
        if(characterMovement.spaceBarDeactivated == false)
        {
            if ((Input.GetKey(KeyCode.Space) || 
        Input.GetKey(KeyCode.JoystickButton0) ||  // A button
        Input.GetKey(KeyCode.JoystickButton1) ||  // B button
        Input.GetKey(KeyCode.JoystickButton2) ||  // X button
        Input.GetKey(KeyCode.JoystickButton3)) && characterMovement.playerIsOutside && characterMovement.playerOnBike)
            {
                // StopBikeFunction
                characterMovement.StartDeactivateSpaceBar();
                characterMovement.playerOnBike = false;
                bikeScript.GetOffBoike();
                bikeStatic.transform.position = transform.position + new Vector3(8, -22, 0);
                currentBikeColor.a = 0f;
            }
        }
        

        if(characterMovement.playerOnThresh && characterMovement.playerOnBike == true)
        {
            //StopBikeFunctionWhile Entering Building

          if(characterMovement.change.x > 0 && characterMovement.change.y > 0)//up right
          {  
              bikeStatic.transform.position = transform.position + new Vector3(8,-22,0) + new Vector3(-16,-8,0);
          }
          if(characterMovement.change.x < 0 && characterMovement.change.y < 0)//down left
          {  
              bikeStatic.transform.position = transform.position + new Vector3(8,-22,0) + new Vector3(16,8,0);
          }
          if(characterMovement.change.x < 0 && characterMovement.change.y > 0)//up left
          {  
              bikeStatic.transform.position = transform.position + new Vector3(8,-22,0) + new Vector3(16,-8,0);
          }
          if(characterMovement.change.x > 0 && characterMovement.change.y < 0)// down right
          {  
              bikeStatic.transform.position = transform.position + new Vector3(8,-22,0) + new Vector3(-16,8,0);
          }
          
          characterMovement.playerOnBike = false;
          bikeScript.GetOffBoike();
          currentBikeColor.a = 0f;
        }
      }
      else
      {
        if (characterMovement.change != Vector3.zero)
          {
          Walk();
          }
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
            dressSprite.sprite = allDressSprites[movementIndices[currentFrame]];
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

    public void RideBike()
    {
        bikeTransformAdjustment.SetBikeTransformPosition();

        currentBikeColor.a = 1f;
        movementStartIndex = rideBike;
        movementFrameCount = 4;
        characterMovement.movementSpeed = characterMovement.initialMovementSpeed + 50;
        animationSpeed = 0.08f;      
        if(characterMovement.change.x > 0)
        {
          bikeSprite.flipX = true;
        }
        else if (characterMovement.change.x < 0)
        {
          bikeSprite.flipX = false;
        }

        if(characterMovement.change.y > 0)
        {
          bikeSprite.sprite = allBikeSprites[0];
        }
        else if (characterMovement.change.y < 0)
        {
          bikeSprite.sprite = allBikeSprites[1];
        }
    }

    public void Sit()
    {
      movementStartIndex = sit;
      movementFrameCount = 1;
    }

    public void ClimbLadder()
    {
      movementStartIndex = climb;
      movementFrameCount = 4;
      animationSpeed = initialAnimationSpeed;
      currentBikeColor.a = 0f;
    }

    public void Run()
    {    
      movementStartIndex = run;
      movementFrameCount = 4;
      characterMovement.movementSpeed = characterMovement.initialMovementSpeed + 20;
      animationSpeed = 0.11f;
      currentBikeColor.a = 0f;
    } 

    public void Walk()
    {
      movementFrameCount = 4;
      movementStartIndex = walk;
      characterMovement.movementSpeed = characterMovement.initialMovementSpeed;
      animationSpeed = initialAnimationSpeed;
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

    // public IEnumerator DeactivateSpaceBar()
    // {
    //   while ((Input.GetKey(KeyCode.Space) || 
    //         Input.GetKey(KeyCode.JoystickButton0) ||  // A button
    //         Input.GetKey(KeyCode.JoystickButton1) ||  // B button
    //         Input.GetKey(KeyCode.JoystickButton2) ||  // X button
    //         Input.GetKey(KeyCode.JoystickButton3)))
    //   {
    //     spaceBarDeactivated = true;

    //     yield return null;
    //   }

    //     spaceBarDeactivated = false;
    // }

    // public void StartDeactivateSpaceBar()
    // {
    //     if (allowTimeForSpaceBarCoro != null)
    //     {
    //         StopCoroutine(allowTimeForSpaceBarCoro);
    //     }
    //     allowTimeForSpaceBarCoro = StartCoroutine(DeactivateSpaceBar());
    // }

    // public void StopDeactivateSpaceBar()
    // {
    //     if (allowTimeForSpaceBarCoro != null)
    //     {
    //         StopCoroutine(allowTimeForSpaceBarCoro);
    //         allowTimeForSpaceBarCoro = null;
    //         spaceBarDeactivated = false;
    //     }
    // }
    public void GetSpritesAndAddToLists(GameObject obj, List<GameObject> spriteList, List<Color> colorList)
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

}





















