using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInfo : MonoBehaviour
{
    public Text infoText;

    private bool displaying = false;
    private float timer = -10.0f;
    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0.0f) {
            timer -= Time.deltaTime;
        }
        if(timer <= 0.0f && timer != -8.0f) {
            StopDisplaying();
            timer = -8.0f;
        }
    }

    public bool DisplayTimed(string text, float time) {
        if (displaying) {
            return false;
        }
        timer = time;
        infoText.text = text;
        Show();
        displaying = true;
        return true;
    }

    public bool Display(string text) {
        if(displaying) {
            return false;
        }
        timer = -8.0f;
        infoText.text = text;
        Show();
        displaying = true;
        return true;
    }

    public void StopDisplaying() {
        if(displaying) {
            Hide();
            displaying = false;
        }
    }

    public void SetTimeOut(float time) {
        if (displaying) {
            timer = time;
        }
    }
    void Hide() {
        GetComponent<CanvasGroup>().alpha = 0f;
        //transform.position -= new Vector3(100.0f, 0.0f, 0.0f);
    }

    void Show() {
        GetComponent<CanvasGroup>().alpha = 1f;
        //transform.position += new Vector3(100.0f,0.0f,0.0f);
    }
}
