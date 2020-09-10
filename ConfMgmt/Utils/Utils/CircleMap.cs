using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class CircleMap
    {
        public class Rect
        {
            public Rectangle _rect;
            public Point LeftTop;
            public Point RightTop;
            public Point LeftBottom;
            public Point RightBottom;

            public Rect(Rectangle rect)
            {
                _rect = rect;
                LeftTop = new Point(_rect.Left, _rect.Top);
                RightTop = new Point(_rect.Right, _rect.Top);
                LeftBottom = new Point(_rect.Left, _rect.Bottom);
                RightBottom = new Point(_rect.Right, _rect.Bottom);
            }
        }

        public int Row;
        public int Col;
        public int X;
        public int Y;

        public int Width;
        public int Height;
        public Point Center;
        public int Radius;

        public CircleMap(int row, int col, int x, int y)
        {
            Row = row;
            Col = col;
            X = x;
            Y = y;

            Width = Row * X;
            Height = Col * Y;

            Center = new Point(0, 0);
            Radius = Width / 2;
        }

        private double YonCircle(int x)
        {
            return Math.Sqrt(Radius * Radius - (Center.X - x) * (Center.X - x));
        }
        private void GetPosition(int idx, out int row, out int col)
        {
            row = (idx / Col);
            col = (idx % Col);
        }
        private Rect GenRect(int idx)
        {
            GetPosition(idx, out var row, out var col);
            var rect = new Rectangle(new Point(-Radius + row * X, -Radius + col * Y), new Size(X, Y));
            return new Rect(rect);
        }

        private bool Cover(int idx)
        {
            Rect rect = GenRect(idx);

            if (rect.LeftTop.Y < YonCircle(rect.LeftTop.X) &&
                rect.LeftBottom.Y > -YonCircle(rect.LeftBottom.X) &&
                rect.RightTop.Y < YonCircle(rect.RightTop.X) &&
                rect.RightBottom.Y > -YonCircle(rect.RightBottom.X))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int[][] GetMatrix()
        {
            var result = new int[Row][];
            for (int i = 0; i < Row; i++)
            {
                result[i] = new int[Col];
            }

            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    if (Cover(i * Col + j))
                    {
                        result[i][j] = 1;
                    }
                }
            }
            return result;
        }
    }
}
