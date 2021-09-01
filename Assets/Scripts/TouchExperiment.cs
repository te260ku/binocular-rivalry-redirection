using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TouchExperiment : MonoBehaviour
{
    public int[] logData;
    float timeA;
    float timeB;
    private bool isOn;
    private StreamWriter sw;
    private AudioSource audioSource;
    public AudioClip stopAudio;
    private float time;
    private float timeNothing;
    public float limitTime = 30f;
	void Start()
    {
        sw = new StreamWriter("testData.txt");
        string[] s1 = { "F", "J", "time" };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);

        audioSource = GetComponent<AudioSource>();
        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            sw.Close();
        }

        if (!isOn) {
            if (Input.anyKeyDown) {
            isOn = true;
            Debug.Log("start");
        }
        }
        

        

        // 開始時間
        // 終了時間
        if (isOn) {
            time += Time.deltaTime;
            if (time >= limitTime) {
                audioSource.PlayOneShot(stopAudio);
                sw.Close();
                
                Debug.Log(timeA.ToString("N1") + ", " + timeB.ToString("N1") + ", " + 
                timeNothing.ToString("N1"));
                isOn = false;
            }

            if (Input.GetKey(KeyCode.A)) {
                timeA += Time.deltaTime;
            } else if (Input.GetKey(KeyCode.S)) {
                timeB += Time.deltaTime;
            } else {
                timeNothing += Time.deltaTime;
        }
        }

        

        
    }
	
   
    public void SaveData(string txt1, string txt2, string txt3)
    {
        string[] s1 = { txt1, txt2, txt3 };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);
    }

    
}
