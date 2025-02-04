
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tesseract;

[TestClass]
public class OcrTest
{
    private static readonly string ImageFilePath = ".\\target.png";

    [TestMethod]
    public void OcrWithMicrosoftOcrTest()
    {
        Test(filename => this.OcrWithMicrosoftOcr(filename).Result);
    }

    [TestMethod]
    public void OcrWithTesseractTest()
    {
        Test(this.OcrWithTesseract);
    }

    private static void Test(Func<string, string> ocr)
    {
        ocr(ImageFilePath).ReplaceLineEndings(string.Empty).Trim().Is("OCR Test");
    }

    private async Task<string> OcrWithMicrosoftOcr(string filename)
    {
        var filePath = Path.GetFullPath(filename);
        var file = await StorageFile.GetFileFromPathAsync(filePath);
        using var stream = await file.OpenAsync(FileAccessMode.Read);

        var decoder = await BitmapDecoder.CreateAsync(stream);
        using var bitmap = await decoder.GetSoftwareBitmapAsync();

        var engine = OcrEngine.TryCreateFromUserProfileLanguages();
        var result = await engine.RecognizeAsync(bitmap);
        return result.Text;
    }

    private string OcrWithTesseract(string filename)
    {
        using var tesseract = new TesseractEngine(".\\tessdata_fast", "eng");

        using var pix = Pix.LoadFromFile(filename);
        using var page = tesseract.Process(pix);
        return page.GetText();
    }
}
