namespace PMLecture.Models
{
    public class KiemTraViewModel
    {

        public string ID { get; set; }

        public string MaGiangVien { get; set; }

        public string MaMonHoc { get; set; }

        public string MaLopMonHoc { get; set; }

        public string MaBuoiKiemTra { get; set; }

        public List<DiemKiemTraSinhVien> sinhVienKiemTras { get; set; }
    }

    public class DiemKiemTraSinhVien
    {
        public string MaSinhVien { get; set; }

        public float DiemKiemTra { get; set; }

    }
}
