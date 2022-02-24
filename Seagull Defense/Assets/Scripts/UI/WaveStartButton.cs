using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveStartButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void OnGUI()
    {
        button.interactable = State.Instance.Current == GameState.Build;
    }
}
