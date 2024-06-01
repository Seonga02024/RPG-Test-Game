using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : SingleTon<GameManager>
{
    [Header("[Basic Setting]")]
    public CharactersController charactersController;
    public MonstersController monstersController;
    public PlayerController playerController;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("[Game Setting]")]
    public float monsterSpawnInterval = 5f;
    public int maxMonsters = 5;
    public float characterResponeTime = 5;

    /* values */
    private float monsterSpawnTimer = 0;
    private bool isGameFirst = false;
    private int allPlayCount = 0;
    [HideInInspector] public bool isStageStart = false;
    [HideInInspector] public int currentStage = 1;

    /////////////////////////////////////////////////////////////////////////////////
    ///
    
    private void Start()
    {
        InitGame();
    }

    private void Update()
    {
        // if stage start, checking monsterSpawn;
        if(isStageStart){
            HandleMonsterSpawning();
        }
    }

    public void ShowStageInfo(){
        SystemCanvas.Instance.GetStagePanel().ChangeStageText(currentStage);
        SystemCanvas.Instance.GetStagePanel().OnPanel(true);
    }

    public void StageStart(){
        //Debug.Log("StageStart");
        // setting stage
        SettingStage();
        isStageStart = true;
    }

    public void StageEnd(){
        //Debug.Log("StageEnd");
        // next stage
        isStageStart = false;
        currentStage++;
        // save data
        SaveLoadManager.Instance.SetCurrentStage(currentStage);
        SaveLoadManager.Instance.SaveData();

        ShowStageInfo();

        // play sfx
        GameSoundManager.Instance.SfxPlay(SFX.Win);
    }

    public void GameOver(){
        //Debug.Log("GameOver");
        // restart stage
        isStageStart = false;
        ShowStageInfo();

        // save data
        SaveLoadManager.Instance.SaveData();

        // play sfx
        GameSoundManager.Instance.SfxPlay(SFX.Lose);
    }

    public void SettingCameraFollowObj(Transform tragetTransform){
        virtualCamera.Follow = tragetTransform;
    }

    /////////////////////////////////////////////////////////////////////////////////
    ///
    
    private void InitGame()
    {
        // Initialize character manager and monster manager
        charactersController = FindObjectOfType<CharactersController>();
        monstersController = FindObjectOfType<MonstersController>();

        // create inital charaters
        charactersController.InstantiateCharacters();
        // Create initial monsters
        CreateMonsters();
        SettingCameraFollowObj(charactersController.characters[0].transform);

        SettingGameData();
    }

    private void SettingGameData(){
        playerController.ChangeCoinData(SaveLoadManager.Instance.GetCoin());
        currentStage = SaveLoadManager.Instance.GetCurrentStage();
        isGameFirst = SaveLoadManager.Instance.GetIsGameFirst();
        allPlayCount = SaveLoadManager.Instance.GetPlayCount();

        charactersController.SettingCharactersData();
    }

    private void SettingStage(){
        charactersController.SettingStageCharacters();
        monstersController.SettingStageMonster(currentStage);
    }

    private void CreateMonsters()
    {
        for (int i = 0; i < maxMonsters; i++)
        {
            monstersController.CreateMonster(i);
        }

        monstersController.CreateBossMonster();
    }

    private void HandleMonsterSpawning()
    {
        monsterSpawnTimer += Time.deltaTime;
        if (monsterSpawnTimer >= monsterSpawnInterval)
        {
            if(monstersController.GetAliveMonsterCount() < maxMonsters){
                monstersController.SpawnMonster();
                monsterSpawnTimer = 0f;
            }
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        // if game background status
        if(pauseStatus) SaveLoadManager.Instance.SaveData();
    }
}
