using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;




namespace DatabaseMNGMNT
{
    class Program
    {
        static string json;

        public Program()
        {
            createDB();
        }


        public void createDB()
        {

            string connectionString;
            string fileName = "localDB.mdf";
            string password = "";


            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            connectionString = string.Format("Datasource=\"{0}\"; Password='{1}'", fileName, password);

            SqlCeEngine en = new SqlCeEngine(connectionString);
            en.CreateDatabase();

            SqlCeConnection cn = null;
            try {
                cn = new SqlCeConnection(connectionString);
                cn.Open();
                SqlCeCommand cmd = cn.CreateCommand();
                cmd.CommandText = "CREATE TABLE Fakulteti( identifier int, Naziv ntext, Telefon ntext, Sajt ntext,  Email ntext, Adresa ntext, Dekan ntext, Text ntext, Smerovi ntext, Zvanja ntext, Uslovi_upisa ntext, Logo ntext)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE Domovi( identifier int, Naziv ntext, Telefon ntext, Sajt ntext, Adresa ntext, Tekst ntext, Prevoz ntext)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE FakultetiSlike( Identifier int, slika1 ntext, slika2 ntext, slika3 ntext, slika4 ntext) ";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE DomoviSlike( Identifier int, slika1 ntext, slika2 ntext, slika3 ntext, slika4 ntext, slika5 ntext) ";
                cmd.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("Bad bad");
            }

            openJSON();
                   
        }


        public async void openJSON()
        {
            string connectionString;
            string fileName = "localDB.sdf";
            string password = "";
            connectionString = string.Format("Datasource=\"{0}\"; Password='{1}'", fileName, password);

            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();

            try
            {
                HttpClient client = new HttpClient();
                json = await client.GetStringAsync("https://raw.github.com/markostakic/31/master/aaa.json");
            }
            catch (Exception e)
            {

                StreamReader sr = new StreamReader("ms-appx:///ViewModel/aaa.txt");
                json = sr.ReadToEnd();
            }

            JObject o = JObject.Parse(json);
            int temp1 = 0;
            

            foreach (var item in o["fakulteti"])
            {
                int j = 0;
                foreach (var slika in item["Images"])
                {
                    string commandMock = "";
                    if (slika[0] != null || !slika[0].Equals(""))
                        commandMock = slika[0].ToString() + "', '";
                    if (slika[1] != null || !slika[1].Equals(""))
                        commandMock=commandMock+ slika[1].ToString()+ "', '";
                    if (slika[2] != null || !slika[2].Equals(""))
                        commandMock = commandMock + slika[2].ToString() + "', '";
                    if (slika[3] != null || !slika[3].Equals(""))
                        commandMock = commandMock + slika[3].ToString() + "', '";
                    if (slika[4] != null || !slika[4].Equals(""))
                        commandMock = commandMock + slika[4].ToString();
                    j++;
                }
              #region sql
                try
                {
                    SqlCeCommand cmd = new SqlCeCommand("INSERT INTO Fakulteti (identifier, Naziv, Telefon, Sajt,  Email, Adresa, Dekan, Text, Smerovi, Zvanja, Uslovi_upisa, Logo) VALUES ('"
                        + temp1 + "', '"
                        + item["Naziv"].ToString() + "', '"
                        + item["Telefon"].ToString() + "', '"
                        + item["Sajt"].ToString() + "', '"
                        + item["Email"].ToString() + "', '"
                        + item["Adresa"].ToString() + "', '"
                        + item["Dekan"].ToString() + "', '"
                        + item["Tekst"].ToString() + "', '"
                        + item["Smerovi"].ToString() + "', '"
                        + item["Zvanja"].ToString() + "', '"
                        + item["Uslovi_upisa"].ToString()  + "', '"
                        + item["Logo"].ToString() + "' );",cn);
                    Console.WriteLine("{0} {1}", item["Naziv"].ToString(), item["Adresa"]);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                #endregion
                int j=0;
              //  foreach (var slika in o["Slike"])
                {
                   
             //   j++;
                }
                temp1++;
            }

            int temp2 = 0;

            foreach (var domValue in o["domovi"])
            {
               
                temp2++;
            }
            cn.Close();
        }
}
        public class asfd
        {
            static void Main(String[] args)
            {
                Program p = new Program();
                Console.ReadKey();
            }
        }

       

    }


 

    

