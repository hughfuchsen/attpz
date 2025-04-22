using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButtonPress : MonoBehaviour
{
    public List<GameObject> treeObjects = new List<GameObject>(); // List for the whole object
    public List<GameObject> vValueOscillatorObjects = new List<GameObject>(); // List for the whole object
    private List<GameObject> sprites = new List<GameObject>(); // Separate list for singular sprites
    private List<GameObject> vSprites = new List<GameObject>(); // Separate list for singular sprites
    private List<Color> initialVValue = new List<Color>();
    private float vValueAmplitude = 0.01f; // The amplitude of the sine wave
    private float vValueFrequency = 10f; // The frequency of the sine wave
    private float time = 0f; // The current time for the sine wave animation
    private bool spacebarReleased = true; // Flag to track if spacebar was released

    public bool isOutside = false;

    [SerializeField] GameObject Player;

    CharacterMovement myCharacterMovement;
    CharacterAnimation myCharacterAnimation;

    // public List<GameObject> colliders = new List<GameObject>();


    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        myCharacterMovement = Player.GetComponent<CharacterMovement>(); 
        myCharacterAnimation = Player.GetComponent<CharacterAnimation>(); 

        for (int i = 0; i < treeObjects.Count; i++)
        {
            FilterObjects(treeObjects[i], sprites);
        } 

        for (int i = 0; i < vValueOscillatorObjects.Count; i++)
        {
            FilterObjects(vValueOscillatorObjects[i], vSprites);
        }  

        for (int i = 0; i < sprites.Count; i++)
        {
            SetAlpha(sprites[i], 0);
        }  
    
        // Populate the initialVValue list with default colors
        for (int i = 0; i < vSprites.Count; i++)
        {   
            initialVValue.Add(vSprites[i].GetComponent<SpriteRenderer>().color);
        }  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        time = 1f;  // Reset time when entering the trigger

        // if(Input.GetKey(KeyCode.Space))
        // {
        //     spacebarReleased = false; // Spacebar is being held down
        // }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!isOutside)
        {
            if (other.CompareTag("PlayerCollider") && !myCharacterMovement.playerIsOutside)
            { 
                if(!myCharacterMovement.playerOnThresh)
                {
                    OscillateVValue();
                
                    if (Input.GetKey(KeyCode.Space) && myCharacterMovement.change == Vector3.zero)
                    {
                        for (int i = 0; i < sprites.Count; i++)
                        {
                            SetAlpha(sprites[i], 1);
                        }

                        for (int i = 0; i < myCharacterAnimation.characterSpriteList.Count; i++)
                        {            
                            myCharacterAnimation.SetAlpha(myCharacterAnimation.characterSpriteList[i], 0.15f);
                        }

                        spacebarReleased = false; // Spacebar is being held down
                    }
                    else if (!Input.GetKey(KeyCode.Space) && !spacebarReleased) // Check if spacebar was released
                    {
                        for (int i = 0; i < sprites.Count; i++)
                        {
                            SetAlpha(sprites[i], 0); // Reset alpha value to 0
                        }
                        for (int i = 0; i < myCharacterAnimation.characterSpriteList.Count; i++)
                        {            
                            myCharacterAnimation.SetAlpha(myCharacterAnimation.characterSpriteList[i], myCharacterAnimation.initialChrctrColorList[i].a);
                        }

                        spacebarReleased = true; // Spacebar is released
                    }
                }
                else if(myCharacterMovement.playerOnThresh)
                {
                    for (int i = 0; i < vSprites.Count; i++)
                    {
                        SetColor(vSprites[i], initialVValue[i]);
                    }  
                }
            }
        }
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if(!isOutside)
        {
            if (other.CompareTag("PlayerCollider") && !myCharacterMovement.playerIsOutside)
            {
                for (int i = 0; i < sprites.Count; i++)
                {
                    SetAlpha(sprites[i], 0);
                }
                
                for (int i = 0; i < vSprites.Count; i++)
                {
                    SetColor(vSprites[i], initialVValue[i]);
                }  

                for (int i = 0; i < myCharacterAnimation.characterSpriteList.Count; i++)
                {            
                    myCharacterAnimation.SetAlpha(myCharacterAnimation.characterSpriteList[i], myCharacterAnimation.initialChrctrColorList[i].a);
                }
            }
        }
    }


    private void FilterObjects(GameObject obj, List<GameObject> spriteList)
    {
        Stack<GameObject> stack = new Stack<GameObject>();

        stack.Push(obj);

        while (stack.Count > 0)
        {
            GameObject current = stack.Pop();
            Transform currentTransform = current.transform;

            if (current.GetComponent<SpriteRenderer>() != null)
            {
                spriteList.Add(current);
            }

            int childCount = currentTransform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject child = currentTransform.GetChild(i).gameObject;
                
                if (child.GetComponent<SpriteRenderer>() != null)
                {
                    spriteList.Add(child);
                }
                
                stack.Push(child);
            }   
        }
    }

    private void SetAlpha(GameObject treeNode, float alpha) 
    {
        SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }  

    private void OscillateVValue()
    {
        float vValue = Mathf.Sin(time * vValueFrequency) * vValueAmplitude;

        for (int i = 0; i < vSprites.Count; i++)
        {
            SetVValue(vSprites[i], vValue);
        }

        time += Time.deltaTime;    
    }  

    private void SetVValue(GameObject treeNode, float vValue) 
    {
        SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();

        Color.RGBToHSV(sr.color, out float h, out float s, out float v);
        Color newColor = Color.HSVToRGB(h, s, v + vValue);

        sr.color = newColor;
    }     
    private void SetColor(GameObject treeNode, Color color) 
    {
        SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();

        sr.color = color;
    }    
}

