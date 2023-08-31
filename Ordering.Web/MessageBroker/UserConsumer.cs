using MassTransit;
using OnlineStore.Shared;
using Ordering.Domain.Interfaces;

namespace Ordering.Web.MessageBroker
{
    public class UserConsumer : IConsumer<DeleteUserMessageDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<DeleteUserMessageDto> context)
        {
            var message = context.Message;

            var user = await _unitOfWork.Users.GetAsync(user => user.UserName == message.UserName);

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.CommitAsync();
        }
    }
}