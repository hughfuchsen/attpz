using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    private float perspectiveAngle = Mathf.Atan(0.5f);
    public List<LevelScript> levelsAbove = new List<LevelScript>();
    public List<LevelScript> levelsBelow = new List<LevelScript>();
    private List<Transform> childColliders = new List<Transform>(); // Separate list for child colliders
    public int roomWidthX = 30;
    public int wallHeight = 31;
    public float displaceSpeed = 100;
    public float fadeSpeed = 100f;
    private float saturationAmount = 0.2f;
    public bool oppositeMovement;
    public bool groundFloor;
    private bool displaced;
    private Vector3 initialPosition;
    private List<Vector3> childColliderInitialPositions = new List<Vector3>();
    private List<GameObject> spriteList = new List<GameObject>();
    private List<Color> initialColorList = new List<Color>();
    private Coroutine currentMotionCoroutine;
    private Coroutine currentColliderMotionCoroutine;



    void Start()
    {
        GetSprites(this.gameObject, spriteList);
        
        for (int i = 0; i < spriteList.Count; i++)
        {
            SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();

            if(spriteList != null)
            {
                Color initialColor = sr.color;
                initialColorList.Add(initialColor);
            }

        }

        initialPosition = this.gameObject.transform.position;

        FindColliderObjects(transform);
    }

    private void FindColliderObjects(Transform transform)
    {
        Stack<Transform> stack = new Stack<Transform>();
        stack.Push(transform);

        while (stack.Count > 0)
        {
            Transform current = stack.Pop();

            foreach (Transform child in current)
            {
                if (child.CompareTag("Collider") || child.CompareTag("Trigger"))
                {
                    childColliders.Add(child);
                    childColliderInitialPositions.Add(child.position); // Store the initial local position
                }
                stack.Push(child);
            }
        }
    }

    public void EnterLevel(bool shouldMoveDown)
    {
            this.MoveIn(false, null);
            if(levelsAbove != null)
            {
                for (int i = 0; i < levelsAbove.Count; i++)
                {           
                    levelsAbove[i].MoveOut(false, 1f);
                }
            }
            if(levelsBelow != null)
            {      
                if(shouldMoveDown)
                {      
                    for (int i = 0; i < levelsBelow.Count; i++)
                    {           
                        levelsBelow[i].MoveDown(true, 1f);
                    }
                }
            }
    }
    
    // public void ExitLevel()
    // {
    //     if(!groundFloor)
    //     {
    //         this.MoveOut();       
    //     }
    //     else
    //     // else
    //     // {
    //     //     this.MoveIn();
    //     //     for (int i = 0; i < levelsAbove.Count; i++)
    //     //     {           
    //     //         levelsAbove[i].MoveOut();
    //     //     }        
    //     //     for (int i = 0; i < levelsBelow.Count; i++)
    //     //     {           
    //     //         levelsBelow[i].MoveOut();
    //     //     }        
    //     // }    
    // }

    public void ExitBuilding() //move levels to initial positions
    {

        if(levelsAbove != null)
        {
            for (int i = 0; i < levelsAbove.Count; i++)
            {           
                levelsAbove[i].MoveUp();
                levelsAbove[i].MoveIn(false, null);
            }
        }
        if(levelsBelow != null)
        {            
            for (int i = 0; i < levelsBelow.Count; i++)
            {           
                levelsBelow[i].MoveUp();
                levelsBelow[i].MoveIn(false, null);            
            }
        }

        this.MoveUp();
        this.MoveIn(false, null);
    }
    public void MoveIn(bool shouldWait, float? waitTime)
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        if (waitTime.HasValue) // Check if waitTime has a value
        {
            currentMotionCoroutine = StartCoroutine(DisplaceAndDeSaturate(shouldWait, waitTime.Value, false, this.gameObject, initialPosition));
        }
        else
        {
            currentMotionCoroutine = StartCoroutine(DisplaceAndDeSaturate(shouldWait, 0f, false, this.gameObject, initialPosition)); // Default to 0 if waitTime is not specified
        }
    }

    public void MoveOut(bool shouldWait, float? waitTime)
    {
        if (oppositeMovement)
        {
            perspectiveAngle = Mathf.Atan(-0.5f);
            roomWidthX = Mathf.Abs(roomWidthX) * -1;        
        }

        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        if (waitTime.HasValue) // Check if waitTime has a value
        {
            currentMotionCoroutine = StartCoroutine(DisplaceAndDeSaturate(shouldWait, waitTime.Value, true, this.gameObject, initialPosition + new Vector3(roomWidthX, roomWidthX/Mathf.Cos(perspectiveAngle)*Mathf.Sin(perspectiveAngle), 0)));
        }
        else
        {
            currentMotionCoroutine = StartCoroutine(DisplaceAndDeSaturate(shouldWait, 0f, true, this.gameObject, initialPosition + new Vector3(roomWidthX, roomWidthX/Mathf.Cos(perspectiveAngle)*Mathf.Sin(perspectiveAngle), 0))); // Default to 0 if waitTime is not specified
        }
    }


    public void MoveUp()
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        currentMotionCoroutine = StartCoroutine(DisplaceAndDeSaturate(false, null, false, this.gameObject, initialPosition));
    }
    
    public void MoveDown(bool shouldWait, float? waitTime)
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }
        if (waitTime.HasValue) // Check if waitTime has a value
        {
            currentMotionCoroutine = StartCoroutine(DisplaceAndDeSaturate(shouldWait, waitTime.Value, true, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0)));
        }
        else
        {
            currentMotionCoroutine = StartCoroutine(DisplaceAndDeSaturate(false, 0f, true, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0)));
        }
        // Move the parent object down
    }

    private IEnumerator DisplaceAndDeSaturate(bool shouldWait, float? waitTime, bool movingOut, GameObject obj, Vector3 targetPosition)
    {
        Vector3 currentPosition = obj.transform.position;

        float displaceDistace = (currentPosition - targetPosition).magnitude;

        float timeToReachTarget = 0.3f;

        float elapsedTime = 0f;

        if(shouldWait)
        {
            yield return new WaitForSeconds(waitTime.Value);
        }

        while (elapsedTime < timeToReachTarget)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / timeToReachTarget);

            // Rearranged LERP:
            // displacements = target - initial
            // initial + displacements * t
            // initial + (target - initial) * t
            // initial + target * t - initial * t
            // initial * (1 - t) + target * t
            obj.transform.position = currentPosition * (1 - t) + targetPosition * t;

            for (int i = 0; i < childColliders.Count; i++)
            {
                childColliders[i].position = childColliderInitialPositions[i];
            }

            yield return null;
        }

        obj.transform.position = targetPosition; // Ensure the object reaches the exact target position

        for (int i = 0; i < childColliders.Count; i++)
        {
            childColliders[i].position = childColliderInitialPositions[i]; // Ensure the object reaches the exact target position
        }
        if(movingOut && displaceDistace != 0)
        {
            SetSaturationAndValue(spriteList);

            for (int i = 0; i < spriteList.Count; i++)
            {
                if (spriteList[i].CompareTag("OpenDoor") || spriteList[i].CompareTag("AlphaZeroEntExt"))
                {
                    SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        Color newColor = sr.color; // Get the current color
                        newColor.a = 0; // Set the alpha component

                        sr.color = newColor; // Assign the modified color
                    }
                }

            }     
            displaced = true;       
            yield return null;
        }

        if(!movingOut)
        {
            SetInitialColor(spriteList);  

            for (int i = 0; i < spriteList.Count; i++)
            {
                if (spriteList[i].CompareTag("OpenDoor") || spriteList[i].CompareTag("AlphaZeroEntExt"))
                {
                    SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        Color newColor = sr.color; // Get the current color
                        newColor.a = 0; // Set the alpha component

                        sr.color = newColor; // Assign the modified color
                    }
                }

            }   
            displaced = false;
            yield return null;
        }
    }

    private IEnumerator Fade(SpriteRenderer sr, float fadeTo)
    {
        for (float t = 0.0f; t < 1; t += Time.deltaTime) 
        {
            float currentAlpha = Mathf.Lerp(sr.color.a, fadeTo, t * fadeSpeed);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, fadeTo);
            yield return null;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, fadeTo);
    }

    // IEnumerator TreeFade(GameObject obj) 
    // {
    //     // for (float t = 0.0f; t < 1; t += Time.deltaTime) 
    //     // {
    //         // float currentAlpha = Mathf.Lerp(fadeFrom, fadeTo, t * fadeSpeed);
    //         SetTreeSaturation(obj);
    //         yield return null;
    //     // }
    // }

    // void SetTreeSaturation(List<GameObject> spriteList)
    // {
    //     if (treeNode == null)
    //     {
    //         Debug.Log("return3d");
    //         return;
    //     }

    //     SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();

    //     if (sr != null)
    //     {
    //         Color color = sr.color;

    //         // Convert the color to HSB
    //         Color.RGBToHSV(color, out float h, out float s, out float v);

    //         s -= 0.2f;

    //         // Convert back to RGB
    //         color = Color.HSVToRGB(h, s, v);

    //         // Apply the modified color back to the SpriteRenderer
    //         sr.color = color;
    //     }

    //     foreach (Transform child in treeNode.transform)
    //     {
    //         SetTreeSaturation(child.gameObject, saturationAmount);
    //     }
    // }
    void SetSaturationAndValue(List<GameObject> spriteList)
    {
        // Iterate through the list of GameObjects
        for (int i = 0; i < spriteList.Count; i++)
        {
            SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();

            // Get the current color
            Color color = sr.color;

            // Convert the current color to HSV
            Color.RGBToHSV(color, out float h, out float s, out float v);

            // Check if saturation and value haven't been decreased yet
            h = 60f/360f;
            if (s > s/1.5f)
            {
                s -= s/1.5f;
            }

            if (v > v/1.5f)
            {
                v -= v/1.5f;
            }
    

            // Convert back to RGB
            color = Color.HSVToRGB(h, s, v);

            // Apply the modified color back to the SpriteRenderer
            sr.color = color;
        }
    }

    // void SetAlpha(List<GameObject> spriteList)
    // {
    //     for (int i = 0; i < spriteList.Count; i++)
    //     {
    //         SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();

    //         Color color = sr.color;

    //         sr.color
    // }
    void SetInitialColor(List<GameObject> spriteList)
    {
        for (int i = 0; i < spriteList.Count; i++)
        {
            SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = initialColorList[i];
            }
        }
    }

    private GameObject FindParentWithTag(GameObject gameObject, string name)
        {
            Transform parent = gameObject.transform.parent;

            while (parent != null)
            {
                if (parent.CompareTag(name))
                {
                    return parent.gameObject;
                }

                parent = parent.parent;
            }

            return null;
        }


    private void GetSprites(GameObject root, List<GameObject> spriteList)
    {
        if(root != null)
        {
            Stack<GameObject> stack = new Stack<GameObject>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                GameObject currentNode = stack.Pop();
                SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

                if (sr != null)
                {
                    spriteList.Add(currentNode);
                }

                foreach (Transform child in currentNode.transform)
                {
                    stack.Push(child.gameObject);
                }
            }
        }
    }
}


