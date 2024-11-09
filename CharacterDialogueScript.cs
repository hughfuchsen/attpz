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

    private CharacterMovement characterMovement;

   
    void Start()
    {
        // Find the TextMeshPro component in the scene (or assign it in the Inspector)
        dialogueDisplay = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        dialogueNameDisplay = GameObject.Find("NameTextForDialogueUI").GetComponent<TextMeshProUGUI>();
        dialogueBGrndImage = GameObject.FindWithTag("DialogueBG").GetComponent<Image>();

        // Initialize the dialogue list
        dialogues = new List<string> { dialogueText1, dialogueText2, dialogueText3, dialogueText4 };

        characterMovement = GetComponent<CharacterMovement>();
    

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

            // Restart the NPC movement coroutine when the player leaves
            if (characterMovement.npcRandomMovementCoro == null)
            {
                characterMovement.npcRandomMovementCoro = characterMovement.StartCoroutine(characterMovement.MoveCharacterRandomly());
            }
        }
    }
}
