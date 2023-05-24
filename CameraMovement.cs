using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour{

    public float zoomSize = 150;
    public Transform target;
    public float smoothing;
    public Vector2 maxPosition;
    public Vector2 minPosition;

    // Start is called before the first frame update
    void Start()
    {
        
        GetComponent<Camera>().orthographicSize = zoomSize;
    }

    void Update ()
    {
        if (Input.GetAxis("Mouse ScrollWheel")>0)
        {
            if (zoomSize>50)
            zoomSize -= 5;
        }

        if (Input.GetAxis("Mouse ScrollWheel")<0)
        {
            if (zoomSize<150)
            zoomSize += 5;
        }
     GetComponent<Camera>().orthographicSize = zoomSize;

    }

    // Update is called once per frame
    void LateUpdate () {
        if(transform.position != target.position)
        {
          Vector3 targetPosition = new Vector3(target.position.x,
                                                target.position.y,
                                                transform.position.z);

        //   targetPosition.x = Mathf.Clamp(targetPosition.x,
        //                                   minPosition.x,
        //                                   maxPosition.x);
        //   targetPosition.y = Mathf.Clamp(targetPosition.y,
        //                                   minPosition.y,
        //                                   maxPosition.y);

          transform.position = Vector3.Lerp(transform.position,
                                            targetPosition, smoothing);
        }
    }
}
