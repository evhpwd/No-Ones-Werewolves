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

    NPCController selectedPawn;

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
        Camera.main.orthographicSize = 10;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            NPCController pawn = null;
            if (hit.collider != null)
            {
                pawn = hit.collider.gameObject.GetComponent<NPCController>();
            }
            ChangeSelectedPawn(pawn);
        }
        else if (Input.GetMouseButtonDown(1) && selectedPawn is not null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedPawn.SetJob(new Walking(selectedPawn, pos));
            ChangeSelectedPawn(null);
        }

        CameraMovement();
    }

    void CameraMovement()
    {
        Vector2 movement = Vector2.zero;
        movement.x += Input.GetAxis("Horizontal");
        movement.y += Input.GetAxis("Vertical");
        if (movement.magnitude != 0)
        {
            Camera.main.transform.Translate(movement / 2);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.orthographicSize > 10)
            {
                Camera.main.orthographicSize--;
           }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.orthographicSize < 30)
            {
                Camera.main.orthographicSize++;
            }
        }

    }

    void ChangeSelectedPawn(NPCController newPawn)
    {
        if (selectedPawn != null)
        {
            selectedPawn.HideOutline();
        }
        selectedPawn = newPawn;
        if (selectedPawn != null)
        {
            selectedPawn.ShowOutline();
        }
    }
}
