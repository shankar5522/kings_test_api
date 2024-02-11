//Add Dependency - RestSharp
//dotnet add package RestSharp --version 110.2.0

//Output:
//1) Total Monarch Count is 57
//The input string '' was not in a correct format.
//2) Longest monarch Victoria ruled for the 64 years.
//3) Longest house ruled is House of Hanover
//4) Most Common First Name is Edward. (11 times Occures)

using Newtonsoft.Json;
using RestSharp;

namespace ModelData{
    public class Program{
        static void Main(String[] args){
            string uri = "https://gist.githubusercontent.com/christianpanton/10d65ccef9f29de3acd49d97ed423736/raw/b09563bc0c4b318132c7a738e679d4f984ef0048/";
            var client = new RestClient(uri);
            var request = new RestRequest("kings");
            var respone = client.Execute(request);
            var myJsonData = JsonConvert.DeserializeObject<List<jsonData>>(respone.Content);
            
            Program program = new Program();
            program.getmonarchsCount(myJsonData);
            int indexRuledMaxYears = program.getMonarchRuledLongest(myJsonData);
            program.getHouseRuledLongest(indexRuledMaxYears, myJsonData);
            program.getMostCommonFirstName(myJsonData);
        }

        public void getmonarchsCount(List<jsonData> list){
            Console.WriteLine("1) Total Monarch Count is " + list.Count);

        }

        public int getMonarchRuledLongest(List<jsonData> list){
            List<int> longestYears = new List<int>();
            int length;
            foreach (var item in list){
                string[] str = (item.yrs).Split("-");
                try{ 
                    if (str.Length == 2){
                        longestYears.Add(Convert.ToInt32(str[1]) - Convert.ToInt32(str[0]));
                    }else{
                        longestYears.Add(Convert.ToInt32(str[0]) - Convert.ToInt32(str[0]));
                    }
                }
                catch (FormatException e){
                    Console.WriteLine(e.Message);
                }
            }
            //Console.WriteLine("Difference in Years for each monarch: " + string.Join(",", longestYears));
            int maxYears = longestYears.Max();
            //Console.WriteLine("Max Years ruled is " + maxYears);
            int index = longestYears.IndexOf(maxYears);
            Console.WriteLine("2) Longest monarch "+ list[index].nm + " ruled for the "+ maxYears + " years.");
            return index;
        }

        public void getHouseRuledLongest(int index, List<jsonData> list){
            Console.WriteLine("3) Longest house ruled is " + list[index].hse);
        }

        public void getMostCommonFirstName(List<jsonData> list){
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (var item in list){
                string[] str = (item.nm).Split(" ");
                var fName = str[0];
                //Console.WriteLine(fName);
                if(dict.ContainsKey(fName)){
                    dict[fName] = dict[fName] + 1;
	            }
	            else{
		           dict[fName] = 1;
	            }
            }

            var max = 0;
            var name = "";
            foreach (KeyValuePair<string, int> item in dict){
      			//Console.WriteLine(item.Key+" => " + item.Value.ToString());
                if(max <= item.Value){
                    max = item.Value;
                    name = item.Key;
                }           
   			}
            Console.WriteLine("4) Most Common First Name is " + name + ". (" + max + " times Occures)");
        }
    } 

    [Serializable]
    public class jsonData
    {
        public int? id { get; set; }
        public string? nm { get; set; }
        public string? cty { get; set; }
        public string? hse { get; set; }
        public string? yrs { get; set; }
    }  
}
