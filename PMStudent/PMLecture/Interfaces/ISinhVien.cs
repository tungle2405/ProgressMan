using CoreLib.DTO;
using PMLecture.Models;

namespace PMLecture.Interfaces
{
    public interface ISinhVien
    {
        List<SinhVienViewModel> GetAllSinhVien();

        SinhVienViewModel GetSinhVien(string maSinhVien);

        CResponseMessage AddSinhVien(SinhVienViewModel sinhVien);

        CResponseMessage UpdateSinhVien(SinhVienViewModel sinhVien);

        CResponseMessage DeleteSinhVien(string maSinhVien);
    }
}
