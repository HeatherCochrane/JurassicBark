using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenges : MonoBehaviour
{
    int researchTaskCompleted = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void completeTask()
    {
        researchTaskCompleted += 1;
        checkResearchTasks();
    }

    public void checkResearchTasks()
    {
        if(researchTaskCompleted == 5)
        {
            Debug.Log("ACHIEVEMENT EARNED");
        }
    }
}
