using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomization : MonoBehaviour 
{
    // CharacterMovement playerMovement;
    // CharacterMovement characterAnimation;
    [HideInInspector] public CharacterAnimation characterAnimation;

    // PlayerCntrlerScript characterAnimation;
    public string chrctrAppearance;



    [HideInInspector] public bool lockedSkinColor = false;
    [HideInInspector] public bool lockedBodyType = false;
    [HideInInspector] public bool lockedHeight = false;
    [HideInInspector] public bool lockedWidth = false;
    [HideInInspector] public bool lockedHair = false;
    [HideInInspector] public bool lockedShirt = false;
    [HideInInspector] public bool lockedWaist = false;
    [HideInInspector] public bool lockedPants = false;
    [HideInInspector] public bool lockedJaketto = false;
    [HideInInspector] public bool lockedShoes = false;

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


    [HideInInspector] public int currentBodyTypeIndex = 0; // Index to track current body type
    [HideInInspector] public int currentHeightIndex = 0;   // Index to track current height
    [HideInInspector] public int currentWidthIndex = 0;    // Index to track current currentWidthIndex
    [HideInInspector] public int currentHairStyleIndex = 0;    //"" "" "" 
    [HideInInspector] public int currentHairColorIndex = 0;    //"" "" "" 
    [HideInInspector] public int currentShirtIndex = 0;    //"" "" "" 
    [HideInInspector] public int currentWaistIndex = 0;   
    [HideInInspector] public int currentPantsIndex = 0;   
    [HideInInspector] public int currentFeetIndex = 0;    
    [HideInInspector] public int currentJakettoIndex = 0;    
    [HideInInspector] public int currentSkinColorIndex = 0;   
    [HideInInspector] public int currentShirtColorIndex = 0;   
    [HideInInspector] public int currentPantsColorIndex = 0;   
    [HideInInspector] public int currentJakettoColorIndex = 0;   



    public void Awake()
    {        
        // characterAnimation = GetComponent<CharacterMovement>(); 
        characterAnimation = GetComponent<CharacterAnimation>(); 
        // characterAnimation = playerMovement;
      
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
            characterAnimation.bodyTypeNumber = 1 + currentWidthIndex;
        }
        if(currentHeightIndex == 0 && currentBodyTypeIndex == 0)
        {
            characterAnimation.bodyTypeNumber = 5 + currentWidthIndex;
        }
        if(currentHeightIndex == 1 && currentBodyTypeIndex == 0)
        {
            characterAnimation.bodyTypeNumber = 9 + currentWidthIndex;
        }
        if(currentHeightIndex == 2 && currentBodyTypeIndex == 1)
        {
            characterAnimation.bodyTypeNumber = 13 + currentWidthIndex;
        }
        if(currentHeightIndex == 0 && currentBodyTypeIndex == 1)
        {
            characterAnimation.bodyTypeNumber = 17 + currentWidthIndex;
        }
        if(currentHeightIndex == 1 && currentBodyTypeIndex == 1)
        {
            characterAnimation.bodyTypeNumber = 21 + currentWidthIndex;
        }
        if(currentHeightIndex == 2 && currentBodyTypeIndex == 2)
        {
            characterAnimation.bodyTypeNumber = 25 + currentWidthIndex;
        }
        if(currentHeightIndex == 0 && currentBodyTypeIndex == 2)
        {
            characterAnimation.bodyTypeNumber = 29 + currentWidthIndex;
        }
        if(currentHeightIndex == 1 && currentBodyTypeIndex == 2)
        {
            characterAnimation.bodyTypeNumber = 33 + currentWidthIndex;
        }


        // adjust the box collider on playa
        if(currentWidthIndex == 0)
        {
            Vector2 newSize = GetComponentInChildren<BoxCollider2D>().size;
            newSize.x = 6f + currentWidthIndex;
            GetComponentInChildren<BoxCollider2D>().size = newSize;
            Vector2 newOffset = GetComponentInChildren<BoxCollider2D>().offset;
            // newOffset = new Vector2(16f, -43.5f);
            GetComponentInChildren<BoxCollider2D>().offset = newOffset;
        }
        if(currentWidthIndex == 1)
        {
            Vector2 newSize = GetComponentInChildren<BoxCollider2D>().size;
            newSize.x = 6f + currentWidthIndex;
            GetComponentInChildren<BoxCollider2D>().size = newSize;
            Vector2 newOffset = GetComponentInChildren<BoxCollider2D>().offset;
            // newOffset = new Vector2(16f, -43.5f);
            GetComponentInChildren<BoxCollider2D>().offset = newOffset;
        }
        if(currentWidthIndex == 2)
        {
            Vector2 newSize = GetComponentInChildren<BoxCollider2D>().size;
            newSize.x = 6f + currentWidthIndex;
            GetComponentInChildren<BoxCollider2D>().size = newSize;
            Vector2 newOffset = GetComponentInChildren<BoxCollider2D>().offset;
            // newOffset = new Vector2(16f, -43.5f);
            GetComponentInChildren<BoxCollider2D>().offset = newOffset;
        }
        if(currentWidthIndex == 3)
        {
            Vector2 newSize = GetComponentInChildren<BoxCollider2D>().size;
            newSize.x = 6f + currentWidthIndex;
            GetComponentInChildren<BoxCollider2D>().size = newSize;
            Vector2 newOffset = GetComponentInChildren<BoxCollider2D>().offset;
            // newOffset = new Vector2(16f, -43.5f);
            GetComponentInChildren<BoxCollider2D>().offset = newOffset;
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
    //     int[] availableWidths = widthOptions[bodyTypeNumber];
    //     bodyTypeNumber = availableWidths[currentWidthIndex];
    //     Debug.Log("Width: " + bodyTypeNumber); // Log current currentWidthIndex
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
            SetHairStyle1();
        }
        else if(currentHairStyleIndex == 1)
        {
            SetHairStyle2();
        }
        else if(currentHairStyleIndex == 2)
        {
            SetHairStyle3();
        }
        else if(currentHairStyleIndex == 3)
        {
            SetHairStyle4();
        }
        else if(currentHairStyleIndex == 4)
        {
            SetHairStyle5();
        }
        else if(currentHairStyleIndex == 5)
        {
            SetHairStyle6();
        }
        else if(currentHairStyleIndex == 6)
        {
            SetHairStyle7();
        }
        else if(currentHairStyleIndex == 7)
        {
            SetHairStyle8();
        }
        else if(currentHairStyleIndex == 8)
        {
            SetHairStyle9();
        }
        else if(currentHairStyleIndex == 9)
        {
            SetHairStyle10();
        }
        else if(currentHairStyleIndex == 10)
        {
            SetHairStyle11();
        }
        else if(currentHairStyleIndex == 11)
        {
            SetHairStyle12();
        }
        else if(currentHairStyleIndex == 12)
        {
            SetHairStyle13();
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
            SetHairColor1();
        }
        else if(currentHairColorIndex == 1)
        {
            SetHairColor2();
        }
        else if(currentHairColorIndex == 2)
        {
            SetHairColor3();
        }
        else if(currentHairColorIndex == 3)
        {
            SetHairColor4();
        }
        else if(currentHairColorIndex == 4)
        {
            SetHairColor5();
        }
        else if(currentHairColorIndex == 5)
        {
            SetHairColor6();
        }
        else if(currentHairColorIndex == 6)
        {
            SetHairColor7();
        }
        else if(currentHairColorIndex == 7)
        {
            SetHairColor8();
        }
        else if(currentHairColorIndex == 8)
        {
            SetHairColor9();
        }
        else if(currentHairColorIndex == 9)
        {
            SetHairColor10();
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
            SetShirtToBirthdaySuit();
        }
        else if(currentShirtIndex == 1)
        {
            SetShirtToSinglet1();
        }
        else if(currentShirtIndex == 2)
        {
            SetShirtToSinglet2();
        }
        else if(currentShirtIndex == 3)
        {
            SetShirtToSinglet3();
        }
        else if(currentShirtIndex == 4)
        {
            SetShirtToTShirt1();
        }
        else if(currentShirtIndex == 5)
        {
            SetShirtToTShirt2();
        }
        else if(currentShirtIndex == 6)
        {
            SetShirtToTShirt3();
        }
        else if(currentShirtIndex == 7)
        {
            SetShirtToLSShirt1();
        }
        else if(currentShirtIndex == 8)
        {
            SetShirtToLSShirt2();
        }
        else if(currentShirtIndex == 9)
        {
            SetShirtToLSShirt3();
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
            SetWaistToShirt();
        }
        else if(currentWaistIndex == 1)
        {
            SetWaistToPants();
        }
        // else if(currentWaistIndex == 2)
        // {
        //     SetWaistToSkin();
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
            SetPantsToShorts();
        }
        else if(currentPantsIndex == 1)
        {
            SetPantsTo3QuarterPants();
        }
        else if(currentPantsIndex == 2)
        {
            SetPantsToPants();
        }
        else if(currentPantsIndex == 3)
        {
            SetPantsToDress();
        }
        // else if(currentPantsIndex == 3)
        // {
        //     SetPantsToBirthdaySuit();
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
            SetNoJaketto();
        }
        else if(currentJakettoIndex == 1 || currentJakettoIndex == 2 || currentJakettoIndex == 3)
        {
            SetJakettoVest();
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
            SetFeetToBoots();
        }
        else if(currentFeetIndex == 1)
        {
            SetFeetToShoes();
        }
        else if(currentFeetIndex == 2)
        {
            SetFeetToBareFoot();
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
            SetSkinColor1();
        }
        else if(currentSkinColorIndex == 1)
        {
            SetSkinColor2();
        }
        else if(currentSkinColorIndex == 2)
        {
            SetSkinColor3();
        }
        else if(currentSkinColorIndex == 3)
        {
            SetSkinColor4();
        }
        else if(currentSkinColorIndex == 4)
        {
            SetSkinColor5();
        }
        else if(currentSkinColorIndex == 5)
        {
            SetSkinColor6();
        }
        else if(currentSkinColorIndex == 6)
        {
            SetSkinColor7();
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
            SetShirtColor1();
        }
        else if(currentShirtColorIndex == 1)
        {
            SetShirtColor2();
        }
        else if(currentShirtColorIndex == 2)
        {
            SetShirtColor3();
        }
        else if(currentShirtColorIndex == 3)
        {
            SetShirtColor4();
        }
        else if(currentShirtColorIndex == 4)
        {
            SetShirtColor5();
        }
        else if(currentShirtColorIndex == 5)
        {
            SetShirtColor6();
        }
        else if(currentShirtColorIndex == 6)
        {
            SetShirtColor7();
        }
        else if(currentShirtColorIndex == 7)
        {
            SetShirtColor8();
        }
        else if(currentShirtColorIndex == 8)
        {
            SetShirtColor9();
        }
        else if(currentShirtColorIndex == 9)
        {
            SetShirtColor10();
        }
        else if(currentShirtColorIndex == 10)
        {
            SetShirtColor11();
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
            SetJakettoColor1();
        }
        else if(currentJakettoColorIndex == 1)
        {
            SetJakettoColor2();
        }
        else if(currentJakettoColorIndex == 2)
        {
            SetJakettoColor3();
        }
        else if(currentJakettoColorIndex == 3)
        {
            SetJakettoColor4();
        }
        else if(currentJakettoColorIndex == 4)
        {
            SetJakettoColor5();
        }
        else if(currentJakettoColorIndex == 5)
        {
            SetJakettoColor6();
        }
        else if(currentJakettoColorIndex == 6)
        {
            SetJakettoColor7();
        }
        else if(currentJakettoColorIndex == 7)
        {
            SetJakettoColor8();
        }
        else if(currentJakettoColorIndex == 8)
        {
            SetJakettoColor9();
        }
        else if(currentJakettoColorIndex == 9)
        {
            SetJakettoColor10();
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
            SetPantsColor1();
        }
        else if(currentPantsColorIndex == 1)
        {
            SetPantsColor2();
        }
        else if(currentPantsColorIndex == 2)
        {
            SetPantsColor3();
        }
        else if(currentPantsColorIndex == 3)
        {
            SetPantsColor4();
        }
        else if(currentPantsColorIndex == 4)
        {
            SetPantsColor5();
        }
        else if(currentPantsColorIndex == 5)
        {
            SetPantsColor6();
        }
        else if(currentPantsColorIndex == 6)
        {
            SetPantsColor7();
        }
        else if(currentPantsColorIndex == 7)
        {
            SetPantsColor8();
        }
        else if(currentPantsColorIndex == 8)
        {
            SetPantsColor9();
        }
        else if(currentPantsColorIndex == 9)
        {
            SetPantsColor10();
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
        if(!lockedShoes)
        {
            currentFeetIndex = Random.Range(0, 3);
            UpdateFeet();
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


    public void ResetAppearance()
    {
        if(chrctrAppearance != null)
        {
            Debug.Log("it exists");
            string[] intParams = chrctrAppearance.Split(',').Where(param => !string.IsNullOrWhiteSpace(param)).ToArray();
            if (intParams.Length == 14)
            {
                List<int> parameters = intParams.Select(param => int.Parse(param.Trim())).ToList();
                UpdateSpecific(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4],
                        parameters[5], parameters[6], parameters[7], parameters[8], parameters[9],
                        parameters[10], parameters[11], parameters[12], parameters[13]);
            }
        }
    }







































    public void SetShirtToBirthdaySuit()
    {
      characterAnimation.throatSprite.color = characterAnimation.currentSkinColor;
      characterAnimation.collarSprite.color = characterAnimation.currentSkinColor;
      characterAnimation.torsoSprite.color = characterAnimation.currentSkinColor;
      
      if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
      {
        characterAnimation.shortSleeveSprite.color = characterAnimation.currentSkinColor;
        characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor;
      }
      else if(currentJakettoIndex == 2) // short sleeve jaketto
      {
        characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
        characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor; 
      }
      else if(currentJakettoIndex == 3) //long sleeeve jaketto
      {
        characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
        characterAnimation.longSleeveSprite.color = characterAnimation.currentJakettoColor;
      }

      if(currentWaistIndex == 0) // if waist is set to shirt color
      {
        characterAnimation.waistSprite.color = characterAnimation.currentPantsColor;
      }
    }
    public void SetShirtToSinglet1()
    {
      characterAnimation.throatSprite.color = characterAnimation.currentSkinColor;
      characterAnimation.collarSprite.color = characterAnimation.currentSkinColor;
      characterAnimation.torsoSprite.color = characterAnimation.currentShirtColor;
      if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
      {
        characterAnimation.shortSleeveSprite.color = characterAnimation.currentSkinColor;
        characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor;
      }
      else if(currentJakettoIndex == 2) // short sleeve jaketto
      {
        characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
        characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor; 
      }
      else if(currentJakettoIndex == 3) //long sleeeve jaketto
      {
        characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
        characterAnimation.longSleeveSprite.color = characterAnimation.currentJakettoColor;
      }

      if(currentWaistIndex == 0) // if waist is set to shirt color
      {
        characterAnimation.waistSprite.color = characterAnimation.currentShirtColor;
        currentWaistIndex = 0;
      }
    }
    public void SetShirtToSinglet2()
    {
      characterAnimation.throatSprite.color = characterAnimation.currentSkinColor;
      characterAnimation.collarSprite.color = characterAnimation.currentShirtColor;
      characterAnimation.torsoSprite.color = characterAnimation.currentShirtColor;
      if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
      {
        characterAnimation.shortSleeveSprite.color = characterAnimation.currentSkinColor;
        characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor;
      }
      else if(currentJakettoIndex == 2) // short sleeve jaketto
      {
        characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
        characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor; 
      }
      else if(currentJakettoIndex == 3) //long sleeeve jaketto
      {
        characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
        characterAnimation.longSleeveSprite.color = characterAnimation.currentJakettoColor;
      }

      if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
      {
        characterAnimation.waistSprite.color = characterAnimation.currentShirtColor;
      }
    }
    public void SetShirtToSinglet3()
    {
      characterAnimation.throatSprite.color = characterAnimation.currentShirtColor;
      characterAnimation.collarSprite.color = characterAnimation.currentShirtColor;
      characterAnimation.torsoSprite.color = characterAnimation.currentShirtColor;
      if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
      {
        characterAnimation.shortSleeveSprite.color = characterAnimation.currentSkinColor;
        characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor;
      }
      else if(currentJakettoIndex == 2) // short sleeve jaketto
      {
        characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
        characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor; 
      }
      else if(currentJakettoIndex == 3) //long sleeeve jaketto
      {
        characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
        characterAnimation.longSleeveSprite.color = characterAnimation.currentJakettoColor;
      }

      if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
      {
        characterAnimation.waistSprite.color = characterAnimation.currentShirtColor;
      }
    }
    public void SetShirtToTShirt1()
    {
        characterAnimation.throatSprite.color = characterAnimation.currentSkinColor;
        characterAnimation.collarSprite.color = characterAnimation.currentShirtColor;
        characterAnimation.torsoSprite.color = characterAnimation.currentShirtColor;

        if (currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentShirtColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor;
        }
        else if (currentJakettoIndex == 2) // short sleeve jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor;
        }
        else if (currentJakettoIndex == 3) // long sleeve jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentJakettoColor;
        }

        if (characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetShirtToTShirt2()
    {
        characterAnimation.throatSprite.color = characterAnimation.currentSkinColor;
        characterAnimation.collarSprite.color = characterAnimation.currentSkinColor;
        characterAnimation.torsoSprite.color = characterAnimation.currentShirtColor;

        if (currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentShirtColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor;
        }
        else if (currentJakettoIndex == 2) // short sleeve jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor;
        }
        else if (currentJakettoIndex == 3) // long sleeve jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentJakettoColor;
        }

        if (characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetShirtToTShirt3()
    {
        characterAnimation.throatSprite.color = characterAnimation.currentShirtColor;
        characterAnimation.collarSprite.color = characterAnimation.currentShirtColor;
        characterAnimation.torsoSprite.color = characterAnimation.currentShirtColor;

        if (currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentShirtColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor;
        }
        else if (currentJakettoIndex == 2) // short sleeve jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentSkinColor;
        }
        else if (currentJakettoIndex == 3) // long sleeve jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentJakettoColor;
        }

        if (characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetShirtToLSShirt1()
        {
        characterAnimation.throatSprite.color = characterAnimation.currentSkinColor;
        characterAnimation.collarSprite.color = characterAnimation.currentShirtColor;
        characterAnimation.torsoSprite.color = characterAnimation.currentShirtColor;

        if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentShirtColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentShirtColor;
        }
                else if(currentJakettoIndex == 2) // short sleeve jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentShirtColor; 
        }
        else if(currentJakettoIndex == 3) //long sleeeve jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentJakettoColor;
        }


        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = characterAnimation.currentShirtColor;
        }
        }
        public void SetShirtToLSShirt2()
        {
        characterAnimation.throatSprite.color = characterAnimation.currentSkinColor;
        characterAnimation.collarSprite.color = characterAnimation.currentSkinColor;
        characterAnimation.torsoSprite.color = characterAnimation.currentShirtColor;

        if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentShirtColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentShirtColor;
        }
        else if(currentJakettoIndex == 2) // short sleeve jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentShirtColor; 
        }
        else if(currentJakettoIndex == 3) //long sleeeve jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentJakettoColor;
        }

        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = characterAnimation.currentShirtColor;
        }
        }
        public void SetShirtToLSShirt3()
        {
        characterAnimation.throatSprite.color = characterAnimation.currentShirtColor;
        characterAnimation.collarSprite.color = characterAnimation.currentShirtColor;
        characterAnimation.torsoSprite.color = characterAnimation.currentShirtColor;
        
        if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentShirtColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentShirtColor;
        }
        else if(currentJakettoIndex == 2) // short sleeve jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentShirtColor; 
        }
        else if(currentJakettoIndex == 3) //long sleeeve jaketto
        {
            characterAnimation.shortSleeveSprite.color = characterAnimation.currentJakettoColor;
            characterAnimation.longSleeveSprite.color = characterAnimation.currentJakettoColor;
        }

        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = characterAnimation.currentShirtColor;
        }
        }


    public void SetPantsToShorts()
    {
        characterAnimation.waistShortsSprite.color = characterAnimation.currentPantsColor;
        characterAnimation.kneesShinsSprite.color = characterAnimation.currentSkinColor;
        if (characterAnimation.anklesSprite.color != characterAnimation.currentShoeColor)
        {
            characterAnimation.anklesSprite.color = characterAnimation.currentSkinColor;
        }
        SetNoDress();
    }

    public void SetPantsTo3QuarterPants()
    {
        if (characterAnimation.anklesSprite.color != characterAnimation.currentShoeColor)
        {
            characterAnimation.waistShortsSprite.color = characterAnimation.currentPantsColor;
            characterAnimation.kneesShinsSprite.color = characterAnimation.currentPantsColor;
            characterAnimation.anklesSprite.color = characterAnimation.currentSkinColor;
        }
        else
        {
            NextPants();
        }
        SetNoDress();
    }

    public void SetPantsToPants()
    {
        characterAnimation.waistShortsSprite.color = characterAnimation.currentPantsColor;
        characterAnimation.kneesShinsSprite.color = characterAnimation.currentPantsColor;
        if (characterAnimation.anklesSprite.color != characterAnimation.currentShoeColor)
        {
            characterAnimation.anklesSprite.color = characterAnimation.currentPantsColor;
        }
        SetNoDress();
    }

    public void SetPantsToDress()
    {
 
      characterAnimation.waistShortsSprite.color = characterAnimation.currentPantsColor;
      characterAnimation.kneesShinsSprite.color = characterAnimation.currentSkinColor;
      if(characterAnimation.anklesSprite.color != characterAnimation.currentShoeColor)
      {
        characterAnimation.anklesSprite.color = characterAnimation.currentSkinColor;
      }

      Color color = characterAnimation.currentPantsColor;
      color.a = 1f;
      characterAnimation.dressSprite.color = color;  
    }


    public void SetNoDress()
    {
        Color color = characterAnimation.currentPantsColor;
        color.a = 0f;
        characterAnimation.dressSprite.color = color;
    }

    public void SetPantsToToiletMode()
    {
        characterAnimation.waistShortsSprite.color = characterAnimation.currentSkinColor;
        characterAnimation.kneesShinsSprite.color = characterAnimation.currentSkinColor;
        characterAnimation.waistSprite.color = characterAnimation.currentPantsColor;
        SetNoDress();
    }


    // public void SetPlayerToSleepMode(GameObject treeNode, float alpha)
    // {
    //     if (treeNode == null && treeNode.name.Contains("hair") && treeNode.name.Contains("head"))
    //     {
    //         return; // TODO: remove this
    //     }
    //     SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();
    //     if (sr != null)
    //     {
    //         sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    //     }
    //     foreach (Transform child in treeNode.transform)
    //     {
    //         SetTreeAlpha(child.gameObject, alpha);
    //     }



    //     // characterAnimation.headSprite
    //     // characterAnimation.eyeSprite 
    //     // characterAnimation.throatSprite
    //     // characterAnimation.collarSprite
    //     // characterAnimation.torsoSprite
    //     // characterAnimation.waistSprite
    //     // characterAnimation.waistShortsSprite
    //     // characterAnimation.kneesShinsSprite
    //     // characterAnimation.anklesSprite
    //     // characterAnimation.feetSprite 
    //     // characterAnimation.jakettoSprite
    //     // characterAnimation.dressSprite
    //     // characterAnimation.longSleeveSprite
    //     // characterAnimation.handSprite
    //     // characterAnimation.shortSleeveSprite
    //     // characterAnimation.mohawk5TopSprite
    //     // characterAnimation.mohawk5BottomSprite 
    //     // characterAnimation.hair0TopSprite
    //     // characterAnimation.hair0BottomSprite
    //     // characterAnimation.hair1TopSprite
    //     // characterAnimation.hair7TopSprite
    //     // characterAnimation.hair8TopSprite
    //     // characterAnimation.hair1BottomSprite
    //     // characterAnimation.hair3BottomSprite
    //     // characterAnimation.hair4BottomSprite
    //     // characterAnimation.hair6BottomSprite
    //     // characterAnimation.hair7BottomSprite
    //     // characterAnimation.hair8BottomSprite
    //     // characterAnimation.hairFringe1Sprite
    //     // characterAnimation.hairFringe2Sprite
    //     // characterAnimation.bikeSprite 
    // }

    public void SetWaistToShirt()
    {
        if (currentShirtIndex == 0) // if no shirt is on
        {
            currentWaistIndex = 1; // set waist to pants
            characterAnimation.waistSprite.color = characterAnimation.currentPantsColor;
        }
        else
        {
            characterAnimation.waistSprite.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetWaistToPants()
    {
        characterAnimation.waistSprite.color = characterAnimation.currentPantsColor;
    }

    public void SetFeetToShoes()
    {
        characterAnimation.feetSprite.color = characterAnimation.currentShoeColor;
        if (currentPantsIndex == 2)
        {
            characterAnimation.anklesSprite.color = characterAnimation.currentPantsColor;
        }
        else
        {
            characterAnimation.anklesSprite.color = characterAnimation.currentSkinColor;
        }
    }

    public void SetFeetToBareFoot()
    {
        characterAnimation.feetSprite.color = characterAnimation.currentSkinColor;
        if (currentPantsIndex == 2)
        {
            characterAnimation.anklesSprite.color = characterAnimation.currentPantsColor;
        }
        else
        {
            characterAnimation.anklesSprite.color = characterAnimation.currentSkinColor;
        }
    }

    public void SetFeetToBoots()
    {
        characterAnimation.feetSprite.color = characterAnimation.currentShoeColor;
        characterAnimation.anklesSprite.color = characterAnimation.currentShoeColor;
    }

    public void SetNoJaketto()
    {
        Color color = characterAnimation.currentJakettoColor;
        color.a = 0f;
        characterAnimation.jakettoSprite.color = color;
        UpdateShirt();
    }

    public void SetJakettoVest()
    {
        Color color = characterAnimation.currentJakettoColor;
        color.a = 1f;
        characterAnimation.jakettoSprite.color = color;
        UpdateShirt();
    }



    public void SetSkinColor1()
    {
        if (characterAnimation.headSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.headSprite.color = HexToColor("#F5CBA7");
        }
        if (characterAnimation.throatSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#F5CBA7");
        }
        if (characterAnimation.collarSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#F5CBA7");
        }
        if (characterAnimation.torsoSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#F5CBA7");
        }
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#F5CBA7");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#F5CBA7");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#F5CBA7");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#F5CBA7");
        }
        if (characterAnimation.feetSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.feetSprite.color = HexToColor("#F5CBA7");
        }
        if (characterAnimation.longSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#F5CBA7");
        }
        if (characterAnimation.handSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.handSprite.color = HexToColor("#F5CBA7");
        }
        if (characterAnimation.shortSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#F5CBA7");
        }
        characterAnimation.currentSkinColor = HexToColor("#F5CBA7");
        if(skinColorButton2 != null)
        {
            skinColorButton2.color = characterAnimation.currentSkinColor;
        }
    }

    public void SetSkinColor2()
    {
        if (characterAnimation.headSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.headSprite.color = HexToColor("#F6B883");
        }
        if (characterAnimation.throatSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#F6B883");
        }
        if (characterAnimation.collarSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#F6B883");
        }
        if (characterAnimation.torsoSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#F6B883");
        }
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#F6B883");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#F6B883");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#F6B883");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#F6B883");
        }
        if (characterAnimation.feetSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.feetSprite.color = HexToColor("#F6B883");
        }
        if (characterAnimation.longSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#F6B883");
        }
        if (characterAnimation.handSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.handSprite.color = HexToColor("#F6B883");
        }
        if (characterAnimation.shortSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#F6B883");
        }
        characterAnimation.currentSkinColor = HexToColor("#F6B883");
        if(skinColorButton2 != null)
        {
            skinColorButton2.color = characterAnimation.currentSkinColor;
        }
    }

    public void SetSkinColor3()
    {
        if (characterAnimation.headSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.headSprite.color = HexToColor("#E1A95F");
        }
        if (characterAnimation.throatSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#E1A95F");
        }
        if (characterAnimation.collarSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#E1A95F");
        }
        if (characterAnimation.torsoSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#E1A95F");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#E1A95F");
        }
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#E1A95F");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#E1A95F");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#E1A95F");
        }
        if (characterAnimation.feetSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.feetSprite.color = HexToColor("#E1A95F");
        }
        if (characterAnimation.longSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#E1A95F");
        }
        if (characterAnimation.handSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.handSprite.color = HexToColor("#E1A95F");
        }
        if (characterAnimation.shortSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#E1A95F");
        }
        characterAnimation.currentSkinColor = HexToColor("#E1A95F");
        if(skinColorButton2 != null)
        {
            skinColorButton2.color = characterAnimation.currentSkinColor;
        }
    }

    public void SetSkinColor4()
    {
        if (characterAnimation.headSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.headSprite.color = HexToColor("#C68642");
        }
        if (characterAnimation.throatSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#C68642");
        }
        if (characterAnimation.collarSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#C68642");
        }
        if (characterAnimation.torsoSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#C68642");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#C68642");
        }
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#C68642");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#C68642");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#C68642");
        }
        if (characterAnimation.feetSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.feetSprite.color = HexToColor("#C68642");
        }
        if (characterAnimation.longSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#C68642");
        }
        if (characterAnimation.handSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.handSprite.color = HexToColor("#C68642");
        }
        if (characterAnimation.shortSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#C68642");
        }
        characterAnimation.currentSkinColor = HexToColor("#C68642");
        if(skinColorButton2 != null)
        {
            skinColorButton2.color = characterAnimation.currentSkinColor;
        }
    }

    public void SetSkinColor5()
    {
        if (characterAnimation.headSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.headSprite.color = HexToColor("#B57A3D");
        }
        if (characterAnimation.throatSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#B57A3D");
        }
        if (characterAnimation.collarSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#B57A3D");
        }
        if (characterAnimation.torsoSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#B57A3D");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#B57A3D");
        }
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#B57A3D");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#B57A3D");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#B57A3D");
        }
        if (characterAnimation.feetSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.feetSprite.color = HexToColor("#B57A3D");
        }
        if (characterAnimation.longSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#B57A3D");
        }
        if (characterAnimation.handSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.handSprite.color = HexToColor("#B57A3D");
        }
        if (characterAnimation.shortSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#B57A3D");
        }
        characterAnimation.currentSkinColor = HexToColor("#B57A3D");
        if(skinColorButton2 != null)
        {
            skinColorButton2.color = characterAnimation.currentSkinColor;
        }
    }

    public void SetSkinColor6()
    {
        if (characterAnimation.headSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.headSprite.color = HexToColor("#DFA10B");
        }
        if (characterAnimation.throatSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#DFA10B");
        }
        if (characterAnimation.collarSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#DFA10B");
        }
        if (characterAnimation.torsoSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#DFA10B");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#DFA10B");
        }
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#DFA10B");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#DFA10B");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#DFA10B");
        }
        if (characterAnimation.feetSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.feetSprite.color = HexToColor("#DFA10B");
        }
        if (characterAnimation.longSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#DFA10B");
        }
        if (characterAnimation.handSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.handSprite.color = HexToColor("#DFA10B");
        }
        if (characterAnimation.shortSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#DFA10B");
        }
        characterAnimation.currentSkinColor = HexToColor("#DFA10B");
        if(skinColorButton2 != null)
        {
            skinColorButton2.color = characterAnimation.currentSkinColor;
        }
    }
