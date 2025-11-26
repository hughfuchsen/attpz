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
}

