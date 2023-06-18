namespace PMLecture.Models
{
    public class DiemDanhViewModel
    {
        public string ID { get; set; }

        public string MaGiangVien { get; set; }

        public string MaMonHoc { get; set; }

        public string MaLopMonHoc { get; set; }

        public string MaCaHoc { get; set; }

        public string MaBuoiHoc { get; set; }

        public DateTime NgayDiemDanh { get; set; }
       
        public List<SinhVienDiemDanh> sinhVienDiemDanhs { get; set; }
    }

    public class SinhVienDiemDanh
    {
        public string MaSinhVien { get; set; }

        public bool TrangThai { get; set; }
    }
}
