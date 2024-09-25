using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;
using System.Reflection;

namespace MyStore.Server.Models.Service.Interfaces
{
    public interface IMemberService
    {
        Task<bool> CreateMemberAsync(MemberAuthInfo member);
        Task<MemberResultModel?> LoginAsync(MemberAuthInfo memberAuthInfo);

    }
}
