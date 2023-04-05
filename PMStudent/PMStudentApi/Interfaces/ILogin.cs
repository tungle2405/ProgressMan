using CoreLib.DTO;
using PMStudentApi.Model;

namespace PMStudentApi.Interfaces
{
    public interface ILogin
    {
        CResponseMessage CheckLogin(LoginApiModel login);
    }
}
