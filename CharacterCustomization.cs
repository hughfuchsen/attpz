// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;

// using UnityEngine;
// using UnityEngine.UI;

// public class CharacterCustomization : MonoBehaviour 
// {
//     PlayerAnimationAndMovement playerMovement;
//     [SerializeField] GameObject Player;



//     public bool lockedSkinColor = false;
//     public bool lockedBodyType = false;
//     public bool lockedHeight = false;
//     public bool lockedWidth = false;
//     public bool lockedHair = false;
//     public bool lockedShirt = false;
//     public bool lockedWaist = false;
//     public bool lockedPants = false;
//     public bool lockedJaketto = false;
//     public bool lockedShoes = false;

//     // Declare Image components for each locked attribute
//     private Image lockBodyImgComponent;
//     private Image lockSkinImgComponent;
//     private Image lockHeightImgComponent;
//     private Image lockWidthImgComponent;
//     private Image lockHairImgComponent;
//     private Image lockShirtImgComponent;
//     private Image lockWaistImgComponent;
//     private Image lockPantsImgComponent;
//     private Image lockJakettoImgComponent;
//     private Image lockShoesImgComponent;

//      // Sprites for locked and unlocked states
//     private Sprite lockedImage;
//     private Sprite unlockedImage;

//     // public Image skinColorButton;
//     public Image skinColorButton2;
//     public Image hairColorButton;
//     public Image shirtColorButton;
//     public Image pantsColorButton;
//     public Image jackettoColorButton;


//     public int currentBodyTypeIndex = 0; // Index to track current body type
//     public int currentHeightIndex = 0;   // Index to track current height
//     public int currentWidthIndex = 0;    // Index to track current currentWidthIndex
//     public int currentHairStyleIndex = 0;    //"" "" "" 
//     public int currentHairColorIndex = 0;    //"" "" "" 
//     public int currentShirtIndex = 0;    //"" "" "" 
//     public int currentWaistIndex = 0;   
//     public int currentPantsIndex = 0;   
//     public int currentFeetIndex = 0;    
//     public int currentJakettoIndex = 0;    
//     public int currentSkinColorIndex = 0;   
//     public int currentShirtColorIndex = 0;   
//     public int currentPantsColorIndex = 0;   
//     public int currentJakettoColorIndex = 0;   



//     public void Start()
//     {
//         Player = GameObject.FindGameObjectWithTag("Player");
//         playerMovement = Player.GetComponent<PlayerAnimationAndMovement>(); 
      
//         int[] bodyTypeIndex1 = {5,17,29};
//         int[] bodyTypeIndex2 = {9,21,33};
//         int[] bodyTypeIndex3 = {1,13,25};

//         int[] heightIndex1 = {5,9,1};
//         int[] heightIndex2 = {17,21,13};
//         int[] heightIndex3 = {29, 33, 25};

//         SetBodyType();


//         unlockedImage = Resources.Load<Sprite>("unlockedUI");
//         lockedImage = Resources.Load<Sprite>("lockedUI");

//          // Find and assign all lock Image components by their tags

//         lockBodyImgComponent = GameObject.FindGameObjectWithTag("LockBodyUI").GetComponent<Image>();
//         lockSkinImgComponent = GameObject.FindGameObjectWithTag("LockSkinUI").GetComponent<Image>();
//         lockHeightImgComponent = GameObject.FindGameObjectWithTag("LockHeightUI").GetComponent<Image>();
//         lockWidthImgComponent = GameObject.FindGameObjectWithTag("LockWidthUI").GetComponent<Image>();
//         lockHairImgComponent = GameObject.FindGameObjectWithTag("LockHairUI").GetComponent<Image>();
//         lockShirtImgComponent = GameObject.FindGameObjectWithTag("LockShirtUI").GetComponent<Image>();
//         lockWaistImgComponent = GameObject.FindGameObjectWithTag("LockWaistUI").GetComponent<Image>();
//         lockPantsImgComponent = GameObject.FindGameObjectWithTag("LockPantsUI").GetComponent<Image>();
//         lockJakettoImgComponent = GameObject.FindGameObjectWithTag("LockJackettoUI").GetComponent<Image>();
//         lockShoesImgComponent = GameObject.FindGameObjectWithTag("LockShoesUI").GetComponent<Image>();
//         // transform.Find("customiseButtons").gameObject.SetActive(false);

//         lockBodyImgComponent.sprite = unlockedImage;
//         lockSkinImgComponent.sprite = unlockedImage;
//         lockHeightImgComponent.sprite = unlockedImage;
//         lockWidthImgComponent.sprite = unlockedImage;
//         lockHairImgComponent.sprite = unlockedImage;
//         lockShirtImgComponent.sprite = unlockedImage;
//         lockWaistImgComponent.sprite = unlockedImage;
//         lockPantsImgComponent.sprite = unlockedImage;
//         lockJakettoImgComponent.sprite = unlockedImage;
//         lockShoesImgComponent.sprite = unlockedImage;


//     }

//     // Body Type Selection
//     public void NextBodyType()
//     {
//         currentBodyTypeIndex ++; // Increment body type index
//         if (currentBodyTypeIndex >= 3)
//         {
//             currentBodyTypeIndex = 0; // Wrap around to the first body type option
//         }
//         SetBodyType();
//         // SetBodyType();
//     }

//     // public void PreviousBodyType()
//     // {
//     //     currentBodyTypeIndex --; // Decrement body type index
//     //     if (currentBodyTypeIndex < 0)
//     //     {
//     //         currentBodyTypeIndex = 2; // Wrap around to the last body type option
//     //     }
//     //     SetBodyType();
//     //     // SetBodyType();    
//     // }

//     // Height Selection
//     public void NextHeight()
//     {
//         // int[] availableHeights = heightOptions[bodyTypeIndexSet[currentBodyTypeIndex]]; // Get height options for current body type
//         currentHeightIndex++;
//         if (currentHeightIndex >= 3)
//         {
//             currentHeightIndex = 0; 
//         }
//         SetBodyType();
//         // SetBodyType();
//     }

//     // public void PreviousHeight()
//     // {
//     //     // int[] availableHeights = heightOptions[bodyTypeIndexSet[currentBodyTypeIndex]]; // Get height options for current body type
//     //     currentHeightIndex--;
//     //     if (currentHeightIndex < 0)
//     //     {
//     //         currentHeightIndex = 2; 
//     //     }
//     //     SetBodyType();
//     //     // SetBodyType();
//     // }

//     private void SetBodyType()
//     {
//         if(currentHeightIndex == 2 && currentBodyTypeIndex == 0)
//         {
//             bodyTypeNumber = 1 + currentWidthIndex;
//         }
//         if(currentHeightIndex == 0 && currentBodyTypeIndex == 0)
//         {
//             bodyTypeNumber = 5 + currentWidthIndex;
//         }
//         if(currentHeightIndex == 1 && currentBodyTypeIndex == 0)
//         {
//             bodyTypeNumber = 9 + currentWidthIndex;
//         }
//         if(currentHeightIndex == 2 && currentBodyTypeIndex == 1)
//         {
//             bodyTypeNumber = 13 + currentWidthIndex;
//         }
//         if(currentHeightIndex == 0 && currentBodyTypeIndex == 1)
//         {
//             bodyTypeNumber = 17 + currentWidthIndex;
//         }
//         if(currentHeightIndex == 1 && currentBodyTypeIndex == 1)
//         {
//             bodyTypeNumber = 21 + currentWidthIndex;
//         }
//         if(currentHeightIndex == 2 && currentBodyTypeIndex == 2)
//         {
//             bodyTypeNumber = 25 + currentWidthIndex;
//         }
//         if(currentHeightIndex == 0 && currentBodyTypeIndex == 2)
//         {
//             bodyTypeNumber = 29 + currentWidthIndex;
//         }
//         if(currentHeightIndex == 1 && currentBodyTypeIndex == 2)
//         {
//             bodyTypeNumber = 33 + currentWidthIndex;
//         }


//         // adjust the box collider on playa
//         if(currentWidthIndex == 0)
//         {
//             Vector2 newSize = Player.GetComponent<BoxCollider2D>().size;
//             newSize.x = 6f + currentWidthIndex;
//             Player.GetComponent<BoxCollider2D>().size = newSize;
//             Vector2 newOffset = Player.GetComponent<BoxCollider2D>().offset;
//             // newOffset = new Vector2(16f, -43.5f);
//             Player.GetComponent<BoxCollider2D>().offset = newOffset;
//         }
//         if(currentWidthIndex == 1)
//         {
//             Vector2 newSize = Player.GetComponent<BoxCollider2D>().size;
//             newSize.x = 6f + currentWidthIndex;
//             Player.GetComponent<BoxCollider2D>().size = newSize;
//             Vector2 newOffset = Player.GetComponent<BoxCollider2D>().offset;
//             // newOffset = new Vector2(16f, -43.5f);
//             Player.GetComponent<BoxCollider2D>().offset = newOffset;
//         }
//         if(currentWidthIndex == 2)
//         {
//             Vector2 newSize = Player.GetComponent<BoxCollider2D>().size;
//             newSize.x = 6f + currentWidthIndex;
//             Player.GetComponent<BoxCollider2D>().size = newSize;
//             Vector2 newOffset = Player.GetComponent<BoxCollider2D>().offset;
//             // newOffset = new Vector2(16f, -43.5f);
//             Player.GetComponent<BoxCollider2D>().offset = newOffset;
//         }
//         if(currentWidthIndex == 3)
//         {
//             Vector2 newSize = Player.GetComponent<BoxCollider2D>().size;
//             newSize.x = 6f + currentWidthIndex;
//             Player.GetComponent<BoxCollider2D>().size = newSize;
//             Vector2 newOffset = Player.GetComponent<BoxCollider2D>().offset;
//             // newOffset = new Vector2(16f, -43.5f);
//             Player.GetComponent<BoxCollider2D>().offset = newOffset;
//         }
//     }

