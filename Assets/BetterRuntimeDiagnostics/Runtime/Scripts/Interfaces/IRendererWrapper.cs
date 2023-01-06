using System.Collections.Generic;

namespace Better.Diagnostics.Runtime
{
    public interface IRendererWrapper
    {
        public IEnumerable<Line> GetLines();
    }
}