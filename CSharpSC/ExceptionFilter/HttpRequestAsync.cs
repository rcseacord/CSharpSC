using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CLSCompliant(true)]
namespace SecureCSharp
{
  class ExceptionFilterDemo 
  {
    public static async Task<string> MakeRequestWithNotModifiedSupportAsync() 
    {
      var client = new System.Net.Http.HttpClient();
      try
      {
        var streamTask = await client.GetStringAsync("https://localHost:10000");
        return streamTask;
      }
      catch (System.Net.Http.HttpRequestException e) when (e.Message.Contains("301"))
      {
        return "Site Moved";
      }
      catch (System.Net.Http.HttpRequestException e)
      {
        if (e.Message.Contains("301"))
          return "Site Moved";
        else
          throw;
      }
    }

    public static string WaitOnRequestAsync()
    {
      var value = MakeRequestWithNotModifiedSupportAsync().Result;
      return value;
    }

    static void Test()
    {
      var responseText = WaitOnRequestAsync();
      Console.WriteLine(responseText);
    }
  }
}
