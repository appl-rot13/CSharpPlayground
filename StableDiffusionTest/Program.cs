
using HPPH;
using HPPH.System.Drawing;

using StableDiffusion.NET;

using StableDiffusionTest.Configuration;
using StableDiffusionTest.Configuration.Models;

try
{
    var appSettings = Configuration.Load<AppSettings>("appsettings.json");

    var initImage =
        string.IsNullOrWhiteSpace(appSettings.InitImagePath)
            ? null
            : ImageHelper.LoadImage(appSettings.InitImagePath).ConvertTo<ColorRGB>();

    var parameter = appSettings.CreateDiffusionParameter();
    using var model = appSettings.CreateDiffusionModel();

    var random = new Random(Environment.TickCount);
    foreach (var i in Enumerable.Range(1, appSettings.GeneratedCount))
    {
        parameter.WithSeed(random.NextInt64(0, long.MaxValue));

        var image =
            initImage == null
                ? model.TextToImage(appSettings.Prompt, parameter)
                : model.ImageToImage(appSettings.Prompt, initImage, parameter);

        var filePath = string.Format(appSettings.GeneratedImagePath, i);
        await File.WriteAllBytesAsync(filePath, image.ToPng());
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.ReadLine();
}
