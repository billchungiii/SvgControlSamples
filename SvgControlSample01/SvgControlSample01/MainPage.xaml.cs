using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SvgControlSample01
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async private void SvgControl_Touch(object sender, SkiaSharp.Views.Forms.SKTouchEventArgs e)
        {
            await DisplayAlert("Test", "Click from svg control", "OK");
        }
    }
}
