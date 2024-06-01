using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Character
{
    [SerializeField] public float health = 400;
    [SerializeField] public int attackPower = 5;
    [SerializeField] public float attackDistance = 1f;
    [SerializeField] public float attackCoolTime = 1.5f;
    [SerializeField] public float skillAttckDistance = 1f;
    [SerializeField] public float skillAttckCoolTime = 3f;
    [SerializeField] public float monsterBlackOutTime = 1f;
    [SerializeField] public CharacterInfoPanel characterInfoPanel;

    [SerializeField] public GameObject skillFXObj;

    public override void Init()
    {
        job = "Knight";

        _health = health;
        _attackPower = attackPower;
        _attackDistance = attackDistance;
        _attackCoolTime = attackCoolTime;
        _skillAttckDistance = skillAttckDistance;
        _skillAttckCoolTime = skillAttckCoolTime;
        _characterInfoPanel = characterInfoPanel;
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

        // Single close range attack skill (black out for 1 second with 100% attack damage to a single enemy within 1m range)
        targetMonster.TakeDamage(this, _attackPower);
        targetMonster.BlackOut(monsterBlackOutTime);

        // play sfx
        GameSoundManager.Instance.SfxPlay(SFX.SkillAttack_Knight);
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