//     private int[] widthIndexSet = {0, 1, 2, 3};


//     // Width Selection
//     public void NextWidth()
//     {
//         currentWidthIndex++;
//         if (currentWidthIndex >= widthIndexSet.Length)
//         {
//             currentWidthIndex = 0; // Wrap around to the first currentWidthIndex option
//         }
//         SetBodyType();
//         // SetBodyType();
//     }

//     public void PreviousWidth()
//     {
//         currentWidthIndex--;
//         if (currentWidthIndex < 0)
//         {
//             currentWidthIndex = widthIndexSet.Length - 1; 
//         }
//         SetBodyType();
//         // SetBodyType();
//     }

//     // private void SetWidth()
//     // {
//     //     int[] availableWidths = widthOptions[bodyTypeNumber];
//     //     bodyTypeNumber = availableWidths[currentWidthIndex];
//     //     Debug.Log("Width: " + bodyTypeNumber); // Log current currentWidthIndex
//     // }


//     public void NextHairStyle()
//     {
//         currentHairStyleIndex++;
//         // if (currentHairStyleIndex >= 10)
//         // {
//         //     currentHairStyleIndex = 0; // Wrap around to the first currentWidthIndex option
//         // }
//         if (currentHairStyleIndex >= 13)
//         {
//             currentHairStyleIndex = 0; // Wrap around to the first currentWidthIndex option
//         }
//         UpdateHairStyle();
//     }

//     public void UpdateHairStyle()
//     {

//         if(currentHairStyleIndex == 0)
//         {
//             SetHairStyle1();
//         }
//         else if(currentHairStyleIndex == 1)
//         {
//             SetHairStyle2();
//         }
//         else if(currentHairStyleIndex == 2)
//         {
//             SetHairStyle3();
//         }
//         else if(currentHairStyleIndex == 3)
//         {
//             SetHairStyle4();
//         }
//         else if(currentHairStyleIndex == 4)
//         {
//             SetHairStyle5();
//         }
//         else if(currentHairStyleIndex == 5)
//         {
//             SetHairStyle6();
//         }
//         else if(currentHairStyleIndex == 6)
//         {
//             SetHairStyle7();
//         }
//         else if(currentHairStyleIndex == 7)
//         {
//             SetHairStyle8();
//         }
//         else if(currentHairStyleIndex == 8)
//         {
//             SetHairStyle9();
//         }
//         else if(currentHairStyleIndex == 9)
//         {
//             SetHairStyle10();
//         }
//         else if(currentHairStyleIndex == 10)
//         {
//             SetHairStyle11();
//         }
//         else if(currentHairStyleIndex == 11)
//         {
//             SetHairStyle12();
//         }
//         else if(currentHairStyleIndex == 12)
//         {
//             SetHairStyle13();
//         }




//     }

//        public void NextHairColor()
//     {
//         currentHairColorIndex++;
//         if (currentHairColorIndex >= 10)
//         {
//             currentHairColorIndex = 0; // Wrap around to the first currentWidthIndex option
//         }
//         UpdateHairColor();
//     }

//     public void UpdateHairColor()
//     {

//         if(currentHairColorIndex == 0)
//         {
//             SetHairColor1();
//         }
//         else if(currentHairColorIndex == 1)
//         {
//             SetHairColor2();
//         }
//         else if(currentHairColorIndex == 2)
//         {
//             SetHairColor3();
//         }
//         else if(currentHairColorIndex == 3)
//         {
//             SetHairColor4();
//         }
//         else if(currentHairColorIndex == 4)
//         {
//             SetHairColor5();
//         }
//         else if(currentHairColorIndex == 5)
//         {
//             SetHairColor6();
//         }
//         else if(currentHairColorIndex == 6)
//         {
//             SetHairColor7();
//         }
//         else if(currentHairColorIndex == 7)
//         {
//             SetHairColor8();
//         }
//         else if(currentHairColorIndex == 8)
//         {
//             SetHairColor9();
//         }
//         else if(currentHairColorIndex == 9)
//         {
//             SetHairColor10();
//         }
//     }
//     public void NextShirt()
//     {
//         currentShirtIndex++;
//         if (currentShirtIndex >= 10)
//         {
//             currentShirtIndex = 0; // Wrap around to the first currentWidthIndex option
//         }
//         UpdateShirt();
//     }

//     public void UpdateShirt()
//     {
//         if(currentShirtIndex == 0)
//         {
//             SetShirtToBirthdaySuit();
//         }
//         else if(currentShirtIndex == 1)
//         {
//             SetShirtToSinglet1();
//         }
//         else if(currentShirtIndex == 2)
//         {
//             SetShirtToSinglet2();
//         }
//         else if(currentShirtIndex == 3)
//         {
//             SetShirtToSinglet3();
//         }
//         else if(currentShirtIndex == 4)
//         {
//             SetShirtToTShirt1();
//         }
//         else if(currentShirtIndex == 5)
//         {
//             SetShirtToTShirt2();
//         }
//         else if(currentShirtIndex == 6)
//         {
//             SetShirtToTShirt3();
//         }
//         else if(currentShirtIndex == 7)
//         {
//             SetShirtToLSShirt1();
//         }
//         else if(currentShirtIndex == 8)
//         {
//             SetShirtToLSShirt2();
//         }
//         else if(currentShirtIndex == 9)
//         {
//             SetShirtToLSShirt3();
//         }
//     }


//     public void NextWaist()
//     {
//         currentWaistIndex++;
//         if (currentWaistIndex >= 2)
//         {
//             currentWaistIndex = 0; // Wrap around to the first currentWidthIndex option
//         }
//         UpdateWaist();
//     }

//     public void UpdateWaist()
//     {
//         if(currentWaistIndex == 0)
//         {
//             SetWaistToShirt();
//         }
//         else if(currentWaistIndex == 1)
//         {
//             SetWaistToPants();
//         }
//         // else if(currentWaistIndex == 2)
//         // {
//         //     SetWaistToSkin();
//         // }
//     }
//     public void NextPants()
//     {
//         currentPantsIndex++;
//         if (currentPantsIndex >= 4)
//         {
//             currentPantsIndex = 0; // Wrap around to the first currentWidthIndex option
//         }
//         UpdatePants();
//     }

//     public void UpdatePants()
//     {
//         if(currentPantsIndex == 0)
//         {
//             SetPantsToShorts();
//         }
//         else if(currentPantsIndex == 1)
//         {
//             SetPantsTo3QuarterPants();
//         }
//         else if(currentPantsIndex == 2)
//         {
//             SetPantsToPants();
//         }
//         else if(currentPantsIndex == 3)
//         {
//             SetPantsToDress();
//         }
//         // else if(currentPantsIndex == 3)
//         // {
//         //     SetPantsToBirthdaySuit();
//         // }
//     }
    
//     public void NextJaketto()
//     {
//         currentJakettoIndex++;
//         if (currentJakettoIndex >= 4)
//         {
//             currentJakettoIndex = 0; // Wrap around to the first currentWidthIndex option
//         }
//         UpdateJaketto();
//     }

//     public void UpdateJaketto()
//     {
//         if(currentJakettoIndex == 0)
//         {
//             SetNoJaketto();
//         }
//         else if(currentJakettoIndex == 1 || currentJakettoIndex == 2 || currentJakettoIndex == 3)
//         {
//             SetJakettoVest();
//         }
//         else
//         {
//             currentJakettoIndex = 0;
//             UpdateJaketto();          
//         }
//     }
    
    
//     public void NextFeet()
//     {
//         currentFeetIndex++;
//         if (currentFeetIndex >= 3)
//         {
//             currentFeetIndex = 0; // Wrap around to the first currentWidthIndex option
//         }
//         UpdateFeet();
//     }

//     public void UpdateFeet()
//     {
//         if(currentFeetIndex == 0)
//         {
//             SetFeetToBoots();
//         }
//         else if(currentFeetIndex == 1)
//         {
//             SetFeetToShoes();
//         }
//         else if(currentFeetIndex == 2)
//         {
//             SetFeetToBareFoot();
//         }
//     }
//     public void NextSkinColor()
//     {
//         currentSkinColorIndex++;
//         if (currentSkinColorIndex >= 7)
//         {
//             currentSkinColorIndex = 0; // Wrap around to the first currentWidthIndex option
//         }
//         UpdateSkinColor();
//     }

