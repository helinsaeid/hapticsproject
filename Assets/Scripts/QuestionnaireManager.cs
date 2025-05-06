using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestionnaireManager : MonoBehaviour
{
    public GameObject questionnaireContainerPrefab;
    public ToggleGroup arousalGroup;
    public ToggleGroup valenceGroup;
    public ToggleGroup emotionGroup;
    public GameObject optionGroup;
    
    // Add emotions
    // Negative          Positive
    // Calm              Exited
    // Reset to middle after each press.
    // Show/Hide in start
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideQuestionnaire();
    }

    public void HideQuestionnaire()
    {
        questionnaireContainerPrefab.SetActive(false);
    }
    public void ShowQuestionnaire()
    {
        questionnaireContainerPrefab.SetActive(true);
    }
    public void logQuestionnaireValues(string message)
    {
        string arousalValue = "0", valenceValue = "0",  emotionValue = "0"; 
        Toggle arousal = arousalGroup.ActiveToggles().FirstOrDefault();
        if (arousal != null)
        {
            arousalValue = arousal.GetComponentInChildren<TMPro.TextMeshProUGUI>().text; 
        }
        
        Toggle valence = valenceGroup.ActiveToggles().FirstOrDefault();
        if (valence != null)
        {
            valenceValue = valence.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        }
        Toggle emotion = emotionGroup.ActiveToggles().FirstOrDefault();
        if (emotion != null)
        {
            emotionValue = emotion.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        }

        bool shouldVibrate = optionGroup.transform.Find("ShouldVibrateToggle").GetComponent<Toggle>().isOn;
        // Torsdag kan Helin
        // 25 istället för 50 meddelanden
        
        
        // If something is wrong the arousal and valence values are 0
        FindFirstObjectByType<Logger>().Log($"'{message}', {emotionValue}, {arousalValue},  {valenceValue}, {shouldVibrate}");
        
        ResetGroups();
    }

    void ResetGroups()
    {
        Toggle arousalToggle = arousalGroup.transform.Find("Option 4").GetComponent<Toggle>();
        arousalToggle.isOn = true;
        
        Toggle valenceToggle = valenceGroup.transform.Find("Option 4").GetComponent<Toggle>();
        valenceToggle.isOn = true;
        
        Toggle emotionToggle = emotionGroup.transform.Find("Joy").GetComponent<Toggle>();
        emotionToggle.isOn = true;
    }
    
    
}
