using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : Character
{
    [SerializeField] public float health = 100;
    [SerializeField] public int attackPower = 10;
    [SerializeField] public float attackDistance = 4f;
    [SerializeField] public float attackCoolTime = 1f;
    [SerializeField] public float skillAttckDistance = 4f;
    [SerializeField] public float skillAttckCoolTime = 6f;
    [SerializeField] public CharacterInfoPanel characterInfoPanel;

    [SerializeField] public GameObject skillFXObj;

    public override void Init()
    {
        job = "Priest";

        _health = health;
        _attackPower = attackPower;
        _attackDistance = attackDistance;
        _attackCoolTime = attackCoolTime;
        _skillAttckDistance = skillAttckDistance;
        _skillAttckCoolTime = skillAttckCoolTime;

        _characterInfoPanel = characterInfoPanel;
        _characterInfoPanel.SettingHPMaxValue(health);
        _characterInfoPanel.SettingEXPValue(0, 100);

        animator = GetComponent<Animator>();
    }

    public override void Reset()
    {
        _health = health;
        _attackPower = attackPower;
        _attackDistance = attackDistance;
        _attackCoolTime = attackCoolTime;
        _skillAttckDistance = skillAttckDistance;
        _skillAttckCoolTime = skillAttckCoolTime;

        Upgrade();
        
        base.Reset();
    }

    public override void EachSkillAttack()
    {
        skillFXObj.SetActive(true);
        Invoke("offFX", 1.0f);
        
        // Single distance healing skill (restores the strength of a single ally within a range of 4m by 250% of its attack)
        // find distance condition is true and loss hp and most less hp character
        Character needHealCharacter = null;
        float currentHealCharacterHP = float.MaxValue;

        if(GameManager.Instance.charactersController.characters.Count > 0){
            foreach (var character in GameManager.Instance.charactersController.characters)
            {
                if(character.isAlive){
                    if (Vector3.Distance(transform.position, character.transform.position) < _skillAttckDistance)
                    {
                        float lossHP = character.GetUpgradeHP() - character._health;
                        if(lossHP > 0 && character._health < currentHealCharacterHP){
                            needHealCharacter = character;
                            currentHealCharacterHP = character._health;
                        }
                    }
                }
            }
        }

        if(needHealCharacter != null){
            needHealCharacter.RestoreHP(attackPower * 2.5f);
            //Debug.Log("priest heal " + needHealCharacter.job);
        }

        // play sfx
        GameSoundManager.Instance.SfxPlay(SFX.SkillAttack_Priest);
    }

    private void offFX()
    {
        skillFXObj.SetActive(false);
    }

    public override void Upgrade()
    {
        _health = health + (_hpUpgradeLevel * SettingGameData.EACH_UPGRADE_CHARACTER_HP_NUM) + level-1;
        _characterInfoPanel.SettingHPMaxValue(_health);
        _attackPower = attackPower + (_attackUpgradeLevel * SettingGameData.EACH_UPGRADE_CHARACTER_ATTACK_NUM) + level-1;
    }

    public override float GetUpgradeHP(){
        return health + (_hpUpgradeLevel * SettingGameData.EACH_UPGRADE_CHARACTER_HP_NUM) + level-1;
    }

    public override float GetUpgradeAttack(){
        return attackPower + (_attackUpgradeLevel * SettingGameData.EACH_UPGRADE_CHARACTER_ATTACK_NUM) + level-1;
    }
}