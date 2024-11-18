using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

public class LoadCSVData : MonoBehaviour
{
    // Input fields for the form
    public TMP_InputField nameInputField;
    public TMP_InputField memoryInputField;
    public TMP_InputField likesInputField;
    public TMP_InputField dislikesInputField;

    public TMP_InputField favQuoteInputField;
    public TMP_Dropdown companionDropdown; // Change from TMP_InputField to TMP_Dropdown

    // Search field and UI display components
    public TMP_InputField searchInputField;
    public TextMeshProUGUI uiText;
    public RectTransform suggestionPanel;
    public GameObject suggestionPrefab;

    // Button for submission
    public Button submitButton;


    public GameObject creationUI;
    public GameObject thanxMsgUI;

    public List<GameObject> npcList = new List<GameObject>();


    // Add a public reference to the warning text UI element

    // CSV management
    private string csvFilePath;
    private CharacterCustomization myCharacterCustomization;
    
    private CharacterMovement characterMovement;
    private TMP_InputField[] inputFields;

    // List to store CSV rows
    private List<string[]> dataRows = new List<string[]>();
    private List<GameObject> suggestionItems = new List<GameObject>();

    private TextMeshProUGUI inputPlaceholder;
    private bool isInitialPlaceholderActive = true;
// Store the original placeholder text and color
    private string originalPlaceholderText;
    private Color originalPlaceholderColor;

    private string likesVerb;
    private string dislikesVerb;
    // private Random random = new Random();

    private List<int> chosenDataRow;

    private GameObject npcPrefab;

    public GameObject npc;



    void Start()
    {
        StartCoroutine(LateStartDisplayRandomBodyCoro());

        npcPrefab = Resources.Load<GameObject>("NPCPrefab");

        inputFields = new TMP_InputField[] { nameInputField, memoryInputField, likesInputField, dislikesInputField, favQuoteInputField};

        // // Initialize dropdown options for "Companion"
        companionDropdown.ClearOptions();
        List<string> companionOptions = new List<string> { "companion", "pony", "chook", "pig", "cat", "dog", "none" };
        companionDropdown.AddOptions(companionOptions);

        AddNPCToList();


        // Initialize paths and references
        csvFilePath = Path.Combine(Application.persistentDataPath, "CharacterProfilesPrototype7.csv");
        GameObject customizationMenu = GameObject.FindGameObjectWithTag("CharacterCustomizationMenu");
        myCharacterCustomization = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterCustomization>();
        characterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();



        // Store original placeholder values
        TextMeshProUGUI placeholder = nameInputField.placeholder.GetComponent<TextMeshProUGUI>();
        originalPlaceholderText = placeholder.text;
        originalPlaceholderColor = placeholder.color;

        // ValidateInputFields();

        // Add listener to call ValidateInputFields when the input changes
        nameInputField.onValueChanged.AddListener(delegate { ValidateInputFields(); });    
        nameInputField.onSelect.AddListener(ResetPlaceholder); // Reset when user clicks on the name field

        ColorBlock buttonColors = submitButton.colors;
        buttonColors.normalColor = buttonColors.disabledColor; // Set to your desired disabled color
        submitButton.colors = buttonColors;


        submitButton.onClick.AddListener(OnSubmit);

        // Load CSV data at start
        LoadCSV(csvFilePath);

        inputPlaceholder = searchInputField.placeholder.GetComponent<TextMeshProUGUI>();
        inputPlaceholder.text = "Search for name here...";
        searchInputField.onValueChanged.AddListener(UpdateSuggestions);
        searchInputField.onValueChanged.AddListener(ClearTextFields);
        searchInputField.onSelect.AddListener(ClearInputFieldOnClick);


        nameInputField.textComponent.enableWordWrapping = true;
        memoryInputField.textComponent.enableWordWrapping = true;
        likesInputField.textComponent.enableWordWrapping = true;
        dislikesInputField.textComponent.enableWordWrapping = true;
        favQuoteInputField.textComponent.enableWordWrapping = true;
    }

