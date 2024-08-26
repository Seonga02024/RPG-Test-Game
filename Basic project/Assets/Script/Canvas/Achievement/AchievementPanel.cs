using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPanel : MonoBehaviour, PanelController
{
    [SerializeField] private GameObject mainPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPanel(bool isAcitve){
        mainPanel.SetActive(isAcitve);
    }
}
