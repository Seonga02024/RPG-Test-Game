using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class characterStat : MonoBehaviour
{
    [Header("[charater info]")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI attackText;
    public GameObject noBuy;
    public GameObject noUse;

    public void ChangeInfo(string name, int hp, int attack, bool isBuy, bool isUse){
        nameText.text = name;
        hpText.text = hp.ToString();
        attackText.text = attack.ToString();
        if(isUse == true){ 
            noUse.SetActive(false);
            if(isBuy) noBuy.SetActive(false);
            else noBuy.SetActive(false);
        }
    }
}
