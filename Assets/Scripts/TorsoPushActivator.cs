using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoPushActivator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        TorsoPushBehaviour target = other.gameObject.GetComponent<TorsoPushBehaviour>();
        if (target) {
            target.active = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        TorsoPushBehaviour target = other.gameObject.GetComponent<TorsoPushBehaviour>();
        if (target) {
            target.active = false;
            target.ResetDeformation();
        }
    }
}
