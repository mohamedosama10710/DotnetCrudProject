using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NSwag;
using NSwag.CodeGeneration.CSharp;

public class ClientGenerator
{
    public static async Task GenerateClientCode()
    {
        var httpClient = new HttpClient();
        var swaggerJson =await httpClient.GetStringAsync("http://localhost:5106/swagger/v1/swagger.json");
        var document = await OpenApiDocument.FromJsonAsync(swaggerJson);

        var settings = new CSharpClientGeneratorSettings
        {
            ClassName = "ApiClient",
            CSharpGeneratorSettings={Namespace="ApiClientNameSpace"}
        };

        var generator = new CSharpClientGenerator(document, settings);
        var code = generator.GenerateFile();

        await File.WriteAllTextAsync("ApiClient.cs",code);

    }

  
}