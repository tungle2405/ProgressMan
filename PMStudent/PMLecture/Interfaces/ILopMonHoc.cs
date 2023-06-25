using CoreLib.DTO;
using PMLecture.Models;

namespace PMLecture.Interfaces
{
    public interface ILopMonHoc
    {
        List<LopMonHocViewModel> GetAllLopMonHoc(string maPhanQuyen);

        List<LopMonHocViewModel> GetLopMonHocTheoGiangVien(string maGiangVien);

        LopMonHocViewModel GetLopMonHoc(string maLopMonHoc);

        List<string> GetMaLopMonHoc(string maLopMonHoc);

        CResponseMessage InsertLopMonHoc(LopMonHocViewModel lopMonHoc);

        CResponseMessage UpdateLopMonHoc(LopMonHocViewModel lopMonHoc);

        CResponseMessage DeleteLopMonHoc(string maLopMonHoc);

        //Sinh viên học lớp môn học
        CResponseMessage InsertSVHocLopMonHocTheoLop(string lopNienChe, string maLopMH);

        CResponseMessage InsertMotSVHocLopMonHocp(string maSinhVien, string maLopMH);

        CResponseMessage DeleteSVHocLopMonHocTheoLop(string MaSinhVien, string MaLopMH);
    }
}
