using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMonster : Monster
{
    [SerializeField] private float health = 100;
    [SerializeField] private float attackPower = 10;
    [SerializeField] private float attackNumber = 1;
    [SerializeField] private float attackDistance = 1;
    [SerializeField] private float exp = 10;
    [SerializeField] private int coin = 10;
    [SerializeField] private MonsterInfoPanel monsterInfoPanel;

    public override void Init()
    {
        _health = health;
        _attackPower = attackPower;
        _attackNumber = attackNumber;
        _attackDistance = attackDistance;
        _monsterInfoPanel = monsterInfoPanel;
        _monsterInfoPanel.SettingHPMaxValue(health);
        _exp = exp;
        _coin = coin;

        animator = GetComponentInChildren<Animator>();
    }

    public override void Reset()
    {
        _health = health;
        _attackPower = attackPower;
        _attackNumber = attackNumber;
        _attackDistance = attackDistance;
        _coin = coin;

        Upgrade();
        
        base.Reset();
    }

    public override void Upgrade()
    {
        // Set Monster Stat based on current stage
        int currentStage = GameManager.Instance.currentStage - 1;

        _health = health + (health * SettingGameData.EACH_STAGE_BASIC_MONSTER_UPGRADE_RATIO * currentStage);
        // setting hp ui
        _monsterInfoPanel.SettingHPMaxValue(_health);
        _attackPower = attackPower + (attackPower * SettingGameData.EACH_STAGE_BASIC_MONSTER_UPGRADE_RATIO * currentStage);
        _coin = (int)(coin + (coin * SettingGameData.EACH_STAGE_BASIC_MONSTER_UPGRADE_RATIO * currentStage));

        // level text change
        _monsterInfoPanel.ChangeLVText(GameManager.Instance.currentStage);
    }
}
