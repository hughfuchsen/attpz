using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomization : MonoBehaviour 
{
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;


    public List<GameObject> bodyParts = new List<GameObject>();

    public bool lockedSkinColor = false;
    public bool lockedBodyType = false;
    public bool lockedHeight = false;
    public bool lockedWidth = false;
    public bool lockedHair = false;
    public bool lockedShirt = false;
    public bool lockedWaist = false;
    public bool lockedPants = false;
    public bool lockedJaketto = false;
    public bool lockedShoes = false;

    // Declare Image components for each locked attribute
    private Image lockBodyImgComponent;
    private Image lockSkinImgComponent;
    private Image lockHeightImgComponent;
    private Image lockWidthImgComponent;
    private Image lockHairImgComponent;
    private Image lockShirtImgComponent;
    private Image lockWaistImgComponent;
    private Image lockPantsImgComponent;
    private Image lockJakettoImgComponent;
    private Image lockShoesImgComponent;

     // Sprites for locked and unlocked states
    private Sprite lockedImage;
    private Sprite unlockedImage;

    // public Image skinColorButton;
    public Image skinColorButton2;
    public Image hairColorButton;
    public Image shirtColorButton;
    public Image pantsColorButton;
    public Image jackettoColorButton;

    private int[] bodyTypeIndex1 = {5, 17, 29};
    private int[] bodyTypeIndex2 = {9, 21, 33};
    private int[] bodyTypeIndex3 = {1, 13, 25};
    private int[] bodyTypeIndexSet;

    private int[] heightIndex1 = {5,9,1};
    private int[] heightIndex2 = {17,21,13};
    private int[] heightIndex3 = {29, 33, 25};
    private int[] heightIndexSet;

    // private Dictionary<int, int[]> heightOptions = new Dictionary<int, int[]>()
    // {
    //     { 5, new int[] { 5, 9, 1 } },
    //     { 17, new int[] { 17, 21, 13 } },
    //     { 29, new int[] { 29, 33, 25 } }
    // };

    // private Dictionary<int, int[]> widthOptions = new Dictionary<int, int[]>()
    // {
    //     { 1, new int[] { 1, 2, 3, 4 } },
    //     { 5, new int[] { 5, 6, 7, 8 } },
    //     { 9, new int[] { 9, 10, 11, 12 } },  // Add 9 to ensure it maps correctly
    //     { 13, new int[] { 13, 14, 15, 16 } },
    //     { 17, new int[] { 17, 18, 19, 20 } },
    //     { 21, new int[] { 21, 22, 23, 24 } },
    //     { 25, new int[] { 25, 26, 27, 28 } },
    //     { 29, new int[] { 29, 30, 31, 32 } },
    //     { 33, new int[] { 33, 34, 35, 36 } }
    // };

    public int currentBodyTypeIndex = 0; // Index to track current body type
    public int currentHeightIndex = 0;   // Index to track current height
    public int currentWidthIndex = 0;    // Index to track current currentWidthIndex
    public int currentHairStyleIndex = 0;    //"" "" "" 
    public int currentHairColorIndex = 0;    //"" "" "" 
    public int currentShirtIndex = 0;    //"" "" "" 
    public int currentWaistIndex = 0;   
    public int currentPantsIndex = 0;   
    public int currentFeetIndex = 0;    
    public int currentJakettoIndex = 0;    
    public int currentSkinColorIndex = 0;   
    public int currentShirtColorIndex = 0;   
    public int currentPantsColorIndex = 0;   
    public int currentJakettoColorIndex = 0;   



    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>(); 
      
        int[] bodyTypeIndex1 = {5,17,29};
        int[] bodyTypeIndex2 = {9,21,33};
        int[] bodyTypeIndex3 = {1,13,25};

        int[] heightIndex1 = {5,9,1};
        int[] heightIndex2 = {17,21,13};
        int[] heightIndex3 = {29, 33, 25};

        SetBodyType();


        unlockedImage = Resources.Load<Sprite>("unlockedUI");
        lockedImage = Resources.Load<Sprite>("lockedUI");

         // Find and assign all lock Image components by their tags

        lockBodyImgComponent = GameObject.FindGameObjectWithTag("LockBodyUI").GetComponent<Image>();
        lockSkinImgComponent = GameObject.FindGameObjectWithTag("LockSkinUI").GetComponent<Image>();
        lockHeightImgComponent = GameObject.FindGameObjectWithTag("LockHeightUI").GetComponent<Image>();
        lockWidthImgComponent = GameObject.FindGameObjectWithTag("LockWidthUI").GetComponent<Image>();
        lockHairImgComponent = GameObject.FindGameObjectWithTag("LockHairUI").GetComponent<Image>();
        lockShirtImgComponent = GameObject.FindGameObjectWithTag("LockShirtUI").GetComponent<Image>();
        lockWaistImgComponent = GameObject.FindGameObjectWithTag("LockWaistUI").GetComponent<Image>();
        lockPantsImgComponent = GameObject.FindGameObjectWithTag("LockPantsUI").GetComponent<Image>();
        lockJakettoImgComponent = GameObject.FindGameObjectWithTag("LockJackettoUI").GetComponent<Image>();
        lockShoesImgComponent = GameObject.FindGameObjectWithTag("LockShoesUI").GetComponent<Image>();
        // transform.Find("customiseButtons").gameObject.SetActive(false);

        lockBodyImgComponent.sprite = unlockedImage;
        lockSkinImgComponent.sprite = unlockedImage;
        lockHeightImgComponent.sprite = unlockedImage;
        lockWidthImgComponent.sprite = unlockedImage;
        lockHairImgComponent.sprite = unlockedImage;
        lockShirtImgComponent.sprite = unlockedImage;
        lockWaistImgComponent.sprite = unlockedImage;
        lockPantsImgComponent.sprite = unlockedImage;
        lockJakettoImgComponent.sprite = unlockedImage;
        lockShoesImgComponent.sprite = unlockedImage;


    }

    // Body Type Selection
    public void NextBodyType()
    {
        currentBodyTypeIndex ++; // Increment body type index
        if (currentBodyTypeIndex >= 3)
        {
            currentBodyTypeIndex = 0; // Wrap around to the first body type option
        }
        SetBodyType();
        // SetBodyType();
    }

    // public void PreviousBodyType()
    // {
    //     currentBodyTypeIndex --; // Decrement body type index
    //     if (currentBodyTypeIndex < 0)
    //     {
    //         currentBodyTypeIndex = 2; // Wrap around to the last body type option
    //     }
    //     SetBodyType();
    //     // SetBodyType();    
    // }

    // Height Selection
    public void NextHeight()
    {
        // int[] availableHeights = heightOptions[bodyTypeIndexSet[currentBodyTypeIndex]]; // Get height options for current body type
        currentHeightIndex++;
        if (currentHeightIndex >= 3)
        {
            currentHeightIndex = 0; 
        }
        SetBodyType();
        // SetBodyType();
    }

    // public void PreviousHeight()
    // {
    //     // int[] availableHeights = heightOptions[bodyTypeIndexSet[currentBodyTypeIndex]]; // Get height options for current body type
    //     currentHeightIndex--;
    //     if (currentHeightIndex < 0)
    //     {
    //         currentHeightIndex = 2; 
    //     }
    //     SetBodyType();
    //     // SetBodyType();
    // }

    private void SetBodyType()
    {
        if(currentHeightIndex == 2 && currentBodyTypeIndex == 0)
        {
            playerMovement.bodyTypeNumber = 1 + currentWidthIndex;
        }
        if(currentHeightIndex == 0 && currentBodyTypeIndex == 0)
        {
            playerMovement.bodyTypeNumber = 5 + currentWidthIndex;
        }
        if(currentHeightIndex == 1 && currentBodyTypeIndex == 0)
        {
            playerMovement.bodyTypeNumber = 9 + currentWidthIndex;
        }
        if(currentHeightIndex == 2 && currentBodyTypeIndex == 1)
        {
            playerMovement.bodyTypeNumber = 13 + currentWidthIndex;
        }
        if(currentHeightIndex == 0 && currentBodyTypeIndex == 1)
        {
            playerMovement.bodyTypeNumber = 17 + currentWidthIndex;
        }
        if(currentHeightIndex == 1 && currentBodyTypeIndex == 1)
        {
            playerMovement.bodyTypeNumber = 21 + currentWidthIndex;
        }
        if(currentHeightIndex == 2 && currentBodyTypeIndex == 2)
        {
            playerMovement.bodyTypeNumber = 25 + currentWidthIndex;
        }
        if(currentHeightIndex == 0 && currentBodyTypeIndex == 2)
        {
            playerMovement.bodyTypeNumber = 29 + currentWidthIndex;
        }
        if(currentHeightIndex == 1 && currentBodyTypeIndex == 2)
        {
            playerMovement.bodyTypeNumber = 33 + currentWidthIndex;
        }


        // adjust the box collider on playa
        if(currentWidthIndex == 0)
        {
            Vector2 newSize = Player.GetComponent<BoxCollider2D>().size;
            newSize.x = 6f + currentWidthIndex;
            Player.GetComponent<BoxCollider2D>().size = newSize;
            Vector2 newOffset = Player.GetComponent<BoxCollider2D>().offset;
            // newOffset = new Vector2(16f, -43.5f);
            Player.GetComponent<BoxCollider2D>().offset = newOffset;
        }
        if(currentWidthIndex == 1)
        {
            Vector2 newSize = Player.GetComponent<BoxCollider2D>().size;
            newSize.x = 6f + currentWidthIndex;
            Player.GetComponent<BoxCollider2D>().size = newSize;
            Vector2 newOffset = Player.GetComponent<BoxCollider2D>().offset;
            // newOffset = new Vector2(16f, -43.5f);
            Player.GetComponent<BoxCollider2D>().offset = newOffset;
        }
        if(currentWidthIndex == 2)
        {
            Vector2 newSize = Player.GetComponent<BoxCollider2D>().size;
            newSize.x = 6f + currentWidthIndex;
            Player.GetComponent<BoxCollider2D>().size = newSize;
            Vector2 newOffset = Player.GetComponent<BoxCollider2D>().offset;
            // newOffset = new Vector2(16f, -43.5f);
            Player.GetComponent<BoxCollider2D>().offset = newOffset;
        }
        if(currentWidthIndex == 3)
        {
            Vector2 newSize = Player.GetComponent<BoxCollider2D>().size;
            newSize.x = 6f + currentWidthIndex;
            Player.GetComponent<BoxCollider2D>().size = newSize;
            Vector2 newOffset = Player.GetComponent<BoxCollider2D>().offset;
            // newOffset = new Vector2(16f, -43.5f);
            Player.GetComponent<BoxCollider2D>().offset = newOffset;
        }
    }

    private int[] widthIndexSet = {0, 1, 2, 3};


    // Width Selection
    public void NextWidth()
    {
        currentWidthIndex++;
        if (currentWidthIndex >= widthIndexSet.Length)
        {
            currentWidthIndex = 0; // Wrap around to the first currentWidthIndex option
        }
        SetBodyType();
        // SetBodyType();
    }

    public void PreviousWidth()
    {
        currentWidthIndex--;
        if (currentWidthIndex < 0)
        {
            currentWidthIndex = widthIndexSet.Length - 1; 
        }
        SetBodyType();
        // SetBodyType();
    }

    // private void SetWidth()
    // {
    //     int[] availableWidths = widthOptions[playerMovement.bodyTypeNumber];
    //     playerMovement.bodyTypeNumber = availableWidths[currentWidthIndex];
    //     Debug.Log("Width: " + playerMovement.bodyTypeNumber); // Log current currentWidthIndex
    // }


    public void NextHairStyle()
    {
        currentHairStyleIndex++;
        // if (currentHairStyleIndex >= 10)
        // {
        //     currentHairStyleIndex = 0; // Wrap around to the first currentWidthIndex option
        // }
        if (currentHairStyleIndex >= 13)
        {
            currentHairStyleIndex = 0; // Wrap around to the first currentWidthIndex option
        }
        UpdateHairStyle();
    }

    public void UpdateHairStyle()
    {

        if(currentHairStyleIndex == 0)
        {
            playerMovement.SetHairStyle1();
        }
        else if(currentHairStyleIndex == 1)
        {
            playerMovement.SetHairStyle2();
        }
        else if(currentHairStyleIndex == 2)
        {
            playerMovement.SetHairStyle3();
        }
        else if(currentHairStyleIndex == 3)
        {
            playerMovement.SetHairStyle4();
        }
        else if(currentHairStyleIndex == 4)
        {
            playerMovement.SetHairStyle5();
        }
        else if(currentHairStyleIndex == 5)
        {
            playerMovement.SetHairStyle6();
        }
        else if(currentHairStyleIndex == 6)
        {
            playerMovement.SetHairStyle7();
        }
        else if(currentHairStyleIndex == 7)
        {
            playerMovement.SetHairStyle8();
        }
        else if(currentHairStyleIndex == 8)
        {
            playerMovement.SetHairStyle9();
        }
        else if(currentHairStyleIndex == 9)
        {
            playerMovement.SetHairStyle10();
        }
        else if(currentHairStyleIndex == 10)
        {
            playerMovement.SetHairStyle11();
        }
        else if(currentHairStyleIndex == 11)
        {
            playerMovement.SetHairStyle12();
        }
        else if(currentHairStyleIndex == 12)
        {
            playerMovement.SetHairStyle13();
        }




    }

       public void NextHairColor()
    {
        currentHairColorIndex++;
        if (currentHairColorIndex >= 10)
        {
            currentHairColorIndex = 0; // Wrap around to the first currentWidthIndex option
        }
        UpdateHairColor();
    }

    public void UpdateHairColor()
    {

        if(currentHairColorIndex == 0)
        {
            playerMovement.SetHairColor1();
        }
        else if(currentHairColorIndex == 1)
        {
            playerMovement.SetHairColor2();
        }
        else if(currentHairColorIndex == 2)
        {
            playerMovement.SetHairColor3();
        }
        else if(currentHairColorIndex == 3)
        {
            playerMovement.SetHairColor4();
        }
        else if(currentHairColorIndex == 4)
        {
            playerMovement.SetHairColor5();
        }
        else if(currentHairColorIndex == 5)
        {
            playerMovement.SetHairColor6();
        }
        else if(currentHairColorIndex == 6)
        {
            playerMovement.SetHairColor7();
        }
        else if(currentHairColorIndex == 7)
        {
            playerMovement.SetHairColor8();
        }
        else if(currentHairColorIndex == 8)
        {
            playerMovement.SetHairColor9();
        }
        else if(currentHairColorIndex == 9)
        {
            playerMovement.SetHairColor10();
        }
    }
    public void NextShirt()
    {
        currentShirtIndex++;
        if (currentShirtIndex >= 10)
        {
            currentShirtIndex = 0; // Wrap around to the first currentWidthIndex option
        }
        UpdateShirt();
    }

    public void UpdateShirt()
    {
        if(currentShirtIndex == 0)
        {
            playerMovement.SetShirtToBirthdaySuit();
        }
        else if(currentShirtIndex == 1)
        {
            playerMovement.SetShirtToSinglet1();
        }
        else if(currentShirtIndex == 2)
        {
            playerMovement.SetShirtToSinglet2();
        }
        else if(currentShirtIndex == 3)
        {
            playerMovement.SetShirtToSinglet3();
        }
        else if(currentShirtIndex == 4)
        {
            playerMovement.SetShirtToTShirt1();
        }
        else if(currentShirtIndex == 5)
        {
            playerMovement.SetShirtToTShirt2();
        }
        else if(currentShirtIndex == 6)
        {
            playerMovement.SetShirtToTShirt3();
        }
        else if(currentShirtIndex == 7)
        {
            playerMovement.SetShirtToLSShirt1();
        }
        else if(currentShirtIndex == 8)
        {
            playerMovement.SetShirtToLSShirt2();
        }
        else if(currentShirtIndex == 9)
        {
            playerMovement.SetShirtToLSShirt3();
        }
    }


    public void NextWaist()
    {
        currentWaistIndex++;
        if (currentWaistIndex >= 2)
        {
            currentWaistIndex = 0; // Wrap around to the first currentWidthIndex option
        }
        UpdateWaist();
    }

    public void UpdateWaist()
    {
        if(currentWaistIndex == 0)
        {
            playerMovement.SetWaistToShirt();
        }
        else if(currentWaistIndex == 1)
        {
            playerMovement.SetWaistToPants();
        }
        // else if(currentWaistIndex == 2)
        // {
        //     playerMovement.SetWaistToSkin();
        // }
    }
    public void NextPants()
    {
        currentPantsIndex++;
        if (currentPantsIndex >= 4)
        {
            currentPantsIndex = 0; // Wrap around to the first currentWidthIndex option
        }
        UpdatePants();
    }

    public void UpdatePants()
    {
        if(currentPantsIndex == 0)
        {
            playerMovement.SetPantsToShorts();
        }
        else if(currentPantsIndex == 1)
        {
            playerMovement.SetPantsTo3QuarterPants();
        }
        else if(currentPantsIndex == 2)
        {
            playerMovement.SetPantsToPants();
        }
        else if(currentPantsIndex == 3)
        {
            playerMovement.SetPantsToDress();
        }
        // else if(currentPantsIndex == 3)
        // {
        //     playerMovement.SetPantsToBirthdaySuit();
        // }
    }
    
    public void NextJaketto()
    {
        currentJakettoIndex++;
        if (currentJakettoIndex >= 4)
        {
            currentJakettoIndex = 0; // Wrap around to the first currentWidthIndex option
        }
        UpdateJaketto();
    }

    public void UpdateJaketto()
    {
        if(currentJakettoIndex == 0)
        {
            playerMovement.SetNoJaketto();
        }
        else if(currentJakettoIndex == 1 || currentJakettoIndex == 2 || currentJakettoIndex == 3)
        {
            playerMovement.SetJakettoVest();
        }
        else
        {
            currentJakettoIndex = 0;
            UpdateJaketto();          
        }
    }
    
    
    public void NextFeet()
    {
        currentFeetIndex++;
        if (currentFeetIndex >= 3)
        {
            currentFeetIndex = 0; // Wrap around to the first currentWidthIndex option
        }
        UpdateFeet();
    }

    public void UpdateFeet()
    {
        if(currentFeetIndex == 0)
        {
            playerMovement.SetFeetToBoots();
        }
        else if(currentFeetIndex == 1)
        {
            playerMovement.SetFeetToShoes();
        }
        else if(currentFeetIndex == 2)
        {
            playerMovement.SetFeetToBareFoot();
        }
    }
    public void NextSkinColor()
    {
        currentSkinColorIndex++;
        if (currentSkinColorIndex >= 7)
        {
            currentSkinColorIndex = 0; // Wrap around to the first currentWidthIndex option
        }
        UpdateSkinColor();
    }

    public void UpdateSkinColor()
    {
        if(currentSkinColorIndex == 0)
        {
            playerMovement.SetSkinColor1();
        }
        else if(currentSkinColorIndex == 1)
        {
            playerMovement.SetSkinColor2();
        }
        else if(currentSkinColorIndex == 2)
        {
            playerMovement.SetSkinColor3();
        }
        else if(currentSkinColorIndex == 3)
        {
            playerMovement.SetSkinColor4();
        }
        else if(currentSkinColorIndex == 4)
        {
            playerMovement.SetSkinColor5();
        }
        else if(currentSkinColorIndex == 5)
        {
            playerMovement.SetSkinColor6();
        }
        else if(currentSkinColorIndex == 6)
        {
            playerMovement.SetSkinColor7();
        }
    }



    public void NextShirtColor()
    {
        currentShirtColorIndex++;
        if (currentShirtColorIndex >= 11)
        {
            currentShirtColorIndex = 0; // Wrap around to the first currentWidthIndex option
        }
        UpdateShirtColor();
    }

    public void UpdateShirtColor()
    {
        if(currentShirtColorIndex == 0)
        {
            playerMovement.SetShirtColor1();
        }
        else if(currentShirtColorIndex == 1)
        {
            playerMovement.SetShirtColor2();
        }
        else if(currentShirtColorIndex == 2)
        {
            playerMovement.SetShirtColor3();
        }
        else if(currentShirtColorIndex == 3)
        {
            playerMovement.SetShirtColor4();
        }
        else if(currentShirtColorIndex == 4)
        {
            playerMovement.SetShirtColor5();
        }
        else if(currentShirtColorIndex == 5)
        {
            playerMovement.SetShirtColor6();
        }
        else if(currentShirtColorIndex == 6)
        {
            playerMovement.SetShirtColor7();
        }
        else if(currentShirtColorIndex == 7)
        {
            playerMovement.SetShirtColor8();
        }
        else if(currentShirtColorIndex == 8)
        {
            playerMovement.SetShirtColor9();
        }
        else if(currentShirtColorIndex == 9)
        {
            playerMovement.SetShirtColor10();
        }
        else if(currentShirtColorIndex == 10)
        {
            playerMovement.SetShirtColor11();
        }
    }


    public void NextJakettoColor()
    {
        currentJakettoColorIndex++;
        if (currentJakettoColorIndex >= 11)
        {
            currentJakettoColorIndex = 0; // Wrap around to the first currentWidthIndex option
        }
        UpdateJakettoColor();
    }

    public void UpdateJakettoColor()
    {
        if(currentJakettoColorIndex == 0)
        {
            playerMovement.SetJakettoColor1();
        }
        else if(currentJakettoColorIndex == 1)
        {
            playerMovement.SetJakettoColor2();
        }
        else if(currentJakettoColorIndex == 2)
        {
            playerMovement.SetJakettoColor3();
        }
        else if(currentJakettoColorIndex == 3)
        {
            playerMovement.SetJakettoColor4();
        }
        else if(currentJakettoColorIndex == 4)
        {
            playerMovement.SetJakettoColor5();
        }
        else if(currentJakettoColorIndex == 5)
        {
            playerMovement.SetJakettoColor6();
        }
        else if(currentJakettoColorIndex == 6)
        {
            playerMovement.SetJakettoColor7();
        }
        else if(currentJakettoColorIndex == 7)
        {
            playerMovement.SetJakettoColor8();
        }
        else if(currentJakettoColorIndex == 8)
        {
            playerMovement.SetJakettoColor9();
        }
        else if(currentJakettoColorIndex == 9)
        {
            playerMovement.SetJakettoColor10();
        }
    }


   public void NextPantsColor()
    {
        currentPantsColorIndex++;
        if (currentPantsColorIndex >= 10)
        {
            currentPantsColorIndex = 0; // Wrap around to the first currentWidthIndex option
        }
        UpdatePantsColor();
    }

    public void UpdatePantsColor()
    {
        if(currentPantsColorIndex == 0)
        {
            playerMovement.SetPantsColor1();
        }
        else if(currentPantsColorIndex == 1)
        {
            playerMovement.SetPantsColor2();
        }
        else if(currentPantsColorIndex == 2)
        {
            playerMovement.SetPantsColor3();
        }
        else if(currentPantsColorIndex == 3)
        {
            playerMovement.SetPantsColor4();
        }
        else if(currentPantsColorIndex == 4)
        {
            playerMovement.SetPantsColor5();
        }
        else if(currentPantsColorIndex == 5)
        {
            playerMovement.SetPantsColor6();
        }
        else if(currentPantsColorIndex == 6)
        {
            playerMovement.SetPantsColor7();
        }
        else if(currentPantsColorIndex == 7)
        {
            playerMovement.SetPantsColor8();
        }
        else if(currentPantsColorIndex == 8)
        {
            playerMovement.SetPantsColor9();
        }
        else if(currentPantsColorIndex == 9)
        {
            playerMovement.SetPantsColor10();
        }
    }

