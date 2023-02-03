using System.Collections.Generic;

namespace Better.Diagnostics.Runtime.DrawingModule.Interfaces
{
    public interface IRendererWrapper : IRemovable
    {
        public void Initialize();
        public IEnumerable<Line> GetLines();
    }
}