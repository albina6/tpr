using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TPR2
{
    public enum EventLogic
    {
        Or,
        And
    }


    public partial class TreeMaker : Form
    {
        int[] _brims = new int[]
        {
            0,0,
            450,
            150,
            130,
            5
        };

        Dictionary<Button, Node> _sits;
        Dictionary<Button, Node> _sits2;
        Dictionary<TextBox, Node> _sits3;
        Dictionary<TextBox, Node> _sits4;
        Node _root;
        //int number = 1;

        public TreeMaker()
        {
            InitializeComponent();
           // button1.BackColor = Color.YellowGreen;
            _sits = new Dictionary<Button, Node>();// список детей узла(для каждого)
            _sits2 = new Dictionary<Button, Node>();// кнопка добавления детей (+), не лог операция 
            _sits3 = new Dictionary<TextBox, Node>();//название
            _sits4 = new Dictionary<TextBox, Node>();//вероятность
            _root = new Node(0,"main", 2);
           // textBox3.Text ="X"+ number.ToString();
            _sits3.Add(textBox3, _root);
            _sits2.Add(button1, _root);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            EventType et = new EventType();
            et.ShowDialog(); 
            EventLogic s = et.S;
            Button mainButton = sender as Button;
            Button b = new Button
            {
                Top = mainButton.Top + 40,
                Left = mainButton.Left + 30
            };
            
            try
            {
                _sits2.Add(mainButton, _root);
            }
            catch { }

            _sits2[mainButton].Sel = s;
            _sits.Add(b, _sits2[mainButton]);
            b.Text = s.ToString();
            b.Click += MakeNode;
            this.Controls.Add(b);
        }
        
        private void MakeNode(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Node newsit = new Node(0, "", _sits[b].Depth + 1);
            TextBox name = new TextBox
            {
                Top = b.Top + 40,
                Left = b.Left + (_sits[b].Down.Count - 1) * _brims[_sits[b].Depth],
                Width = textBox3.Width,
                Height = textBox3.Height,
               // Name= newsit.Name,
               // Text = newsit.Name
            };

            TextBox prob = new TextBox
            {
                Top = name.Top + name.Height + 5,
                Left = name.Left,
                Width = textBox1.Width,
                Height = textBox1.Height
            };

            Button add = new Button
            {
                Top = prob.Top + prob.Height + 1,
                Left = name.Left,
                Width = button1.Width,
                Height = button1.Height
            };
            add.Text = "+";
           // add.BackColor = Color.YellowGreen;
            add.MouseClick += button1_Click;
            _sits[b].Down.Add(newsit);

            name.TextChanged += textBox3_TextChanged;
            prob.TextChanged += textBox1_TextChanged;

            _sits3.Add(name, newsit);
            _sits2.Add(add, newsit);
            _sits4.Add(prob, newsit);

            this.Controls.Add(name);
            this.Controls.Add(prob);
            this.Controls.Add(add);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

        private void рассчитатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Solution s = new Solution();
            s.Show();
            s.textBox1.Text = _root.Falb().Replace("!!", "");
            s.textBox2.Text = _root.Fal();
            double prop = _root.CalculateProp();
            s.textBox4.Text = prop.ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e) => _sits3[sender as TextBox].Name = (sender as TextBox).Text;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _sits4[sender as TextBox].Property = double.Parse((sender as TextBox).Text.Replace(".", ","));
            }
            catch
            {
            }
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sits.Clear();
            _sits2.Clear();
            _sits3.Clear();
            _sits4.Clear();
            foreach (TextBox txtBox in this.Controls.OfType<TextBox>().Where(a => a.Name != "textBox3" && a.Name != "textBox1").Select(a => a)
                .ToArray())
                txtBox.Dispose();

            foreach (Button btn in this.Controls.OfType<Button>().Where(a => a.Name != "button1").Select(a => a)
                .ToArray())
                btn.Dispose();
            _root = new Node(0, "main", 2);
        }
    }
}
