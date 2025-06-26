using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using QuickHash.Hashing;

namespace MtarTool.Core.Utility
{
    public static class NameResolver
    {
        // The dictionary now stores the legacy hash (as a ulong, though it's a 32-bit value) and the string name.
        static Dictionary<ulong, string> hashDictionary = new Dictionary<ulong, string>();
        static List<string> outputList = new List<string>(0);

        // Static constructor to initialize the dictionary.
        static NameResolver()
        {
            try
            {
                string dictionaryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\mtar_dictionary.txt";
                if (File.Exists(dictionaryPath))
                {
                    string[] dictionaryLines = File.ReadAllLines(dictionaryPath);
                    foreach (string entry in dictionaryLines)
                    {
                        // Use the legacy hashing function on the dictionary entries.
                        // We assume the dictionary entries do not have extensions, so removeExtension = true.
                        ulong hash = Hashing.HashFileNameLegacy(entry, true);
                        if (!hashDictionary.ContainsKey(hash))
                        {
                            hashDictionary.Add(hash, entry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // It's good practice to handle potential exceptions, like the dictionary file not being found.
                Console.WriteLine($"Error loading hash dictionary: {ex.Message}");
            }
        }

        public static string TryFindName(ulong hash)
        {
            // The hashes from the file are likely 64-bit, but the part we care about is the lower 32-bit value.
            // We apply a mask to get the relevant part of the hash.
            ulong maskedHash = hash & 0xFFFFFFFFFFFF;

            if (hashDictionary.ContainsKey(maskedHash))
            {
                string foundName = hashDictionary[maskedHash];
                outputList.Add($"{maskedHash:x} = {foundName}");
                return foundName;
            }

            // Fallback for unknown hashes
            string unknownName = $"unknown_{maskedHash:x}";
            outputList.Add($"{maskedHash:x} = {unknownName}");
            return unknownName;
        }

        public static ulong GetHashFromName(string text)
        {
            // This function is now primarily for repacking, and it should use the legacy hash.
            string cleanName = Path.GetFileNameWithoutExtension(text);

            if (char.IsDigit(cleanName[0]) && cleanName.Length > 4 && cleanName[4] == '_')
            {
                cleanName = cleanName.Substring(5);
            }

            ulong hash = Hashing.HashFileNameLegacy(cleanName, true);
            outputList.Add($"{hash:x} = {cleanName}");
            return hash;
        }

        public static void WriteOutputList()
        {
            // To avoid duplicates from both finding and generating hashes, we can use LINQ's Distinct().
            File.WriteAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\hashed_names.txt", outputList.Distinct().ToList());
        }
    }
}