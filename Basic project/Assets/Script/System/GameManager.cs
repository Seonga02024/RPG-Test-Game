using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// State interface
public interface IGameState
{
    void EnterState(GameManager gameManager);
    void UpdateState(GameManager gameManager);
    void ExitState(GameManager gameManager);
}

// BattleState Class
public class BattleState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entering Battle State");
        gameManager.ShowStageInfo();
        //gameManager.StageStart();
    }

    public void UpdateState(GameManager gameManager)
    {
        gameManager.HandleMonsterSpawning();
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting Battle State");
        gameManager.StageEnd();
    }
}

// StoryState Class
public class StoryState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entering Story State");
        if(gameManager.currentStage == 7 || gameManager.currentStage == 15 || gameManager.currentStage == 25){
            SystemCanvas.Instance.GetStoryPanel().OnPanel(true);
            SystemCanvas.Instance.GetStoryPanel().ShowStory(gameManager.currentStage);
        }else{
            gameManager.ChangeState(gameManager.BattleState);
        }
    }

    public void UpdateState(GameManager gameManager)
    {
        // Add any logic for story updates if necessary
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting Story State");
    }
}

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

    private IGameState currentState;
    public IGameState BattleState { get; private set; }
    public IGameState StoryState { get; private set; }

    private void Start()
    {
        // Initialize states
        BattleState = new BattleState();
        StoryState = new StoryState();

        InitGame();
        // Set initial state to StoryState
        //ChangeState(StoryState);
    }

    private void Update()
    {
        // Delegate Update to the current state
        currentState?.UpdateState(this);
    }

    public void ChangeState(IGameState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    public void ShowStageInfo()
    {
        SystemCanvas.Instance.GetStagePanel().ChangeStageText(currentStage);
        SystemCanvas.Instance.GetStagePanel().OnPanel(true);
    }

    public void StageStart()
    {
        SettingStage();
        isStageStart = true;
    }

    public void StageEnd()
    {
        isStageStart = false;
        currentStage++;

        SaveLoadManager.Instance.SetCurrentStage(currentStage);
        SaveLoadManager.Instance.SaveData();

        ShowStageInfo();

        GameSoundManager.Instance.SfxPlay(SFX.Win);
    }

    public void GameOver()
    {
        isStageStart = false;
        ShowStageInfo();
        SaveLoadManager.Instance.SaveData();
        GameSoundManager.Instance.SfxPlay(SFX.Lose);
    }

    public void SettingCameraFollowObj(Transform targetTransform)
    {
        virtualCamera.Follow = targetTransform;
    }

    private void InitGame()
    {
        charactersController = FindObjectOfType<CharactersController>();
        monstersController = FindObjectOfType<MonstersController>();
        charactersController.InstantiateCharacters();
        CreateMonsters();
        SettingCameraFollowObj(charactersController.characters[0].transform);
        SettingGameData();
    }

    private void SettingGameData()
    {
        playerController.ChangeCoinData(SaveLoadManager.Instance.GetCoin());
        currentStage = SaveLoadManager.Instance.GetCurrentStage();
        isGameFirst = SaveLoadManager.Instance.GetIsGameFirst();
        allPlayCount = SaveLoadManager.Instance.GetPlayCount();

        charactersController.SettingCharactersData();
    }

    private void SettingStage()
    {
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

    public void HandleMonsterSpawning()
    {
        monsterSpawnTimer += Time.deltaTime;
        if (monsterSpawnTimer >= monsterSpawnInterval)
        {
            if (monstersController.GetAliveMonsterCount() < maxMonsters)
            {
                monstersController.SpawnMonster();
                monsterSpawnTimer = 0f;
            }
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) SaveLoadManager.Instance.SaveData();
    }

    public void OnBattleModeButtonPressed()
    {
        ChangeState(BattleState);
    }

    public void OnStoryModeButtonPressed()
    {
        ChangeState(StoryState);
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Cinemachine;

// public class GameManager : SingleTon<GameManager>
// {
//     [Header("[Basic Setting]")]
//     public CharactersController charactersController;
//     public MonstersController monstersController;
//     public PlayerController playerController;
//     [SerializeField] private CinemachineVirtualCamera virtualCamera;

//     [Header("[Game Setting]")]
//     public float monsterSpawnInterval = 5f;
//     public int maxMonsters = 5;
//     public float characterResponeTime = 5;

//     /* values */
//     private float monsterSpawnTimer = 0;
//     private bool isGameFirst = false;
//     private int allPlayCount = 0;
//     [HideInInspector] public bool isStageStart = false;
//     [HideInInspector] public int currentStage = 1;

//     /////////////////////////////////////////////////////////////////////////////////
//     ///
    
//     private void Start()
//     {
//         InitGame();
//     }

//     private void Update()
//     {
//         // if stage start, checking monsterSpawn;
//         if(isStageStart){
//             HandleMonsterSpawning();
//         }
//     }

//     public void ShowStageInfo(){
//         SystemCanvas.Instance.GetStagePanel().ChangeStageText(currentStage);
//         SystemCanvas.Instance.GetStagePanel().OnPanel(true);
//     }

//     public void StageStart(){
//         //Debug.Log("StageStart");
//         // setting stage
//         SettingStage();
//         isStageStart = true;
//     }

//     public void StageEnd(){
//         //Debug.Log("StageEnd");
//         // next stage
//         isStageStart = false;
//         currentStage++;
//         // save data
//         SaveLoadManager.Instance.SetCurrentStage(currentStage);
//         SaveLoadManager.Instance.SaveData();

//         ShowStageInfo();

//         // play sfx
//         GameSoundManager.Instance.SfxPlay(SFX.Win);
//     }

//     public void GameOver(){
//         //Debug.Log("GameOver");
//         // restart stage
//         isStageStart = false;
//         ShowStageInfo();

//         // save data
//         SaveLoadManager.Instance.SaveData();

//         // play sfx
//         GameSoundManager.Instance.SfxPlay(SFX.Lose);
//     }

//     public void SettingCameraFollowObj(Transform tragetTransform){
//         virtualCamera.Follow = tragetTransform;
//     }

//     /////////////////////////////////////////////////////////////////////////////////
//     ///
    
//     private void InitGame()
//     {
//         // Initialize character manager and monster manager
//         charactersController = FindObjectOfType<CharactersController>();
//         monstersController = FindObjectOfType<MonstersController>();

//         // create inital charaters
//         charactersController.InstantiateCharacters();
//         // Create initial monsters
//         CreateMonsters();
//         SettingCameraFollowObj(charactersController.characters[0].transform);

//         SettingGameData();
//     }

//     private void SettingGameData(){
//         playerController.ChangeCoinData(SaveLoadManager.Instance.GetCoin());
//         currentStage = SaveLoadManager.Instance.GetCurrentStage();
//         isGameFirst = SaveLoadManager.Instance.GetIsGameFirst();
//         allPlayCount = SaveLoadManager.Instance.GetPlayCount();

//         charactersController.SettingCharactersData();
//     }

//     private void SettingStage(){
//         charactersController.SettingStageCharacters();
//         monstersController.SettingStageMonster(currentStage);
//     }

//     private void CreateMonsters()
//     {
//         for (int i = 0; i < maxMonsters; i++)
//         {
//             monstersController.CreateMonster(i);
//         }

//         monstersController.CreateBossMonster();
//     }

//     private void HandleMonsterSpawning()
//     {
//         monsterSpawnTimer += Time.deltaTime;
//         if (monsterSpawnTimer >= monsterSpawnInterval)
//         {
//             if(monstersController.GetAliveMonsterCount() < maxMonsters){
//                 monstersController.SpawnMonster();
//                 monsterSpawnTimer = 0f;
//             }
//         }
//     }

//     private void OnApplicationPause(bool pauseStatus)
//     {
//         // if game background status
//         if(pauseStatus) SaveLoadManager.Instance.SaveData();
//     }
// }
