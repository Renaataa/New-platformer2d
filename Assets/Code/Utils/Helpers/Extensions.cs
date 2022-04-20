using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class Extensions
{
    public static int SublistIndex<T>(this List<T> list, List<T> sublist)
    {
        return list.SublistIndex(sublist, 1);
    }

    public static int SublistIndex<T>(this List<T> list, List<T> sublist, int interval)
    {
        int index = -1;
        if(interval <= 0)
        {
            throw new ArgumentOutOfRangeException("interval", "Interval cannot be less or equal to 0 (zero)");
        }

        for(int i =0; i<list.Count; i+= interval)
        {
            if(i>= list.Count || i+sublist.Count-1 >= list.Count)
            {
                return -1;
            }
            if(list[i].Equals(sublist[0]))
            {
                for(int j = 1; j<sublist.Count;j++)
                {
                    if (list[i+j].Equals(sublist[j]))
                    {
                        index = i;
                    }
                    else
                    {
                        index = -1;
                        break;
                    }
                }
                if(index != -1)
                {
                    break;
                }
            }
        }
        return index;
    }

    public static bool ContainsList<T>(this List<T> list, List<T> listToCheck)
    {
        foreach(var e in listToCheck)
        {
            if(!list.Contains(e))
            {
                return false;
            }
        }
        return true;
    }

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        try
        {
            wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        }
        catch
        {
            wrapper = JsonUtility.FromJson<Wrapper<T>>("{\"Items\":" + json + "}");
        }
        return wrapper.Items;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