//     public void UpdateSkinColor()
//     {
//         if(currentSkinColorIndex == 0)
//         {
//             SetSkinColor1();
//         }
//         else if(currentSkinColorIndex == 1)
//         {
//             SetSkinColor2();
//         }
//         else if(currentSkinColorIndex == 2)
//         {
//             SetSkinColor3();
//         }
//         else if(currentSkinColorIndex == 3)
//         {
//             SetSkinColor4();
//         }
//         else if(currentSkinColorIndex == 4)
//         {
//             SetSkinColor5();
//         }
//         else if(currentSkinColorIndex == 5)
//         {
//             SetSkinColor6();
//         }
//         else if(currentSkinColorIndex == 6)
//         {
//             SetSkinColor7();
//         }
//     }



//     public void NextShirtColor()
//     {
//         currentShirtColorIndex++;
//         if (currentShirtColorIndex >= 11)
//         {
//             currentShirtColorIndex = 0; // Wrap around to the first currentWidthIndex option
//         }
//         UpdateShirtColor();
//     }

//     public void UpdateShirtColor()
//     {
//         if(currentShirtColorIndex == 0)
//         {
//             SetShirtColor1();
//         }
//         else if(currentShirtColorIndex == 1)
//         {
//             SetShirtColor2();
//         }
//         else if(currentShirtColorIndex == 2)
//         {
//             SetShirtColor3();
//         }
//         else if(currentShirtColorIndex == 3)
//         {
//             SetShirtColor4();
//         }
//         else if(currentShirtColorIndex == 4)
//         {
//             SetShirtColor5();
//         }
//         else if(currentShirtColorIndex == 5)
//         {
//             SetShirtColor6();
//         }
//         else if(currentShirtColorIndex == 6)
//         {
//             SetShirtColor7();
//         }
//         else if(currentShirtColorIndex == 7)
//         {
//             SetShirtColor8();
//         }
//         else if(currentShirtColorIndex == 8)
//         {
//             SetShirtColor9();
//         }
//         else if(currentShirtColorIndex == 9)
//         {
//             SetShirtColor10();
//         }
//         else if(currentShirtColorIndex == 10)
//         {
//             SetShirtColor11();
//         }
//     }


//     public void NextJakettoColor()
//     {
//         currentJakettoColorIndex++;
//         if (currentJakettoColorIndex >= 11)
//         {
//             currentJakettoColorIndex = 0; // Wrap around to the first currentWidthIndex option
//         }
//         UpdateJakettoColor();
//     }

//     public void UpdateJakettoColor()
//     {
//         if(currentJakettoColorIndex == 0)
//         {
//             SetJakettoColor1();
//         }
//         else if(currentJakettoColorIndex == 1)
//         {
//             SetJakettoColor2();
//         }
//         else if(currentJakettoColorIndex == 2)
//         {
//             SetJakettoColor3();
//         }
//         else if(currentJakettoColorIndex == 3)
//         {
//             SetJakettoColor4();
//         }
//         else if(currentJakettoColorIndex == 4)
//         {
//             SetJakettoColor5();
//         }
//         else if(currentJakettoColorIndex == 5)
//         {
//             SetJakettoColor6();
//         }
//         else if(currentJakettoColorIndex == 6)
//         {
//             SetJakettoColor7();
//         }
//         else if(currentJakettoColorIndex == 7)
//         {
//             SetJakettoColor8();
//         }
//         else if(currentJakettoColorIndex == 8)
//         {
//             SetJakettoColor9();
//         }
//         else if(currentJakettoColorIndex == 9)
//         {
//             SetJakettoColor10();
//         }
//     }


//    public void NextPantsColor()
//     {
//         currentPantsColorIndex++;
//         if (currentPantsColorIndex >= 10)
//         {
//             currentPantsColorIndex = 0; // Wrap around to the first currentWidthIndex option
//         }
//         UpdatePantsColor();
//     }

//     public void UpdatePantsColor()
//     {
//         if(currentPantsColorIndex == 0)
//         {
//             SetPantsColor1();
//         }
//         else if(currentPantsColorIndex == 1)
//         {
//             SetPantsColor2();
//         }
//         else if(currentPantsColorIndex == 2)
//         {
//             SetPantsColor3();
//         }
//         else if(currentPantsColorIndex == 3)
//         {
//             SetPantsColor4();
//         }
//         else if(currentPantsColorIndex == 4)
//         {
//             SetPantsColor5();
//         }
//         else if(currentPantsColorIndex == 5)
//         {
//             SetPantsColor6();
//         }
//         else if(currentPantsColorIndex == 6)
//         {
//             SetPantsColor7();
//         }
//         else if(currentPantsColorIndex == 7)
//         {
//             SetPantsColor8();
//         }
//         else if(currentPantsColorIndex == 8)
//         {
//             SetPantsColor9();
//         }
//         else if(currentPantsColorIndex == 9)
//         {
//             SetPantsColor10();
//         }
//     }

// private void UpdateLockImage()
// {
//     // thisButtonIsLocked = !thisButtonIsLocked;  // Example placeholder

//     // Check all locked states and update their images accordingly
    
//     if (lockedSkinColor)
//     {
//         lockSkinImgComponent.sprite = lockedImage;
//     }
//     else
//     {
//         lockSkinImgComponent.sprite = unlockedImage;
//     }

//     if (lockedBodyType)
//     {
//         lockBodyImgComponent.sprite = lockedImage;
//     }
//     else
//     {
//         lockBodyImgComponent.sprite = unlockedImage;
//     }

//     if (lockedHeight)
//     {
//         lockHeightImgComponent.sprite = lockedImage;
//     }
//     else
//     {
//         lockHeightImgComponent.sprite = unlockedImage;
//     }

//     if (lockedWidth)
//     {
//         lockWidthImgComponent.sprite = lockedImage;
//     }
//     else
//     {
//         lockWidthImgComponent.sprite = unlockedImage;
//     }

//     if (lockedHair)
//     {
//         lockHairImgComponent.sprite = lockedImage;
//     }
//     else
//     {
//         lockHairImgComponent.sprite = unlockedImage;
//     }

//     if (lockedShirt)
//     {
//         lockShirtImgComponent.sprite = lockedImage;
//     }
//     else
//     {
//         lockShirtImgComponent.sprite = unlockedImage;
//     }

//     if (lockedWaist)
//     {
//         lockWaistImgComponent.sprite = lockedImage;
//     }
//     else
//     {
//         lockWaistImgComponent.sprite = unlockedImage;
//     }

//     if (lockedPants)
//     {
//         lockPantsImgComponent.sprite = lockedImage;
//     }
//     else
//     {
//         lockPantsImgComponent.sprite = unlockedImage;
//     }

//     if (lockedJaketto)
//     {
//         lockJakettoImgComponent.sprite = lockedImage;
//     }
//     else
//     {
//         lockJakettoImgComponent.sprite = unlockedImage;
//     }

//     if (lockedShoes)
//     {
//         lockShoesImgComponent.sprite = lockedImage;
//     }
//     else
//     {
//         lockShoesImgComponent.sprite = unlockedImage;
//     }
// }

//     public void SkinColorLock()
//     {
//         lockedSkinColor = !lockedSkinColor;
//         UpdateLockImage();
//     }

//     public void BodyTypeLock()
//     {
//         lockedBodyType = !lockedBodyType;
//         UpdateLockImage();
//     }
//     public void HeightLock()
//     {
//         lockedHeight = !lockedHeight;
//         UpdateLockImage();
//     }
//     public void WidthLock()
//     {
//         lockedWidth = !lockedWidth;
//         UpdateLockImage();
//     }
//     public void HairLock()
//     {
//         lockedHair = !lockedHair;
//         UpdateLockImage();
//     }
//     public void ShirtLock()
//     {
//         lockedShirt = !lockedShirt;
//         UpdateLockImage();
//     }
//     public void WaistLock()
//     {
//         lockedWaist = !lockedWaist;
//         UpdateLockImage();
//     }
//     public void PantsLock()
//     {
//         lockedPants = !lockedPants;
//         UpdateLockImage();
//     }
//     public void JakettoLock()
//     {
//         lockedJaketto = !lockedJaketto;
//         UpdateLockImage();
//     }
//     public void ShoesLock()
//     {
//         lockedShoes = !lockedShoes;
//         UpdateLockImage();
//     }

