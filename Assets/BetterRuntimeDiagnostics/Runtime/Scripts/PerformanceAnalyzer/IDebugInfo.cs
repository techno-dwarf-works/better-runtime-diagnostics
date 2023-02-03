namespace Better.Diagnostics.Runtime.PerformanceAnalyzer
{
    public interface IDebugInfo
    {
        public void Initialize();
        public void OnGUI();
        public void Deconstruct();
    }
}