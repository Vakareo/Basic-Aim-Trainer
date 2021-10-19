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
    private float currentHalfDistance;
    private Vector3 initialPosition;
    private int switchCount;
    private float currentSpeed;
    private float currentOffset;
    private int startSide;
    private bool isPlaying;
    public float respawnTime;
    private float currentTime;

    private void OnValidate()
    {
        minSpeed = Mathf.Min(maxSpeed, minSpeed);
    }


    private void InitializeObject()
    {
        targetObject = Instantiate(targetPrefab, transform.position, Quaternion.identity, transform);
        targetTransform = targetObject.transform;
        targetObject.SetActive(false);
    }

    private void SetNewCurrents()
    {
        var depth = depthRanges.GetRange();
        var horizontal = horizontalRanges.GetRange();
        currentHalfDistance = UnityEngine.Random.Range(horizontal.startRange, horizontal.endRange) * 0.5f;
        currentDepth = UnityEngine.Random.Range(depth.startRange, depth.endRange) + initialPosition.z;
        currentHeight = UnityEngine.Random.Range(0, maxHeight) + initialPosition.y;
        currentSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        currentOffset = GetRandomOffset();
        startSide = UnityEngine.Random.Range(0, 2);
    }

    private void RespawnObjectToNextPosition()
    {
        switchCount = 0;
        var value = GetNewTravel();
        SetNewTravel();
        targetTransform.position = value;
    }

    private void Update()
    {
        if (isPlaying)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= respawnTime)
            {
                SetNewCurrents();
                RespawnObjectToNextPosition();
                currentTime = 0;
            }
            var direction = (travelTo - targetTransform.position).normalized;
            var move = direction * currentSpeed * Time.deltaTime;
            targetTransform.position += move;
            var newDirection = (travelTo - targetTransform.position).normalized;
            // Vector3.Dot(direction, newDirection) < 0
            if (newDirection != direction)
            {
                SetNewTravel();
            }
        }
    }
    private Vector3 GetNewTravel()
    {
        var odd = (switchCount + startSide) & 1;
        var randDistance = currentHalfDistance + GetRandomRadius();
        var tempDistance = (randDistance * odd) + ((-randDistance) * (odd - 1) * -1);
        return new Vector3(tempDistance + initialPosition.x + currentOffset, currentHeight, currentDepth);
    }

    private void SetNewTravel()
    {
        switchCount++;
        travelTo = GetNewTravel();
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

    private void OnDestroy()
    {
        Stop();
    }

    public void Play()
    {
        initialPosition = transform.position;
        SetNewCurrents();
        RespawnObjectToNextPosition();
        targetObject.SetActive(true);
        isPlaying = true;
    }

    public void Stop()
    {
        isPlaying = false;
        targetObject.SetActive(false);
    }

    public void Initialize()
    {
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
}
