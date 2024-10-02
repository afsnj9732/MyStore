using Microsoft.EntityFrameworkCore;
using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;
using MyStore.Server.Models.Repository.Interfaces;

namespace MyStore.Server.Models.Repository.Implements
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DbStoreContext _db;
        public MemberRepository(DbStoreContext db)
        {
            _db = db;
        }

        public async Task<TMember> AddAsync(MemberCondition memberInfo)
        {
            var member = new TMember
            {
                Email = memberInfo.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(memberInfo.Password),
                UserName = memberInfo.Email.Split('@')[0]
            };
            await _db.TMembers.AddAsync(member);
            return member;
        }

        public async Task<MemberDataModel?> CheckAsync(MemberCondition memberCondition)
        {
            var member = await _db.TMembers
                .Where(member => member.Email == memberCondition.Email).FirstOrDefaultAsync();
            if (member == null 
                || !BCrypt.Net.BCrypt.Verify(memberCondition.Password, member.Password)) 
            { return null; }
            var result = new MemberDataModel
            {
                MemberId = member.MemberId,
                Email = member.Email,
                UserName = member.UserName,
            };
            return result;
        }

        public async Task<MemberDataModel?> GetAsync(string email)
        {
            var member = await _db.TMembers.Where(member =>member.Email == email).FirstOrDefaultAsync();
            if (member == null) { return null; }
            var result = new MemberDataModel {
              MemberId = member.MemberId,
              Email = member.Email,
              UserName = member.UserName
            };
            return result;
        }
    }
}
