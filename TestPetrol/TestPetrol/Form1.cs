using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace TestPetrol
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-493DFJA\SQLEXPRESS;Initial Catalog=TestBenzin;Integrated Security=True");
        private void Form1_Load(object sender, EventArgs e)
        {
            KASA();
            SqlDataAdapter dt = new SqlDataAdapter("SELECT PETROLTUR AS'Yakıt Türü', ALISFIYAT as'Alış Fiyatı',SATISFIYAT as'Satış Fiyat' FROM TBLBENZİN", con);
            DataTable da = new DataTable();
            dt.Fill(da);
            dataGridView1.DataSource = da;
            for (int i = 0; i < dataGridView1.Rows.Count-1 ; i++)
            {
                DataGridViewCellStyle renk = new DataGridViewCellStyle();

                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[2].Value) == true)
                {
                    renk.BackColor = Color.DimGray;
                  

                }


                dataGridView1.Rows[i].DefaultCellStyle = renk;

            }


            con.Open();
            SqlCommand komut = new SqlCommand("select * from TBLBENZİN", con);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox1BENZİNTÜRÜ.Items.Add(dr[1]);
                

            }
            con.Close();
        }







        double alısfiyat, satısfiyat;


        bool durum;
        void fiyatSATARKEN()
        {
            
            con.Open()
   ;            SqlCommand kmt = new SqlCommand("select * from TBLBENZİN where PETROLTUR=@P1", con);
            kmt.Parameters.AddWithValue("@p1", comboBox1BENZİNTÜRÜ.Text);
            SqlDataReader dr = kmt.ExecuteReader();
            while (dr.Read())
            {
                alısfiyat = Convert.ToDouble(dr[2]);
                satısfiyat = Convert.ToDouble(dr[3]);
                progressBar1.Value = int.Parse(dr[4].ToString());
                labelSTOK.Text = dr[4].ToString();
                if (int.Parse(labelSTOK.Text) >= 10000)
                {
                    durum = false;
                }
                else
                {
                    durum = true;
                }
               

            }
            con.Close();
        }
        void KASA()
        {
            con.Open();
            SqlCommand com = new SqlCommand("select * from TBLMİKTAR", con);
            SqlDataReader dr1 = com.ExecuteReader();
            while (dr1.Read())
            {
                labelKASA.Text = dr1[0].ToString();
            }
            con.Close();



        }
        

        private void button1SATIS_Click(object sender, EventArgs e)
        {
            if(textBox1LİTRE.Text != "0") { 
               
            con.Open();
            SqlCommand komutt = new SqlCommand("insert into TBLHAREKETPETROL (PLAKA,BENZINTURU,LITRE,FIYAT) values (@p1,@p2,@p3,@p4)", con);
            komutt.Parameters.AddWithValue("@p1", textBoxPLAKA.Text);
            komutt.Parameters.AddWithValue("@p2", comboBox1BENZİNTÜRÜ.Text);
            komutt.Parameters.AddWithValue("@p3", textBox1LİTRE.Text);
            komutt.Parameters.AddWithValue("@p4", decimal.Parse(textBox2TUTAR.Text));
            komutt.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("SATIS YAPILDI","BİLGİ",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            KASA();
            fiyatSATARKEN();
        }

        private void comboBox1BENZİNTÜRÜ_SelectedIndexChanged(object sender, EventArgs e)
        {
            fiyatSATARKEN();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
                if (durum ==false)
                {
                    MessageBox.Show("depo full alım yapamayız");
                }
                if(durum==true)
                {
                    con.Open();
                    SqlCommand komutt = new SqlCommand("insert into TBLBENZİNALIS (BENZINTURU,LITRE,FIYAT) values (@p1,@p2,@p3)", con);
                    komutt.Parameters.AddWithValue("@p1", comboBox1BENZİNTÜRÜ.Text);
                    komutt.Parameters.AddWithValue("@p2", Convert.ToInt32(textBox2LİTREAL.Text));
                    komutt.Parameters.AddWithValue("@p3", decimal.Parse(textBox1TUTARAL.Text));
                    komutt.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("PETROL ALINDI ", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            KASA();
            fiyatSATARKEN();
               
            
        }

        private void textBox2LİTREAL_TextChanged(object sender, EventArgs e)
        {
            fiyatSATARKEN();
            try
            {
                double litre;
                litre = Convert.ToDouble(textBox2LİTREAL.Text);
                textBox1TUTARAL.Text = (litre * alısfiyat).ToString();
            }
            catch (Exception)
            {


            }
        }

         private void textBox1LİTRE_TextChanged(object sender, EventArgs e)
        {
            fiyatSATARKEN();
            try
            {
                double litre;
                litre = Convert.ToDouble(textBox1LİTRE.Text);
                textBox2TUTAR.Text = (litre * satısfiyat).ToString();
            }
            catch (Exception)
            {

                
            }
      
        }
    }
}
