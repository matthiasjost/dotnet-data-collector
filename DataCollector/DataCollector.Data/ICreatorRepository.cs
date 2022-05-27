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
        Task Create(CreatorDbItem creator);
        Task UpdateById(string id, CreatorDbItem creator);
        Task RemoveById(string id);
    }

}
