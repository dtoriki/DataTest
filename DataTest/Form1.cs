using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTest
{
    public partial class Form1 : Form
    {
        private int table_count;
        private List<string[]> data = new List<string[]>();
        public Form1()
        {
            InitializeComponent();
            Load += Loaded;
            Disposed += OnDispose;
        }

        private void OnDispose(object sender, EventArgs e)
        {
            Load -= Loaded;
            Disposed -= OnDispose;
        }

        private async void Loaded(object sender, EventArgs e)
        {
            LoadData1();
            LoadData2();
            Next();
            LoadKritTerm();
            await LoadDB();
            EngineConditionInDataGridFromEngine();
        }


        //кнопка сохранения:
        private void LoadSaveAll(object sender, EventArgs e)
        {
            Print("LoadSaveAll Starts");
            SaveData1();
            SaveData2();
            Next();
            SaveKritTerm();
            dataGridFromEngine.EndEdit();
            SaveDB();
            dataGridFromEngine.Rows.Clear();

            LoadDB().GetAwaiter().OnCompleted(() =>
            {
                EngineConditionInDataGridFromEngine();
            });

            Print("LoadSaveAll-OK");

        }



        public void LoadData1()
        {
            dataGridView1.Rows.Add("\n", "\n", "\n");
            StreamReader sr = new StreamReader(Path.Combine(Environment.CurrentDirectory, "Data", "dataGridView1.txt"), UTF8Encoding.Default);
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    dataGridView1[i, 0].Value = sr.ReadLine();
                }
                catch (Exception) { continue; }
            }
            sr.Close();
        }
        public void LoadData2()
        {
            dataGridView2.Rows.Add("\n", "\n");
            StreamReader sr;
            sr = new StreamReader(Path.Combine(Environment.CurrentDirectory, "Data", "dataGridView2.txt"), UTF8Encoding.Default);
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    dataGridView2[i, 0].Value = sr.ReadLine();
                }
                catch (Exception) { continue; }
            }
            sr.Close();
            Next();
        }
        public void SaveData1()
        {
            StreamWriter sw;
            //FileInfo txt = new FileInfo("dataGridView1.txt");
            sw = new StreamWriter("dataGridView1.txt", false, UTF8Encoding.Default, 10);
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    sw.WriteLine(dataGridView1[i, 0].Value.ToString());
                }
                catch (Exception) { sw.WriteLine("\n"); }
            }
            sw.Close();
        }
        public void SaveData2()
        {
            StreamWriter sw;
            //FileInfo txt = new FileInfo("dataGridView2.txt");
            sw = new StreamWriter("dataGridView2.txt", false, UTF8Encoding.Default, 10);
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    sw.WriteLine(dataGridView2[i, 0].Value.ToString());
                }
                catch (Exception) { sw.WriteLine("\n"); }
            }
            sw.Close();
        }
        public void SaveKritTerm()
        {
            StreamWriter sw;
            sw = new StreamWriter("KritTerm.txt", false, UTF8Encoding.Default, 10);
            try
            {
                sw.WriteLine(richTextBox5.Text);
            }
            catch (Exception) { sw.WriteLine("\n"); }
            try
            {
                sw.WriteLine(richTextBox6.Text);
            }
            catch (Exception) { sw.WriteLine("\n"); }
            try
            {
                sw.WriteLine(richTextBox7.Text);
            }
            catch (Exception) { sw.WriteLine("\n"); }
            //
            try
            {
                sw.WriteLine(richTextBox8.Text);
            }
            catch (Exception) { sw.WriteLine("\n"); }
            try
            {
                sw.WriteLine(richTextBox9.Text);
            }
            catch (Exception) { sw.WriteLine("\n"); }
            try
            {
                sw.WriteLine(richTextBox10.Text);
            }
            catch (Exception) { sw.WriteLine("\n"); }
            //
            try
            {
                sw.WriteLine(richTextBox11.Text);
            }
            catch (Exception) { sw.WriteLine("\n"); }
            try
            {
                sw.WriteLine(richTextBox12.Text);
            }
            catch (Exception) { sw.WriteLine("\n"); }
            try
            {
                sw.WriteLine(richTextBox13.Text);
            }
            catch (Exception) { sw.WriteLine("\n"); }
            //
            try
            {
                sw.WriteLine(richTextBox14.Text);
            }
            catch (Exception) { sw.WriteLine("\n"); }
            try
            {
                sw.WriteLine(richTextBox15.Text);
            }
            catch (Exception) { sw.WriteLine("\n"); }
            try
            {
                sw.WriteLine(richTextBox16.Text);
            }
            catch (Exception) { sw.WriteLine("\n"); }
            sw.Close();
        }
        public void LoadKritTerm()
        {
            StreamReader sr;
            try
            {
                sr = new StreamReader("KritTerm.txt", UTF8Encoding.Default);
            }
            catch (Exception)
            {
                StreamWriter sw = new StreamWriter("KritTerm.txt", false, UTF8Encoding.Default, 10);
                sw.Close();
                sr = new StreamReader("KritTerm.txt", UTF8Encoding.Default);
            }
            try
            {
                richTextBox5.Text = sr.ReadLine();
                richTextBox6.Text = sr.ReadLine();
                richTextBox7.Text = sr.ReadLine();

                richTextBox8.Text = sr.ReadLine();
                richTextBox9.Text = sr.ReadLine();
                richTextBox10.Text = sr.ReadLine();

                richTextBox11.Text = sr.ReadLine();
                richTextBox12.Text = sr.ReadLine();
                richTextBox13.Text = sr.ReadLine();

                richTextBox14.Text = sr.ReadLine();
                richTextBox15.Text = sr.ReadLine();
                richTextBox16.Text = sr.ReadLine();
            }
            catch (Exception e) { Console.WriteLine(e); }
            sr.Close();
        }

        public async Task LoadDB()
        {
            string connectString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Path.Combine(Environment.CurrentDirectory, "Database.mdf")};Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(connectString);
            Task openConnectionTask = myConnection.OpenAsync();

            Print("L_0.0__");
            Print("L_0.1__");
            table_count = 0;

            string query = "SELECT * FROM [Table] ORDER BY Id";
            await openConnectionTask;
            SqlDataReader reader = null;
            try
            {
                SqlCommand command = new SqlCommand(query, myConnection);
                reader = await command.ExecuteReaderAsync();
                data.Clear();
                while (await reader.ReadAsync())
                {
                    data.Add(new string[14]);
                    table_count++;
                    for (int i = 0; i < 14; i++)
                    {
                        data[data.Count - 1][i] = reader[i].ToString();
                    }
                }
            }
            finally
            {
                reader?.Close();
                myConnection.Close();
            }


            foreach (string[] s in data)
            {
                try
                {
                    lock (dataGridFromEngine)
                    {
                        dataGridFromEngine.Rows.Add(s);
                    }
                }
                catch (Exception e) { Print("!!!!!!!!!!!!!!!!!!!"); Print(e); Print("!!!!!!!!!!!!!!!!!!!"); }
            }
            Print("L_0.2___");

            Print("L_0.3___OK...");
        }

        public void SaveDB()
        {
            data.Clear();
            for (int q = 0; q < dataGridFromEngine.Rows.Count - 1; q++)
            {
                data.Add(new string[14]);
                for (int i = 1; i < 14; i++)
                {
                    if (dataGridFromEngine[i, q].Value != null)
                    {
                        data[q][i] = dataGridFromEngine[i, q].Value.ToString();
                    }
                    else
                    {
                        data[q][i] = "";
                    }
                }
            }

            string connectString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Master\source\repos\DataTest\DataTest\Database.mdf;Integrated Security=True";

            SqlConnection myConnection = new SqlConnection(connectString);

            SqlCommand command;
            try
            {
                myConnection.Open();

                int c = 0;

                command = new SqlCommand("DELETE FROM [dbo].[Table]", myConnection);
                command.ExecuteNonQuery();
                /*
                while (c < table_count)
                {
                    command = new SqlCommand($"UPDATE [dbo].[Table] SET [1]='{data[c][1]}',[2]='{data[c][2]}',[3]='{data[c][3]}',[4]='{data[c][4]}',[5]='{data[c][5]}',[6]='{data[c][6]}',[7]='{data[c][7]}',[8]='{data[c][8]}',[9]='{data[c][9]}',[10]='{data[c][10]}',[11]='{data[c][11]}',[12]='{data[c][12]}',[13]='{data[c][13]}' WHERE [Id]={c}", myConnection);
                    command.ExecuteNonQuery();
                    c++;
                }*/

                while (c < data.Count)
                {
                    command = new SqlCommand($"INSERT INTO [dbo].[Table] VALUES ({c},'{data[c][1]}','{data[c][2]}','{data[c][3]}','{data[c][4]}','{data[c][5]}','{data[c][6]}','{data[c][7]}','{data[c][8]}','{data[c][9]}','{data[c][10]}','{data[c][11]}','{data[c][12]}','{data[c][13]}')", myConnection);
                    command.ExecuteNonQuery();
                    c++;
                }

                //отладка

                myConnection.Close();
            }
            catch (Exception E) { Print(E); myConnection.Close(); }

        }
        public void Next()
        {
            try
            {
                DateTime date = DateTime.Parse(dataGridView2[0, 0].Value.ToString());
                date = date.AddDays(7);
                label3.Text = String.Format("{0:dd/MM/yyyy}", date);
            }
            catch (Exception) { label3.Text = "Проверьте, что дата указана в формате \"День/Месяц/Год\""; }
        }
        public void EngineConditionInDataGridFromEngine()
        {
            Print("L_1.0__");
            //if (dataGridFromEngine[6,0] != null) Print($"A\nA \nA НЕ ПУСТО! \n {dataGridFromEngine[6, 0].Value} \n A \n A\n A");

            List<double> kriterm = new List<double>();

            try
            {
                kriterm.Add(Double.Parse(richTextBox5.Text));
                kriterm.Add(Double.Parse(richTextBox6.Text));
                kriterm.Add(Double.Parse(richTextBox7.Text));

                kriterm.Add(Double.Parse(richTextBox8.Text));
                kriterm.Add(Double.Parse(richTextBox9.Text));
                kriterm.Add(Double.Parse(richTextBox10.Text));

                kriterm.Add(Double.Parse(richTextBox11.Text));
                kriterm.Add(Double.Parse(richTextBox12.Text));
                kriterm.Add(Double.Parse(richTextBox13.Text));

                kriterm.Add(Double.Parse(richTextBox14.Text));
                kriterm.Add(Double.Parse(richTextBox15.Text));
                kriterm.Add(Double.Parse(richTextBox16.Text));

                double temp;
                for (int c = 0; c < dataGridFromEngine.RowCount; c++)
                {

                    if (Double.TryParse(dataGridFromEngine[6, c].Value?.ToString(), out temp))
                    {
                        if (temp > kriterm[0] && temp <= kriterm[1])
                        {
                            dataGridFromEngine[7, c].Value = "norm";
                        }
                        else if (temp > kriterm[1] && temp <= kriterm[2])
                        {
                            dataGridFromEngine[7, c].Value = "predavar";
                        }
                        else if (temp > kriterm[2])
                        {
                            dataGridFromEngine[7, c].Value = "avar";
                        }
                    }

                    if (Double.TryParse(dataGridFromEngine[8, c].Value?.ToString(), out temp))
                    {
                        temp = Double.Parse(dataGridFromEngine[8, c].Value?.ToString());
                        if (temp > kriterm[3] && temp <= kriterm[4])
                        {
                            dataGridFromEngine[9, c].Value = "norm";
                        }
                        else if (temp > kriterm[4] && temp <= kriterm[5])
                        {
                            dataGridFromEngine[9, c].Value = "predavar";
                        }
                        else if (temp > kriterm[5])
                        {
                            dataGridFromEngine[9, c].Value = "avar";
                        }

                    }

                    if (Double.TryParse(dataGridFromEngine[10, c].Value?.ToString(), out temp))
                    {
                        if (temp > kriterm[6] && temp <= kriterm[7])
                        {
                            dataGridFromEngine[11, c].Value = "norm";
                        }
                        else if (temp > kriterm[7] && temp <= kriterm[8])
                        {
                            dataGridFromEngine[11, c].Value = "predavar";
                        }
                        else if (temp > kriterm[8])
                        {
                            dataGridFromEngine[11, c].Value = "avar";
                        }
                    }

                    if (Double.TryParse(dataGridFromEngine[12, c].Value?.ToString(), out temp))
                    {
                        if (temp > kriterm[9] && temp <= kriterm[10])
                        {
                            dataGridFromEngine[13, c].Value = "norm";
                        }
                        else if (temp > kriterm[10] && temp <= kriterm[11])
                        {
                            dataGridFromEngine[13, c].Value = "predavar";
                        }
                        else if (temp > kriterm[11])
                        {
                            dataGridFromEngine[13, c].Value = "avar";
                        }
                    }

                }
            }
            catch (Exception) { }
            Print("L_1.1__ALL...");
        }

        private void Print<T>(T a)
        { Console.WriteLine(a); }

        private void dataGridView3_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dataGridFromEngine.RowCount > 1)
            {
                dataGridFromEngine[0, dataGridFromEngine.RowCount - 1].Value = Int32.Parse(dataGridFromEngine[0, dataGridFromEngine.RowCount - 2].Value.ToString()) + 1;
                if (dataGridFromEngine.RowCount > 2)
                {
                    dataGridFromEngine[2, dataGridFromEngine.RowCount - 1].Value = dataGridFromEngine[2, dataGridFromEngine.RowCount - 3].Value;
                    dataGridFromEngine[3, dataGridFromEngine.RowCount - 1].Value = dataGridFromEngine[3, dataGridFromEngine.RowCount - 3].Value;
                    dataGridFromEngine[4, dataGridFromEngine.RowCount - 1].Value = dataGridFromEngine[4, dataGridFromEngine.RowCount - 3].Value;
                    dataGridFromEngine[5, dataGridFromEngine.RowCount - 1].Value = dataGridFromEngine[5, dataGridFromEngine.RowCount - 3].Value;

                    dataGridFromEngine[1, dataGridFromEngine.RowCount - 1].Value = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
                }
                if (dataGridFromEngine.RowCount == 2)
                {
                    dataGridFromEngine[2, dataGridFromEngine.RowCount - 1].Value = dataGridFromEngine[2, dataGridFromEngine.RowCount - 2].Value;
                    dataGridFromEngine[3, dataGridFromEngine.RowCount - 1].Value = dataGridFromEngine[3, dataGridFromEngine.RowCount - 2].Value;
                    dataGridFromEngine[4, dataGridFromEngine.RowCount - 1].Value = dataGridFromEngine[4, dataGridFromEngine.RowCount - 2].Value;
                    dataGridFromEngine[5, dataGridFromEngine.RowCount - 1].Value = dataGridFromEngine[5, dataGridFromEngine.RowCount - 2].Value;

                    dataGridFromEngine[1, dataGridFromEngine.RowCount - 1].Value = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
                }
            }
        }
    }
}