private void UpdateLockImage()
{
    // thisButtonIsLocked = !thisButtonIsLocked;  // Example placeholder

    // Check all locked states and update their images accordingly
    
    if (lockedSkinColor)
    {
        lockSkinImgComponent.sprite = lockedImage;
    }
    else
    {
        lockSkinImgComponent.sprite = unlockedImage;
    }

    if (lockedBodyType)
    {
        lockBodyImgComponent.sprite = lockedImage;
    }
    else
    {
        lockBodyImgComponent.sprite = unlockedImage;
    }

    if (lockedHeight)
    {
        lockHeightImgComponent.sprite = lockedImage;
    }
    else
    {
        lockHeightImgComponent.sprite = unlockedImage;
    }

    if (lockedWidth)
    {
        lockWidthImgComponent.sprite = lockedImage;
    }
    else
    {
        lockWidthImgComponent.sprite = unlockedImage;
    }

    if (lockedHair)
    {
        lockHairImgComponent.sprite = lockedImage;
    }
    else
    {
        lockHairImgComponent.sprite = unlockedImage;
    }

    if (lockedShirt)
    {
        lockShirtImgComponent.sprite = lockedImage;
    }
    else
    {
        lockShirtImgComponent.sprite = unlockedImage;
    }

    if (lockedWaist)
    {
        lockWaistImgComponent.sprite = lockedImage;
    }
    else
    {
        lockWaistImgComponent.sprite = unlockedImage;
    }

    if (lockedPants)
    {
        lockPantsImgComponent.sprite = lockedImage;
    }
    else
    {
        lockPantsImgComponent.sprite = unlockedImage;
    }

    if (lockedJaketto)
    {
        lockJakettoImgComponent.sprite = lockedImage;
    }
    else
    {
        lockJakettoImgComponent.sprite = unlockedImage;
    }

    if (lockedShoes)
    {
        lockShoesImgComponent.sprite = lockedImage;
    }
    else
    {
        lockShoesImgComponent.sprite = unlockedImage;
    }
}

    public void SkinColorLock()
    {
        lockedSkinColor = !lockedSkinColor;
        UpdateLockImage();
    }

    public void BodyTypeLock()
    {
        lockedBodyType = !lockedBodyType;
        UpdateLockImage();
    }
    public void HeightLock()
    {
        lockedHeight = !lockedHeight;
        UpdateLockImage();
    }
    public void WidthLock()
    {
        lockedWidth = !lockedWidth;
        UpdateLockImage();
    }
    public void HairLock()
    {
        lockedHair = !lockedHair;
        UpdateLockImage();
    }
    public void ShirtLock()
    {
        lockedShirt = !lockedShirt;
        UpdateLockImage();
    }
    public void WaistLock()
    {
        lockedWaist = !lockedWaist;
        UpdateLockImage();
    }
    public void PantsLock()
    {
        lockedPants = !lockedPants;
        UpdateLockImage();
    }
    public void JakettoLock()
    {
        lockedJaketto = !lockedJaketto;
        UpdateLockImage();
    }
    public void ShoesLock()
    {
        lockedShoes = !lockedShoes;
        UpdateLockImage();
    }

    public void UpdateRandom()
    {
        //duplicate if statements because the indexes trip over each other otherwise!!!!!! :) be happy ok!
        if(!lockedShoes)
        {
            currentFeetIndex = Random.Range(0, 3);
            UpdateFeet();
        }
        // if(!lockedPants)
        // {
        //     currentPantsIndex = Random.Range(0, 4);
        //     UpdatePants();
        // }
        if(!lockedWaist)
        {
            currentWaistIndex = Random.Range(0, 2);
            UpdateWaist();
        }
        if(!lockedHair)
        {
            currentHairColorIndex = Random.Range(0, 10);
            UpdateHairColor();
        }
        if(!lockedShirt)
        {
            currentShirtIndex = Random.Range(1, 10); // omit no shirt for rando
            UpdateShirt();
        }
        if(!lockedHeight)
        {
            currentHeightIndex = Random.Range(0, 3);
            SetBodyType();
        }
        if(!lockedBodyType)
        {
            currentBodyTypeIndex = Random.Range(0, 3);
            SetBodyType();
        }
        if(!lockedWidth)
        {
            currentWidthIndex = Random.Range(0, 4);
            SetBodyType();
        }
        if(!lockedPants)
        {
            currentPantsColorIndex = Random.Range(0, 10);
            UpdatePantsColor();
        }
        if(!lockedPants)
        {
            currentPantsIndex = Random.Range(0, 4);
            UpdatePants();
        }
        if(!lockedShirt)
        {
            currentShirtColorIndex = Random.Range(0, 11);
            UpdateShirtColor();
        }
        if(!lockedHair)
        {
            currentHairStyleIndex = Random.Range(0, 13);
            UpdateHairStyle(); 
        }
        if(!lockedJaketto)
        {
            currentJakettoIndex = Random.Range(0, 9);
            UpdateJaketto();
            currentJakettoColorIndex = Random.Range(0, 10);
            UpdateJakettoColor();
        }
        if(!lockedSkinColor)
        {
            currentSkinColorIndex = Random.Range(0, 7);
            UpdateSkinColor();
        }
    }



    
    public void UpdateSpecific(int feet, 
                                int pants, 
                                int waist, 
                                int hairCol, 
                                int shirt, 
                                int height, 
                                int bodyType, 
                                int width, 
                                int pantsCol, 
                                int shirtCol, 
                                int hairStyle, 
                                int jaketto, 
                                int jakettoCol, 
                                int skinCol)
    {
            currentPantsColorIndex = pantsCol;
            UpdatePantsColor();
        
            currentShirtColorIndex = shirtCol;
            UpdateShirtColor();

            currentJakettoColorIndex = jakettoCol;
            // UpdateJakettoColor();

            currentHairColorIndex = hairCol;
            UpdateHairColor();

            currentSkinColorIndex = skinCol;
            UpdateSkinColor();


            currentShirtIndex = shirt;
            // UpdateShirt();

            currentJakettoIndex = jaketto;
            UpdateJakettoColor();

            // UpdateShirt();

            
            currentFeetIndex = feet;
            UpdateFeet();
       
            // currentPantsIndex = pants;
            // UpdatePants();
       
            currentWaistIndex = waist;
            UpdateWaist();
      

       
            currentHeightIndex = height;
            SetBodyType();
       
            currentBodyTypeIndex = bodyType;
            SetBodyType();
       
            currentWidthIndex = width;
            SetBodyType();
      
            currentPantsColorIndex = pantsCol;
            UpdatePantsColor();

            currentPantsIndex = pants;
            UpdatePants();
        
            currentShirtColorIndex = shirtCol;
            UpdateShirtColor();
       
            currentHairStyleIndex = hairStyle;
            UpdateHairStyle();

     
            // currentJakettoIndex = jaketto;
            // UpdateJakettoColor();

      

        
    }

    // public void NextPants()
    // {
    //     currentPantsIndex++;
    //     if (currentPantsIndex >= 3)
    //     {
    //         currentPantsIndex = 0; // Wrap around to the first currentWidthIndex option
    //     }
    //     UpdatePants();
    // }

    // public void UpdatePants()
    // {
    //     if(currentPantsIndex == 0)
    //     {
    //         playerMovement.SetPantsToShorts();
    //     }
    //     else if(currentPantsIndex == 1)
    //     {
    //         playerMovement.SetPantsTo3QuarterPants();
    //     }
    //     else if(currentPantsIndex == 2)
    //     {
    //         playerMovement.SetPantsToPants();
    //     }
    // }




    public void ToggleCustomizeUI()
    {
        GameObject customizeButtons = transform.Find("customiseButtons").gameObject;
        bool isActive = customizeButtons.activeSelf;
        customizeButtons.SetActive(!isActive);  // Toggles the active state
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
