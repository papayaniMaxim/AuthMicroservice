using System;
using System.Collections.Generic;
using AuthMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthMicroservice.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<User> _users;

        public UserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _users = dbContext.Set<User>();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _users.FindAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<List<User>> GetUsersAsync()
        {
            return await _users.ToListAsync<User>();
        }
        public async Task AddAsync(User user)
        {
            await _users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }

}

