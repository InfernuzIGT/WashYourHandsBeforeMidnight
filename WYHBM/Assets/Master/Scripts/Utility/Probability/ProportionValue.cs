using System;
using System.Collections.Generic;

public class ProportionValue<T>
{
    public double Proportion { get; set; }
    public T Value { get; set; }
}

public static class ProportionValue
{
    public static ProportionValue<T> Create<T>(double proportion, T value)
    {
        return new ProportionValue<T> { Proportion = proportion, Value = value };
    }

    static Random random = new Random();

    public static T ChooseByRandom<T>(this IEnumerable<ProportionValue<T>> collection)
    {
        var rnd = random.NextDouble();
        foreach (var item in collection)
        {
            if (rnd < item.Proportion)
                return item.Value;
            rnd -= item.Proportion;
        }

        return default(T);

        //throw new InvalidOperationException("The proportions in the collection do not add up to 1.");
    }

    public static bool GetProbability(float value)
    {
        ProportionValue<bool>[] tempProb = new ProportionValue<bool>[2];

        tempProb[0] = ProportionValue.Create(value, true);
        tempProb[1] = ProportionValue.Create(1 - value, false);

        return tempProb.ChooseByRandom();
    }
}