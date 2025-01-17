using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class CharacterInfo
{
    public int characterID = -1;
    public bool isUse = false;
    public bool isBuy = false;
    public string Job = "None";
    public int Level = 1;
    public int HpUpgradeLevel = 0;
    public int AttackUpgradeLevel = 0;
}

[System.Serializable]
public class GameData
{
    public int currentStage;
    public int coin;
    public bool isGameFirst;
    public float masterSound;
    public float bgmSound;
    public float sfxSound;
    public int playCount;
    public List<CharacterInfo> charactersInfo;
    //private string[] jobList = {"Knight", "Thief", "Archer", "Priest"};


    public GameData()
    {
        currentStage = 1;
        coin = 0;
        isGameFirst = false;
        masterSound = 1;
        bgmSound = 1;
        sfxSound = 1;
        playCount = 0;

        // 4개의 캐릭터
        charactersInfo = new List<CharacterInfo>();
        for (int i = 0; i < 4; i++){
            //charactersInfo.Add(new CharacterInfo{ Job = jobList[i] });
            charactersInfo.Add(new CharacterInfo{ characterID = i });
        }
    }
}

public class SaveLoadManager : SingleTon<SaveLoadManager>
{
    private string savePath;
    private GameData _gameData;

    public GameData GameData
    {
        get
        {
            if(_gameData == null)
            {
                _gameData = LoadData();
                SaveData();
            }

            return _gameData;
        }
    }

    private void Start() 
    {
        savePath = Path.Combine(Application.persistentDataPath, "GameData.json");
        Debug.Log(Application.persistentDataPath);
    }

    private GameData LoadData()
    {
        //Debug.Log(savePath);
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(jsonData);
            return loadedData;
        }
        else
        {
            //Debug.Log("create new data file");
            GameData gameData = new GameData();

            return gameData;
        }
    }

    public void SetCurrentStage(int currentStage) { GameData.currentStage = currentStage; } 
    public int GetCurrentStage() { return GameData.currentStage; }

    public void SetCoin(int coin) { GameData.coin = coin; } 
    public int GetCoin() { return GameData.coin; }

    public bool GetIsGameFirst() { return GameData.isGameFirst; }
    public void SetIsGameFirst() { GameData.isGameFirst = true; } 

    public void SetMasterSound(float sound) { GameData.masterSound = sound; } 
    public float GetMasterSound() { return GameData.masterSound; } 

    public void SetBgmSound(float sound) { GameData.bgmSound = sound; } 
    public float GetBgmSound() { return GameData.bgmSound; } 

    public void SetSfxSound(float sound) { GameData.sfxSound = sound; } 
    public float GetSfxSound() { return GameData.sfxSound; } 

    public void SetPlayCount(int value) { GameData.playCount = value; } 
    public int GetPlayCount() { return GameData.playCount; } 

    // public CharacterInfo GetCharacterInfo(string jobName) { return FindCharacterInfo(jobName); } 
    // public void SetCharacterInfoLevel(string jobName, int value) { FindCharacterInfo(jobName).Level = value; }
    // public void SetCharacterInfoHpUpgradeLevel(string jobName, int value) { FindCharacterInfo(jobName).HpUpgradeLevel = value; }
    // public void SetCharacterInfoAttackUpgradeLevel(string jobName, int value) { FindCharacterInfo(jobName).AttackUpgradeLevel = value; }
    // private CharacterInfo FindCharacterInfo(string jobName){
    //     foreach( CharacterInfo characterInfo in GameData.charactersInfo){
    //         if(characterInfo.Job == jobName){
    //             return characterInfo;
    //         }
    //     }
    //     return null;
    // }

    public CharacterInfo GetCharacterInfo(int characterID) { return FindCharacterInfo(characterID); } 
    public void SetCharacterInfoLevel(int characterID, int value) { FindCharacterInfo(characterID).Level = value; }
    public void SetCharacterInfoHpUpgradeLevel(int characterID, int value) { FindCharacterInfo(characterID).HpUpgradeLevel = value; }
    public void SetCharacterInfoAttackUpgradeLevel(int characterID, int value) { FindCharacterInfo(characterID).AttackUpgradeLevel = value; }
    private CharacterInfo FindCharacterInfo(int characterID){
        foreach( CharacterInfo characterInfo in GameData.charactersInfo){
            if(characterInfo.characterID == characterID){
                return characterInfo;
            }
        }
        return null;
    }

    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(_gameData);
        File.WriteAllText(savePath, jsonData);
        //Debug.Log("save data");
    }

    void OnApplicationQuit()
    {
        SaveData();
    }
}
