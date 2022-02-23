using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectedButton
{
    None,
    Path,
    Turret
}

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Color highlightColor;
    [SerializeField]
    private GameObject pathBuyButton;
    [SerializeField]
    private GameObject turretBuyButton;
    [SerializeField]
    private Text pathText;

    private SelectedButton selected;
    private List<GameObject> buttons;

    void Start()
    {
        buttons = new List<GameObject>() { pathBuyButton, turretBuyButton };
        pathBuyButton.GetComponent<Button>().onClick.AddListener(() => ToggleButton(SelectedButton.Path));
        turretBuyButton.GetComponent<Button>().onClick.AddListener(() => ToggleButton(SelectedButton.Turret));
    }

    void OnGUI()
    {
        pathText.text = $"{State.Instance.TilesRemaining}";
    }

    private void ToggleButton(SelectedButton sel)
    {
        Debug.Log($"Toggling button {sel.ToString()}");
        if (selected == sel)
        {
            selected = SelectedButton.None;
        }
        else
        {
            selected = sel;
        }
        UpdateButtonColors();
    }

    private void UpdateButtonColors()
    {
        foreach (GameObject b in buttons)
        {
            b.GetComponent<Image>().color = Color.white;
        }
        switch (selected)
        {
            case SelectedButton.Path:
                //highlight path button
                pathBuyButton.transform.GetComponent<Image>().color = highlightColor;
                break;
            case SelectedButton.Turret:
                //highlight turret button
                turretBuyButton.transform.GetComponent<Image>().color = highlightColor;
                break;
        }
    }

    public SelectedButton Selected
    {
        get { return selected; }
    }
}
