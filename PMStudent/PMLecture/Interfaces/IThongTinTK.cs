using CoreLib.DTO;
using PMLecture.Models;

namespace PMLecture.Interfaces
{
    public interface IThongTinTK
    {
        GiangVienViewModel GetThongTin(string maNhanVien);
    }
}
