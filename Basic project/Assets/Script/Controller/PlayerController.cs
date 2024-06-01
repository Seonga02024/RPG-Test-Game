using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int _coin = 0;
    public int Coin 
    {
        get { return _coin; }
        private set { _coin = value; }
    }

    public void ChangeCoinData(int coin){
        _coin = coin;
        SystemCanvas.Instance.GetPlayerInfoPanel().ChangeCoinText(_coin);
    }

    public void AddCoin(int coin){
        _coin += coin;
        SystemCanvas.Instance.GetPlayerInfoPanel().ChangeCoinText(_coin);
        SaveLoadManager.Instance.SetCoin(_coin);
    }

    public void MinusCoin(int coin){
        _coin -= coin;
        SystemCanvas.Instance.GetPlayerInfoPanel().ChangeCoinText(_coin);
        SaveLoadManager.Instance.SetCoin(_coin);
    }
}
