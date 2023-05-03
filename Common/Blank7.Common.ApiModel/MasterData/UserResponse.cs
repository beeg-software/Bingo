using Blank7.Common.DomainModel.MasterData;

namespace Blank7.Common.ApiModel.MasterData
{
    public class UserResponse
    {
        public User foundUser { get; set; }

        public string message { get; set; }
    }
}
