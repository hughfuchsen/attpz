using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeTransformAdjustment : MonoBehaviour
{

    PlayerAnimationAndMovement playerMovement;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject bike;

    public int rideBikeInt = 15;


    public Vector3 initialBikeTransformPosition;
    public Vector3 newBikeTransformPosition;

    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerAnimationAndMovement>();  

        bike = transform.Find("bike").gameObject;
        initialBikeTransformPosition = bike.transform.localPosition;   
    }


//   public int rightDownAnim;
//   public int leftDownAnim;
//   public int rightAnim;
//   public int leftAnim;
//   public int upRightAnim;
//   public int upLeftAnim;
    public void SetBikeTransformPosition()
    {
        if(playerMovement.bodyTypeNumber == 1 || playerMovement.bodyTypeNumber == 2)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, 0, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, 0, 0);
            }
        }



        else if(playerMovement.bodyTypeNumber == 3 || playerMovement.bodyTypeNumber == 4)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, 0, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, 0, 0);
            }
        }
        
   
        else if(playerMovement.bodyTypeNumber == 5)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, -1, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, -1f, 0);
            }
        }

        else if(playerMovement.bodyTypeNumber == 6)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, -1, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -1f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 7 || playerMovement.bodyTypeNumber == 8)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -1f, 0);
            }
        }
       
        else if(playerMovement.bodyTypeNumber == 9)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, 1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, 1f, 0);
            }
        }
  
        else if(playerMovement.bodyTypeNumber == 10  || playerMovement.bodyTypeNumber == 11 || playerMovement.bodyTypeNumber == 12)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 1f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 13)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, 0f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 14 || playerMovement.bodyTypeNumber == 15 || playerMovement.bodyTypeNumber == 16)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 17)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, -2f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 18 || playerMovement.bodyTypeNumber == 19 || playerMovement.bodyTypeNumber == 20)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -2f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 21)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, 1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, 1f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 22 || playerMovement.bodyTypeNumber == 23 || playerMovement.bodyTypeNumber == 24)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 1f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 25)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, -1f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 26 || playerMovement.bodyTypeNumber == 27 || playerMovement.bodyTypeNumber == 28)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -1f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 29)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, -1f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 30 || playerMovement.bodyTypeNumber == 31 || playerMovement.bodyTypeNumber == 32)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -1f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 33)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, 0f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, 0f, 0);
            }
        }
        
        else if(playerMovement.bodyTypeNumber == 34 || playerMovement.bodyTypeNumber == 35 || playerMovement.bodyTypeNumber == 36)
        {
            if(playerMovement.currentAnimationDirection == playerMovement.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0, 0);
            }

            else if(playerMovement.currentAnimationDirection == playerMovement.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0, 0);
            }
        }
        
        
   
    }

}
