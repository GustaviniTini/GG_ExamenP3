using GG_ExamenP3.GG_MVVM.ViewModels;

namespace GG_ExamenP3.GG_MVVM.Views;

public partial class GG_ClimaView : ContentPage
{
	public GG_ClimaView()
	{
		InitializeComponent();
		BindingContext = new ClimaViewModel();
	}
}