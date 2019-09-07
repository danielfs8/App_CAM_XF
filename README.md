# App_CAM_XF

Exemplo simples de como usar a Câmera com Xamarin Forms e o plugin Xam.Plugin.Media

Para utilizar a camera utilizo os seguintes Plugins-NuGet: 
 - plugin Xam.Plugin.Media - 4.3.1-beta 
 - Plugin.Permissions - 5.0.0-beta
 
Ambos desenvolvidos por [James Montemagno](https://github.com/jamesmontemagno).

Fique atento à versão do plugin que está intalando no projeto, pois pode ser necessário uma implementação diferente.
Os arquivos README disponibilizados nos Plugins são bem simples de enternder e bem completos.

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




