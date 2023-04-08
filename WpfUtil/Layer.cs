using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfUtil
{
    /// <summary>
    /// 图层基类
    /// </summary>
    public class Layer : Canvas
    {
        #region 构造方法

        public Layer()
        {
            // 启用裁剪
            ClipToBounds = true;
        }

        #endregion

        #region 重写

        /// <summary>可视对象数量</summary>
        protected override int VisualChildrenCount => _visualList.Count;

        /// <summary>
        /// 获取可视对象
        /// </summary>
        protected override Visual GetVisualChild(int index) => _visualList[index];

        #endregion

        #region 公开方法

        /// <summary>
        /// 初始化图层
        /// </summary>
        public virtual void Init() { }

        /// <summary>
        /// 更新图层
        /// </summary>
        public virtual void UpdateLayer() { }

        /// <summary>
        /// 清空图层
        /// </summary>
        public virtual void Clear()
        {
            // 清空可视对象
            foreach (var visual in _visualList) visual.RenderOpen().Close();
        }

        /// <summary>
        /// 添加可视对象
        /// </summary>
        public virtual void AddVisual(VisualBase visual)
        {
            _visualList.Add(visual);

            AddVisualChild(visual);
            AddLogicalChild(visual);
        }

        /// <summary>
        /// 删除可视对象
        /// </summary>
        public virtual void DeleteVisual(VisualBase visual)
        {
            _visualList.Remove(visual);

            RemoveVisualChild(visual);
            RemoveLogicalChild(visual);
        }

        /// <summary>
        /// 获取命中的可视对象
        /// </summary>
        public VisualBase? GetHitedVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult?.VisualHit as VisualBase;
        }

        /// <summary>
        /// 获取与区域交叉的可视对象列表
        /// </summary>
        public List<VisualBase> GetVisualListByRect(Geometry rect)
        {
            _hitedList.Clear();

            GeometryHitTestParameters parameters = new(rect);
            HitTestResultCallback callback = new(HitTestCallback);
            VisualTreeHelper.HitTest(this, null, callback, parameters);

            return _hitedList;
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 创建可视对象
        /// </summary>
        protected T CreateVisual<T>() where T : VisualBase, new()
        {
            T visual = new();
            AddVisual(visual);
            return visual;
        }

        /// <summary>
        /// 绘制顶点
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void DrawVertex(DrawingContext dc, Point point, double size, Brush brush, Pen pen)
        {
            double x = point.X - size / 2;
            double y = point.Y - size / 2;
            dc.DrawRectangle(brush, pen, new Rect(x, y, size, size));
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 命中测试回调
        /// </summary>
        private HitTestResultBehavior HitTestCallback(HitTestResult result)
        {
            GeometryHitTestResult geometryResult = (GeometryHitTestResult)result;
            if (result.VisualHit is VisualBase visual &&
                geometryResult.IntersectionDetail is IntersectionDetail.Intersects or IntersectionDetail.FullyInside)
            {
                _hitedList.Add(visual);
            }
            return HitTestResultBehavior.Continue;
        }

        #endregion

        #region 字段

        /// <summary>可视对象列表</summary>
        protected readonly List<VisualBase> _visualList = new List<VisualBase>();
        /// <summary>命中的可视对象列表</summary>
        private readonly List<VisualBase> _hitedList = new List<VisualBase>();

        #endregion
    }
}