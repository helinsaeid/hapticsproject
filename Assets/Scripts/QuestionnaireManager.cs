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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void logQuestionnaireValues(string message)
    {
        Toggle arousal = arousalGroup.ActiveToggles().FirstOrDefault();
        string arousalValue = arousal.GetComponentInChildren<TMPro.TextMeshProUGUI>().text; // This can be NULL 
        
        Toggle valence = valenceGroup.ActiveToggles().FirstOrDefault();
        string valenceValue = valence.GetComponentInChildren<TMPro.TextMeshProUGUI>().text; // This can be NULL

        FindFirstObjectByType<Logger>().Log($"{message}, {arousalValue},  {valenceValue}");
        
    }
    
    
}
