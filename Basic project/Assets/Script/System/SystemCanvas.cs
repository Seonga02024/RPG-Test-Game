using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCanvas : SingleTon<SystemCanvas>
{
    [SerializeField] private PlayerInfoPanel playerInfoPanel;
    [SerializeField] private StatUpPanel statUpPanel;
    [SerializeField] private TitlePanel titlePanel;
    [SerializeField] private StagePanel stagePanel;
    [SerializeField] private SettingPanel settingPanel;

    private List<PanelController> panelControllers;

    private void Start()
    {
        panelControllers = new List<PanelController>();
        panelControllers.Add(playerInfoPanel.GetComponent<PanelController>());
        panelControllers.Add(statUpPanel.GetComponent<PanelController>());
        panelControllers.Add(titlePanel.GetComponent<PanelController>());
        panelControllers.Add(stagePanel.GetComponent<PanelController>());
        panelControllers.Add(settingPanel.GetComponent<PanelController>());
    }

    public PlayerInfoPanel GetPlayerInfoPanel(){
        return playerInfoPanel;
    }

    public StatUpPanel GetStatUpPanel(){
        return statUpPanel;
    }

    public TitlePanel GetTitlePanel(){
        return titlePanel;
    }

    public StagePanel GetStagePanel(){
        return stagePanel;
    }

    public SettingPanel GetSettingPanel(){
        return settingPanel;
    }

    public void OnAllPanel(bool isActive)
    {
        foreach (var controller in panelControllers)
        {
            controller.OnPanel(isActive);
        }
    }
}

// all panel Common Function Management
public interface PanelController
{
    // on or off panel
    public void OnPanel(bool isActive);
}