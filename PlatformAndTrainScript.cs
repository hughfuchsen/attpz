using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AreaType
{
    train,
    platform
}
public class PlatformAndTrainScript : MonoBehaviour
{
    public AreaType areaType;

    // public PlatformAndTrainScript previousArea;
    // public PlatformAndTrainScript currentArea;

    private float perspectiveAngle = Mathf.Atan(0.5f);
    private List<Transform> childColliders = new List<Transform>(); // Separate list for child colliders
    public int platformWidthX = 30;
    public int wallHeight = 27;
    public bool oppositeMovement;
    public Vector3 angularEquation;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private List<Vector3> childColliderInitialPositions = new List<Vector3>();
    private List<GameObject> spriteList = new List<GameObject>();    // Start is called before the first frame update


    public List<GameObject> characterListForArea = new List<GameObject>();
    [HideInInspector] public List<GameObject> npcSpriteListForArea = new List<GameObject>();
    [HideInInspector] public List<Color> npcColorListForArea = new List<Color>();
    [HideInInspector] public Coroutine currentMotionCoroutine;






    void Awake()
    {
        GetSprites(this.gameObject, spriteList);

        switch (areaType)
        {
            case AreaType.train:
                areaType = AreaType.train;
            break;

            case AreaType.platform:
                areaType = AreaType.platform;
            break;
        }
    }

    void Start()
    {
        initialPosition = this.gameObject.transform.localPosition;

        if (oppositeMovement)
        {
            perspectiveAngle = Mathf.Atan(-0.5f);
            platformWidthX = Mathf.Abs(platformWidthX) * -1;
            angularEquation = new Vector3(platformWidthX, platformWidthX / Mathf.Cos(perspectiveAngle) * Mathf.Sin(perspectiveAngle) + 1, 0);
        }
        else
            angularEquation = new Vector3(platformWidthX, platformWidthX / Mathf.Cos(perspectiveAngle) * Mathf.Sin(perspectiveAngle) + 1, 0);


        FindColliderObjects(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CharacterEnterArea(GameObject character)
    {
        var ca = character.GetComponent<CharacterAnimation>();
        var cm = character.GetComponent<CharacterMovement>();
        var iss = character.GetComponent<IsoSpriteSorting>();

        // HandleInclineCollisionIgnoring(character);

        // if NPC was already in another room

            // cm.previousArea.characterListForArea.Remove(character);

            characterListForArea.Add(character);
        
    }

    public void MoveDown(bool shouldWait, float? waitTime)
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }
        if (waitTime.HasValue) // Check if waitTime has a value
        {
            currentMotionCoroutine = StartCoroutine(LerpPosition(shouldWait, waitTime.Value, false, true, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0)));
        }
        else
        {
            currentMotionCoroutine = StartCoroutine(LerpPosition(false, 0f, false, true, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0)));
        }
        // Move the parent object down
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
    private void FindColliderObjects(Transform transform)
    {
        Stack<Transform> stack = new Stack<Transform>();
        stack.Push(transform);

        while (stack.Count > 0)
        {
            Transform current = stack.Pop();

            foreach (Transform child in current)
            {
                if (child.GetComponent<BoxCollider2D>() != null)
                {
                    childColliders.Add(child);
                    childColliderInitialPositions.Add(child.position); // Store the initial local position
                }
                stack.Push(child);
            }
        }
    }

    private IEnumerator LerpPosition(bool shouldWait, float? waitTime, bool movingOut, bool movingDown, GameObject obj, Vector3 targetLevelPos)
    {
        
        Vector3 initialPos = obj.transform.localPosition;
        float displaceDistace = (initialPos - targetLevelPos).magnitude;
        float timeToReachTarget = 0.3f;
        float elapsedTime = 0f;

        bool isMoving = Mathf.Abs(initialPos.y - targetLevelPos.y) > 0.001f;


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
            obj.transform.localPosition = initialPos * (1 - t) + targetLevelPos * t;

            for (int i = 0; i < childColliders.Count; i++)
            {
                childColliders[i].position = childColliderInitialPositions[i];
            }

            // Move NPCs
            for (int i = 0; i < characterListForArea.Count; i++)
            {
                var npc = characterListForArea[i];
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
                        targetPos += angularEquation; 

                        spriteChild.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
                    }

                }

                // Sorter offset
                Vector3 startOffset = new Vector3(8, -28, 0);
                Vector3 targetOffset = startOffset;

                if (isMoving)
                {
                    targetOffset += angularEquation;


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
        for (int i = 0; i < characterListForArea.Count; i++)
        {
            var npc = characterListForArea[i];
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
                    targetPos += angularEquation;
                }

                // FIX — snap to final
                // spriteChild.transform.localPosition = targetPos;
            }

            // Finalize sorter offset
            Vector3 startOffset = new Vector3(8, -28, 0);
            Vector3 targetOffset = startOffset;

            if (isMoving)
            {
                targetOffset += angularEquation;

            }

            // iso.SorterPositionOffset = targetOffset; // FIX — snap final sorter offset

            cm.change = Vector3.zero;
        }
        yield return null;
    }
}
