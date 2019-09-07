# App_CAM_XF

Exemplo simples de como usar a Câmera com Xamarin Forms e o plugin Xam.Plugin.Media

Para utilizar a camera utilizo os seguintes Plugins-NuGet: 
 - plugin Xam.Plugin.Media - 4.3.1-beta 
 - Plugin.Permissions - 5.0.0-beta
 
Ambos desenvolvidos por [James Montemagno](https://github.com/jamesmontemagno).

Fique atento à versão do plugin que está intalando no projeto, pois pode ser necessário uma implementação diferente.
Os arquivos README disponibilizados nos Plugins são bem simples de enternder e bem completos.


No arquivo MainActivity.cs (Android) no método "OnRequestPermissionsResult" precisamos adicionar as duas linhas a seguir:
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
