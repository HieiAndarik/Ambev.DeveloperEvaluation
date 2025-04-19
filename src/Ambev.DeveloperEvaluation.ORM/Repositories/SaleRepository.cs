// Infrastructure/ORM/Repositories/SaleRepository.cs
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<Sale?> GetByIdAsync(Guid id)
        {
            return await _context.Set<Sale>()
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync(int page, int size, string orderBy)
        {
            var query = _context.Set<Sale>().AsQueryable();

            // Apply ordering
            if (!string.IsNullOrEmpty(orderBy))
            {
                var parts = orderBy.Split(',');
                foreach (var part in parts)
                {
                    var trimmedPart = part.Trim();
                    var descending = trimmedPart.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
                    var property = descending ? trimmedPart.Replace(" desc", "", StringComparison.OrdinalIgnoreCase) : trimmedPart;

                    switch (property.ToLower())
                    {
                        case "saledate":
                            query = descending ? query.OrderByDescending(s => s.SaleDate) : query.OrderBy(s => s.SaleDate);
                            break;
                        case "salenumber":
                            query = descending ? query.OrderByDescending(s => s.SaleNumber) : query.OrderBy(s => s.SaleNumber);
                            break;
                        case "totalamount":
                            query = descending ? query.OrderByDescending(s => s.TotalAmount) : query.OrderBy(s => s.TotalAmount);
                            break;
                        case "customername":
                            query = descending ? query.OrderByDescending(s => s.CustomerName) : query.OrderBy(s => s.CustomerName);
                            break;
                        default:
                            query = query.OrderByDescending(s => s.SaleDate);
                            break;
                    }
                }
            }
            else
            {
                query = query.OrderByDescending(s => s.SaleDate);
            }

            return await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Set<Sale>().CountAsync();
        }

        public async Task<Guid> AddAsync(Sale sale)
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

        public async Task DeleteAsync(Guid id)
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