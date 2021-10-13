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
namespace Laba1
{
    public partial class Form1 : Form
    {
        private static int count_columns =0;
        private static int count_rows = 0;
        private static int size = 0;
        private static int location_x = 10;
        private static int location_y = 10;
        private static int count_square = 0;
        private static bool[,] bool_mass;
        private static string russian_alphavite = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ `~!@#$%^&*()_+-=/\\|.,<>?:;\"'[]{}№"; 
        public Form1()
        {
            InitializeComponent();
        }

        private void ЗагрузитьТекстДляШифрованияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "txt files(*.txt)| *.txt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string text_for_encrypt = File.ReadAllText(ofd.FileName, Encoding.Default);
                    bool success = true;
                    for (int i = 0; i < text_for_encrypt.Length; i++)
                    {
                        if (!russian_alphavite.Contains(text_for_encrypt[i]))
                        {
                            MessageBox.Show($"{text_for_encrypt[i]}");
                            success = false;
                            break;
                        }
                        
                    }
                    if (success)
                    {
                        count_square = 0;
                        for (int row = 0; row < count_rows; row++)
                        {
                            for (int column = 0; column < count_columns; column++)
                            {
                                bool_mass[row, column] = false;
                            }
                        }
                        richTextBox1.Text = text_for_encrypt;
                        richTextBox1.Visible = true;
                        groupBox1.Visible = true;
                        groupBox2.Visible = false;
                        button2.Visible = false;
                        button3.Visible = false;
                        textBox1.Text = String.Empty;
                        textBox2.Text = String.Empty;
                        panel1.Controls.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Программа работает только с русскими символами и знаками пунктуации. Повторите загрузку файла!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != String.Empty && textBox2.Text != String.Empty)
                {
                    count_square = 0;
                   
                    count_columns = Convert.ToInt32(textBox1.Text);
                    count_rows = Convert.ToInt32(textBox2.Text);
                    size = count_columns * count_rows;
                    if (size > richTextBox1.Text.Length)
                    {
                        location_x = 10;
                        location_y = 10;
                        panel1.Controls.Clear();
                        bool_mass = new bool[count_rows,count_columns];
                        for (int row = 0; row < count_rows; row++)
                        {
                            for (int column = 0; column < count_columns; column++)
                            {
                                bool_mass[row, column] = false;
                            }
                        }
                        for (int row = 0; row < count_rows; row++)
                        {
                            for (int column = 0; column < count_columns; column++)
                            {
                                Label picture = new Label();
                                picture.BorderStyle = BorderStyle.FixedSingle;
                                picture.Size = new Size(50, 50);
                                picture.Location = new Point(location_x, location_y);
                                picture.Name = $"{row}:{column}";
                                picture.Click += Picture_Click;
                                picture.Text = "-";
                                picture.Font = new Font("Times New Roman", 14f);
                                picture.TextAlign = ContentAlignment.MiddleCenter;
                                location_x += picture.Size.Width + 10;
                                panel1.Controls.Add(picture);
                            }
                            location_x = 10;
                            location_y += 50+10;
                        }
                        groupBox2.Visible = true;
                        button2.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Размер решетки не может быть меньше, чем количество символов в шифруемом тексте!");
                    }
                }
                else
                {
                    MessageBox.Show("Введите размер решетки Кардано");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Picture_Click(object sender, EventArgs e)
        {
            Label picture = (Label)sender;
            bool flag = true;
            int index_of_separator = picture.Name.IndexOf(':');
            int x_mass = Convert.ToInt32(picture.Name.Substring(0, index_of_separator));
            int y_mass = Convert.ToInt32(picture.Name.Substring(index_of_separator + 1, picture.Name.Length - index_of_separator - 1));
            if (picture.Text == "+")
            {
                picture.Text = "-";
                flag = false;
                count_square--;
                bool_mass[x_mass, y_mass] = false;
                button2.Visible = false;
            }
            if (flag)
            {
                if (picture.Text == "-")
                {
                    if (count_square != richTextBox1.Text.Length)
                    {
                        picture.Text = "+";
                        count_square++;
                        bool_mass[x_mass, y_mass] = true;
                        if (count_square != richTextBox1.Text.Length)
                        {
                            button2.Visible = false;
                        }
                        else
                        {
                            button2.Visible = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Количество выбранных квадратов равно длине текста!");

                    }
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            int cursorPosition = textBox1.SelectionStart;
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            int cursorPosition = textBox2.SelectionStart;
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            int index_symbol = 0;
            try
            {
                Random rand = new Random();
                for (int i = 0; i < panel1.Controls.Count; i++)
                {
                    Label label = (Label)panel1.Controls[i];
                    int index_of_separator = label.Name.IndexOf(':');
                    int x_mass = Convert.ToInt32(label.Name.Substring(0, index_of_separator));
                    int y_mass = Convert.ToInt32(label.Name.Substring(index_of_separator + 1, label.Name.Length - index_of_separator - 1));
                    if (bool_mass[x_mass,y_mass] == true && index_symbol < richTextBox1.Text.Length)
                    {
                        label.Text = richTextBox1.Text[index_symbol].ToString();
                        index_symbol++;
                    }
                    if(bool_mass[x_mass, y_mass] == false)
                    {
                        label.Text = ((char)rand.Next(0x0410, 0x44F)).ToString();
                    }
                }
                richTextBox1.Text = String.Empty;
                richTextBox1.Visible = false;
                button2.Visible = false;
                button3.Visible = true;
                groupBox1.Visible = false;
                groupBox2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < panel1.Controls.Count; i++)
            {
                Label label = (Label)panel1.Controls[i];
                int index_of_separator = label.Name.IndexOf(':');
                int x_mass = Convert.ToInt32(label.Name.Substring(0, index_of_separator));
                int y_mass = Convert.ToInt32(label.Name.Substring(index_of_separator + 1, label.Name.Length - index_of_separator - 1));
                if (bool_mass[x_mass, y_mass])
                {
                    label.BackColor = Color.Aqua;
                }
                else
                {
                    label.Text = String.Empty;
                }
            }
            button3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                count_square = 0;
                int count = 0;
                for (int row = 0; row < count_rows; row++)
                {
                    for (int column = 0; column < count_columns; column++)
                    {
                        bool_mass[row, column] = false;
                    }
                }
                Random rand = new Random();
                while (count != richTextBox1.Text.Length)
                {
                    int index_column = rand.Next(0, count_columns);
                    int index_row = rand.Next(0, count_rows);
                    if (!bool_mass[index_row, index_column])
                    {
                        bool_mass[index_row, index_column] = true;
                        count++;
                    }
                }
                int index = 0;
                for (int row = 0; row < count_rows; row++)
                {
                    for (int column = 0; column < count_columns; column++)
                    {
                        if (bool_mass[row, column])
                        {
                            panel1.Controls[index].Text = "+";
                            count_square++;
                        }
                        else
                        {
                            panel1.Controls[index].Text = "-";
                        }
                        index++;
                    }
                }
                if (count_square == richTextBox1.Text.Length)
                {
                    button2.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
