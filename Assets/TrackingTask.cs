using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTask : MonoBehaviour, IGameMode
{
    [SerializeField] GameObject targetPrefab;
    [Range(0, 10f)]
    public float maxDepth = 5f;
    [Range(0, 20f)]
    public float maxHorizontal = 10f;
    [Range(0, 10f)]
    public float minSpeed = 1f;
    [Range(0, 10f)]
    public float maxSpeed = 2f;
    [Range(0, 10f)]
    public float maxHeight = 4f;
    [Range(0, 10f)]
    public float maxOffset = 5f;
    [Range(0, 5f)]
    public float switchRadius = 1f;
    private Transform targetTransform;
    private GameObject targetObject;
    private WeightedSelection horizontalRanges;
    private WeightedSelection depthRanges;
    private Vector3 travelTo;
    private float currentDepth;
    private float currentHeight;
    private float halfDistance;
    private Vector3 initialPosition;
    private int switchCount;
    private float currentSpeed;
    private float currentOffset;
    private void OnValidate()
    {
        minSpeed = Mathf.Min(maxSpeed, minSpeed);
    }

    private void Awake()
    {
        totalCount = 1;
        initialPosition = transform.position;
        var tempRanges = new WeightedRange[]{
            new WeightedRange(GetHorizontal(0.5f) , GetHorizontal(1f), 85),
            new WeightedRange(GetHorizontal(0.25f) , GetHorizontal(0.5f), 10),
            new WeightedRange(GetHorizontal(0.15f) , GetHorizontal(0.25f), 5)
        };
        horizontalRanges = new WeightedSelection(tempRanges);

        tempRanges = new WeightedRange[]{
            new WeightedRange(GetDepth(0.5f) , GetDepth(1f), 85),
            new WeightedRange(GetDepth(0.25f) , GetDepth(0.5f), 10),
            new WeightedRange(GetDepth(0.15f) , GetDepth(0.25f), 5)
        };
        depthRanges = new WeightedSelection(tempRanges);
        InitializeObject();
    }

    private void InitializeObject()
    {
        var depth = depthRanges.GetRange();
        var horizontal = horizontalRanges.GetRange();
        halfDistance = UnityEngine.Random.Range(horizontal.startRange, horizontal.endRange) * 0.5f;
        currentDepth = UnityEngine.Random.Range(depth.startRange, depth.endRange) + initialPosition.z;
        currentHeight = UnityEngine.Random.Range(0, maxHeight) + initialPosition.y;
        currentSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        currentOffset = GetRandomOffset();
        targetObject = Instantiate(targetPrefab, StartPos(), Quaternion.identity, transform);
        targetTransform = targetObject.transform;
    }

    private Vector3 StartPos()
    {
        switchCount = 0;
        SetNewTravel();
        var value = travelTo;
        SetNewTravel();
        return value;
    }

    private void Update()
    {
        var direction = (travelTo - targetTransform.position).normalized;
        var move = direction * currentSpeed * Time.deltaTime;
        targetTransform.position += move;
        var newDirection = (travelTo - targetTransform.position).normalized;
        if (newDirection != direction)
        {
            SetNewTravel();
        }
    }

    private void SetNewTravel()
    {
        var odd = switchCount & 1;
        var randDistance = halfDistance + GetRandomRadius();
        var tempDistance = ((randDistance) * odd) + ((-randDistance) * (odd - 1) * -1);
        switchCount++;
        travelTo = new Vector3(tempDistance + initialPosition.x + currentOffset, currentHeight, currentDepth);
    }

    private float GetRandomRadius()
    {
        return UnityEngine.Random.Range(-switchRadius * 0.5f, switchRadius * 0.5f);
    }

    private float GetRandomOffset()
    {
        return UnityEngine.Random.Range(-maxOffset, maxOffset);
    }

    private float GetDepth(float percent)
    {
        return maxDepth * percent;
    }

    private float GetHorizontal(float percent)
    {
        return maxHorizontal * percent;
    }

    public void Play()
    {

    }

    public void Stop()
    {

    }
}
