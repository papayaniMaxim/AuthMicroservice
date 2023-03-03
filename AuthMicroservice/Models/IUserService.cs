using System;
namespace AuthMicroservice.Models
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto> GetUserAsync(int id);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task UpdateUserAsync(UpdateUserDto updateUserDto);
        Task DeleteUserAsync(int id);
    }

}

