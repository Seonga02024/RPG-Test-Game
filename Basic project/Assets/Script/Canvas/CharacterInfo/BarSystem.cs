using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarSystem : MonoBehaviour
{
    [Header("Basic Settings")]
    private float _value = 100;
    private float valueLerpTimer;
    private float _maxValue = 100;
    private float valueSpeed = 2;
    [SerializeField] private Image backBarImg;
    [SerializeField] private Image middleBarImg;
    [SerializeField] private Image frontBarImg;

    void Update()
    {
        _value = Mathf.Clamp(_value, 0, _maxValue);
        UpdateUI();
    }

    /////////////////////////////////////////////////////////////////////////////////
    ///

    public void Restore(float amount)
    {
        //Debug.Log("Restore");
        _value += amount;
        if(_value > _maxValue) _value = _maxValue;
        valueLerpTimer = 0;
    }

    public void Damage(float damage)
    {
        //Debug.Log("Damage");
        _value -= damage;
        if(_value <= 0) _value = 0;
        valueLerpTimer = 0;
    }

    // full restore
    public void RestoreAll(){
        _value = _maxValue;
        valueLerpTimer = 0;
    }

    public void SettingMaxValue(float maxValue){
        _maxValue = maxValue;
        _value = maxValue;
        frontBarImg.fillAmount = 1;
        middleBarImg.fillAmount = 1;
    }

    public void SettingValueAndMaxValue(float Value, float maxValue){
        _value = Value;
        _maxValue = maxValue;
    }

    public void ZeroValue(){
        _value = 0;
    }
    
    /////////////////////////////////////////////////////////////////////////////////
    ///

    private void UpdateUI()
    {
        float fillF = frontBarImg.fillAmount;
        float fillB = middleBarImg.fillAmount;
        float hFraction = _value / _maxValue;

        if (fillB > hFraction)
        {
            frontBarImg.fillAmount = hFraction;
            valueLerpTimer += Time.deltaTime;
            float percentComplete = valueLerpTimer / valueSpeed;
            percentComplete = percentComplete * percentComplete;
            middleBarImg.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            middleBarImg.fillAmount = hFraction;
            valueLerpTimer += Time.deltaTime;
            float percentComplete = valueLerpTimer / valueSpeed;
            percentComplete = percentComplete * percentComplete;
            frontBarImg.fillAmount = Mathf.Lerp(fillF, middleBarImg.fillAmount, percentComplete);
        }
    }
}
