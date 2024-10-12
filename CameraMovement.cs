using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    private float zoomSize = 300;
    public Transform target;
    
    public float smoothing = 200f;

    public bool freezeCamPos = false;

    public Coroutine adjustSmoothingCoro;

    public float initialSmoothing = 200f;
    // public Vector2 maxPosition;
    // public Vector2 minPosition;

    private Vector3 targetPosition;

    [SerializeField] GameObject innerBuildingBackdrop;
    [SerializeField] GameObject activateCharacterUI;
    private ActivateCharacterUITrigger activateCharacterUITriggerScript;

    public Texture2D cursorTexture; // Assign your PNG in the Inspector
    public Vector2 hotSpot = Vector2.zero; // Hotspot of the cursor (can be adjusted)


    void Awake()
    {
        innerBuildingBackdrop.SetActive(true);
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<Camera>().orthographicSize = zoomSize;
        targetPosition = transform.localPosition;
        initialSmoothing = smoothing;
        
        FixZCoordinates();
        FixZCoordinates();

        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>(); 

        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);

        activateCharacterUITriggerScript = activateCharacterUI.GetComponent<ActivateCharacterUITrigger>();

    }

    void FixedUpdate()
    {
        // Handle zooming
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0)
        {
            // Adjust zoomSize by a fixed amount
            zoomSize = Mathf.Clamp(zoomSize - zoomDelta * 100, 50, 220);
            GetComponent<Camera>().orthographicSize = zoomSize;
        }
    }

    // void Update()
    // {
    //     if (playerMovement.movementStartIndex != playerMovement.run && playerMovement.movementStartIndex != playerMovement.rideBike)
    //     {
    //         // When player is neither running nor riding a bike, use initial smoothing
    //         smoothing = initialSmoothing;
    //     }
    //     else
    //     {
    //         // When player is either running or riding a bike, adjust smoothing
    //         AdjustSmoothing(200f);
    //     }
    // }

    void LateUpdate() // handle the nature of the camera following player
    {
        // Interpolate towards the target position
        targetPosition = new Vector3(target.position.x + 7, target.position.y - 12, transform.position.z);
        if(!freezeCamPos)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        }

    }
    void FixZCoordinates()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Check if the object is not the main camera
            if (obj != gameObject)
            {
                // Set the object's position with zero Z
                Vector3 newPosition = obj.transform.position;
                newPosition.z = 0f;
                obj.transform.position = newPosition;
            }
        }
    }

    public IEnumerator AdjustSmoothing(float final)
    {

        float elapsedTime = 0f;
        float duration = 1f;

        if(smoothing != final)
        {
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                smoothing = Mathf.Lerp(initialSmoothing, final, elapsedTime / duration);
                yield return null;
            }
            smoothing = final;
        }

    }

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

