﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float SingleNodeMoveTime = 0.1f;

    public EnvironmentTile CurrentPosition { get; set; }

    bool currentlyMoving = false;
    int foodWater = 0;


    private void Start()
    {

    }
    private IEnumerator DoMove(Vector3 position, Vector3 destination)
    {
        // Move between the two specified positions over the specified amount of time
        if (position != destination)
        {
            transform.rotation = Quaternion.LookRotation(destination - position, Vector3.up);

            Vector3 p = transform.position;
            float t = 0.0f;

            while (t < SingleNodeMoveTime)
            {
                t += Time.deltaTime;
                p = Vector3.Lerp(position, destination, t / SingleNodeMoveTime);
                transform.position = p;
                yield return null;
            }
        }
    }

    private IEnumerator DoGoTo(List<EnvironmentTile> route)
    {
        int takeCount = 0;

        // Move through each tile in the given route
        if (route != null)
        {
            currentlyMoving = true;
            Vector3 position = CurrentPosition.Position;
            for (int count = 0; count < route.Count; ++count)
            {
                Vector3 next = route[count].Position;
                yield return DoMove(position, next);
                CurrentPosition = route[count];
                route[count].IsAccessible = false;
                if (route[count] != route[0])
                {
                    route[count - 1].IsAccessible = true;
                }
                position = next;
            }

        }
       
        currentlyMoving = false;

        if (this.CurrentPosition.hasFoodBowl || this.CurrentPosition.hasWaterBowl && takeCount == 0)
        {
            if(foodWater == 0)
            {
                this.GetComponent<DogBehaviour>().drinkWater();
            }
            else if(foodWater == 1)
            {
                this.GetComponent<DogBehaviour>().eatFood();
            }

            this.CurrentPosition.GetComponentInChildren<FoodWater>().removePiece();
            takeCount += 1;

        }

    }

    public void GoTo(List<EnvironmentTile> route, int fW)
    {
        // Clear all coroutines before starting the new route so 
        // that clicks can interupt any current route animation
        foodWater = fW;
        currentlyMoving = false;
        StopAllCoroutines();
        StartCoroutine(DoGoTo(route));
    }

    public bool getIfMoving()
    {
        return currentlyMoving;
    }
}
