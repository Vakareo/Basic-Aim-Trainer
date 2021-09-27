using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedSelection
{
    //Assume ranges sorted by largest weight first
    private readonly WeightedRange[] ranges;
    private readonly int sum;

    public WeightedSelection(WeightedRange[] ranges)
    {
        this.ranges = ranges;
        int tempSum = 0;
        for (int i = 0; i < ranges.Length; i++)
            tempSum += ranges[i].weight;
        sum = tempSum;
    }

    public WeightedRange GetRange()
    {
        int randomNum = Random.Range(0, sum);
        int max = 0;
        for (int i = 0; i < ranges.Length; i++)
        {
            max += ranges[i].weight;
            if (randomNum <= max)
            {
                return ranges[i];
            }
        }
        return ranges[ranges.Length - 1];
    }
}

public struct WeightedRange
{
    public float startRange;
    public float endRange;
    public int weight;

    public WeightedRange(float startRange, float endRange, int weight)
    {
        this.startRange = startRange;
        this.endRange = endRange;
        this.weight = weight;
    }
}
