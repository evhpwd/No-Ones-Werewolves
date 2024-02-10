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
        if (current_job is null || current_job.Done())
        {
            NextJob();
        }
        current_job.Tick();
    }

    // Dequeues the next job into `current_job`.
    // If there are no queued jobs:
    //   - If the last job was `Idle`, add a `Walk` job
    //   - Otherwise, add an `Idle` job
    // There will always be a `current_job` at the end of this
    void NextJob()
    {
        if (jobs.Count > 0)
        {
            current_job = jobs.Dequeue();
        }
        else if (current_job is Idle)
        {
            Vector3 dest = transform.position + (Vector3)Random.insideUnitCircle * 7.5f;
            current_job = new Walking(this, dest);
        }
        else
        {
            current_job = new Idle(Random.Range(1.5f, 2.5f));
        }
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
    // Return a value between 0 and 1 representing the progress on this task
    float Progress();
    bool Done();
    // Do whatever work is required for the task this frame
    void Tick();
    static string Description;
}

class Walking : IJob
{
    readonly Vector3 startPos;
    readonly Vector3 endPos;
    NPCController Pawn
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

class Idle : IJob
{
    readonly float startTime;
    readonly float endTime;
    float IJob.Progress()
    {
        var totalLength = endTime - startTime;
        var elapsed = Time.time - startTime;
        if (elapsed > 0)
        {
            return totalLength / elapsed;
        } else
        {
            return 0;
        }
    }
    bool IJob.Done()
    {
        return Time.time >= endTime;
    }
    void IJob.Tick() { }

    public Idle(float time)
    {
        startTime = Time.time;
        endTime = startTime + time;
    }
}