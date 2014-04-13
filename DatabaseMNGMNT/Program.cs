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
            string fileName = "localDB.sdf";
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
                cmd.CommandText = "CREATE TABLE FakultetiSlike( Identifier int, slika1 ntext, slika2 ntext, slika3 ntext, slika4 ntext, slika5 ntext) ";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE DomoviSlike( Identifier int, slika1 ntext, slika2 ntext, slika3 ntext, slika4 ntext, slika5 ntext) ";
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
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
                #region sql
                try
                {
                    string tempFakultetiString = "INSERT INTO Fakulteti (identifier, Naziv, Telefon, Sajt,  Email, Adresa, Dekan, Text, Smerovi, Zvanja, Uslovi_upisa, Logo) VALUES ('"
                        + temp1 + "', '"
                        + item["Naziv"] + "', '"
                        + item["Telefon"] + "', '"
                        + item["Sajt"] + "', '"
                        + item["Email"] + "', '"
                        + item["Adresa"] + "', '"
                        + item["Dekan"] + "', '"
                        + item["Tekst"] + "', '"
                        + item["Smerovi"] + "', '"
                        + item["Zvanja"] + "', '"
                        + item["Uslovi_upisa"]  + "', '"
                        + item["Logo"] + "' );";
                    Console.WriteLine(tempFakultetiString);
                    SqlCeCommand cmd2 = new SqlCeCommand(tempFakultetiString,cn);
                    Console.WriteLine("{0} {1}", item["Naziv"].ToString(), item["Adresa"]);
                    cmd2.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                int j = 0;
                string commandMock = "INSERT INTO FakultetiSlike( Identifier, slika1, slika2, slika3, slika4, slika5) VALUES ('" + temp1 + "', '";

                foreach (var slika in item["Images"])
                {
                    if (slika.ToString() != null || !slika.Equals(""))
                        commandMock = commandMock + slika.ToString() + "', '";
                    else
                        commandMock = commandMock + "'' " + "', '";
                    j++;
                }
                commandMock = commandMock.Substring(0, commandMock.Length - 3);
                switch (j)
                {
                    case 1:{
                            commandMock = commandMock + ", '', '', '', ''";
                            break;
                        }
                    case 2:{
                            commandMock = commandMock + ", '', '', ''";
                            break;
                        }
                    case 3:{
                            commandMock = commandMock + ", '', ''";
                            break;
                        }
                    case 4:{
                            commandMock = commandMock + ", ''";
                            break;
                        }
                    case 5:{
                            commandMock = commandMock + "'";
                            break;
                        }
                }
                commandMock = commandMock + " );";
                Console.WriteLine(commandMock);
                try
                {
                    SqlCeCommand cmd = new SqlCeCommand(commandMock, cn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
               
                #endregion
                temp1++;
            }

            int temp2 = 0;

            foreach (var domValue in o["domovi"])
            {
                #region sql
                try
                {
                    string tempDomoviString = "INSERT INTO Domovi (identifier, Naziv, Telefon, Sajt, Adresa, Tekst, Prevoz) VALUES ('"
                        + temp2 + "', '"
                        + domValue["Naziv"] + "', '"
                        + domValue["Telefon"] + "', '"
                        + domValue["Sajt"] + "', '"
                        + domValue["Adresa"] + "', '"
                        + domValue["Opis"] + "', '"
                        + domValue["Prevoz"]+  "' );";
                    Console.WriteLine(tempDomoviString);
                    SqlCeCommand cmd2 = new SqlCeCommand(tempDomoviString, cn);
                    Console.WriteLine("{0} {1}", domValue["Naziv"].ToString(), domValue["Adresa"]);
                    cmd2.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                int j = 0;
                string commandMock = "INSERT INTO DomoviSlike( Identifier, slika1, slika2, slika3, slika4, slika5) VALUES ('" + temp2 + "', '";

                foreach (var slika in domValue["Images"])
                {
                    if (slika.ToString() != null || !slika.Equals(""))
                        commandMock = commandMock + slika.ToString() + "', '";
                    else
                        commandMock = commandMock + "'' " + "', '";
                    j++;
                }
                commandMock = commandMock.Substring(0, commandMock.Length - 3);
                switch (j)
                {
                    case 1:
                        {
                            commandMock = commandMock + ", '', '', '', ''";
                            break;
                        }
                    case 2:
                        {
                            commandMock = commandMock + ", '', '', ''";
                            break;
                        }
                    case 3:
                        {
                            commandMock = commandMock + ", '', ''";
                            break;
                        }
                    case 4:
                        {
                            commandMock = commandMock + ", ''";
                            break;
                        }
                    case 5:
                        {
                            commandMock = commandMock + "'";
                            break;
                        }
                }
                commandMock = commandMock + " );";
                Console.WriteLine(commandMock);
                try
                {
                    SqlCeCommand cmd = new SqlCeCommand(commandMock, cn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                #endregion
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


 

    

