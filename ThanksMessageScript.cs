using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThanksMessageScript : MonoBehaviour
{
    public GameObject canvasObject; // Assign your canvas object here
    public CharacterMovement myCharacterMovement; // Reference to your CharacterMovement script
    private Coroutine dormancyCoroutine;

    void Start()
    {
    myCharacterMovement = GameObject.FindWithTag("Player").GetComponent<CharacterMovement>();

    }
    private void Update()
    {
        // Check for dormancy: no movement and no input from keyboard or mouse
        if (myCharacterMovement.change == Vector3.zero && !Input.anyKey)
        {
            if (dormancyCoroutine == null)
            {
                dormancyCoroutine = StartCoroutine(ActivateCanvasAfterDormancy());
            }
        }
        else
        {
            // If there's movement or input, stop the dormancy timer and hide the canvas
            if (dormancyCoroutine != null)
            {
                StopCoroutine(dormancyCoroutine);
                dormancyCoroutine = null;
            }
            canvasObject.SetActive(false); // Set canvas inactive when movement or input resumes
        }
    }
    // private bool IsMouseMoving()
    // {
    //     // Check if the mouse has moved or any button is pressed
    //     return Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2);
    // }

    private IEnumerator ActivateCanvasAfterDormancy()
    {
        yield return new WaitForSeconds(20);
        canvasObject.SetActive(true);
    }
}
