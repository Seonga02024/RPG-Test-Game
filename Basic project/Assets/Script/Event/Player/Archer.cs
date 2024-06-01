using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Character
{
    [SerializeField] public float health = 100;
    [SerializeField] public int attackPower = 15;
    [SerializeField] public float attackDistance = 4f;
    [SerializeField] public float attackCoolTime = 1f;
    [SerializeField] public float skillAttckDistance = 4f;
    [SerializeField] public float skillAttckCoolTime = 4f;
    [SerializeField] public CharacterInfoPanel characterInfoPanel;

    [SerializeField] public GameObject skillFXObj;

    public override void Init()
    {
        job = "Archer";

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

        // Single long-range attack skill (damages a single target within a range of 4m by 250% attack)
        targetMonster.TakeDamage(this, _attackPower * 2.5f);

        // play sfx
        GameSoundManager.Instance.SfxPlay(SFX.SkillAttack_Archer);
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
