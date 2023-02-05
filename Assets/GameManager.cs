using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Objective> objectives = new List<Objective>();
    public Transform player;
    public int health;

    public List<Transform> GetObjectiveTransforms()
    {
        List<Transform> list = new List<Transform>();

        foreach(Objective objective in objectives)
        {
            list.Add(objective.GetTransform());
        }

       return list;
    }
}
