using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        while (true)
        {
            int choice;
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("1. Test Extract Email Addresses");
            Console.WriteLine("2. Extract Max Integer");
            Console.WriteLine("3. Delete Unique Words");
            Console.WriteLine("4. WriteRealNumbersToFile");
            Console.WriteLine("5. FolderManipulations");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice: ");
            choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    TestExtractEmailAddresses();
                    break;
                case 2:
                    ExtractMaxInteger();
                    break;
                case 3:
                    DeleteUniqueWords();
                    break;
                case 4:
                    WriteRealNumbersToFile();
                    break;
                case 5:
                    FolderManipulations();
                    break;
                case 6:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void TestExtractEmailAddresses()
    {
        string inputFilePath = "../../../inputTask1.txt";
        string outputFilePath = "../../../outputTask1.txt";

        string fileContent = File.ReadAllText(inputFilePath);

        List<string> emailAddresses = ExtractEmailAddresses(fileContent);

        Console.WriteLine("Extracted Email Addresses:");
        foreach (string email in emailAddresses)
        {
            Console.WriteLine(email);
        }
        Console.WriteLine($"Total Email Addresses: {emailAddresses.Count}");

        string replacedContent = ReplaceEmailAddresses(fileContent, "[REPLACED_EMAIL]");
        string deletedContent = DeleteEmailAddresses(fileContent);

        File.WriteAllText(outputFilePath, replacedContent);

        File.WriteAllText("../../../deleted_emails.txt", string.Join("\n", emailAddresses));
    }

    static void ExtractMaxInteger()
    {
        {
            string inputFilePath = "../../../inputTask2.txt";
            string outputFilePath = "../../../outputTask2.txt";

            try
            {
                string[] lines = File.ReadAllLines(inputFilePath);
                char[] punctuationMarks = { '.', ',', ';', ':', '!', '?' };

                var integers = lines
                    .SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    .Select(word => new string(
                        word.Where(c => !punctuationMarks.Contains(c)).ToArray()
                    ))
                    .Where(word => int.TryParse(word, out _))
                    .Select(int.Parse);

                if (integers.Any())
                {
                    int maxInteger = integers.Max();

                    File.WriteAllText(outputFilePath, $"The maximum integer is: {maxInteger}");

                    Console.WriteLine($"The maximum integer is: {maxInteger}");
                }
                else
                {
                    Console.WriteLine("No valid integers found in the text.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    static void DeleteUniqueWords()
    {
        string inputFilePath = "../../../inputTask3.txt";
        string outputFilePath = "../../../outputTask3.txt";

        try
        {
            string[] lines = File.ReadAllLines(inputFilePath);

            List<string> resultLines = ProcessLines(lines);

            File.WriteAllLines(outputFilePath, resultLines);

            Console.WriteLine("Operation completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void FolderManipulations()
    {
        string studentName = "Hurkin";

        string folder1Path = $"temp\\{studentName}1";
        string folder2Path = $"temp\\{studentName}2";

        Directory.CreateDirectory(folder1Path);
        Directory.CreateDirectory(folder2Path);

        string t1FilePath = Path.Combine(folder1Path, "t1.txt");
        string t2FilePath = Path.Combine(folder1Path, "t2.txt");

        File.WriteAllText(
            t1FilePath,
            "Шевченко Степан Іванович, 2001 року народження, місце проживання м. Суми"
        );
        File.WriteAllText(
            t2FilePath,
            "Комар Сергій Федорович, 2000 року народження, місце проживання м. Київ"
        );

        Console.WriteLine("Text files with text were created!");
        Console.WriteLine("Press any Key to continue...");
        Console.ReadLine();

        string t3FilePath = Path.Combine(folder2Path, "t3.txt");

        File.AppendAllText(t3FilePath, File.ReadAllText(t1FilePath));
        File.AppendAllText(t3FilePath, File.ReadAllText(t2FilePath));

        Console.WriteLine("Text file with text was created!");
        Console.WriteLine("Press any Key to continue...");
        Console.ReadLine();

        PrintFileDetails(t1FilePath);
        PrintFileDetails(t2FilePath);
        PrintFileDetails(t3FilePath);

        string newT2FilePath = Path.Combine(folder2Path, "t2.txt");
        File.Move(t2FilePath, newT2FilePath);

        string newT1FilePath = Path.Combine(folder2Path, "t1.txt");
        File.Copy(t1FilePath, newT1FilePath);

        string allFolderPath = $"temp\\ALL";
        Directory.Move(folder2Path, allFolderPath);

        Directory.Delete(folder1Path, true);

        string[] allFiles = Directory.GetFiles(allFolderPath);
        Console.WriteLine("Files in the folder All:");
        foreach (string file in allFiles)
        {
            PrintFileDetails(file);
        }
    }

    static void PrintFileDetails(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        Console.WriteLine(
            $"File: {fileInfo.Name}, Size: {fileInfo.Length} bytes, Last Modified: {fileInfo.LastWriteTime}"
        );
    }

    static List<string> ProcessLines(string[] lines)
    {
        // Create a dictionary to store word frequencies across all lines
        Dictionary<string, int> wordFrequencies = new Dictionary<string, int>(
            StringComparer.OrdinalIgnoreCase
        );

        // Count the occurrences of each word across all lines
        foreach (string line in lines)
        {
            // Split the line into words
            string[] words = line.Split(
                new[] { ' ', ',', '.', ';', ':', '!', '?', '@', '\'' },
                StringSplitOptions.RemoveEmptyEntries
            );

            // Update word frequencies
            foreach (string word in words)
            {
                if (wordFrequencies.ContainsKey(word))
                {
                    wordFrequencies[word]++;
                }
                else
                {
                    wordFrequencies[word] = 1;
                }
            }
        }

        // Process each line to delete words that appear only once
        List<string> resultLines = new List<string>();
        foreach (string line in lines)
        {
            resultLines.Add(ProcessLine(line, wordFrequencies));
        }

        return resultLines;
    }

    static void WriteRealNumbersToFile()
    {
        string filePath = "../../../realNumbers.bin";

        // Write a sequence of real numbers to the binary file
        WriteRealNumbersToFile(filePath);

        // Print components that do not fall into the given range
        PrintOutOfRangeComponents(filePath, 10.0, 20.0);
    }

    static string ProcessLine(string line, Dictionary<string, int> wordFrequencies)
    {
        // Split the line into words
        string[] words = line.Split(
            new[] { ' ', ',', '.', ';', ':', '!', '?', '@', '\'' },
            StringSplitOptions.RemoveEmptyEntries
        );

        // Delete words that appear only once
        StringBuilder resultLine = new StringBuilder();
        foreach (string word in words)
        {
            if (wordFrequencies[word] > 1)
            {
                resultLine.Append(word).Append(' ');
            }
        }

        // Trim any trailing space
        return resultLine.ToString().Trim();
    }

    static List<string> ExtractEmailAddresses(string text)
    {
        // Regular expression pattern for matching email addresses
        string pattern = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b";
        Regex regex = new Regex(pattern);

        // Match email addresses using the regular expression
        MatchCollection matches = regex.Matches(text);

        // Extract and return the email addresses
        List<string> emailAddresses = new List<string>();
        foreach (Match match in matches)
        {
            emailAddresses.Add(match.Value);
        }

        return emailAddresses;
    }

    static string ReplaceEmailAddresses(string text, string replacement)
    {
        string pattern = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b";
        Regex regex = new Regex(pattern);

        // Perform the replacement
        string replacedText = regex.Replace(text, replacement);

        return replacedText;
    }

    static string DeleteEmailAddresses(string text)
    {
        string pattern = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b";
        Regex regex = new Regex(pattern);

        // Remove email addresses from the text
        string deletedText = regex.Replace(text, "");

        return deletedText;
    }

    static void WriteRealNumbersToFile(string filePath)
    {
        try
        {
            // Create an array of real numbers
            double[] realNumbers = { 5.0, 15.0, 25.0, 35.0, 45.0 };

            // Write the real numbers to the binary file
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                foreach (double number in realNumbers)
                {
                    writer.Write(number);
                }
            }

            Console.WriteLine("Real numbers written to file successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void PrintOutOfRangeComponents(string filePath, double lowerBound, double upperBound)
    {
        try
        {
            // Read real numbers from the binary file and print those out of range
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                Console.WriteLine($"Numbers out of range ({lowerBound} - {upperBound}):");

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    double number = reader.ReadDouble();

                    if (number < lowerBound || number > upperBound)
                    {
                        Console.WriteLine(number);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
