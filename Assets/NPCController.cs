using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPCController: MonoBehaviour
{
    public string PawnName {
        get;
        set;
    }

    Queue<IJob> jobs;
    IJob current_job;
    [SerializeField]
    Material outlineMaterial;

    GameObject outline;

    void Start()
    {
        jobs = new Queue<IJob>();
        var nameCanvas = transform.Find("NameCanvas");
        nameCanvas.GetComponent<Canvas>().pixelPerfect = true;
        nameCanvas.GetComponent<Text>().text = PawnName;
        name = "Pawn (" + PawnName + ")";
        outline = transform.Find("Sprite/Outline").gameObject;
        Vector2[] offsets = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Sprite mySprite = transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite;
        foreach (Vector2 off in offsets)
        { 
            var obj = new GameObject();
            obj.transform.parent = outline.transform;
            obj.transform.position = new Vector3(off.x / 10, off.y / 10, 11);
            var render = obj.AddComponent<SpriteRenderer>();
            render.material = outlineMaterial;
            render.sprite = mySprite;
        }
        outline.SetActive(false);
    }

    void Update()
    {
        if ((current_job is null || current_job.Done()) && !jobs.TryDequeue(out current_job)) {
            Vector3 dest = transform.position + (Vector3)Random.insideUnitCircle * 5;
            current_job = new Walking(this, dest);
        }
        current_job.Tick();
    }

    public void ShowOutline()
    {
        outline.SetActive(true);
    }

    public void HideOutline()
    {
        outline.SetActive(false);
    }
}

interface IJob
{
    float Progress();
    bool Done();
    void Tick();
    static string Description;
    NPCController Pawn
    {
        get;
        set;
    }
}

class Walking : IJob
{
    Vector3 startPos;
    Vector3 endPos;
    public NPCController Pawn
    {
        get;
        set;
    }

    float IJob.Progress()
    {
        float walkLength = (endPos - startPos).magnitude;
        float distance = (Pawn.transform.position - startPos).magnitude;
        return walkLength / distance;
    }

    bool IJob.Done()
    {
        return (endPos - Pawn.transform.position).magnitude < 0.1f;
    }

    void IJob.Tick()
    {
        Pawn.transform.position = Vector3.MoveTowards(Pawn.transform.position, endPos, Time.deltaTime);
    }

    public Walking(NPCController pawn, Vector3 destination)
    {
        Pawn = pawn;
        startPos = pawn.transform.position;
        endPos = destination;
    }
}