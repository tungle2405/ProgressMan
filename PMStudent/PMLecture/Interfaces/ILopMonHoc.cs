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

        CResponseMessage DeleteLopMonHoc(LopMonHocViewModel lopMonHoc);
    }
}
