using System.Reflection;

namespace Employee_Hotline.Tests.Integration.Fixtures;

public static class FileUploadHelper
{
    private static readonly string TestFilesPath = Path.Combine(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Integration", "TestFiles");

    public static MultipartFormDataContent CreateFileContent(
        string fileName, string mediaType)
    {
        var filePath = Path.Combine(TestFilesPath, fileName);

        // 경로 디버깅용
        if (!File.Exists(filePath))
            throw new FileNotFoundException(
                $"테스트 파일을 찾을 수 없습니다.\n경로: {filePath}\n" +
                $"존재하는 파일: {string.Join(", ", Directory.GetFiles(Path.GetDirectoryName(filePath)!))}");

        var fileBytes = File.ReadAllBytes(filePath);
        var formData  = new MultipartFormDataContent();

        formData.Add(
            new ByteArrayContent(fileBytes) { Headers = { ContentType = new(mediaType) } },
            name: "file",
            fileName: fileName
        );

        return formData;
    }
}