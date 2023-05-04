using CoreLib.DTO;
using PMLecture.Models;

namespace PMLecture.Interfaces
{
    public interface IMonHoc
    {
        List<MonHocViewModel> GetAllMonHoc();

        MonHocViewModel GetMonHoc(string maMonHoc);

        CResponseMessage InsertMonHoc(MonHocViewModel monHoc);

        CResponseMessage UpdateMonHoc(MonHocViewModel monHoc);

        CResponseMessage DeleteMonHoc(MonHocViewModel monHoc);
    }
}
