using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVWriter : MonoBehaviour
{
    private StreamWriter sw;
    string filePath;
    // public string fileName;
    public bool init;
    public AudioClip audioclip;
    
    private void Start() {
        InitCSV();
    }

    private void InitCSV() {
        if (!init) {
            Debug.Log("init");


            filePath = Application.persistentDataPath;

            #if UNITY_EDITOR
            filePath = Application.dataPath;
            

            #elif UNITY_ANDROID
            filePath = Application.persistentDataPath;
            #endif

            
            if(!PlayerPrefs.HasKey("num")) {
                PlayerPrefs.SetInt("num", 1);
                
            } else {
                int num = PlayerPrefs.GetInt("num");
                PlayerPrefs.SetInt("num", num+1);
            }
            PlayerPrefs.Save();
            
            string fileName = PlayerPrefs.GetInt("num").ToString() + ".csv";
            Debug.Log(fileName);

            var combinedPath = Path.Combine(filePath, fileName);
            sw = new StreamWriter(combinedPath);

            string[] s1 = { "RotationValue", "InitialCamRotationValue", "Answer", "Correction", "Result"};
            string s2 = string.Join(",", s1);
            sw.WriteLine(s2);

            
            
            init = true;

            GetComponent<AudioSource>().PlayOneShot(audioclip);
        }
    }
 
    public void SaveData(float rotationValue, float camRotationValue, int answer)
    {
        if (init) {
            int correction, result;
            if (rotationValue > 0) {
                correction = 1;
                if (correction == answer) {
                    result = 1;
                } else {
                    result = 0;
                }
            } else if (rotationValue < 0) {
                correction = 0;
                if (correction == answer) {
                    result = 1;
                } else {
                    result = 0;
                }
            } else {
                correction = 2;
                result = 2;
            }

            string[] s1 = { 
                rotationValue.ToString(), 
                camRotationValue.ToString(), 
                answer.ToString(), 
                correction.ToString(), 
                result.ToString()};
            string s2 = string.Join(",", s1);
            sw.WriteLine(s2);
        }

        
        
    }

    public void CloseSW() {
        sw.Close();
        Debug.Log("close");
    }

    // private void OnApplicationQuit() {
    //     CloseSW();
    // }
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CloseSW();
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            InitCSV();
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            PlayerPrefs.DeleteKey("num");
            Debug.Log("delete");
        }
    }
}
