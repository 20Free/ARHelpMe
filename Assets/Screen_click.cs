using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Screen_click : MonoBehaviour
{
    // Start is called before the first frame update

    private bool toggle;
    //Keep this one as a global variable (outside the functions) too and use GetComponent during start to save resources
    private float startRecordingTime;
    public AudioClip myAudioClip;
    AudioSource audio;
    public Camera camera;
    public Renderer cameraShot;
    private Face[] faces;

    private int resWidth;
    private int resHeight;

    void Start()
    {
        toggle = true;
        resHeight = camera.pixelHeight;
        resWidth = camera.pixelWidth;
        //AudioSource audio = GetComponent<AudioSource>();

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
        //if (fingerCount > 0)
        //{
        //    words.text += "yes ";
        //}
        if (Input.GetMouseButtonDown(0))
        {
            click();
        }
    }

    bool shot()
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        return true;
        //string filename = add file name
        //System.IO.File.WriteAllBytes(filename, bytes); save file name
        //Debug.Log(string.Format("Took screenshot to: {0}", filename));
    }

    void click()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hit))
        {
            GameObject selected = hit.transform.gameObject;

            if(selected)
            {
                if(camera && selected)
                {
                    if(shot())
                    {
                        StartCoroutine(DoFaceDetection());
                    }
                }
                else if(cameraShot && selected == cameraShot.gameObject)
                {
                    StartCoroutine(DoFaceDetection());
                }
            }
        }
        //if (toggle)
        //    startRec();
        //else
        //    stopRec();
        //toggle = !toggle;
    }

    // makes camera shot and displays it on the camera-shot object
	/*private bool DoCameraShot()
	{
		if(cameraShot && camera)
		{
			
			Texture tex = camera.GetSnapshot();
			cameraShot.GetComponent<Renderer>().material.mainTexture = tex;

			Vector3 localScale = cameraShot.transform.localScale;
			localScale.x = (float)tex.width / (float)tex.height * Mathf.Sign(localScale.x);
			cameraShot.transform.localScale = localScale;

			return true;
		}

		return false;
	}*/

	// imports image and displays it on the camera-shot object
	private bool DoImageImport()
	{
        Texture2D tex = FaceDetectionUtils.ImportImage();

        if (tex && cameraShot)
        {
            cameraShot.GetComponent<Renderer>().material.mainTexture = tex;

            Vector3 localScale = cameraShot.transform.localScale;
            localScale.x = (float)tex.width / (float)tex.height * Mathf.Sign(localScale.x);
            cameraShot.transform.localScale = localScale;

			return true;
        }

        return false;
	}

    // performs face detection
	private IEnumerator DoFaceDetection()
	{
		// get the image to detect
		faces = null;
		Texture2D texCamShot = null;

		if(cameraShot)
		{
			texCamShot = (Texture2D)cameraShot.GetComponent<Renderer>().material.mainTexture;
			
			//SetHintText("Wait...");
		}

		// get the face manager instance
		CloudFaceManager faceManager = CloudFaceManager.Instance;

		if(texCamShot && faceManager)
		{
			if(faces != null && faces.Length > 0)
			{
				
				//if(displayFaceRectangles)
				{
					//faceManager.DrawFaceRects(texCamShot, faces, FaceDetectionUtils.FaceColors, this.displayHeadDirection);
					faceManager.AddFaceToList(texCamShot, faces);
				}
				//Add most promininent face to faceList.
				
				//SetHintText("Click on the camera image to make a shot");
			}
			else
			{
				//SetHintText("No faces detected.");
			}

			yield return faceManager.DetectFaces(texCamShot);
			faces = faceManager.faces;
		}
		else
		{
			//SetHintText("Check if the FaceManager component exists in the scene.");
		}

		yield return null;
	}

    void startRec()
    {
        myAudioClip = Microphone.Start(null, false, 5, 44100);
    }

    void stopRec()
    {
        Microphone.End(null);
        AudioSource audio = GetComponent<AudioSource>();
        //audio.Stop();
        audio.clip = myAudioClip;
        audio.loop = false;
        audio.Play();
    }

}
