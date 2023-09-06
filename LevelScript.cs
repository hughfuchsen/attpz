using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    private float perspectiveAngle = Mathf.Atan(0.5f);
    public List<LevelScript> levelAbove = new List<LevelScript>();
    public List<LevelScript> levelBelow = new List<LevelScript>();
    private List<Transform> childColliders = new List<Transform>(); // Separate list for child colliders
    public int roomWidthX = 30;
    public int wallHeight = 31;
    public float displaceSpeed = 100;
    public float fadeSpeed = 100f;
    private float saturationAmount = 0.2f;
    public bool oppositeMovement;
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
            Color initialColor = spriteList[i].GetComponent<SpriteRenderer>().color;
            initialColorList.Add(initialColor);
        }

        initialPosition = this.gameObject.transform.position;

        FindColliderObjects(transform);

        // if(!oppositeMovement)
        // {
        //     this.MoveOut();
        // }
        // else
        // {
        //     this.MoveIn();
        // }        
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


    public void EnterLevel()
    {
        if(!oppositeMovement)
        {
            this.MoveIn();
        }
        else
        {
            this.MoveOut();
        }
        // for (int i = 0; i < levelAbove.Count; i++)
        // {
        //     levelAbove[i].MoveIn();
        // }

        // for (int i = 0; i < levelBelow.Count; i++)
        // {
        //     levelBelow[i].MoveOut();
        // }
    }
    
    public void ExitLevel()
    {
        if(!oppositeMovement)
        {
            this.MoveOut();
        }
        else
        {
            this.MoveIn();
        }    
    }
    public void ExitBuilding()
    {
        // for (int i = 0; i < levelAbove.Count; i++)
        // {
        //     levelAbove[i].MoveIn();
        // }

        // for (int i = 0; i < levelBelow.Count; i++)
        // {
        //     levelBelow[i].MoveIn();
        // }
    }

    private void MoveIn()
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        currentMotionCoroutine = StartCoroutine(Displace(false, this.gameObject, initialPosition));
    }
    
    private void MoveOut()
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        // Move the parent object down
        currentMotionCoroutine = StartCoroutine(Displace(true, this.gameObject, initialPosition + new Vector3(roomWidthX, roomWidthX/Mathf.Cos(perspectiveAngle)*Mathf.Sin(perspectiveAngle), 0)));//, 1));
    }

    public void MoveUp()
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        currentMotionCoroutine = StartCoroutine(Displace(false, this.gameObject, initialPosition));
    }
    
    public void MoveDown()
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        // Move the parent object down
        currentMotionCoroutine = StartCoroutine(Displace(true, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0)));//, 1));
    }

    private IEnumerator Displace(bool movingOut, GameObject obj, Vector3 targetPosition)
    {
        Vector3 currentPosition = obj.transform.position;

        float distance = (currentPosition - targetPosition).magnitude;

        float timeToReachTarget = distance / displaceSpeed;

        float elapsedTime = 0f;

        
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
        if(movingOut)
        {
            SetSaturation(spriteList);
            
            yield return null;
        }

        if(!movingOut)
        {
            SetInitialColor(spriteList);            
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
    void SetSaturation(List<GameObject> spriteList)
    {
        for (int i = 0; i < spriteList.Count; i++)
        {
            SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();

            Color color = sr.color;

            // Convert the color to HSB
            Color.RGBToHSV(color, out float h, out float s, out float v);

            s -= 0.5f;
            v -= 0.5f;

            // Convert back to RGB
            color = Color.HSVToRGB(h, s, v);

            // Apply the modified color back to the SpriteRenderer
            sr.color = color;
        }
    }
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


