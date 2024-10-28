using System.Collections;
using UnityEngine;

public class LateStartDeactivate : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LateStartCoroutine());
    }

    IEnumerator LateStartCoroutine()
    {
        // Wait until the end of the current frame
        yield return new WaitForEndOfFrame();

        // Or, alternatively, just wait for the next frame with yield return null
        // yield return null;

        // Set the GameObject inactive in the next frame
        gameObject.SetActive(false);
    }
}
