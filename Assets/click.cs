using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class click : MonoBehaviour
{
    // Start is called before the first frame update
    public Text words;
    private bool toggle;

    void Start()
    {
        toggle = true;
    }

    // Update is called once per frame
    void Update()
    {
        //var fingerCount = 0;
        //foreach (Touch touch in Input.touches)
        //{
        //    if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
        //    {
        //        fingerCount++;
        //    }
        //}
        //if (Input.GetMouseButtonDown(0) && toggle)
        //{
        //    words.text += "yes ";
        //    toggle = false;
        //}
        //else if (Input.GetMouseButtonUp(0) && !toggle)
        //{
        //    words.text += "no ";
        //    toggle = true;
        //}
        //var fingerCount = 0;
        //foreach (Touch touch in Input.touches)
        //{
        //    if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
        //    {
        //        fingerCount++;
        //    }
        //}
        //if (fingerCount > 0)
        //{
        //    words.text += "yes ";
        //}
    }
}
