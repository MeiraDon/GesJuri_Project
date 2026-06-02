using System.Text.Json;

namespace GesCPSI_Project.Reports
{
    public class ActReportJsonService
    {
        private readonly ActReportDataBuilder _builder;
        private readonly IWebHostEnvironment _env;

        public ActReportJsonService(
            ActReportDataBuilder builder,
            IWebHostEnvironment env)
        {
            _builder = builder;
            _env = env;
        }

        public async Task<string> GenerateJsonFileAsync(int acteId)
        {
            var payload = await BuildPayloadAsync(acteId);

            var folder = Path.Combine(_env.WebRootPath, "uploads", "actes", "json");
            Directory.CreateDirectory(folder);

            var fileName = $"acte_{acteId}_{DateTime.Now:yyyyMMddHHmmss}.json";
            var fullPath = Path.Combine(folder, fileName);

            var json = JsonSerializer.Serialize(payload, GetJsonOptions());
            await File.WriteAllTextAsync(fullPath, json);

            return fullPath;
        }

        public async Task<string> GetJsonContentAsync(int acteId)
        {
            var payload = await BuildPayloadAsync(acteId);
            return JsonSerializer.Serialize(payload, GetJsonOptions());
        }

        private async Task<List<ActCautionnementReportModel>> BuildPayloadAsync(int acteId)
        {
            var model = await _builder.BuildAsync(acteId);
            return new List<ActCautionnementReportModel> { model };
        }

        private static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }
    }
}
