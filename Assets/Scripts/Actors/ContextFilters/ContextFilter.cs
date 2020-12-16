using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContextFilter
{
    public static List<Transform> FilterContext( List<Transform> context, string tagName)
    {
        List<Transform> filteredContext = new List<Transform>();
        foreach(Transform t in context)
        {
            if(t.tag == tagName)
            {
                filteredContext.Add(t);
            }
        }

        return filteredContext;
    }

    public static List<Transform> FilterForActors(List<Transform> context)
    {
        List<Transform> filteredContext = new List<Transform>(0);
        foreach (Transform t in context)
        {
            if(t.GetComponent<Actor>() != null)
            {
                filteredContext.Add(t);
            }
        }

        return filteredContext;
    }
    public static List<Transform> FilterForHerd(List<Transform> context)
    {
        List<Transform> filteredContext = new List<Transform>(0);
        foreach (Transform t in context)
        {
            if (t.GetComponent<ShpdAnimal>() != null)
            {
                filteredContext.Add(t);
            }
        }

        return filteredContext;
    }

    public  static bool ContextContainsSpecific(List<Transform> context, Transform specific)
    {
        return context.Contains(specific);
    }
}
