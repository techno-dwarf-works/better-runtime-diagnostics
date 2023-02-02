namespace Better.Diagnostics.Runtime.Interfaces
{
    public interface IRemovable
    {
        public bool IsMarkedForRemove { get; }

        public void MarkForRemove();
    }
}