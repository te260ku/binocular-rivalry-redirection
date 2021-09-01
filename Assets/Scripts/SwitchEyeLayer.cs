using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchEyeLayer : MonoBehaviour
{
    public GameObject rightObject;
    public GameObject leftObject;
    public GameObject flipObject;
    public GameObject defaultObject;
    private bool isFliped;
    private int rightLayer = 8;
    private int leftLayer = 9;
    // Start is called before the first frame update
    void Start()
    {
        SwitchObject();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || Input.GetKeyDown(KeyCode.Space))
        {
            // audioSource.PlayOneShot(triggerAudio);
            isFliped = !isFliped;
            SwitchObject();
        }
    }

    private void SwitchObject() {
        if (!isFliped) {
                flipObject.SetActive(false);
                defaultObject.SetActive(true);
            } else {
                flipObject.SetActive(true);
                defaultObject.SetActive(false);
            }
    }
}
