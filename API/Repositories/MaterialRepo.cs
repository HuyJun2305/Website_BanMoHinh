using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class MaterialRepo : IMaterialRepo
    {
        private readonly ApplicationDbContext _context;
        public MaterialRepo(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task Create(Material data)
        {
            if (await GetMaterialById(data.Id) != null) throw new DuplicateWaitObjectException($"Material : {data.Id} is existed!");
            await _context.Materials.AddAsync(data);
        }

        public async Task Delete(Guid id)
        {
            var data = await GetMaterialById(id);
            if (data == null) throw new KeyNotFoundException("Not found this material!");
            if (_context.Products.Where(p => p.IdMaterial == id).Any()) throw new Exception("This material has used for some product!");
            _context.Materials.Remove(data);
        }

        public async Task<List<Material>> GetAllMaterials()
        {
            return await _context.Materials.ToListAsync();
        }

        public async Task<Material> GetMaterialById(Guid id)
        {
            return await _context.Materials.FindAsync(id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Material data)
        {
            if (await GetMaterialById(data.Id) == null) throw new KeyNotFoundException("Not found this material!");
            _context.Entry(data).State = EntityState.Modified;
        }
    }
}
