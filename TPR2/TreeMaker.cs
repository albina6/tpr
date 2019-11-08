using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace TPR2
{
    public enum EventLogic
    {
        Or,
        And
    }

    public partial class TreeMaker : Form
    {
        int[] brims = new int[]
        {
            0,0,
            450,
            270,
            
            100,
            70,
            10//если больше ?
        };

        Dictionary<Button, Node> sitsLog;
        Dictionary<Button, Node> sitsPlus;
        Dictionary<TextBox, Node> sitsName;
        Dictionary<TextBox, Node> sitsProp;
        List<Node> listNode;
        Node _root;
        public TreeMaker()
        {
            InitializeComponent();
            sitsLog = new Dictionary<Button, Node>();
            sitsPlus = new Dictionary<Button, Node>();
            sitsName = new Dictionary<TextBox, Node>();
            sitsProp = new Dictionary<TextBox, Node>();
            listNode = new List<Node>();
            _root = new Node(0, "main", 2);
            listNode.Add(_root);
            sitsName.Add(nameTB, _root);
            sitsProp.Add(propTB, _root);
            sitsPlus.Add(plusButton, _root);
        }
        private void dravListNodes(List<Node> listN, Button logB, int chil=0)
        {
            Node _node = listN.First();
            int countChildren = chil;
            Button logButton = new Button
            {
                Top = logB.Top + 40,
                Left = logB.Left + 30,//что-нибудь про размер
                Text = _node.Sel.ToString(),
                Height=23,
                Width=57
            };
            this.Controls.Add(logButton);
            sitsLog.Add(logButton, _node);
            while (countChildren< _node.Down.Count())
            {
                Node child =  _node.Down.ElementAt(countChildren);
                TextBox name = new TextBox
                {
                    Top = logButton.Top + 40,
                    Left = logButton.Left + (sitsLog[logButton].Down.IndexOf(child)-2) * brims[sitsLog[logButton].Depth]+50,
                    Width = nameTB.Width,
                    Height = nameTB.Height,
                    Text = child.Name
                };

                TextBox prob = new TextBox
                {
                    Top = name.Top + name.Height + 5,
                    Left = name.Left,
                    Width = name.Width,
                    Height = name.Height,
                    Text=child.Property.ToString()
                };

                Button add = new Button
                {
                    Top = prob.Top + prob.Height + 1,
                    Left = name.Left,
                    Width = plusButton.Width,
                    Height = plusButton.Height,
                    Text = "+"
                };

                add.MouseClick += plusButton_Click;
                name.TextChanged += name_Changed;
                prob.TextChanged += prop_Changed;

                sitsName.Add(name, child);
                sitsPlus.Add(add, child);
                sitsProp.Add(prob, child);
                this.Controls.Add(name);
                this.Controls.Add(prob);
                this.Controls.Add(add);
                countChildren++;
                if (listN.Count>2 && (_node.Down.IndexOf(listN.ElementAt(1)) != -1))
                {
                    listN.Remove(_node);
                    dravListNodes(listN, add,countChildren);
                    
                }
                
               else if (listN.First().Down.Count() > 0)
                {
                    listN.Remove(_node);
                    dravListNodes(listN, add);
                }
                   

            }
        }
        private void makeNewGroupBox(object sender, EventArgs e)
        {
            Button logicalButton = sender as Button;
            Node newsit = new Node(0, "", sitsLog[logicalButton].Depth + 1);
            TextBox name = new TextBox
            {
                Top = logicalButton.Top + 40,
                Left = logicalButton.Left + (sitsLog[logicalButton].Down.Count - 1) * brims[sitsLog[logicalButton].Depth]+50,
                Width = nameTB.Width,
                Height = nameTB.Height
            };

            TextBox prob = new TextBox
            {
                Top = name.Top + name.Height + 5,
                Left = name.Left,
                Width = name.Width,
                Height = name.Height
            };

            Button add = new Button
            {
                Top = prob.Top + prob.Height + 1,
                Left = name.Left,
                Width = plusButton.Width,
                Height = plusButton.Height,
                Text="+"
            };
            
            add.MouseClick += plusButton_Click;
            sitsLog[logicalButton].Down.Add(newsit);

            name.TextChanged += name_Changed;
            prob.TextChanged += prop_Changed;

            sitsName.Add(name, newsit);
            sitsPlus.Add(add, newsit);
            sitsProp.Add(prob, newsit);
            listNode.Add(newsit);

            this.Controls.Add(name);
            this.Controls.Add(prob);
            this.Controls.Add(add);
        }

        private void plusButton_Click(object sender, EventArgs e)
        {
            EventType et = new EventType();
            et.ShowDialog();
            EventLogic s = et.S;
            Button mPlusButton = sender as Button;
            Button logButton = new Button
            {
                Height = 23,
                Width = 57,
                Top = mPlusButton.Top + 40,
                Left = mPlusButton.Left + 30//что-нибудь про размер
            };

            try
            {
                sitsPlus.Add(mPlusButton, _root);
            }
            catch
            {

            }
            
            sitsPlus[mPlusButton].Sel = s;
            sitsLog.Add(logButton, sitsPlus[mPlusButton]);
            logButton.Text = s.ToString();
            logButton.Click += makeNewGroupBox;
            this.Controls.Add(logButton);
        }

        private void name_Changed(object sender, EventArgs e) => sitsName[sender as TextBox].Name = (sender as TextBox).Text;

        private void prop_Changed(object sender, EventArgs e)
        {
            try
            {
                sitsProp[sender as TextBox].Property = double.Parse((sender as TextBox).Text.Replace(".", ","));
            }
            catch
            {
            }
        }
        private void calculate_Click(object sender, EventArgs e)
        {
            _root = listNode.First();
            Solution s = new Solution();
            s.Show();
           
            s.textBox1.Text = _root.Falb().Replace("!!", "");
            s.textBox2.Text = _root.Fal();
            double prop = _root.CalculateProp();
            s.textBox4.Text = prop.ToString();
        }

        private void clear_Click(object sender, EventArgs e)
        {
            sitsLog.Clear();
            sitsPlus.Clear();
            sitsName.Clear();
            sitsProp.Clear();
            listNode.Clear();
            foreach (TextBox txtBox in this.Controls.OfType<TextBox>().Where(a => a.Name != "nameTB" && a.Name != "propTB").Select(a => a)
                .ToArray())
                txtBox.Dispose();

            foreach (Button btn in this.Controls.OfType<Button>().Where(a => a.Name != "plusButton").Select(a => a)
                .ToArray())
                btn.Dispose();
            _root = new Node(0, "main", 2); sitsPlus.Clear();
            sitsName.Add(nameTB,_root);
            sitsProp.Add(nameTB,_root);
            listNode.Add(_root);
            nameTB.Text = "";
            propTB.Text = "";
        }

        private void save_Click(object sender, EventArgs e)
        {

            if (listNode == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(listNode.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, listNode);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save("listNode");////////////////////

                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }

        private void download_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty("listNode"))
            {

            }

            else
            {
                TextReader reader = null;
                clear_Click(clear, e);
                try
                {
                    var serializer = new XmlSerializer(typeof(List<Node>));
                    reader = new StreamReader("listNode");
                    listNode= (List<Node>)serializer.Deserialize(reader);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
                List<Node> liNode = new List<Node>(listNode);
                _root = liNode.First();
                //sitsName.Add(nameTB,_root);
                //sitsProp.Add(propTB, _root);
                //sitsPlus.Add(plusButton, _root);
                nameTB.Text = _root.Name;
                propTB.Text = _root.Property.ToString();
                if (_root.Down.Count > 0)
                {
                    dravListNodes(liNode, plusButton);
                }
                else { }
                liNode.Clear();
            }
        }
      
    }
}