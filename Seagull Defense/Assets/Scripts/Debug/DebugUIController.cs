using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIController : MonoBehaviour
{
    [SerializeField]
    private Text debugText;

    private string text;
    private string prevText;

    // Update is called once per frame
    void Update()
    {
        text = ConstructDebugString();
        if (text != prevText)
        {
            debugText.text = text;
            prevText = text;
        }
    }


    private string ConstructDebugString()
    {
        return $"Phase: {State.Instance.Current.ToString()}\n" +
            $"Wave: {State.Instance.Wave}\n" +
            $"Tiles: {State.Instance.TilesRemaining}/{Data.Instance.MaxTiles}";
    }
}
