using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButtonPress : MonoBehaviour
{
    public List<GameObject> treeObjects = new List<GameObject>(); // List for the whole object
    public List<GameObject> vValueOscillatorObjects = new List<GameObject>(); // List for the whole object
    public List<GameObject> sprites = new List<GameObject>(); // Separate list for singular sprites
    public List<GameObject> vSprites = new List<GameObject>(); // Separate list for singular sprites
    private List<Color> initialVValue = new List<Color>();
    private float vValueAmplitude = 0.01f; // The amplitude of the sine wave
    private float vValueFrequency = 10f; // The frequency of the sine wave
    private float time = 0f; // The current time for the sine wave animation
    public List<GameObject> childColliders = new List<GameObject>(); // Separate list for singular child colliders
    private bool spacebarReleased = true; // Flag to track if spacebar was released


    // public List<GameObject> colliders = new List<GameObject>();


    void Start()
    {

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
        
        for (int i = 0; i < childColliders.Count; i++)
        {
            childColliders[i].SetActive(false);
        }      
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OscillateVValue();

            if (Input.GetKey(KeyCode.Space))
            {
                for (int i = 0; i < sprites.Count; i++)
                {
                    SetAlpha(sprites[i], 1);
                }

                for (int i = 0; i < childColliders.Count; i++)
                {
                    childColliders[i].SetActive(true);
                }

                spacebarReleased = false; // Spacebar is being held down
            }
            else if (!Input.GetKey(KeyCode.Space) && !spacebarReleased) // Check if spacebar was released
            {
                for (int i = 0; i < sprites.Count; i++)
                {
                    SetAlpha(sprites[i], 0); // Reset alpha value to 0
                }

                for (int i = 0; i < childColliders.Count; i++)
                {
                    childColliders[i].SetActive(false);
                }

                spacebarReleased = true; // Spacebar is released
            }
        }
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                SetAlpha(sprites[i], 0);
            }

            for (int i = 0; i < childColliders.Count; i++)
            {
                childColliders[i].SetActive(false);
            }
            
            for (int i = 0; i < vSprites.Count; i++)
            {
                SetColor(vSprites[i], initialVValue[i]);
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

            int childCount = currentTransform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject child = currentTransform.GetChild(i).gameObject;

                if (child.CompareTag("Collider"))
                {
                    childColliders.Add(child);
                }
                
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