//7d4e34
    public void SetSkinColor7()
    {
        if (characterAnimation.headSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.headSprite.color = HexToColor("#7d4e34");
        }
        if (characterAnimation.throatSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#7d4e34");
        }
        if (characterAnimation.collarSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#7d4e34");
        }
        if (characterAnimation.torsoSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#7d4e34");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#7d4e34");
        }
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#7d4e34");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#7d4e34");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#7d4e34");
        }
        if (characterAnimation.feetSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.feetSprite.color = HexToColor("#7d4e34");
        }
        if (characterAnimation.longSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#7d4e34");
        }
        if (characterAnimation.handSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.handSprite.color = HexToColor("#7d4e34");
        }
        if (characterAnimation.shortSleeveSprite.color == characterAnimation.currentSkinColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#7d4e34");
        }
        characterAnimation.currentSkinColor = HexToColor("#7d4e34");
        if(skinColorButton2 != null)
        {
            skinColorButton2.color = characterAnimation.currentSkinColor;
        }
    }


    public void SetHairColor1()
    {
        Transform hair = characterAnimation.transform.Find("hair");
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
            characterAnimation.currentHairColor = HexToColor("#4E342E");
            if(hairColorButton != null)
            {
                hairColorButton.color = characterAnimation.currentHairColor;
            }
        }
    }

    public void SetHairColor2()
    {
        Transform hair = characterAnimation.transform.Find("hair");
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
            characterAnimation.currentHairColor = HexToColor("#1C1C1C");
            if(hairColorButton != null)
            {
                hairColorButton.color = characterAnimation.currentHairColor;
            }
        }
    }

    public void SetHairColor3()
    {
        Transform hair = characterAnimation.transform.Find("hair");
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
            characterAnimation.currentHairColor = HexToColor("#F5D76E");
            if(hairColorButton != null)
            {
                hairColorButton.color = characterAnimation.currentHairColor;
            }
        }
    }

    public void SetHairColor4()
    {
        Transform hair = characterAnimation.transform.Find("hair");
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
            characterAnimation.currentHairColor = HexToColor("#A52A2A");
            if(hairColorButton != null)
            {
                hairColorButton.color = characterAnimation.currentHairColor;
            }
        }
    }

    public void SetHairColor5()
    {
        Transform hair = characterAnimation.transform.Find("hair");
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
            characterAnimation.currentHairColor = HexToColor("#A9A9A9");
            if(hairColorButton != null)
            {
                hairColorButton.color = characterAnimation.currentHairColor;
            }
        }
    }

    public void SetHairColor6()
    {
        Transform hair = characterAnimation.transform.Find("hair");
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
            characterAnimation.currentHairColor = HexToColor("#00BFFF");
            if(hairColorButton != null)
            {
                hairColorButton.color = characterAnimation.currentHairColor;
            }
        }
    }

    public void SetHairColor7()
    {
        Transform hair = characterAnimation.transform.Find("hair");
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
            characterAnimation.currentHairColor = HexToColor("#FFB6C1");
            if(hairColorButton != null)
            {
                hairColorButton.color = characterAnimation.currentHairColor;
            }
        }
    }

    public void SetHairColor8()
    {
        Transform hair = characterAnimation.transform.Find("hair");
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
            characterAnimation.currentHairColor = HexToColor("#E6E6FA");
            if(hairColorButton != null)
            {
                hairColorButton.color = characterAnimation.currentHairColor;
            }
        }
    }

    public void SetHairColor9()
    {
        Transform hair = characterAnimation.transform.Find("hair");
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
            characterAnimation.currentHairColor = HexToColor("#98FF98");
            if(hairColorButton != null)
            {
                hairColorButton.color = characterAnimation.currentHairColor;
            }
        }
    }

    public void SetHairColor10()
    {
        Transform hair = characterAnimation.transform.Find("hair");
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
            characterAnimation.currentHairColor = HexToColor("#8A2BE2");
            if(hairColorButton != null)
            {
                hairColorButton.color = characterAnimation.currentHairColor;
            }
        }
    }



    public void SetHairStyle1()
    {
        Transform hair = characterAnimation.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair0Top") || child.gameObject.name.Contains("hair0Bottom"))
                  {
                      spriteRenderer.color = characterAnimation.currentHairColor;  
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
        Transform hair = characterAnimation.transform.Find("hair");
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
                      spriteRenderer.color = characterAnimation.currentHairColor;                    
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
        Transform hair = characterAnimation.transform.Find("hair");
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
                      spriteRenderer.color = characterAnimation.currentHairColor;                    
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
        Transform hair = characterAnimation.transform.Find("hair");
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
                      spriteRenderer.color = characterAnimation.currentHairColor;  
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
        Transform hair = characterAnimation.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("mohawk"))
                  {
                      spriteRenderer.color = characterAnimation.currentHairColor;  
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
        Transform hair = characterAnimation.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") || child.gameObject.name.Contains("hair2Bottom") )
                  {
                      spriteRenderer.color = characterAnimation.currentHairColor;  
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
        Transform hair = characterAnimation.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") || child.gameObject.name.Contains("hair3Bottom") )
                  {
                      spriteRenderer.color = characterAnimation.currentHairColor;  
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
        Transform hair = characterAnimation.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") || child.gameObject.name.Contains("hair4Bottom") )
                  {
                      spriteRenderer.color = characterAnimation.currentHairColor;  
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
        Transform hair = characterAnimation.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") || child.gameObject.name.Contains("hair1Bottom") || child.gameObject.name.Contains("hair6Bottom") )
                  {
                      spriteRenderer.color = characterAnimation.currentHairColor;  
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
        Transform hair = characterAnimation.transform.Find("hair");
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
                      spriteRenderer.color = characterAnimation.currentHairColor;  
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
        Transform hair = characterAnimation.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair8Top") || child.gameObject.name.Contains("hair8Bottom"))
                  {
                      spriteRenderer.color = characterAnimation.currentHairColor;  
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
        Transform hair = characterAnimation.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair0Bottom"))
                  {
                      spriteRenderer.color = characterAnimation.currentHairColor;  
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
        Transform hair = characterAnimation.transform.Find("hair");
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
        if(characterAnimation.throatSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#FF6347");
        }
        if(characterAnimation.collarSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#FF6347");
        }
        if(characterAnimation.torsoSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#FF6347");
        }
        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#FF6347");
        }
        if(characterAnimation.longSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#FF6347");
        }
        if(characterAnimation.shortSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#FF6347");
        }
        characterAnimation.currentShirtColor = HexToColor("#FF6347");
        if(shirtColorButton != null)
        {
            shirtColorButton.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetShirtColor2()
    {
        if(characterAnimation.throatSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#4682B4");
        }
        if(characterAnimation.collarSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#4682B4");
        }
        if(characterAnimation.torsoSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#4682B4");
        }
        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#4682B4");
        }
        if(characterAnimation.longSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#4682B4");
        }
        if(characterAnimation.shortSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#4682B4");
        }
        characterAnimation.currentShirtColor = HexToColor("#4682B4");
        if(shirtColorButton != null)
        {
            shirtColorButton.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetShirtColor3()
    {
        if(characterAnimation.throatSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#32CD32");
        }
        if(characterAnimation.collarSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#32CD32");
        }
        if(characterAnimation.torsoSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#32CD32");
        }
        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#32CD32");
        }
        if(characterAnimation.longSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#32CD32");
        }
        if(characterAnimation.shortSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#32CD32");
        }
        characterAnimation.currentShirtColor = HexToColor("#32CD32");
        if(shirtColorButton != null)
        {
            shirtColorButton.color = characterAnimation.currentShirtColor;
        }   
    }

    public void SetShirtColor4()
    {
        if(characterAnimation.throatSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#FFD700");
        }
        if(characterAnimation.collarSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#FFD700");
        }
        if(characterAnimation.torsoSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#FFD700");
        }
        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#FFD700");
        }
        if(characterAnimation.longSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#FFD700");
        }
        if(characterAnimation.shortSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#FFD700");
        }
        characterAnimation.currentShirtColor = HexToColor("#FFD700");
        if(shirtColorButton != null)
        {
            shirtColorButton.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetShirtColor5()
    {
        if(characterAnimation.throatSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#8A2BE2");
        }
        if(characterAnimation.collarSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#8A2BE2");
        }
        if(characterAnimation.torsoSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#8A2BE2");
        }
        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#8A2BE2");
        }
        if(characterAnimation.longSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#8A2BE2");
        }
        if(characterAnimation.shortSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#8A2BE2");
        }
        characterAnimation.currentShirtColor = HexToColor("#8A2BE2");
        if(shirtColorButton != null)
        {
            shirtColorButton.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetShirtColor6()
    {
        if(characterAnimation.throatSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#DC143C");
        }
        if(characterAnimation.collarSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#DC143C");
        }
        if(characterAnimation.torsoSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#DC143C");
        }
        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#DC143C");
        }
        if(characterAnimation.longSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#DC143C");
        }
        if(characterAnimation.shortSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#DC143C");
        }
        characterAnimation.currentShirtColor = HexToColor("#DC143C");
        if(shirtColorButton != null)
        {
            shirtColorButton.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetShirtColor7()
    {
        if(characterAnimation.throatSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#20B2AA");
        }
        if(characterAnimation.collarSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#20B2AA");
        }
        if(characterAnimation.torsoSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#20B2AA");
        }
        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#20B2AA");
        }
        if(characterAnimation.longSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#20B2AA");
        }
        if(characterAnimation.shortSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#20B2AA");
        }
        characterAnimation.currentShirtColor = HexToColor("#20B2AA");
        if(shirtColorButton != null)
        {
            shirtColorButton.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetShirtColor8()
    {
        if(characterAnimation.throatSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#D3D3D3");
        };
        if(characterAnimation.collarSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#D3D3D3");
        };
        if(characterAnimation.torsoSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#D3D3D3");
        };
        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#D3D3D3");
        };
        if(characterAnimation.longSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#D3D3D3");
        };
        if(characterAnimation.shortSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#D3D3D3");
        };
        characterAnimation.currentShirtColor = HexToColor("#D3D3D3");
        if(shirtColorButton != null)
        {
            shirtColorButton.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetShirtColor9()
    {
        if(characterAnimation.throatSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#F08080");
        };
        if(characterAnimation.collarSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#F08080");
        };
        if(characterAnimation.torsoSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#F08080");
        };
        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#F08080");
        };
        if(characterAnimation.longSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#F08080");
        };
        if(characterAnimation.shortSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#F08080");
        };
        characterAnimation.currentShirtColor = HexToColor("#F08080");
        if(shirtColorButton != null)
        {
            shirtColorButton.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetShirtColor10()
    {
        if(characterAnimation.throatSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#2F4F4F");
        };
        if(characterAnimation.collarSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#2F4F4F");
        };
        if(characterAnimation.torsoSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#2F4F4F");
        };
        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#2F4F4F");
        };
        if(characterAnimation.longSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#2F4F4F");
        };
        if(characterAnimation.shortSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#2F4F4F");
        };
        characterAnimation.currentShirtColor = HexToColor("#2F4F4F");
        if(shirtColorButton != null)
        {
            shirtColorButton.color = characterAnimation.currentShirtColor;
        }
    }

    public void SetShirtColor11()
    {
        if(characterAnimation.throatSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.throatSprite.color = HexToColor("#FF34F4");
        };
        if(characterAnimation.collarSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.collarSprite.color = HexToColor("#FF34F4");
        };
        if(characterAnimation.torsoSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.torsoSprite.color = HexToColor("#FF34F4");
        };
        if(characterAnimation.waistSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#FF34F4");
        };
        if(characterAnimation.longSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.longSleeveSprite.color = HexToColor("#FF34F4");
        };
        if(characterAnimation.shortSleeveSprite.color == characterAnimation.currentShirtColor)
        {
            characterAnimation.shortSleeveSprite.color = HexToColor("#FF34F4");
        };
        characterAnimation.currentShirtColor = HexToColor("#FF34F4");
        if(shirtColorButton != null)
        {
            shirtColorButton.color = characterAnimation.currentShirtColor;
        }
    }




    public void SetPantsColor1()
    {
        characterAnimation.dressSprite.color = HexToColor("#2C3E50");
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#2C3E50");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#2C3E50");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#2C3E50");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#2C3E50");
        }
        characterAnimation.currentPantsColor = HexToColor("#2C3E50");
        if(pantsColorButton != null)
        {
            pantsColorButton.color = characterAnimation.currentPantsColor;
        }
    }

    public void SetPantsColor2()
    {
        characterAnimation.dressSprite.color = HexToColor("#A0522D");
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#A0522D");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#A0522D");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#A0522D");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#A0522D");
        }
        characterAnimation.currentPantsColor = HexToColor("#A0522D");
        if(pantsColorButton != null)
        {
            pantsColorButton.color = characterAnimation.currentPantsColor;
        }
    }

    public void SetPantsColor3()
    {
        characterAnimation.dressSprite.color = HexToColor("#808080");
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#808080");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#808080");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#808080");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#808080");
        }
        characterAnimation.currentPantsColor = HexToColor("#808080");
        if(pantsColorButton != null)
        {
            pantsColorButton.color = characterAnimation.currentPantsColor;
        }
    }

    public void SetPantsColor4()
    {
        characterAnimation.dressSprite.color = HexToColor("#556B2F");
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#556B2F");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#556B2F");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#556B2F");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#556B2F");
        }
        characterAnimation.currentPantsColor = HexToColor("#556B2F");
        if(pantsColorButton != null)
        {
            pantsColorButton.color = characterAnimation.currentPantsColor;
        }
    }

    public void SetPantsColor5()
    {
        characterAnimation.dressSprite.color = HexToColor("#B0C4DE");
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#B0C4DE");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#B0C4DE");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#B0C4DE");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#B0C4DE");
        }
        characterAnimation.currentPantsColor = HexToColor("#B0C4DE");
        if(pantsColorButton != null)
        {
            pantsColorButton.color = characterAnimation.currentPantsColor;
        }
    }

    public void SetPantsColor6()
    {
        characterAnimation.dressSprite.color = HexToColor("#4B0082");
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#4B0082");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#4B0082");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#4B0082");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#4B0082");
        }
        characterAnimation.currentPantsColor = HexToColor("#4B0082");
        if(pantsColorButton != null)
        {
            pantsColorButton.color = characterAnimation.currentPantsColor;
        }
    }

    public void SetPantsColor7()
    {
        characterAnimation.dressSprite.color = HexToColor("#8B4513");
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#8B4513");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#8B4513");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#8B4513");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#8B4513");
        }
        characterAnimation.currentPantsColor = HexToColor("#8B4513");
        if(pantsColorButton != null)
        {
            pantsColorButton.color = characterAnimation.currentPantsColor;
        }
    }

    public void SetPantsColor8()
    {
        characterAnimation.dressSprite.color = HexToColor("#696969");
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#696969");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#696969");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#696969");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#696969");
        }
        characterAnimation.currentPantsColor = HexToColor("#696969");
        if(pantsColorButton != null)
        {
            pantsColorButton.color = characterAnimation.currentPantsColor;
        }
    }

    public void SetPantsColor9()
    {
        characterAnimation.dressSprite.color = HexToColor("#4682B4");
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#4682B4");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#4682B4");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#4682B4");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#4682B4");
        }
        characterAnimation.currentPantsColor = HexToColor("#4682B4");
        if(pantsColorButton != null)
        {
            pantsColorButton.color = characterAnimation.currentPantsColor;
        }
    }

    public void SetPantsColor10()
    {
        characterAnimation.dressSprite.color = HexToColor("#000000");
        if (characterAnimation.waistShortsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistShortsSprite.color = HexToColor("#000000");
        }
        if (characterAnimation.waistSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.waistSprite.color = HexToColor("#000000");
        }
        if (characterAnimation.kneesShinsSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.kneesShinsSprite.color = HexToColor("#000000");
        }
        if (characterAnimation.anklesSprite.color == characterAnimation.currentPantsColor)
        {
            characterAnimation.anklesSprite.color = HexToColor("#000000");
        }
        characterAnimation.currentPantsColor = HexToColor("#000000");
        if(pantsColorButton != null)
        {
            pantsColorButton.color = characterAnimation.currentPantsColor;
        }
    }


    public void SetJakettoColor1()
    {
        characterAnimation.currentJakettoColor = HexToColor("#556B2F");  // Dark Olive
        if(jackettoColorButton != null)
        {
            jackettoColorButton.color = characterAnimation.currentJakettoColor;
        }
        UpdateJaketto();
    }

    public void SetJakettoColor2()
    {
        characterAnimation.currentJakettoColor = HexToColor("#800020");  // Deep Burgundy
        if(jackettoColorButton != null)
        {
            jackettoColorButton.color = characterAnimation.currentJakettoColor;
        }
        UpdateJaketto();
    }

    public void SetJakettoColor3()
    {
        characterAnimation.currentJakettoColor = HexToColor("#6A5ACD");  // Slate Blue
        if(jackettoColorButton != null)
        {
            jackettoColorButton.color = characterAnimation.currentJakettoColor;
        }
        UpdateJaketto();
    }

    public void SetJakettoColor4()
    {
        characterAnimation.currentJakettoColor = HexToColor("#FFDB58");  // Mustard Yellow
        if(jackettoColorButton != null)
        {
            jackettoColorButton.color = characterAnimation.currentJakettoColor;
        }
        UpdateJaketto();
    }

    public void SetJakettoColor5()
    {
        characterAnimation.currentJakettoColor = HexToColor("#36454F");  // Charcoal
        if(jackettoColorButton != null)
        {
            jackettoColorButton.color = characterAnimation.currentJakettoColor;
        }
        UpdateJaketto();
    }

    public void SetJakettoColor6()
    {
        characterAnimation.currentJakettoColor = HexToColor("#228B22");  // Forest Green
        if(jackettoColorButton != null)
        {
            jackettoColorButton.color = characterAnimation.currentJakettoColor;
        }
        UpdateJaketto();
    }

    public void SetJakettoColor7()
    {
        characterAnimation.currentJakettoColor = HexToColor("#000080");  // Navy Blue
        if(jackettoColorButton != null)
        {
            jackettoColorButton.color = characterAnimation.currentJakettoColor;
        }
        UpdateJaketto();
    }

    public void SetJakettoColor8()
    {
        characterAnimation.currentJakettoColor = HexToColor("#8B4513");  // Chocolate Brown
        if(jackettoColorButton != null)
        {
            jackettoColorButton.color = characterAnimation.currentJakettoColor;
        }
        UpdateJaketto();
    }

    public void SetJakettoColor9()
    {
        characterAnimation.currentJakettoColor = HexToColor("#008080");  // Teal
        if(jackettoColorButton != null)
        {
            jackettoColorButton.color = characterAnimation.currentJakettoColor;
        }
        UpdateJaketto();
    }

    public void SetJakettoColor10()
    {
        characterAnimation.currentJakettoColor = HexToColor("#800000");  // Maroon
        if(jackettoColorButton != null)
        {
            jackettoColorButton.color = characterAnimation.currentJakettoColor;
        }
        UpdateJaketto();
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

 
    // public void StopDeactivateSpaceBar()
    // {
    //     if (allowTimeForSpaceBarCoro != null)
    //     {
    //         StopCoroutine(allowTimeForSpaceBarCoro);
    //         allowTimeForSpaceBarCoro = null;
    //         spaceBarDeactivated = false;
    //     }
    // }


    // Check if any input field is focused
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
    //         SetPantsToShorts();
    //     }
    //     else if(currentPantsIndex == 1)
    //     {
    //         SetPantsTo3QuarterPants();
    //     }
    //     else if(currentPantsIndex == 2)
    //     {
    //         SetPantsToPants();
    //     }
    // }




    public void ToggleCustomizeUI()
    {
        GameObject customizeButtons = transform.Find("customiseButtons").gameObject;
        bool isActive = customizeButtons.activeSelf;
        customizeButtons.SetActive(!isActive);  // Toggles the active state
    }





    // Color HexToColor(string hex)
    // {
    //     Color newCol;
    //     if (ColorUtility.TryParseHtmlString(hex, out newCol))
    //     {
    //         return newCol;
    //     }
    //     else
    //     {
    //         Debug.LogError("Invalid hex color string: " + hex);
    //         return Color.black; // Return black if conversion fails
    //     }
    // }
}
