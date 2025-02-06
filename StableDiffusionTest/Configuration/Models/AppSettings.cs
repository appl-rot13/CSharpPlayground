
namespace StableDiffusionTest.Configuration.Models;

using StableDiffusion.NET;

public class AppSettings
{
    private string? prompt;
    private string? negativePrompt;

    public required string ModelPath { get; init; }
    public string? LoraModelPath { get; init; }

    public string? InitImagePath { get; init; }
    public required string GeneratedImagePath { get; init; }
    public required int GeneratedCount { get; init; }

    public required IReadOnlyCollection<string> Prompts { get; init; }
    public required IReadOnlyCollection<string> NegativePrompts { get; init; }

    public string Prompt => this.prompt ??= string.Join(',', this.Prompts);
    public string NegativePrompt => this.negativePrompt ??= string.Join(',', this.NegativePrompts);

    public int? Width { get; init; }
    public int? Height { get; init; }
    public Sampler? SampleMethod { get; init; }
    public int? SampleSteps { get; init; }
    public float? Strength { get; init; }
    public int? ClipSkip { get; init; }
    public float? CfgScale { get; init; }

    public DiffusionModel CreateDiffusionModel()
    {
        var modelBuilder = ModelBuilder.StableDiffusion(this.ModelPath).WithMultithreading();
        if (!string.IsNullOrWhiteSpace(this.LoraModelPath))
        {
            modelBuilder.WithLoraSupport(this.LoraModelPath);
        }

        return modelBuilder.Build();
    }

    public DiffusionParameter CreateDiffusionParameter()
    {
        var parameter = DiffusionParameter.SD1Default;
        if (!string.IsNullOrWhiteSpace(this.NegativePrompt))
        {
            parameter.WithNegativePrompt(this.NegativePrompt);
        }

        if (this.Width.HasValue || this.Height.HasValue)
        {
            parameter.WithSize(this.Width, this.Height);
        }

        if (this.SampleMethod.HasValue)
        {
            parameter.WithSampler(this.SampleMethod.Value);
        }

        if (this.SampleSteps.HasValue)
        {
            parameter.WithSteps(this.SampleSteps.Value);
        }

        if (this.Strength.HasValue)
        {
            //parameter.WithStrength(this.Strength.Value);
            parameter.Strength = this.Strength.Value;
        }

        if (this.ClipSkip.HasValue)
        {
            parameter.WithClipSkip(this.ClipSkip.Value);
        }

        if (this.CfgScale.HasValue)
        {
            parameter.WithCfg(this.CfgScale.Value);
        }

        return parameter;
    }
}
