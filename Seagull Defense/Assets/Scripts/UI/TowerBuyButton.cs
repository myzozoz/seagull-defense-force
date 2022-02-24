using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuyButton : MonoBehaviour
{
    public TowerSO towerSO;
    public Text priceText;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        priceText.text = $"${towerSO.cost}";
        Data.Instance.Bank.balanceChangeEvent.AddListener(OnBalanceChange);
    }

    private void OnBalanceChange(int balance)
    {
        button.enabled = balance >= towerSO.cost;
    }
}
