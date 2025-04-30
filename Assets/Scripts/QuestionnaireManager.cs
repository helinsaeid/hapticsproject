using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestionnaireManager : MonoBehaviour
{
    public ToggleGroup arousalGroup;
    public ToggleGroup valenceGroup;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void logQuestionnaireValues(string message)
    {
        string arousalValue = "0", valenceValue = "0"; 
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
        
        // If something is wrong the arousal and valence values are 0
        FindFirstObjectByType<Logger>().Log($"'{message}', {arousalValue},  {valenceValue}");
        
    }
    
    
}
