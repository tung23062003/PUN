using System;
using System.Collections.Generic;

public class WeightedRandom<T>
{
    private class WeightedItem
    {
        public T Item { get; }
        public double Weight { get; }

        public WeightedItem(T item, double weight)
        {
            Item = item;
            Weight = weight;
        }
    }

    private List<WeightedItem> _items = new List<WeightedItem>();
    private double _totalWeight = 0;
    private Random _random = new Random();

    public void Add(T item, double weight)
    {
        if (weight <= 0)
            throw new ArgumentException("Weight must be positive.", nameof(weight));

        _items.Add(new WeightedItem(item, weight));
        _totalWeight += weight;
    }

    public T GetValue()
    {
        double randomValue = _random.NextDouble() * _totalWeight;
        double cumulativeWeight = 0;

        foreach (var weightedItem in _items)
        {
            cumulativeWeight += weightedItem.Weight;
            if (randomValue < cumulativeWeight)
            {
                return weightedItem.Item;
            }
        }

        // Should never reach here if weights are positive and items are added correctly
        throw new InvalidOperationException("Failed to get a random item.");
    }
}