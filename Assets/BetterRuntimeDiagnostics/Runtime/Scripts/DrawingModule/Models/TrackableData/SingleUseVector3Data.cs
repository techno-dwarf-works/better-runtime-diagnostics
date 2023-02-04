using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule.TrackableData
{
    public class SingleUseVector3Data : SingleUseData<Vector3>
    {
        public override void OnRemoved()
        {
            RemovablePool.Instance.Add(this);
        }
    }
}