using AutoServiceMVC.Models;
using AutoServiceMVC.Models.System;

namespace AutoServiceMVC.Services.System
{
    public interface IPointService
    {
        public Task<StatusMessage> ChangePointAsync(int userId, int amount, string message);
    }
    public class PointService : IPointService
    {
        private readonly ICommonRepository<User> _userRepo;
        private readonly ICommonRepository<PointTrading> _pointTrandingRepo;

        public PointService(ICommonRepository<User> userRepo,
                            ICommonRepository<PointTrading> pointTrandingRepo)
        {
            _userRepo = userRepo;
            _pointTrandingRepo = pointTrandingRepo;
        }

        public async Task<StatusMessage> ChangePointAsync(int userId, int amount, string message)
        {
            var user = (await _userRepo.GetByIdAsync(userId)).Data as User;
            if (user == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "User was not found."
                };
            }

            int userPoint = user.Point;
            int afterChangePoint = userPoint + amount;

            if(afterChangePoint < 0)
            {
                return new StatusMessage
                {
                    IsSuccess = false,
                    Message = "Point is not enough."
                };
            }

            user.Point = afterChangePoint;

            PointTrading trading = new PointTrading()
            {
                UserId = userId,
                Point = amount,
                TradeDescription = message
            };

            await _pointTrandingRepo.CreateAsync(trading);
            await _userRepo.UpdateAsync(user);

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = "Trading successful."
            };
        }
    }
}
