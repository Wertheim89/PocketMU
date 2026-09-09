using Android.App;
using Android.OS;
using Android.Widget;

namespace PocketMU.SmokeAndroid;

[Activity(Label = "PocketMU Gate A", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
public sealed class MainActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        var view = new LinearLayout(this) { Orientation = Orientation.Vertical };
        view.SetPadding(48, 48, 48, 48);
        view.AddView(new TextView(this)
        {
            Text = "PocketMU\nPhase 1 Gate A\nAsset-free Android smoke build is running.",
            TextSize = 24
        });
        SetContentView(view);
    }
}

