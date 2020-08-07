using DataParser.Infrastructure.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataParser.Models
{
    public class CompositeData : BaseData, ICompositeData
    {
        public CompositeData(string name) : base(name)
        {
            Data = new ObservableCollection<BaseData>();
        }

        public ObservableCollection<BaseData> Data { get; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
            foreach (var item in Data)
            {
                item.Accept(visitor);
            }
        }

        public void Add(BaseData data)
        {
            Data.Add(data);
        }

        public void ConfigureArraySegment()
        {
            this.ArraySegment = new ArraySegment<byte>(this.Data.First().ArraySegment.Array, this.Data.First().ArraySegment.Offset,
                this.Data.Last().ArraySegment.Offset - this.Data.First().ArraySegment.Offset + 1);
        }

        public void ConfigureArraySegment(int lastSegmentOffset)
        {
            this.ArraySegment = new ArraySegment<byte>(this.Data.First().ArraySegment.Array,
                this.Data.First().ArraySegment.Offset, lastSegmentOffset - this.Data.First().ArraySegment.Offset + 1);
        }

        public BaseData Last()
        {
            return Data.Last();
        }

    }
}