//     public void UpdateRandom()
//     {
//         //duplicate if statements because the indexes trip over each other otherwise!!!!!! :) be happy ok!
//         if(!lockedShoes)
//         {
//             currentFeetIndex = Random.Range(0, 3);
//             UpdateFeet();
//         }
//         // if(!lockedPants)
//         // {
//         //     currentPantsIndex = Random.Range(0, 4);
//         //     UpdatePants();
//         // }
//         if(!lockedWaist)
//         {
//             currentWaistIndex = Random.Range(0, 2);
//             UpdateWaist();
//         }
//         if(!lockedHair)
//         {
//             currentHairColorIndex = Random.Range(0, 10);
//             UpdateHairColor();
//         }
//         if(!lockedShirt)
//         {
//             currentShirtIndex = Random.Range(1, 10); // omit no shirt for rando
//             UpdateShirt();
//         }
//         if(!lockedHeight)
//         {
//             currentHeightIndex = Random.Range(0, 3);
//             SetBodyType();
//         }
//         if(!lockedBodyType)
//         {
//             currentBodyTypeIndex = Random.Range(0, 3);
//             SetBodyType();
//         }
//         if(!lockedWidth)
//         {
//             currentWidthIndex = Random.Range(0, 4);
//             SetBodyType();
//         }
//         if(!lockedPants)
//         {
//             currentPantsColorIndex = Random.Range(0, 10);
//             UpdatePantsColor();
//         }
//         if(!lockedPants)
//         {
//             currentPantsIndex = Random.Range(0, 4);
//             UpdatePants();
//         }
//         if(!lockedShirt)
//         {
//             currentShirtColorIndex = Random.Range(0, 11);
//             UpdateShirtColor();
//         }
//         if(!lockedHair)
//         {
//             currentHairStyleIndex = Random.Range(0, 13);
//             UpdateHairStyle(); 
//         }
//         if(!lockedJaketto)
//         {
//             currentJakettoIndex = Random.Range(0, 9);
//             UpdateJaketto();
//             currentJakettoColorIndex = Random.Range(0, 10);
//             UpdateJakettoColor();
//         }
//         if(!lockedSkinColor)
//         {
//             currentSkinColorIndex = Random.Range(0, 7);
//             UpdateSkinColor();
//         }
//     }



    
//     public void UpdateSpecific(int feet, 
//                                 int pants, 
//                                 int waist, 
//                                 int hairCol, 
//                                 int shirt, 
//                                 int height, 
//                                 int bodyType, 
//                                 int width, 
//                                 int pantsCol, 
//                                 int shirtCol, 
//                                 int hairStyle, 
//                                 int jaketto, 
//                                 int jakettoCol, 
//                                 int skinCol)
//     {
//             currentPantsColorIndex = pantsCol;
//             UpdatePantsColor();
        
//             currentShirtColorIndex = shirtCol;
//             UpdateShirtColor();

//             currentJakettoColorIndex = jakettoCol;
//             // UpdateJakettoColor();

//             currentHairColorIndex = hairCol;
//             UpdateHairColor();

//             currentSkinColorIndex = skinCol;
//             UpdateSkinColor();


//             currentShirtIndex = shirt;
//             // UpdateShirt();

//             currentJakettoIndex = jaketto;
//             UpdateJakettoColor();

//             // UpdateShirt();

            
//             currentFeetIndex = feet;
//             UpdateFeet();
       
//             // currentPantsIndex = pants;
//             // UpdatePants();
       
//             currentWaistIndex = waist;
//             UpdateWaist();
      

       
//             currentHeightIndex = height;
//             SetBodyType();
       
//             currentBodyTypeIndex = bodyType;
//             SetBodyType();
       
//             currentWidthIndex = width;
//             SetBodyType();
      
//             currentPantsColorIndex = pantsCol;
//             UpdatePantsColor();

//             currentPantsIndex = pants;
//             UpdatePants();
        
//             currentShirtColorIndex = shirtCol;
//             UpdateShirtColor();
       
//             currentHairStyleIndex = hairStyle;
//             UpdateHairStyle();

     
//             // currentJakettoIndex = jaketto;
//             // UpdateJakettoColor();

      

        
//     }

//     // public void NextPants()
//     // {
//     //     currentPantsIndex++;
//     //     if (currentPantsIndex >= 3)
//     //     {
//     //         currentPantsIndex = 0; // Wrap around to the first currentWidthIndex option
//     //     }
//     //     UpdatePants();
//     // }

//     // public void UpdatePants()
//     // {
//     //     if(currentPantsIndex == 0)
//     //     {
//     //         SetPantsToShorts();
//     //     }
//     //     else if(currentPantsIndex == 1)
//     //     {
//     //         SetPantsTo3QuarterPants();
//     //     }
//     //     else if(currentPantsIndex == 2)
//     //     {
//     //         SetPantsToPants();
//     //     }
//     // }




//     public void ToggleCustomizeUI()
//     {
//         GameObject customizeButtons = transform.Find("customiseButtons").gameObject;
//         bool isActive = customizeButtons.activeSelf;
//         customizeButtons.SetActive(!isActive);  // Toggles the active state
//     }





//     Color HexToColor(string hex)
//     {
//         Color newCol;
//         if (ColorUtility.TryParseHtmlString(hex, out newCol))
//         {
//             return newCol;
//         }
//         else
//         {
//             Debug.LogError("Invalid hex color string: " + hex);
//             return Color.black; // Return black if conversion fails
//         }
//     }
// }




