using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Data
{
    public interface ICreatorRepository
    {
        Task<CreatorDbItem> FindFirstByName(string name);
        Task<CreatorDbItem> FindFirstById(string id);
        Task<List<CreatorDbItem>> GetAllItems();
        void Create(CreatorDbItem creator);
        void UpdateById(string id, CreatorDbItem creator);
        void RemoveById(string id);
    }

}
