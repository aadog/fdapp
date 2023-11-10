using Java.IO;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Byte = Java.Lang.Byte;

namespace fdapp.Service
{
    public partial class RootShell
    {
        public static async partial Task<string> Exec(string commond)
        {
            var strRes = "";
            using var process = Runtime.GetRuntime()!.Exec("su");
            await using var outputStream = process?.OutputStream;
            await using var inputStream = process?.InputStream;
            await using var errorStream=process?.ErrorStream;
            await outputStream!.WriteAsync(Encoding.UTF8.GetBytes($"{commond}"));
            await outputStream!.WriteAsync(Encoding.UTF8.GetBytes("\n"));
            await outputStream!.FlushAsync();
            outputStream.Close();
            await process.WaitForAsync();
            if (process.ExitValue() != 0)
            {
                using var rd=new StreamReader(errorStream!);
                var r = await rd.ReadToEndAsync();
                if (r == "")
                {
                    r=await rd.ReadToEndAsync();
                }
                throw new System.Exception(r);
            }
            else
            {
                using var rd = new StreamReader(inputStream!);
                strRes = await rd.ReadToEndAsync();
            }

            return strRes.TrimEnd('\n');
        }
    }
}

