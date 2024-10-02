using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;

namespace MyStore.Server.Models.Repository.Interfaces
{
    public interface IMemberRepository
    {
        Task<MemberDataModel?> GetAsync(string email);
        Task<MemberDataModel?> CheckAsync(MemberCondition memberCondition);
        Task<TMember> AddAsync(MemberCondition member);
    }
}
