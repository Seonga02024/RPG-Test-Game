using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersController : MonoBehaviour
{
    [SerializeField] private Monster monsterPrefab; 
    [SerializeField] private Monster bossMonsterPrefab; 
    [SerializeField] private float spwanRandomRange = 10f; 
   
    // current monsters list
    [SerializeField] public readonly List<Monster> spawnedMonsters = new List<Monster>();

    /* values */
    private int firstStageMaxNormalMonsterCount = 5;
    private int noramlMonsterMaxNum = 0; 
    private int countMonsterSpawn = 0;
    [SerializeField] private int currentStageDieMonsterCount = 0; 
    // current Stage Max Normal Monster Count -> After meeting the conditions, boss monsters appear
    private int currentStageMaxNormalMonsterCount = 0; 
    private bool isBossMonsterSpawn = false;

    /////////////////////////////////////////////////////////////////////////////////
    ///
    
    // each stage setting about monsters 
    public void SettingStageMonster(int currentStage){
        // reset monster and spawn
        foreach(Monster monster in spawnedMonsters){
            if(monster.index < noramlMonsterMaxNum){
                monster.gameObject.transform.position = SettingRandomSpawnPos();
                monster.Reset();
            }
        }
        // off boss monster
        spawnedMonsters[spawnedMonsters.Count - 1].isAlive = false;
        spawnedMonsters[spawnedMonsters.Count - 1].gameObject.SetActive(false);

        // setting current stage monster condition
        currentStageMaxNormalMonsterCount = firstStageMaxNormalMonsterCount + currentStage * 5;
        countMonsterSpawn = noramlMonsterMaxNum;
        SystemCanvas.Instance.GetPlayerInfoPanel().ChangeKillMonsterCountText(0);
        isBossMonsterSpawn = false;
        currentStageDieMonsterCount = 0;

        //Debug.Log("SettingStageMonster currentStageMaxNormalMonsterCount : " + currentStageMaxNormalMonsterCount);
    }

    // returns the number of live monsters
    public int GetAliveMonsterCount()
    {
        int count = 0;
        foreach(Monster monster in spawnedMonsters){
            if(monster.isAlive == true){
                count++;
            }
        }
        return count;
    }

    public void CountDieMonster(){
        currentStageDieMonsterCount++;
        SystemCanvas.Instance.GetPlayerInfoPanel().ChangeKillMonsterCountText(currentStageDieMonsterCount);

        // If the boss monster is dead
        if(isBossMonsterSpawn && GetAliveMonsterCount() == 0){
            // Stage End
            GameManager.Instance.StageEnd();
        }
        // If all the normal monsters are dead && If the boss monster did not appear
        else if(currentStageDieMonsterCount == currentStageMaxNormalMonsterCount && isBossMonsterSpawn == false){
            //Debug.Log("spawn Boss Monster");

            if (spawnedMonsters.Count > 0)
            {
                Monster bossMonster = spawnedMonsters[spawnedMonsters.Count - 1];
                bossMonster.gameObject.SetActive(true);
                bossMonster.gameObject.transform.position = SettingRandomSpawnPos();
                bossMonster.Reset();
                isBossMonsterSpawn = true;
            }
        }
    }

    public void SpawnMonster()
    {
        if(countMonsterSpawn < currentStageMaxNormalMonsterCount){
            foreach(Monster monster in spawnedMonsters){
                // if monster is alive , is the basic monster => spawn
                // list last monster is boss monster
                if(monster.isAlive == false && monster.index < noramlMonsterMaxNum){
                    monster.gameObject.transform.position = SettingRandomSpawnPos();
                    monster.Reset();
                    countMonsterSpawn++;
                    return;
                }
            }
        }
    }

    // Game setting for the first time only
    public void CreateMonster(int index)
    {
        // Create Monster in a Random Location
        Monster newMonster = Instantiate(monsterPrefab, SettingRandomSpawnPos(), Quaternion.identity);
        
        // Add created monsters to the list
        spawnedMonsters.Add(newMonster);
        newMonster.Init();
        newMonster.SettingIndex(index);

        noramlMonsterMaxNum++;
    }

    // Game setting for the first time only
    public void CreateBossMonster(){
        Monster newMonster = Instantiate(bossMonsterPrefab, SettingRandomSpawnPos(), Quaternion.identity);
        spawnedMonsters.Add(newMonster);
        newMonster.Init();
        newMonster.SettingIndex(noramlMonsterMaxNum);
        newMonster.isAlive = false;
        newMonster.gameObject.SetActive(false);
    }

    /////////////////////////////////////////////////////////////////////////////////
    ///

    private Vector3 SettingRandomSpawnPos(){
        // Create a random location within a moveRange around a character 1 location
        Vector2 newPos2 = ((Vector2)GameManager.Instance.charactersController.characters[0].transform.position + Random.insideUnitCircle * spwanRandomRange);
        Vector3 newPos3 = new Vector3(newPos2.x, newPos2.y, 0);
        return newPos3;
    }
}
