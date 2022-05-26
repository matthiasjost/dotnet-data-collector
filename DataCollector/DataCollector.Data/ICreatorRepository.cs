using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Data
{
    public interface ICreatorRepository
    {
        public List<CreatorDbItem> Get();
        public CreatorDbItem Get(string id);
        public CreatorDbItem Create(CreatorDbItem creator);
        public void Update(string id, CreatorDbItem creator);
        public void Remove(CreatorDbItem creator);
        public void Remove(string id);

        public Task<CreatorDbItem> FindFirstWithName(string name);
    }
}
