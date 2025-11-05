using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public SurfaceType roomSurface = SurfaceType.Carpet; // Default value 
    public BuildingScript building;
    [HideInInspector] public CameraMovement cameraMovement;
    public LevelScript level = null;
    public List<RoomScript> roomsSameOrAbove = new List<RoomScript>();
    public List<RoomScript> roomsBelow = new List<RoomScript>();
    private List<GameObject> roomSpriteList = new List<GameObject>();
    private List<Color> initialColorList = new List<Color>();
    private List<Color> desaturatedColorList = new List<Color>();
    // private List<Color> initialColorListToBeChangedWithLevelMovements = new List<Color>();
    private List<Transform> childColliders = new List<Transform>(); // Separate list for child colliders
    public List<RoomThresholdColliderScript> doorsBelow = new List<RoomThresholdColliderScript>();
    private List<Coroutine> doorsBelowCoros = new List<Coroutine>();
    public int wallHeight = 31;
    private Vector3 initialPosition;
    private List<Vector3> childColliderInitialPositions = new List<Vector3>();
    private Coroutine currentMotionCoroutine;
    private Coroutine currentColorCoroutine;
    private Coroutine currentColliderMotionCoroutine;
    public List<GameObject> npcListForRoom = new List<GameObject>();
    [HideInInspector] public List<GameObject> npcSpriteListForRoom = new List<GameObject>();
    [HideInInspector] public List<Color> npcColorListForRoom = new List<Color>();

    void Awake()
    {
        GetSpritesAndAddToLists(this.gameObject, roomSpriteList);
        for (int i = 0; i < roomSpriteList.Count; i++)
        {
            // if(!roomSpriteList[i].CompareTag("ClosedDoor"))
            // {
                SpriteRenderer sr = roomSpriteList[i].GetComponent<SpriteRenderer>();

                Color initialColor = sr.color;
                initialColorList.Add(initialColor);
                // initialColorListToBeChangedWithLevelMovements.Add(initialColor);
            // }
        }
    }

    void Start()
    {
        initialPosition = this.gameObject.transform.localPosition;

        building = FindParentByBuildingScriptComponent();
 
        level = FindParentByLevelScriptComponent();

        FindColliderObjects(transform);

        StartCoroutine(LateStartCoroForNonPlayerCharacters());

        cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();


        AddInitialColorsToDesaturatedColorList();

        wallHeight = wallHeight + 9;
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
                // if (child.CompareTag("Collider") || child.CompareTag("Trigger"))
                if ((child.GetComponent<BoxCollider2D>() != null || child.GetComponent<PolygonCollider2D>() != null)
                    && !child.CompareTag("MovableCollider")
                    )
                {
                    childColliders.Add(child);
                    childColliderInitialPositions.Add(child.position); // Store the initial local position
                }
                stack.Push(child);
            }
        }
    }

    public void EnterRoom(bool shouldWait, float? waitTime)
    {
        for (int i = 0; i < roomsSameOrAbove.Count; i++)
        {
            roomsSameOrAbove[i].MoveUp();
        }

        for (int i = 0; i < roomsBelow.Count; i++)
        {
            if (waitTime.HasValue)
            {
                roomsBelow[i].MoveDown(shouldWait, waitTime.Value);
            }
            else
            {
                roomsBelow[i].MoveDown(false, 0f);
            }        
        }

        for (int i = 0; i < doorsBelow.Count; i++)
        {
            doorsBelow[i].SetPlayerIsInRoomAbove(true);
            doorsBelow[i].SetPlayerIsInDoorway(false);
        }

        cameraMovement.currentRoom = this;
    }
    
    public void ExitRoomAndSetDoorwayInstances() 
    {
        for (int i = 0; i < doorsBelow.Count; i++)
        {
            doorsBelow[i].SetPlayerIsInRoomAbove(false);
            doorsBelow[i].SetPlayerIsInDoorway(false);
        }
        cameraMovement.currentRoom = null;
    }
    
    public void ResetRoomPositions() //move rooms to initial positions
    {
        for (int i = 0; i < roomsSameOrAbove.Count; i++)
        {
            roomsSameOrAbove[i].MoveUp();
        }

        for (int i = 0; i < roomsBelow.Count; i++)
        {
            roomsBelow[i].MoveUp();
        }
    }

    private void MoveUp(bool movingUp = true)
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }
        // if (currentColorCoroutine != null)
        // {
        //     StopCoroutine(currentColorCoroutine);
        // }

        currentMotionCoroutine = StartCoroutine(Displace(false, null, this.gameObject, initialPosition, movingUp));
        // currentColorCoroutine = StartCoroutine(ApplyColorToRoom(false));
    }
    
    public void MoveDown(bool shouldWait, float? waitTime, bool movingUp = false)
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }
        // if (currentColorCoroutine != null)
        // {
        //     StopCoroutine(currentColorCoroutine);
        // }

        if (waitTime.HasValue)
        {
            // Move the parent object down
            currentMotionCoroutine = StartCoroutine(Displace(shouldWait, waitTime.Value, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0), movingUp));
            // currentColorCoroutine = StartCoroutine(ApplyColorToRoom(true));

        }
        else
        {
            // Handle the case where waitTime is null (optional)
            // Debug.LogError("waitTime is null. You should handle this case appropriately.");
        }
    }



    IEnumerator Displace(bool shouldWait, float? waitTime, GameObject obj, Vector3 targetPosition, bool movingUp)
    {
        Vector3 currentPosition = obj.transform.localPosition; 
        float timeToReachTarget = 0.3f;
        float elapsedTime = 0f;

        // Store initial NPC positions and initial collider positions
        List<Vector3> npcInitialPositions = new List<Vector3>();
        List<Vector3> colliderInitialPositions = new List<Vector3>();

        for (int i = 0; i < npcListForRoom.Count; i++)
        {
            npcInitialPositions.Add(npcListForRoom[i].transform.position);
            npcListForRoom[i].transform.parent = this.transform;
            SetZToZero(npcListForRoom[i]);


            // Get the child collider's position and save it
            var collider = npcListForRoom[i].GetComponentInChildren<BoxCollider2D>();
            if (collider != null)
            {
                colliderInitialPositions.Add(collider.transform.position);
            }
        }

        if (shouldWait)
        {
            yield return new WaitForSeconds(waitTime.Value);
        }

        while (elapsedTime < timeToReachTarget)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / timeToReachTarget);

            // Move main object toward target
            obj.transform.localPosition = Vector3.Lerp(currentPosition, targetPosition, t);

            for (int i = 0; i < childColliders.Count; i++)
            {
                childColliders[i].position = childColliderInitialPositions[i];
            }

            // Move each NPC toward its target position
            for (int i = 0; i < npcListForRoom.Count; i++)
            {
                npcListForRoom[i].GetComponent<CharacterMovement>().change = Vector3.zero;

                var collider = npcListForRoom[i].GetComponentInChildren<BoxCollider2D>();
                if (collider != null)
                {
                    collider.transform.position = colliderInitialPositions[i];
                }
            }

            yield return null;
        }

        // Finalize the position of the main object
        obj.transform.localPosition = targetPosition;
    
        for (int i = 0; i < childColliders.Count; i++)
        {
            childColliders[i].position = childColliderInitialPositions[i];
        }

        // Detach NPCs from parent and reset their positions if needed
        for (int i = 0; i < npcListForRoom.Count; i++)
        {
            npcListForRoom[i].transform.parent = null;
            npcInitialPositions[i] = npcListForRoom[i].transform.position;
            SetZToZero(npcListForRoom[i]);

            var collider = npcListForRoom[i].GetComponentInChildren<BoxCollider2D>();
            if (collider != null)
            {
                Vector3 colliderTransform = npcListForRoom[i].GetComponentInChildren<BoxCollider2D>().transform.position;
                if(colliderTransform.y < 5)
                {
                    colliderTransform.y = 0;
                    colliderTransform.x = 0;
                }
                colliderInitialPositions[i] = collider.transform.position;
            }
            // npcListForRoom[i].GetComponent<BoxCollider2D>().offset = npcInitialColliderPos[i]; // Reset offset
        }
    }


    IEnumerator ApplyColorToRoom(bool applyColor)
    {
        if (applyColor == true)
        {
            Desaturate();
        }
        else
        {
            Resaturate();
        }

        yield return null;
    }

    public void Desaturate()
    {
        for (int i = 0; i < roomSpriteList.Count; i++)
        {
            SpriteRenderer sr = roomSpriteList[i].GetComponent<SpriteRenderer>();

            if (roomSpriteList[i].CompareTag("OpenDoor") || roomSpriteList[i].CompareTag("AlphaZeroEntExt"))
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

    public void Resaturate()
    {
        for (int i = 0; i < roomSpriteList.Count; i++)
        {
            SpriteRenderer sr = roomSpriteList[i].GetComponent<SpriteRenderer>();

            if (roomSpriteList[i].CompareTag("OpenDoor") || roomSpriteList[i].CompareTag("AlphaZeroEntExt"))
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

        // Finds the parent GameObject by tag
    public BuildingScript FindParentByBuildingScriptComponent()
    {
        Transform current = transform;

        while (current != null)
        {
            if (current.GetComponent<BuildingScript>() != null)
            {
                return current.GetComponent<BuildingScript>();
            }
            current = current.parent; // Move up to the next parent
        }

        return null; // Return null if no matching parent is found
    }
    public LevelScript FindParentByLevelScriptComponent()
    {
        Transform current = transform;

        while (current != null)
        {
            if (current.GetComponent<LevelScript>() != null)
            {
                return current.GetComponent<LevelScript>();
            }
            current = current.parent; // Move up to the next parent
        }

        return null; // Return null if no matching parent is found
    }

    IEnumerator LateStartCoroForNonPlayerCharacters()
    {
        yield return new WaitForEndOfFrame();

        
        foreach(GameObject obj in npcListForRoom) {
            GetSpritesAndAddToLists(obj, npcSpriteListForRoom);
        }


        // for (int i = 0; i < npcSpriteListForRoom.Count; i++)
        // {  
        //     SetTreeAlpha(npcSpriteListForRoom[i], 0f);
        // } 

    }


    private void GetSpritesAndAddToLists(GameObject obj, List<GameObject> spriteList)
    {
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(obj);

        while (stack.Count > 0)
        {
            GameObject currentNode = stack.Pop();
            SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

            if (sr != null && !currentNode.CompareTag("ClosedDoor"))
            {
                spriteList.Add(currentNode);
            }

            foreach (Transform child in currentNode.transform)
            {
                stack.Push(child.gameObject);
            }
        }
    }

    public void SetZToZero(GameObject obj)
        {
            // Set the object's z position to zero
            Vector3 newPosition = obj.transform.position;
            newPosition.z = 0;
            obj.transform.position = newPosition;

            // Recursively set z position for all children
            foreach (Transform child in obj.transform)
            {
                SetZToZero(child.gameObject);
            }
        }

    void AddInitialColorsToDesaturatedColorList()
        {
            for (int i = 0; i < initialColorList.Count; i++)
            {
                Color desaturatedColor = initialColorList[i];
                Color.RGBToHSV(initialColorList[i], out float h, out float s, out float v);

                
                s -= s / 2f;
                // v -= v / 5f;

                desaturatedColor = Color.HSVToRGB(h, s, v); // Update desaturatedColor with modified HSV values

                desaturatedColorList.Add(desaturatedColor); // Add the modified color to the new list
            }
        }

    // IEnumerator LateStartCoroForNonPlayerCharacters()
    // {
    //     yield return new WaitForEndOfFrame();
        
    //     foreach(GameObject obj in npcListForRoom) {
    //         GetSpritesAndAddToLists(obj, npcSpriteListForRoom, new List<GameObject>(), npcColorListForRoom);
    //     }

    //     for (int i = 0; i < npcSpriteListForRoom.Count; i++)
    //     {  
    //         SetTreeAlpha(npcSpriteListForRoom[i], 0f);
    //     } 

    // }

    // private void GetSpritesAndAddToLists(GameObject obj, List<GameObject> spriteList, List<GameObject> excludeList, List<Color> colorList)
    // {
    //     Stack<GameObject> stack = new Stack<GameObject>();
    //     stack.Push(obj);

    //     while (stack.Count > 0)
    //     {
    //         GameObject currentNode = stack.Pop();
    //         SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

    //         if (sr != null)
    //         {
    //             Color col = sr.color;
    //             spriteList.Add(currentNode);
    //             colorList.Add(col);
    //         }

    //         foreach (Transform child in currentNode.transform)
    //         {
    //             if (!excludeList.Contains(child.gameObject)){
    //                 stack.Push(child.gameObject);
    //             }
    //         }
    //     }
    // }
}

