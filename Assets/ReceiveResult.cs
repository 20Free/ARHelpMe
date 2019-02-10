using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ReceiveResult : MonoBehaviour {

    // Use this for initialization

    public Text words;

	void Start () {
        //GameObject.Find("Text").GetComponent<Text>().text = "You need to be connected to Internet";
        words.text = "I hope this works";
    }
	
    void onActivityResult(string recognizedText){
        Debug.Log("PLEASEEE!!!!!!!!!!!!!!!!");
        char[] delimiterChars = {'~'};
        string[] result = recognizedText.Split(delimiterChars);

        //You can get the number of results with result.Length
        //And access a particular result with result[i] where i is an int
        //I have just assigned the best result to UI text
        words.text = result[0];

    }

	// Update is called once per frame
	void Update () {
		
	}
}
