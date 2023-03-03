using System;
namespace AuthMicroservice.Models
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

