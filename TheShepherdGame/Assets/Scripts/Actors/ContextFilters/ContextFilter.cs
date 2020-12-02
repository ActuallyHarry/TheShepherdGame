using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextFilter
{
    public  List<Transform> FilterContext( List<Transform> context, string tagName)
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
    public  bool ContextContainsSpecific(List<Transform> context, Transform specific)
    {
        return context.Contains(specific);
    }
}
