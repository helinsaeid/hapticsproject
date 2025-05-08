using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Random = System.Random;
using JetBrains.Annotations;


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
    public GameObject chatBubble;

    private string currentMessage;
    private long[] currentVibration;
    
    private bool shouldVibrate = true;
    // Define varables
    // Define variables
    Dictionary<string, long[]> promptsAndVibrations = new Dictionary<string, long[]>();
    List<string> messages = new List<string>() {
        // Joy
        "I got the job!",
        "I'm doing great!",
        "It's such a beautiful day today!",
        "Can't wait to see you today!",
        "I just got amazing news!",
        "Everything is working out perfectly!",
        "That made me laugh so much!",
        "I'm feeling really lucky today.",
        "I finally finished it, I'm so proud",
        "Life feels really good right now.",

        // Sadness
        "It didn't turn out the way I hoped.",
        "I don't feel like it today.",
        "It feels like a heavy day.",
        "Why does this happen to me?",
        "I miss how things used to be.",
        "I'm feeling really low right now.",
        "It’s hard to keep pretending I’m okay.",
        "I wish things were different.",
        "I just feel so alone.",
        "Today feels like too much.",

        // Relaxation
        "Im sitting by the beach, listening to waves",
        "No stress today, just taking it easy",
        "Enjoying a cup of tea and some silence.",
        "I'm just lying on the couch doing nothing.",
        "Watching the sunset in peace.",
        "Taking a slow walk through the park.",
        "Just breathing and being present.",
        "Curled up with a book and blanket.",
        "Letting the day unfold slowly.",
        "Just listening to soft music and unwinding.",

        // Anger
        "Why do you never answer?!",
        "This is completely unacceptable.",
        "I can't deal with this right now.",
        "I'm seriously bothered.",
        "That’s not okay.",
        "I’m done explaining myself.",
        "This keeps happening and I’m sick of it.",
        "You never listen!",
        "That really crossed a line.",
        "I need space, I’m too upset to talk.",

        // Neutral
        "See you later.",
        "I left the package outside your door.",
        "The meeting starts at three.",
        "I will walk there.",
        "I'm eating lunch right now.",
        "I just finished my meeting.",
        "I ate lunch!",
        "I'll call you tomorrow.",
        "It's on the table.",
        "I'm going to the store now."
    };
    
    long[] joy = { 0, 150, 100, 150, 100, 150 };
    long[] sadness = { 0, 600, 100, 600 };
    long[] relaxation = { 0, 400, 800, 400 };
    long[] anger = { 0, 300, 1000, 300 };

    Dictionary<string, long[]> emotionMap = new Dictionary<string, long[]>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Add to map
        
        emotionMap.Add("joy", joy);
        emotionMap.Add("sadness", sadness);
        emotionMap.Add("relaxation", relaxation);
        emotionMap.Add("anger", anger);
        
        // Add patterns and messages
        AddMessagesWithRange(0, 9, joy);
        AddMessagesWithRange(10, 19, sadness);
        AddMessagesWithRange(20, 29, relaxation);
        AddMessagesWithRange(30, 39, anger);
        AddMessagesWithRangeAndRandomVibration(40, 49);
    }

    // Is called when user presses "Next pattern"-button
    public void nextPattern()
    {
        // I know this is ugly, but it works... And i don't want to make it pretty right now.
        if (promptsAndVibrations.Count == 0)
        {
            // Check if gameobjects with message tag exists and destroy found.
            var gos = GameObject.FindGameObjectsWithTag("Message");
            foreach (var go in gos)
            {
                Destroy(go);
            }
            
            messageObject = Instantiate(messagePrefab);
            messageObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Experiment DONE :) " +
                                                                       "Thank you for participating";
            messageObject.transform.SetParent(chatBubble.transform);  
            messageObject.transform.localPosition = Vector3.zero /*+ new Vector3(0, 250, 0)*/;
            FindFirstObjectByType<QuestionnaireManager>().HideQuestionnaire();
            return;
        }
        
        
        if (currentMessage != null)
        {
            string e;
            if (currentVibration.SequenceEqual(joy))
                e = "joy";
            else if (currentVibration.SequenceEqual(sadness))
                e = "sadness";
            else if (currentVibration.SequenceEqual(relaxation))
                e = "relaxation";
            else if (currentVibration.SequenceEqual(anger))
                e = "anger";
            else
                e = "unknown";
        
            FindFirstObjectByType<QuestionnaireManager>().logQuestionnaireValues($"{currentMessage}", e);
        }
        
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
            messageObject.transform.SetParent(chatBubble.transform);  
            messageObject.transform.localPosition = Vector3.zero /*+ new Vector3(0, 250, 0)*/;

            // Vibrate Button
            if (shouldVibrate)
            {
                setVibrateBtn(value);
            }
            
            // Debug
            // FindFirstObjectByType<Logger>().Log($"Message: {messages[randomNr]}, Pattern: {value}");
            // Set current message
            currentMessage = messages[randomNr];
            currentVibration = value;
            //Start Vibration
            if (shouldVibrate)
            {
                AndroidVibrate(value, -1);
            }
            
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
        btn.transform.localPosition = Vector3.zero + new Vector3(0, -680, 0);
        
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
            });
        } else {
            // Android -- Custom pattern
            btn.onClick.AddListener(() =>
            {
                AndroidVibrate(pattern, -1);
            });

        }
        
    }

    void AndroidVibrate(long[] pattern, int repeat)
    {
        Vibration.Init();
        // Vibration.VibrateAndroid(pattern, repeat);
        // FindFirstObjectByType<Logger>().Log($"Vibrate with pattern: {pattern}");
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

    public void SetShouldVibrate(bool value)
    {
        shouldVibrate = value;
    }

    void AddMessagesWithRange(int start, int end, long[] pattern)
    {
        for (var i = start; i <= end; i++)
        {
            promptsAndVibrations.Add(messages[i], pattern);  
        }
    }

    void AddMessagesWithRangeAndRandomVibration(int start, int end)
    {
        for (var i = start; i <= end; i++)
        {
            Random rnd = new Random();
            int randomNr  = rnd.Next(1, 5);
            switch (randomNr)
            {
                case 1: promptsAndVibrations.Add(messages[i], joy);
                    break;
                case 2: promptsAndVibrations.Add(messages[i], sadness);
                    break;
                case 3: promptsAndVibrations.Add(messages[i], relaxation);
                    break;
                case 4:
                default:
                    promptsAndVibrations.Add(messages[i], anger);
                    break;
            }
        }
    }
}
