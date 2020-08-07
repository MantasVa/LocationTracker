using DataParser.Models;

namespace DataParser.Infrastructure.Interfaces
{
    public interface IVisitor
    {
        void Visit(BaseData componentData);
    }
}
