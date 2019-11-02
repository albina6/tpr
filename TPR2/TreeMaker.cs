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
            150,
            130,
            5//если больше ?
        };

        // Dictionary<Button,GroupBox> childDict;
        Dictionary<Button, Node> sitsLog;
       // Dictionary<Button, Node> _sits;
        Dictionary<Button, Node> sitsPlus;
        Dictionary<TextBox, Node> sitsName;
        Dictionary<TextBox, Node> sitsProp;
        List<Node> listNode;
        Node _root;
        public TreeMaker()
        {
            InitializeComponent();
           // childDict = new Dictionary<Button, GroupBox>();
            sitsLog = new Dictionary<Button, Node>();
            sitsPlus = new Dictionary<Button, Node>();
            sitsName = new Dictionary<TextBox, Node>();
            sitsProp = new Dictionary<TextBox, Node>();
            listNode = new List<Node>();
            _root = new Node(0, "main", 2);
            listNode.Add(_root);
            sitsName.Add(nameTB, _root);
            sitsPlus.Add(plusButton, _root);
        }
        private void dravListNodes(List<Node> listN, Button logB)
        {
            Node _node = listN.First();
            int countChildren =0 ;
            Button logButton = new Button
            {
                Top = logB.Top + 40,
                Left = logB.Left + 30,//что-нибудь про размер
                Text = _node.Sel.ToString()
            };
            this.Controls.Add(logButton);
            sitsLog.Add(logButton, _node);
            while (countChildren< _node.Down.Count())
            {
                //if (countChildren == 0)
                //{
                //Button logButton = new Button
                //{
                //    Top = logB.Top + 40,
                //    Left = logB.Left + 30,//что-нибудь про размер
                //    Text = _node.Sel.ToString()
                //};
                //sitsLog.Add(logButton, _node);
                //}
                Node child =  _node.Down.ElementAt(countChildren);
                TextBox name = new TextBox
                {
                    Top = logButton.Top + 40,
                    Left = logButton.Left + (sitsLog[logButton].Down.IndexOf(child)-1) * brims[sitsLog[logButton].Depth]+40,
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
                //sitsLog[logB].Down.Add(newsit);

                name.TextChanged += name_Changed;
                prob.TextChanged += prop_Changed;

                sitsName.Add(name, child);
                sitsPlus.Add(add, child);
                sitsProp.Add(prob, child);
               // listNode.Add(newsit);

                this.Controls.Add(name);
                this.Controls.Add(prob);
                this.Controls.Add(add);
                countChildren++;
                listN.Remove(_node);
                if ( listN.First().Down.Count()>0)
                    dravListNodes(listN, add);

            }

            
            //listNode.Add(_node);

            //listN.RemoveAt(0);
            //if (listN.Count() > 0)
            //{

            //}

        }
        private void makeNewGroupBox(object sender, EventArgs e)
        {
            Button logicalButton = sender as Button;
            Node newsit = new Node(0, "", sitsLog[logicalButton].Depth + 1);
            TextBox name = new TextBox
            {
                Top = logicalButton.Top + 40,
                Left = logicalButton.Left + (sitsLog[logicalButton].Down.Count - 1) * brims[sitsLog[logicalButton].Depth],
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

            //Button logicalButton = sender as Button;
            //Node newNode = new Node(0, "", sitsLog[logicalButton].Depth + 1);
            ////GroupBox gb = new GroupBox()
            ////{
            ////    Top = logicalButton.Top + 40,
            ////    Left = logicalButton.Left + (sitsLog[logicalButton].Down.Count - 1) * brims[sitsLog[logicalButton].Depth],
            ////    Width = logicalButton.Width,
            ////    Height = logicalButton.Height
            ////};
            //TextBox name = new TextBox
            //{
            //    Top=logicalButton.Top + 40,
            //    Left=logicalButton.Left+( sitsLog[logicalButton].Down.Count-1)*brims[sitsLog[logicalButton].Depth],
            //    Width = logicalButton.Width,
            //    Height = logicalButton.Height
            //};
            //TextBox prop = new TextBox
            //{
            //    Top = name.Top + name.Height + 5,
            //    Left = name.Left,
            //    Width = logicalButton.Width,
            //    Height = logicalButton.Height
            //};

            //Button plusBut = new Button
            //{
            //    Top = prop.Top + prop.Height + 1,
            //    Left = name.Left,
            //    Width = logicalButton.Width,
            //    Height = logicalButton.Height,
            //    Text="+",
            //};
            //plusBut.MouseClick += plusButton_Click;
            //sitsLog[logicalButton].Down.Add(newNode);

            //name.TextChanged += name_Changed;
            //prop.TextChanged += prop_Changed;

            //sitsLog[logicalButton].Down.Add(newNode);

            //name.TextChanged += name_Changed;
            //prop.TextChanged += prop_Changed;

            //sitsName.Add(name, newNode);
            //sitsPlus.Add(plusBut, newNode);
            //sitsProp.Add(prop, newNode);

            //this.Controls.Add(name);
            //this.Controls.Add(prop);
            //this.Controls.Add(plusBut);

            //this.Controls.Add(gb);
        }

        private void plusButton_Click(object sender, EventArgs e)
        {
            EventType et = new EventType();
            et.ShowDialog();
            EventLogic s = et.S;
            Button mPlusButton = sender as Button;
            Button logButton = new Button
            {
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
            //создание нового groupBox
            
            //childDict.Add(logButton,);

        }

        private void name_Changed(object sender, EventArgs e) => sitsName[sender as TextBox].Name = (sender as TextBox).Text;

        private void prop_Changed(object sender, EventArgs e)
        // => sitsProp[sender as TextBox].Property = double.Parse((sender as TextBox).Text.Replace(".", ","));
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
            listNode
                .Clear();
            foreach (TextBox txtBox in this.Controls.OfType<TextBox>().Where(a => a.Name != "nameTB" && a.Name != "propTB").Select(a => a)
                .ToArray())
                txtBox.Dispose();

            foreach (Button btn in this.Controls.OfType<Button>().Where(a => a.Name != "plusButton").Select(a => a)
                .ToArray())
                btn.Dispose();
            _root = new Node(0, "main", 2);
        }

        //private void save_Click(object sender, EventArgs e)
        //{

        //    if (listNode == null) { return; }

        //    try
        //    {
        //        XmlDocument xmlDocument = new XmlDocument();
        //        XmlSerializer serializer = new XmlSerializer(listNode.GetType());
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            serializer.Serialize(stream, listNode);
        //            stream.Position = 0;
        //            xmlDocument.Load(stream);
        //            xmlDocument.Save("listNode");////////////////////

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log exception here
        //    }
        //}

        private void save_Click(object sender, EventArgs e)
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(Node));
                writer = new StreamWriter("D:/pro/7sem/теория принятия решений/TPR2 - master/TPR2/bin/Debug/listNode.txt", false);
                serializer.Serialize(writer, listNode);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        private void download_Click(object sender, EventArgs e)
        {
            //TextReader reader = null;
            //try
            //{
            //    var serializer = new XmlSerializer(typeof(Node));
            //    reader = new StreamReader("D:/pro/7sem/теория принятия решений/TPR2 - master/TPR2/bin/Debug/listNode");
            //    listNode.Add( (Node)serializer.Deserialize(reader));
            //}
            //finally
            //{
            //    if (reader != null)
            //        reader.Close();
            //}







           // clear_Click(clear, e);
            //nameTB.Text = "";
            //propTB.Text = "";
            /////////////////очистить экран
            List<Node> liNode = new List<Node>(listNode);

            clear_Click(clear, e);
            Node _nodeMain = liNode.First();

            // liNode.RemoveAt(0);////
            nameTB.Text = _nodeMain.Name;
            propTB.Text = _nodeMain.Property.ToString();
            if (_nodeMain.Down.Count > 0)
            {
                //Button logButton = new Button
                //{
                //    Top = plusButton.Top + 40,
                //    Left = plusButton.Left + 30,//что-нибудь про размер
                //    Text = _nodeMain.Sel.ToString()
                //};
                try
                {
                    sitsPlus.Add(plusButton, _nodeMain);
                }
                catch { }
                //sitsLog.Add(logButton, sitsPlus[plusButton]);
                sitsName.Add(nameTB, _nodeMain);
                sitsProp.Add(propTB, _nodeMain);

               // logButton.Click += makeNewGroupBox;
               // this.Controls.Add(logButton);
                dravListNodes(liNode, plusButton);
            }
            else { }
            ///// вызов метода отрисовки 

            liNode.Clear();
        }
        //private void download_Click(object sender, EventArgs e)
        //{

        //    if (string.IsNullOrEmpty("listNode"))
        //    {
        //        // listNode.Clear();
        //    }
        //    else
        //    {
        //        listNode.Clear();
        //        //Node objectOut = default(Node);

        //        try
        //        {
        //            XmlDocument xmlDocument = new XmlDocument();
        //            xmlDocument.Load("listNode");
        //            string xmlString = xmlDocument.OuterXml;

        //            using (StringReader read = new StringReader(xmlString))
        //            {
        //                Type outType = typeof(Node);

        //                XmlSerializer serializer = new XmlSerializer(outType);
        //                using (XmlReader reader = new XmlTextReader(read))
        //                {
        //                    listNode.Add((Node)serializer.Deserialize(reader));
        //                    //listNode=(Node)serializer.Deserialize(reader);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //Log exception here
        //        }
        //        clear_Click(clear, e);
        //        nameTB.Text = "";
        //        propTB.Text = "";
        //        /////////////////очистить экран
        //        List<Node> liNode = new List<Node>(listNode);
        //        Node _nodeMain = liNode.First();

        //        // liNode.RemoveAt(0);////
        //        nameTB.Text = _nodeMain.Name;
        //        propTB.Text = _nodeMain.Property.ToString();
        //        if (_nodeMain.Down.Count != 0)
        //        {
        //            Button logButton = new Button
        //            {
        //                Top = plusButton.Top + 40,
        //                Left = plusButton.Left + 30,//что-нибудь про размер
        //                Text = _nodeMain.Sel.ToString()
        //            };
        //            try
        //            {
        //                sitsPlus.Add(plusButton, _nodeMain);
        //            }
        //            catch { }
        //            //sitsLog.Add(logButton, sitsPlus[plusButton]);
        //            sitsName.Add(nameTB, _nodeMain);
        //            sitsProp.Add(propTB, _nodeMain);

        //            logButton.Click += makeNewGroupBox;
        //            this.Controls.Add(logButton);
        //            dravListNodes(liNode, logButton);
        //        }
        //        else { }
        //        ///// вызов метода отрисовки 

        //        liNode.Clear();
        //    }
        //    // return objectOut;
        //}
    }
}