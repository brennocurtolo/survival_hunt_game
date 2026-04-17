using UnityEngine;
using System.Collections;

public class TrailGuide : MonoBehaviour
{
    public Transform player;
    public Transform target;

    private TrailRenderer[] trails;

    [Header("Movement")]
    public float duration = 2f;
    private float reachDistance = 0.1f;

    [Header("Curve")]
    public float curveHeight = 3f;
    public float sideCurveStrength = 2f;

    [Header("Wave Effect")]
    public float waveAmplitude = 0.7f;
    public float waveFrequency = 10f;

    private float journeyTime = 0f;
    private float trailTimer = 0f;
    private float maxTrailTime = 0f;

    private Vector3 startPoint;
    private Vector3 controlPoint;

    private enum State { Idle, Moving, WaitingTrailEnd }
    private State currentState = State.Idle;

    void Awake()
    {
        trails = GetComponentsInChildren<TrailRenderer>();

        foreach (var t in trails)
        {
            if (t.time > maxTrailTime)
                maxTrailTime = t.time;
        }
    }

    public void StartTrail()
    {
        if (target == null) return;

        foreach (var t in trails)
        {
            t.emitting = false;
            t.Clear();
        }

        transform.position = player.position;

        startPoint = player.position;

        // cria uma curva lateral + altura
        Vector3 direction = (target.position - player.position).normalized;
        Vector3 side = Vector3.Cross(direction, Vector3.up);

        Vector3 midPoint = (player.position + target.position) / 2f;

        controlPoint =
            midPoint +
            Vector3.up * curveHeight +
            side * sideCurveStrength;

        journeyTime = 0f;

        StartCoroutine(EnableTrailNextFrame());

        currentState = State.Moving;
    }

    IEnumerator EnableTrailNextFrame()
    {
        yield return null;

        foreach (var t in trails)
        {
            t.emitting = true;
        }
    }

    void Update()
    {
        if (target == null) return;

        switch (currentState)
        {
            case State.Moving:
                MoveToTarget();
                break;

            case State.WaitingTrailEnd:
                WaitTrailToDisappear();
                break;
        }
    }

    void MoveToTarget()
    {
        journeyTime += Time.deltaTime;
        float t = journeyTime / duration;
        t = Mathf.Clamp01(t);

        // curva Bezier
        Vector3 a = Vector3.Lerp(startPoint, controlPoint, t);
        Vector3 b = Vector3.Lerp(controlPoint, target.position, t);
        Vector3 curvedPosition = Vector3.Lerp(a, b, t);

        // efeito de onda (energia viva)
        Vector3 direction = (target.position - startPoint).normalized;
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up);

        float wave = Mathf.Sin(t * Mathf.PI * waveFrequency) * waveAmplitude * (1 - t);

        curvedPosition += perpendicular * wave;

        transform.position = curvedPosition;

        if (Vector3.Distance(transform.position, target.position) < reachDistance || t >= 1f)
        {
            currentState = State.WaitingTrailEnd;
            trailTimer = 0f;
        }
    }

    void WaitTrailToDisappear()
    {
        trailTimer += Time.deltaTime;

        if (trailTimer >= maxTrailTime)
        {
            StartTrail();
        }
    }

    public void OnTargetCollected()
    {
        target = null;

        foreach (var t in trails)
        {
            t.emitting = false;
            t.Clear();
        }

        currentState = State.Idle;
    }

    public void SetNewTarget(Transform newTarget)
    {
        target = newTarget;
        StartTrail();
    }
}