namespace Better.Diagnostics.Runtime.InfoDisplayer.Interfaces
{
    public interface IDebugInfo
    {
        public void Initialize();
        public void OnGUI();
        public void Deconstruct();
    }

    public interface IUpdateableInfo
    {
        public void Update();
    }
    
    public interface IFixedUpdateableInfo
    {
        public void FixedUpdate();
    }
}