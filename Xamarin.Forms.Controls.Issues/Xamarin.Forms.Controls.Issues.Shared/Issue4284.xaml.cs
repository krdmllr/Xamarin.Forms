using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Controls.Issues
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Preserve(AllMembers = true)]
    [Issue(IssueTracker.Github, 4284, "ListView selection done programatically is not reflected in UI Xamarin.Forms.WPF", PlatformAffected.WPF)]
	public partial class Issue4284
    {
	    public Issue4284()
	    {
			InitializeComponent();
			myList.ItemsSource = numList;
		}

	    ObservableCollection<string> numList = new ObservableCollection<string>
	    {
		    "One","Two","Three","Four","Five","Six","Seven","Eight","Nine","Ten"
	    }; 

	    private void myButton_Clicked(object sender, EventArgs e)
	    {
		    myList.SelectedItem = numList[4];
	    }

		protected override void Init()
	    {
		    
		}
    }
}