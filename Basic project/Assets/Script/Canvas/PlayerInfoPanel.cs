using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoPanel : MonoBehaviour, PanelController
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI killMonsterCountText;
    [SerializeField] private TextMeshProUGUI timeScaleText;
    
    public void OnPanel(bool isAcitve){
        mainPanel.SetActive(isAcitve);
    }

    public void ChangeCoinText(int coin){
        coinText.text = "Coin : " + coin.ToString();
    }

    public void ChangeKillMonsterCountText(int num){
        killMonsterCountText.text = "Kill Monster Count : " + num.ToString();
    }

    public void ClickChangeTimeScaleBtn(){
        if(Time.timeScale == 1) Time.timeScale = 2;
        else Time.timeScale = 1;
        timeScaleText.text = "X" + Time.timeScale.ToString();
    }

    public void ClickChangeCameraFollowObjBtn(int num){
        if(GameManager.Instance.charactersController.characters.Length <= num) return;
        GameManager.Instance.SettingCameraFollowObj(GameManager.Instance.charactersController.characters[num].transform);
    }

    public void ClickSettingBtn(){
        SystemCanvas.Instance.GetSettingPanel().OnPanel(true);
    }
}