using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomization : MonoBehaviour 
{
    // PlayerAnimationAndMovement playerMovement;
    PlayerAnimationAndMovement playerControllerScript;

    // PlayerCntrlerScript playerControllerScript;
    [SerializeField] GameObject Player;



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
        
        playerControllerScript = Player.GetComponent<PlayerAnimationAndMovement>(); 
        // playerControllerScript = playerMovement;
      
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
            playerControllerScript.bodyTypeNumber = 1 + currentWidthIndex;
        }
        if(currentHeightIndex == 0 && currentBodyTypeIndex == 0)
        {
            playerControllerScript.bodyTypeNumber = 5 + currentWidthIndex;
        }
        if(currentHeightIndex == 1 && currentBodyTypeIndex == 0)
        {
            playerControllerScript.bodyTypeNumber = 9 + currentWidthIndex;
        }
        if(currentHeightIndex == 2 && currentBodyTypeIndex == 1)
        {
            playerControllerScript.bodyTypeNumber = 13 + currentWidthIndex;
        }
        if(currentHeightIndex == 0 && currentBodyTypeIndex == 1)
        {
            playerControllerScript.bodyTypeNumber = 17 + currentWidthIndex;
        }
        if(currentHeightIndex == 1 && currentBodyTypeIndex == 1)
        {
            playerControllerScript.bodyTypeNumber = 21 + currentWidthIndex;
        }
        if(currentHeightIndex == 2 && currentBodyTypeIndex == 2)
        {
            playerControllerScript.bodyTypeNumber = 25 + currentWidthIndex;
        }
        if(currentHeightIndex == 0 && currentBodyTypeIndex == 2)
        {
            playerControllerScript.bodyTypeNumber = 29 + currentWidthIndex;
        }
        if(currentHeightIndex == 1 && currentBodyTypeIndex == 2)
        {
            playerControllerScript.bodyTypeNumber = 33 + currentWidthIndex;
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










































    public void SetShirtToBirthdaySuit()
    {
      playerControllerScript.throatSprite.color = playerControllerScript.currentSkinColor;
      playerControllerScript.collarSprite.color = playerControllerScript.currentSkinColor;
      playerControllerScript.torsoSprite.color = playerControllerScript.currentSkinColor;
      
      if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
      {
        playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentSkinColor;
        playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor;
      }
      else if(currentJakettoIndex == 2) // short sleeve jaketto
      {
        playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
        playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor; 
      }
      else if(currentJakettoIndex == 3) //long sleeeve jaketto
      {
        playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
        playerControllerScript.longSleeveSprite.color = playerControllerScript.currentJakettoColor;
      }

      if(currentWaistIndex == 0) // if waist is set to shirt color
      {
        playerControllerScript.waistSprite.color = playerControllerScript.currentPantsColor;
      }
    }
    public void SetShirtToSinglet1()
    {
      playerControllerScript.throatSprite.color = playerControllerScript.currentSkinColor;
      playerControllerScript.collarSprite.color = playerControllerScript.currentSkinColor;
      playerControllerScript.torsoSprite.color = playerControllerScript.currentShirtColor;
      if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
      {
        playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentSkinColor;
        playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor;
      }
      else if(currentJakettoIndex == 2) // short sleeve jaketto
      {
        playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
        playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor; 
      }
      else if(currentJakettoIndex == 3) //long sleeeve jaketto
      {
        playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
        playerControllerScript.longSleeveSprite.color = playerControllerScript.currentJakettoColor;
      }

      if(currentWaistIndex == 0) // if waist is set to shirt color
      {
        playerControllerScript.waistSprite.color = playerControllerScript.currentShirtColor;
        currentWaistIndex = 0;
      }
    }
    public void SetShirtToSinglet2()
    {
      playerControllerScript.throatSprite.color = playerControllerScript.currentSkinColor;
      playerControllerScript.collarSprite.color = playerControllerScript.currentShirtColor;
      playerControllerScript.torsoSprite.color = playerControllerScript.currentShirtColor;
      if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
      {
        playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentSkinColor;
        playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor;
      }
      else if(currentJakettoIndex == 2) // short sleeve jaketto
      {
        playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
        playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor; 
      }
      else if(currentJakettoIndex == 3) //long sleeeve jaketto
      {
        playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
        playerControllerScript.longSleeveSprite.color = playerControllerScript.currentJakettoColor;
      }

      if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
      {
        playerControllerScript.waistSprite.color = playerControllerScript.currentShirtColor;
      }
    }
    public void SetShirtToSinglet3()
    {
      playerControllerScript.throatSprite.color = playerControllerScript.currentShirtColor;
      playerControllerScript.collarSprite.color = playerControllerScript.currentShirtColor;
      playerControllerScript.torsoSprite.color = playerControllerScript.currentShirtColor;
      if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
      {
        playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentSkinColor;
        playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor;
      }
      else if(currentJakettoIndex == 2) // short sleeve jaketto
      {
        playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
        playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor; 
      }
      else if(currentJakettoIndex == 3) //long sleeeve jaketto
      {
        playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
        playerControllerScript.longSleeveSprite.color = playerControllerScript.currentJakettoColor;
      }

      if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
      {
        playerControllerScript.waistSprite.color = playerControllerScript.currentShirtColor;
      }
    }
    public void SetShirtToTShirt1()
    {
        playerControllerScript.throatSprite.color = playerControllerScript.currentSkinColor;
        playerControllerScript.collarSprite.color = playerControllerScript.currentShirtColor;
        playerControllerScript.torsoSprite.color = playerControllerScript.currentShirtColor;

        if (currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentShirtColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor;
        }
        else if (currentJakettoIndex == 2) // short sleeve jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor;
        }
        else if (currentJakettoIndex == 3) // long sleeve jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentJakettoColor;
        }

        if (playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = playerControllerScript.currentShirtColor;
        }
    }

    public void SetShirtToTShirt2()
    {
        playerControllerScript.throatSprite.color = playerControllerScript.currentSkinColor;
        playerControllerScript.collarSprite.color = playerControllerScript.currentSkinColor;
        playerControllerScript.torsoSprite.color = playerControllerScript.currentShirtColor;

        if (currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentShirtColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor;
        }
        else if (currentJakettoIndex == 2) // short sleeve jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor;
        }
        else if (currentJakettoIndex == 3) // long sleeve jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentJakettoColor;
        }

        if (playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = playerControllerScript.currentShirtColor;
        }
    }

    public void SetShirtToTShirt3()
    {
        playerControllerScript.throatSprite.color = playerControllerScript.currentShirtColor;
        playerControllerScript.collarSprite.color = playerControllerScript.currentShirtColor;
        playerControllerScript.torsoSprite.color = playerControllerScript.currentShirtColor;

        if (currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentShirtColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor;
        }
        else if (currentJakettoIndex == 2) // short sleeve jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentSkinColor;
        }
        else if (currentJakettoIndex == 3) // long sleeve jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentJakettoColor;
        }

        if (playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = playerControllerScript.currentShirtColor;
        }
    }

    public void SetShirtToLSShirt1()
        {
        playerControllerScript.throatSprite.color = playerControllerScript.currentSkinColor;
        playerControllerScript.collarSprite.color = playerControllerScript.currentShirtColor;
        playerControllerScript.torsoSprite.color = playerControllerScript.currentShirtColor;

        if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentShirtColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentShirtColor;
        }
                else if(currentJakettoIndex == 2) // short sleeve jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentShirtColor; 
        }
        else if(currentJakettoIndex == 3) //long sleeeve jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentJakettoColor;
        }


        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = playerControllerScript.currentShirtColor;
        }
        }
        public void SetShirtToLSShirt2()
        {
        playerControllerScript.throatSprite.color = playerControllerScript.currentSkinColor;
        playerControllerScript.collarSprite.color = playerControllerScript.currentSkinColor;
        playerControllerScript.torsoSprite.color = playerControllerScript.currentShirtColor;

        if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentShirtColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentShirtColor;
        }
        else if(currentJakettoIndex == 2) // short sleeve jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentShirtColor; 
        }
        else if(currentJakettoIndex == 3) //long sleeeve jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentJakettoColor;
        }

        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = playerControllerScript.currentShirtColor;
        }
        }
        public void SetShirtToLSShirt3()
        {
        playerControllerScript.throatSprite.color = playerControllerScript.currentShirtColor;
        playerControllerScript.collarSprite.color = playerControllerScript.currentShirtColor;
        playerControllerScript.torsoSprite.color = playerControllerScript.currentShirtColor;
        
        if(currentJakettoIndex == 0 || currentJakettoIndex == 1) // vest or no jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentShirtColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentShirtColor;
        }
        else if(currentJakettoIndex == 2) // short sleeve jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentShirtColor; 
        }
        else if(currentJakettoIndex == 3) //long sleeeve jaketto
        {
            playerControllerScript.shortSleeveSprite.color = playerControllerScript.currentJakettoColor;
            playerControllerScript.longSleeveSprite.color = playerControllerScript.currentJakettoColor;
        }

        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = playerControllerScript.currentShirtColor;
        }
        }


    public void SetPantsToShorts()
    {
        playerControllerScript.waistShortsSprite.color = playerControllerScript.currentPantsColor;
        playerControllerScript.kneesShinsSprite.color = playerControllerScript.currentSkinColor;
        if (playerControllerScript.anklesSprite.color != playerControllerScript.currentShoeColor)
        {
            playerControllerScript.anklesSprite.color = playerControllerScript.currentSkinColor;
        }
        SetNoDress();
    }

    public void SetPantsTo3QuarterPants()
    {
        if (playerControllerScript.anklesSprite.color != playerControllerScript.currentShoeColor)
        {
            playerControllerScript.waistShortsSprite.color = playerControllerScript.currentPantsColor;
            playerControllerScript.kneesShinsSprite.color = playerControllerScript.currentPantsColor;
            playerControllerScript.anklesSprite.color = playerControllerScript.currentSkinColor;
        }
        else
        {
            NextPants();
        }
        SetNoDress();
    }

    public void SetPantsToPants()
    {
        playerControllerScript.waistShortsSprite.color = playerControllerScript.currentPantsColor;
        playerControllerScript.kneesShinsSprite.color = playerControllerScript.currentPantsColor;
        if (playerControllerScript.anklesSprite.color != playerControllerScript.currentShoeColor)
        {
            playerControllerScript.anklesSprite.color = playerControllerScript.currentPantsColor;
        }
        SetNoDress();
    }

        public void SetPantsToDress()
    {
 
      playerControllerScript.waistShortsSprite.color = playerControllerScript.currentPantsColor;
      playerControllerScript.kneesShinsSprite.color = playerControllerScript.currentSkinColor;
      if(playerControllerScript.anklesSprite.color != playerControllerScript.currentShoeColor)
      {
        playerControllerScript.anklesSprite.color = playerControllerScript.currentSkinColor;
      }

      Color color = playerControllerScript.currentPantsColor;
      color.a = 1f;
      playerControllerScript.dressSprite.color = color;  
    }


    public void SetNoDress()
    {
        Color color = playerControllerScript.currentPantsColor;
        color.a = 0f;
        playerControllerScript.dressSprite.color = color;
    }


    public void SetWaistToShirt()
    {
        if (currentShirtIndex == 0) // if no shirt is on
        {
            currentWaistIndex = 1; // set waist to pants
            playerControllerScript.waistSprite.color = playerControllerScript.currentPantsColor;
        }
        else
        {
            playerControllerScript.waistSprite.color = playerControllerScript.currentShirtColor;
        }
    }

    public void SetWaistToPants()
    {
        playerControllerScript.waistSprite.color = playerControllerScript.currentPantsColor;
    }

    public void SetFeetToShoes()
    {
        playerControllerScript.feetSprite.color = playerControllerScript.currentShoeColor;
        if (currentPantsIndex == 2)
        {
            playerControllerScript.anklesSprite.color = playerControllerScript.currentPantsColor;
        }
        else
        {
            playerControllerScript.anklesSprite.color = playerControllerScript.currentSkinColor;
        }
    }

    public void SetFeetToBareFoot()
    {
        playerControllerScript.feetSprite.color = playerControllerScript.currentSkinColor;
        if (currentPantsIndex == 2)
        {
            playerControllerScript.anklesSprite.color = playerControllerScript.currentPantsColor;
        }
        else
        {
            playerControllerScript.anklesSprite.color = playerControllerScript.currentSkinColor;
        }
    }

    public void SetFeetToBoots()
    {
        playerControllerScript.feetSprite.color = playerControllerScript.currentShoeColor;
        playerControllerScript.anklesSprite.color = playerControllerScript.currentShoeColor;
    }

    public void SetNoJaketto()
    {
        Color color = playerControllerScript.currentJakettoColor;
        color.a = 0f;
        playerControllerScript.jakettoSprite.color = color;
        UpdateShirt();
    }

    public void SetJakettoVest()
    {
        Color color = playerControllerScript.currentJakettoColor;
        color.a = 1f;
        playerControllerScript.jakettoSprite.color = color;
        UpdateShirt();
    }



    public void SetSkinColor1()
    {
        if (playerControllerScript.headSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.headSprite.color = HexToColor("#F5CBA7");
        }
        if (playerControllerScript.throatSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#F5CBA7");
        }
        if (playerControllerScript.collarSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#F5CBA7");
        }
        if (playerControllerScript.torsoSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#F5CBA7");
        }
        if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistShortsSprite.color = HexToColor("#F5CBA7");
        }
        if (playerControllerScript.waistSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#F5CBA7");
        }
        if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.kneesShinsSprite.color = HexToColor("#F5CBA7");
        }
        if (playerControllerScript.anklesSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.anklesSprite.color = HexToColor("#F5CBA7");
        }
        if (playerControllerScript.feetSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.feetSprite.color = HexToColor("#F5CBA7");
        }
        if (playerControllerScript.longSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#F5CBA7");
        }
        if (playerControllerScript.handSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.handSprite.color = HexToColor("#F5CBA7");
        }
        if (playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#F5CBA7");
        }
        playerControllerScript.currentSkinColor = HexToColor("#F5CBA7");
        skinColorButton2.color = playerControllerScript.currentSkinColor;
    }

    public void SetSkinColor2()
    {
        if (playerControllerScript.headSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.headSprite.color = HexToColor("#F6B883");
        }
        if (playerControllerScript.throatSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#F6B883");
        }
        if (playerControllerScript.collarSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#F6B883");
        }
        if (playerControllerScript.torsoSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#F6B883");
        }
        if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistShortsSprite.color = HexToColor("#F6B883");
        }
        if (playerControllerScript.waistSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#F6B883");
        }
        if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.kneesShinsSprite.color = HexToColor("#F6B883");
        }
        if (playerControllerScript.anklesSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.anklesSprite.color = HexToColor("#F6B883");
        }
        if (playerControllerScript.feetSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.feetSprite.color = HexToColor("#F6B883");
        }
        if (playerControllerScript.longSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#F6B883");
        }
        if (playerControllerScript.handSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.handSprite.color = HexToColor("#F6B883");
        }
        if (playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#F6B883");
        }
        playerControllerScript.currentSkinColor = HexToColor("#F6B883");
        skinColorButton2.color = playerControllerScript.currentSkinColor;
    }

    public void SetSkinColor3()
    {
        if (playerControllerScript.headSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.headSprite.color = HexToColor("#E1A95F");
        }
        if (playerControllerScript.throatSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#E1A95F");
        }
        if (playerControllerScript.collarSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#E1A95F");
        }
        if (playerControllerScript.torsoSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#E1A95F");
        }
        if (playerControllerScript.waistSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#E1A95F");
        }
        if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistShortsSprite.color = HexToColor("#E1A95F");
        }
        if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.kneesShinsSprite.color = HexToColor("#E1A95F");
        }
        if (playerControllerScript.anklesSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.anklesSprite.color = HexToColor("#E1A95F");
        }
        if (playerControllerScript.feetSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.feetSprite.color = HexToColor("#E1A95F");
        }
        if (playerControllerScript.longSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#E1A95F");
        }
        if (playerControllerScript.handSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.handSprite.color = HexToColor("#E1A95F");
        }
        if (playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#E1A95F");
        }
        playerControllerScript.currentSkinColor = HexToColor("#E1A95F");
        skinColorButton2.color = playerControllerScript.currentSkinColor;
    }

    public void SetSkinColor4()
    {
        if (playerControllerScript.headSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.headSprite.color = HexToColor("#C68642");
        }
        if (playerControllerScript.throatSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#C68642");
        }
        if (playerControllerScript.collarSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#C68642");
        }
        if (playerControllerScript.torsoSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#C68642");
        }
        if (playerControllerScript.waistSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#C68642");
        }
        if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistShortsSprite.color = HexToColor("#C68642");
        }
        if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.kneesShinsSprite.color = HexToColor("#C68642");
        }
        if (playerControllerScript.anklesSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.anklesSprite.color = HexToColor("#C68642");
        }
        if (playerControllerScript.feetSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.feetSprite.color = HexToColor("#C68642");
        }
        if (playerControllerScript.longSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#C68642");
        }
        if (playerControllerScript.handSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.handSprite.color = HexToColor("#C68642");
        }
        if (playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#C68642");
        }
        playerControllerScript.currentSkinColor = HexToColor("#C68642");
        skinColorButton2.color = playerControllerScript.currentSkinColor;
    }

    public void SetSkinColor5()
    {
        if (playerControllerScript.headSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.headSprite.color = HexToColor("#B57A3D");
        }
        if (playerControllerScript.throatSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#B57A3D");
        }
        if (playerControllerScript.collarSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#B57A3D");
        }
        if (playerControllerScript.torsoSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#B57A3D");
        }
        if (playerControllerScript.waistSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#B57A3D");
        }
        if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistShortsSprite.color = HexToColor("#B57A3D");
        }
        if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.kneesShinsSprite.color = HexToColor("#B57A3D");
        }
        if (playerControllerScript.anklesSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.anklesSprite.color = HexToColor("#B57A3D");
        }
        if (playerControllerScript.feetSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.feetSprite.color = HexToColor("#B57A3D");
        }
        if (playerControllerScript.longSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#B57A3D");
        }
        if (playerControllerScript.handSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.handSprite.color = HexToColor("#B57A3D");
        }
        if (playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#B57A3D");
        }
        playerControllerScript.currentSkinColor = HexToColor("#B57A3D");
        skinColorButton2.color = playerControllerScript.currentSkinColor;
    }

    public void SetSkinColor6()
    {
        if (playerControllerScript.headSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.headSprite.color = HexToColor("#DFA10B");
        }
        if (playerControllerScript.throatSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#DFA10B");
        }
        if (playerControllerScript.collarSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#DFA10B");
        }
        if (playerControllerScript.torsoSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#DFA10B");
        }
        if (playerControllerScript.waistSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#DFA10B");
        }
        if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistShortsSprite.color = HexToColor("#DFA10B");
        }
        if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.kneesShinsSprite.color = HexToColor("#DFA10B");
        }
        if (playerControllerScript.anklesSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.anklesSprite.color = HexToColor("#DFA10B");
        }
        if (playerControllerScript.feetSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.feetSprite.color = HexToColor("#DFA10B");
        }
        if (playerControllerScript.longSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#DFA10B");
        }
        if (playerControllerScript.handSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.handSprite.color = HexToColor("#DFA10B");
        }
        if (playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#DFA10B");
        }
        playerControllerScript.currentSkinColor = HexToColor("#DFA10B");
        skinColorButton2.color = playerControllerScript.currentSkinColor;
    }

    public void SetSkinColor7()
    {
        if (playerControllerScript.headSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.headSprite.color = HexToColor("#E1B24B");
        }
        if (playerControllerScript.throatSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#E1B24B");
        }
        if (playerControllerScript.collarSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#E1B24B");
        }
        if (playerControllerScript.torsoSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#E1B24B");
        }
        if (playerControllerScript.waistSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#E1B24B");
        }
        if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.waistShortsSprite.color = HexToColor("#E1B24B");
        }
        if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.kneesShinsSprite.color = HexToColor("#E1B24B");
        }
        if (playerControllerScript.anklesSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.anklesSprite.color = HexToColor("#E1B24B");
        }
        if (playerControllerScript.feetSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.feetSprite.color = HexToColor("#E1B24B");
        }
        if (playerControllerScript.longSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#E1B24B");
        }
        if (playerControllerScript.handSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.handSprite.color = HexToColor("#E1B24B");
        }
        if (playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentSkinColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#E1B24B");
        }
        playerControllerScript.currentSkinColor = HexToColor("#E1B24B");
        skinColorButton2.color = playerControllerScript.currentSkinColor;
    }


    public void SetHairColor1()
    {
        Transform hair = playerControllerScript.transform.Find("hair");
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
            playerControllerScript.currentHairColor = HexToColor("#4E342E");
            hairColorButton.color = playerControllerScript.currentHairColor;
        }
    }

    public void SetHairColor2()
    {
        Transform hair = playerControllerScript.transform.Find("hair");
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
            playerControllerScript.currentHairColor = HexToColor("#1C1C1C");
            hairColorButton.color = playerControllerScript.currentHairColor;
        }
    }

    public void SetHairColor3()
    {
        Transform hair = playerControllerScript.transform.Find("hair");
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
            playerControllerScript.currentHairColor = HexToColor("#F5D76E");
            hairColorButton.color = playerControllerScript.currentHairColor;
        }
    }

    public void SetHairColor4()
    {
        Transform hair = playerControllerScript.transform.Find("hair");
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
            playerControllerScript.currentHairColor = HexToColor("#A52A2A");
            hairColorButton.color = playerControllerScript.currentHairColor;
        }
    }

    public void SetHairColor5()
    {
        Transform hair = playerControllerScript.transform.Find("hair");
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
            playerControllerScript.currentHairColor = HexToColor("#A9A9A9");
            hairColorButton.color = playerControllerScript.currentHairColor;
        }
    }

    public void SetHairColor6()
    {
        Transform hair = playerControllerScript.transform.Find("hair");
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
            playerControllerScript.currentHairColor = HexToColor("#00BFFF");
            hairColorButton.color = playerControllerScript.currentHairColor;
        }
    }

    public void SetHairColor7()
    {
        Transform hair = playerControllerScript.transform.Find("hair");
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
            playerControllerScript.currentHairColor = HexToColor("#FFB6C1");
            hairColorButton.color = playerControllerScript.currentHairColor;
        }
    }

    public void SetHairColor8()
    {
        Transform hair = playerControllerScript.transform.Find("hair");
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
            playerControllerScript.currentHairColor = HexToColor("#E6E6FA");
            hairColorButton.color = playerControllerScript.currentHairColor;
        }
    }

    public void SetHairColor9()
    {
        Transform hair = playerControllerScript.transform.Find("hair");
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
            playerControllerScript.currentHairColor = HexToColor("#98FF98");
            hairColorButton.color = playerControllerScript.currentHairColor;
        }
    }

    public void SetHairColor10()
    {
        Transform hair = playerControllerScript.transform.Find("hair");
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
            playerControllerScript.currentHairColor = HexToColor("#8A2BE2");
            hairColorButton.color = playerControllerScript.currentHairColor;
        }
    }



    public void SetHairStyle1()
    {
        Transform hair = playerControllerScript.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair0Top") || child.gameObject.name.Contains("hair0Bottom"))
                  {
                      spriteRenderer.color = playerControllerScript.currentHairColor;  
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
        Transform hair = playerControllerScript.transform.Find("hair");
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
                      spriteRenderer.color = playerControllerScript.currentHairColor;                    
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
        Transform hair = playerControllerScript.transform.Find("hair");
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
                      spriteRenderer.color = playerControllerScript.currentHairColor;                    
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
        Transform hair = playerControllerScript.transform.Find("hair");
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
                      spriteRenderer.color = playerControllerScript.currentHairColor;  
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
        Transform hair = playerControllerScript.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("mohawk"))
                  {
                      spriteRenderer.color = playerControllerScript.currentHairColor;  
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
        Transform hair = playerControllerScript.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") || child.gameObject.name.Contains("hair2Bottom") )
                  {
                      spriteRenderer.color = playerControllerScript.currentHairColor;  
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
        Transform hair = playerControllerScript.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") || child.gameObject.name.Contains("hair3Bottom") )
                  {
                      spriteRenderer.color = playerControllerScript.currentHairColor;  
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
        Transform hair = playerControllerScript.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") || child.gameObject.name.Contains("hair4Bottom") )
                  {
                      spriteRenderer.color = playerControllerScript.currentHairColor;  
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
        Transform hair = playerControllerScript.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair1Top") || child.gameObject.name.Contains("hair1Bottom") || child.gameObject.name.Contains("hair6Bottom") )
                  {
                      spriteRenderer.color = playerControllerScript.currentHairColor;  
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
        Transform hair = playerControllerScript.transform.Find("hair");
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
                      spriteRenderer.color = playerControllerScript.currentHairColor;  
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
        Transform hair = playerControllerScript.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair8Top") || child.gameObject.name.Contains("hair8Bottom"))
                  {
                      spriteRenderer.color = playerControllerScript.currentHairColor;  
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
        Transform hair = playerControllerScript.transform.Find("hair");
        if (hair != null)
        {
            foreach (Transform child in hair)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                  if (child.gameObject.name.Contains("hair0Bottom"))
                  {
                      spriteRenderer.color = playerControllerScript.currentHairColor;  
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
        Transform hair = playerControllerScript.transform.Find("hair");
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
        if(playerControllerScript.throatSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#FF6347");
        }
        if(playerControllerScript.collarSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#FF6347");
        }
        if(playerControllerScript.torsoSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#FF6347");
        }
        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#FF6347");
        }
        if(playerControllerScript.longSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#FF6347");
        }
        if(playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#FF6347");
        }
        playerControllerScript.currentShirtColor = HexToColor("#FF6347");
        shirtColorButton.color = playerControllerScript.currentShirtColor;
    }

    public void SetShirtColor2()
    {
        if(playerControllerScript.throatSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#4682B4");
        }
        if(playerControllerScript.collarSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#4682B4");
        }
        if(playerControllerScript.torsoSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#4682B4");
        }
        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#4682B4");
        }
        if(playerControllerScript.longSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#4682B4");
        }
        if(playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#4682B4");
        }
        playerControllerScript.currentShirtColor = HexToColor("#4682B4");
        shirtColorButton.color = playerControllerScript.currentShirtColor;
    }

    public void SetShirtColor3()
    {
        if(playerControllerScript.throatSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#32CD32");
        }
        if(playerControllerScript.collarSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#32CD32");
        }
        if(playerControllerScript.torsoSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#32CD32");
        }
        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#32CD32");
        }
        if(playerControllerScript.longSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#32CD32");
        }
        if(playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#32CD32");
        }
        playerControllerScript.currentShirtColor = HexToColor("#32CD32");
        shirtColorButton.color = playerControllerScript.currentShirtColor;
    }

    public void SetShirtColor4()
    {
        if(playerControllerScript.throatSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#FFD700");
        }
        if(playerControllerScript.collarSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#FFD700");
        }
        if(playerControllerScript.torsoSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#FFD700");
        }
        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#FFD700");
        }
        if(playerControllerScript.longSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#FFD700");
        }
        if(playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#FFD700");
        }
        playerControllerScript.currentShirtColor = HexToColor("#FFD700");
        shirtColorButton.color = playerControllerScript.currentShirtColor;
    }

    public void SetShirtColor5()
    {
        if(playerControllerScript.throatSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#8A2BE2");
        }
        if(playerControllerScript.collarSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#8A2BE2");
        }
        if(playerControllerScript.torsoSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#8A2BE2");
        }
        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#8A2BE2");
        }
        if(playerControllerScript.longSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#8A2BE2");
        }
        if(playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#8A2BE2");
        }
        playerControllerScript.currentShirtColor = HexToColor("#8A2BE2");
        shirtColorButton.color = playerControllerScript.currentShirtColor;
    }

    public void SetShirtColor6()
    {
        if(playerControllerScript.throatSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#DC143C");
        }
        if(playerControllerScript.collarSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#DC143C");
        }
        if(playerControllerScript.torsoSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#DC143C");
        }
        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#DC143C");
        }
        if(playerControllerScript.longSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#DC143C");
        }
        if(playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#DC143C");
        }
        playerControllerScript.currentShirtColor = HexToColor("#DC143C");
        shirtColorButton.color = playerControllerScript.currentShirtColor;
    }

    public void SetShirtColor7()
    {
        if(playerControllerScript.throatSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#20B2AA");
        }
        if(playerControllerScript.collarSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#20B2AA");
        }
        if(playerControllerScript.torsoSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#20B2AA");
        }
        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#20B2AA");
        }
        if(playerControllerScript.longSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#20B2AA");
        }
        if(playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#20B2AA");
        }
        playerControllerScript.currentShirtColor = HexToColor("#20B2AA");
        shirtColorButton.color = playerControllerScript.currentShirtColor;
    }

    public void SetShirtColor8()
    {
        if(playerControllerScript.throatSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#D3D3D3");
        };
        if(playerControllerScript.collarSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#D3D3D3");
        };
        if(playerControllerScript.torsoSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#D3D3D3");
        };
        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#D3D3D3");
        };
        if(playerControllerScript.longSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#D3D3D3");
        };
        if(playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#D3D3D3");
        };
        playerControllerScript.currentShirtColor = HexToColor("#D3D3D3");
        shirtColorButton.color = playerControllerScript.currentShirtColor;
    }

    public void SetShirtColor9()
    {
        if(playerControllerScript.throatSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#F08080");
        };
        if(playerControllerScript.collarSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#F08080");
        };
        if(playerControllerScript.torsoSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#F08080");
        };
        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#F08080");
        };
        if(playerControllerScript.longSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#F08080");
        };
        if(playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#F08080");
        };
        playerControllerScript.currentShirtColor = HexToColor("#F08080");
        shirtColorButton.color = playerControllerScript.currentShirtColor;
    }

    public void SetShirtColor10()
    {
        if(playerControllerScript.throatSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#2F4F4F");
        };
        if(playerControllerScript.collarSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#2F4F4F");
        };
        if(playerControllerScript.torsoSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#2F4F4F");
        };
        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#2F4F4F");
        };
        if(playerControllerScript.longSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#2F4F4F");
        };
        if(playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#2F4F4F");
        };
        playerControllerScript.currentShirtColor = HexToColor("#2F4F4F");
        shirtColorButton.color = playerControllerScript.currentShirtColor;
    }

    public void SetShirtColor11()
    {
        if(playerControllerScript.throatSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.throatSprite.color = HexToColor("#FF34F4");
        };
        if(playerControllerScript.collarSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.collarSprite.color = HexToColor("#FF34F4");
        };
        if(playerControllerScript.torsoSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.torsoSprite.color = HexToColor("#FF34F4");
        };
        if(playerControllerScript.waistSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.waistSprite.color = HexToColor("#FF34F4");
        };
        if(playerControllerScript.longSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.longSleeveSprite.color = HexToColor("#FF34F4");
        };
        if(playerControllerScript.shortSleeveSprite.color == playerControllerScript.currentShirtColor)
        {
            playerControllerScript.shortSleeveSprite.color = HexToColor("#FF34F4");
        };
        playerControllerScript.currentShirtColor = HexToColor("#FF34F4");
        shirtColorButton.color = playerControllerScript.currentShirtColor;
    }




