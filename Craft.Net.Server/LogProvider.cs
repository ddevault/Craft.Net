using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server
{
   public static class LogProvider
   {
      static LogProvider()
      {
         logProviders = new List<ILogProvider>();
      }

      private static List<ILogProvider> logProviders { get; set; }

      public static void RegisterProvider(ILogProvider logProvider)
      {
         logProviders.Add(logProvider);
      }

      public static void Log(string text)
      {
         Log(text, LogImportance.High);
      }

      public static void Log(string text, LogImportance importance)
      {
         foreach (ILogProvider provider in logProviders)
            provider.Log(text, importance);
         }
      }
}