using UnityEngine;
using TMPro;
using System;

public class GoldUI : MonoBehaviour
{
    public TMP_Text goldText;

    private void Start()
    {
        goldText.text = GoldManager.Instance.CurrentGold.ToString();
        GoldManager.Instance.OnGoldChanged += UpdateUI;
    }

    void UpdateUI(int newGold)
    {
        goldText.text = newGold.ToString();
    }

    private void OnDestroy()
    {
        GoldManager.Instance.OnGoldChanged -= UpdateUI;
    }
}
