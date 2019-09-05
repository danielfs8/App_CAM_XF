using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media.Abstractions;
using Plugin.Media;

namespace App_CAM_XF
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void TirarFoto(Object sender, EventArgs args)
        {
            

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                    await DisplayAlert("Ops", "Nenhuma Câmera encontrada!", "Ok");
                    return;
            }

            var Foto = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "MeuAlbum",
                    //Name = "NomeDoArquivo" 
                }); 

            if (Foto == null)
            {
                return;
            }


            MinhaFoto.Source = ImageSource.FromStream(() => 
            {
                var CaminhoFoto = Foto.GetStream();
                Foto.Dispose();
                return CaminhoFoto;

            });
            
            

        }


        
    }
}
