using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddRowToCSV : MonoBehaviour
{
    public TMP_InputField nameInputField;        // Input field for Name
    public TMP_InputField memoryInputField;      // Input field for Favorite Memory
    public TMP_InputField likesInputField;       // Input field for Likes
    public TMP_InputField dislikesInputField;    // Input field for Dislikes

    public Button submitButton;                  // Button to submit new row to the CSV

    private string csvFilePath;                  // Path to the CSV file
    private CharacterCustomization customization; // Reference to CharacterCustomization script

    // Array to store input fields
    private TMP_InputField[] inputFields;

    void Start()
    {
        // Initialize inputFields array
        inputFields = new TMP_InputField[] { nameInputField, memoryInputField, likesInputField, dislikesInputField };

        // Set the CSV file path to a writable location
        csvFilePath = Path.Combine(Application.persistentDataPath, "CharacterProfilesPrototype7.csv");
        Debug.Log(csvFilePath);

        // Find the game object with the tag and get the CharacterCustomization component
        GameObject customizationMenu = GameObject.FindGameObjectWithTag("CharacterCustomizationMenu");
        customization = customizationMenu.GetComponent<CharacterCustomization>();

        // Add listener to the submit button
        submitButton.onClick.AddListener(OnSubmit);
    }

    void Update()
    {
        // Check for Tab key press
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NavigateToNextField();
        }
    }

    // Navigate to the next input field when Tab is pressed
    private void NavigateToNextField()
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i].isFocused)
            {
                // Move focus to the next input field, wrapping back to the first if needed
                int nextFieldIndex = (i + 1) % inputFields.Length;
                inputFields[nextFieldIndex].Select();
                break;
            }
        }
    }

    // Called when the user submits the form
    public void OnSubmit()
    {
        // Collect user input
        string name = nameInputField.text;
        string memory = memoryInputField.text;
        string likes = likesInputField.text;
        string dislikes = dislikesInputField.text;

        // Fetch character customization parameters from the CharacterCustomization component
        string charParams = GetCharacterCustomizationParams();

        // Create a new row in CSV format
        StringBuilder newRow = new StringBuilder();
        newRow.Append(name).Append(",")
              .Append(memory).Append(",")
              .Append(likes).Append(",")
              .Append(dislikes).Append(",")
              .Append(charParams);

        // Append the new row to the CSV file
        AppendRowToCSV(newRow.ToString());
    }

    // Fetches the 14 parameters from CharacterCustomization
    private string GetCharacterCustomizationParams()
    {
        return $"{customization.currentBodyTypeIndex}," +
               $"{customization.currentHeightIndex}," +
               $"{customization.currentWidthIndex}," +
               $"{customization.currentHairStyleIndex}," +
               $"{customization.currentHairColorIndex}," +
               $"{customization.currentShirtIndex}," +
               $"{customization.currentWaistIndex}," +
               $"{customization.currentPantsIndex}," +
               $"{customization.currentFeetIndex}," +
               $"{customization.currentJakettoIndex}," +
               $"{customization.currentSkinColorIndex}," +
               $"{customization.currentShirtColorIndex}," +
               $"{customization.currentPantsColorIndex}," +
               $"{customization.currentJakettoColorIndex}";
    }

    // Appends a new row to the CSV file
    private void AppendRowToCSV(string row)
    {
        // Write the row to the CSV file at the writable location
        File.AppendAllText(csvFilePath, "\n" + row);
        Debug.Log($"New row added: {row}");
        LoadCSVData loadCSVDataScript = FindObjectOfType<LoadCSVData>();
        loadCSVDataScript.LoadCSV("CharacterProfilesPrototype7");
    }
}
