using System.Collections.Generic;

namespace TPR2
{
    class Node
    {
        public double Property;
        public string Name;
        public List<Node> Down;
        public EventLogic Sel;
        public int Depth;
        
        public Node(double property, string name, int dep)
        {
            Depth = dep;
            this.Property = property;
            Name = name;
            this.Down = new List<Node>();
            this.Sel = EventLogic.Or;
        }

        public double CalculateProp()
        {
            if (Down.Count == 0)
                return Property;
            double prop = 1;
            if (Sel == EventLogic.And)
                foreach (var t in Down)
                    prop *= t.CalculateProp();
            else
            {
                foreach (var t in Down)
                    prop *= 1 - t.CalculateProp();

                prop = 1 - prop;
            }
            Property = prop;
            return prop;
        }
        
        public string Falb()
        {
            if (Down.Count == 0)
                return Name;
            string form = "";
            if (Sel == EventLogic.And)
            {
                form = "(" + Down[0].Falb();
                for (var i = 1; i < Down.Count; i++)
                    form += " Ʌ " + Down[i].Falb();
                form += ")";
            }

            else
            {
                form = "(" + Down[0].Falb();
                for (var i = 1; i < Down.Count; i++)
                    form += " V " + Down[i].Falb();
                form += ")";
            }
            return form;
        }

        public string Fal()
        {
            if (Down.Count == 0)
                return Name;
            string form = "";
            if (Sel == EventLogic.And)
            {
                form = "(" + Down[0].Fal();
                for (int i = 1; i < Down.Count; i++)
                    form += Down[i].Fal();
                form += ")";
            }

            else
            {
                form = "(1 - " + Down[0].Fal();
                for (int i = 1; i < Down.Count; i++)
                    form += Down[i].Fal();
                form += ")";
            }
            return form;
        }
    }
}
