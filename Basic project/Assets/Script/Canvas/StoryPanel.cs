using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private Button backButton;

    public void Start(){
        backButton?.onClick.AddListener(OnBackButtonPressed);
    }

    public void OnPanel(bool isAcitve){
        mainPanel.SetActive(isAcitve);
    }
    
    public void ChangeStoryText(string text){
        storyText.text = text;
    }

    private void OnBackButtonPressed()
    {
        Debug.Log("Back button pressed");
        OnPanel(false);
        GameManager.Instance.ChangeState(GameManager.Instance.BattleState);
    }
}
