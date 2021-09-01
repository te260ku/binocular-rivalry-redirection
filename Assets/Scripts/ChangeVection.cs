using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVection : MonoBehaviour
{
    int count;
    public GameObject[] objects;
    // Start is called before the first frame update
    void Start()
    {
        SwitchObject();
    }

    private void Update() {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            SwitchObject();
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            SwitchObject();
        }
    }
    private void SwitchObject() {

        for (int i=0; i<objects.Length; i++) {
            if (i == count) {
                objects[i].SetActive(true);
            } else {
                objects[i].SetActive(false);
            }
            
        }
        count ++;
        if (count >= objects.Length) {
            count = 0;
        }
        
        

        
        
    }

    
}
