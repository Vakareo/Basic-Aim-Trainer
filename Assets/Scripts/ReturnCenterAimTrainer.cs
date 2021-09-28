using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnCenterAimTrainer : MonoBehaviour
{
    public GameObject targetPrefab;
    private GameObject spawnedTarget;
    private TargetComponent targetComponent;
    public float maxDistance = 20f;
    public float minDistance = 2f;
    public float maxHeight = 5f;
    public float maxDepth = 6f;
    public float time = 2f;
    private float waitTime;
    private Vector3 initialPosition;
    private Vector3 newPosition;
    private int spawnCount = 0;
    public WeightedSelection outerRanges;
    private bool isHit;
    private float totalTime;
    private int totalCount;

    public static Action<int> OnUpdatedAvg;
    private int avgKillTime;

    private void OnValidate()
    {
        initialPosition = transform.position;
    }

    private void Awake()
    {
        totalCount = 1;
        initialPosition = transform.position;
        var tempRanges = new WeightedRange[]{
            new WeightedRange(GetOuterPercent(0.7f) , GetOuterPercent(1f), 80),
            new WeightedRange(GetOuterPercent(0.3f) , GetOuterPercent(0.5f), 15),
            new WeightedRange(GetOuterPercent(0f) , GetOuterPercent(0.1f), 5)
        };
        outerRanges = new WeightedSelection(tempRanges);
        InitializeObject();
    }
    private void InitializeObject()
    {

        spawnedTarget = Instantiate(targetPrefab, GetRandomCenter(), Quaternion.identity, transform);
        targetComponent = spawnedTarget.AddComponent<TargetComponent>();
        targetComponent.OnHit += OnHit;
        spawnCount = 1;
    }

    private void OnDestroy()
    {
        targetComponent.OnHit -= OnHit;
    }

    private void OnHit()
    {
        isHit = true;
    }

    private void SetAvgKillTime()
    {
        avgKillTime = (int)(totalTime / totalCount * 1000f);
        OnUpdatedAvg?.Invoke(avgKillTime);
    }

    private Vector3 GetRandomCenter()
    {
        float randomZ = GetRandom(maxDepth);
        float randomY = GetRandom(maxHeight);
        return new Vector3(initialPosition.x, initialPosition.y + randomY, initialPosition.z + randomZ);
    }

    private Vector3 GetRandomOuter()
    {
        float randomZ = GetRandom(maxDepth);
        float randomY = GetRandom(maxHeight);
        var range = outerRanges.GetRange();
        float randomX = UnityEngine.Random.Range(range.startRange, range.endRange) * GetRandomFlip();
        return new Vector3(initialPosition.x + randomX, initialPosition.y + randomY, initialPosition.z + randomZ);
    }
    private float GetRandom(float max)
    {
        return UnityEngine.Random.Range(0f, max);
    }

    private float GetRandomFlip()
    {
        int value = UnityEngine.Random.Range(0, 2);
        if (value == 0)
            return value - 1;
        return value;
    }

    private void Update()
    {
        waitTime += Time.deltaTime;
        if (isHit)
        {
            ResetObject();
            isHit = false;
        }
        if (waitTime >= time)
        {
            ResetObject();
        }
    }

    private void ResetObject()
    {
        var odd = spawnCount & 1;
        totalTime += waitTime;
        totalCount = spawnCount;
        SetAvgKillTime();
        if (odd == 1)
        {
            SetObjectToOuter();
        }
        else
        {
            SetObjectToCenter();
        }
    }

    private void SetObjectToCenter()
    {
        spawnedTarget.transform.position = GetRandomCenter();
        spawnCount++;
        waitTime = 0;
    }

    private void SetObjectToOuter()
    {
        spawnedTarget.transform.position = GetRandomOuter();
        spawnCount++;
        waitTime = 0;
    }

    private float GetOuterPercent(float percent)
    {
        return minDistance + ((maxDistance - minDistance) * percent);
    }
}
