//using System.Linq;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Windows.Speech;

//public class Audio : MonoBehaviour
//{
//    private Dictionary<string, Action> keyActs = new Dictionary<string, Action>();
//    private AudioSource soundSource;
//    public AudioClip[] sounds;
//    private KeywordRecognizer recognizer;
//    // Start is called before the first frame update
//    void Start()
//    {
//        soundSource = GetComponent<AudioSource>();
//        keyActs.Add("The name is", Confirmation);
//        recognizer = new KeywordRecognizer(keyActs.Keys.ToArray());
//        recognizer.OnPhraseRecognized += OnKeywordsRecognized;
//        recognizer.Start();
//    }

//    void Confirmation()
//    {
//        recognizer = new KeywordRecognizer(keyActs.Keys.ToArray());
//        recognizer.OnPhraseRecognized += OnKeywordsRecognized;
//        //recognizer.Start();
//        Debug.Log(recognizer);
//    }

//    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
//    {
//        Debug.Log("Command: " + args.text);
//        keyActs[args.text].Invoke();
//    }

//}

