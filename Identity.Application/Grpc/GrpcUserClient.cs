using Identity.Application.Grpc.Protos;
using Identity.Domain.Entities;

namespace Identity.Application.Grpc
{
    public class GrpcUserClient
    {
        private readonly GrpcUser.GrpcUserClient _client;

        public GrpcUserClient(GrpcUser.GrpcUserClient client)
        {
            _client = client;
        }

        public async Task CreateUser(ApplicationUser applicationUser)
        {
            var userDto = new UserDto { UserName = applicationUser.UserName };

            var request = new CreateUserRequest()
            {
                User = userDto
            };

            await _client.CreateUserAsync(request);
        }
    }
}