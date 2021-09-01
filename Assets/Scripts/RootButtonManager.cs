using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RootButtonManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip selectAudio;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectScene(int num) {
        Debug.Log("push");
        StartCoroutine(ChangeScene(num));
    }

    public IEnumerator ChangeScene(int num) {
        audioSource.PlayOneShot(selectAudio);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(num);
    }
}
