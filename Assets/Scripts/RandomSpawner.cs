using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float time = 2f;
    private WeightedSelection ranges;

    private WeightedRange randRange;

    private GameObject spawnedObject;
    private Vector3 intialPosition;

    private float waitTime;
    private WaitForSeconds myWait;

    private void OnValidate()
    {
        myWait = new WaitForSeconds(time);
    }

    private void Awake()
    {
        var tempRanges = new WeightedRange[]{
            new WeightedRange(10, 15, 90),
            new WeightedRange(5, 6, 5),
            new WeightedRange(1, 2, 5)
        };
        ranges = new WeightedSelection(tempRanges);
    }

    private void Start()
    {
        intialPosition = transform.position;
        spawnedObject = Instantiate(prefab, new Vector3(intialPosition.x, intialPosition.y, intialPosition.z + RandomFloat()), Quaternion.identity, transform);
        StartCoroutine(TimedSpawner());
    }

    private void Update()
    {
        // waitTime += Time.deltaTime;
        // if (waitTime >= time)
        // {
        //     SetObjectToRandomPosition();
        //     waitTime = 0;
        // }
    }

    IEnumerator TimedSpawner()
    {
        while (true)
        {
            yield return myWait;
            SetObjectToRandomPosition();
        }
    }
    private void SetObjectToRandomPosition()
    {
        spawnedObject.transform.position = new Vector3(intialPosition.x, intialPosition.y, intialPosition.z + RandomFloat());
    }

    private float RandomFloat()
    {
        randRange = ranges.GetRange();
        return Random.Range(randRange.startRange, randRange.endRange);
    }
}
