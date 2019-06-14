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
        transform.position -= new Vector3(100.0f, 0.0f, 0.0f);
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
        transform.position += new Vector3(100.0f, 0.0f, 0.0f);
        displaying = true;
        return true;
    }

    public bool Display(string text) {
        if(displaying) {
            return false;
        }
        timer = -8.0f;
        infoText.text = text;
        transform.position += new Vector3(100.0f,0.0f,0.0f);
        displaying = true;
        return true;
    }

    public void StopDisplaying() {
        if(displaying) {
            transform.position -= new Vector3(100.0f, 0.0f, 0.0f);
            displaying = false;
        }
    }

    public void SetTimeOut(float time) {
        if (displaying) {
            timer = time;
        }
    }
}
