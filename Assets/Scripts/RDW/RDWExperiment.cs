using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;


public class RDWExperiment : MonoBehaviour
{
    public GameObject leftObj;
    public GameObject rightObj;
    public GameObject rightWall;
    public GameObject leftWall;
    public GameObject rightWalls;
    public GameObject leftWalls;
    int count;
    Color defaultColor;
    public bool changeColor;
    public bool perEye = true;
    
    private int stepCount;
    private AudioSource audioSource;
    public AudioClip triggerAudio;
    public int loopCount = 1;
    public float inducedTime = 0.5f;
    public float intervalTime = 0.5f;
    public OVRScreenFade OVRScreenFade;
    public int camRotationValue;
    public GameObject cam;
    public Camera centerEyeAnchor;
    public Camera rightEyeAnchor;
    public Camera leftEyeAnchor;
    public CSVWriter CSVWriter;
    public Animator plane;
    public GameObject rightPlane;
    public GameObject leftPlane;
    bool rotStart = false;
    float speed = 1f;
    float rotAngle = 360f;
    float variation;
    float rot;
    bool isRotatingRight;
    float unitRotationValue;
    public GameObject buttonUI;
    public GameObject endUI;
    public Canvas ui;
    public Image backR;
    public Image backL;
    public GameObject distractor;
    public GameObject distractorPos;
    public GameObject controller;
    public AudioClip buttonAudio;
    public Text answerText;
    
    // independet values
    public List<float> rotationValues;
    
    private float rotationValue;
    public int trialCount;
    public bool smoothRotation;
    public bool randomizeCamRot = true;
    public bool rotZ;
    public bool showAnswer;


    // Start is called before the first frame update
    void Start()
    {
        


        defaultColor = rightWall.GetComponent<Renderer>().material.color;

        audioSource = GetComponent<AudioSource>();

        // cam.transform.rotation = new Quaternion(0, 0, 0, 0);
        rightPlane.SetActive(false);
        leftPlane.SetActive(false);

        // 回転角度のセットを用意
        int num = rotationValues.Count;
        for (int i=0; i<num; i++) {
            float item = rotationValues[i];
            for (int j=0; j<trialCount-1; j++) {  
                rotationValues.Add(item);
            }
        }


        backL.enabled = false;
        backR.enabled = false;

        // controller.SetActive(true);
        

    }

    public void RecordAnswer(int answer) {
        CSVWriter.SaveData(rotationValue, camRotationValue, answer);
        audioSource.PlayOneShot(buttonAudio);

        if (answer == 0) {
            backL.enabled = true;
            backR.enabled = false;
        } else {
            backL.enabled = false;
            backR.enabled = true;
        }

        if (showAnswer) answerText.text = rotationValue.ToString();

        
        
    }

    // Update is called once per frame
    void Update()
    {
        // 回答を記録する
        if (Input.GetKeyDown(KeyCode.S)) {
            RecordAnswer(1);
        } else if (Input.GetKeyDown(KeyCode.A)) {
            RecordAnswer(0);

        } else if (Input.GetKeyDown(KeyCode.D)) {
            // デバッグ用
            rightObj.transform.Rotate(0, 5, 0);
            leftObj.transform.Rotate(0, 5, 0);
        } else if (Input.GetKeyDown(KeyCode.F)) {
            // デバッグ用
            rightObj.transform.Rotate(0, -5, 0);
            leftObj.transform.Rotate(0, -5, 0);
        }

        // if (OVRInput.GetDown(OVRInput.RawButton.A))
        // {
        //     CSVWriter.CloseSW();
        // }
        
        // 次の試行を開始する
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (showAnswer) answerText.text = rotationValue.ToString();
            SwicthNextTrial();

        }


