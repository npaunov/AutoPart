using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using AutoPartApp.Models;

namespace AutoPartApp.Utilities;

/// <summary>
/// A static service class responsible for importing data from CSV files.
/// </summary>
public static class DataImportUtil
{
    /// <summary>
    /// A collection to store the imported parts.
    /// </summary>
    public static ObservableCollection<Part> ImportedParts { get; private set; } = new();

    /// <summary>
    /// Imports data from a specified CSV file and stores it in the `ImportedParts` collection.
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
            ImportedParts.Clear();

            // Read all lines from the CSV file
            var lines = File.ReadAllLines(filePath);

            // Skip the header row and parse each line into a Part object
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                var values = line.Split(';');

                if (values.Length < 5)
                {
                    return $"Invalid data format on line {i + 1}.";
                }

                try
                {
                    var part = new Part
                    {
                        Id = values[0],
                        Description = values[1],
                        PriceBGN = decimal.Parse(values[2], CultureInfo.InvariantCulture),
                        Package = int.Parse(values[3]),
                        InStore = int.Parse(values[4])
                    };

                    ImportedParts.Add(part);
                }
                catch (Exception ex)
                {
                    return $"Error parsing line {i + 1}: {ex.Message}";
                }
            }
            var a = ImportedParts;

            return "Data imported successfully!";
        }
        catch (Exception ex)
        {
            return $"An error occurred during data import: {ex.Message}";
        }
    }
}
