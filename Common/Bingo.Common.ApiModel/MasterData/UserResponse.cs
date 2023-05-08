using Bingo.Common.DomainModel.MasterData;

namespace Bingo.Common.ApiModel.MasterData
{
    public class UserResponse
    {
        public User foundUser { get; set; }

        public string message { get; set; }
    }
}
