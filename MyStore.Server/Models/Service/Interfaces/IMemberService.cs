using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;

namespace MyStore.Server.Models.Service.Interfaces
{
    public interface IMemberService
    {
        Task<bool> CreateMemberAsync(MemberAuthInfo member);
        Task<MemberResultModel?> GetMemberAsync(MemberAuthInfo memberAuthInfo);

    }
}
