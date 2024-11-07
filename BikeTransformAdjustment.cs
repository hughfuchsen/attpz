using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeTransformAdjustment : MonoBehaviour
{

    CharacterAnimation characterAnimation;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject bike;

    public int rideBikeInt = 15;


    public Vector3 initialBikeTransformPosition;
    public Vector3 newBikeTransformPosition;

    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        characterAnimation = Player.GetComponent<CharacterAnimation>();  

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
        if(characterAnimation.bodyTypeNumber == 1 || characterAnimation.bodyTypeNumber == 2)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, 0, 0);
            }
        }



        else if(characterAnimation.bodyTypeNumber == 3 || characterAnimation.bodyTypeNumber == 4)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, 0, 0);
            }
        }
        
   
        else if(characterAnimation.bodyTypeNumber == 5)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, -1, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, -1f, 0);
            }
        }

        else if(characterAnimation.bodyTypeNumber == 6)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, -1, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -1f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 7 || characterAnimation.bodyTypeNumber == 8)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -1f, 0);
            }
        }
       
        else if(characterAnimation.bodyTypeNumber == 9)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, 1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, 1f, 0);
            }
        }
  
        else if(characterAnimation.bodyTypeNumber == 10  || characterAnimation.bodyTypeNumber == 11 || characterAnimation.bodyTypeNumber == 12)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 1f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 13)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, 0f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 14 || characterAnimation.bodyTypeNumber == 15 || characterAnimation.bodyTypeNumber == 16)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 17)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, -2f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 18 || characterAnimation.bodyTypeNumber == 19 || characterAnimation.bodyTypeNumber == 20)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -2f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 21)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, 1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, 1f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 22 || characterAnimation.bodyTypeNumber == 23 || characterAnimation.bodyTypeNumber == 24)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, 1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 1f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 25)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, -1f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 26 || characterAnimation.bodyTypeNumber == 27 || characterAnimation.bodyTypeNumber == 28)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -1f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 29)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, -1f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 30 || characterAnimation.bodyTypeNumber == 31 || characterAnimation.bodyTypeNumber == 32)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -2f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, -1f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 33)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(2f, 0f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-2f, 0f, 0);
            }
        }
        
        else if(characterAnimation.bodyTypeNumber == 34 || characterAnimation.bodyTypeNumber == 35 || characterAnimation.bodyTypeNumber == 36)
        {
            if(characterAnimation.currentAnimationDirection == characterAnimation.rightDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftDownAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.rightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.leftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(0f, -1f, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upRightAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(1f, 0, 0);
            }

            else if(characterAnimation.currentAnimationDirection == characterAnimation.upLeftAnim)
            {
                bike.transform.localPosition = initialBikeTransformPosition + new Vector3(-1f, 0, 0);
            }
        }
        
        
   
    }

}
