namespace ProyectoAdoptBack.Services
{
    public interface ISupabaseStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder);
        Task DeleteAsync(string url);
    }

    public class SupabaseStorageService : ISupabaseStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _serviceRoleKey;
        private const string Bucket = "imagenes";

        public SupabaseStorageService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["Supabase:Url"]!.TrimEnd('/');
            _serviceRoleKey = configuration["Supabase:ServiceRoleKey"]!;
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder)
        {
            var filePath = $"{Bucket}/{folder}/{Guid.NewGuid()}-{fileName}";
            var requestUri = $"{_baseUrl}/storage/v1/object/{filePath}";

            using var content = new StreamContent(fileStream);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = content
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _serviceRoleKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return $"{_baseUrl}/storage/v1/object/public/{filePath}";
        }

        public async Task DeleteAsync(string url)
        {
            if (!url.Contains("/storage/v1/object/public/")) return;

            var relativePath = url.Substring(url.IndexOf("/storage/v1/object/public/") + "/storage/v1/object/public/".Length);
            var requestUri = $"{_baseUrl}/storage/v1/object/{relativePath}";

            using var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _serviceRoleKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
