﻿namespace Blank7.UI.Shared.Services
{
    public class UserService : IUserService
    {
        public UserService()
        {
        }

        public Task<List<string>> GetUsersAsync()
        {
            return Task.FromResult(new List<string>()
            {
                "User1",
                "User2",
                "User3"
            });
        }
    }
}
