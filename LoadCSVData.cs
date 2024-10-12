using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import the TextMesh Pro namespace
using System.IO;

public class LoadCSVData : MonoBehaviour
{
    public TextMeshProUGUI uiText; // Reference to TextMeshProUGUI component in the Inspector
    private List<string[]> dataRows = new List<string[]>(); // Store the CSV data (900 rows with 4 columns)

    void Start()
    {
        LoadCSV("CharacterProfilesPrototype4"); // Path to the CSV file (without the .csv extension)
        DisplayRandomRow(); // Display a random row at the start
    }

    // Load CSV file into dataRows
    void LoadCSV(string filePath)
    {
        TextAsset csvData = Resources.Load<TextAsset>(filePath); // Load the CSV file from Resources folder
        if (csvData == null)
        {
            Debug.LogError("CSV file not found");
            return;
        }

        StringReader reader = new StringReader(csvData.text);
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            string[] row = line.Split(','); // Split the line by commas into columns
            if (row.Length == 4) // Ensure that there are exactly 4 columns
            {
                dataRows.Add(row); // Add each valid row to dataRows
            }
            else
            {
                Debug.LogWarning($"Skipped row with {row.Length} columns instead of 4.");
            }
        }
    }

    // Display a specific row by index
    void DisplayRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= dataRows.Count) return; // Prevent out-of-bounds access

        string[] row = dataRows[rowIndex];
        // Update the TextMeshProUGUI text field with the row's data (four columns)
        uiText.text = $"<color=#FF5733>Name:</color>\n{row[0]}\n" + // Extra break after Name and value
                    $"<color=#33A1FF>Fav Memory:</color>\n{row[1]}\n" + // Extra break after Fav Memory and value
                    $"<color=#4CAF50>Likes:</color>\n{row[2]}\n" + // Extra break after Likes and value
                    $"<color=#FF6347>Dislikes:</color>\n{row[3]}\n"; // Extra break after Dislikes and value

    }

    // Display a random row from the CSV
    public void DisplayRandomRow()
    {
        int randomIndex = Random.Range(0, dataRows.Count); // Pick a random row index (0 to 899)
        DisplayRow(randomIndex); // Display the row at the random index
    }
}
