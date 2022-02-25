using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Bank", menuName = "ScriptableObjects/Bank")]
public class BankSO : ScriptableObject
{
    [SerializeField]
    private int startingBalance = 100;

    private int balance;

    [System.NonSerialized]
    public UnityEvent<int> balanceChangeEvent;

    private void OnEnable()
    {
        balance = startingBalance;
        if (balanceChangeEvent == null)
        {
            balanceChangeEvent = new UnityEvent<int>();
        }
    }

    public int Balance
    {
        get { return balance; }
    }

    public bool ChangeBalance(int val)
    {
        if (balance + val < 0)
        {
            return false;
        }

        balance += val;
        balanceChangeEvent.Invoke(balance);
        return true;
    }
}
