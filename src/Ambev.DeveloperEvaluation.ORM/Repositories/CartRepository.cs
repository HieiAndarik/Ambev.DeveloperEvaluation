using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CartRepository : ICartRepository
{
    private readonly DefaultContext _context;

    public CartRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cart>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Carts
            .AsNoTracking()
            .Include(c => c.Items)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Cart>> GetAllAsync()
    {
        return await _context.Carts
            .AsNoTracking()
            .Include(c => c.Items)
            .ToListAsync();
    }

    public async Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Carts
            .AsNoTracking()
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Cart?> GetByIdAsync(int id)
    {
        return await _context.Carts
            .AsNoTracking()
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        await _context.Carts.AddAsync(cart, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task DeleteAsync(Cart cart)
    {
        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<bool> DeleteByIdAsync(int id)
    {
        var cart = await _context.Carts
            .FirstOrDefaultAsync(p => p.Id == id);

        if (cart == null)
        {
            return false;
        }

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cart = await _context.Carts
            .FirstOrDefaultAsync(p => p.Id == id);

        if (cart == null)
        {
            return false;
        }

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task RemoveItemsAsync(IEnumerable<CartItem> items, CancellationToken cancellationToken)
    {
        _context.CartItems.RemoveRange(items);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
