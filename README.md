# App_CAM_XF

Exemplo simples de como usar a Câmera com Xamarin Forms e o plugin Xam.Plugin.Media

Para utilizar a camera utilizo os seguintes Plugins-NuGet: 
 - [plugin Xam.Plugin.Media - 4.3.1-beta](https://github.com/jamesmontemagno/MediaPlugin)
 - [Plugin.Permissions - 5.0.0-beta](https://github.com/jamesmontemagno/PermissionsPlugin)
 
Ambos desenvolvidos por [James Montemagno](https://github.com/jamesmontemagno).

**Importante:**  

**1.** Fique atento à versão do plugin que está instalando no projeto, pois pode ser necessário uma implementação diferente.
Os arquivos README disponibilizados nos Plugins são bem simples de enternder e bem completos.

**2.** Ao ler e implementar os arquivos README de ambos os plugins fique atento, pois eles se complementam. Em algumas situações as implementações solicitadas em um são substituidas pela informada no outro.

**--------------------------------------------------------------------------------------------------------------------------**  

## No arquivo MainActivity.cs (Android):

**1.** No método  *"OnCreate"* precisamos adicionar a linhas a seguir:  

-  Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);  

**Ficou assim:**

```
protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);


            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
```

**2.** No método *"OnRequestPermissionsResult"* precisamos adicionar as duas linhas a seguir:
- base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
- PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

**Ficou assim:**
```
public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
          
           Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

           base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

           PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            
        }
```

**--------------------------------------------------------------------------------------------------------------------------**  
## Observações importantes:

>  As permissões `WRITE_EXTERNAL_STORAGE`, `READ_EXTERNAL_STORAGE` são necessárias, mas a biblioteca adiciona automaticamente para você.

> O Assembly a seguir é adicionado também: (arquivo *AssemblyInfo.cs*)
```
[assembly: UsesFeature("android.hardware.camera", Required = false)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]
```

**--------------------------------------------------------------------------------------------------------------------------**  

## No arquivo AndroidManifest.xml (Android):   

**1.** No arquivo *AndroidManifest.xml* dentro da Tag `<application>` precisamos adcionar o seguinte código:
 
```
<provider android:name="android.support.v4.content.FileProvider" 
				android:authorities="${applicationId}.fileprovider" 
				android:exported="false" 
				android:grantUriPermissions="true">
			<meta-data android:name="android.support.FILE_PROVIDER_PATHS" 
				android:resource="@xml/file_paths"></meta-data>
</provider>
```

**--------------------------------------------------------------------------------------------------------------------------**  
 
## Criação da pasta **XML** e do arquivo **`file_paths.xml`**.

Vamos Precisar adicionar uma pasta com o nome **XML** nas respectivas pastas Resources.

**1.** Adicionar uma pasta chamada **XML** dentro da pasta **Resources**.

**2.** Adicionar um arquivo XML com o nome **`file_paths.xml`** e adicionar o seguinte código:  

```
<?xml version="1.0" encoding="utf-8"?>
<paths xmlns:android="http://schemas.android.com/apk/res/android">
    <external-files-path name="my_images" path="Pictures" />
    <external-files-path name="my_movies" path="Movies" />
</paths>
```

**--------------------------------------------------------------------------------------------------------------------------**  
## No arquivo **Info.plist** (IOS):

Precisamos adicionar algumas chaves no arquivo **`Info.plist`** para solicitar as permissões ao usuário .

**1.** Adicione as seguintes chaves no arquivo **`Info.plist`**:

```
<key>NSCameraUsageDescription</key>
<string>This app needs access to the camera to take photos.</string>
<key>NSPhotoLibraryUsageDescription</key>
<string>This app needs access to photos.</string>
<key>NSMicrophoneUsageDescription</key>
<string>This app needs access to microphone.</string>
<key>NSPhotoLibraryAddUsageDescription</key>
<string>This app needs access to the photo gallery.</string>

```

**--------------------------------------------------------------------------------------------------------------------------**  

## Agora no Projeto Compartilhado

**1.** No arquivo **MainPage.xaml** criei uma tela bem simples com apenas dois componentes:  


```
<StackLayout Padding="10" Margin="10">
        <!-- Cria o componente da imagem-->
        <Image x:Name="MinhaFoto" VerticalOptions="StartAndExpand" HeightRequest="200" WidthRequest="150"/>
        
        <!-- Cria o Botão de captura da foto-->
        <Button Text="Capturar" Clicked="TirarFoto" HorizontalOptions="Center"/>
    </StackLayout>

```

**2.** No arquivo **MainPage.xaml.cs** criei uma implementação bem simples:
- Abaixo do Contrutor adicionei o métoto: TirarFoto
> Fique atento ao código, note que aqui utilizamos chamadas de métodos assíncronos, sendo assim, temos que definir o método **`TirarFoto`** como **`async`**. 
Note que a declaração do médodo **"`private async void TirarFoto(Object sender, EventArgs args){...}`"** possui a palavra reservada **`async`**, tornando assim o método assíncrono.

```
...
using Plugin.Media.Abstractions;
using Plugin.Media;
...

public MainPage()
        {
            InitializeComponent();
        }

        //Metodo executado ao clicar no botão CAPTURAR
        private async void TirarFoto(Object sender, EventArgs args)
        {
            
            //Testa se o Device(Aparelho) tem câmera e suporta tirar fotos 
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                    await DisplayAlert("Ops", "Nenhuma Câmera encontrada!", "Ok");
                    return;
            }

            //Aqui chamo o metodo que vai tirar a foto com os parametros declarados dentro do mesmo.
            var Foto = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "MeuAlbum",
                    //Name = "NomeDaFoto" ,
                    //PhotoSize = PhotoSize.Custom, /*Ajuste-o para Small, Medium, ou Large, que é 25%, 50% ou 75% ou o original. Se não informar essa parâmtro será utilizado a resolução máxima da câmera */
                    //CompressionQuality = 92    /* Valor de compactação de 0 a 100, suportado apenas no Android e iOS */
                    //DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front
                }); 
		

            //Se a foto for null(não existir) não retorno nada
            if (Foto == null)
            {
                return;
            }

            //Se a foto existe, abro ela e mostro na tela.
            MinhaFoto.Source = ImageSource.FromStream(() => 
            {
                var CaminhoFoto = Foto.GetStream();
                Foto.Dispose();
                return CaminhoFoto;

            });
            
            

        }
...

```

