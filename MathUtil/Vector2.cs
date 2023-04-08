namespace MathUtil
{
    /// <summary>
    /// 表示一个二维向量。使用两个点描述方向。
    /// </summary>
    public class Vector2
    {
        #region 构造方法

        public Vector2() { }

        public Vector2(Point2D origin, Point2D target)
        {
            Origin = origin;
            Target = target;
            _angle = origin.AngleTo(target);
            Distance = GetDistance();
        }

        #endregion

        #region 属性

        /// <summary>原点</summary>
        public Point2D Origin { get; set; } = new Point2D();

        /// <summary>目标点</summary>
        public Point2D Target { get; set; } = new Point2D();

        /// <summary>角度</summary>
        public double Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                double radian;
                radian = Math.PI * (Angle / 180);
                double x = Origin.X + Distance * Math.Cos(radian);
                double y = Origin.Y + Distance * Math.Sin(radian);
                Target = new Point2D(x, y);
            }
        }

        /// <summary>距离</summary>
        public double Distance { get; private set; }

        #endregion

        #region 公开方法

        /// <summary>
        /// 转字符串
        /// </summary>
        public override string ToString() => $"{Origin} -> {Target}";

        /// <summary>
        /// 获取两点间距离
        /// </summary>
        public double GetDistance()
        {
            double delta_x = Target.X - Origin.X;
            double delta_y = Target.Y - Origin.Y;
            return Math.Sqrt(Math.Pow(delta_x, 2) + Math.Pow(delta_y, 2));
        }

        /// <summary>
        /// 计算与另一个向量的交点
        /// </summary>
        public Point2D IntersectionWith(Vector2 other)
        {
            // 创建两个直线方程
            LineEquation e1 = new(Origin, Target);
            LineEquation e2 = new(other.Origin, other.Target);
            // 获取交点
            return e1.GetIntersection(e2);
        }

        /// <summary>
        /// 绕原点旋转
        /// </summary>
        public void Rotate(double angle) => Angle += angle;

        #endregion

        private double _angle = 0;
    }
}