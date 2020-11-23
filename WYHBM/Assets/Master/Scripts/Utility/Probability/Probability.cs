using UnityEngine;

[System.Serializable]
public class ObjectString
{
    public string objectName;
    [Range (0f, 1f)]
    public double percentage;
}

[System.Serializable]
public class ObjectBool
{
    public bool objectState;
    [Range (0f, 1f)]
    public double percentage;
}

public class Probability : MonoBehaviour
{
    public ObjectString[] objectStrings;
    public ObjectBool objectBool;

    private void Start ()
    {
        //CalculateBool ();
        //CalculateString ();
    }

    public bool CalculateBool ()
    {
        ProportionValue<bool>[] newList = new ProportionValue<bool>[1];

        newList[0] = ProportionValue.Create (objectBool.percentage, objectBool.objectState);

        bool result = newList.ChooseByRandom ();

        if (!result)
        {
            Debug.Log ("FALSE");
        }
        else
        {
            Debug.Log ($"Result is TRUE");
        }

        return result;
    }

    public string CalculateString ()
    {
        ProportionValue<string>[] newList = new ProportionValue<string>[objectStrings.Length];

        for (int i = 0; i < objectStrings.Length; i++)
        {
            newList[i] = ProportionValue.Create (objectStrings[i].percentage, objectStrings[i].objectName);
        }

        string result = newList.ChooseByRandom ();

        if (result == null)
        {
            Debug.Log ("NULL");
        }
        else
        {
            Debug.Log ($"Result: {result}");
        }

        return result;
    }

}