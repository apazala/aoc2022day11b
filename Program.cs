internal class Program
{
    private delegate int op(int v);
    class Monke
    {
        private static long chineseMod = 1;
        private Queue<int> items = new Queue<int>();
        private int k;

        private int mod;

        private int truMonke;

        private int falseMonke;

        private op operation;

        private int itemsChecked;
        public int ItemsChecked { get => itemsChecked; }

        private static char[] itemSeps = { ' ', ',' };
        public Monke(string[] lines, ref int i)
        {
            itemsChecked = 0;

            i++; //Items line: <<  Starting items:>> has 17 chars
            string[] tokens = lines[i].Substring(17).Split(itemSeps, StringSplitOptions.RemoveEmptyEntries);
            foreach (string itemStr in tokens)
            {
                int v = int.Parse(itemStr);
                items.Enqueue(v);
            }
            i++; //Operation line: <<  Operation: new = old>> has 22 chars
            tokens = lines[i].Substring(22).Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (tokens[0][0] == '*')
            {
                if(tokens[1][0] == 'o')
                {
                    operation = SquareOp;
                } 
                else{
                    operation = MultOp;
                    k = int.Parse(tokens[1]);
                }

            }else{
                operation = AddOp;
                k = int.Parse(tokens[1]);
            }



            i++; //Divisible line: <<  Test: divisible by>> has 20 chars
            mod = int.Parse(lines[i].Substring(20));
            chineseMod*=mod;

            i++; // if true line: <<    If true: throw to monkey>> has 28 chars
            truMonke = int.Parse(lines[i].Substring(28));


            i++; // if true line: <<    If false: throw to monkey>> has 29 chars
            falseMonke = int.Parse(lines[i].Substring(29));
            i++;
        }

        public void Add(int item)
        {
            items.Enqueue(item);
        }

        public void ProcessItems(List<Monke> monkes)
        {
            int item;
            int monkeId;
            while (items.Count > 0)
            {
                item = items.Dequeue();
                item = operation(item);
                monkeId = (item%mod == 0 ? truMonke : falseMonke);
                monkes[monkeId].Add(item);

                itemsChecked++;
            }
        }

        private int AddOp(int v)
        {
            v += k;
            return (int)(v>=chineseMod?v-chineseMod:v);
        }

        private int MultOp(int v)
        {
            long res = v;
            res*=k;
            return (int)(res>=chineseMod?res%chineseMod:res);
        }

        private int SquareOp(int v)
        {
            long res = v;
            res*=v;
            return (int)(res>=chineseMod?res%chineseMod:res);
        }

    }

    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines(@"input.txt");
        List<Monke> monkesList = new List<Monke>();
        for (int i = 0; i < lines.Length; i++)
        {
            Monke monk = new Monke(lines, ref i);
            monkesList.Add(monk);
        }

        for (int i = 0; i < 10000; i++)
        {
            foreach (Monke monk in monkesList)
            {
                monk.ProcessItems(monkesList);
            }
        }

        int max = -1, secondMax = -1;
        foreach (Monke monk in monkesList)
        {
            if(monk.ItemsChecked > max)
            {
                secondMax = max;
                max = monk.ItemsChecked;
            }else if(monk.ItemsChecked > secondMax)
            {
                secondMax = monk.ItemsChecked;
            }
        }

        Console.WriteLine(max*(long)secondMax);
    }
}