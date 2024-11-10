using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterDialogueScript : MonoBehaviour
{
    public string nameText;
    public string dialogueText1;
    public string dialogueText2;
    public string dialogueText3;
    public string dialogueText4;

    private TextMeshProUGUI dialogueDisplay;
    private TextMeshProUGUI dialogueNameDisplay;
    public Image dialogueBGrndImage;
    private int currentDialogueIndex = 0;
    public List<string> dialogues;
    private bool isPlayerInRange = false;
    private Color zeroAlphaColor;

    private Coroutine npcStareAtPlayerCoro;
    private Coroutine stopNpcStareAtPlayerCoro;
    private bool staring = false;

    public CharacterAnimation characterAnimation;

    private CharacterMovement characterMovement;
    private Transform myCharacterTransform;


   
    void Start()
    {
        // Find the TextMeshPro component in the scene (or assign it in the Inspector)
        dialogueDisplay = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        dialogueNameDisplay = GameObject.Find("NameTextForDialogueUI").GetComponent<TextMeshProUGUI>();
        dialogueBGrndImage = GameObject.FindWithTag("DialogueBG").GetComponent<Image>();

        // Initialize the dialogue list
        dialogues = new List<string> { dialogueText1, dialogueText2, dialogueText3, dialogueText4 };

        characterMovement = GetComponent<CharacterMovement>();
        characterAnimation = GetComponent<CharacterAnimation>();

        myCharacterTransform = GameObject.FindWithTag("Player").transform;

    

        dialogueNameDisplay.text = ""; // Clear the name display on start
        dialogueDisplay.text = ""; // Clear the dialogue display on start


        zeroAlphaColor = Color.white;
        zeroAlphaColor.a = 0f;

        dialogueBGrndImage.color = zeroAlphaColor;
        // dialogueBGrndImage.SetActive(false);
    }

    void Update()
    {
        // Check if the player is in range and the space key is pressed
        if (isPlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.Space) || 
                    Input.GetKeyDown(KeyCode.JoystickButton0) ||  // A button
                    Input.GetKeyDown(KeyCode.JoystickButton1) ||  // B button
                    Input.GetKeyDown(KeyCode.JoystickButton2) ||  // X button
                    Input.GetKeyDown(KeyCode.JoystickButton3))            
            {  
                ShowNextDialogue();
                dialogueBGrndImage.color = Color.white;
                // dialogueBGrndImage.SetActive(true);
            }
        }
    }

    void ShowNextDialogue()
    {
        // Display the current dialogue in the TextMeshPro component
        dialogueDisplay.text = dialogues[currentDialogueIndex];
        dialogueNameDisplay.text = nameText + ":";

        // Move to the next dialogue, reset if at the end
        currentDialogueIndex = (currentDialogueIndex + 1) % dialogues.Count;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            currentDialogueIndex = Random.Range(0,4); // Reset dialogue index on re-entry

            // Stop the NPC movement coroutine if it's running
            if (characterMovement.npcRandomMovementCoro != null)
            {
                characterMovement.StopCoroutine(characterMovement.npcRandomMovementCoro);
                characterMovement.change = Vector3.zero;
                characterMovement.npcRandomMovementCoro = null;
            }


            staring = true; //initiate staring;
            if (stopNpcStareAtPlayerCoro != null)
            {
                StopCoroutine(StopNpcStareAtPlayer());
                stopNpcStareAtPlayerCoro = null;
            }
            if (npcStareAtPlayerCoro != null)
            {
                StopCoroutine(NpcStareAtPlayer());
                npcStareAtPlayerCoro = null;
            }
            
            if (npcStareAtPlayerCoro == null)
            {
                npcStareAtPlayerCoro = StartCoroutine(NpcStareAtPlayer());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueNameDisplay.text = ""; // Clear the name display when player leaves
            dialogueDisplay.text = ""; // Clear the dialogue display when player leaves
            // dialogueBGrndImage.SetActive(false);
            
            dialogueBGrndImage.color = zeroAlphaColor;
            
            // initiate the stopping of the staring at the player
            stopNpcStareAtPlayerCoro = StartCoroutine(StopNpcStareAtPlayer());

        }
    }


    private IEnumerator NpcStareAtPlayer()
    {
        while (staring)  // Keep looping
        {
            Vector2 directionToPlayer = myCharacterTransform.position - transform.position;
            float angle = (Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg + 90) % 360;
            if (angle < 0) angle += 360; // Normalize angle to 0-360 range

            // Determine the appropriate direction based on the angle
            if (angle >= 0 && angle < 60)
            {
                characterAnimation.currentAnimationDirection = characterAnimation.rightDownAnim;
            }
            else if (angle >= 60 && angle < 120)
            {
                characterAnimation.currentAnimationDirection = characterAnimation.rightAnim;
            }
            else if (angle >= 120 && angle < 180)
            {
                characterAnimation.currentAnimationDirection = characterAnimation.upRightAnim;
            }
            else if (angle >= 180 && angle < 240)
            {
                characterAnimation.currentAnimationDirection = characterAnimation.upLeftAnim;
            }
            else if (angle >= 240 && angle < 300)
            {
                characterAnimation.currentAnimationDirection = characterAnimation.leftAnim;
            }
            else if (angle >= 300 && angle < 360)
            {
                characterAnimation.currentAnimationDirection = characterAnimation.leftDownAnim;
            }

            yield return null; // Wait until the next frame
        }
    }

    private IEnumerator StopNpcStareAtPlayer()
    {
        yield return new WaitForSeconds(Random.Range(6,10)); // Wait a random time
            
        // Restart the NPC movement coroutine when the player leaves
        if (characterMovement.npcRandomMovementCoro == null)
        {
            characterMovement.npcRandomMovementCoro = characterMovement.StartCoroutine(characterMovement.MoveCharacterRandomly());
        }
        if(isPlayerInRange == false)
        {
            staring = false;
        }
    }


}
