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

    void Start()
    {
        jobs = new Queue<IJob>();
        var nameCanvas = transform.Find("NameCanvas");
        nameCanvas.GetComponent<Canvas>().pixelPerfect = true;
        nameCanvas.GetComponent<Text>().text = PawnName;
    }

    void Update()
    {
        if ((current_job is null || current_job.Done()) && !jobs.TryDequeue(out current_job)) {
            Vector3 dest = transform.position + (Vector3)Random.insideUnitCircle * 5;
            current_job = new Walking(this, dest);
        }
        current_job.Tick();
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