using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Icon : MonoBehaviour
{
    [SerializeField] private Image lockImg;
    [SerializeField] private Button iconButton;

    // Start is called before the first frame update
    void Start()
    {
        iconButton.onClick.AddListener(OniconButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OniconButtonClick(){

    }
}
