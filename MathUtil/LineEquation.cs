namespace MathUtil
{
    /// <summary>
    /// 表示一个直线方程
    /// </summary>
    public class LineEquation
    {
        public LineEquation(Point2D point1, Point2D point2)
        {
            A = point2.Y - point1.Y;
            B = point1.X - point2.X;
            C = A * point1.X + B * point1.Y;
            Start = point1;
            End = point2;
        }

        public double A { get; set; } = 1;
        public double B { get; set; } = 1;
        public double C { get; set; } = 1;

        public Point2D Start { get; set; } = new Point2D();
        public Point2D End { get; set; } = new Point2D();

        /// <summary>
        /// 获取与另一条直线的交点
        /// </summary>
        public Point2D GetIntersection(LineEquation other)
        {
            double delta = A * other.B - other.A * B;
            return new Point2D
            {
                X = (other.B * C - B * other.C) / delta,
                Y = (A * other.C - other.A * C) / delta
            };
        }
    }
}