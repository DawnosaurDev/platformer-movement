using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visualization
{
    public class Visual
    {
        public string Name { get; private set; }

        protected VisualDrawer drawer;
        protected GameObject obj;

        public void InitializeVisual(VisualDrawer drawer, string name)
        {
            Name = name;
            this.drawer = drawer;
        }

        public void DestroyVisual()
        {
            Object.Destroy(obj);
        }
    }
}
