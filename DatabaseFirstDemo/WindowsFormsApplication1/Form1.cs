using DatabaseFirstDemo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            
            using (new TransactionScope())
            using (var context = new SchoolEntities())
            {
                await Task.Run(() => context.People.AddRange(Enumerable.Range(0, 10000).Select(i => new Student { FirstName = "Student" + i, EnrollmentDate = DateTime.Today })));
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            button1.Enabled = true;
        }

        private async Task Run()
        {
            await Task.Delay(5000);
        }
    }
}
