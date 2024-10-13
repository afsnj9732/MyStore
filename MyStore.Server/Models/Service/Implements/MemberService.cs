using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;
using MyStore.Server.Models.Service.Interfaces;
using MyStore.Server.Models.UnitOfWork;

namespace MyStore.Server.Models.Service.Implements
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MemberService> _logger;
        public MemberService(IUnitOfWork unitOfWork, ILogger<MemberService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> CreateMemberAsync(MemberAuthInfo memberInfo)
        {
            var member = await _unitOfWork.MemberRepository.GetAsync(memberInfo.Email);
            if (member != null)
            {
                _logger.LogWarning("會員帳號已經註冊");
                return false;
            }
            var memberCondition = new MemberCondition
            {
                Email = memberInfo.Email,
                Password = memberInfo.Password
            };
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var newMember = await _unitOfWork.MemberRepository.AddAsync(memberCondition);
                    await _unitOfWork.SaveChangeAsync();//關聯式資料庫需先透過SaveChanges()才能獲得識別項主鍵
                    await _unitOfWork.CartRepository.CreateAsync(newMember.MemberId);//透過SaveChange()返回的主鍵
                    await _unitOfWork.SaveChangeAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch(Exception ex) 
                {
                    _logger.LogError("會員新增錯誤,參考訊息:{Message}",ex.Message);
                    await transaction.RollbackAsync();
                    return false;
                }
            }

        }
        public async Task<MemberResultModel?> GetMemberAsync(MemberAuthInfo memberAuthInfo)
        {
            var condition = new MemberCondition
            {
                Email = memberAuthInfo.Email,
                Password = memberAuthInfo.Password
            };
            var checkResult = await _unitOfWork.MemberRepository.CheckAsync(condition);
            if (checkResult == null)
            {
                return null;
            }
            var result = new MemberResultModel
            {
                Email = checkResult.Email,
                MemberId = checkResult.MemberId,
                UserName = checkResult.UserName
            };
            return result;
        }
    }
}