public void SetPantsColor1()
{
    playerControllerScript.dressSprite.color = HexToColor("#2C3E50");
    if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistShortsSprite.color = HexToColor("#2C3E50");
    }
    if (playerControllerScript.waistSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistSprite.color = HexToColor("#2C3E50");
    }
    if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.kneesShinsSprite.color = HexToColor("#2C3E50");
    }
    if (playerControllerScript.anklesSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.anklesSprite.color = HexToColor("#2C3E50");
    }
    playerControllerScript.currentPantsColor = HexToColor("#2C3E50");
    pantsColorButton.color = playerControllerScript.currentPantsColor;
}

public void SetPantsColor2()
{
    playerControllerScript.dressSprite.color = HexToColor("#A0522D");
    if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistShortsSprite.color = HexToColor("#A0522D");
    }
    if (playerControllerScript.waistSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistSprite.color = HexToColor("#A0522D");
    }
    if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.kneesShinsSprite.color = HexToColor("#A0522D");
    }
    if (playerControllerScript.anklesSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.anklesSprite.color = HexToColor("#A0522D");
    }
    playerControllerScript.currentPantsColor = HexToColor("#A0522D");
    pantsColorButton.color = playerControllerScript.currentPantsColor;
}

public void SetPantsColor3()
{
    playerControllerScript.dressSprite.color = HexToColor("#808080");
    if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistShortsSprite.color = HexToColor("#808080");
    }
    if (playerControllerScript.waistSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistSprite.color = HexToColor("#808080");
    }
    if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.kneesShinsSprite.color = HexToColor("#808080");
    }
    if (playerControllerScript.anklesSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.anklesSprite.color = HexToColor("#808080");
    }
    playerControllerScript.currentPantsColor = HexToColor("#808080");
    pantsColorButton.color = playerControllerScript.currentPantsColor;
}

