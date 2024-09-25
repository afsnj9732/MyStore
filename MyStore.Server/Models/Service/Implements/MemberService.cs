using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Interfaces;
using MyStore.Server.Models.Service.Dtos.ResultModels;
using MyStore.Server.Models.UnitOfWork;

namespace MyStore.Server.Models.Service.Implements
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateMemberAsync(MemberAuthInfo memberInfo)
        {
            var member = await _unitOfWork.MemberRepository.GetMemberAsync(memberInfo.Email);
            if (member != null)
            {
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
                    var newMember = await _unitOfWork.MemberRepository.AddMemberAsync(memberCondition);
                    await _unitOfWork.Save();//關聯式資料庫需先透過SaveChanges()才能獲得識別項主鍵
                    await _unitOfWork.CartRepository.CreateCartAsync(newMember.MemberId);//透過SaveChange()返回的主鍵
                    await _unitOfWork.Save();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }

        }
        public async Task<MemberResultModel?> LoginAsync(MemberAuthInfo memberAuthInfo)
        {
            var condition = new MemberCondition
            {
                Email = memberAuthInfo.Email,
                Password = memberAuthInfo.Password
            };
            var checkResult = await _unitOfWork.MemberRepository.CheckMemberAsync(condition);
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
