using restful_api_joaodias.Data.VO;
using restful_api_joaodias.Model;
using restful_api_joaodias.Model.Context;
using System.Security.Cryptography;
using System.Text;

namespace restful_api_joaodias.Repository.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly MySQLContext _context;

        public UserRepository(MySQLContext context)
        {
            _context = context;
        }
        public User ValidateCredentials(UserVO user)
        {
            var password = ComputeHash(user.Password);
            return _context.Users.FirstOrDefault(u => (u.UserName == user.UserName) && (u.Password == password));
        }
        public User RefreshUserInfo(User user)
        {
            if (!_context.Users.Any(u => u.Id.Equals(user.Id)))
            {
                return null;
            }
            var result = _context.Users.SingleOrDefault(p => p.Id.Equals(user.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        private object ComputeHash(string input)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = SHA256.HashData(inputBytes);
            return BitConverter.ToString(hashedBytes);
        }

        public User ValidateCredentials(string userName)
        {
            return _context.Users?.SingleOrDefault(u => u.UserName == userName);
        }

        public bool RevokeToken(string userName)
        {

            try
            {
                var user = _context.Users.SingleOrDefault(u => u.UserName == userName);
                if (user is null)
                {
                    return false;
                }

                user.RefreshToken = null;

                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