    IEnumerator LateStartDisplayRandomBodyCoro()
    {
        // Wait until the end of the current frame
        yield return new WaitForEndOfFrame();

        DisplayRandomRow();

        UpdateAllNPCS();
    }

    public void UpdateAllNPCS()
    {
        foreach (GameObject npc in npcList)
        {
            UpdateNPC(npc);
        }
    }   
    public void UpdateAllNPCSDuringPlay()
    {
        foreach (GameObject npc in npcList)
        {
            if(npc.GetComponent<CharacterMovement>().playerIsOutside)
            {
                UpdateNPC(npc);
            }
        }
    }   
    private void ValidateInputFields()
    {
        // Get the current ColorBlock of the submit button
        ColorBlock buttonColors = submitButton.colors;

        // Check if the input field is not empty or contains only spaces
        if (string.IsNullOrWhiteSpace(nameInputField.text))
        {
            // Set the button's color to the disabled state
            buttonColors.normalColor = buttonColors.disabledColor; // Set to your desired disabled color
            buttonColors.highlightedColor = buttonColors.disabledColor;
        }
        else
        {
            // Set the button's color to the normal state
            buttonColors.normalColor = buttonColors.pressedColor; // Return to normal color
            buttonColors.highlightedColor = buttonColors.disabledColor;            
        }

        // Apply the modified ColorBlock back to the button
        submitButton.colors = buttonColors;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NavigateToNextField();
        }
    }

    private void NavigateToNextField()
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i].isFocused)
            {
                int nextFieldIndex = (i + 1) % inputFields.Length;
                inputFields[nextFieldIndex].Select();
                break;
            }
        }
    }

    public void OnSubmit()
    {
        // Trim whitespace from input fields
        string name = nameInputField.text.Trim();
        string memory = memoryInputField.text.Trim();
        string likes = likesInputField.text.Trim();
        string dislikes = dislikesInputField.text.Trim();
        string favQuote = favQuoteInputField.text.Trim(); 

        // Get selected companion value
        string selectedCompanion = companionDropdown.options[companionDropdown.value].text;

        // Check if the name field is empty after trimming
        if (string.IsNullOrWhiteSpace(name))
        {
            // Show warning about missing name
            TextMeshProUGUI placeholder = nameInputField.placeholder.GetComponent<TextMeshProUGUI>();
            nameInputField.text = ""; // Clear input field
            placeholder.text = "Please enter a name!";
            placeholder.color = Color.red;
            return; // Exit the method to prevent form submission
        }

        // Get character myCharacterCustomization parameters and format them
        string charParams = GetCharacterCustomizationParams();

        // Create the new column for the CSV
        StringBuilder newRow = new StringBuilder();
        // Check for commas in fields and wrap them in quotes if necessary
        newRow.Append(WrapIfNeeded(name)).Append(",")
            .Append(WrapIfNeeded(memory)).Append(",")
            .Append(WrapIfNeeded(likes)).Append(",")
            .Append(WrapIfNeeded(dislikes)).Append(",")
            .Append(WrapIfNeeded(favQuote)).Append(",");

        // Only add the companion if it's not "none"
        if (selectedCompanion == "none")
        {
            newRow.Append(","); // Keep placeholder for no companion
        }
        else if (selectedCompanion == "companion")
        {
            newRow.Append(","); // Keep placeholder for no companion
        }
        else
        {
            newRow.Append(selectedCompanion).Append(",");
        }

        // Append character parameters to the column
        newRow.Append(charParams);

        // hopfully one day can get convos for player-made characters
        newRow.Append(",");
        newRow.Append(",");
        newRow.Append(",");
        newRow.Append(",");
        // hopfully one day can get convos for player-made characters


        // Append the column to the CSV file
        AppendRowToCSV(newRow.ToString());

        // Reload the CSV to reflect the newly added entry
        LoadCSV(csvFilePath);

        // Display the details of the newly added name
        DisplayDetailsForName(name);

        // Clear the input fields
        nameInputField.text = string.Empty;
        memoryInputField.text = string.Empty;
        likesInputField.text = string.Empty;
        dislikesInputField.text = string.Empty;
        favQuoteInputField.text = string.Empty;  // Clear favorite quote field
        companionDropdown.value = 0;  // Reset dropdown to "none"

        thanxMsgUI.SetActive(true);
        creationUI.SetActive(false);
    }

    // Helper method to wrap fields in quotes if they contain commas
    private string WrapIfNeeded(string field)
    {
        // Escape any double quotes in the field by replacing " with ""
        string escapedField = field.Replace("\"", "\"\"");

        // Wrap the field in quotes if it contains a comma or a quote
        if (escapedField.Contains(",") || escapedField.Contains("\""))
        {
            return "\"" + escapedField + "\""; // Wrap in quotes and return
        }

        return escapedField; // Return the field as is if no commas or quotes
    }


    private void AddNPCToList()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

        // Clear the list first if you want to refresh it each time you call this method
        npcList.Clear();

        foreach (GameObject npc in npcs)
        {
            npcList.Add(npc);
        }
    }




    private void ResetPlaceholder(string selectedText)
    {
        // Reset the placeholder text and color when the user clicks on the input field
        TextMeshProUGUI placeholder = nameInputField.placeholder.GetComponent<TextMeshProUGUI>();
        placeholder.text = originalPlaceholderText;
        placeholder.color = originalPlaceholderColor;
    }

    private string GetCharacterCustomizationParams()
    {
        // Join the myCharacterCustomization parameters with commas and wrap them in quotes
        int[] customizationParams = new int[]
        {
            myCharacterCustomization.currentFeetIndex, myCharacterCustomization.currentPantsIndex, myCharacterCustomization.currentWaistIndex,
            myCharacterCustomization.currentHairColorIndex, myCharacterCustomization.currentShirtIndex, myCharacterCustomization.currentHeightIndex,
            myCharacterCustomization.currentBodyTypeIndex, myCharacterCustomization.currentWidthIndex, myCharacterCustomization.currentPantsColorIndex,
            myCharacterCustomization.currentShirtColorIndex, myCharacterCustomization.currentHairStyleIndex, myCharacterCustomization.currentJakettoIndex,
            myCharacterCustomization.currentJakettoColorIndex, myCharacterCustomization.currentSkinColorIndex
        };

        // Join parameters as a comma-separated string and wrap them in double quotes
        return "\"" + string.Join(",", customizationParams) + "\"";
    }

    private void AppendRowToCSV(string column)
    {
        File.AppendAllText(csvFilePath, "\n" + column);
        Debug.Log($"New column added: {column}");
    }

    // Load CSV data for search functionality
    public void LoadCSV(string fileName)
    {
        // Clear the existing rows to avoid duplicates
        dataRows.Clear();   


        // Combine persistent data path with the file name
        string fullPath = Path.Combine(Application.persistentDataPath, fileName);
        
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"CSV file not found at path: {fullPath}");
            return;
        }

        // Read all lines from the CSV file
        string[] lines = File.ReadAllLines(fullPath);

        // Define a regex pattern to handle comma-separated values with quotes
        string csvPattern = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))"; // Regex pattern to split by commas but ignore commas inside quotes

        foreach (string line in lines)
        {
            // Split by commas but ignore commas inside quotes
            string[] column = Regex.Split(line, csvPattern);

            // Trim quotes and spaces from each element
            for (int i = 0; i < column.Length; i++)
            {
                column[i] = column[i].Trim(' ', '"');
            }

            // Ensure that we are working with a 11-column column
            if (column.Length == 11) 
            {
                dataRows.Add(column); // Add each valid column to dataRows
            }
            else
            {
                Debug.LogWarning($"Skipped column with {column.Length} columns instead of 11.");
            }
        }
    }

    // Search functionality
    public void UpdateSuggestions(string currentText)
    {
        // Debug.Log($"search {currentText}");
        ClearSuggestions();
        if (string.IsNullOrEmpty(currentText)) return;

        foreach (var column in dataRows)
        {
            string name = column[0];
            if (name.ToLower().StartsWith(currentText.ToLower()))
            {
                GameObject suggestionItem = Instantiate(suggestionPrefab, suggestionPanel);
                suggestionItem.GetComponentInChildren<TextMeshProUGUI>().text = name;
                suggestionItem.GetComponent<Button>().onClick.AddListener(() =>
                {
                    searchInputField.text = name;
                    DisplayDetailsForName(name);
                    ClearSuggestions();
                });
                suggestionItems.Add(suggestionItem);
            }
        }

        suggestionPanel.gameObject.SetActive(suggestionItems.Count > 0);
    }

    public void ClearSuggestions()
    {
        foreach (var item in suggestionItems) Destroy(item);
        suggestionItems.Clear();
    }

    public void ClearTextFields(string currentText)
    {
        if (!string.IsNullOrEmpty(currentText)) uiText.text = "";
    }


    public void DisplayDetailsForName(string name)
    {
        // Synonyms or similar phrases to 'remembers'
        string[] memoryPhrases = { 
            "remembers", "recollects", "recalls", "thinks back to", "has memories of", 
            "reflects on", "smiles upon the memory of", "holds dear", "keeps close to heart", 
            "often thinks of", "cherishes the memory of", "looks back fondly on", 
            "can vividly picture", "reflects warmly on", "keeps alive in memory", 
            "relives the moment of", "fondly recalls"
        };
        string memoryVerb = memoryPhrases[Random.Range(0, memoryPhrases.Length)];

        string[] heSheLikesPhrases = { 
            "likes", "enjoys", "appreciates", "is fond of", "cherishes", "has a liking for", 
            "is drawn to", "has a soft spot for", "is enthusiastic about", "takes pleasure in", 
            "finds joy in", "relishes in", "thrives on", "values", 
            "feels passionate about", "is captivated by", "is keen on", "finds delight in"
        };
        string heSheLikesVerb = heSheLikesPhrases[Random.Range(0, heSheLikesPhrases.Length)];

        string[] theyLikePhrases = { 
            "like", "enjoy", "appreciate", "are fond of", "cherishe", "have a liking for", 
            "are drawn to", "have a soft spot for", "are enthusiastic about", "take pleasure in", 
            "find joy in", "relish in", "thrive on", "value", 
            "feel passionate about", "are captivated by", "are keen on", "find delight in"
        };
        string theyLikeVerb = theyLikePhrases[Random.Range(0, theyLikePhrases.Length)];

        string[] heSheDislikesPhrases = { 
            "dislikes", "avoids", "has a distaste for", "is not fond of", "feels uneasy about", 
            "is put off by", "steers clear of", "is wary of", 
            "feels aversion towards", "tends to shun", "is turned off by", "feels discomfort around", 
            "prefers to avoid", "finds it hard to enjoy", "is bothered by", "gets annoyed with", 
            "is irritated by"
        };
        string heSheDislikesVerb = heSheDislikesPhrases[Random.Range(0, heSheDislikesPhrases.Length)];

        string[] theyDislikePhrases = { 
            "dislike", "avoid", "have a distaste for", "are not fond of", "feel uneasy about", 
            "are put off by", "steer clear of", "are wary of", 
            "feel aversion towards", "tend to shun", "are turned off by", "feel discomfort around", 
            "prefer to avoid", "find it hard to enjoy", "are bothered by", "get annoyed with", 
            "are irritated by"
        };
        string theyDislikeVerb = theyDislikePhrases[Random.Range(0, theyDislikePhrases.Length)];


        foreach (var column in dataRows)
        {
            if (column[0].Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                StringBuilder displayText = new StringBuilder();

                string pronounSubject = "They";
                string memoryText = column[1];
                string likesText = column[2];
                string dislikesText = column[3];
                string dialogueText1 = column[7];
                string dialogueText2 = column[8];
                string dialogueText3 = column[9];
                string dialogueText4 = column[10];

                // Pronoun determination
                if (Regex.IsMatch(memoryText, @"\b(his|he)\b", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(likesText, @"\b(his|he)\b", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(dislikesText, @"\b(his|he)\b", RegexOptions.IgnoreCase))
                {
                    pronounSubject = "He";
                }
                else if (Regex.IsMatch(memoryText, @"\b(her|she)\b", RegexOptions.IgnoreCase) ||
                            Regex.IsMatch(likesText, @"\b(her|she)\b", RegexOptions.IgnoreCase) ||
                            Regex.IsMatch(dislikesText, @"\b(her|she)\b", RegexOptions.IgnoreCase))
                {
                    pronounSubject = "She";
                }

                if(pronounSubject == "They")
                {
                    likesVerb = theyLikeVerb;
                    dislikesVerb = theyDislikeVerb;
                }
                else if(pronounSubject == "He" || pronounSubject == "She")
                {
                    likesVerb = heSheLikesVerb;
                    dislikesVerb = heSheDislikesVerb;
                }



                // Start building the display text
                if (!string.IsNullOrWhiteSpace(column[0]))
                    displayText.Append($"<color=#FF5733>{TrimFullStop(column[0])}</color> ");

                if (!string.IsNullOrWhiteSpace(column[1]))
                    displayText.Append($"\n{memoryVerb} {TrimFullStop(column[1])}\n");

                if (!string.IsNullOrWhiteSpace(column[2]))
                    displayText.Append($"\n{pronounSubject} {likesVerb} {TrimFullStop(column[2])}\n");

                if (!string.IsNullOrWhiteSpace(column[3]))
                    displayText.Append($"\n{pronounSubject} {dislikesVerb} {TrimFullStop(column[3])}\n");

                if (!string.IsNullOrWhiteSpace(column[4]))
                    displayText.Append($"\n<color=#FFD700>Fav Quote:</color>\n{TrimFullStop(column[4])}\n");

                if (!string.IsNullOrWhiteSpace(column[5]))
                    displayText.Append($"\n<color=#8A2BE2>Companion:</color>\n{TrimFullStop(column[5])}");

                uiText.text = displayText.ToString();

                // Handle character myCharacterCustomization params
                string[] intParams = column[6].Split(',').Where(param => !string.IsNullOrWhiteSpace(param)).ToArray();
                if (intParams.Length == 14)
                {
                    List<int> parameters = intParams.Select(param => int.Parse(param.Trim())).ToList();
                    chosenDataRow = parameters;
                    myCharacterCustomization.UpdateSpecific(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4],
                                                parameters[5], parameters[6], parameters[7], parameters[8], parameters[9],
                                                parameters[10], parameters[11], parameters[12], parameters[13]);
                }
            }
        }
    }

    public void UpdateNPC(GameObject obj)
    {
        int randomIndex = Random.Range(0, 650);

        var row = dataRows[randomIndex];
        
        string nameText = row[0];
        string chrctrAppearance = row[6];

        // Handle character myCharacterCustomization params
        string[] intParams = row[6].Split(',').Where(param => !string.IsNullOrWhiteSpace(param)).ToArray();
        if (intParams.Length == 14)
        {
            List<int> parameters = intParams.Select(param => int.Parse(param.Trim())).ToList();
            obj.GetComponent<CharacterCustomization>().UpdateSpecific(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4],
                    parameters[5], parameters[6], parameters[7], parameters[8], parameters[9],
                    parameters[10], parameters[11], parameters[12], parameters[13]);
        }
    
        string dialogueText1 = row[7];
        string dialogueText2 = row[8];
        string dialogueText3 = row[9];
        string dialogueText4 = row[10];
        
        obj.GetComponent<CharacterDialogueScript>().nameText = nameText;
        obj.GetComponent<CharacterCustomization>().chrctrAppearance = chrctrAppearance;
        obj.GetComponent<CharacterDialogueScript>().dialogueText1 = dialogueText1;
        obj.GetComponent<CharacterDialogueScript>().dialogueText2 = dialogueText2;
        obj.GetComponent<CharacterDialogueScript>().dialogueText3 = dialogueText3;
        obj.GetComponent<CharacterDialogueScript>().dialogueText4 = dialogueText4;

        obj.GetComponent<CharacterDialogueScript>().dialogues[0] = dialogueText1;
        obj.GetComponent<CharacterDialogueScript>().dialogues[1] = dialogueText2;
        obj.GetComponent<CharacterDialogueScript>().dialogues[2] = dialogueText3;
        obj.GetComponent<CharacterDialogueScript>().dialogues[3] = dialogueText4;
    }

    // public void UpdateNPC(GameObject obj)
    // {
    //     int randomIndex = Random.Range(0, 650);

    //     var row = dataRows[randomIndex];
        
    //     string nameText = row[0];

    //     // Handle character myCharacterCustomization params
    //     string[] intParams = row[6].Split(',').Where(param => !string.IsNullOrWhiteSpace(param)).ToArray();
    //     if (intParams.Length == 14)
    //     {
    //         List<int> parameters = intParams.Select(param => int.Parse(param.Trim())).ToList();
    //         obj.GetComponent<CharacterCustomization>().UpdateSpecific(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4],
    //                 parameters[5], parameters[6], parameters[7], parameters[8], parameters[9],
    //                 parameters[10], parameters[11], parameters[12], parameters[13]);
    //     }
    
    //     string dialogueText1 = row[7];
    //     string dialogueText2 = row[8];
    //     string dialogueText3 = row[9];
    //     string dialogueText4 = row[10];
        
    //     obj.GetComponent<CharacterDialogueScript>().nameText = nameText;
    //     obj.GetComponent<CharacterDialogueScript>().dialogueText1 = dialogueText1;
    //     obj.GetComponent<CharacterDialogueScript>().dialogueText2 = dialogueText2;
    //     obj.GetComponent<CharacterDialogueScript>().dialogueText3 = dialogueText3;
    //     obj.GetComponent<CharacterDialogueScript>().dialogueText4 = dialogueText4;
    // }

    public void SpawnRandomPerson()
    {
        // at the current dataRows[i] there is a list of indices that we want to get and pass to an NPC controller script.
        npc= Instantiate(npcPrefab, new Vector3(-1608, 84, 0), Quaternion.identity); 
        UpdateNPC(npc);
    }


    public void DisplayRandomRow()
    {
        int randomIndex = Random.Range(0, dataRows.Count);
        string randomName = dataRows[randomIndex][0];
        DisplayDetailsForName(randomName);
        searchInputField.text = "";
        inputPlaceholder.text = "Search for name here...";
    }

    public void ClearInputFieldOnClick(string selectedText)
    {
        searchInputField.text = "";
        inputPlaceholder.text = "Search for name here...";
    }

    string ColorToHex(Color color)
    {
        return ColorUtility.ToHtmlStringRGB(color); // Converts to hex without alpha
    }

    // private string TrimFullStop(string text)
    // {
    //     if (text.EndsWith("."))
    //     {
    //         return text.Substring(0, text.Length - 1);
    //     }
    //     return text;
    // }
    private string TrimFullStop(string text)
    {
        return text.EndsWith(".") ? text.Substring(0, text.Length - 1) : text;
    }

}
