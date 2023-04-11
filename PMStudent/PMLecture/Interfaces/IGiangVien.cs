using CoreLib.DTO;
using PMLecture.Models;

namespace PMLecture.Interfaces
{
    public interface IGiangVien
    {
        List<GiangVienViewModel> GetAll();

        GiangVienViewModel GetGiangVien(string maGiangVien);

        CResponseMessage AddGiangVien(GiangVienViewModel giangVien);

        CResponseMessage UpdateGiangVien(GiangVienViewModel giangVien);

        CResponseMessage DeleteGiangVien(string maGiangVien);
    }
}
