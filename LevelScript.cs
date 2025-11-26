using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum DisplacementType
{
    Initializing,
    Angular,
    Vertical
}

public class LevelScript : MonoBehaviour
{
    public BuildingScript building;
    CharacterMovement myCM;
    [SerializeField] GameObject player;
    private float perspectiveAngle = Mathf.Atan(0.5f);
    [HideInInspector] public CameraMovement cameraMovement;
    public List<LevelScript> levelsAbove = new List<LevelScript>();
    public List<LevelScript> levelsBelow = new List<LevelScript>();
    public List<InclineThresholdColliderScript> inclineEntrances = new List<InclineThresholdColliderScript>();
    private List<Transform> childColliders = new List<Transform>(); // Separate list for child colliders
    public int roomWidthX = 30;
    public int wallHeight = 31;
    public bool oppositeMovement;

    [HideInInspector] public bool isDisplacedVertical = false;
    [HideInInspector] public bool isDisplacedAngular = false;
    public Vector3 angularEquation;
    public bool groundFloor;
    private bool displaced;

    public bool desaturateBoolean = false;
    private Vector3 initialPosition;

    private Vector3 targetLevelPosition;
    private List<Vector3> childColliderInitialPositions = new List<Vector3>();
    private List<GameObject> spriteList = new List<GameObject>();
    private List<Color> initialColorList = new List<Color>();
    private List<Color> desaturatedColorList = new List<Color>();
    private List<Color> initialColorListToBeChangedWithLevelMovements = new List<Color>();
    private Color innerBuildingBackDropColor = new Color();
    private Coroutine currentMotionCoroutine;
    private Coroutine currentColliderMotionCoroutine;

    public List<LevelScript> roomScripts; 

    public List<GameObject> npcListForLevel = new List<GameObject>();
    [HideInInspector] public List<GameObject> npcSpriteListForLevel = new List<GameObject>();
    [HideInInspector] public List<Color> npcColorListForLevel = new List<Color>();



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


