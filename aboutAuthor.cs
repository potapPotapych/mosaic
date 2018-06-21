using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mosaic
{
    public partial class aboutAuthor : Form
    {
        
        public aboutAuthor()
        {
            InitializeComponent();
            textBox1.Text = Potapov.Pavel.firstName;
            textBox2.Text = Potapov.Pavel.lastName;
            textBox3.Text = Potapov.Pavel.age.ToString();
            textBox4.Text = Potapov.Pavel.status;
        }
        author Potapov = new author();
        
    }

    public partial class person
    {
        public int age;
        public string status, firstName, lastName;
        delegate int ageDel();

        public person(string fn, string ln, string st)
        {
            ageDel a = new ageDel(GetAge);
            age = a();
            status = st;
            firstName = fn;
            lastName = ln;
        }
        public person()
        {
        }

        int GetAge()
        {
            DateTime dateBirthDay = new DateTime(1998, 8, 30);
            DateTime dateNow = DateTime.Now;
            int year = dateNow.Year - dateBirthDay.Year;
            if (dateNow.Month < dateBirthDay.Month ||
                (dateNow.Month == dateBirthDay.Month && dateNow.Day < dateBirthDay.Day)) year--;
            return year;
        }


    }
    
    public partial class author : person
    {
        public person Pavel = new person("Павел", "Потапов", "Студент");
    }
}
    

