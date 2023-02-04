namespace Better.Diagnostics.Runtime.DrawingModule.Interfaces
{
    public interface IRemovable
    {
        public bool IsMarkedForRemove { get; }

        public void MarkForRemove();

        public void OnRemoved();
    }

    
}