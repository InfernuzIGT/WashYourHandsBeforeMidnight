using UnityEngine;

public static class Utils
{
    public static int GetNextIndex(bool isNext, int index, int max, bool reset = true)
    {
        if (reset)
        {
            if (isNext)
            {
                index = index < max ? index + 1 : 0;
            }
            else
            {
                index = index > 0 ? index - 1 : max;
            }
        }
        else
        {
            if (isNext)
            {
                index = index < max ? index + 1 : index;
            }
            else
            {
                index = index > 0 ? index - 1 : 0;
            }
        }

        return index;
    }
}