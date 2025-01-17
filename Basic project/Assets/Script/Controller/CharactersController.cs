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

    //public Character[] characters; // current charaters
    public List<Character> characters = new List<Character>(); // current charaters

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
            Debug.Log(character.id);
            CharacterInfo charactersInfo = SaveLoadManager.Instance.GetCharacterInfo(character.id);
            character.id = charactersInfo.characterID;
            character.level = charactersInfo.Level;
            character._hpUpgradeLevel = charactersInfo.HpUpgradeLevel;
            character._attackUpgradeLevel = charactersInfo.AttackUpgradeLevel;
        }
    }

    public void InstantiateCharacters()
    {
        // characters = new Character[4];

        // test code
        // characters = new Character[2];
        // characters[0] = InstantiateCharacter(knightPrefab, "Knight", 0);
        // characters[1] = InstantiateCharacter(archerPrefab, "Archer", 1);
        //characters[0] = InstantiateCharacter(priestPrefab, "Priest", 2);
        //characters[0] = InstantiateCharacter(thiefPrefab, "Thief", 3);

        // Instantiate and initialize characters
        // characters[0] = InstantiateCharacter(knightPrefab, "Knight", 0);
        // characters[1] = InstantiateCharacter(thiefPrefab, "Thief", 1); 
        // characters[2] = InstantiateCharacter(archerPrefab, "Archer", 2);
        // characters[3] = InstantiateCharacter(priestPrefab, "Priest", 3);

        for (int i = 0; i < 4; i++){
            CharacterInfo charactersInfo = SaveLoadManager.Instance.GetCharacterInfo(i);
            Character character;
            if(charactersInfo.Job == "Knight"){
                character = InstantiateCharacter(knightPrefab, "Knight", i);
                characters.Add(character);
            }
            else if(charactersInfo.Job == "Thief"){
                character = InstantiateCharacter(thiefPrefab, "Thief", i); 
                characters.Add(character);
            }
            else if(charactersInfo.Job == "Archer"){
                character = InstantiateCharacter(archerPrefab, "Archer", i);
                characters.Add(character);
            }
            else if(charactersInfo.Job == "Priest"){
                character = InstantiateCharacter(priestPrefab, "Priest", i);
                characters.Add(character);
            }
        }

        Debug.Log("characters.Count : " + characters.Count);
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
        character.id = index;
        return character;
    }
}
