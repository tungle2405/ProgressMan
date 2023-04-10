using CoreLib.DTO;
using PMLecture.Models;

namespace PMLecture.Interfaces
{
    public interface ILoginGV
    {
        CResponseMessage CheckLogin(LoginViewModel login);
    }
}
