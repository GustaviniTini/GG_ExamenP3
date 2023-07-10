using GG_ExamenP3.GG_MVVM.Views;

namespace GG_ExamenP3;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new GG_ClimaView();
	}
}
