using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public List<RoomScript> roomsSameOrAbove = new List<RoomScript>();
    public List<RoomScript> roomsBelow = new List<RoomScript>();

    public List<SpriteRenderer> doorsBelow = new List<SpriteRenderer>();
    private List<Coroutine> doorsBelowCoros = new List<Coroutine>();

    public int wallHeight = 30;
    public float displaceSpeed = 100;
    public float fadeSpeed = 100;

    private Vector3 initialPosition;

    private Coroutine currentMotionCoroutine;

    void Start()
    {
        initialPosition = this.gameObject.transform.position;
    }

    public void EnterRoom()
    {
        for (int i = 0; i < roomsSameOrAbove.Count; i++)
        {
            roomsSameOrAbove[i].MoveUp();
        };
        for (int i = 0; i < roomsBelow.Count; i++)
        {
            roomsBelow[i].MoveDown();
        };
        ResetDoorsBelowCoros();
        for (int i = 0; i < doorsBelow.Count; i++)
        {
            doorsBelowCoros.Add(StartCoroutine(Fade(doorsBelow[i], 0.35f)));
        };
    }
    
    public void ExitRoom()
    {
        ResetDoorsBelowCoros();
        for (int i = 0; i < doorsBelow.Count; i++)
        {
            doorsBelowCoros.Add(StartCoroutine(Fade(doorsBelow[i], 1)));
        };
    }

    public void MoveUp() {
        if (currentMotionCoroutine != null) {
            StopCoroutine(currentMotionCoroutine);
        }
        currentMotionCoroutine = StartCoroutine(Displace(this.gameObject, initialPosition));
    }
    
    public void MoveDown() {
        if (currentMotionCoroutine != null) {
            StopCoroutine(currentMotionCoroutine);
        }
        Vector3 downPosition = initialPosition + new Vector3(0, -wallHeight, 0);
        currentMotionCoroutine = StartCoroutine(Displace(this.gameObject, downPosition));
    }

    // TODO put this in a utility class or something
    private IEnumerator Displace(GameObject obj, Vector3 objTargetPosition)
    {
        for (float t = 0.0f; t < 1; t += Time.deltaTime)
        {
          obj.transform.position = Vector3.MoveTowards(obj.transform.position, objTargetPosition, displaceSpeed * Time.deltaTime);
          yield return null;
        }
    }

    private IEnumerator Fade(SpriteRenderer sr, float fadeTo)
    {
        // for (float t = 0.0f; t < 1; t += Time.deltaTime) {
            // float currentAlpha = Mathf.Lerp(sr.color.a, fadeTo, t * fadeSpeed);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, fadeTo);
            
            yield return null;
        // }
    }

    private void ResetDoorsBelowCoros()
    {
        for (int i = 0; i < doorsBelowCoros.Count; i++)
        {
            StopCoroutine(doorsBelowCoros[i]);
        };
        doorsBelowCoros = new List<Coroutine>();
    }
}
