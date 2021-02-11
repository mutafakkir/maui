using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace Xamarin.Forms.Platform.UWP
{
	public interface IIconElementHandler : IRegisterable
	{
		Task<Microsoft.UI.Xaml.Controls.IconSource> LoadIconSourceAsync(ImageSource imagesource, CancellationToken cancellationToken = default(CancellationToken));
		Task<IconElement> LoadIconElementAsync(ImageSource imagesource, CancellationToken cancellationToken = default(CancellationToken));
	}
}