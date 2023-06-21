using CoreLib.DTO;
using PMLecture.Models;

namespace PMLecture.Interfaces
{
    public interface IKiemTra
    {
        List<BuoiHocViewModel> GetAllBuoiKiemTra();

        List<BuoiHocViewModel> GetAllBuoiKiemTra(string maLopMonHoc);

        public CResponseMessage ThemDiemKiemTraSinhVien(KiemTraViewModel listKiemTra);
    }
}
