using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    public GameObject objectivePanel;
    public TextMeshProUGUI objectiveText;

    void Awake()
    {
        instance = this;
    }

    public void ShowObjective(string text)
    {
        objectivePanel.SetActive(true);
        objectiveText.text = text;
    }

    public void HideObjective()
    {
        objectivePanel.SetActive(false);
    }
}