using System.Windows;
using System.Windows.Media;

namespace WpfUtil
{
    /// <summary>
    /// 可视对象基类
    /// </summary>
    public class VisualBase : DrawingVisual
    {
        #region 属性

        /// <summary>坐标</summary>
        public Point Point { get; set; } = new Point();

        #endregion

        /// <summary>
        /// 更新可视对象
        /// </summary>
        public void Update()
        {
            using DrawingContext context = RenderOpen();
            OnDraw(context);
        }

        public virtual void OnDraw(DrawingContext context) { }
    }
}