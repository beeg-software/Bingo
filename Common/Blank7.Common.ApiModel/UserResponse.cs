using Blank7.Common.DomainModel;

namespace Blank7.Common.ApiModel
{
    public class UserResponse
    {
        public User foundUser { get; set; }

        public string message { get; set; }
    }
}
