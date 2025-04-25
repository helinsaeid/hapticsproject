using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Random = System.Random;


public class VibrationManager : MonoBehaviour
{
    [Tooltip("Should have TextMeshProUGUI component")]
    public GameObject messagePrefab;
    private GameObject messageObject;
    [Tooltip("Next vibration button")]
    public GameObject vibrateButtonPrefab;
    private GameObject vibrateButtonObject;
    [Tooltip("Canvas container for buttons and messages")]
    public GameObject canvasObject;
    // Define varables
    Dictionary<string, long[]> promptsAndVibrations =
        new Dictionary<string, long[]>();
    List<string> messages = new List<string>() {"Hello1", "Hello2", "Hello3", "Hello4", "Hello5", "Hello6", "Hello7", "Hello8"};
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*DEFINE PATTERNS*/
        long[] joy = { 1, 1000, 1000, 1000, 1000 };
        long[] sadness = { 2, 1000, 1000, 1000, 1000 };
        long[] relaxation = { 10, 1000, 1000, 1000, 1000 };
        long[] anger = { 3, 1000, 1000, 1000, 1000 };
        
        
        // Add patterns and messages
        promptsAndVibrations.Add(messages[0], joy);
        promptsAndVibrations.Add(messages[1], joy);
        promptsAndVibrations.Add(messages[2], sadness);
        promptsAndVibrations.Add(messages[3], sadness);
        promptsAndVibrations.Add(messages[4], anger);
        promptsAndVibrations.Add(messages[5], anger);
        promptsAndVibrations.Add(messages[6], relaxation);
        promptsAndVibrations.Add(messages[7], relaxation);
    }

    // Is called when user presses "Next pattern"-button
    public void nextPattern()
    {
        // Random value between 0 and length of dict.
        Random rnd = new Random();
        int randomNr  = rnd.Next(0, promptsAndVibrations.Count-1);  // creates a number between 0 and length of dict
        long[] value;
        if (promptsAndVibrations.TryGetValue(messages[randomNr], out value))
        {
            // Check if gameobjects with message tag exists and destroy found.
            var gos = GameObject.FindGameObjectsWithTag("Message");
            foreach (var go in gos)
            {
                Destroy(go);
            }
            
            // Message Object 
            messageObject = Instantiate(messagePrefab);
            messageObject.GetComponent<TMPro.TextMeshProUGUI>().text = messages[randomNr];
            messageObject.transform.SetParent(canvasObject.transform);  
            messageObject.transform.localPosition = Vector3.zero + new Vector3(0, 200, 0);

            // Vibrate Button
            setVibrateBtn(value);
            
            // Debug
            FindFirstObjectByType<Logger>().Log($"Message: {messages[randomNr]}, Pattern: {value}");
            
            // Remove from lists.
            promptsAndVibrations.Remove(messages[randomNr]);
            messages.Remove(messages[randomNr]);
            
        }
        else
        {
            // Debug
            FindFirstObjectByType<Logger>().Log($"Key = {messages[randomNr]} is not found.");
        }
    }

    void setVibrateBtn(long[] pattern = null)
    {
        // If pattern is not provided
        if (pattern == null)
        {
            return;
        }
        
        var gos = GameObject.FindGameObjectsWithTag("VibrateBtn");
        foreach (var go in gos)
        {
            StopCoroutine(VibratePattern1());
            StopCoroutine(VibratePattern2());
            StopCoroutine(VibratePattern3());
            Destroy(go);
        }
        vibrateButtonObject = Instantiate(vibrateButtonPrefab);
        
        Button btn = vibrateButtonObject.GetComponent<Button>();
        
        btn.transform.SetParent(canvasObject.transform);
        btn.transform.localPosition = Vector3.zero + new Vector3(200, 0, 0);
        
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            // iOS pattern -- Random Vibration
            Random rnd = new Random();
            int randomNr  = rnd.Next(1, 4);
            btn.onClick.AddListener(() =>
            {
                switch (randomNr)
                {
                    case 1:
                        StartCoroutine(VibratePattern1());
                        break;
                    case 2:
                        StartCoroutine(VibratePattern2());
                        break;
                    case 3:
                        StartCoroutine(VibratePattern3());
                        break;
                    default:
                        StartCoroutine(VibratePattern1());
                        break;
                }
                FindFirstObjectByType<Logger>().Log($"Vibrate with pattern: {pattern}");
                
            });
        } else {
            // Android -- Custom pattern
            btn.onClick.AddListener(() =>
            {
                FindFirstObjectByType<Logger>().Log($"Vibrate with pattern: {pattern}");
                VibrateAndroid();
            });

        }
        
    }

    void VibrateAndroid()
    {
        
    }
    
    // IOS test
    IEnumerator VibratePattern1()
    {
        Vibration.VibrateNope();
        yield return new WaitForSeconds(0.1f);
        Vibration.VibrateNope();
        yield return new WaitForSeconds(0.1f);
        Vibration.VibrateNope();
        yield return new WaitForSeconds(0.5f);

        Vibration.VibrateNope();
        yield return new WaitForSeconds(0.5f);

        Vibration.VibrateNope();
    }
    
    IEnumerator VibratePattern2()
    {
        Vibration.VibrateNope();
        yield return new WaitForSeconds(0.5f);

        Vibration.VibrateNope();
        yield return new WaitForSeconds(0.5f);

        Vibration.VibrateNope();
        yield return new WaitForSeconds(0.5f);
        
        Vibration.VibrateNope();
        yield return new WaitForSeconds(0.5f);

        Vibration.VibrateNope();
    }
    
    IEnumerator VibratePattern3()
    {
        Vibration.VibrateNope();
        yield return new WaitForSeconds(1.0f);
        
        Vibration.VibrateNope();
        yield return new WaitForSeconds(1.0f);
        
        Vibration.VibrateNope();
        yield return new WaitForSeconds(1.0f);

        Vibration.VibrateNope();
        yield return new WaitForSeconds(0.1f);

        Vibration.VibrateNope();
        yield return new WaitForSeconds(0.1f);
        
        Vibration.VibrateNope();
        yield return new WaitForSeconds(0.1f);

        Vibration.VibrateNope();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
