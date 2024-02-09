using UnityEngine;
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

    public Sprite treeSprite;

    void Start()
    {
        foreach (string name in names)
        {
            NPCController character = Instantiate(pawnPrefab, Vector3.zero, Quaternion.identity);
            character.PawnName = name;
        }

        for (int i = 0; i < 10; i++)
        {
            GameObject tree = new GameObject();
            var treePos = (Vector3)Random.insideUnitCircle * 10;
            treePos.z = 0;
            tree.transform.position = treePos;
            tree.name = "Tree";
            var sprite = tree.AddComponent<SpriteRenderer>();
            sprite.sprite = treeSprite;
        }
    }
}