        if (rotStart) {
            ProcessSmoothRotation(isRotatingRight);
        }
    }

     private void ManipulateObjects() {
        

         // 左右のオブジェクトの回転をリセットする
        rightObj.transform.rotation = new Quaternion(0, 0, 0, 0);
        leftObj.transform.rotation = new Quaternion(0, 0, 0, 0);

        // カメラの初期角度を決定して回転させる
        camRotationValue = 10*Random.Range(0, 35);
        if (randomizeCamRot) cam.transform.Rotate(0, camRotationValue, 0);

        distractor.transform.rotation = new Quaternion(0, 0, 0, 0);
        distractor.transform.rotation = centerEyeAnchor.transform.rotation;

        Vector3 rotvec = centerEyeAnchor.transform.rotation.eulerAngles;

        Quaternion rot = Quaternion.Euler(0, rotvec.y, 0);
        distractor.transform.rotation = rot;
        // distractor.transform.RotateAround(Vector3.zero, Vector3.up,centerEyeAnchor.transform.rotation.y);

        answerText.text = "";
    }

    public void SwicthNextTrial() {
        
        if (rotationValues.Count > 0) {

            audioSource.PlayOneShot(buttonAudio);

            controller.SetActive(false);

            buttonUI.SetActive(false);
            backL.enabled = false;
            backR.enabled = false;
        
                

            // 回転角を決定する
            int rand = Random.Range(0, rotationValues.Count);
            rotationValue = rotationValues[rand];
            rotationValues.RemoveAt(rand);
            Debug.Log(rotationValue);

            // 左右の目どちらから回転を始めるかを決定する
            float v = Random.value;
            // if (v < 0.5) {
            //     StartCoroutine(StartRotation(loopCount, rotationValue));
            // } else {
            //     StartCoroutine(StartRotationL(loopCount, rotationValue));
            // }

            StartCoroutine(StartRotation(loopCount, rotationValue));

            
            if (stepCount >= 3) {
                stepCount = 0;
            }
        } else {
            endUI.SetActive(true);
            CSVWriter.CloseSW();
        }
    }

    private void ProcessSmoothRotation(bool isRotatingRight) {
        // 時間を指定して回転させる
        if (rotStart) {
            // 1秒間の変化量
            variation = unitRotationValue / inducedTime;
            GameObject target;

            if (isRotatingRight) {
                target = rightObj;
            } else {
                target = leftObj;
            }


                target.transform.Rotate (0, variation * Time.deltaTime, 0);
                rot += variation * Time.deltaTime;
                if (Mathf.Abs(rot) >= Mathf.Abs(rotationValue)) {
                    rotStart = false;
                    target.transform.rotation = Quaternion.Euler (0, rotationValue, 0);
                }
        }
    }

    public void ChangeWallsColor(GameObject walls, Color col) {
        if (walls.GetComponent<Renderer>()) {
            walls.GetComponent<Renderer>().material.color = col;
        } else {
            foreach (Transform wall in walls.transform)
            {
                wall.gameObject.GetComponent<Renderer>().material.color = col;
            }
        }
        
        
    }

    private void RotateObject(GameObject obj, float rotValue) {
        if (!smoothRotation) {
            // 離散回転
            obj.transform.Rotate(0, rotValue, 0);
        } else {
            // 連続回転
            rot = 0f;
            variation = rotationValue / speed;
            
            rotStart = true;
        }
        
    }

    private void SwitchPlaneState(int state) {
        switch (state)
        {
            // 両方ともオフ
            case 0:
                rightPlane.SetActive(false);
                leftPlane.SetActive(false);
                break;
            // 右目オン
            case 1:
                rightPlane.SetActive(true);
                leftPlane.SetActive(false);
                break;
            // 左目オン
            case 2:
                rightPlane.SetActive(false);
                leftPlane.SetActive(true);
                break;
            default:
                break;
        }
        
    }

    IEnumerator coSub(float c, bool isRight)
    {
        int state = 0;
        if (isRight) {
            state = 0;
        } else {
            state = 1;
        }

        while (count > 0) {

            isRotatingRight = false;
            RotateObject(leftObj, unitRotationValue);

            if (changeColor) {
                ChangeWallsColor(rightWalls, Color.red);
            } else {
                SwitchPlaneState(state);
            }

            yield return new WaitForSeconds(inducedTime);
            
            // ChangeWallsColor(rightWalls, defaultColor);

            SwitchPlaneState(0);
            
            yield return new WaitForSeconds(intervalTime);
            count --;
        }
    }

    private IEnumerator StartRotation(int loopCount, float rotationValue) {

        
        count = loopCount;
        // 分割時の1回あたりの回転量を計算する
        unitRotationValue = rotationValue/(float)loopCount;

        // ブラックアウト
        plane.SetTrigger("OnPlane");

        // 被験者には見えない挙動------
        yield return new WaitForSeconds(2f);

        ManipulateObjects();


        // ------

        yield return new WaitForSeconds(2.5f);
        audioSource.PlayOneShot(triggerAudio);
        yield return new WaitForSeconds(3f);

        if (perEye) {

            while (count > 0) {

                

                if (changeColor) {
                    ChangeWallsColor(rightWalls, Color.red);
                    // rightWall.GetComponent<Renderer>().material.color = Color.red;
                } else {
                    rightPlane.SetActive(true);
                    leftPlane.SetActive(false);
                }

                // yield return new WaitForSeconds(0.2f);

                isRotatingRight = false;
                RotateObject(leftObj, unitRotationValue);

                yield return new WaitForSeconds(inducedTime);
                
                // ChangeWallsColor(rightWalls, defaultColor);

                rightPlane.SetActive(false);
                leftPlane.SetActive(false);
                // rightWall.GetComponent<Renderer>().material.color = defaultColor;
                yield return new WaitForSeconds(intervalTime);
                count --;
                
            }

            count = loopCount;

            while (count > 0) {

                
                if (changeColor) {
                    ChangeWallsColor(leftWalls, Color.red);
                    // leftWall.GetComponent<Renderer>().material.color = Color.red;
                } else {
                    rightPlane.SetActive(false);
                leftPlane.SetActive(true);
                }

                // yield return new WaitForSeconds(0.5f);
                isRotatingRight = true;
                RotateObject(rightObj, unitRotationValue);
                
                yield return new WaitForSeconds(inducedTime);
                // ChangeWallsColor(leftWalls, defaultColor);

                rightPlane.SetActive(false);
                leftPlane.SetActive(false);
                // leftWall.GetComponent<Renderer>().material.color = defaultColor;
    
                yield return new WaitForSeconds(intervalTime);
                count --;
            }


            yield return new WaitForSeconds(1f);
            EndRotation();

        } 
        
    }

    private void EndRotation() {
        audioSource.PlayOneShot(triggerAudio);
        buttonUI.SetActive(true);
        controller.SetActive(true);

        
    }




    private IEnumerator StartRotationL(int loopCount, float rotationValue) {

        
        count = loopCount;
        unitRotationValue = rotationValue/(float)loopCount;

        plane.SetTrigger("OnPlane");

        yield return new WaitForSeconds(2f);

        ManipulateObjects();

        
        
        yield return new WaitForSeconds(2.5f);
        audioSource.PlayOneShot(triggerAudio);
        yield return new WaitForSeconds(3f);
        if (perEye) {

            while (count > 0) {
                // rightObj.transform.Rotate(0, unitRotationValue, 0);
                
                if (changeColor) {
                    ChangeWallsColor(leftWalls, Color.red);
                    // leftWall.GetComponent<Renderer>().material.color = Color.red;
                } else {
                    rightPlane.SetActive(false);
                leftPlane.SetActive(true);
                }

                // yield return new WaitForSeconds(0.5f);
                isRotatingRight = true;
                RotateObject(rightObj, unitRotationValue);
                
                yield return new WaitForSeconds(inducedTime);
                // ChangeWallsColor(leftWalls, defaultColor);

                rightPlane.SetActive(false);
                leftPlane.SetActive(false);
                // leftWall.GetComponent<Renderer>().material.color = defaultColor;
    
                yield return new WaitForSeconds(intervalTime);
                count --;
            }


            count = loopCount;

            
            while (count > 0) {
                // leftObj.transform.Rotate(0, unitRotationValue, 0);
                
                if (changeColor) {
                    ChangeWallsColor(rightWalls, Color.red);
                    // rightWall.GetComponent<Renderer>().material.color = Color.red;
                } else {
                rightPlane.SetActive(true);
                leftPlane.SetActive(false);
                }

                // yield return new WaitForSeconds(0.2f);


                isRotatingRight = false;
                RotateObject(leftObj, unitRotationValue);

                yield return new WaitForSeconds(inducedTime);
                
                // ChangeWallsColor(rightWalls, defaultColor);

                rightPlane.SetActive(false);
                leftPlane.SetActive(false);
                // rightWall.GetComponent<Renderer>().material.color = defaultColor;
                yield return new WaitForSeconds(intervalTime);
                count --;
                
            }

            yield return new WaitForSeconds(1f);
            EndRotation();
        } 
        
    }
}
