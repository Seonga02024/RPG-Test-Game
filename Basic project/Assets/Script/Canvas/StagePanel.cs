using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StagePanel : MonoBehaviour, PanelController
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private TextMeshProUGUI stageText;

    public void OnPanel(bool isAcitve){
        mainPanel.SetActive(isAcitve);
    }

    public void ClickUpgradeCharacterBtn(){
        //mainPanel.SetActive(false);
        SystemCanvas.Instance.GetStatUpPanel().OnSelectCharacterPanel(true);
    }

    public void ClickOffBtn(){
        mainPanel.SetActive(false);
        GameManager.Instance.StageStart();
    }
    
    public void ChangeStageText(int stage){
        stageText.text = "Stage " + stage.ToString();
    }
}
