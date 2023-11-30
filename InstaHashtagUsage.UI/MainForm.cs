using Microsoft.AspNetCore.Components.WebView.WindowsForms;

namespace InstaHashtagUsage.UI
{
    public partial class MainForm : Form
    {
        public MainForm(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            blazorWebView1.HostPage = "wwwroot\\index.html";
            blazorWebView1.Services = serviceProvider;
            blazorWebView1.RootComponents.Add<StartScreen>("#app");
        }
    }
}
