using DataParser.Models;

namespace DataParser.Infrastructure.Interfaces
{
    public interface ICompositeData
    {
        void Add(BaseData data);
        BaseData Last();
        void ConfigureArraySegment();
    }
}
