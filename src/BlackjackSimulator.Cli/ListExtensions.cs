using System;
using System.Collections.Generic;

namespace BlackjackSimulator.Cli;

public static class ListExtensions
{
    private static readonly Random _random;

    static ListExtensions()
    {
        _random = new Random(DateTime.UtcNow.Millisecond);
    }

    public static IReadOnlyCollection<T> Shuffle<T>(this List<T> extended)
    {
        var n = extended.Count;

        while (n > 1)
        {
            n--;
            var k = _random.Next(n + 1);
            (extended[k], extended[n]) = (extended[n], extended[k]);
        }

        return extended;
    }
}