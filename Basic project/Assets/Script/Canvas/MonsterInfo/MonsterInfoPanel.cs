using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterInfoPanel : MonoBehaviour
{
    [SerializeField] private BarSystem hpBar;
    [SerializeField] private TextMeshProUGUI LvText;

    public void DamgeHP(float amount){
        hpBar.Damage(amount);
    }

    public void RestoreHP(float amount){
        hpBar.Restore(amount);
    }

    public void ZeroHP(){
        hpBar.ZeroValue();
    }

    public void SettingHPMaxValue(float maxValue){
        hpBar.SettingMaxValue(maxValue);
    }

    public void RestoreHPAll(){
        hpBar.RestoreAll();
    }

    public void ChangeLVText(int level){
        LvText.text = "LV " + level.ToString();
    }
}
