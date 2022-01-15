using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visualization
{
    public class VisualDrawer
    {
       public List<Visual> ActiveVisuals  { get; private set;} = new List<Visual>();

        public T CreateVisual<T>(string name) where T : Visual, new()
        {
            T visual = new T();
            visual.InitializeVisual(this, name);
            ActiveVisuals.Add(visual);
            return visual;
        }
       
        public T GetVisual<T>(string name) where T : Visual, new()
        {
            foreach (Visual v in ActiveVisuals)
            {
                if(v.Name == name)
                    return v as T;             
            }

            return null;
        }
    }
}
