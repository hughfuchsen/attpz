using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject Player;
    public Transform target;

    public float smoothing = 1000f;

    public bool freezeCamPos = false;

    private Vector3 targetPosition;

    private float[] zoomLevels = { 50f, 100f, 150f, 200f, 220f }; // Array of zoom levels
    private int currentZoomLevelIndex = 2; // Keep track of the current zoom level
    private float zoomSize; // The current zoom size

    private bool isZoomingIn = true;

    public RoomScript currentRoom;
    public LevelScript currentLevel;
    public BuildingScript currentBuilding;

    [SerializeField] GameObject innerBuildingBackdrop;
    [SerializeField] GameObject activateCharacterUI;
    private ActivateCharacterUITrigger activateCharacterUITriggerScript;

    public Texture2D cursorTexture; // Assign your PNG in the Inspector
    public Vector2 hotSpot = Vector2.zero; // Hotspot of the cursor (can be adjusted)

    [HideInInspector] string initialMotionDirection = null;

    void Awake()
    {
        innerBuildingBackdrop.SetActive(true);
        currentRoom = null;
        currentLevel = null;
        currentBuilding = null;
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        zoomSize = zoomLevels[currentZoomLevelIndex]; // Set initial zoom size
        GetComponent<Camera>().orthographicSize = zoomSize;
        targetPosition = transform.localPosition;

        Player = GameObject.FindGameObjectWithTag("Player");
        activateCharacterUITriggerScript = activateCharacterUI.GetComponent<ActivateCharacterUITrigger>();

        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);


    }

    void Update()
    {
        // Check if Y button is pressed
        if (Input.GetKeyDown(KeyCode.JoystickButton3)) // Y button on Xbox controller
        {
            IncrementZoom();
        }
        
            if (Input.GetKeyDown(KeyCode.G)) 
            {
                if(!Player.GetComponent<CharacterMovement>().playerIsOutside)
                {
                    if(currentLevel != null)
                    {
                        currentLevel.ResetLevels();
                    }
                    if(currentRoom != null)
                    {
                        currentRoom.ResetRoomPositions();
                    }
                }
                    initialMotionDirection = Player.GetComponent<CharacterMovement>().motionDirection;
                    Player.GetComponent<CharacterMovement>().motionDirection = "none";
            }

            if (Input.GetKeyUp(KeyCode.G)) 
            {
                if(!Player.GetComponent<CharacterMovement>().playerIsOutside)
                {
                    if(currentLevel != null)
                    {
                        currentLevel.EnterLevel(false, true);
                    }
                    if(currentRoom != null)
                    {
                        currentRoom.EnterRoom(false, 0.3f);
                    }
                }
                Player.GetComponent<CharacterMovement>().motionDirection = initialMotionDirection;
            }
        
    }

     void FixedUpdate()
    {
        // Handle zooming with mouse scroll wheel
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");

        if (zoomDelta != 0)
        {
            // Adjust zoomSize by a fixed amount
            zoomSize = Mathf.Clamp(zoomSize - zoomDelta * 100, 50, 220);
            GetComponent<Camera>().orthographicSize = zoomSize;
        }
        // transform.position = target.transform.position + new Vector3(8,-14,0);
    }


    void IncrementZoom()
    {
        if (isZoomingIn)
        {
            currentZoomLevelIndex++;
            if (currentZoomLevelIndex >= zoomLevels.Length - 1)
            {
                currentZoomLevelIndex = zoomLevels.Length - 1; // Ensure it stays within bounds
                isZoomingIn = false; // Reverse direction when max is reached
            }
        }
        else
        {
            currentZoomLevelIndex--;
            if (currentZoomLevelIndex <= 0)
            {
                currentZoomLevelIndex = 0; // Ensure it stays within bounds
                isZoomingIn = true; // Reverse direction when min is reached
            }
        }
        
        zoomSize = zoomLevels[currentZoomLevelIndex];
        GetComponent<Camera>().orthographicSize = zoomSize;
    }


    void LateUpdate() // handle the nature of the camera following player
    {
        targetPosition = new Vector3(target.position.x + 7, target.position.y - 12, transform.position.z);
        if (!freezeCamPos)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        }
    }


    void FixZCoordinates() // new
    {
        Transform[] allTransforms = UnityEngine.Object.FindObjectsOfType<Transform>();

        foreach (Transform t in allTransforms)
        {
            // Skip if object is tagged "MainCamera"
            if (t.CompareTag("MainCamera"))
                continue;

            Vector3 pos = t.position;
            pos.z = 0f;
            t.position = pos;
        }
    }

    // public IEnumerator AdjustSmoothing(float final)
    // {

    //     float elapsedTime = 0f;
    //     float duration = 1f;

    //     if(smoothing != final)
    //     {
    //         while (elapsedTime < duration)
    //         {
    //             elapsedTime += Time.deltaTime;
    //             smoothing = Mathf.Lerp(initialSmoothing, final, elapsedTime / duration);
    //             yield return null;
    //         }
    //         smoothing = final;
    //     }

    // }

    // public IEnumerator AdjustSmoothing(float final)
    // {

    //     float elapsedTime = 0f;
    //     float duration = 4f;

    //     while (elapsedTime < duration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         smoothing = Mathf.Lerp(smoothing, final, elapsedTime / duration);
    //         yield return null;
    //     }

    //     smoothing = final;
    // }
}



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CameraMovement : MonoBehaviour
// {
//     [SerializeField] GameObject Player;
//     public Transform target;