        player = GameObject.FindGameObjectWithTag("Player");
        myCM = player.GetComponent<CharacterMovement>();
        wallHeight = 70;
        roomWidthX += 10;
                
    }
    void Start()
    {
        building = FindParentByBuildingScriptComponent();
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

        if (oppositeMovement)
        {
            perspectiveAngle = Mathf.Atan(-0.5f);
            roomWidthX = Mathf.Abs(roomWidthX) * -1;
            angularEquation = new Vector3(roomWidthX, roomWidthX / Mathf.Cos(perspectiveAngle) * Mathf.Sin(perspectiveAngle) + 1, 0);
        }
        else
            angularEquation = new Vector3(roomWidthX, roomWidthX / Mathf.Cos(perspectiveAngle) * Mathf.Sin(perspectiveAngle) + 1, 0);


        FindColliderObjects(transform);

        cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();

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
        cameraMovement.currentLevel = this;
        myCM.currentLevel = this;

        HandleInclineCollisionIgnoring(player);

        this.InitializeLevelPosition(false, null);
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
    // public void npcEnterLevel(GameObject character)
    // {
    //     if(character.GetComponent<CharacterMovement>().currentLevel != null)
    //     {character.GetComponent<CharacterMovement>().currentLevel.npcListForLevel.Remove(character);}
    //     character.GetComponent<CharacterMovement>().currentLevel = null;
    //     character.GetComponent<CharacterMovement>().currentLevel = this;
    //     character.GetComponent<CharacterMovement>().currentLevel.npcListForLevel.Add(character);
    // }

    public void NpcEnterLevel(GameObject character, RoomScript room)
    {
        var NPCca = character.GetComponent<CharacterAnimation>();
        var NPCcm = character.GetComponent<CharacterMovement>();
        var npcIsoSS = character.GetComponent<IsoSpriteSorting>();

        HandleInclineCollisionIgnoring(character);

        // if NPC was already in another room
        if (NPCcm.currentLevel != null)
        {
            //Set variables
            NPCcm.previousLevel = NPCcm.currentLevel;
            NPCcm.currentLevel = this;

            // Remove and assign from old room to new room
            NPCcm.previousLevel.npcListForLevel.Remove(character);
            npcListForLevel.Add(character);

            // if(NPCcm)
            NpcDesaturateOrSaturate();
        }
        else
        {
            NPCcm.currentLevel = this;
            npcListForLevel.Add(character);
            NpcDesaturateOrSaturate();
        }
    }


    public void ResetLevels() //move levels to initial positions
    {
        if (levelsAbove != null)
        {
            for (int i = 0; i < levelsAbove.Count; i++)
            {
                // levelsAbove[i].MoveUp();
                levelsAbove[i].InitializeLevelPosition(false, null);
            }
        }
        if (levelsBelow != null)
        {
            for (int i = 0; i < levelsBelow.Count; i++)
            {
                // levelsBelow[i].MoveUp();
                levelsBelow[i].InitializeLevelPosition(false, null);
            }
        }

        // this.MoveUp();
        this.InitializeLevelPosition(false, null);
    }
    public void InitializeLevelPosition(bool shouldWait, float? waitTime)
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        if (waitTime.HasValue) // Check if waitTime has a value
        {
            currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(shouldWait, waitTime.Value, DisplacementType.Initializing, false, false, this.gameObject, initialPosition, false));
        }
        else
        {
            currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(shouldWait, 0f, DisplacementType.Initializing, false, false, this.gameObject, initialPosition, false)); // Default to 0 if waitTime is not specified
        }
    }

    public void MoveOut(bool shouldWait, float? waitTime)
    {
        // if (oppositeMovement)
        // {
        //     perspectiveAngle = Mathf.Atan(-0.5f);
        //     roomWidthX = Mathf.Abs(roomWidthX) * -1;
        //     angularEquation = new Vector3(roomWidthX, roomWidthX / Mathf.Cos(perspectiveAngle) * Mathf.Sin(perspectiveAngle) + 1, 0);
        // }

        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        if (waitTime.HasValue) // Check if waitTime has a value
        {
            currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(shouldWait, waitTime.Value, DisplacementType.Angular, true, false, this.gameObject, initialPosition + angularEquation, true));
        }
        else
        {
            currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(shouldWait, 0f, DisplacementType.Angular, true, false, this.gameObject, initialPosition + angularEquation, true)); // Default to 0 if waitTime is not specified
        }
    }


    public void MoveDown(bool shouldWait, float? waitTime)
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }
        if (waitTime.HasValue) // Check if waitTime has a value
        {
            currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(shouldWait, waitTime.Value, DisplacementType.Vertical, false, true, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0), true));
        }
        else
        {
            currentMotionCoroutine = StartCoroutine(LerpPosAndSetSaturationValue(false, 0f, DisplacementType.Vertical, false, true, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0), true));
        }
        // Move the parent object down
    }


    private IEnumerator LerpPosAndSetSaturationValue(bool shouldWait, float? waitTime, DisplacementType displacementType, bool movingOut, bool movingDown, GameObject obj, Vector3 targetLevelPos, bool desaturate)
    {
        
        Vector3 startLevelPos = obj.transform.localPosition;
        float displaceDistace = (startLevelPos - targetLevelPos).magnitude;
        float timeToReachTarget = 0.3f;
        float elapsedTime = 0f;

        bool isMoving = Mathf.Abs(startLevelPos.y - targetLevelPos.y) > 0.001f;


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
            obj.transform.localPosition = startLevelPos * (1 - t) + targetLevelPos * t;

            for (int i = 0; i < childColliders.Count; i++)
            {
                childColliders[i].position = childColliderInitialPositions[i];
            }

            // Move NPCs
            for (int i = 0; i < npcListForLevel.Count; i++)
            {
                var npc = npcListForLevel[i];
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
                        switch (displacementType)
                        {
                            case DisplacementType.Angular:
                                if (!isDisplacedAngular && movingOut)
                                    targetPos += angularEquation;
                                // else if (isDisplacedAngular && !movingOut)
                                //     startPos += angularEquation;
                                break;

                            case DisplacementType.Vertical:
                                if (!isDisplacedVertical && movingDown)
                                    targetPos += new Vector3(0, -wallHeight, 0); // moving up
                                // else if (!isDisplacedVertical && movingDown)
                                //     targetPos += new Vector3(0, -wallHeight, 0); // moving down
                                break;

                            case DisplacementType.Initializing: // initializing position
                                if (isDisplacedAngular && !movingOut)
                                    startPos += angularEquation;
                                else if (isDisplacedVertical && !movingDown)
                                    startPos += new Vector3(0, -wallHeight, 0); 
                                break;
                        }

                        spriteChild.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
                    }

                }

                // Sorter offset
                Vector3 startOffset = new Vector3(8, -28, 0);
                Vector3 targetOffset = startOffset;

                if (isMoving)
                {
                    switch (displacementType)
                    {
                        case DisplacementType.Angular:
                            if (!isDisplacedAngular && movingOut)
                                targetOffset += angularEquation;
                            break;

                        case DisplacementType.Vertical:
                            if (!isDisplacedVertical && movingDown)
                                targetOffset += new Vector3(0, -wallHeight, 0); // moving up
                            break;

                        case DisplacementType.Initializing: // initializing position
                            if (isDisplacedAngular && !movingOut)
                                startOffset += angularEquation;
                            else if (isDisplacedVertical && !movingDown)
                                startOffset += new Vector3(0, -wallHeight, 0); 
                            break;
                    }

                    iso.SorterPositionOffset = Vector3.Lerp(startOffset, targetOffset, t);               
                }
                cm.change = Vector3.zero;
            }

            yield return null;
        }


        // ---FINALIZE----
        obj.transform.localPosition = targetLevelPos; // Ensure the object reaches the exact target position

        for (int i = 0; i < childColliders.Count; i++)
        {
            childColliders[i].position = childColliderInitialPositions[i]; // Ensure the object reaches the exact target position
        }



        // --- FINALIZE NPC POSITIONS AND SORTER OFFSETS ---
        for (int i = 0; i < npcListForLevel.Count; i++)
        {
            var npc = npcListForLevel[i];
            var ca = npc.GetComponent<CharacterAnimation>();
            var cm = npc.GetComponent<CharacterMovement>();
            var iso = npc.GetComponent<IsoSpriteSorting>();

            // Finalize sprite positions
            for (int k = 0; k < ca.characterSpriteList.Count; k++)
            {
                var spriteChild = ca.characterSpriteList[k];

                // Always use true initial base
                Vector3 startPos = ca.initialChrctrSpriteTransformList[k];
                Vector3 targetPos = startPos;

                if (isMoving)
                {
                    switch (displacementType)
                    {
                        case DisplacementType.Angular:
                            if (!isDisplacedAngular && movingOut)
                                targetPos += angularEquation;
                            break;

                        case DisplacementType.Vertical:
                            if (!isDisplacedVertical && movingDown)
                                targetPos += new Vector3(0, -wallHeight, 0);
                            break;

                        case DisplacementType.Initializing:
                            if (isDisplacedAngular && !movingOut)
                                startPos += angularEquation;
                            else if (isDisplacedVertical && !movingDown)
                                startPos += new Vector3(0, -wallHeight, 0);
                            break;
                    }
                }

                // FIX — snap to final
                // spriteChild.transform.localPosition = targetPos;
            }

            // Finalize sorter offset
            Vector3 startOffset = new Vector3(8, -28, 0);
            Vector3 targetOffset = startOffset;

            if (isMoving)
            {
                switch (displacementType)
                {
                    case DisplacementType.Angular:
                        if (!isDisplacedAngular && movingOut)
                            targetOffset += angularEquation;
                        break;

                    case DisplacementType.Vertical:
                        if (!isDisplacedVertical && movingDown)
                            targetOffset += new Vector3(0, -wallHeight, 0);
                        break;

                    case DisplacementType.Initializing:
                        if (isDisplacedAngular && !movingOut)
                            startOffset += angularEquation;
                        else if (isDisplacedVertical && !movingDown)
                            startOffset += new Vector3(0, -wallHeight, 0);
                        break;
                }
            }

            // iso.SorterPositionOffset = targetOffset; // FIX — snap final sorter offset

            cm.change = Vector3.zero;
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

        // --- STATE MANAGEMENT ---
        // movingOut means we’re displacing outward (true -> becomes displaced)
        // movingDown means we’re returning upward (false -> no longer displaced)
        switch (displacementType)
        {
            case DisplacementType.Angular:
                isDisplacedAngular = movingOut;
                isDisplacedVertical = false;
                break;

            case DisplacementType.Vertical:
                isDisplacedVertical = movingDown;
                isDisplacedAngular = false;
                break;

            default:
                isDisplacedAngular = false;
                isDisplacedVertical = false;
                break;
        }
    }

    private void Resaturate()
    {
        // --- Resaturate regular scene sprites ---
        for (int i = 0; i < spriteList.Count; i++)
        {
            SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            if (spriteList[i].CompareTag("OpenDoor") || spriteList[i].CompareTag("AlphaZeroEntExt"))
            {
                Color newColor = sr.color;
                newColor.a = 0; // stay invisible
                sr.color = newColor;
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

        if(myCM.currentBuilding == this.building)
        {
            // --- Resaturate NPC sprites ---
            for (int i = 0; i < npcListForLevel.Count; i++)
            {
                var npc = npcListForLevel[i];
                var ca = npc.GetComponent<CharacterAnimation>();
                if (ca == null) continue;

                for (int j = 0; j < ca.characterSpriteList.Count; j++)
                {
                    var spriteChild = ca.characterSpriteList[j];
                    var initialColor = ca.initialChrctrColorList[j];
                    SpriteRenderer sr = spriteChild.GetComponent<SpriteRenderer>();
                    if (sr == null) continue;

                    
                    Color newColor = sr.color;
                    newColor = initialColor; // still hidden
                    sr.color = newColor;
                }
            }
        }
            
        desaturateBoolean = false;
    }
   

    private void Desaturate()
    {
        // --- Desaturate regular scene sprites ---
        for (int i = 0; i < spriteList.Count; i++)
        {
            SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();

            if (spriteList[i].CompareTag("OpenDoor") || spriteList[i].CompareTag("AlphaZeroEntExt"))
            {
                Color newColor = sr.color;
                newColor.a = 0; // make fully transparent
                sr.color = newColor;
            }
            else
            {
                sr.color = desaturatedColorList[i]; // assign stored desaturateBoolean color
            }
        }

        // --- Desaturate NPC sprites ---
        for (int i = 0; i < npcListForLevel.Count; i++)
        {
            var npc = npcListForLevel[i];
            var ca = npc.GetComponent<CharacterAnimation>();
            if (ca == null) continue;

            for (int j = 0; j < ca.characterSpriteList.Count; j++)
            {
                var spriteChild = ca.characterSpriteList[j];
                var originalColor = ca.initialChrctrColorList[j];
                SpriteRenderer sr = spriteChild.GetComponent<SpriteRenderer>();
                if (sr == null) continue;

                if (sr.color.a != 0)
                {
                    // Convert RGB → HSV
                    Color.RGBToHSV(originalColor, out float h, out float s, out float v);

                    // Halve saturation and brightness
                    s *= 0.5f;
                    v *= 0.5f;

                    // Convert back to RGB (preserve alpha)
                    Color desatColor = Color.HSVToRGB(h, s, v);
                    desatColor.a = sr.color.a;

                    sr.color = desatColor;
                }
            }
        }

        desaturateBoolean = true;
    }


    private void NpcDesaturateOrSaturate()
    {
        // --- Desaturate NPC sprites ---
        for (int i = 0; i < npcListForLevel.Count; i++)
        {
            var npc = npcListForLevel[i];
            var ca = npc.GetComponent<CharacterAnimation>();
            var cm = npc.GetComponent<CharacterMovement>();
            if (ca == null) continue;

            if(desaturateBoolean)
            {
                for (int j = 0; j < ca.characterSpriteList.Count; j++)
                {
                    var spriteChild = ca.characterSpriteList[j];
                    var originalColor = ca.initialChrctrColorList[j];
                    SpriteRenderer sr = spriteChild.GetComponent<SpriteRenderer>();
                    if (sr == null) continue;

                    if (sr.color.a != 0)
                    {
                        // Convert RGB → HSV
                        Color.RGBToHSV(originalColor, out float h, out float s, out float v);

                        // Halve saturation and brightness
                        s *= 0.5f;
                        v *= 0.5f;

                        // Convert back to RGB (preserve alpha)
                        Color desatColor = Color.HSVToRGB(h, s, v);
                        desatColor.a = sr.color.a;

                        sr.color = desatColor;
                    }
                }
            }
            else
            {
                for (int j = 0; j < ca.characterSpriteList.Count; j++)
                {
                    var spriteChild = ca.characterSpriteList[j];
                    var initialColor = ca.initialChrctrColorList[j];
                    SpriteRenderer sr = spriteChild.GetComponent<SpriteRenderer>();
                    if (sr == null) continue;

                    // if (sr.color.a != 0)
                    // {
                        Color newColor = sr.color;
                        newColor = initialColor; // still hidden
                        if(myCM.currentBuilding == cm.currentBuilding)
                            {sr.color = newColor;}
                    // }
                }
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

    public void HandleInclineCollisionIgnoring(GameObject character)
    {
        // Get the NPC's collider
        BoxCollider2D characterCol = character.GetComponentInChildren<BoxCollider2D>(true);

        if (characterCol == null)
        {
            Debug.LogWarning("Character has no Collider2D!");
            return;
        }

        // foreach(LevelScript level in levelsAbove)
        // {
        //     foreach(InclineThresholdColliderScript inclineEntrance in level.inclineEntrances)
        //     {
        //         BoxCollider2D inclineCol = inclineEntrance.gameObject.GetComponent<BoxCollider2D>();
        //         // ignore collision only if this incline is NOT in this script’s list
        //         if (!inclineEntrances.Contains(inclineEntrance))
        //         {
        //             Physics2D.IgnoreCollision(characterCol, inclineCol, true);
        //         }
        //         else
        //         {
        //             Physics2D.IgnoreCollision(characterCol, inclineCol, false);
        //         }                
        //     }
        // }
        // foreach(LevelScript level in levelsBelow)
        // {
        //     foreach(InclineThresholdColliderScript inclineEntrance in level.inclineEntrances)
        //     {
        //         BoxCollider2D inclineCol = inclineEntrance.gameObject.GetComponent<BoxCollider2D>();
        //         // ignore collision only if this incline is NOT in this script’s list
        //         if (!inclineEntrances.Contains(inclineEntrance))
        //         {
        //             Physics2D.IgnoreCollision(characterCol, inclineCol, true);
        //         }
        //         else
        //         {
        //             Physics2D.IgnoreCollision(characterCol, inclineCol, false);
        //         }    
        //     }
        // }
        
        // Loop through all incline thresholds
        foreach (InclineThresholdColliderScript incThresh in inclineEntrances)
        {
            BoxCollider2D inclineCol = incThresh.GetComponent<BoxCollider2D>();
            if (inclineCol == null) 
                continue;

            Physics2D.IgnoreCollision(characterCol, inclineCol, false);
            Debug.Log("GONG");
        }
    }

    public void IgnoreSiblingInclineMoevementScripts
    (InclineThresholdColliderScript incThresh, BoxCollider2D characterCol, bool ignoreCollision)
    {
        foreach (Transform sibling in incThresh.transform.parent) // or transform if same level
        {
            if (sibling.CompareTag("Trigger"))
            {
                BoxCollider2D siblingCollider = sibling.GetComponent<BoxCollider2D>();
                if (siblingCollider != null)
                {
                    Physics2D.IgnoreCollision(characterCol, siblingCollider, ignoreCollision);
                }
            }
        }
    }

}

