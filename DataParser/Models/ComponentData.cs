using DataParser.Infrastructure;
using DataParser.Infrastructure.Enums;
using DataParser.Infrastructure.Interfaces;
using System;

namespace DataParser.Models
{
    public class ComponentData : BaseData
    {
        public ComponentData(DataType dataType, ArraySegment<byte> arraySegment) : base(dataType, arraySegment) { }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