public void SetPantsColor4()
{
    playerControllerScript.dressSprite.color = HexToColor("#556B2F");
    if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistShortsSprite.color = HexToColor("#556B2F");
    }
    if (playerControllerScript.waistSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistSprite.color = HexToColor("#556B2F");
    }
    if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.kneesShinsSprite.color = HexToColor("#556B2F");
    }
    if (playerControllerScript.anklesSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.anklesSprite.color = HexToColor("#556B2F");
    }
    playerControllerScript.currentPantsColor = HexToColor("#556B2F");
    pantsColorButton.color = playerControllerScript.currentPantsColor;
}

public void SetPantsColor5()
{
    playerControllerScript.dressSprite.color = HexToColor("#B0C4DE");
    if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistShortsSprite.color = HexToColor("#B0C4DE");
    }
    if (playerControllerScript.waistSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistSprite.color = HexToColor("#B0C4DE");
    }
    if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.kneesShinsSprite.color = HexToColor("#B0C4DE");
    }
    if (playerControllerScript.anklesSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.anklesSprite.color = HexToColor("#B0C4DE");
    }
    playerControllerScript.currentPantsColor = HexToColor("#B0C4DE");
    pantsColorButton.color = playerControllerScript.currentPantsColor;
}

public void SetPantsColor6()
{
    playerControllerScript.dressSprite.color = HexToColor("#4B0082");
    if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistShortsSprite.color = HexToColor("#4B0082");
    }
    if (playerControllerScript.waistSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistSprite.color = HexToColor("#4B0082");
    }
    if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.kneesShinsSprite.color = HexToColor("#4B0082");
    }
    if (playerControllerScript.anklesSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.anklesSprite.color = HexToColor("#4B0082");
    }
    playerControllerScript.currentPantsColor = HexToColor("#4B0082");
    pantsColorButton.color = playerControllerScript.currentPantsColor;
}

