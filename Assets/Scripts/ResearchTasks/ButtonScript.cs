using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    ResearchTaskHandler handler;
    // Start is called before the first frame update
    void Start()
    {
        handler = GameObject.Find("ResearchTaskHandler").GetComponent<ResearchTaskHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startTask()
    {
        if (!this.GetComponent<Task>().alreadyStarted())
        {
            handler.startNewTask(this.gameObject);
        }
    }
}
