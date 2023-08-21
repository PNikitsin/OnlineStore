using Grpc.Core;
using Ordering.Domain.Entities;
using Ordering.Domain.Interfaces;
using Ordering.Web.Grpc.Protos;

namespace Ordering.Web.Grpc
{
    public class GrpcUserService : GrpcUser.GrpcUserBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GrpcUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task<UserDto> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            var user = new User
            {
                UserName = request.User.UserName
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return request.User;
        }
    }
}