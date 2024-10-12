using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    private float perspectiveAngle = Mathf.Atan(0.5f);
    public List<LevelScript> levelsAbove = new List<LevelScript>();
    public List<LevelScript> levelsBelow = new List<LevelScript>();
    private List<Transform> childColliders = new List<Transform>(); // Separate list for child colliders
    public int roomWidthX = 30;
    public int wallHeight = 31;
    public bool oppositeMovement;
    public bool groundFloor;
    private bool displaced;
    private Vector3 initialPosition;
    private List<Vector3> childColliderInitialPositions = new List<Vector3>();
    private List<GameObject> spriteList = new List<GameObject>();
    private List<Color> initialColorList = new List<Color>();
    private List<Color> desaturatedColorList = new List<Color>();
    private List<Color> initialColorListToBeChangedWithLevelMovements = new List<Color>();
    private Color innerBuildingBackDropColor = new Color();
    private Coroutine currentMotionCoroutine;
    private Coroutine currentColliderMotionCoroutine;


    void Awake()
    {
        GetSprites(this.gameObject, spriteList);
        for (int i = 0; i < spriteList.Count; i++)
        {
            SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();

            Color initialColor = sr.color;
            initialColorList.Add(initialColor);
            initialColorListToBeChangedWithLevelMovements.Add(initialColor);
        }


        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>();
                
    }
    void Start()
    {
        innerBuildingBackDropColor = GameObject.FindGameObjectWithTag("InnerBuildingBackdrop").GetComponent<SpriteRenderer>().color;
        Color.RGBToHSV(innerBuildingBackDropColor, out float bdh, out float bds, out float bdv);
        
        for (int i = 0; i < initialColorList.Count; i++)
        {
            Color desaturatedColor = initialColorList[i];
            Color.RGBToHSV(initialColorList[i], out float h, out float s, out float v);

            h = bdh;
            s -= s / 1.5f;
            v -= v / 1.5f;

            desaturatedColor = Color.HSVToRGB(h, s, v); // Update desaturatedColor with modified HSV values

            desaturatedColorList.Add(desaturatedColor); // Add the modified color to the new list
        }

        initialPosition = this.gameObject.transform.localPosition;

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

    public void EnterLevel(bool shouldWait, bool shouldMoveDown)
    {
        this.MoveIn(false, null);
        if (levelsAbove != null)
        {
            for (int i = 0; i < levelsAbove.Count; i++)
            {
                levelsAbove[i].MoveOut(shouldWait, 0.3f);
            }
        }
        if (levelsBelow != null)
        {
            if (shouldMoveDown)
            {
                for (int i = 0; i < levelsBelow.Count; i++)
                {
                    levelsBelow[i].MoveDown(shouldWait, 1f);
                }
            }
        }
    }

    public void ResetLevels() //move levels to initial positions
    {

        if (levelsAbove != null)
        {
            for (int i = 0; i < levelsAbove.Count; i++)
            {
                levelsAbove[i].MoveUp();
                levelsAbove[i].MoveIn(false, null);
            }
        }
        if (levelsBelow != null)
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
            currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(shouldWait, waitTime.Value, false, this.gameObject, initialPosition, false));
        }
        else
        {
            currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(shouldWait, 0f, false, this.gameObject, initialPosition, false)); // Default to 0 if waitTime is not specified
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
            currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(shouldWait, waitTime.Value, true, this.gameObject, initialPosition + new Vector3(roomWidthX, roomWidthX / Mathf.Cos(perspectiveAngle) * Mathf.Sin(perspectiveAngle) + 1, 0), true));
        }
        else
        {
            currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(shouldWait, 0f, true, this.gameObject, initialPosition + new Vector3(roomWidthX, roomWidthX / Mathf.Cos(perspectiveAngle) * Mathf.Sin(perspectiveAngle) + 1, 0), true)); // Default to 0 if waitTime is not specified
        }
    }


    public void MoveUp()
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(false, null, false, this.gameObject, initialPosition, false));
    }

    public void MoveDown(bool shouldWait, float? waitTime)
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }
        if (waitTime.HasValue) // Check if waitTime has a value
        {
            currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(shouldWait, waitTime.Value, true, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0), true));
        }
        else
        {
            currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(false, 0f, true, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0), true));
        }
        // Move the parent object down
    }


    private IEnumerator LerpPosAndSetSaturationValue(bool shouldWait, float? waitTime, bool movingOut, GameObject obj, Vector3 targetPosition, bool desaturate)
    {
        Vector3 currentPosition = obj.transform.localPosition;
        float displaceDistace = (currentPosition - targetPosition).magnitude;
        float timeToReachTarget = 0.3f;
        float elapsedTime = 0f;
        if (shouldWait)
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
            obj.transform.localPosition = currentPosition * (1 - t) + targetPosition * t;

            for (int i = 0; i < childColliders.Count; i++)
            {
                childColliders[i].position = childColliderInitialPositions[i];
            }
            yield return null;
        }

        obj.transform.localPosition = targetPosition; // Ensure the object reaches the exact target position

        for (int i = 0; i < childColliders.Count; i++)
        {
            childColliders[i].position = childColliderInitialPositions[i]; // Ensure the object reaches the exact target position
        }

        if (desaturate == true)
        {
            Desaturate();
        }
        else
        {
            Resaturate();
        }
        yield return null;
    }

    private void Resaturate()
    {
        for (int i = 0; i < spriteList.Count; i++)
        {
            SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();

            if (spriteList[i].CompareTag("OpenDoor") || spriteList[i].CompareTag("AlphaZeroEntExt"))
            {
                if (sr != null)
                {
                    Color newColor = sr.color; // Get the current color
                    newColor.a = 0; // Set the alpha component
                    sr.color = newColor; // Assign the modified color
                }
            }
            else if (sr.color.a == 0f)
            {
                sr.color = new Color(initialColorList[i].r, initialColorList[i].g, initialColorList[i].b, 0f);
            }
            else
            {
                sr.color = initialColorList[i];
            }
        }
    }

    private void Desaturate()
    {
        for (int i = 0; i < spriteList.Count; i++)
        {
            SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();

            if (spriteList[i].CompareTag("OpenDoor") || spriteList[i].CompareTag("AlphaZeroEntExt"))
            {
                Color newColor = sr.color; // Get the current color
                newColor.a = 0; // Set the alpha component
                sr.color = newColor; // Assign the modified color
            }
            else
            {
                sr.color = desaturatedColorList[i]; // Assign the previously calculated desaturated color
            }
        }
    }

    void SetSaturationAndValue()
    {
        // Iterate through the list of GameObjects
        for (int i = 0; i < spriteList.Count; i++)
        {
            SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();

            // Get the current color
            Color color = initialColorListToBeChangedWithLevelMovements[i];

            // Convert the current color to HSV
            Color.RGBToHSV(color, out float h, out float s, out float v);

            // Check if saturation and value haven't been decreased yet
            h = 44f / 360f;
            if (s > s / 1.5f)
            {
                s -= s / 1.5f;
            }

            if (v > v / 1.5f)
            {
                v -= v / 1.5f;
            }


            // Convert back to RGB
            color = Color.HSVToRGB(h, s, v);

            // Apply the modified color back to the SpriteRenderer
            sr.color = color;
        }
    }    
    void SetTheInitialColorToBeChangedWithLevelMovements(List<GameObject> spriteList, List<Color> initialColorList, List<Color> initialColorListToBeChangedWithLevelMovements)
    {
        for (int i = 0; i < spriteList.Count; i++)
        {
            SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                // float h, s, v;
                // Color.RGBToHSV(sr.color, out h, out s, out v);
                Color initialColor = initialColorList[i];
                // Color.RGBToHSV(initialColor, out float x, out float y, out float z);

                // Check if sprite color is transparent or differs from initial color in HSV space
                if (sr.color != initialColor)
                {
                    sr.color = initialColorListToBeChangedWithLevelMovements[i];
                }
                else if (sr.color.a == 0f)
                {
                    sr.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
                }
            }
        }
    }

    void SetInitialColorToBeChangedWithLevelMovements()
    {
        for (int i = 0; i < spriteList.Count; i++)
        {
            SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();
            if (sr.color.a != 0)
            {
                sr.color = initialColorListToBeChangedWithLevelMovements[i];
            }
            else if (sr.color.a == 0f)
            {
                sr.color = new Color(initialColorList[i].r, initialColorList[i].g, initialColorList[i].b, 0f);
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
        if (root != null)
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


