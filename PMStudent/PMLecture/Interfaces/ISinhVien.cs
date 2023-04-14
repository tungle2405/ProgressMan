using CoreLib.DTO;
using PMLecture.Models;

namespace PMLecture.Interfaces
{
    public interface ISinhVien
    {
        List<SinhVienViewModel> GetAllSinhVien();

        SinhVienViewModel GetSinhVien(string maSinhVien);

        List<string> GetMaSinhVien(string maSinhVien);

        CResponseMessage InsertSinhVien(SinhVienViewModel sinhVien);

        CResponseMessage UpdateSinhVien(SinhVienViewModel sinhVien);

        CResponseMessage DeleteSinhVien(string maSinhVien);
    }
}
