using System.Collections.Generic;
using System.IO;

namespace AutoPartApp.Services;

/// <summary>
/// A static service class responsible for importing data from CSV files.
/// </summary>
public static class DataImportService
{
    /// <summary>
    /// A collection to store the imported data.
    /// </summary>
    public static List<string[]> ImportedData { get; private set; } = new();

    /// <summary>
    /// Imports data from a specified CSV file and stores it in the `ImportedData` collection.
    /// </summary>
    /// <param name="filePath">The path to the CSV file.</param>
    /// <returns>A string indicating the result of the import operation.</returns>
    public static string ImportCsv(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            return "Invalid file path. Please select a valid file.";
        }

        try
        {
            // Clear the existing data
            ImportedData.Clear();

            // Read all lines from the CSV file
            var lines = File.ReadAllLines(filePath);

            // Parse each line and add it to the collection
            foreach (var line in lines)
            {
                var values = line.Split(',');
                ImportedData.Add(values);
            }

            var a = ImportedData;

            return "Data imported successfully!";
        }
        catch
        {
            return "An error occurred during data import.";
        }
    }
}
