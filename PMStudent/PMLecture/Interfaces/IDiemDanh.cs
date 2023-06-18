using CoreLib.DTO;
using PMLecture.Models;

namespace PMLecture.Interfaces
{
    public interface IDiemDanh
    {
        List<CaHocViewModel> GetAllCaHoc();

        List<BuoiHocViewModel> GetAllBuoiHoc();

        List<BuoiHocViewModel> GetAllBuoiHoc(string maLopMonHoc);

        CResponseMessage DiemDanhSinhVien(DiemDanhViewModel listDiemDanh);

        List<DiemChuyenCanViewModel> ListDiemChuyenCan(string maLopMonHoc);

        CResponseMessage HuyBoDD(DiemDanhViewModel listDiemDanh);
    }
}
