using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class CanvasManager : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject startMenu;
    public GameObject chatMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        optionsMenu.SetActive(true);
        chatMenu.SetActive(false);
        FindFirstObjectByType<QuestionnaireManager>().HideQuestionnaire();
    }

    public void OptionsConfirmed()
    {
        FindFirstObjectByType<VibrationManager>().SetShouldVibrate(optionsMenu.transform.Find("ShouldVibrateToggle").GetComponent<Toggle>().isOn);
        optionsMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void StartButton()
    {
        startMenu.SetActive(false);
        chatMenu.SetActive(true);
        FindFirstObjectByType<VibrationManager>().nextPattern();
        FindFirstObjectByType<QuestionnaireManager>().ShowQuestionnaire();
    }
}
