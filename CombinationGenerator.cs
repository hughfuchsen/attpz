using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CombinationGenerator : MonoBehaviour
{
    static System.Random random = new System.Random();
    private const string filePath = "Assets/AATPZAssets/Resources/combinations.17.09.25.csv"; // File path for the CSV

    // Start is called before the first frame update
    void Start()
    {
        // Number of combinations
        int numCombinations = 1000;

        // List to store generated combinations
        List<string> combinations = new List<string>();

        // Generate the combinations
        for (int i = 0; i < numCombinations; i++)
        {
            string combination = GenerateRandomCombination();
            combinations.Add(combination);
        }

        // Write combinations to a CSV file
        WriteCombinationsToCSV(combinations);
    }

    // Function to generate a random combination of 14 parameters
    static string GenerateRandomCombination()
    {
        // Generate the combination as a comma-separated string
        return string.Join(",", new int[]
        {
            random.Next(0, 2),   // feet (0-1)
            random.Next(0, 4),   // pants (0-3)//with dresso
            random.Next(0, 2),   // waist (0-1)
            random.Next(0, 10),  // hairCol (1-10)
            random.Next(1, 10),  // shirt (1-9)
            random.Next(0, 3),   // height (0-2)
            random.Next(0, 3),   // bodyType (0-2)
            random.Next(0, 4),   // width (0-3)
            random.Next(0, 10),  // pantsCol (0-10)
            random.Next(0, 11),  // shirtCol (0-10)
            random.Next(0, 1),  // hat (0-1)
            random.Next(0, 13),  // hairStyle (1-13)
            random.Next(0, 9),   // jaketto (0-8)
            random.Next(0, 10),  // jakettoCol (0-10)
            random.Next(0, 7)    // skinCol (0-6)
        });
    }

    // Function to write combinations to a CSV file
    void WriteCombinationsToCSV(List<string> combinations)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write each combination to the file as a single line
            foreach (var combo in combinations)
            {
                writer.WriteLine(combo);  // Each combo is written on a new line
            }
        }

        Debug.Log($"Combinations written to {filePath}");
    }
}
