using fdapp.Service;
using Microsoft.Maui.Controls.Compatibility;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;

namespace fdapp
{
    public partial class MainPage : ContentPage
    {
        private string fridaName="fs";
        private MainPageBindingModel _mainPageBindingModel = new MainPageBindingModel();
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = _mainPageBindingModel;
            Task.Run(async () =>
            {
                var r=await RootShell.Exec("whoami");
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    _mainPageBindingModel.rootState = $"({r})";
                    _mainPageBindingModel.Ref();
                });
                var state = await RootShell.Exec($"ps -ef|grep {fridaName}|grep -v grep");
                if (state!="") {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        _mainPageBindingModel.fridaIsStart = true;
                        _mainPageBindingModel.Ref();
                    });
                }
            });
        }
        

        private async void ButtonStartFrida_Clicked(object sender, EventArgs e)
        {
            try
            {
                _mainPageBindingModel.executeIng = true;
                _mainPageBindingModel.Ref();
                
                if (!File.Exists($"/data/local/tmp/{fridaName}"))
                {
                    await DisplayAlert("cuow", "test", "ok");
                    await using var stream = await FileSystem.OpenAppPackageFileAsync("fs-16.1.5");
                    await using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);



                    File.WriteAllBytes($"{FileSystem.CacheDirectory}/{fridaName}", memoryStream.ToArray());
                    await RootShell.Exec($"cp {FileSystem.CacheDirectory}/{fridaName} /data/local/tmp/{fridaName}");
                    File.Delete($"{FileSystem.CacheDirectory}/{fridaName}");
                    await RootShell.Exec($"chmod 777 /data/local/tmp/{fridaName}");
                    
                }

           

                var execCommand = $"unset LANG\n/data/local/tmp/{fridaName} -D";
                if (_mainPageBindingModel.FridaUseNetwork) {
                    execCommand = $"unset LANG\n/data/local/tmp/{fridaName} -l 0.0.0.0:{_mainPageBindingModel.FridaPort} -D";
                }

             
                await RootShell.Exec(execCommand);
                _mainPageBindingModel.fridaIsStart = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("错误", $"{ex}", "ok");
            }
            finally {
                _mainPageBindingModel.executeIng = false;
                _mainPageBindingModel.Ref();
            }
        }

        private async void ButtonStopFrida_Clicked(object sender, EventArgs e)
        {
            try
            {
                _mainPageBindingModel.executeIng = true;
                _mainPageBindingModel.Ref();
                await RootShell.Exec($"pkill -9 {fridaName}");
                _mainPageBindingModel.fridaIsStart = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("错误", $"{ex}", "ok");
            }
            finally {
                _mainPageBindingModel.executeIng = false;
                _mainPageBindingModel.Ref();
            }
        }

        private async void ButtonClearFrida_Clicked(object sender, EventArgs e)
        {
            try
            {
                _mainPageBindingModel.executeIng = true;
                _mainPageBindingModel.Ref();
                await RootShell.Exec($"pkill -9 {fridaName}");
                await RootShell.Exec($"rm -rf /data/local/tmp/{fridaName}");
                _mainPageBindingModel.fridaIsStart = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("错误", $"{ex}", "ok");
            }
            finally
            {
                _mainPageBindingModel.executeIng = false;
                _mainPageBindingModel.Ref();
            }
        }
    }

}
