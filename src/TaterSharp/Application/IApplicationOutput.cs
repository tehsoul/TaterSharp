using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaterSharp.Application;
public interface IApplicationOutput
{
    void WriteHeader(string text, bool includeTimestamp = true);
    void WriteLine(string text, bool includeTimestamp = true);
    void WriteMinedBlockCelebration(string companyId, string minerId);
    void WriteApplicationStartup(string companyInfo, string apiInfo);
    void WriteException(Exception exception);
    void WriteDeserializationException(string json, JsonException jsonException);
}
