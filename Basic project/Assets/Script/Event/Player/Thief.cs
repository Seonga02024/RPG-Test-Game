using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Character
{
    [SerializeField] public float health = 100;
    [SerializeField] public int attackPower = 10;
    [SerializeField] public float attackDistance = 1f;
    [SerializeField] public float attackCoolTime = 1f;
    [SerializeField] public float skillAttckDistance = 2f;
    [SerializeField] public float skillAttckCoolTime = 3f;
    [SerializeField] public CharacterInfoPanel characterInfoPanel;

    [SerializeField] public GameObject skillFXObj;

    public override void Init()
    {
        job = "Thief";

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

        // Range attack skill (damages all enemies 2m around by 100% attack power)
        if(GameManager.Instance.monstersController.spawnedMonsters.Count > 0){
            foreach (var monster in GameManager.Instance.monstersController.spawnedMonsters)
            {
                if(monster.isAlive){
                    float distanceToMonster = Vector3.Distance(transform.position, monster.transform.position);
                    if (distanceToMonster < _skillAttckDistance)
                    {
                        monster.TakeDamage(this, _attackPower);
                    }
                }
            }
        }

        // play sfx
        GameSoundManager.Instance.SfxPlay(SFX.SkillAttack_Thief);
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
