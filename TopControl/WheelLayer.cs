using MathUtil;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using WpfUtil;

namespace TopControl
{
    public class WheelLayer : SingleLayer
    {
        #region 属性

        /// <summary>槽高度：180px - 上下边框（4px）</summary>
        public int GrooveHeight { get; set; } = 176;

        /// <summary>角度：圆弧切线与槽边的夹角</summary>
        public int Angle { get; set; } = 90;

        /// <summary>刻度线数量</summary>
        public int LineCount { get; set; } = 90;

        /// <summary>角度偏移</summary>
        public double AngleOffset { get; set; } = 0;

        #endregion

        #region 公开方法

        public override void Init()
        {
            // 起点、终点、中点
            Point2D startPoint = new Point2D(0, 0);
            Point2D endPoint = new Point2D(GrooveHeight, 0);
            Point2D centerPoint = startPoint.CenterWith(endPoint);
            // 向量：中点 -> 起点
            Vector2 centerToStart = new Vector2(centerPoint, startPoint);
            centerToStart.Rotate(-90);
            // 向量：终点 -> 中点
            Vector2 endToCenter = new Vector2(endPoint, centerPoint);
            endToCenter.Rotate(-90 + Angle);
            // 圆心
            _circleCenter = centerToStart.IntersectionWith(endToCenter);
            // 向量：圆心 -> 起点
            Vector2 vector = new Vector2(_circleCenter, startPoint);
            _radius = vector.Distance;
            _anglePerLine = 360.0 / LineCount;
        }

        protected override void OnUpdate()
        {
            // 最高点
            Point2D top = new Point2D(_circleCenter.X, _circleCenter.Y - _radius);
            // 向量：圆心 -> 最高点
            Vector2 vector = new Vector2(_circleCenter, top);
            double max = Math.Abs(vector.Target.Y);
            // 偏移角度
            vector.Rotate(AngleOffset);
            // 开始旋转计算刻度位置
            List<Point2D> pointList = new List<Point2D>();
            for (int counter = 0; counter < LineCount; counter++)
            {
                if (vector.Target.Y < 0) pointList.Add(vector.Target);
                vector.Rotate(-_anglePerLine);
            }

            // 绘制刻度线
            foreach (var item in pointList)
                DrawHorizontalLine(item.X, Math.Abs(item.Y) / max);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 绘制水平线
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DrawHorizontalLine(double y, double opacity)
        {
            Pen pen = new Pen(new SolidColorBrush(Color.FromArgb((byte)(opacity * 255), 32, 32, 32)), 1);
            Pen pen2 = new Pen(new SolidColorBrush(Color.FromArgb((byte)(opacity * 255), 64, 64, 64)), 1);
            _dc.DrawLine(pen, new Point(2, y - 0.5), new Point(Width - 2, y - 0.5));
            _dc.DrawLine(pen2, new Point(2, y + 0.5), new Point(Width - 2, y + 0.5));
        }

        #endregion

        #region 字段

        /// <summary>圆心</summary>
        private Point2D _circleCenter = new Point2D(0, 0);
        /// <summary>半径</summary>
        private double _radius = 0;
        /// <summary>刻度线之间的夹角</summary>
        private double _anglePerLine = 0;

        #endregion
    }
}