public void SetPantsColor7()
{
    playerControllerScript.dressSprite.color = HexToColor("#8B4513");
    if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistShortsSprite.color = HexToColor("#8B4513");
    }
    if (playerControllerScript.waistSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistSprite.color = HexToColor("#8B4513");
    }
    if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.kneesShinsSprite.color = HexToColor("#8B4513");
    }
    if (playerControllerScript.anklesSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.anklesSprite.color = HexToColor("#8B4513");
    }
    playerControllerScript.currentPantsColor = HexToColor("#8B4513");
    pantsColorButton.color = playerControllerScript.currentPantsColor;
}

public void SetPantsColor8()
{
    playerControllerScript.dressSprite.color = HexToColor("#696969");
    if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistShortsSprite.color = HexToColor("#696969");
    }
    if (playerControllerScript.waistSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistSprite.color = HexToColor("#696969");
    }
    if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.kneesShinsSprite.color = HexToColor("#696969");
    }
    if (playerControllerScript.anklesSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.anklesSprite.color = HexToColor("#696969");
    }
    playerControllerScript.currentPantsColor = HexToColor("#696969");
    pantsColorButton.color = playerControllerScript.currentPantsColor;
}

public void SetPantsColor9()
{
    playerControllerScript.dressSprite.color = HexToColor("#4682B4");
    if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistShortsSprite.color = HexToColor("#4682B4");
    }
    if (playerControllerScript.waistSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistSprite.color = HexToColor("#4682B4");
    }
    if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.kneesShinsSprite.color = HexToColor("#4682B4");
    }
    if (playerControllerScript.anklesSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.anklesSprite.color = HexToColor("#4682B4");
    }
    playerControllerScript.currentPantsColor = HexToColor("#4682B4");
    pantsColorButton.color = playerControllerScript.currentPantsColor;
}

public void SetPantsColor10()
{
    playerControllerScript.dressSprite.color = HexToColor("#000000");
    if (playerControllerScript.waistShortsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistShortsSprite.color = HexToColor("#000000");
    }
    if (playerControllerScript.waistSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.waistSprite.color = HexToColor("#000000");
    }
    if (playerControllerScript.kneesShinsSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.kneesShinsSprite.color = HexToColor("#000000");
    }
    if (playerControllerScript.anklesSprite.color == playerControllerScript.currentPantsColor)
    {
        playerControllerScript.anklesSprite.color = HexToColor("#000000");
    }
    playerControllerScript.currentPantsColor = HexToColor("#000000");
    pantsColorButton.color = playerControllerScript.currentPantsColor;
}


    public void SetJakettoColor1()
    {
        playerControllerScript.currentJakettoColor = HexToColor("#556B2F");  // Dark Olive
        jackettoColorButton.color = playerControllerScript.currentJakettoColor;
        UpdateJaketto();
    }

    public void SetJakettoColor2()
    {
        playerControllerScript.currentJakettoColor = HexToColor("#800020");  // Deep Burgundy
        jackettoColorButton.color = playerControllerScript.currentJakettoColor;
        UpdateJaketto();
    }

    public void SetJakettoColor3()
    {
        playerControllerScript.currentJakettoColor = HexToColor("#6A5ACD");  // Slate Blue
        jackettoColorButton.color = playerControllerScript.currentJakettoColor;
        UpdateJaketto();
    }

    public void SetJakettoColor4()
    {
        playerControllerScript.currentJakettoColor = HexToColor("#FFDB58");  // Mustard Yellow
        jackettoColorButton.color = playerControllerScript.currentJakettoColor;
        UpdateJaketto();
    }

    public void SetJakettoColor5()
    {
        playerControllerScript.currentJakettoColor = HexToColor("#36454F");  // Charcoal
        jackettoColorButton.color = playerControllerScript.currentJakettoColor;
        UpdateJaketto();
    }

    public void SetJakettoColor6()
    {
        playerControllerScript.currentJakettoColor = HexToColor("#228B22");  // Forest Green
        jackettoColorButton.color = playerControllerScript.currentJakettoColor;
        UpdateJaketto();
    }

    public void SetJakettoColor7()
    {
        playerControllerScript.currentJakettoColor = HexToColor("#000080");  // Navy Blue
        jackettoColorButton.color = playerControllerScript.currentJakettoColor;
        UpdateJaketto();
    }

    public void SetJakettoColor8()
    {
        playerControllerScript.currentJakettoColor = HexToColor("#8B4513");  // Chocolate Brown
        jackettoColorButton.color = playerControllerScript.currentJakettoColor;
        UpdateJaketto();
    }

    public void SetJakettoColor9()
    {
        playerControllerScript.currentJakettoColor = HexToColor("#008080");  // Teal
        jackettoColorButton.color = playerControllerScript.currentJakettoColor;
        UpdateJaketto();
    }

    public void SetJakettoColor10()
    {
        playerControllerScript.currentJakettoColor = HexToColor("#800000");  // Maroon
        jackettoColorButton.color = playerControllerScript.currentJakettoColor;
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
