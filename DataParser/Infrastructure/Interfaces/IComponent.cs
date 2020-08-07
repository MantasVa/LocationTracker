namespace DataParser.Infrastructure.Interfaces
{
    public interface IComponent
    {
        void Accept(IVisitor visitor);
    }
}
