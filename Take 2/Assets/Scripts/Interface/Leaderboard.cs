using UnityEngine;
using System.Collections.Generic;

public class Leaderboard {

    //Max amount of entries on the leaderboard
    public const int EntryCount = 10;

    //Used to store the name and score
    public struct ScoreEntry
    {
        public string mName;
        public int mScore;
        //Can have additional variables, just add them into the other functions also.

        public ScoreEntry(string name, int score)
        {
            this.mName = name;
            this.mScore = score;
        }
    }

    private static List<ScoreEntry> tempEntries;

    private static List<ScoreEntry> entries
    {
        get
        {
            if (tempEntries == null)
            {
                tempEntries = new List<ScoreEntry>();
                LoadScores();
            }
            return tempEntries;
        }
    }

    private const string PlayerPrefsBaseKey = "leaderboard";

    //Compares the score of b to a, and if b is bigger, moves it to a's position in the list
    private static void SortScores()
    {
        tempEntries.Sort((a, b) => b.mScore.CompareTo(a.mScore));
    }

    //Loads the scores into the tempEntries array
    private static void LoadScores()
    {
        tempEntries.Clear();
        for(int i = 0; i < EntryCount; i++)
        {
            ScoreEntry entry;
            entry.mName = PlayerPrefs.GetString(PlayerPrefsBaseKey + "[" + i + "].mName", "");
            entry.mScore = PlayerPrefs.GetInt(PlayerPrefsBaseKey + "[" + i + "].mScore", 0);
            tempEntries.Add(entry);
        }
        SortScores();
    }
    
    //Stores the scores into the player prefs
    private static void SaveScores()
    {
        for(int i = 0; i < EntryCount; i++)
        {
            ScoreEntry entry = tempEntries[i];
            PlayerPrefs.SetString(PlayerPrefsBaseKey + "[" + i + "].mName", entry.mName);
            PlayerPrefs.SetInt(PlayerPrefsBaseKey + "[" + i + "].mScore", entry.mScore);
        }
    }

    //Returns the entry at index
    public static ScoreEntry GetEntry(int index)
    {
        return entries[index];
    }

    //Adds the entry to the entries list
    public static void Record(string name, int score)
    {
        entries.Add(new ScoreEntry(name, score));
        SortScores();
        if (entries.Count > EntryCount)
        {
            entries.RemoveAt(entries.Count - 1);
        }
        SaveScores();
    }


    //Returns whether the score is higher than any of the others in the entry list
    public static bool CheckScore(int currentScore)
    {
        for(int i = 0; i < EntryCount; i++)
        {
            if(entries[i].mScore < currentScore)
            {
                return true;
            }
        }
        return false;
    }

}
