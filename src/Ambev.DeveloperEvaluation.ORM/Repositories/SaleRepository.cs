// Infrastructure/Repositories/SaleRepository.cs
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Sale> GetByIdAsync(int id)
        {
            return await _context.Set<Sale>()
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync(int page, int size, string orderBy)
        {
            var query = _context.Set<Sale>().AsQueryable();

            // Aplicar ordenação
            if (!string.IsNullOrEmpty(orderBy))
            {
                // Aqui você implementaria a lógica de ordenação dinâmica
                // Exemplo simplificado:
                if (orderBy.Contains("saleDate"))
                {
                    query = orderBy.Contains("desc")
                        ? query.OrderByDescending(s => s.SaleDate)
                        : query.OrderBy(s => s.SaleDate);
                }
            }

            return await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Sale sale)
        {
            await _context.Set<Sale>().AddAsync(sale);
            await _context.SaveChangesAsync();
            return sale.Id;
        } 

        public async Task UpdateAsync(Sale sale)
        {
            _context.Set<Sale>().Update(sale);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sale = await GetByIdAsync(id);
            if (sale != null)
            {
                _context.Set<Sale>().Remove(sale);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetNextSaleNumberAsync()
        {
            var lastSale = await _context.Set<Sale>()
                .OrderByDescending(s => s.SaleNumber)
                .FirstOrDefaultAsync();

            return lastSale != null ? lastSale.SaleNumber + 1 : 1;
        }
    }
}