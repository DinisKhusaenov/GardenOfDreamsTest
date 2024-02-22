using System;
using UnityEngine;

public interface IHealth
{
    event Action<int> HealthChanged;

    int MaxHealth { get;}
    void Reduce(int value);
    void Add(int value);
}
