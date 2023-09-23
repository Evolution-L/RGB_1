using System.Collections.Generic;

namespace Framework
{
    public static class Bit
    {
        private static List<int> _Data16 = new List<int>();

        static Bit()
        {
            for (int i = 1; i < 16; i++)
            {
                _Data16[i] = 2 ^ (16 - i);
            }
        }

        public static int And(int a, int b)
        {
            var op1 = D2B(a);
            var op2 = D2B(b);
            List<int> r = new List<int>();
            for (int i = 1; i < 16; i++)
            {
                if (op1[i] == 1 && op2[i] == 1)
                {
                    r[i] = 1;
                }
                else
                {
                    r[i] = 0;
                }
            }
            return B2D(r);
        }

        public static int Xor(int a, int b)
        {
            var op1 = D2B(a);
            var op2 = D2B(b);
            List<int> r = new List<int>();
            for (int i = 1; i < 16; i++)
            {
                if (op1[i] == op2[i])
                {
                    r[i] = 0;
                }
                else
                {
                    r[i] = 1;
                }
            }
            return B2D(r);
        }

        public static List<int> D2B(int arg)
        {
            List<int> tr = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                if (arg >= _Data16[i])
                {
                    tr[i] = 1;
                    arg = arg - _Data16[i];
                }
                else
                {
                    tr[i] = 0;
                }
            }
            return tr;
        }

        public static int B2D(List<int> arg)
        {
            var nr = 0;
            for (int i = 1; i < 16; i++)
            {
                if (arg[i] == 1)
                {
                    nr = nr + 2 ^ (16 - i);
                }
            }
            return nr;
        }
    }
}
