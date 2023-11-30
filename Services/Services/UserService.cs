using AutoMapper;
using DAL.Context;
using Model.Entities;
using Services.Interfaces;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly LibaryMDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(LibaryMDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> CheckUser(string name)
        {
            try
            {
                var isUserExist = _dbContext.Users.FirstOrDefault(a => a.Name == name);

                if (isUserExist is not null)
                {
                    Log.Information($"User with name '{name}' found. UserId: {isUserExist.Id}");
                    return isUserExist.Id;
                }
                else
                {
                    int userId = await Create(name);
                    Log.Information($"User with name '{name}' not found. New user created. UserId: {userId}");
                    return userId;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while checking or creating user with name '{name}'");
                throw;
            }
        }

        private async Task<int> Create(string name)
        {
            try
            {
                var newUser = new User { Name = name };

                _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                Log.Information($"New user created with name '{name}'. UserId: {newUser.Id}");

                return newUser.Id;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while creating user with name '{name}'");
                throw;
            }
        }
    }
}
