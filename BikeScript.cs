using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeScript : MonoBehaviour
{
    PlayerAnimationAndMovement playerMovement;
    IsoSpriteSorting isoSpriteSorting;

    [SerializeField] GameObject bike;
    public SpriteRenderer bikeSprite;

    public Sprite[] allBikeSprites;

    public Color bikeColor;

    public Vector3 scale;



    // public GameObject bikeCollider;
    [SerializeField] GameObject Player;


    // Start is called before the first frame update
    public void Start()
    {
      Player = GameObject.FindGameObjectWithTag("Player");
      playerMovement = Player.GetComponent<PlayerAnimationAndMovement>();

      bikeSprite = bike.transform.Find("bikeSprite").GetComponent<SpriteRenderer>();
      isoSpriteSorting = bike.GetComponent<IsoSpriteSorting>();
      bikeColor = bikeSprite.color;
    //   bikeCollider = bike.transform.Find("bikeCollider").gameObject;


      allBikeSprites = Resources.LoadAll<Sprite>("bike");
      bikeSprite.sprite = allBikeSprites[1];

    }


    public void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {    
            if (playerMovement.spaceBarDeactivated == false)
            {
                if ((Input.GetKey(KeyCode.Space) || 
    Input.GetKey(KeyCode.JoystickButton0) ||  // A button
    Input.GetKey(KeyCode.JoystickButton1) ||  // B button
    Input.GetKey(KeyCode.JoystickButton2) ||  // X button
    Input.GetKey(KeyCode.JoystickButton3)) && playerMovement.playerIsOutside && !playerMovement.playerOnBike)
                {
                    playerMovement.StartDeactivateSpaceBar(); // Use centralized method

                    playerMovement.playerOnBike = true;
                    bikeColor.a = 0f;
                    bikeSprite.color = bikeColor;
                    bike.transform.Find("bikeCollider").gameObject.SetActive(false);
                }
            }
        }
    }

    // public void OnTriggerExit2D(Collider2D other)
    // {
    //     if(other.CompareTag("Player"))
    //     {   
    //         playerMovement.StopDeactivateSpaceBar(); // Use centralized method
    //     }
    // }


    public void GetOffBoike()
    {
    //   bikeCollider.GetComponent<BoxCollider2D>().isTrigger = true;
      bike.transform.Find("bikeCollider").gameObject.SetActive(true);                
      bikeColor.a = 1;
      bikeSprite.color = bikeColor;

      if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
      {
        this.transform.parent.localScale = new Vector3(1,1,1);
        bikeSprite.flipX = true;
        bikeSprite.sprite = allBikeSprites[0];
        isoSpriteSorting.SorterPositionOffset.y = -8;
        isoSpriteSorting.SorterPositionOffset2.y = -0;
      }
      else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
      {
        this.transform.parent.localScale = new Vector3(-1,1,1);
        bikeSprite.flipX = true;
        bikeSprite.sprite = allBikeSprites[0];
        isoSpriteSorting.SorterPositionOffset.y = 0;
        isoSpriteSorting.SorterPositionOffset2.y = -8;
      }
      else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim 
      || playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
      {
        this.transform.parent.localScale = new Vector3(-1,1,1);
        bikeSprite.flipX = false;
        bikeSprite.sprite = allBikeSprites[1];
        isoSpriteSorting.SorterPositionOffset.y = 0;
        isoSpriteSorting.SorterPositionOffset2.y = -8;
      }
      else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim 
      || playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
      {
        this.transform.parent.localScale = new Vector3(1,1,1);
        bikeSprite.flipX = false;
        bikeSprite.sprite = allBikeSprites[1];
        isoSpriteSorting.SorterPositionOffset.y = -8;
        isoSpriteSorting.SorterPositionOffset2.y = 0;
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