//     public float smoothing = 1000f;

//     public bool freezeCamPos = false;

//     private Vector3 targetPosition;

//     private float[] zoomLevels = { 50f, 100f, 150f, 200f, 220f }; // Array of zoom levels
//     private int currentZoomLevelIndex = 2; // Keep track of the current zoom level
//     private float zoomSize; // The current zoom size

//     private bool isZoomingIn = true;

//     public RoomScript currentRoom;
//     public LevelScript currentLevel;
//     public BuildingScript currentBuilding;


//     [SerializeField] GameObject innerBuildingBackdrop;
//     [SerializeField] GameObject activateCharacterUI;
//     private ActivateCharacterUITrigger activateCharacterUITriggerScript;

//     public Texture2D cursorTexture; // Assign your PNG in the Inspector
//     public Vector2 hotSpot = Vector2.zero; // Hotspot of the cursor (can be adjusted)


//     void Awake()
//     {
//         innerBuildingBackdrop.SetActive(true);
//     }

//     void Start()
//     {
//         target = GameObject.FindGameObjectWithTag("Player").transform;
//         zoomSize = zoomLevels[currentZoomLevelIndex]; // Set initial zoom size
//         GetComponent<Camera>().orthographicSize = zoomSize;
//         targetPosition = transform.localPosition;

//         Player = GameObject.FindGameObjectWithTag("Player");
//         activateCharacterUITriggerScript = activateCharacterUI.GetComponent<ActivateCharacterUITrigger>();

//         Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);

//         currentRoom = null;
//         currentLevel = null;
//         currentBuilding = null;
//     }

//     void Update()
//     {
//         // Check if Y button is pressed
//         if (Input.GetKeyDown(KeyCode.JoystickButton3)) // Y button on Xbox controller
//         {
//             IncrementZoom();
//         }
//     }

//      void FixedUpdate()
//     {
//         // Handle zooming with mouse scroll wheel
//         float zoomDelta = Input.GetAxis("Mouse ScrollWheel");

//         if (zoomDelta != 0)
//         {
//             // Adjust zoomSize by a fixed amount
//             zoomSize = Mathf.Clamp(zoomSize - zoomDelta * 100, 50, 220);
//             GetComponent<Camera>().orthographicSize = zoomSize;
//         }
//         // transform.position = target.transform.position + new Vector3(8,-14,0);
//     }


//     void IncrementZoom()
//     {
//         if (isZoomingIn)
//         {
//             currentZoomLevelIndex++;
//             if (currentZoomLevelIndex >= zoomLevels.Length - 1)
//             {
//                 currentZoomLevelIndex = zoomLevels.Length - 1; // Ensure it stays within bounds
//                 isZoomingIn = false; // Reverse direction when max is reached
//             }
//         }
//         else
//         {
//             currentZoomLevelIndex--;
//             if (currentZoomLevelIndex <= 0)
//             {
//                 currentZoomLevelIndex = 0; // Ensure it stays within bounds
//                 isZoomingIn = true; // Reverse direction when min is reached
//             }
//         }
        
//         zoomSize = zoomLevels[currentZoomLevelIndex];
//         GetComponent<Camera>().orthographicSize = zoomSize;
//     }


