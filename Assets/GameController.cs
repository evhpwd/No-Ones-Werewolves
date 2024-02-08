using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    IReadOnlyCollection<string> names = new List<string> {
        "Margaret",
        "Jamie",
        "Evie",
        "LouisD",
        "LouisM",
        "Finn",
        "Molly",
        "Michelle",
        "Barack"
    };

    public NPCController pawnPrefab;

    void Start()
    {
        foreach (string name in names)
        {
            NPCController character = Instantiate(pawnPrefab, Vector3.zero, Quaternion.identity);
            character.PawnName = name;
        }
    }

    void Update()
    {
        
    }
}
