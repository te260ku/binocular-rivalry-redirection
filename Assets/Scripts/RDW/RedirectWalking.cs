using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectWalking : MonoBehaviour
{
    public GameObject leftObj;
    public GameObject rightObj;
    public GameObject rightWall;
    public GameObject leftWall;
    int count;
    Color defaultColor;
    public bool changeColor = true;
    public bool perEye = true;
    public float rotationValue = 0.5f;
    private int stepCount;
    private AudioSource audioSource;
    public AudioClip triggerAudio;
    public int loopCount;
    public float inducedTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = rightWall.GetComponent<Renderer>().material.color;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)) {
            // audioSource.PlayOneShot(triggerAudio);
            StartCoroutine(RotateObject(loopCount));
            // stepCount ++;
            if (stepCount >= 3) {
                stepCount = 0;
            }
        }
        
    }

    private IEnumerator RotateObject(int loopCount) {
        count = loopCount;

        switch (stepCount)
        {
            case 0:
                // full
                perEye = true;
                changeColor = true;
                break;
            case 1:
                perEye = true;
                changeColor = false;
                break;
            case 2:
                perEye = false;
                changeColor = true;
                break;
            case 3:
                perEye = false;
                changeColor = false;
                break;
            default:
                break;
        }

        if (perEye) {
            

            while (count > 0) {
                leftObj.transform.Rotate(0, rotationValue, 0);
                if (changeColor) rightWall.GetComponent<Renderer>().material.color = Color.red;
                yield return new WaitForSeconds(inducedTime);
                rightWall.GetComponent<Renderer>().material.color = defaultColor;
                // wall.GetComponent<Renderer>().material.color = Color.gray;
                yield return new WaitForSeconds(inducedTime);
                count --;
            }

            count = loopCount;

            while (count > 0) {
                rightObj.transform.Rotate(0, rotationValue, 0);
                if (changeColor) leftWall.GetComponent<Renderer>().material.color = Color.red;
                yield return new WaitForSeconds(inducedTime);
                leftWall.GetComponent<Renderer>().material.color = defaultColor;
                // wall.GetComponent<Renderer>().material.color = Color.gray;
                yield return new WaitForSeconds(inducedTime);
                count --;
            }

        } else {
            while (count > 0) {
            leftObj.transform.Rotate(0, rotationValue, 0);
            if (changeColor) rightWall.GetComponent<Renderer>().material.color = Color.red;

            rightObj.transform.Rotate(0, rotationValue, 0);
            if (changeColor) leftWall.GetComponent<Renderer>().material.color = Color.red;

               
            yield return new WaitForSeconds(inducedTime);
            rightWall.GetComponent<Renderer>().material.color = defaultColor;
            leftWall.GetComponent<Renderer>().material.color = defaultColor;
            // wall.GetComponent<Renderer>().material.color = Color.gray;
            yield return new WaitForSeconds(inducedTime);
            count --;
            }
        }
        

        // rightObj.transform.rotation = leftObj.transform.rotation;
    }
}
