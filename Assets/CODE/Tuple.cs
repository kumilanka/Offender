using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Silly tuple implementation because Unity Mono is 10 years old x)
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
public class Tuple<T1, T2>
{
    public T1 one;
    public T2 two;

    public Tuple(T1 one, T2 two)
    {
        this.one = one;
        this.two = two;
    }
}
