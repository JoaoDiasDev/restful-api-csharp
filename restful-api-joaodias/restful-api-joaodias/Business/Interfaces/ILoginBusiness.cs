using restful_api_joaodias.Data.VO;

namespace restful_api_joaodias.Business.Interfaces
{
    public interface ILoginBusiness
    {
        TokenVO ValidateCredentials(UserVO user);
        TokenVO ValidateCredentials(TokenVO token);
        bool RevokeToken(string userName);
    }
}
