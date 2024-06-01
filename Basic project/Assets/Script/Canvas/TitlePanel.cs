using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanel : MonoBehaviour, PanelController
{
    [SerializeField] private GameObject mainPanel;

    public void OnPanel(bool isActive)
    {
        mainPanel.SetActive(isActive);
    }

    public void ClickGameStartBtn(){
        mainPanel.SetActive(false);
        GameManager.Instance.ShowStageInfo();
    }

    public void ClickBackBtn(){
        #if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
    }
}
