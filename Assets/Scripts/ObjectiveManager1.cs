using UnityEngine;
using TMPro;

public class ObjectiveManager1 : MonoBehaviour
{
    public static ObjectiveManager1 instance;

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