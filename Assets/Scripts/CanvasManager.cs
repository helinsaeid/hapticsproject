using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CanvasManager : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject chatMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startMenu.SetActive(true);
        FindFirstObjectByType<QuestionnaireManager>().HideQuestionnaire();
    }

    public void StartButton()
    {
        startMenu.SetActive(false);
        chatMenu.SetActive(true);
        FindFirstObjectByType<VibrationManager>().nextPattern();
        FindFirstObjectByType<QuestionnaireManager>().ShowQuestionnaire();
    }
}
