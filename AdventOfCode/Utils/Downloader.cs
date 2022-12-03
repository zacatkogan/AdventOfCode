namespace AdventOfCode.Utils;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;

public class Downloader
{
    private const string SESSION_COOKIE = "53616c7465645f5f88f10881ad7bb00bd5de3ae597b0593046f6ca57a73adcd1de70ea8006a2bc0df7ea5b1820475fce20c13f7a2f87908d53b28ee537d1cfc6";
    private const string URL = "https://adventofcode.com/{0}/day/{1}/input";

    public static string DefaultPath = "./data/{0}/{1}.txt"; // 0 - year, 1 - day

    public string OutputPath = DefaultPath;

    public Downloader() : this(SESSION_COOKIE) {}

    public Downloader(string cookie) => this.Cookie = cookie;
    
    public string Cookie;

    public string GetData(int day, int year)
    {
        if (TryLoadData(day, year, out string data))
            return data;

        data = DownloadData(day, year);
        SaveData(day, year, data);
        return data;
    }

    public void SaveData(int day, int year, string data)
    {
        var path = string.Format(OutputPath, year, day);

        var parentDir = Directory.GetParent(path)!;

        if (!parentDir.Exists)
            System.IO.Directory.CreateDirectory(parentDir.FullName);

        System.IO.File.WriteAllText(path, data);
    }

    public bool TryLoadData(int day, int year, out string data)
    {
        var path = string.Format(OutputPath, year, day);

        try
        {
            data = System.IO.File.ReadAllText(path);
            return true;
        }
        catch (System.Exception)
        {
            data = "";
            return false;
        }
    }

    public string DownloadData(int day, int year)
    {
        var client = new HttpClient();

        var url = string.Format(URL, year, day);

        var message = new HttpRequestMessage(HttpMethod.Get, url);
        message.Headers.Add("cookie", "session=" + SESSION_COOKIE);

        var response = client.Send(message);

        response.EnsureSuccessStatusCode();

        return response.Content.ReadAsStringAsync().Result;
    }
}
