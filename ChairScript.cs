using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairScript : MonoBehaviour
{
    CharacterMovement playerMovement;
    CameraMovement cameraMovement;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject chairScriptAnchorPoint;
    public Vector3 currentSeatAnchorPoint;
    public Vector3 initialPlayerPosBeforeSitting;


    



    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
        chairScriptAnchorPoint = transform.Find("chairScriptAnchorPoint").gameObject;
        playerMovement = Player.GetComponent<CharacterMovement>();
        currentSeatAnchorPoint = chairScriptAnchorPoint.transform.position;
    }

    // Make sure the method has the correct signature
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
                initialPlayerPosBeforeSitting = Player.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the collision object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
                playerMovement.playerSitting = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // if(cameraMovement.adjustSmoothingCoro != null)
                // {
                //     StopCoroutine(cameraMovement.adjustSmoothingCoro); 
                // }


                
                // Player sits
                initialPlayerPosBeforeSitting = Player.transform.position;

                // Freeze the camera position
                cameraMovement.freezeCamPos = true;

                playerMovement.playerSitting = true;

                // Move the player to the current seat anchor point
                Player.transform.position = currentSeatAnchorPoint + new Vector3(-8, 22, 0) + new Vector3(-4, 1, 0);
            }
            // else if (Input.GetKeyUp(KeyCode.Space))
            // {
            //     // Player stands up
            //     playerMovement.playerSitting = false;

            //     // Unfreeze the camera and let it follow the player again
            //     // cameraMovement.freezeCamPos = false;

            //     Player.transform.position = initialPlayerPosBeforeSitting;

            // }
            else
            {

                // cameraMovement.adjustSmoothingCoro = StartCoroutine(cameraMovement.AdjustSmoothing()); 

                playerMovement.playerSitting = false;
                cameraMovement.freezeCamPos = false;

            }
        }
    }




}
