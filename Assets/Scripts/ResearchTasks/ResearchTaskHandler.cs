using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchTaskHandler : MonoBehaviour
{
    int reward = 0;
    int time = 0;

    [System.Serializable]
    struct task
    {
        [SerializeField]
        public int reward;
        [SerializeField]
        public int time;
        [SerializeField]
        public int cost;

    }
    [SerializeField]
    List<task> tasks = new List<task>();

    task newTask;

    GameObject createdTask;

    [SerializeField]
    GameObject taskParent;

    [SerializeField]
    GameObject taskScreen;

    [SerializeField]
    GameObject taskPrefab;

    Button buttonPressed;

    [SerializeField]
    GameObject taskButton;
    // Start is called before the first frame update
    void Start()
    {
        taskScreen.SetActive(false);
        spawnNewTask();
        spawnNewTask();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //Spawn the button
    public void spawnNewTask()
    {
        newTask = tasks[Random.Range(0, tasks.Count)];
        createdTask = Instantiate(taskPrefab);
        createdTask.transform.SetParent(taskParent.transform);
        createdTask.GetComponent<Task>().setReward(newTask.reward);
        createdTask.GetComponent<Task>().setTaskTime(newTask.time);
        createdTask.GetComponent<Task>().setCost(newTask.cost);

        createdTask.transform.GetChild(0).GetComponent<Text>().text = "Reward: " + newTask.reward.ToString();
        createdTask.transform.GetChild(1).GetComponent<Text>().text = "Time: " + newTask.time.ToString();
        createdTask.transform.GetChild(2).GetComponent<Text>().text = "Cost: £" + newTask.cost.ToString();

        createdTask.transform.localScale = new Vector3(1, 1, 1);
    }

    //Clicking on the button will start the task
    public void startNewTask(GameObject t)
    {
        t.GetComponent<Task>().setReward(t.GetComponent<Task>().getReward());
        t.GetComponent<Task>().setTaskTime(t.GetComponent<Task>().getTime());
        t.GetComponent<Task>().setTaskTime(t.GetComponent<Task>().getCost());
        t.GetComponent<Task>().startTask();
    }

    public void completeTask()
    {
        spawnNewTask();
    }

    public void showTasks(bool set)
    {
        taskScreen.SetActive(set);
        GameObject.Find("ResearchTasksButton").transform.GetChild(1).gameObject.SetActive(false);
    }
}
