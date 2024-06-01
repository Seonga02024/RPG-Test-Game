using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersController : MonoBehaviour
{
    [Header("[charater jobs]")]
    [SerializeField] private Character knightPrefab;
    [SerializeField] private Character archerPrefab;
    [SerializeField] private Character priestPrefab;
    [SerializeField] private Character thiefPrefab;

    [Header("[charater slot points]")]
    [SerializeField] private Transform[] slotPoints; 

    public Character[] characters; // current charaters

    /////////////////////////////////////////////////////////////////////////////////
    ///

    public void SettingStageCharacters()
    {
        int index = 0;
        foreach(Character character in characters){
            character.SetPosition(slotPoints[index].position);
            character.Reset();
            index++;
        }
    }

    public void SettingCharactersData(){
        foreach(Character character in characters){
            CharacterInfo charactersInfo = SaveLoadManager.Instance.GetCharacterInfo(character.job);
            character.level = charactersInfo.Level;
            character._hpUpgradeLevel = charactersInfo.HpUpgradeLevel;
            character._attackUpgradeLevel = charactersInfo.AttackUpgradeLevel;
        }
    }

    public void InstantiateCharacters()
    {
        characters = new Character[4];

        // test code
        // characters = new Character[2];
        // characters[0] = InstantiateCharacter(knightPrefab, "Knight", 0);
        // characters[1] = InstantiateCharacter(archerPrefab, "Archer", 1);
        //characters[0] = InstantiateCharacter(priestPrefab, "Priest", 2);
        //characters[0] = InstantiateCharacter(thiefPrefab, "Thief", 3);

        // Instantiate and initialize characters
        characters[0] = InstantiateCharacter(knightPrefab, "Knight", 0);
        characters[1] = InstantiateCharacter(thiefPrefab, "Thief", 1); 
        characters[2] = InstantiateCharacter(archerPrefab, "Archer", 2);
        characters[3] = InstantiateCharacter(priestPrefab, "Priest", 3);
    }

    public void CheckAllCharacterDie(){
        bool isAllDie = true;

        foreach (Character character in characters){
            if(character.isAlive){
                isAllDie = false;
                break;
            }
        }

        if(isAllDie){
            GameManager.Instance.GameOver();
        }
    }
    
    /////////////////////////////////////////////////////////////////////////////////
    ///

    private Character InstantiateCharacter(Character prefab, string job, int index)
    {
        Character character = Instantiate(prefab);
        character.SetPosition(slotPoints[index].position);
        character.Init();
        character.job = job;
        return character;
    }
}