//     void LateUpdate() // handle the nature of the camera following player
//     {
//         targetPosition = new Vector3(target.position.x + 7, target.position.y - 12, transform.position.z);
//         if (!freezeCamPos)
//         {
//             transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
//         }
//     }

//     public IEnumerator SetSaturation(bool exiting, bool shouldWait, float? waitTime)
//     {
//         if (shouldWait && waitTime.HasValue)
//         {
//             yield return new WaitForSeconds(waitTime.Value);
//         }

//         if (currentLevel != null && !exiting)
//         {
//             for (int i = 0; i < currentLevel.levelsAbove.Count; i++)
//             {
//                 currentLevel.levelsAbove[i].Desaturate();
//             }
//             for (int i = 0; i < currentLevel.levelsBelow.Count; i++)
//             {
//                 currentLevel.levelsBelow[i].Desaturate();
//             }
//             currentLevel.Resaturate();
//         }
//         else if(currentLevel != null && exiting == true)
//         {
//             for (int i = 0; i < currentLevel.levelsAbove.Count; i++)
//             {
//                 currentLevel.levelsAbove[i].Resaturate();
//             }
//             for (int i = 0; i < currentLevel.levelsBelow.Count; i++)
//             {
//                 currentLevel.levelsBelow[i].Resaturate();
//             }
//             currentLevel.Resaturate();

//         }

//         // if (currentRoom != null)
//         // {
//         //     for (int i = 0; i < currentRoom.roomsSameOrAbove.Count; i++)
//         //     {
//         //         currentRoom.roomsSameOrAbove[i].Resaturate();
//         //     }

//         //     for (int i = 0; i < currentRoom.roomsBelow.Count; i++)
//         //     {
//         //         currentRoom.roomsBelow[i].Desaturate();
//         //     }
//         // }
//     }

//     // public void SetSaturation(bool shouldWait, float? waitTime) // can delete
//     // {
//     //     if(shouldWait)
//     //     {
//     //         wait for waitTime
//     //     }

//     //         if(currentLevel != null)
//     //         {
//     //             for (int i = 0; i < currentLevel.levelsAbove.Count; i++)
//     //             {
//     //                 currentLevel.levelsAbove[i].Desaturate();
//     //             }
//     //             for (int i = 0; i < currentLevel.levelsBelow.Count; i++)
//     //             {
//     //                 currentLevel.levelsBelow[i].Desaturate();
//     //             }
//     //             currentLevel.Resaturate();
                
//     //         }

//     //         if(currentRoom != null)
//     //         {
//     //             for (int i = 0; i < currentRoom.roomsSameOrAbove.Count; i++)
//     //             {
//     //                 currentRoom.roomsSameOrAbove[i].Resaturate();
//     //             }

//     //             for (int i = 0; i < currentRoom.roomsBelow.Count; i++)
//     //             {
//     //                 currentRoom.roomsBelow[i].Desaturate();
//     //             }
//     //         }
//     // }


//     // void FixZCoordinates()
//     // {
//     //     GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

//     //     foreach (GameObject obj in allObjects)
//     //     {
//     //         // Check if the object is not the main camera
//     //         if (obj != gameObject)
//     //         {
//     //             // Set the object's position with zero Z
//     //             Vector3 newPosition = obj.transform.position;
//     //             newPosition.z = 0f;
//     //             obj.transform.position = newPosition;
//     //         }
//     //     }
//     // }




//     // public IEnumerator AdjustSmoothing(float final)
//     // {

//     //     float elapsedTime = 0f;
//     //     float duration = 1f;

//     //     if(smoothing != final)
//     //     {
//     //         while (elapsedTime < duration)
//     //         {
//     //             elapsedTime += Time.deltaTime;
//     //             smoothing = Mathf.Lerp(initialSmoothing, final, elapsedTime / duration);
//     //             yield return null;
//     //         }
//     //         smoothing = final;
//     //     }

//     // }

//     // public IEnumerator AdjustSmoothing(float final)
//     // {

//     //     float elapsedTime = 0f;
//     //     float duration = 4f;

//     //     while (elapsedTime < duration)
//     //     {
//     //         elapsedTime += Time.deltaTime;
//     //         smoothing = Mathf.Lerp(smoothing, final, elapsedTime / duration);
//     //         yield return null;
//     //     }

//     //     smoothing = final;
//     // }
// }

