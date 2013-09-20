using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Data;
using System.Data.SqlClient;

namespace TestCrawler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            string url = @"http://www.adondevivir.com/propiedades/departamento_alquiler_lima_lima.html";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse res=(HttpWebResponse) req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string textoHTML = sr.ReadToEnd();
            sr.Close();
            res.Close();
            GrabarBD(extraerLinks(textoHTML));
        }
        List<string> extraerLinks(string pagina)
        {
             List<string> links=new List<string>();
            while (true)
            {
                int partida= pagina.IndexOf("<a href=\"");
                if (partida != -1)
                {
                    partida += 9;
                    pagina = pagina.Substring(partida, pagina.Length - partida - 1);
                    int final = pagina.IndexOf("\"");
                    string link = pagina.Substring(0, final);
                    links.Add(link);
                    pagina = pagina.Substring(final, pagina.Length - final - 1);
                }
                else
                {
                    break;
                }
                
            }
            return links;          
        }

        void GrabarBD(List<string> links)
        {
            SqlConnection cnn=new SqlConnection("Server=.;Initial Catalog=Base;trusted_connection=yes");
            SqlCommand cmd=new SqlCommand("insData",cnn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter par = cmd.Parameters.Add("@URL", System.Data.SqlDbType.VarChar, 400);
            par.Direction = System.Data.ParameterDirection.Input;
            foreach (var url in links)
            {
                par.Value = url;
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            MessageBox.Show("ya ta");

        }

        
    }
}
