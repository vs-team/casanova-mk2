using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;


public class AssetsImport : AssetPostprocessor
{
  static AssetsImport Singleton = new AssetsImport();

    [MenuItem ("Tools/Clear Console %#c")] // CMD + SHIFT + C
     static void ClearConsole () {
         // This simply does "LogEntries.Clear()" the long way:
         var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
         var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
         clearMethod.Invoke(null,null);
     }


  static void SystemCall(string cmd, string args, string cnv_file)
  {
        ClearConsole();
				UnityEngine.Debug.Log ("Beginning Casanova compilation");
				try {

						Process myProcess = new Process ();
						myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						myProcess.StartInfo.CreateNoWindow = true;
						myProcess.StartInfo.FileName = cmd;
						myProcess.StartInfo.Arguments = args;
						myProcess.EnableRaisingEvents = true;
						myProcess.StartInfo.UseShellExecute = false;
						myProcess.StartInfo.RedirectStandardOutput = true;
						myProcess.StartInfo.RedirectStandardError = true;
      
						var process = Process.Start (myProcess.StartInfo);
						process.WaitForExit ();
	  
// string str;
//    while ((str = process.StandardOutput.ReadLine()) != null)
//    {
//      UnityEngine.Debug.Log(str);
//    }
//    isDone1 = true;
//  });
						bool hasErrors = false;
						string error = "";
						while ((error = process.StandardError.ReadLine()) != null) {
								hasErrors = true;
								//UnityEngine.Debug.Log("Error!!!");
								//Singleton.LogError("Error!!!");
								if (error.Contains ("error"))
										Singleton.LogError (error);
								else
                  Singleton.LogError (error);
								break;
						}
						if (!hasErrors)
								UnityEngine.Debug.Log ("Finished Casanova compilation");
						else{							
								if(File.Exists(cnv_file))
					File.WriteAllText(cnv_file, "error");
			}

				} catch (System.Exception e) {
						UnityEngine.Debug.Log (e);
				}
	}

  static void CasanovaCompile(string fileName)
  {
    var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);



    var assets_folder = Path.Combine(Directory.GetCurrentDirectory(), "Assets");
    var cnv_file = Path.Combine(assets_folder, fileNameWithoutExtension + ".cnv");
    var cs_file = Path.Combine(assets_folder, fileNameWithoutExtension + ".cs");
    var compiler_path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\CasanovaCompiler\CasanovaCompiler.exe");


    string args = "\"" + cnv_file + "\" " + "\"" + cs_file + "\"";
    SystemCall(compiler_path, args, cs_file);
  }

  static IEnumerable<string> ProjectChildren(string project)
  {
    foreach (string line in File.ReadAllLines(project))
    {
      if (line.StartsWith("import"))
      {
        string file = line.Replace("import ", "").Trim(new[] { ' ', '\"' });
        yield return System.IO.Path.GetFullPath(System.IO.Path.Combine("Assets", file));
      }
    }
  }

  static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
  {
    Dictionary<string, bool> projectsToBuild = new Dictionary<string, bool>();
    foreach (string asset in importedAssets)
    {
      if (asset.EndsWith(".cnv"))
      {
        projectsToBuild.Add(asset, true);
      }
      //if (asset.EndsWith(".cnv"))
      //{
      //  foreach (var file in System.IO.Directory.GetFiles("Assets"))
      //  {
      //    if (file.EndsWith(".cnvproj"))
      //    {
      //      //UnityEngine.Debug.Log("Looking for project file in " + file);
      //      if (ProjectChildren(file).Contains(asset))
      //      {
      //        //UnityEngine.Debug.Log("Found!");
      //        if (!projectsToBuild.ContainsKey(System.IO.Path.GetFullPath(file)))
      //          projectsToBuild.Add(System.IO.Path.GetFullPath(file), true);
      //      }
      //    }
      //  }
      //}

      //if (asset.EndsWith(".cnvproj"))
      //{
      //  if (!projectsToBuild.ContainsKey(System.IO.Path.GetFullPath(asset)))
      //    projectsToBuild.Add(System.IO.Path.GetFullPath(asset), true);
      //}
    }

    //UnityEngine.Debug.Log("Compiling..");
    foreach (var project in projectsToBuild)
    {
      CasanovaCompile(System.IO.Path.GetFileNameWithoutExtension(project.Key));
    }

    foreach (var file in System.IO.Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Assets", "*.cs"))
      System.IO.File.AppendAllText(file, " ");
    
    //UnityEngine.Debug.Log("Exit..");

  }
}
