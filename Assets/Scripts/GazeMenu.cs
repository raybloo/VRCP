using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeMenu : MonoBehaviour
{
    private GameObject lastMenuItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position,transform.forward * 100.0f);
        Debug.DrawRay(transform.position, transform.forward * 100.0f);
        if(Physics.Raycast(ray,out hit)) {
            if (hit.collider.tag == "Menu Item") {
                GameObject menuItem = hit.collider.gameObject;
                if (menuItem) {
                    MenuBehaviour menuBehaviour = menuItem.GetComponent<MenuBehaviour>();
                    if (menuBehaviour) {
                        if (lastMenuItem && menuItem != lastMenuItem) {
                            lastMenuItem.GetComponent<MenuBehaviour>().accTime = 0.0f;
                        }
                        if (!menuBehaviour.blocked) {
                            menuBehaviour.accTime += Time.deltaTime;
                        }
                        lastMenuItem = menuItem;
                    } else {
                        Debug.Log("menu item has no behaviour");
                    }
                }  
            } else if(lastMenuItem) {
                lastMenuItem.GetComponent<MenuBehaviour>().accTime = 0.0f;
            }
        } else if(lastMenuItem) {
            lastMenuItem.GetComponent<MenuBehaviour>().accTime = 0.0f;
        } 
    }
}
