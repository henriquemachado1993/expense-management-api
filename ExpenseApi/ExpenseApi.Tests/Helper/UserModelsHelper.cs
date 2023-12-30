using ExpenseApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Tests.Helper
{
    public static class UserModelsHelper
    {
        public static List<User> GetListUserAsync()
        {
            var lst = new List<User>()
            {
                new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    BirthDate = DateTime.Now,
                    Password = "12345",
                    Address = new Address() {
                        City = "Brasilia",
                        Street = "X",
                        ZipCode = "123123"
                    }
                }
            };

            return lst;
        }

        public static User GetUser()
        {
            var users = GetListUserAsync();
            return users.FirstOrDefault() ?? new User();
        }
    }
}
