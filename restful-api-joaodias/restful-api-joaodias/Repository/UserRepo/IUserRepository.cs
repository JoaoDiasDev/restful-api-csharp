using restful_api_joaodias.Data.VO;
using restful_api_joaodias.Model;

namespace restful_api_joaodias.Repository.UserRepo
{
    public interface IUserRepository
    {
        User ValidateCredentials(UserVO user);
        User ValidateCredentials(string userName);
        bool RevokeToken(string userName);
        User RefreshUserInfo(User user);
    }
}
