namespace MathUtil
{
    /// <summary>
    /// 表示一个二维坐标
    /// </summary>
    public struct Point2D
    {
        public Point2D() { }

        public Point2D(double x, double y) { X = x; Y = y; }

        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;

        public bool SameOf(Point2D point)
        {
            if (X != point.X || Y != point.Y) return false;
            return true;
        }

        /// <summary>
        /// 获取中点
        /// </summary>
        public Point2D CenterWith(Point2D target)
        {
            double x = X + (target.X - X) / 2;
            double y = Y + (target.Y - Y) / 2;
            return new Point2D(x, y);
        }

        /// <summary>
        /// 获取与另一个坐标连线在平面上的角度
        /// </summary>
        public double AngleTo(Point2D target)
        {
            // 计算增量
            double delta_x = target.X - X;
            double delta_y = target.Y - Y;

            // 两点重合
            if (delta_x == 0 && delta_y == 0) return 0;
            // 两点垂直
            if (delta_x == 0) return delta_y > 0 ? 90 : 270;
            // 两点水平
            if (delta_y == 0 && delta_x < 0) return 180;

            double angle;
            angle = System.Math.Atan(System.Math.Abs(delta_y) / System.Math.Abs(delta_x)) / System.Math.PI * 180;
            // 转换为周角
            if (delta_x < 0 && delta_y > 0) angle = 90 + (90 - angle);
            else if (delta_x < 0 && delta_y < 0) angle = 180 + angle;
            else if (delta_x > 0 && delta_y < 0) angle = 270 + (90 - angle);
            return System.Math.Round(angle, 1);
        }

        public override string ToString() => $"{X:0.00}, {Y:0.00}";

        public string ToIntPointString() => $"{(int)X}, {(int)Y}";
    }
}