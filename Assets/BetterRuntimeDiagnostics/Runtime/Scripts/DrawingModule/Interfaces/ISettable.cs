namespace Better.Diagnostics.Runtime.DrawingModule.Interfaces
{
    public interface ISettable<in TSet, out TReturn> where TReturn : IRemovable
    {
        public TReturn Set(TSet data);
    }
    
    public interface ISettable<in TSet1, in TSet2, out TReturn> where TReturn : IRemovable
    {
        public TReturn Set(TSet1 data1, TSet2 data2);
    }

    public interface ISettable<in TSet1, in TSet2, in TSet3, out TReturn> where TReturn : IRemovable
    {
        public TReturn Set(TSet1 data1, TSet2 data2, TSet3 data3);
    }

    public interface ISettable<in TSet1, in TSet2, in TSet3, in TSet4, out TReturn> where TReturn : IRemovable
    {
        public TReturn Set(TSet1 data1, TSet2 data2, TSet3 data3, TSet4 data4);
    }
    
    public interface ISettable<in TSet1, in TSet2, in TSet3, in TSet4, in TSet5, out TReturn> where TReturn : IRemovable
    {
        public TReturn Set(TSet1 data1, TSet2 data2, TSet3 data3, TSet4 data4, TSet5 data5);
    }
}