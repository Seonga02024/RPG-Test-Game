using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoPanel : MonoBehaviour
{
    [SerializeField] private BarSystem hpBar;
    [SerializeField] private BarSystem expBar;
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

    public void RestoreHPAll(){
        hpBar.RestoreAll();
    }

    public void SettingHPMaxValue(float maxValue){
        hpBar.SettingMaxValue(maxValue);
    }

    public void DamgeEXP(float amount){
        expBar.Damage(amount);
    }

    public void RestoreEXP(float amount){
        expBar.Restore(amount);
    }

    public void SettingEXPValue(float value, float maxValue){
        expBar.SettingValueAndMaxValue(value, maxValue);
    }

    public void SettingEXPMaxValue(float maxValue){
        expBar.SettingMaxValue(maxValue);
    }

    public void ChangeLVText(int level){
        LvText.text = "LV " + level.ToString();
    }
}
