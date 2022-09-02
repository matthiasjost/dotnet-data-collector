using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Data
{
    public interface ICreatorRepository
    {
        Task<CreatorEntity> FindFirstByName(string name);
        Task<CreatorEntity> FindFirstById(string id);
        Task<List<CreatorEntity>> GetAllItems();
        Task Create(CreatorEntity creator);
        Task UpdateById(CreatorEntity creator);
        Task RemoveById(string id);
        Task<List<CreatorEntity>> GetItems(string searchValue);
    }

}
