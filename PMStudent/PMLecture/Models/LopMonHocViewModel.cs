namespace PMLecture.Models
{
    public class LopMonHocViewModel
    {
        public string MaLopMonHoc { get; set; }

        public string TenLopMonHoc { get; set; }

        public string MaGiangVien { get; set; }

        public string TenGiangVien { get; set; }

        public string MaMonHoc { get; set; }

        public string TenMonHoc { get; set; }

        public string TongSoTiet { get; set; }

        public string SoTietLyThuyet { get; set; }

        public string SoTietThucHanh { get; set; }

        public string HocKy { get; set; }

        public string NamHoc { get;set; }

        public string SoSinhVien { get; set; }

        public List<SinhVienViewModel> SinhVien { get; set; }
    }
}
