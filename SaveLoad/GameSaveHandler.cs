using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveHandler : MonoBehaviour
{
    [Header("SaveMagics")]
    private const string SAVE_SEPARATOR = "#SAVE-VALUE#";

    [Header("FieldsToSave")]
    [SerializeField]private SelectCharacters selectCharacters;
    [SerializeField] private MainMenuLevelHandler levelHandler;

    private void Awake()
    {
        InitSave();
    }
    public void LoadCheck()
    {
        //Application
        if (File.Exists(Application.persistentDataPath + "/saveFile.json"))
            Load();
        else
        {
            FirstSave();
        }
    }
    private void InitSave()    //ONLY USED BY SAVEHANDLER AWAKE TO AVOID LOADING WITH CHARACTER SCREEN NOT ACTIVE(PREVENTS NULL COROUTINES FIRING)
    {
        if (!File.Exists(Application.persistentDataPath + "/saveFile.json"))
            FirstSave();
    }
    private void FirstSave()
    {
        SaveData squadData = new SaveData
        {survivor1Name = "Starter"};

        string json = JsonUtility.ToJson(squadData);
        //File.WriteAllText(Application.dataPath + "/saveFile.json", json);
        File.WriteAllText(Application.persistentDataPath + "/saveFile.json", json);
    }
    public void SaveCharacters()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/saveFile.json");
        SaveData squadData = JsonUtility.FromJson<SaveData>(json);

        //SAVE SQUAD DATA
        squadData.squadSize = selectCharacters.currentSquadSize;

        squadData.survivor1Name = selectCharacters.currentSquad[0];
        squadData.survivor2Name = selectCharacters.currentSquad[1];
        squadData.survivor3Name = selectCharacters.currentSquad[2];
        squadData.survivor4Name = selectCharacters.currentSquad[3];

        json = JsonUtility.ToJson(squadData);
        //File.WriteAllText(Application.dataPath + "/saveFile.json", json);
        File.WriteAllText(Application.persistentDataPath + "/saveFile.json", json);
    }
    private void Load()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/saveFile.json");
        SaveData squadData = JsonUtility.FromJson<SaveData>(json);

        selectCharacters.currentSquadSize = squadData.squadSize;

        //UnlockCharacters
        if (squadData.survivor1Unlocked)
        {
            selectCharacters.charactersInRow[1].GetComponent<CharacterInformation>().UnlockCharacter();
            selectCharacters.charactersInRow[1].transform.parent.GetComponent<DragDrop>().isUnlocked = true;
        }
        if (squadData.survivor2Unlocked)
        {
            selectCharacters.charactersInRow[2].GetComponent<CharacterInformation>().UnlockCharacter();
            selectCharacters.charactersInRow[2].transform.parent.GetComponent<DragDrop>().isUnlocked = true;
        }
        if (squadData.survivor3Unlocked)
        {
            selectCharacters.charactersInRow[3].GetComponent<CharacterInformation>().UnlockCharacter();
            selectCharacters.charactersInRow[3].transform.parent.GetComponent<DragDrop>().isUnlocked = true;
        }
        if (squadData.survivor4Unlocked)
        {
            selectCharacters.charactersInRow[4].GetComponent<CharacterInformation>().UnlockCharacter();
            selectCharacters.charactersInRow[4].transform.parent.GetComponent<DragDrop>().isUnlocked = true;
        }
        if (squadData.survivor5Unlocked)
        {
            selectCharacters.charactersInRow[5].GetComponent<CharacterInformation>().UnlockCharacter();
            selectCharacters.charactersInRow[5].transform.parent.GetComponent<DragDrop>().isUnlocked = true;
        }
        if (squadData.survivor6Unlocked)
        {
            selectCharacters.charactersInRow[6].GetComponent<CharacterInformation>().UnlockCharacter();
            selectCharacters.charactersInRow[6].transform.parent.GetComponent<DragDrop>().isUnlocked = true;
        }
        if (squadData.survivor7Unlocked)
        {
            selectCharacters.charactersInRow[7].GetComponent<CharacterInformation>().UnlockCharacter();
            selectCharacters.charactersInRow[7].transform.parent.GetComponent<DragDrop>().isUnlocked = true;
        }
        if (squadData.survivor8Unlocked)
        {
            selectCharacters.charactersInRow[8].GetComponent<CharacterInformation>().UnlockCharacter();
            selectCharacters.charactersInRow[8].transform.parent.GetComponent<DragDrop>().isUnlocked = true;
        }
        if (squadData.survivor9Unlocked)
        {
            selectCharacters.charactersInRow[9].GetComponent<CharacterInformation>().UnlockCharacter();
            selectCharacters.charactersInRow[9].transform.parent.GetComponent<DragDrop>().isUnlocked = true;
        }
        if (squadData.survivor10Unlocked)
        {
            selectCharacters.charactersInRow[10].GetComponent<CharacterInformation>().UnlockCharacter();
            selectCharacters.charactersInRow[10].transform.parent.GetComponent<DragDrop>().isUnlocked = true;
        }
        if (squadData.survivor11Unlocked)
        {
            selectCharacters.charactersInRow[11].GetComponent<CharacterInformation>().UnlockCharacter();
            selectCharacters.charactersInRow[11].transform.parent.GetComponent<DragDrop>().isUnlocked = true;
        }

        //LoadSquad
        selectCharacters.LoadSquad(squadData.survivor1Name, squadData.survivor2Name, squadData.survivor3Name, squadData.survivor4Name);

        //Load Levels

    }
    
    private class SaveData
    {
        //SQUAD INFORMATION
        public int squadSize;

        public string survivor1Name;
        public string survivor2Name;
        public string survivor3Name;
        public string survivor4Name;

        public bool survivor1Unlocked;  //Butcher
        public bool survivor2Unlocked;  //Farmer
        public bool survivor3Unlocked;  //CoolGuy
        public bool survivor4Unlocked;  //Lumberjack
        public bool survivor5Unlocked;  //Nun
        public bool survivor6Unlocked;  //Cop
        public bool survivor7Unlocked;  //Punk
        public bool survivor8Unlocked;  //Red
        public bool survivor9Unlocked;  //CowGal
        public bool survivor10Unlocked; //Ooma
        public bool survivor11Unlocked; //Agent

        //LEVEL INFORMATION
        //0 = locked 1=unlocked but unplayed 2,3,4= bronze silver gold
        public int lvl1, lvl2, lvl3, lvl4, lvl5, lvl6, lvl7, lvl8, lvl9, lvl10, lvl11, lvl12, lvl13, lvl14, lvl15, lvl16, lvl17, lvl18, lvl19, lvl20;
    }

    public List<int> GiveLevelInts()
    {
        List<int> levelInts = new List<int>();
        string json = File.ReadAllText(Application.persistentDataPath + "/saveFile.json");
        SaveData levelData = JsonUtility.FromJson<SaveData>(json);

        levelInts.Add(levelData.lvl1);
        levelInts.Add(levelData.lvl2);
        levelInts.Add(levelData.lvl3);
        levelInts.Add(levelData.lvl4);
        levelInts.Add(levelData.lvl5);
        levelInts.Add(levelData.lvl6);
        levelInts.Add(levelData.lvl7);
        levelInts.Add(levelData.lvl8);
        levelInts.Add(levelData.lvl9);
        levelInts.Add(levelData.lvl10);
        levelInts.Add(levelData.lvl11);
        levelInts.Add(levelData.lvl12);
        levelInts.Add(levelData.lvl13);
        levelInts.Add(levelData.lvl14);
        levelInts.Add(levelData.lvl15);
        levelInts.Add(levelData.lvl16);
        levelInts.Add(levelData.lvl17);
        levelInts.Add(levelData.lvl18);
        levelInts.Add(levelData.lvl19);
        levelInts.Add(levelData.lvl20);

        return levelInts;
    }
    public void UnlockAll()
    {
        SaveData squadData = new SaveData();
        //DEBUG USE ONLY
        squadData.survivor1Unlocked = true;
        squadData.survivor2Unlocked = true;
        squadData.survivor3Unlocked = true;
        squadData.survivor4Unlocked = true;
        squadData.survivor5Unlocked = true;
        squadData.survivor6Unlocked = true;
        squadData.survivor7Unlocked = true;
        squadData.survivor8Unlocked = true;
        squadData.survivor9Unlocked = true;
        squadData.survivor10Unlocked = true;
        squadData.survivor11Unlocked = true;

        string json = JsonUtility.ToJson(squadData);
        File.WriteAllText(Application.persistentDataPath + "/saveFile.json", json);
    }
}
