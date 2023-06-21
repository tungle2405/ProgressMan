using PMLecture.Models;
using PMStudentApi.Models;

namespace PMLecture.Interfaces
{
    public interface IDiemDanh
    {
        //List<CaHocViewModel> GetAllCaHoc();

        SinhVienViewModel GetThongTinSV(string maSinhVien);

        List<DiemDanhViewModel> GetAllDiemDanhByMaSinhVien(string maSinhVien, string maLopMonHoc);

        List<LopMonHocViewModel> GetLopMonHocTheoSinhVien(string maSinhVien);
    }
}
