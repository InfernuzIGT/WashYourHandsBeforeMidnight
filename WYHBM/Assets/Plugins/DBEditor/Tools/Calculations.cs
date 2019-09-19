using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Calculations
{
    /// <summary>
    /// Get the Median in a range of values
    /// </summary>
    public static int GetMedianInt(this IEnumerable<int> source)
    {
        // Create a copy of the input, and sort the copy
        int[] temp = source.ToArray();
        Array.Sort(temp);

        int count = temp.Length;
        if (count == 0)
        {
            throw new InvalidOperationException("Empty collection");
        }
        else if (count % 2 == 0)
        {
            // Count is even, average two middle elements
            int a = temp[count / 2 - 1];
            int b = temp[count / 2];
            int result = (int)((a + b) / 2m);
            return result;
        }
        else
        {
            // Count is odd, return the middle element
            return temp[count / 2];
        }
    }

    public static float GetMedianFloat(this IEnumerable<int> source)
    {
        // Create a copy of the input, and sort the copy
        int[] temp = source.ToArray();
        Array.Sort(temp);

        int count = temp.Length;
        if (count == 0)
        {
            throw new InvalidOperationException("Empty collection");
        }
        else if (count % 2 == 0)
        {
            // Count is even, average two middle elements
            float a = temp[count / 2 - 1];
            float b = temp[count / 2];
            return RoundToNearestHalf((a + b) / 2);
        }
        else
        {
            // Count is odd, return the middle element
            return RoundToNearestHalf(temp[count / 2]);
        }
    }

    /// <summary>
    /// Get the Q1, Q2 and Q3 in a range of values
    /// </summary>
    public static void GetQuartilesInt(this IEnumerable<int> source, out int Q1, out int Q2, out int Q3)
    {
        // Prepare variables
        List<int> listQ1 = new List<int>();
        List<int> listQ2 = new List<int>();
        int indexQ2;

        // Create a copy of the input, and sort the copy
        int[] temp = source.ToArray();
        Array.Sort(temp);

        int count = temp.Length;
        if (count == 0)
        {
            throw new InvalidOperationException("Empty collection");
        }
        else if (count % 2 == 0)
        {
            // Count is even, average two middle elements
            int a = temp[count / 2 - 1];
            int b = temp[count / 2];
            Q2 = (int)((a + b) / 2m);

            // Get the ranges between quartiles
            indexQ2 = Array.IndexOf(temp, b);

            for (int i = 0; i < indexQ2; i++)
                listQ1.Add(temp[i]);

            for (int i = indexQ2; i < temp.Length; i++)
                listQ2.Add(temp[i]);
        }
        else
        {
            // Count is odd, return the middle element
            Q2 = temp[count / 2];

            // Get the ranges between quartiles
            indexQ2 = Array.IndexOf(temp, Q2);

            for (int i = 0; i < indexQ2; i++)
                listQ1.Add(temp[i]);

            for (int i = indexQ2; i < temp.Length; i++)
                listQ2.Add(temp[i]);
        }

        GetQuartilInt(listQ1, out Q1);
        GetQuartilInt(listQ2, out Q3);
    }

    public static void GetQuartilesFloat(this IEnumerable<int> source, out float Q1, out float Q2, out float Q3)
    {
        // Prepare variables
        List<float> listQ1 = new List<float>();
        List<float> listQ2 = new List<float>();
        int indexQ2;

        // Create a copy of the input, and sort the copy
        int[] temp = source.ToArray();
        Array.Sort(temp);

        int count = temp.Length;
        if (count == 0)
        {
            throw new InvalidOperationException("Empty collection");
        }
        else if (count % 2 == 0)
        {
            // Count is even, average two middle elements
            float a = temp[count / 2 - 1];
            float b = temp[count / 2];
            Q2 = RoundToNearestHalf((a + b) / 2);

            // Used to Index
            int tempB = temp[count / 2];

            // Get the ranges between quartiles
            indexQ2 = Array.IndexOf(temp, tempB);

            for (int i = 0; i < indexQ2; i++)
                listQ1.Add(temp[i]);

            for (int i = indexQ2; i < temp.Length; i++)
                listQ2.Add(temp[i]);
        }
        else
        {
            // Count is odd, return the middle element
            Q2 = RoundToNearestHalf(temp[count / 2]);

            // Used to Index
            int tempQ2 = temp[count / 2];

            // Get the ranges between quartiles
            indexQ2 = Array.IndexOf(temp, tempQ2);

            for (int i = 0; i < indexQ2; i++)
                listQ1.Add(temp[i]);

            for (int i = indexQ2; i < temp.Length; i++)
                listQ2.Add(temp[i]);
        }

        GetQuartilFloat(listQ1, out Q1);
        GetQuartilFloat(listQ2, out Q3);
    }

    public static void GetQuartilesPosition(this IEnumerable<float> source, out float Q1, out float Q2, out float Q3)
    {
        // Prepare variables
        List<float> listQ1 = new List<float>();
        List<float> listQ2 = new List<float>();
        int indexQ2;

        // Create a copy of the input, and sort the copy
        float[] temp = source.ToArray();
        Array.Sort(temp);

        int count = temp.Length;
        if (count == 0)
        {
            throw new InvalidOperationException("Empty collection");
        }
        else if (count % 2 == 0)
        {
            // Count is even, average two middle elements
            float a = temp[count / 2 - 1];
            float b = temp[count / 2];
            Q2 = ((a + b) / 2);

            // Used to Index
            float tempB = temp[count / 2];

            // Get the ranges between quartiles
            indexQ2 = Array.IndexOf(temp, tempB);

            for (int i = 0; i < indexQ2; i++)
                listQ1.Add(temp[i]);

            for (int i = indexQ2; i < temp.Length; i++)
                listQ2.Add(temp[i]);
        }
        else
        {
            // Count is odd, return the middle element
            Q2 = (temp[count / 2]);

            // Used to Index
            float tempQ2 = temp[count / 2];

            // Get the ranges between quartiles
            indexQ2 = Array.IndexOf(temp, tempQ2);

            for (int i = 0; i < indexQ2; i++)
                listQ1.Add(temp[i]);

            for (int i = indexQ2; i < temp.Length; i++)
                listQ2.Add(temp[i]);
        }

        GetQuartilPosition(listQ1, out Q1);
        GetQuartilPosition(listQ2, out Q3);
    }

    private static void GetQuartilInt(this IEnumerable<int> source, out int quartil)
    {
        // Create a copy of the input, and sort the copy
        int[] temp = source.ToArray();

        int count = temp.Length;
        if (count % 2 == 0)
        {
            // Count is even, average two middle elements
            int a = temp[count / 2 - 1];
            int b = temp[count / 2];
            quartil = (int)((a + b) / 2m);
        }
        else
        {
            // Count is odd, return the middle element
            quartil = temp[count / 2];
        }
    }

    private static void GetQuartilFloat(this IEnumerable<float> source, out float quartil)
    {
        // Create a copy of the input, and sort the copy
        float[] temp = source.ToArray();

        int count = temp.Length;
        if (count % 2 == 0)
        {
            // Count is even, average two middle elements
            float a = temp[count / 2 - 1];
            float b = temp[count / 2];
            quartil = RoundToNearestHalf((a + b) / 2);
        }
        else
        {
            // Count is odd, return the middle element
            quartil = RoundToNearestHalf(temp[count / 2]);
        }
    }
    
    private static void GetQuartilPosition(this IEnumerable<float> source, out float quartil)
    {
        // Create a copy of the input, and sort the copy
        float[] temp = source.ToArray();

        int count = temp.Length;
        if (count % 2 == 0)
        {
            // Count is even, average two middle elements
            float a = temp[count / 2 - 1];
            float b = temp[count / 2];
            quartil = ((a + b) / 2);
        }
        else
        {
            // Count is odd, return the middle element
            quartil = (temp[count / 2]);
        }
    }

    /// <summary>
    /// Generate distributes points between 2 positions
    /// </summary>
    public static void GenerateDistributedPoints(Vector3 from, Vector3 to, List<Vector3> result, int pointsCount)
    {
        // Divider must be between 0 and 1
        float divider = 1f / pointsCount;
        float linear = 0f;

        if (pointsCount == 0)
        {
            Debug.LogError($"<color=red><b> ERROR: PointsCount must be > 0 instead of {pointsCount} </b></color>");
            return;
        }

        if (pointsCount == 1)
        {
            result.Add(Vector3.Lerp(from, to, 0.5f));
            return;
        }

        for (int i = 0; i < pointsCount; i++)
        {
            linear = i == 0 ? divider / 2 : linear + divider;
            result.Add(Vector3.Lerp(from, to, linear));
        }
    }

    /// <summary>
    /// Generate distributes points between 2 positions, using Filter to points count
    /// </summary>
    public static void GenerateDistributedPoints(float from, float to, List<Vector2> result, int minValue, int maxValue, List<int> filter)
    {
        Vector2 vecFrom = new Vector2(from, 0);
        Vector2 vecTo = new Vector2(to, 0);

        int pointsCount = maxValue - minValue;
        int filterIndex = 0;

        // Divider must be between 0 and 1
        float divider = 1f / pointsCount;
        float linear = 0f;

        for (int i = minValue; i < maxValue; i++)
        {
            linear = i == 0 ? divider / 2 : linear + divider;

            if (filter[filterIndex] == i)
            {
                filterIndex++;
                result.Add(Vector2.Lerp(vecFrom, vecTo, linear));
                if (filterIndex == filter.Count)
                    break;
            }
        }
    }

    /// <summary>
    /// Arrange objects in grid
    /// </summary>
    public static void ArrangeGrid(Vector2 startPosition, List<GameObject> children, int rows, float spaceX, float spaceY)
    {
        int row;
        int column;
        Vector2 newPosition = new Vector2();

        for (int i = 0; i < children.Count; ++i)
        {
            row = i % rows;
            column = i / rows;

            newPosition.x = column * spaceX;
            newPosition.y = row * spaceY;

            children[i].transform.position = newPosition + startPosition;
        }
    }

    /// <summary>
    /// Round to nearest 0.5
    /// </summary>
    public static float RoundToNearestHalf(float a)
    {
        return a = Mathf.Round(a * 2f) * 0.5f;
    }

}