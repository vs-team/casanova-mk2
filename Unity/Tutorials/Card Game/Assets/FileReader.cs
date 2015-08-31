using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Casanova.Prelude;


namespace Assets
{
  struct FileReader
  {
    

    public static List<Tuple<string, string>> getWords()
    {
      var reader = new StreamReader(File.OpenRead(@Directory.GetCurrentDirectory() + "/Assets/Resources/words.txt"));
      List<Tuple<string, string>> listA = new List<Tuple<string, string>>();
      List<string> listB = new List<string>();
      while (!reader.EndOfStream)
      {
        var line = reader.ReadLine();
        var values = line.Split(';');
        listA.Add(new Tuple<string, string>(values[0], values[1]));
      }
      
      return listA;
      
    }
  

  }
}
                                                                                                                                                                                                                                                                                                                                              