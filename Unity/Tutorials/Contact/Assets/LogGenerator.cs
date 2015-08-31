using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LogGenerator
{
  public string Log { get; set; }

  public bool WritingLog { get; set; }

  public static void Write(LogGenerator log, string message)
  {
    log.Log = log.Log + message + "\n";
  }

  public LogGenerator()
  {
    Log = "Player performing action,Action type,Player targeted by action,Parameters,Time (h/m/s/ms)\n";
  }

  public static void SaveLog(LogGenerator log, string path)
  {
    string directory = Path.GetDirectoryName(path);
    string exeDirectory = Application.dataPath;
    if (!Directory.Exists(directory) && directory != "")
      Directory.CreateDirectory(directory);
    path = Path.Combine(exeDirectory, path);
    File.WriteAllText(path, log.Log);
  }

  public static LogGenerator CreateLog()
  {
    return new LogGenerator();
  }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        