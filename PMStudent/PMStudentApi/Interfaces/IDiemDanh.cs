using PMStudentApi.Models;

namespace PMLecture.Interfaces
{
    public interface IDiemDanh
    {
        //List<CaHocViewModel> GetAllCaHoc();

        //List<BuoiHocViewModel> GetAllBuoiHoc();

        List<DiemDanhViewModel> GetAllDiemDanhByMaSinhVien(string maSinhVien, string maLopMonHoc);

        List<LopMonHocViewModel> GetLopMonHocTheoSinhVien(string maSinhVien);
    }
}
