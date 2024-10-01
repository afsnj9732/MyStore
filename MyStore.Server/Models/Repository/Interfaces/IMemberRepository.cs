using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;

namespace MyStore.Server.Models.Repository.Interfaces
{
    public interface IMemberRepository
    {
        Task<MemberDataModel?> GetMemberAsync(string email);
        Task<MemberDataModel?> CheckMemberAsync(MemberCondition memberCondition);
        Task<TMember> AddMemberAsync(MemberCondition member);
    }
}
