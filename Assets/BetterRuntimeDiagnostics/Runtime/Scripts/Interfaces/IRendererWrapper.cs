using System.Collections.Generic;
using Better.Diagnostics.Runtime.Models;

namespace Better.Diagnostics.Runtime.Interfaces
{
    public interface IRendererWrapper : IRemovable
    {
        public void Initialize();
        public IEnumerable<Line> GetLines();
    }
}