using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float zoomSize = 300;
    [SerializeField] Transform target;
    public float smoothing;
    public Vector2 maxPosition;
    public Vector2 minPosition;

    private Vector3 targetPosition;

    [SerializeField] GameObject innerBuildingBackdrop;

    // void OnEnable()
    // {
    //     innerBuildingBackdrop = GameObject.FindWithTag("InnerBuildingBackdrop");
    //     if (innerBuildingBackdrop != null)
    //     {
    //     innerBuildingBackdrop.SetActive(true); 
    //     }
    // }
    void Awake()
    {
        innerBuildingBackdrop.SetActive(true);
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<Camera>().orthographicSize = zoomSize;
        targetPosition = transform.position;
        
        
        FixZCoordinates();
        FixZCoordinates();
    }

    void FixedUpdate()
    {
        // Handle zooming
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0)
        {
            // Adjust zoomSize by a fixed amount
            zoomSize = Mathf.Clamp(zoomSize - zoomDelta * 100, 50, 500);
            GetComponent<Camera>().orthographicSize = zoomSize;
        }
    }

    void LateUpdate()
    {
        // Interpolate towards the target position
        targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
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
}

