using ChamDiemQuaTrinh.HttpRepository;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace ChamDiemQuaTrinh.Pages
{
    public partial class Logout
    {

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthenticationService.Logout();
            NavigationManager.NavigateTo("/");
        }
    }
}
