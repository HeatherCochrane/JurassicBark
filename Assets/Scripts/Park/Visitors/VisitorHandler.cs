using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorHandler : MonoBehaviour
{

    [SerializeField]
    List<GameObject> visitors = new List<GameObject>();

    GameObject newVisitor;
    public List<GameObject> totalVisitors = new List<GameObject>();
    //This will increase depending on park
    int visitorMaxCount = 10;

    EnvironmentTile spawnTile;

    [SerializeField]
    Environment mMap;

    bool isParkClosed = false;

    [SerializeField]
    Currency currency;

    int admissionFee = 5;
    // Start is called before the first frame update
    void Start()
    {
        //spawnNewVisitor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSpawnPoint(EnvironmentTile tile)
    {
        spawnTile = tile;
    }

    void spawnNewVisitor()
    {
        if (!isParkClosed)
        {
            if (spawnTile != null)
            {
                newVisitor = Instantiate(visitors[Random.Range(0, visitors.Count)]);
                newVisitor.GetComponent<Visitor>().CurrentPosition = spawnTile;
                newVisitor.transform.position = new Vector3(spawnTile.transform.position.x + 5, spawnTile.transform.position.y + 3, spawnTile.transform.position.z + 5);
                totalVisitors.Add(newVisitor);
                currency.addMoney(admissionFee);
            }
            if (totalVisitors.Count < visitorMaxCount)
            {
                Invoke("spawnNewVisitor", Random.Range(2, 3));
            }
        }
    }
    

    public void parkClosed()
    {
        isParkClosed = true;

        if (totalVisitors.Count > 0)
        {
            for (int i = 0; i < totalVisitors.Count; i++)
            {
                totalVisitors[i].GetComponent<Visitor>().goToExit(spawnTile);
                totalVisitors[i].GetComponent<Visitor>().setParkClosed(true);
            }

            totalVisitors.Clear();
        }

    }

    public void parkOpen()
    {
        isParkClosed = false;
        spawnNewVisitor();
    }

    public void destroyVisitors()
    {
        if (totalVisitors.Count > 0)
        {
            for (int i = 0; i < totalVisitors.Count; i++)
            {
                if (totalVisitors[i] != null)
                {
                    totalVisitors[i].GetComponent<Visitor>().kill();
                }
            }

            totalVisitors.Clear();
        }
    }

}
