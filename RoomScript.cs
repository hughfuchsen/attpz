using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public SurfaceType roomSurface = SurfaceType.Carpet; // Default value 
    public BuildingScript building;
    public LevelScript level = null;
    [HideInInspector] public Vector3 levelOffset = Vector3.zero;
    [HideInInspector] public CameraMovement cameraMovement;
    [HideInInspector] public CharacterMovement myCM;
    [HideInInspector] public bool roomIsMoving = false;
     public bool isDown = false;
    public List<RoomScript> roomsSameOrAbove = new List<RoomScript>();
    public List<RoomScript> roomsBelow = new List<RoomScript>();
    private List<GameObject> roomSpriteList = new List<GameObject>();
    private List<Color> initialColorList = new List<Color>();
    private List<Color> desaturatedColorList = new List<Color>();
    // private List<Color> initialColorListToBeChangedWithLevelMovements = new List<Color>();
    private List<Transform> childColliders = new List<Transform>(); // Separate list for child colliders
    private List<Vector3> childColliderInitialPositions = new List<Vector3>();

    public List<RoomThresholdColliderScript> doorsBelow = new List<RoomThresholdColliderScript>();
    private List<Coroutine> doorsBelowCoros = new List<Coroutine>();
    public int wallHeight = 31;
    private Vector3 initialPosition;
    private Coroutine currentMotionCoroutine;
    private Coroutine currentColorCoroutine;
    private Coroutine currentColliderMotionCoroutine;
    public List<GameObject> npcListForRoom = new List<GameObject>();
    [HideInInspector] public List<GameObject> npcSpriteListForRoom = new List<GameObject>();
    [HideInInspector] public List<Color> npcColorListForRoom = new List<Color>();

    [HideInInspector] public Coroutine npcRoomMoveCoro;

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
        myCM = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();


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
        for (int i = 0; i < npcListForRoom.Count; i++)
        {
            // StartCoroutine(HandleNPCsInRooms(npcListForRoom[i]));
        }

        cameraMovement.currentRoom = this;
        myCM.currentRoom = this;
    }
    public void NpcEnterRoom(GameObject character)
    {
        var NPCca = character.GetComponent<CharacterAnimation>();
        var NPCcm = character.GetComponent<CharacterMovement>();
        var npcIsoSS = character.GetComponent<IsoSpriteSorting>();

        
        // if NPC was already in another room
        if (NPCcm.currentRoom != null)
        {
            NPCcm.previousRoom = NPCcm.currentRoom;
            bool previousRoomWasDown = NPCcm.previousRoom.isDown;
            // Remove from old room
            NPCcm.previousRoom.npcListForRoom.Remove(character);
            // Assign to this room
            NPCcm.currentRoom = this;
            npcListForRoom.Add(character);

            
            
            // Assign level if entering building
            if(NPCcm.currentBuilding == null)
            {
                if(building != null)
                {
                    NPCcm.currentBuilding = this.building;
                    NPCcm.currentBuilding.NpcEnterExitBuilding(character, true);
                }
            }

            if(level != null)
            {
                if(level.isDisplacedAngular)
                {
                    levelOffset = level.angularEquation;
                }
                else if (level.isDisplacedVertical)
                {
                    levelOffset = new Vector3(0, -level.wallHeight, 0);
                }
                else
                    {levelOffset = Vector3.zero;}

            }

            // Adjust sprite & sorting offsets if room vertical state changed
            for (int i = 0; i < NPCca.characterSpriteList.Count; i++)
            {
                Vector3 basePos = NPCca.initialChrctrSpriteTransformList[i] + levelOffset;

                if (!isDown && previousRoomWasDown)
                {
                    // Moved up
                    NPCca.characterSpriteList[i].transform.localPosition = basePos;
                }
                else if (isDown && !previousRoomWasDown)
                {
                    // Moved down
                    NPCca.characterSpriteList[i].transform.localPosition = basePos + new Vector3(0, -wallHeight, 0);
                }
                else if (isDown && previousRoomWasDown)
                {
                    // Same vertical level — keep original
                    NPCca.characterSpriteList[i].transform.localPosition = basePos + new Vector3(0, -wallHeight, 0);
                }
                else 
                {
                    // Same vertical level — keep original
                    NPCca.characterSpriteList[i].transform.localPosition = basePos;
                }
            }

            Vector3 baseOffset = new Vector3(8, -28, 0) + levelOffset;

            if (!isDown && previousRoomWasDown)
            {
                // Moved up
                // if(!level.isDisplacedAngular && level.isDisplacedVertical)
                //     baseOffset += new Vector3(0, wallHeight, 0);
            }
            else if (isDown && !previousRoomWasDown)
            {
                // Moved down
                baseOffset -= new Vector3(0, wallHeight, 0);
            }
            else if (isDown && previousRoomWasDown)
            {
                // Moved down
                baseOffset -= new Vector3(0, wallHeight, 0);
            }

            npcIsoSS.SorterPositionOffset = baseOffset;


            // Assign level if entering building
            if(NPCcm.previousLevel != level)
            {
                if(level != null)
                {
                    // Debug.Log("entering");
                    level.NpcEnterLevel(character, this);
                }
            }
        }
        else
        {
            // First room entry (no previous room)
            NPCcm.currentRoom = this;
            npcListForRoom.Add(character);

            // Assign level if entering building
            if(NPCcm.previousLevel != level)
            {
                
                if(level != null)
                {
                    if(level.isDisplacedAngular)
                    {
                        levelOffset = level.angularEquation;
                    }
                    else if (level.isDisplacedVertical)
                    {
                        levelOffset = new Vector3(0, -level.wallHeight, 0);
                    }
                    else
                        {levelOffset = Vector3.zero;}

                    level.NpcEnterLevel(character, this);
                }

                // Adjust sprite & sorting offsets if room vertical state changed
                for (int i = 0; i < NPCca.characterSpriteList.Count; i++)
                {
                    Vector3 basePos = NPCca.initialChrctrSpriteTransformList[i] + levelOffset;

                    if (!isDown)
                    {
                        // Moved up
                        NPCca.characterSpriteList[i].transform.localPosition = basePos;
                    }
                    else if (isDown)
                    {
                        // Moved down
                        NPCca.characterSpriteList[i].transform.localPosition = basePos + new Vector3(0, -wallHeight, 0);
                    }
                    else
                    {
                        // Same vertical level — keep original
                        NPCca.characterSpriteList[i].transform.localPosition = basePos;
                    }
                }

            }

            // Assign building
            if(NPCcm.currentBuilding == null)
            {
                if(building != null)
                {
                    NPCcm.currentBuilding = this.building;
                    NPCcm.currentBuilding.NpcEnterExitBuilding(character, true);
                }
            }



            // Adjust sprite & sorting offsets if room vertical state changed
            for (int i = 0; i < NPCca.characterSpriteList.Count; i++)
            {
                Vector3 basePos = NPCca.initialChrctrSpriteTransformList[i] + levelOffset;

                if (isDown)
                {
                    // Moved down
                    NPCca.characterSpriteList[i].transform.localPosition = basePos + new Vector3(0, -wallHeight, 0);
                }
                else
                {
                    // Same vertical level — keep original
                    NPCca.characterSpriteList[i].transform.localPosition = basePos;
                }
            }

            Vector3 baseOffset = new Vector3(8, -28, 0) + levelOffset;

            if (isDown)
            {
                // Moved down
                baseOffset -= new Vector3(0, wallHeight, 0);
            }

            npcIsoSS.SorterPositionOffset = baseOffset;
        }
        if(NPCcm.currentRoom == myCM.currentRoom)
        {
            HandleDoorFade();
        }
        else if(NPCcm.previousRoom == myCM.currentRoom)
        {
            myCM.currentRoom.HandleDoorFade();
        }


    }
    public void NpcExitRoom(GameObject character)
    {
        var NPCca = character.GetComponent<CharacterAnimation>();
        var NPCcm = character.GetComponent<CharacterMovement>();
        var npcIsoSS = character.GetComponent<IsoSpriteSorting>();

        // if NPC was already in another room
        if (NPCcm.currentRoom != null)
        {
            
            // Assign level if entering building
            if(NPCcm.currentBuilding != null)
            {
                if(building != null)
                {
                    NPCcm.currentBuilding.NpcEnterExitBuilding(character, false);
                }
            }

            // bool previousRoomWasDown = NPCcm.previousRoom.isDown;
            // Remove from old room
            // NPCcm.currentRoom = null;

            // Adjust sprite & sorting offsets if room vertical state changed
            for (int i = 0; i < NPCca.characterSpriteList.Count; i++)
            {
                Vector3 basePos = NPCca.initialChrctrSpriteTransformList[i];
                // Moved up
                if (NPCcm.previousRoom.isDown)
                {
                    // previous room is down down!!! down
                    NPCca.characterSpriteList[i].transform.localPosition = basePos;
                }
                else
                {
                    // Same vertical level — keep original
                    NPCca.characterSpriteList[i].transform.localPosition = basePos;
                }
            }
            npcIsoSS.SorterPositionOffset.y = -28f;

        }
        if(NPCcm.previousRoom == myCM.currentRoom)
        {
            myCM.currentRoom.HandleDoorFade();
        }
    }


    public IEnumerator HandleNPCsInRooms(GameObject character)
    {
        IsoSpriteSorting npcIsoSS = character.GetComponent<IsoSpriteSorting>();
        CharacterAnimation NPCca = character.GetComponent<CharacterAnimation>();
        CharacterMovement NPCcm = character.GetComponent<CharacterMovement>();
      if(NPCcm.currentRoom != null)
      {  
        if(myCM.currentRoom != NPCcm.currentRoom)
        {   
            for (int i = 0; i < NPCca.characterSpriteList.Count; i++)
            {
                NPCca.characterSpriteList[i].transform.localPosition = 
                isDown ? 
                (NPCca.initialChrctrSpriteTransformList[i] + new Vector3(0, -wallHeight, 0)) 
                : NPCca.initialChrctrSpriteTransformList[i];
            }
            npcIsoSS.SorterPositionOffset.y = 
                isDown ? 
                (npcIsoSS.initialSorterPositionOffset1.y - wallHeight) 
                : npcIsoSS.initialSorterPositionOffset1.y;
        }
      }
        yield return null;
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
            FinalizeRoomStateInstantly(); // ensure isDown matches current partial displacement
        }

        currentMotionCoroutine = StartCoroutine(LerpRoomPositions(false, null, this.gameObject, initialPosition, movingUp));
        // isDown = false;
    }
    
    public void MoveDown(bool shouldWait, float? waitTime, bool movingUp = false)
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
            FinalizeRoomStateInstantly();
        }
        // if (currentColorCoroutine != null)
        // {
        //     StopCoroutine(currentColorCoroutine);
        // }

        if (waitTime.HasValue)
        {
            // Move the parent object down
            currentMotionCoroutine = StartCoroutine(LerpRoomPositions(shouldWait, waitTime.Value, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0), movingUp));

        }
        else
        {
            // Handle the case where waitTime is null (optional)
            // Debug.LogError("waitTime is null. You should handle this case appropriately.");
        }
        // isDown = true;
    }


    private void FinalizeRoomStateInstantly()
    {
        // Snap the room to wherever it currently is
        Vector3 pos = transform.localPosition;
        float halfway = initialPosition.y - wallHeight / 2f;

        // Check which side of the halfway point we’re on
        if (pos.y < halfway)
            isDown = true;
        else
            isDown = false;
    }
    // public IEnumerator LerpRoomPositions(bool shouldWait, float? waitTime, GameObject obj, Vector3 targetRoomPosition, bool movingUp)
    // {
    //     roomIsMoving = true;

    //     // Wait if requested
    //     if (shouldWait && waitTime.HasValue)
    //         yield return new WaitForSeconds(waitTime.Value);

    //     // Capture the starting position of the room
    //     Vector3 startRoomPos = obj.transform.localPosition;

    //     float timeToReachTarget = 0.3f;
    //     float elapsedTime = 0f;

    //     bool isMoving = Mathf.Abs(startRoomPos.y - targetRoomPosition.y) > 0.001f;

    //     // Take a snapshot of NPCs to avoid issues if npcListForRoom changes mid-coroutine
    //     var movingNPCs = new List<GameObject>(npcListForRoom);

    //     // Capture current sprite positions and SorterPositionOffsets at start
    //     List<List<Vector3>> npcSpriteStartPositions = new List<List<Vector3>>();
    //     List<float> npcStartOffsets = new List<float>();

    //     foreach (var npc in movingNPCs)
    //     {
    //         var ca = npc.GetComponent<CharacterAnimation>();
    //         var iso = npc.GetComponent<IsoSpriteSorting>();

    //         List<Vector3> spritePositions = new List<Vector3>();
    //         foreach (var sprite in ca.characterSpriteList)
    //             spritePositions.Add(sprite.transform.localPosition);
    //         npcSpriteStartPositions.Add(spritePositions);

    //         npcStartOffsets.Add(iso.SorterPositionOffset.y);
    //     }

    //     // --- Main motion loop ---
    //     while (elapsedTime < timeToReachTarget)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         float t = Mathf.Clamp01(elapsedTime / timeToReachTarget);

    //         // Move the room
    //         obj.transform.localPosition = Vector3.Lerp(startRoomPos, targetRoomPosition, t);

    //         // Keep non-NPC colliders fixed
    //         for (int j = 0; j < childColliders.Count; j++)
    //             childColliders[j].position = childColliderInitialPositions[j];

    //         // Move NPCs
    //         for (int i = 0; i < movingNPCs.Count; i++)
    //         {
    //             var npc = movingNPCs[i];
    //             var ca = npc.GetComponent<CharacterAnimation>();
    //             var cm = npc.GetComponent<CharacterMovement>();
    //             var iso = npc.GetComponent<IsoSpriteSorting>();

    //             for (int k = 0; k < ca.characterSpriteList.Count; k++)
    //             {
    //                 var spriteChild = ca.characterSpriteList[k];
    //                 Vector3 startPos = npcSpriteStartPositions[i][k];
    //                 Vector3 targetPos = startPos;

    //                 if (isMoving)
    //                 {
    //                     if (!isDown && !movingUp)
    //                         targetPos += new Vector3(0, -wallHeight, 0); // move down
    //                     else if (isDown && movingUp)
    //                         targetPos += new Vector3(0, wallHeight, 0);  // move up
    //                 }

    //                 spriteChild.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
    //             }

    //             // Move the sorter offset relative to current value
    //             float startOffset = npcStartOffsets[i];
    //             float targetOffset = startOffset;

    //             if (isMoving)
    //             {
    //                 if (!isDown && !movingUp)
    //                     targetOffset -= wallHeight;
    //                 else if (isDown && movingUp)
    //                     targetOffset += wallHeight;
    //             }

    //             iso.SorterPositionOffset.y = Mathf.Lerp(startOffset, targetOffset, t);

    //             cm.change = Vector3.zero;
    //         }

    //         yield return null;
    //     }

    //     // --- Finalize positions ---
    //     obj.transform.localPosition = targetRoomPosition;

    //     for (int j = 0; j < childColliders.Count; j++)
    //         childColliders[j].position = childColliderInitialPositions[j];

    //     // Update flags
    //     roomIsMoving = false;
    //     isDown = !movingUp;
    // }

    public IEnumerator LerpRoomPositions(bool shouldWait, float? waitTime, GameObject obj, Vector3 targetRoomPosition, bool movingUp)
    {
        roomIsMoving = true;

        // Optional pre-wait
        if (shouldWait && waitTime.HasValue)
            yield return new WaitForSeconds(waitTime.Value);

        // Capture the starting position of the room
        Vector3 startRoomPos = obj.transform.localPosition;
        float timeToReachTarget = 0.3f;
        float elapsedTime = 0f;

        bool isMoving = Mathf.Abs(startRoomPos.y - targetRoomPosition.y) > 0.001f;

        // --- Motion loop ---
        while (elapsedTime < timeToReachTarget)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / timeToReachTarget);

            // Move the room
            obj.transform.localPosition = Vector3.Lerp(startRoomPos, targetRoomPosition, t);

            // Keep non-NPC colliders fixed
            for (int j = 0; j < childColliders.Count; j++)
                childColliders[j].position = childColliderInitialPositions[j];

            // Move NPCs
            for (int i = 0; i < npcListForRoom.Count; i++)
            {
                var npc = npcListForRoom[i];
                var ca = npc.GetComponent<CharacterAnimation>();
                var cm = npc.GetComponent<CharacterMovement>();
                var iso = npc.GetComponent<IsoSpriteSorting>();

                for (int k = 0; k < ca.characterSpriteList.Count; k++)
                {
                    var spriteChild = ca.characterSpriteList[k];

                    // Always start from the true base transform
                    Vector3 startPos = ca.initialChrctrSpriteTransformList[k];
                    Vector3 targetPos = startPos;

                    if (isMoving)
                    {
                        if (isDown && movingUp && !level.isDisplacedAngular && cm.currentLevel != myCM.currentLevel)
                            {targetPos += level.angularEquation; // moving down
                            startPos += new Vector3(0, -wallHeight, 0);}
                        else if (!isDown && !movingUp)
                            targetPos += new Vector3(0, -wallHeight, 0); // moving down
                        else if (isDown && movingUp)
                            startPos += new Vector3(0, -wallHeight, 0); // moving up    

                        spriteChild.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
                    }

                }


                // Sorter offset
                Vector3 startOffset = new Vector3(8f, -28f, 0f);
                Vector3 targetOffset = startOffset;

                if (isMoving)
                {
                    if (isDown && movingUp && !level.isDisplacedAngular && cm.currentLevel != myCM.currentLevel)
                    {
                        targetOffset += new Vector3(level.angularEquation.x, level.angularEquation.y, 0f);
                        startOffset += new Vector3(0f, -wallHeight,  0f); 
                    }
                    else if (!isDown && !movingUp)
                    {
                        targetOffset += new Vector3(0f, -wallHeight, 0f);
                    }
                    else if (isDown && movingUp)
                    {
                        startOffset += new Vector3(0f, -wallHeight, 0f);
                    }

                    // Lerp the offset (Vector3)
                    Vector3 sorterOffset = Vector3.Lerp(startOffset, targetOffset, t);

                    iso.SorterPositionOffset = sorterOffset;
                }


                cm.change = Vector3.zero;
            }

            yield return null;
        }

        // --- FINALIZE ---
        obj.transform.localPosition = targetRoomPosition;

        // Reset colliders
        for (int j = 0; j < childColliders.Count; j++)
            childColliders[j].position = childColliderInitialPositions[j];

        // Finalize NPC positions + sorter offsets
        // for (int i = 0; i < npcListForRoom.Count; i++)
        // {
        //     var npc = npcListForRoom[i];
        //     var ca = npc.GetComponent<CharacterAnimation>();
        //     var iso = npc.GetComponent<IsoSpriteSorting>();

        //     for (int k = 0; k < ca.characterSpriteList.Count; k++)
        //     {
        //         Vector3 finalPos = ca.initialChrctrSpriteTransformList[k];

        //         if (!isDown && !movingUp)
        //             finalPos += new Vector3(0, -wallHeight, 0); // moved down
        //         else if (isDown && movingUp)
        //             finalPos = ca.initialChrctrSpriteTransformList[k]; // back to start

        //         ca.characterSpriteList[k].transform.localPosition = finalPos;
        //     }

        //     float finalOffset = -28f;
        //     if (!isDown && !movingUp)
        //         finalOffset -= wallHeight;
        //     else if (isDown && movingUp)
        //         finalOffset = -28f; // back to normal

        //     Vector3 finalSorterOffset = iso.SorterPositionOffset;
        //     finalSorterOffset.y = finalOffset;
        //     iso.SorterPositionOffset = finalSorterOffset;
        // }

        // --- Update state ---
        roomIsMoving = false;
        isDown = !movingUp;

        // Debug.Log(gameObject.name + " isDown: " + isDown);    
        
    }


    // public IEnumerator LerpRoomPositions(bool shouldWait, float? waitTime, GameObject obj, Vector3 targetRoomPosition, bool movingUp)
    // {
    //     roomIsMoving = true;

    //     if (shouldWait)
    //         yield return new WaitForSeconds(waitTime.Value);

    //     Vector3 currentRoomPosition = obj.transform.localPosition;
    //     float timeToReachTarget = 0.3f;
    //     float elapsedTime = 0f;

    //     bool isMoving = Mathf.Abs(currentRoomPosition.y - targetRoomPosition.y) > 0.001f;


    //     // --- main motion loop ---
    //     while (elapsedTime < timeToReachTarget)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         float t = Mathf.Clamp01(elapsedTime / timeToReachTarget);

    //         // move room
    //         obj.transform.localPosition = Vector3.Lerp(currentRoomPosition, targetRoomPosition, t);

    //         // keep colliders fixed
    //         for (int j = 0; j < childColliders.Count; j++)
    //             childColliders[j].position = childColliderInitialPositions[j];

    //         // move NPCs
    //         for (int i = 0; i < npcListForRoom.Count; i++)
    //         {
    //             var NPCca = npcListForRoom[i].GetComponent<CharacterAnimation>();
    //             var NPCcm = npcListForRoom[i].GetComponent<CharacterMovement>();
    //             var NPCIso = npcListForRoom[i].GetComponent<IsoSpriteSorting>();

    //             for (int k = 0; k < NPCca.characterSpriteList.Count; k++)
    //             {
    //                 var spriteChild = NPCca.characterSpriteList[k];
    //                 Vector3 startPos = NPCca.initialChrctrSpriteTransformList[k].localPosition;
    //                 Vector3 targetPos = startPos;

    //                 if (isMoving && isDown && movingUp)
    //                     startPos += new Vector3(0, -wallHeight, 0);
    //                 else if (isMoving && !isDown && !movingUp)
    //                     targetPos += new Vector3(0, -wallHeight, 0);

    //                 spriteChild.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
    //             }

    //             float startISSOffset = -28f;
    //             float targetOffset = startISSOffset;
                
    //             if(isMoving)
    //             {
    //                 if (!isDown && !movingUp)
    //                 {
    //                     targetOffset -= wallHeight;
    //                     NPCIso.SorterPositionOffset.y = Mathf.Lerp(startISSOffset, targetOffset, t);
    //                 }
    //                 else if (isDown && movingUp)
    //                 {
    //                     startISSOffset -= wallHeight;
    //                     NPCIso.SorterPositionOffset.y = Mathf.Lerp(startISSOffset, targetOffset, t);
    //                 }
    //             }
                    

    //             NPCcm.change = Vector3.zero;
    //         }

    //         yield return null;
    //     }

    //     // finalize
    //     obj.transform.localPosition = targetRoomPosition;
    //     for (int i = 0; i < childColliders.Count; i++)
    //         childColliders[i].position = childColliderInitialPositions[i];

    //     roomIsMoving = false;
    //     isDown = !movingUp;
    // }



    List<Transform> GetAllSpriteChildren(Transform parent)
    {
        List<Transform> spriteChildren = new List<Transform>();

        foreach (Transform child in parent)
        {
            // Skip collider children
            if (child.GetComponent<Collider2D>() != null)
                continue;

            // If child has a SpriteRenderer, include it
            if (child.GetComponent<SpriteRenderer>() != null)
                spriteChildren.Add(child);

            // Recurse into deeper children
            spriteChildren.AddRange(GetAllSpriteChildren(child));
        }

        return spriteChildren;
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


    public void HandleDoorFade()
    {
        for (int i = 0; i < doorsBelow.Count; i++)
        {
            doorsBelow[i].SetPlayerIsInRoomAbove(true);
            doorsBelow[i].SetPlayerIsInDoorway(false);
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

