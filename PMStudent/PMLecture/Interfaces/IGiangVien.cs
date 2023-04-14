using CoreLib.DTO;
using PMLecture.Models;

namespace PMLecture.Interfaces
{
    public interface IGiangVien
    {
        List<GiangVienViewModel> GetAllGiangVien();

        List<GiangVienViewModel> GetAllPhongDaoTao();

        GiangVienViewModel GetGiangVien(string maGiangVien);

        CResponseMessage InsertNhanVien(GiangVienViewModel giangVien);

        CResponseMessage UpdateGiangVien(GiangVienViewModel giangVien);

        CResponseMessage DeleteGiangVien(string maGiangVien);
    }
}
