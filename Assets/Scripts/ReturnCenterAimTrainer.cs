using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnCenterAimTrainer : MonoBehaviour, IGameMode
{
    public GameObject targetPrefab;
    private GameObject targetSpawned;
    private TargetComponent targetComponent;
    [Range(1f, 25f)]
    public float maxDistance = 20f;
    [Range(0f, 5f)]
    public float minDistance = 2f;
    [Range(0f, 10f)]
    public float maxHeight = 5f;
    [Range(0f, 10f)]
    public float maxDepth = 6f;
    public float time = 2f;
    private float waitTime;
    private Vector3 initialPosition;
    private int spawnedCount = 0;
    public WeightedSelection outerRanges;
    private bool isHit;
    private float totalTime;
    public static Action<int> OnUpdatedAvg;
    private int avgKillTime;

    public bool isPlaying;

    private void OnValidate()
    {
        initialPosition = transform.position;
        minDistance = Mathf.Min(minDistance, maxDistance);
    }
    private void InitializeObject()
    {
        targetSpawned = Instantiate(targetPrefab, transform.position, Quaternion.identity, transform);
        targetComponent = targetSpawned.AddComponent<TargetComponent>();
        targetSpawned.SetActive(false);
    }
    private void Update()
    {
        if (isPlaying)
        {
            waitTime += Time.deltaTime;
            if (isHit)
            {
                SetObjectToNextPosition();
                isHit = false;
            }
            if (waitTime >= time)
            {
                SetObjectToNextPosition();
            }
        }
    }

    private void OnDestroy()
    {
        Stop();
    }

    private void OnHit()
    {
        isHit = true;
    }

    private void UpdateAvgKillTime()
    {
        avgKillTime = (int)(totalTime / spawnedCount * 1000f);
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


    private void SetObjectToNextPosition()
    {
        var odd = spawnedCount & 1;
        totalTime += waitTime;
        UpdateAvgKillTime();
        if (odd == 1)
        {
            SetObjectToOuter();
        }
        else
        {
            SetObjectToCenter();
        }
        spawnedCount++;
        waitTime = 0;
    }

    private void SetObjectToCenter()
    {
        targetSpawned.transform.position = GetRandomCenter();
    }

    private void SetObjectToOuter()
    {
        targetSpawned.transform.position = GetRandomOuter();
    }

    private float GetOuterPercent(float percent)
    {
        return minDistance + ((maxDistance - minDistance) * percent);
    }

    public void Play()
    {
        initialPosition = transform.position;
        targetComponent.OnHit += OnHit;
        InitializeRanges();
        ResetStats();
        targetSpawned.SetActive(true);
        targetSpawned.transform.position = GetRandomCenter();
        isPlaying = true;
    }

    private void ResetStats()
    {
        spawnedCount = 1;
        totalTime = 0;
        waitTime = 0;
        avgKillTime = 0;
    }

    private void InitializeRanges()
    {
        var tempRanges = new WeightedRange[]{
            new WeightedRange(GetOuterPercent(0.7f) , GetOuterPercent(1f), 80),
            new WeightedRange(GetOuterPercent(0.3f) , GetOuterPercent(0.5f), 15),
            new WeightedRange(GetOuterPercent(0f) , GetOuterPercent(0.1f), 5)
        };
        outerRanges = new WeightedSelection(tempRanges);
    }

    public void Stop()
    {
        targetSpawned.SetActive(false);
        isPlaying = false;
        targetComponent.OnHit -= OnHit;
    }

    public void Initialize()
    {
        InitializeObject();
    }
}
