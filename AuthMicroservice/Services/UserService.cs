using System;
using System.Security.Cryptography;
using System.Text;
using AuthMicroservice.Models;

namespace AuthMicroservice.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Создаем новый экземпляр класса User, используя данные из CreateUserDto
            var user = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                //Username = createUserDto.Username
            };

            // Вычисляем хэш пароля и генерируем salt
            CreatePasswordHash(createUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Добавляем пользователя в базу данных и сохраняем изменения
            await _userRepository.AddAsync(user);

            // Возвращаем созданный пользователь в виде объекта UserDto
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                //Username = user.Username
            };
        }

        public async Task<UserDto> GetUserAsync(int id)
        {
            // Получаем пользователя из базы данных по его Id
            var user = await _userRepository.GetByIdAsync(id);

            // Если пользователь не найден, возвращаем null
            if (user == null)
            {
                return null;
            }

            // Возвращаем пользователя в виде объекта UserDto
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                //Username = user.Username
            };
        }
        //Всех пользователей пока не получаем
        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            // Получаем список всех пользователей из базы данных
            var users = await _userRepository.GetUsersAsync();

            // Конвертируем список пользователей в список объектов UserDto
            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                //Username = u.Username
            });

            return userDtos;
        }

        public async Task UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            // Получаем пользователя из базы данных по его Id
            var user = await _userRepository.GetByIdAsync(updateUserDto.Id);

            // Если пользователь не найден, выбрасываем исключение
            if (user == null)
            {
                throw new ArgumentException($"User with id {updateUserDto.Id} not found");
            }

            // Обновляем свойства пользователя
            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            //user.Username = updateUserDto.Username;

            // Если пароль был изменен, вычисляем новый хэш и salt
            //if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
            //{
            //    CreatePasswordHash(updateUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            //    user.PasswordHash = passwordHash;
            //    user.PasswordSalt = passwordSalt;
            //}

            // Сохраняем изменения
        }

        public async Task DeleteUserAsync(int id)
        {
            // Получаем пользователя из базы данных по его Id
            var user = await _userRepository.GetByIdAsync(id);

            // Если пользовательне найден, выбрасываем исключение
            if (user == null)
            {
                throw new ArgumentException($"User with id {id} not found");
            }
            // Удаляем пользователя из базы данных и сохраняем изменения
            await _userRepository.DeleteAsync(user);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Проверяем аргументы метода на null
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            // Проверяем, что пароль не пустая строка
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            }

            // Генерируем случайный salt длиной 128 бит (16 байт)
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;

            // Вычисляем хэш пароля
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

}

