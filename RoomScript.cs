using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public BuildingScript building;
    public LevelScript level = null;
    public List<RoomScript> roomsSameOrAbove = new List<RoomScript>();
    public List<RoomScript> roomsBelow = new List<RoomScript>();
    private List<Transform> childColliders = new List<Transform>(); // Separate list for child colliders
    public List<RoomThresholdColliderScript> doorsBelow = new List<RoomThresholdColliderScript>();
    private List<Coroutine> doorsBelowCoros = new List<Coroutine>();
    public int wallHeight = 30;
    private Vector3 initialPosition;
    private List<Vector3> childColliderInitialPositions = new List<Vector3>();
    private Coroutine currentMotionCoroutine;
    private Coroutine currentColliderMotionCoroutine;

    public List<GameObject> npcList = new List<GameObject>();
    [HideInInspector] public List<GameObject> npcSpriteList = new List<GameObject>();
    [HideInInspector] public List<Color> npcColorList = new List<Color>();



    void Start()
    {
        initialPosition = this.gameObject.transform.localPosition;

        building = FindParentByBuildingScriptComponent();
 
        level = FindParentByLevelScriptComponent();

        FindColliderObjects(transform);

        StartCoroutine(LateStartCoroForNonPlayerCharacters());

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
                if (child.GetComponent<BoxCollider2D>() != null || child.GetComponent<PolygonCollider2D>() != null)
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
    }
    
    public void ExitRoomAndSetDoorwayInstances() 
    {
        for (int i = 0; i < doorsBelow.Count; i++)
        {
            doorsBelow[i].SetPlayerIsInRoomAbove(false);
            doorsBelow[i].SetPlayerIsInDoorway(false);
        }
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

        currentMotionCoroutine = StartCoroutine(Displace(false, null, this.gameObject, initialPosition, movingUp));
    }
    
    public void MoveDown(bool shouldWait, float? waitTime, bool movingUp = false)
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        if (waitTime.HasValue)
        {
            // Move the parent object down
            currentMotionCoroutine = StartCoroutine(Displace(shouldWait, waitTime.Value, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0), movingUp));
        }
        else
        {
            // Handle the case where waitTime is null (optional)
            // Debug.LogError("waitTime is null. You should handle this case appropriately.");
        }
    }


    // IEnumerator Displace(bool shouldWait, float? waitTime, GameObject obj, Vector3 targetPosition)
    // {
    //     Vector3 currentPosition = obj.transform.localPosition;

    //     float distance = (currentPosition - targetPosition).magnitude;

    //     float timeToReachTarget = 0.3f;

    //     float elapsedTime = 0f;

    //     List<Vector2> npcInitialColliderPos = new List<Vector2>();

    //     for (int i = 0; i < building.npcList.Count; i++)
    //     {

    //         npcInitialColliderPos.Add(building.npcList[i].GetComponent<BoxCollider2D>().offset);
    //         // npcInitialColliderPos[i] = building.npcList[i].GetComponent<BoxCollider2D>().offset;
    //     }


    //     if(shouldWait)
    //     {
    //         yield return new WaitForSeconds(waitTime.Value);
    //     }
        
    //     while (elapsedTime < timeToReachTarget)
    //     {
    //         elapsedTime += Time.deltaTime;

    //         float t = Mathf.Clamp01(elapsedTime / timeToReachTarget);

    //         // Rearranged LERP:
    //         // displacements = target - initial
    //         // initial + displacements * t
    //         // initial + (target - initial) * t
    //         // initial + target * t - initial * t
    //         // initial * (1 - t) + target * t
    //         obj.transform.localPosition = currentPosition * (1 - t) + targetPosition * t;

    //         for (int i = 0; i < childColliders.Count; i++)
    //         {
    //             childColliders[i].position = childColliderInitialPositions[i];
    //         }

    //         for (int i = 0; i < building.npcList.Count; i++)
    //         {
    //             // building.npcList[i].GetComponent<CharacterMovement>().change = Vector3.zero;
    //             building.npcList[i].GetComponent<BoxCollider2D>().offset = npcInitialColliderPos[i];
    //         }


    //         yield return null;
    //     }

    //     obj.transform.localPosition = targetPosition; // Ensure the object reaches the exact target position

    //     for (int i = 0; i < childColliders.Count; i++)
    //     {
    //         childColliders[i].position = childColliderInitialPositions[i]; // Ensure the object reaches the exact target position
    //     }
    // }

    // IEnumerator Displace(bool shouldWait, float? waitTime, GameObject obj, Vector3 targetPosition, bool movingUp)
    // {

    //     Vector3 currentPosition = obj.transform.localPosition;
    //     float timeToReachTarget = 0.3f;
    //     float elapsedTime = 0f;

    //     // Populate npcInitialColliderPos list and save initial and target positions for each NPC
    //     List<Vector2> npcInitialColliderPos = new List<Vector2>();
    //     List<Vector3> npcInitialPositions = new List<Vector3>();
    //     List<Vector3> npcTargetPositions = new List<Vector3>();

    //     Vector3 npcTargetPosition;

    //     for (int i = 0; i < npcList.Count; i++)
    //     {            
    //         npcInitialColliderPos.Add(npcList[i].GetComponent<BoxCollider2D>().offset);
    //         npcList[i].transform.parent = this.transform;
    //     }

    //     for (int i = 0; i < npcSpriteList.Count; i++)
    //     {      
    //         Vector3 npcInitialPosition = npcSpriteList[i].transform.position;
    //         npcInitialPositions.Add(npcInitialPosition);

    //     }

    //     if (shouldWait)
    //     {
    //         yield return new WaitForSeconds(waitTime.Value);
    //     }

    //     while (elapsedTime < timeToReachTarget)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         float t = Mathf.Clamp01(elapsedTime / timeToReachTarget);

    //         // Move main object toward target
    //         obj.transform.localPosition = currentPosition * (1 - t) + targetPosition * t;

    //         // Move each NPC from its initial position to its target position


            

    //         for (int i = 0; i < childColliders.Count; i++)
    //         {
    //             childColliders[i].position = childColliderInitialPositions[i];
    //         }

 

    //         yield return null;
    //     }

    //     for (int i = 0; i < npcList.Count; i++)
    //     {
    //         npcList[i].transform.parent = null;
    //     }

    //     obj.transform.localPosition = targetPosition;

    //     for (int i = 0; i < npcList.Count; i++)
    //     {
    //         npcList[i].GetComponent<BoxCollider2D>().offset = npcInitialColliderPos[i];
    //     }

    //     for (int i = 0; i < childColliders.Count; i++)
    //     {
    //         childColliders[i].position = childColliderInitialPositions[i];
    //     }
    // }


   

    // IEnumerator Displace(bool shouldWait, float? waitTime, GameObject obj, Vector3 targetPosition, bool movingUp)
    // {
    //     Vector3 currentPosition = obj.transform.localPosition;
    //     float timeToReachTarget = 0.3f;
    //     float elapsedTime = 0f;

    //     // Save initial collider offsets and positions for each NPC
    //     List<Vector3> npcInitialCollider = new List<Vector2>();
    //     List<Vector3> npcInitialPositions = new List<Vector3>();

    //     for (int i = 0; i < npcList.Count; i++)
    //     {            
    //         npcInitialColliderPos.Add(npcList[i].GetComponentInChildren<BoxCollider2D>().transform.position);
    //         npcInitialPositions.Add(npcList[i].transform.position);  // Store initial world position
    //         npcList[i].transform.parent = this.transform;
    //     }

    //     if (shouldWait)
    //     {
    //         yield return new WaitForSeconds(waitTime.Value);
    //     }

    //     while (elapsedTime < timeToReachTarget)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         float t = Mathf.Clamp01(elapsedTime / timeToReachTarget);

    //         // Move main object toward target
    //         obj.transform.localPosition = Vector3.Lerp(currentPosition, targetPosition, t);

    //         for (int i = 0; i < childColliders.Count; i++)
    //         {
    //             childColliders[i].position = childColliderInitialPositions[i];
    //         }

    //         // Adjust each NPC's collider offset to simulate a fixed collider position
    //         for (int i = 0; i < npcList.Count; i++)
    //         {
    //             Vector3 npcColliderObj = npcList[i].GetComponentInChildren<BoxCollider2D>();

    //             npcColliderObj.offset = npcInitialColliderPos[i] + (Vector2)displacement;

    //             npcList[i].GetComponent<CharacterMovement>().change = Vector3.zero;
    //         }

    //         yield return null;
    //     }

    //     // Detach NPCs from parent and reset their positions if needed
    //     for (int i = 0; i < npcList.Count; i++)
    //     {
    //         npcList[i].transform.parent = null;
    //         npcInitialPositions[i] = npcList[i].transform.position;
    //         // npcList[i].GetComponent<BoxCollider2D>().offset = npcInitialColliderPos[i]; // Reset offset
    //     }

    //     obj.transform.localPosition = targetPosition;

    //     for (int i = 0; i < childColliders.Count; i++)
    //     {
    //         childColliders[i].position = childColliderInitialPositions[i];
    //     }
    // }



    IEnumerator Displace(bool shouldWait, float? waitTime, GameObject obj, Vector3 targetPosition, bool movingUp)
    {
        Vector3 currentPosition = obj.transform.localPosition;
        float timeToReachTarget = 0.3f;
        float elapsedTime = 0f;

        // Store initial NPC positions and initial collider positions
        List<Vector3> npcInitialPositions = new List<Vector3>();
        List<Vector3> colliderInitialPositions = new List<Vector3>();

        for (int i = 0; i < npcList.Count; i++)
        {
            npcInitialPositions.Add(npcList[i].transform.position);
            npcList[i].transform.parent = this.transform;
            SetZToZero(npcList[i]);


            // Get the child collider's position and save it
            var collider = npcList[i].GetComponentInChildren<BoxCollider2D>();
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
            for (int i = 0; i < npcList.Count; i++)
            {
                npcList[i].GetComponent<CharacterMovement>().change = Vector3.zero;

                var collider = npcList[i].GetComponentInChildren<BoxCollider2D>();
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
        for (int i = 0; i < npcList.Count; i++)
        {
            npcList[i].transform.parent = null;
            npcInitialPositions[i] = npcList[i].transform.position;
            SetZToZero(npcList[i]);

            var collider = npcList[i].GetComponentInChildren<BoxCollider2D>();
            if (collider != null)
            {
                Vector3 colliderTransform = npcList[i].GetComponentInChildren<BoxCollider2D>().transform.position;
                if(colliderTransform.y < 5)
                {
                    colliderTransform.y = 0;
                    colliderTransform.x = 0;
                }
                colliderInitialPositions[i] = collider.transform.position;
            }
            // npcList[i].GetComponent<BoxCollider2D>().offset = npcInitialColliderPos[i]; // Reset offset
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

        
        foreach(GameObject obj in npcList) {
            GetSpritesAndAddToLists(obj, npcSpriteList);
        }


        // for (int i = 0; i < npcSpriteList.Count; i++)
        // {  
        //     SetTreeAlpha(npcSpriteList[i], 0f);
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

    // IEnumerator LateStartCoroForNonPlayerCharacters()
    // {
    //     yield return new WaitForEndOfFrame();
        
    //     foreach(GameObject obj in npcList) {
    //         GetSpritesAndAddToLists(obj, npcSpriteList, new List<GameObject>(), npcColorList);
    //     }

    //     for (int i = 0; i < npcSpriteList.Count; i++)
    //     {  
    //         SetTreeAlpha(npcSpriteList[i], 0f);
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

