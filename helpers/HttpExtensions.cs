namespace photoContainer.helpers;

public static class HttpExtensions
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header) {
            response.Headers["Pagination"] = JsonSerializer.Serialize(header, JsonOptions);
            response.Headers["Access-Control-Expose-Headers"] = "Pagination";
        }
    }
