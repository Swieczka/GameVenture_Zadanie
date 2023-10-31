using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roundNumberText, hpText, InfoPanelText;
    [SerializeField] GameObject startPanel;
    [TextArea] public string infoStartText;
    private void Start()
    {
        UpdateInfo(infoStartText);
        
    }
    public void UpdateHP(string hp)
    {
        hpText.text = $"Health Points: {hp}";
    }
    public void UpdateRounds(string roundNumber)
    {
        roundNumberText.text = $"Round: {roundNumber}";
    }
    public void UpdateInfo(string info)
    {
        InfoPanelText.text = info;
    }

    public void PanelVisibility(bool isVisible)
    {
        if(isVisible) startPanel.SetActive(true);
        else startPanel.SetActive(false);
    }
}
