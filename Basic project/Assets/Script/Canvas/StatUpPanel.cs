using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUpPanel : MonoBehaviour, PanelController
{
    [Header("Basic Settings")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject selectCharacterPanel;
    [SerializeField] private GameObject upgradeCharacterPanel;

    [Header("upgrade Character Panel Setting")]
    [SerializeField] private TextMeshProUGUI hpStatText;
    [SerializeField] private TextMeshProUGUI attackStatText;
    [SerializeField] private TextMeshProUGUI hpUpgradeCoinText;
    [SerializeField] private TextMeshProUGUI attackUpgradeCoinText;
    [SerializeField] private TextMeshProUGUI CharacterNameText;

    /* values */
    private int currentHPUpgradeLevel = 0;
    private int currentAttackUpgradeLevel = 0;
    private int currentHPUpgradCoin = 0;
    private int currentAttackUpgradCoin = 0;
    private Character currentSelectCharacter = null;

    /////////////////////////////////////////////////////////////////////////////////
    /// select character paenl
    
    public void OnSelectCharacterPanel(bool isAcitve){
        OnPanel(true);
        selectCharacterPanel.SetActive(isAcitve);
    }

    public void SelectCharacter(int num){
        if(GameManager.Instance.charactersController.characters.Length <= num) return;

        // setting select character data
        SettingUpgradeCharacterPanel(GameManager.Instance.charactersController.characters[num]);
        OnUpgradeCharacterPanel(true);

        // play sfx
        GameSoundManager.Instance.SfxPlay(SFX.SelectUI);
    }

    public void OnPanel(bool isAcitve){
        mainPanel.SetActive(isAcitve);
    }

    /////////////////////////////////////////////////////////////////////////////////
    /// upgrade Character Panel 
    // checking buy and upgrade
    
    public void UpgradeHP(){
        if(currentSelectCharacter == null) return;
        if(currentHPUpgradCoin <= GameManager.Instance.playerController.Coin){
            GameManager.Instance.playerController.MinusCoin(currentHPUpgradCoin);
            currentHPUpgradeLevel++;
            currentSelectCharacter.UpgradeHPLevel(currentHPUpgradeLevel);
            SettingUpgradeCharacterPanel(currentSelectCharacter);
        }
    }

    public void UpgradeAttack(){
        if(currentSelectCharacter == null) return;
        if(currentAttackUpgradCoin <= GameManager.Instance.playerController.Coin){
            GameManager.Instance.playerController.MinusCoin(currentHPUpgradCoin);
            currentAttackUpgradeLevel++;
            currentSelectCharacter.UpgradeAttackLevel(currentAttackUpgradeLevel);
            SettingUpgradeCharacterPanel(currentSelectCharacter);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    /// character ui data setting

    private void OnUpgradeCharacterPanel(bool isAcitve){
        upgradeCharacterPanel.SetActive(isAcitve);
    }

    private void SettingUpgradeCharacterPanel(Character character){
        currentSelectCharacter = character;
         
        CharacterNameText.text = character.job;

        hpStatText.text = "Current HP : " + character.GetUpgradeHP();
        attackStatText.text = "Current Attack : " + character.GetUpgradeAttack();

        currentHPUpgradeLevel = character._hpUpgradeLevel;
        currentAttackUpgradeLevel = character._attackUpgradeLevel;

        currentHPUpgradCoin = (int)(SettingGameData.EACH_UPGRADE_CHARACTER_HP_BASIC_COIN + 
        (SettingGameData.EACH_UPGRADE_CHARACTER_HP_BASIC_COIN * currentHPUpgradeLevel * SettingGameData.EACH_UPGRADE_CHARACTER_HP_COIN_RATIO));

        currentAttackUpgradCoin = (int)(SettingGameData.EACH_UPGRADE_CHARACTER_ATTACK_BASIC_COIN + 
        (SettingGameData.EACH_UPGRADE_CHARACTER_ATTACK_BASIC_COIN * currentAttackUpgradeLevel * SettingGameData.EACH_UPGRADE_CHARACTER_ATTACK_COIN_RATIO));
        
        hpUpgradeCoinText.text = currentHPUpgradCoin.ToString();
        attackUpgradeCoinText.text = currentAttackUpgradCoin.ToString();
    }
}
