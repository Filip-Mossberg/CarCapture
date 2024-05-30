using BLL;
using BLL.IService;
using BLL.Service;
using BLL.UIService;
using CarCaptureUI.Components;
using Microsoft.Extensions.ML;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ICarDetectorService, CarDetectorService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IColorClassificationService, ColorClassificationService>();
builder.Services.AddScoped<ICarDetectorUIService, CarDetectorUIService>();

builder.Services.AddPredictionEnginePool<CarDetectorModel.ModelInput, CarDetectorModel.ModelOutput>()
.FromFile("C:\\Users\\Joakim\\source\\repos\\CarCapture\\BLL\\CarDetectorModel.mlnet");

builder.Services.AddPredictionEnginePool<CarColorClassificationModel.ModelInput, CarColorClassificationModel.ModelOutput>()
.FromFile("C:\\Users\\Joakim\\source\\repos\\CarCapture\\BLL\\CarColorClassificationModel.mlnet");

//builder.Services.AddServerSideBlazor()
//    .AddCircuitOptions(options =>
//    {
//        options.DetailedErrors = true;
//        options.DisconnectedCircuitMaxRetained = 100;
//        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
//        options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(3);
//        options.MaxBufferedUnacknowledgedRenderBatches = 10;
//    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
