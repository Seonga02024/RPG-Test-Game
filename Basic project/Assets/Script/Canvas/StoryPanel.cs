using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doublsb.Dialog;
using TMPro;

public class StoryPanel : MonoBehaviour
{
    [SerializeField] public DialogManager DialogManager;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Button backButton;
    [SerializeField] public Image backgroudImg;
    [SerializeField] public Sprite[] backgroundImgSprites;
    private List<DialogData> dialogTexts = new List<DialogData>();

    public void Start(){
        backButton?.onClick.AddListener(OnBackButtonPressed);
        if (DialogManager != null)
        {
            DialogManager.OnDialogFinished += HandleDialogFinished;
        }
    }

    public void OnPanel(bool isAcitve){
        mainPanel.SetActive(isAcitve);
        backButton.gameObject.SetActive(false);
    }

    private void OnBackButtonPressed()
    {
        Debug.Log("Back button pressed");
        OnPanel(false);
        GameManager.Instance.ChangeState(GameManager.Instance.BattleState);
    }

    private void HandleDialogFinished()
    {
        Debug.Log("StoryPanel received signal from DialogManager!");
        backButton.gameObject.SetActive(true);
    }

    public void ShowStory(int storyNum){
        dialogTexts.Clear();
        Debug.Log("storyNum : " + storyNum);
        if(storyNum == 7) Story1();
        else if (storyNum == 15) Story15();
        else if (storyNum == 25) Story25();
    }

    private void Story1(){
        backgroudImg.sprite = backgroundImgSprites[0];
        dialogTexts.Add(new DialogData("Is this the inside of the tower...")); // 여기가 탑 내부인가…
        dialogTexts.Add(new DialogData("..."));
        dialogTexts.Add(new DialogData("I hope it's not too hard from the beginning...")); // 처음부터 너무 힘들지 않았으면 좋겠는데…
        DialogManager.Show(dialogTexts, 3);
    }

    private void Story15(){
        backgroudImg.sprite = backgroundImgSprites[0];
        dialogTexts.Add(new DialogData("I feel like the monsters are getting stronger as I go up...")); // 올라갈수록 점점 몬스터가 세지는 것 같은 기분이…
        DialogManager.Show(dialogTexts, 1);
    }

    private void Story25(){
        backgroudImg.sprite = backgroundImgSprites[0];
        dialogTexts.Add(new DialogData("There are other people besides me...")); // 나 말고도 다른 사람들이 있구나… 
        dialogTexts.Add(new DialogData("Should I talk to him?")); // 말 걸어볼까?
        DialogManager.Show(dialogTexts, 2);
    }
}
